' ----------------------------------------------------------------------------------------
' Author:                    Wimukthi Bandara
' Company:                   Grey Element Software
' Assembly version:          1.1.1.173
' ----------------------------------------------------------------------------------------
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' any later version.
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' You should have received a copy of the GNU General Public License
' along with this program.  If not, see http://www.gnu.org/licenses/.
' ----------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.IO


Public Class frmMain

    Private strCharacters As String 'Variable to hold the characters
    Private strNewPassword As String 'Holds the generated password
    Private strNewPasswordEntropyBuffer As String 'Stores the new password so the Entropy can be calculated
    Public Plength As Integer 'Stores the length of the password

    Public WindowTitle As String 'Sores the window title


    Private Sub frmMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try


            WindowTitle = "Grey Element Software - PassGen v" & My.Application.Info.Version.ToString
            Me.Text = WindowTitle & " - Idle" 'Set the Default window title

        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try
    End Sub

    Private Sub btnGenerate_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerate.Click
        Try


            Plength = txtMaxLength.Value 'Set the currently selected password length
            progGen.Maximum = txtMaxLength.Value  'Sets the maximum value to be displayed on the progress bar

            If Val(txtMaxLength.Value) > 0 Then ' if password length is not 0
                If bwgen.IsBusy <> True Then
                    ' Start the asynchronous operation.
                    bwgen.RunWorkerAsync()
                End If


            End If
        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try
    End Sub

    Private Sub bwgen_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwgen.DoWork
        Try

            'Create a new background worker so the progress can be reported back
            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

            strCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~`!@#$%^&*()_+=-{[}]|;:'<,>.? " & txtCustomChars.Text 'Set the default char set

            Dim p As Integer


            If chkUpperCase.Checked = False Then 'Removes the Upper case characters from the string
                strCharacters = strCharacters.Remove(strCharacters.IndexOf("ABCDEFGHIJKLMNOPQRSTUVWXYZ"), Len("ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
            End If
            If chkLowerCase.Checked = False Then 'Removes the lower case characters from the string
                strCharacters = strCharacters.Remove(strCharacters.IndexOf("abcdefghijklmnopqrstuvwxyz"), Len("abcdefghijklmnopqrstuvwxyz"))
            End If
            If chkNumbers.Checked = False Then 'Removes the Numbers from the string
                strCharacters = strCharacters.Remove(strCharacters.IndexOf("0123456789"), Len("0123456789"))
            End If
            If chkSpecialCharacters.Checked = False Then 'Removes the special characters from the string
                strCharacters = strCharacters.Remove(strCharacters.IndexOf("~`!@#$%^&*()_+=-{[}]|;:'<,>.?"), Len("~`!@#$%^&*()_+=-{[}]|;:'<,>.?"))
            End If
            If chkSpace.Checked = False Then 'Removes Space characters from the string
                strCharacters = strCharacters.Remove(strCharacters.IndexOf(" "), Len(" "))
            End If
            If chkCustomChars.Checked = False Then 'Removes Custom characters from the string
                strCharacters = strCharacters.Remove(strCharacters.IndexOf(txtCustomChars.Text), Len(txtCustomChars.Text))
            End If
            'initialize the Pesudo Random number generator
            Randomize()

            If strCharacters.Length <> 0 Then
                For p = 0 To (Plength - 1) 'Loop through the string each turn selecting a random character and adding it to the strnewpassword variable do this 0 to plength
                    strNewPassword = strNewPassword + Mid(strCharacters, Len(strCharacters) * Rnd() + 1, 1) 'random character selection
                    worker.ReportProgress(p) 'Reports the progress
                Next
                e.Result = strNewPassword 'Sends the new password back

                'txtoutput.Text = strNewPassword
            End If
        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try
    End Sub

    Private Sub bwgen_RunWorkerCompleted(sender As System.Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwgen.RunWorkerCompleted
        Try

            If strCharacters.Length <> 0 Then

                txtoutput.Text = e.Result 'Display the Generated password

                Me.Text = WindowTitle & " - Idle" 'Reset the window title

                strNewPassword = "" 'Clear the variable so a new password can be generated

                If progGen.Value = txtMaxLength.Value - 1 Then 'Clear the progress bar
                    progGen.Value = 0

                End If
            End If
        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try
    End Sub




    Private Sub bwgen_ProgressChanged(sender As System.Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bwgen.ProgressChanged
        Try
            If strCharacters.Length <> 0 Then

                progGen.Value = e.ProgressPercentage.ToString()
                If progGen.Value < txtMaxLength.Value Then
                    Me.Text = WindowTitle & " - Processing Loop : " & progGen.Value.ToString

                End If
            End If

        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try
    End Sub


    Private Sub btnsave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Try
            Dim SavePassword As System.Windows.Forms.SaveFileDialog
            SavePassword = New System.Windows.Forms.SaveFileDialog()
            SavePassword.CreatePrompt = False
            SavePassword.Filter = "Text Files(*.txt) |*.txt"
            SavePassword.Title = "Save the generated Password and Hash values to a file!"
            If txtoutput.Text <> "" Then

                If SavePassword.ShowDialog() = DialogResult.OK Then
                    Dim StreamSavePass As StreamWriter
                    StreamSavePass = New StreamWriter(SavePassword.FileName)


                    StreamSavePass.Write("<Key-Start>")
                    StreamSavePass.Write(txtoutput.Text)
                    StreamSavePass.Write("<Key-End>")
                    StreamSavePass.Write(vbCrLf)
                    StreamSavePass.Write(vbCrLf)

                    StreamSavePass.Write("<MD5-Start>")
                    StreamSavePass.Write(txtMD5.Text)
                    StreamSavePass.Write("<MD5-End>")
                    StreamSavePass.Write(vbCrLf)
                    StreamSavePass.Write(vbCrLf)

                    StreamSavePass.Write("<SHA256-Start>")
                    StreamSavePass.Write(txtSHA256.Text)
                    StreamSavePass.Write("<SHA256-End>")
                    StreamSavePass.Write(vbCrLf)
                    StreamSavePass.Write(vbCrLf)

                    StreamSavePass.Write("<SHA512-Start>")
                    StreamSavePass.Write(txtSHA512.Text)
                    StreamSavePass.Write("<SHA512-End>")

                    StreamSavePass.Close()
                End If
            End If
        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try
    End Sub

    Private Sub chkCustomChars_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkCustomChars.CheckedChanged
        txtCustomChars.Enabled = chkCustomChars.CheckState
    End Sub

    Private Sub progEntropy_Click(sender As System.Object, e As System.EventArgs) Handles progEntropy.Click

    End Sub

    Private Sub txtoutput_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtoutput.TextChanged
        Try
            If txtoutput.Text <> "" Then
                strNewPasswordEntropyBuffer = txtoutput.Text 'Sends thew new password to the Entropy Buffer
                If Not threadEntropy.IsBusy = True Then
                    threadEntropy.RunWorkerAsync()
                End If
            Else
                lblEntropy.Text = "Password Entropy : 0 bits"
                txtSHA256.Text = ""
                txtSHA512.Text = ""
            End If

        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try
    End Sub

    Private Sub threadEntropy_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles threadEntropy.DoWork
        'Create a new background worker so the progress can be reported back
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        Try

            Dim i As Long
            Dim myArray(2048) As Short
            Dim TtlLen As Short
            Dim Unique As Short

            TtlLen = Len(strNewPasswordEntropyBuffer)
            For i = 1 To TtlLen
                ' There is one myArray index for each character of the ASCII table.
                ' Each time a specific character is found in the original string,
                ' the respective myArray index is increased by one.
                myArray(Asc(Mid(strNewPasswordEntropyBuffer, i, 1))) = myArray(Asc(Mid(strNewPasswordEntropyBuffer, i, 1))) + 1
                ' If the above line results in the respective index of myArray being = to 1,
                ' then this is a unique character. When the value of this array index goes
                ' to 2 and above, then the character in question is no longer unique.
                Unique = Unique + (CShort(myArray(Asc(Mid(strNewPasswordEntropyBuffer, i, 1))) = 1) * -1)
                worker.ReportProgress(i) 'Reports the progress
            Next i

            e.Result = Unique
        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try
    End Sub

    Private Sub threadEntropy_ProgressChanged(sender As System.Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles threadEntropy.ProgressChanged
        progGen.Value = e.ProgressPercentage.ToString()
        Me.Text = WindowTitle & " - Building Entropy Buffer : " & e.ProgressPercentage
        lblEntropy.Text = "Calculating Entropy, Please Wait.."
    End Sub

    Private Sub threadEntropy_RunWorkerCompleted(sender As System.Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles threadEntropy.RunWorkerCompleted
        Try


            Dim pwentropy As String = PasswordEntropy(txtMaxLength.Value.ToString, e.Result)



            If pwentropy > 256 Then
                progEntropy.Value = 256
            Else
                progEntropy.Value = pwentropy
            End If
            Me.Text = WindowTitle & " - Idle"


            If txtoutput.Text <> "" Then
                lblEntropy.Text = "Password Entropy : " & Int(pwentropy) & " bits"
                'Calculate the SHA Hash values
                txtSHA256.Text = SHA.GenerateHash(txtoutput.Text, "SHA256", Nothing)
                txtSHA512.Text = SHA.GenerateHash(txtoutput.Text, "SHA512", Nothing)
                txtMD5.Text = MD5.GenMD5(txtoutput.Text)
            End If


        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
        End Try

    End Sub

    Private Sub btnAbout_Click(sender As System.Object, e As System.EventArgs) Handles btnAbout.Click
        frmabout.ShowDialog(Me) 'Show the ABout dialog box
    End Sub

    Private Sub btnCopyMD5_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyMD5.Click
        If txtMD5.Text <> "" Then Clipboard.SetText(txtMD5.Text)

    End Sub

    Private Sub btnCopySHA256_Click(sender As System.Object, e As System.EventArgs) Handles btnCopySHA256.Click
        If txtSHA256.Text <> "" Then Clipboard.SetText(txtSHA256.Text)
    End Sub

    Private Sub btnCopySHA512_Click(sender As System.Object, e As System.EventArgs) Handles btnCopySHA512.Click
        If txtSHA512.Text <> "" Then Clipboard.SetText(txtSHA512.Text)
    End Sub
End Class
