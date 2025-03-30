Imports System.IO
Imports System.Windows.Forms ' Needed for ListViewItem

Public Class PasswordSaver

    Private _errorLogger As ErrorLogger

    ''' <summary>
    ''' Initializes a new instance of the PasswordSaver class.
    ''' </summary>
    ''' <param name="logger">An instance of ErrorLogger for logging potential errors.</param>
    Public Sub New(logger As ErrorLogger)
        If logger Is Nothing Then Throw New ArgumentNullException("logger")
        _errorLogger = logger
    End Sub

    ''' <summary>
    ''' Saves the provided list of password data to the specified file path using a specific tagged format.
    ''' </summary>
    ''' <param name="filePath">The full path of the file to save to.</param>
    ''' <param name="passwordItems">A ListView.ListViewItemCollection containing the password data to save.
    ''' Each item's SubItems should correspond to: Index(0), Password(1), Entropy(2), Length(3), MD5(4), SHA256(5), SHA512(6).</param>
    ''' <returns>True if saving was successful, False otherwise.</returns>
    Public Function SavePasswordsToFile(filePath As String, passwordItems As ListView.ListViewItemCollection) As Boolean
        ' Validate inputs: Ensure file path is provided and there are items to save.
        If String.IsNullOrWhiteSpace(filePath) OrElse passwordItems Is Nothing OrElse passwordItems.Count = 0 Then
            Return False ' Nothing to save or invalid path
        End If

        Dim success As Boolean = False
        Dim streamSavePass As StreamWriter = Nothing ' Initialize to Nothing for Finally block safety.

        Try
            ' Open the file for writing. Creates the file if it doesn't exist, overwrites it if it does.
            ' Consider using a Using block for automatic disposal, though the Finally block handles it here.
            streamSavePass = New StreamWriter(filePath)

            ' Iterate through each ListViewItem representing a generated password.
            For Each item As ListViewItem In passwordItems
                ' Ensure the item has enough subitems (columns) to prevent index out-of-bounds errors.
                If item.SubItems.Count >= 7 Then
                    ' Extract data from the relevant ListViewItem sub-items based on column index.
                    Dim password As String = item.SubItems(1).Text
                    ' Dim entropy As String = item.SubItems(2).Text ' Entropy text is not saved in this format.
                    ' Dim length As String = item.SubItems(3).Text  ' Length is not saved in this format.
                    Dim md5Hash As String = item.SubItems(4).Text
                    Dim sha256Hash As String = item.SubItems(5).Text
                    Dim sha512Hash As String = item.SubItems(6).Text

                    ' --- Write data for this password in the specific tagged format ---
                    ' Each piece of data is enclosed in start/end tags on separate lines.
                    ' Blank lines are added for readability between blocks.

                    streamSavePass.WriteLine("<Key-Start>") ' Start tag for the password itself.
                    streamSavePass.WriteLine(password)
                    streamSavePass.WriteLine("<Key-End>")   ' End tag for the password.
                    streamSavePass.WriteLine()              ' Blank line separator.

                    streamSavePass.WriteLine("<MD5-Start>") ' Start tag for the MD5 hash.
                    streamSavePass.WriteLine(md5Hash)
                    streamSavePass.WriteLine("<MD5-End>")   ' End tag for the MD5 hash.
                    streamSavePass.WriteLine()              ' Blank line separator.

                    streamSavePass.WriteLine("<SHA256-Start>") ' Start tag for the SHA256 hash.
                    streamSavePass.WriteLine(sha256Hash)
                    streamSavePass.WriteLine("<SHA256-End>")   ' End tag for the SHA256 hash.
                    streamSavePass.WriteLine()                ' Blank line separator.

                    streamSavePass.WriteLine("<SHA512-Start>") ' Start tag for the SHA512 hash.
                    streamSavePass.WriteLine(sha512Hash)
                    streamSavePass.WriteLine("<SHA512-End>")   ' End tag for the SHA512 hash.
                    streamSavePass.WriteLine()                ' Blank line separator.

                    streamSavePass.WriteLine("--------------------") ' Separator line between different password entries.
                    streamSavePass.WriteLine()                      ' Blank line after separator.
                Else
                    ' Log a warning if a ListViewItem doesn't have the expected structure (e.g., fewer than 7 columns).
                    _errorLogger.WriteToErrorLog("ListViewItem skipped during save due to insufficient SubItems.", $"Item Index: {item.Index}", "Password Save Warning")
                End If
            Next ' Move to the next password item.

            success = True ' Mark as successful if the loop completes without throwing an exception.
        Catch ex As Exception
            ' Handle potential file I/O errors (e.g., disk full, permissions issues).
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error saving passwords to file in PasswordSaver")
            success = False ' Ensure success is false if an error occurred.
        Finally
            ' Ensure the StreamWriter is always closed and disposed, even if an error occurred.
            If streamSavePass IsNot Nothing Then
                streamSavePass.Close()
                streamSavePass.Dispose() ' Explicitly dispose to release resources.
            End If
        End Try

        Return success ' Return the success status.
    End Function

End Class