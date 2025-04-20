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
    ''' Writes the data for a single ListViewItem to the StreamWriter in the specific tagged format.
    ''' </summary>
    ''' <param name="writer">The StreamWriter to write to.</param>
    ''' <param name="item">The ListViewItem containing the password data.</param>
    Private Sub WritePasswordDataTagged(writer As StreamWriter, item As ListViewItem)
        ' Ensure the item has enough subitems (columns) to prevent index out-of-bounds errors.
        If item.SubItems.Count >= 7 Then
            ' Extract data from the relevant ListViewItem sub-items based on column index.
            Dim password As String = item.SubItems(1).Text
            Dim md5Hash As String = item.SubItems(4).Text
            Dim sha256Hash As String = item.SubItems(5).Text
            Dim sha512Hash As String = item.SubItems(6).Text

            ' --- Write data for this password in the specific tagged format ---
            writer.WriteLine("<Key-Start>")
            writer.WriteLine(password)
            writer.WriteLine("<Key-End>")
            writer.WriteLine()

            writer.WriteLine("<MD5-Start>")
            writer.WriteLine(md5Hash)
            writer.WriteLine("<MD5-End>")
            writer.WriteLine()

            writer.WriteLine("<SHA256-Start>")
            writer.WriteLine(sha256Hash)
            writer.WriteLine("<SHA256-End>")
            writer.WriteLine()

            writer.WriteLine("<SHA512-Start>")
            writer.WriteLine(sha512Hash)
            writer.WriteLine("<SHA512-End>")
            writer.WriteLine()

            writer.WriteLine("--------------------") ' Separator line between different password entries.
            writer.WriteLine()
        Else
            ' Log a warning if a ListViewItem doesn't have the expected structure.
            _errorLogger?.WriteToErrorLog("ListViewItem skipped during save due to insufficient SubItems.", $"Item Index: {item.Index}", "Password Save Warning")
        End If
    End Sub

    ''' <summary>
    ''' Saves ALL provided password data to the specified file path using the tagged format.
    ''' </summary>
    ''' <param name="filePath">The full path of the file to save to.</param>
    ''' <param name="passwordItems">A ListView.ListViewItemCollection containing ALL password data to save.</param>
    ''' <returns>True if saving was successful, False otherwise.</returns>
    Public Function SavePasswordsToFile(filePath As String, passwordItems As ListView.ListViewItemCollection) As Boolean
        If String.IsNullOrWhiteSpace(filePath) OrElse passwordItems Is Nothing OrElse passwordItems.Count = 0 Then
            Return False
        End If

        Try
            Using writer As New StreamWriter(filePath, False, System.Text.Encoding.UTF8) ' Overwrite, UTF8
                For Each item As ListViewItem In passwordItems
                    WritePasswordDataTagged(writer, item) ' Call the helper method
                Next
            End Using
            Return True
        Catch ex As Exception
            _errorLogger?.WriteToErrorLog($"Error saving all passwords to file '{filePath}': {ex.Message}", ex.StackTrace, "SavePasswordsToFile Error")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Saves ONLY the SELECTED password data to the specified file path using the tagged format.
    ''' </summary>
    ''' <param name="filePath">The full path to the file where passwords should be saved.</param>
    ''' <param name="selectedItems">The collection of SELECTED ListViewItems containing password data.</param>
    ''' <returns>True if saving was successful, False otherwise.</returns>
    Public Function SaveSelectedPasswordsToFile(filePath As String, selectedItems As ListView.SelectedListViewItemCollection) As Boolean
        If String.IsNullOrWhiteSpace(filePath) OrElse selectedItems Is Nothing OrElse selectedItems.Count = 0 Then
            Return False
        End If

        Try
            Using writer As New StreamWriter(filePath, False, System.Text.Encoding.UTF8) ' Overwrite, UTF8
                For Each item As ListViewItem In selectedItems
                    WritePasswordDataTagged(writer, item) ' Call the SAME helper method
                Next
            End Using
            Return True
        Catch ex As Exception
             _errorLogger?.WriteToErrorLog($"Error saving selected passwords to file '{filePath}': {ex.Message}", ex.StackTrace, "SaveSelectedPasswordsToFile Error")
            Return False
        End Try
    End Function

End Class