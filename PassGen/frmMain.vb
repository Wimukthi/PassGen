Imports System.ComponentModel
Imports System.IO
' Removed System.Security.Cryptography import as it's now encapsulated in services
Imports System.Tuple ' Required for passing multiple results from BackgroundWorker
Imports System.Collections.Generic ' Needed for List(Of T)

Public Class FrmMain ' IDE1006: Renamed class

    ' --- Service Instances ---
    ' These instances provide the core functionalities like generation, hashing, etc.
    Private _errorLogger As ErrorLogger
    Private _passwordGenerator As PasswordGenerator
    Private _entropyCalculator As EntropyCalculator
    Private _hashingService As HashingService
    Private _passwordSaver As PasswordSaver

    ' --- Context Menu for Password List ---
    Private WithEvents cmsPasswordList As ContextMenuStrip
    Private WithEvents tsmiCopyPassword As ToolStripMenuItem
    Private WithEvents tsmiSaveSelectedPasswords As ToolStripMenuItem

    ' --- State Variables ---
    Public Plength As Integer = 35 ' Stores the desired length of the password(s) for the current generation batch. Updated from UI.
    Public TotalKeysInBatch As Integer = 1 ' Stores the *total* number of passwords requested for the current batch. Updated from UI.
    Public WindowTitle As String ' Stores the base window title (e.g., "PassGen vX.Y") for easy reuse.
    Private _isGeneratingBatch As Boolean = False ' Flag to prevent starting a new generation while one is already running.

    ' --- Helper Structure ---
    ' Structure to hold all relevant data for a single generated password.
    ' Used to collect results in the background worker before updating the UI in bulk.
    Private Structure PasswordData
        Public Password As String
        Public StrengthDisplayText As String ' e.g., "Password Strength: 100 bits (Strong)"
        Public EntropyBits As Integer ' Raw entropy value for potential sorting or other logic.
        Public Length As Integer
        Public MD5Hash As String
        Public SHA256Hash As String
        Public SHA512Hash As String
    End Structure

    Private Sub FrmMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load ' IDE1006: Renamed method
        Try
            ' --- Initialize Services ---
            ' Instantiate all required service classes, passing the logger where needed.
            _errorLogger = New ErrorLogger() ' Initialize logger first
            _passwordGenerator = New PasswordGenerator(_errorLogger)
            _entropyCalculator = New EntropyCalculator(_errorLogger)
            _hashingService = New HashingService(_errorLogger)
            _passwordSaver = New PasswordSaver(_errorLogger)

            ' --- Initialize UI ---
            ' Set the initial window title including the application version.
            WindowTitle = "Grey Element Software - PassGen v" & My.Application.Info.Version.ToString ' Corrected &
            Me.Text = WindowTitle & " - Idle" ' Corrected & ' Set the Default window title
            ' Set default values for UI controls based on initial state variables.
            txtMaxLength.Value = Plength ' Set default length in UI
            txtPassAmount.Value = TotalKeysInBatch ' Set default amount in UI

            ' --- Initialize Context Menu ---
            ' Check if components container exists, otherwise create a new one.
            If Me.components Is Nothing Then
                 Me.components = New System.ComponentModel.Container()
            End If
            Me.cmsPasswordList = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.tsmiCopyPassword = New System.Windows.Forms.ToolStripMenuItem()
            Me.tsmiSaveSelectedPasswords = New System.Windows.Forms.ToolStripMenuItem()
            Me.cmsPasswordList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiCopyPassword, Me.tsmiSaveSelectedPasswords})
            Me.cmsPasswordList.Name = "cmsPasswordList"
            Me.cmsPasswordList.Size = New System.Drawing.Size(181, 48) ' Auto-size might adjust this later
            '
            ' tsmiCopyPassword
            '
            Me.tsmiCopyPassword.Name = "tsmiCopyPassword"
            Me.tsmiCopyPassword.Size = New System.Drawing.Size(180, 22) ' Example size
            Me.tsmiCopyPassword.Text = "Copy"
            '
            ' tsmiSaveSelectedPasswords
            '
            Me.tsmiSaveSelectedPasswords.Name = "tsmiSaveSelectedPasswords"
            Me.tsmiSaveSelectedPasswords.Size = New System.Drawing.Size(180, 22) ' Example size
            Me.tsmiSaveSelectedPasswords.Text = "Save Selected..."
            '
            ' Assign context menu to ListView
            '
            Me.lstvKeys.ContextMenuStrip = Me.cmsPasswordList

        Catch ex As Exception
            ' Handle potential errors during form loading.
            ' Use the initialized logger if available, otherwise show a basic message box.
            If _errorLogger IsNot Nothing Then ' IDE0029: Null check can be simplified (already is simple)
                _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error during Form Load")
            Else
                MessageBox.Show("A critical error occurred during application startup: " & ex.Message, "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ' Corrected &
            End If
            ' Consider closing the application here if startup fails critically
            ' Application.Exit()
        End Try
    End Sub

    Private Sub BtnGenerate_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerate.Click ' IDE1006: Renamed method
        ' Prevent overlapping generation attempts by checking the flag and worker status.
        ' Note: We only check bwgen now, as threadEntropy is being removed.
        If _isGeneratingBatch OrElse bwgen.IsBusy Then
            MessageBox.Show("Password generation is already in progress. Please wait.", "Busy", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            ' --- Validate Inputs ---
            ' Ensure user has provided valid length and amount.
            If txtMaxLength.Value <= 0 Then
                MessageBox.Show("Password length must be greater than 0.", "Invalid Length", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If txtPassAmount.Value <= 0 Then
                MessageBox.Show("Number of passwords must be greater than 0.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' --- Start Batch Generation ---
            _isGeneratingBatch = True ' Set the flag to indicate generation is active.
            ' Update state variables from UI controls for this specific batch.
            Plength = CInt(txtMaxLength.Value) ' Get current length from UI for this batch
            TotalKeysInBatch = CInt(txtPassAmount.Value) ' Get total count for this batch

            ' --- Configure Progress Bars ---
            ' Set up the main progress bar for the batch count.
            progGen.Minimum = 0
            progGen.Maximum = TotalKeysInBatch
            progGen.Value = 0
            ' Set up the entropy progress bar (max is kept for display consistency).
            progEntropy.Minimum = 0
            progEntropy.Maximum = 128 ' Default max - Keep for display consistency when selecting items
            progEntropy.Value = 0

            ' --- Clear UI elements *once* at the start of the batch ---
            ' Prepare the UI for new results.
            lstvKeys.Items.Clear()
            txtoutput.Clear()
            txtMD5.Text = ""
            txtSHA256.Text = ""
            txtSHA512.Text = ""
            lblEntropy.Text = "Password Entropy :"
            lblEntropy.ForeColor = SystemColors.ControlText ' Reset color

            ' --- Start the asynchronous batch password generation ---
            Me.Text = WindowTitle & " - Starting Batch (0/" & TotalKeysInBatch & ")" ' Corrected & (multiple)
            ' Pass necessary parameters to the worker if they aren't class members or easily accessible
            ' In this case, Plength and TotalKeysInBatch are class members, accessible directly in DoWork
            bwgen.RunWorkerAsync() ' Start the background worker to perform generation off the UI thread.

        Catch ex As Exception
            _isGeneratingBatch = False ' Reset flag on error during setup.
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error in btnGenerate_Click")
            MessageBox.Show("An error occurred starting the generation: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ' Corrected &
            Me.Text = WindowTitle & " - Error" ' Corrected &
        End Try
    End Sub

    ' --- Background Worker: Password Generation (Now handles the entire batch) ---

    ' This method runs on a separate thread to avoid freezing the UI during generation.
    Private Sub Bwgen_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwgen.DoWork ' IDE1006: Renamed method
        ' This worker now generates the entire batch of passwords.
        Dim generatedDataList As New List(Of PasswordData) ' List to hold results for all passwords in the batch.
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker) ' Cast sender to access ReportProgress and CancellationPending.

        Try
            ' 1. Build Character Set ONCE using the service
            ' Determine the pool of characters based on user selections in the UI.
            Dim characterSet As String = _passwordGenerator.BuildCharacterSet(
                chkUpperCase.Checked,
                chkLowerCase.Checked,
                chkNumbers.Checked,
                chkSpecialCharacters.Checked,
                chkSpace.Checked,
                chkCustomChars.Checked,
                txtCustomChars.Text
            )

            ' If no character types are selected, the set will be empty. Stop generation.
            If String.IsNullOrEmpty(characterSet) Then
                 ' Pass back an empty list. RunWorkerCompleted will handle the user message.
                 e.Result = generatedDataList
                 Return
            End If

            Dim characterSetSize As Integer = characterSet.Length ' Cache the size for entropy calculation.

            ' 2. Loop to generate all passwords requested in the batch.
            For i As Integer = 1 To TotalKeysInBatch
                ' Check if the user has requested cancellation via the UI (not implemented in this snippet, but supported by BackgroundWorker).
                If worker.CancellationPending Then
                    e.Cancel = True ' Set the Cancel flag for RunWorkerCompleted.
                    Exit For ' Stop the loop.
                End If

                ' Generate a single Password using the service.
                Dim currentPassword As String = _passwordGenerator.GeneratePassword(characterSet, Plength)
                If String.IsNullOrEmpty(currentPassword) Then
                    ' Should not happen if characterSet is valid, but handle defensively.
                    Throw New Exception("Password generation failed unexpectedly within the loop.")
                End If

                ' Calculate Entropy using the service.
                Dim pwentropy As Double = _entropyCalculator.CalculateEntropy(Plength, characterSetSize)
                Dim entropyBits As Integer = CInt(Math.Floor(pwentropy)) ' Truncate to integer bits.

                ' Determine Strength Category based on entropy bits.
                Dim strengthCategory As String
                If entropyBits < 40 Then
                    strengthCategory = "Very Weak"
                ElseIf entropyBits < 60 Then
                    strengthCategory = "Weak"
                ElseIf entropyBits < 80 Then
                    strengthCategory = "Moderate"
                ElseIf entropyBits < 128 Then
                    strengthCategory = "Strong"
                Else
                    strengthCategory = "Very Strong"
                End If
                Dim strengthDisplayText As String = $"Password Strength: {entropyBits} bits ({strengthCategory})"

                ' Calculate Hashes using the service.
                Dim md5Hash As String = _hashingService.GenerateMD5Hash(currentPassword)
                Dim sha256Hash As String = _hashingService.GenerateSHA256Hash(currentPassword)
                Dim sha512Hash As String = _hashingService.GenerateSHA512Hash(currentPassword)

                ' Store all calculated data for this password in the helper structure.
                Dim data As New PasswordData With {
                    .Password = currentPassword,
                    .StrengthDisplayText = strengthDisplayText,
                    .EntropyBits = entropyBits,
                    .Length = Plength,
                    .MD5Hash = md5Hash,
                    .SHA256Hash = sha256Hash,
                    .SHA512Hash = sha512Hash
                }
                generatedDataList.Add(data) ' Add the data to the list for the batch.

                ' Report Progress back to the UI thread. Pass the number of passwords generated so far (i).
                worker.ReportProgress(i)

            Next ' End of loop for generating one password.

            ' 3. Pass the complete list of generated data back as the result of the background operation.
            e.Result = generatedDataList

        Catch ex As Exception
            ' Log any errors that occur during the background generation process.
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error in bwgen_DoWork (Batch Generation)")
            ' Pass the exception object itself back to RunWorkerCompleted for handling on the UI thread.
            e.Result = ex
        End Try
    End Sub

    ' This method runs on the UI thread when the background worker completes, cancels, or errors.
    Private Sub Bwgen_RunWorkerCompleted(sender As System.Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwgen.RunWorkerCompleted ' IDE1006: Renamed method
        Try
            ' Reset batch flag regardless of outcome (success, error, cancel).
             _isGeneratingBatch = False

            ' Check for errors that occurred during DoWork.
            ' Check the e.Error property first (unhandled exceptions in DoWork).
            If e.Error IsNot Nothing Then
                 _errorLogger.WriteToErrorLog(e.Error.Message, e.Error.StackTrace, "Unhandled Error in bwgen_DoWork")
                 MessageBox.Show("An unhandled error occurred during password generation: " & e.Error.Message, "Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ' Corrected &
                 Me.Text = WindowTitle & " - Error" ' Corrected &
                 Return
            ' Check if an exception was explicitly passed back via e.Result.
            ElseIf e.Result IsNot Nothing AndAlso TypeOf e.Result Is Exception Then
                Dim ex As Exception = CType(e.Result, Exception)
                ' Error was already logged in DoWork, just inform the user via MessageBox.
                MessageBox.Show("An error occurred during password generation: " & ex.Message, "Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ' Corrected &
                Me.Text = WindowTitle & " - Error" ' Corrected &
                Return
            End If

            ' Check if the operation was cancelled by the user.
            If e.Cancelled Then
                Me.Text = WindowTitle & " - Cancelled" ' Corrected &
                MessageBox.Show("Password generation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Clear potentially partially filled UI elements if needed (e.g., progress bar).
                progGen.Value = 0
                Return
            End If

            ' --- Process successful batch generation result ---
            Dim generatedDataList As List(Of PasswordData) = Nothing
            ' Safely cast the result back to the expected list type.
            If e.Result IsNot Nothing AndAlso TypeOf e.Result Is List(Of PasswordData) Then
                generatedDataList = CType(e.Result, List(Of PasswordData))
            Else
                 ' Handle unexpected result type (shouldn't happen if DoWork is correct).
                 _errorLogger.WriteToErrorLog("Invalid result type received from bwgen_DoWork.", $"Expected List(Of PasswordData), Got: {e.Result?.GetType().ToString()}", "Internal Error")
                 Me.Text = WindowTitle & " - Error" ' Corrected &
                 MessageBox.Show("An internal error occurred processing password generation data.", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                 Return
            End If

            ' Check if generation actually produced anything.
            ' This can happen if the character set was empty (handled in DoWork) or other unexpected issues.
             If generatedDataList Is Nothing OrElse generatedDataList.Count = 0 Then
                 ' Check specifically if the character set was the issue (DoWork returns empty list in this case).
                 Dim characterSet As String = _passwordGenerator.BuildCharacterSet(chkUpperCase.Checked, chkLowerCase.Checked, chkNumbers.Checked, chkSpecialCharacters.Checked, chkSpace.Checked, chkCustomChars.Checked, txtCustomChars.Text)
                 If String.IsNullOrEmpty(characterSet) Then
                      ' Inform user that no character types were selected.
                      MessageBox.Show("Password generation failed: No character types selected. Batch stopped.", "Generation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                      Me.Text = WindowTitle & " - Failed" ' Corrected &
                 Else
                      ' Some other unexpected reason for an empty list. Log it.
                      _errorLogger.WriteToErrorLog("Received empty data list from successful worker completion.", "N/A", "Internal Warning")
                      MessageBox.Show("Password generation completed but produced no results.", "Generation Issue", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                      Me.Text = WindowTitle & " - Empty" ' Corrected &
                 End If
                 progGen.Value = 0 ' Reset progress bar.
                 Return
             End If

            ' --- Batch Generation successful, update UI ---
            Me.Text = WindowTitle & " - Populating List..." ' Corrected &

            ' Use ListView.BeginUpdate() and EndUpdate() for performance when adding many items.
            lstvKeys.BeginUpdate()
            Try
                Dim listViewItems As New List(Of ListViewItem) ' Temporary list to hold new ListView items.
                ' Iterate through the generated data.
                For i As Integer = 0 To generatedDataList.Count - 1
                    Dim data As PasswordData = generatedDataList(i)
                    ' Create a new ListViewItem for each password.
                    Dim newItem As New ListViewItem((i + 1).ToString()) ' Use loop index + 1 for numbering in the first column.
                    ' Add sub-items for each piece of data, matching the ListView columns.
                    newItem.SubItems.Add(data.Password)
                    newItem.SubItems.Add(data.StrengthDisplayText)
                    newItem.SubItems.Add(data.Length.ToString())
                    newItem.SubItems.Add(data.MD5Hash)
                    newItem.SubItems.Add(data.SHA256Hash)
                    newItem.SubItems.Add(data.SHA512Hash)
                    ' Store the raw entropy bits in the Tag property for easy retrieval later (e.g., in SelectedIndexChanged).
                    newItem.Tag = data.EntropyBits
                    listViewItems.Add(newItem) ' Add the fully populated item to the temporary list.
                Next

                ' Clear existing items *before* adding the new batch.
                lstvKeys.Items.Clear()
                ' Add all newly created items to the ListView at once.
                lstvKeys.Items.AddRange(listViewItems.ToArray())

                ' Select and display the details of the first generated password automatically.
                If lstvKeys.Items.Count > 0 Then
                    lstvKeys.Items(0).Selected = True
                    lstvKeys.Items(0).EnsureVisible() ' Scroll the list if necessary.
                    ' Trigger SelectedIndexChanged manually to update the details view (textboxes, entropy bar) for the first item.
                    LstvKeys_SelectedIndexChanged(lstvKeys, EventArgs.Empty)
                End If

            Finally ' Ensure EndUpdate is always called, even if errors occur during item creation/addition.
                lstvKeys.EndUpdate()
            End Try


            ' --- Final UI Updates ---
            progGen.Value = progGen.Maximum ' Ensure progress bar shows 100% complete.
            Me.Text = WindowTitle & " - Batch Complete" ' Corrected &
            MessageBox.Show("Finished generating all " & TotalKeysInBatch & " passwords.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information) ' Corrected &

        Catch ex As Exception
            ' Catch any errors that occur specifically within the RunWorkerCompleted handler itself (UI update logic).
            _isGeneratingBatch = False ' Ensure flag is reset.
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error in bwgen_RunWorkerCompleted")
            MessageBox.Show("An error occurred after password generation: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ' Corrected &
            Me.Text = WindowTitle & " - Error" ' Corrected &
        End Try
    End Sub

    ' This method runs on the UI thread when the background worker calls ReportProgress.
    Private Sub Bwgen_ProgressChanged(sender As System.Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bwgen.ProgressChanged ' IDE1006: Renamed method
        ' Update the main progress bar based on the number of passwords generated so far (passed in e.ProgressPercentage).
        progGen.Value = e.ProgressPercentage
        ' Update window title to show live progress.
        Me.Text = WindowTitle & " - Generating key " & e.ProgressPercentage & "/" & TotalKeysInBatch ' Corrected & (multiple)
    End Sub


    ' --- UI Event Handlers ---

    Private Sub BtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click ' IDE1006: Renamed method
        ' Check if there are any passwords in the list to save.
        If lstvKeys.Items.Count = 0 Then
            MessageBox.Show("There are no generated passwords in the list to save.", "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Use a SaveFileDialog to let the user choose the save location and filename.
        ' IDE0017: Object initialization can be simplified
        Using saveFileDialog As New SaveFileDialog With {
            .Filter = "Text Files(*.txt)|*.txt",
            .Title = "Save Generated Passwords and Hashes",
            .FileName = "GeneratedPasswords.txt" ' Suggest a default filename
        }
            ' Show the dialog and proceed only if the user clicks OK.
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Try
                    ' Call the PasswordSaver service to handle the actual file writing.
                    Dim success As Boolean = _passwordSaver.SavePasswordsToFile(saveFileDialog.FileName, lstvKeys.Items)
                    ' Inform the user of the outcome.
                    If success Then
                        MessageBox.Show("Passwords saved successfully to " & saveFileDialog.FileName, "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information) ' Corrected &
                    Else
                        MessageBox.Show("An error occurred while saving the file. Please check the error log.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Catch ex As Exception
                     ' Handle unexpected errors during the save process.
                     _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error in btnSave_Click")
                     MessageBox.Show("An unexpected error occurred during saving: " & ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ' Corrected &
                End Try
            End If
        End Using ' Dispose the SaveFileDialog.
    End Sub

    Private Sub ChkCustomChars_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkCustomChars.CheckedChanged ' IDE1006: Renamed method
        ' Enable/disable the custom characters textbox based on the checkbox state.
        txtCustomChars.Enabled = chkCustomChars.Checked
    End Sub

    Private Sub ProgEntropy_Click(sender As System.Object, e As System.EventArgs) Handles progEntropy.Click ' IDE1006: Renamed method
        ' No action needed when clicking the entropy progress bar.
    End Sub

    Private Sub Txtoutput_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtoutput.TextChanged ' IDE1006: Renamed method
        ' No action needed when the main password output text changes (it's updated programmatically).
    End Sub

    Private Sub BtnAbout_Click(sender As System.Object, e As System.EventArgs) Handles btnAbout.Click ' IDE1006: Renamed method
        ' Show the About form modally.
        Using aboutForm As New FrmAbout() ' Assuming FrmAbout is the class name
             aboutForm.ShowDialog(Me) ' ShowDialog makes it modal.
        End Using
    End Sub

    ' --- Clipboard Copy Button Handlers ---
    Private Sub BtnCopyMD5_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyMD5.Click ' IDE1006: Renamed method
        If Not String.IsNullOrEmpty(txtMD5.Text) Then
            Try
                Clipboard.SetText(txtMD5.Text)
            Catch ex As Exception
                 _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error copying MD5 to clipboard")
                 MessageBox.Show("Could not copy text to clipboard: " & ex.Message, "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) ' Corrected &
            End Try
        End If
    End Sub

    Private Sub BtnCopySHA256_Click(sender As System.Object, e As System.EventArgs) Handles btnCopySHA256.Click ' IDE1006: Renamed method
         If Not String.IsNullOrEmpty(txtSHA256.Text) Then
            Try
                Clipboard.SetText(txtSHA256.Text)
            Catch ex As Exception
                 _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error copying SHA256 to clipboard")
                 MessageBox.Show("Could not copy text to clipboard: " & ex.Message, "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) ' Corrected &
            End Try
        End If
    End Sub

    Private Sub BtnCopySHA512_Click(sender As System.Object, e As System.EventArgs) Handles btnCopySHA512.Click ' IDE1006: Renamed method
         If Not String.IsNullOrEmpty(txtSHA512.Text) Then
            Try
                Clipboard.SetText(txtSHA512.Text)
            Catch ex As Exception
                 _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error copying SHA512 to clipboard")
                 MessageBox.Show("Could not copy text to clipboard: " & ex.Message, "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) ' Corrected &
            End Try
        End If
    End Sub

    ' --- ValueChanged Handlers for NumericUpDowns (No action needed currently) ---
    Private Sub TxtPassAmount_ValueChanged(sender As Object, e As EventArgs) Handles txtPassAmount.ValueChanged
        ' No action needed here when the amount value changes directly.
    End Sub

    Private Sub TxtMaxLength_ValueChanged(sender As Object, e As EventArgs) Handles txtMaxLength.ValueChanged
        ' No action needed here when the length value changes directly.
    End Sub

    ' This event handler updates the detail view (textboxes, entropy bar) when the user selects an item in the ListView.
    Private Sub LstvKeys_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstvKeys.SelectedIndexChanged ' IDE1006: Renamed method
        Try
            ' Check if at least one item is selected.
            If lstvKeys.SelectedItems.Count > 0 Then
                ' Get the first selected item (assuming single selection mode).
                Dim selectedItem As ListViewItem = lstvKeys.SelectedItems(0)

                ' Ensure the selected item has the expected number of sub-items (columns).
                If selectedItem.SubItems.Count >= 7 Then
                    ' --- Update TextBoxes ---
                    ' Populate the textboxes with data from the selected item's sub-items.
                    ' Indices correspond to the column order in the ListView.
                    txtoutput.Text = selectedItem.SubItems(1).Text ' Password
                    txtMD5.Text = selectedItem.SubItems(4).Text    ' MD5
                    txtSHA256.Text = selectedItem.SubItems(5).Text ' SHA256
                    txtSHA512.Text = selectedItem.SubItems(6).Text ' SHA512

                    ' --- Update Entropy Display ---
                    ' Get the pre-formatted strength display text from the sub-item.
                    Dim strengthDisplayText As String = selectedItem.SubItems(2).Text
                    lblEntropy.Text = strengthDisplayText ' Set the label text directly.

                    ' --- Parse Entropy Bits for Color and Progress Bar ---
                    Dim entropyBits As Integer = 0
                    ' Try getting the raw bits from the Tag property first (more reliable if set during generation).
                    If selectedItem.Tag IsNot Nothing AndAlso TypeOf selectedItem.Tag Is Integer Then
                        entropyBits = CInt(selectedItem.Tag)
                    Else
                        ' Fallback: If Tag is not available or not an integer, try parsing the bits from the display text.
                        Try
                            ' Use Regex to extract the number of bits from the string like "Password Strength: XX bits (...)".
                            Dim match As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(strengthDisplayText, ":\s*(\d+)\s*bits")
                            If match.Success AndAlso match.Groups.Count > 1 Then
                                ' Try parsing the captured group.
                                Integer.TryParse(match.Groups(1).Value, entropyBits)
                            Else
                                 ' Log a warning if parsing fails.
                                 _errorLogger.WriteToErrorLog($"Could not parse entropy bits from ListView text using Regex: '{strengthDisplayText}'.", "Parsing Error", "UI Warning")
                                 entropyBits = 0 ' Default to 0 on parsing failure.
                            End If
                        Catch ex As Exception
                            ' Log errors during Regex matching or parsing.
                            _errorLogger.WriteToErrorLog($"Failed to parse entropy bits from ListView text: '{strengthDisplayText}'. Error: {ex.Message}", ex.StackTrace, "UI Warning")
                            entropyBits = 0 ' Default to 0 on error.
                        End Try
                    End If


                    ' --- Determine Color Based on Parsed Bits ---
                    ' Set the color of the entropy label based on the strength category.
                    Dim strengthColor As Color
                    If entropyBits < 40 Then
                        strengthColor = Color.Red
                    ElseIf entropyBits < 60 Then
                        strengthColor = Color.Orange
                    ElseIf entropyBits < 80 Then
                        strengthColor = Color.YellowGreen
                    ElseIf entropyBits < 128 Then
                        strengthColor = Color.Green
                    Else
                        strengthColor = Color.DarkGreen
                    End If
                    lblEntropy.ForeColor = strengthColor

                    ' --- Update Entropy Progress Bar ---
                    ' Set the value of the progress bar, clamping it within the bar's min/max range.
                    progEntropy.Value = CInt(Math.Max(progEntropy.Minimum, Math.Min(progEntropy.Maximum, entropyBits)))

                Else
                     ' Log a warning if the selected item doesn't have enough sub-items.
                     _errorLogger.WriteToErrorLog("Selected ListViewItem has insufficient SubItems.", $"Item Index: {selectedItem.Index}, SubItem Count: {selectedItem.SubItems.Count}", "UI Warning")
                End If
            End If
        Catch ex As Exception
            ' Log any errors occurring within the SelectedIndexChanged handler.
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error in lstvKeys_SelectedIndexChanged")
        End Try
    End Sub

    ' --- Context Menu Event Handlers ---

    Private Sub cmsPasswordList_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmsPasswordList.Opening
        Dim hasSelection As Boolean = (lstvKeys.SelectedItems.Count > 0)

        tsmiCopyPassword.Enabled = hasSelection
        tsmiSaveSelectedPasswords.Enabled = hasSelection

        ' Prevent the menu from showing if nothing is selected
        If Not hasSelection Then
            e.Cancel = True
        End If
    End Sub

    Private Sub tsmiCopyPassword_Click(sender As Object, e As EventArgs) Handles tsmiCopyPassword.Click
        If lstvKeys.SelectedItems.Count > 0 Then
            Dim sb As New System.Text.StringBuilder()
            For Each item As ListViewItem In lstvKeys.SelectedItems
                ' Assuming password is in the second column (index 1)
                If item.SubItems.Count > 1 Then
                    sb.AppendLine(item.SubItems(1).Text)
                End If
            Next

            If sb.Length > 0 Then
                Try
                    ' Remove the last newline if it exists (optional, but cleaner)
                    If sb.ToString().EndsWith(Environment.NewLine) Then
                         sb.Length -= Environment.NewLine.Length
                    End If
                    Clipboard.SetText(sb.ToString())
                Catch ex As Exception
                    _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error copying selected passwords to clipboard")
                    MessageBox.Show("Could not copy text to clipboard: " & ex.Message, "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Try
            End If
        End If
    End Sub

    Private Sub tsmiSaveSelectedPasswords_Click(sender As Object, e As EventArgs) Handles tsmiSaveSelectedPasswords.Click
        If lstvKeys.SelectedItems.Count = 0 Then
            MessageBox.Show("No passwords selected to save.", "Nothing Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Using saveFileDialog As New SaveFileDialog With {
            .Filter = "Text Files(*.txt)|*.txt|All Files(*.*)|*.*", ' Added All Files option
            .Title = "Save Selected Passwords",
            .FileName = "SelectedPasswords.txt" ' Suggest a default filename
        }
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Try
                    ' Call the *new* PasswordSaver service method
                    Dim success As Boolean = _passwordSaver.SaveSelectedPasswordsToFile(saveFileDialog.FileName, lstvKeys.SelectedItems)

                    If success Then
                        MessageBox.Show($"Selected passwords saved successfully to {saveFileDialog.FileName}", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        ' Specific error should have been logged by the service
                        MessageBox.Show("An error occurred while saving the selected passwords. Please check the error log.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Catch ex As Exception
                     _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error in tsmiSaveSelectedPasswords_Click")
                     MessageBox.Show("An unexpected error occurred during saving: " & ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

End Class
