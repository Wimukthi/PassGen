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
    ''' Saves the provided list of password data to the specified file path.
    ''' Matches the original saving format.
    ''' </summary>
    ''' <param name="filePath">The full path of the file to save to.</param>
    ''' <param name="passwordItems">A ListView.ListViewItemCollection containing the password data to save.
    ''' Each item's SubItems should correspond to: Index, Password, Entropy, Length, MD5, SHA256, SHA512.</param>
    ''' <returns>True if saving was successful, False otherwise.</returns>
    Public Function SavePasswordsToFile(filePath As String, passwordItems As ListView.ListViewItemCollection) As Boolean
        If String.IsNullOrWhiteSpace(filePath) OrElse passwordItems Is Nothing OrElse passwordItems.Count = 0 Then
            Return False ' Nothing to save or invalid path
        End If

        Dim success As Boolean = False
        Dim streamSavePass As StreamWriter = Nothing ' Initialize to Nothing

        Try
            streamSavePass = New StreamWriter(filePath) ' Overwrites if exists, creates if not

            For Each item As ListViewItem In passwordItems
                ' Ensure the item has enough subitems to prevent index errors
                If item.SubItems.Count >= 7 Then
                    ' Extract data from the ListViewItem (adjust indices if needed)
                    Dim password As String = item.SubItems(1).Text
                    ' Dim entropy As String = item.SubItems(2).Text ' Not saved in original format block
                    ' Dim length As String = item.SubItems(3).Text ' Not saved in original format block
                    Dim md5Hash As String = item.SubItems(4).Text
                    Dim sha256Hash As String = item.SubItems(5).Text
                    Dim sha512Hash As String = item.SubItems(6).Text

                    ' Write data for this password in the specific format
                    streamSavePass.WriteLine("<Key-Start>")
                    streamSavePass.WriteLine(password)
                    streamSavePass.WriteLine("<Key-End>")
                    streamSavePass.WriteLine() ' Blank line

                    streamSavePass.WriteLine("<MD5-Start>")
                    streamSavePass.WriteLine(md5Hash)
                    streamSavePass.WriteLine("<MD5-End>")
                    streamSavePass.WriteLine() ' Blank line

                    streamSavePass.WriteLine("<SHA256-Start>")
                    streamSavePass.WriteLine(sha256Hash)
                    streamSavePass.WriteLine("<SHA256-End>")
                    streamSavePass.WriteLine() ' Blank line

                    streamSavePass.WriteLine("<SHA512-Start>")
                    streamSavePass.WriteLine(sha512Hash)
                    streamSavePass.WriteLine("<SHA512-End>")
                    streamSavePass.WriteLine() ' Blank line
                    streamSavePass.WriteLine("--------------------") ' Separator between keys
                    streamSavePass.WriteLine() ' Blank line
                Else
                    ' Log a warning if an item doesn't have the expected structure
                    _errorLogger.WriteToErrorLog("ListViewItem skipped during save due to insufficient SubItems.", $"Item Index: {item.Index}", "Password Save Warning")
                End If
            Next
            success = True ' Mark as successful if loop completes without throwing
        Catch ex As Exception
            ' Handle potential file writing errors
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error saving passwords to file in PasswordSaver")
            ' Optionally re-throw or display a message, but here we just return false
            success = False
        Finally
            ' Ensure the stream is closed even if an error occurs
            If streamSavePass IsNot Nothing Then
                streamSavePass.Close()
                streamSavePass.Dispose() ' Explicitly dispose
            End If
        End Try

        Return success
    End Function

End Class