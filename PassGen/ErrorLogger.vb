Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Provides functionality to log error messages and stack traces to a text file.
''' </summary>
<CLSCompliant(True)> _
Public Class ErrorLogger

    ''' <summary>
    ''' Initializes a new instance of the ErrorLogger class.
    ''' </summary>
    Public Sub New()

        'default constructor - no specific initialization needed here

    End Sub

    ''' <summary>
    ''' Writes error details to the Error_Log.txt file located in an 'Errors' subdirectory within the application's startup path.
    ''' Creates the directory and file if they do not exist. Appends new entries to the log.
    ''' </summary>
    ''' <param name="msg">The error message to log.</param>
    ''' <param name="stkTrace">The stack trace associated with the error.</param>
    ''' <param name="title">A title or context for the error entry.</param>
    Public Sub WriteToErrorLog(ByVal msg As String, ByVal stkTrace As String, ByVal title As String)
        Try
            ' Define the directory path for errors
            Dim errorDirectory As String = Path.Combine(Application.StartupPath, "Errors")
            ' Define the full path for the error log file
            Dim errorLogPath As String = Path.Combine(errorDirectory, "Error_Log.txt")

            ' Ensure the error directory exists
            If Not Directory.Exists(errorDirectory) Then
                Directory.CreateDirectory(errorDirectory)
            End If

            ' Log the error details
            ' Using FileMode.Append automatically creates the file if it doesn't exist
            ' Using statement ensures the StreamWriter and FileStream are properly disposed
            Using fs As FileStream = New FileStream(errorLogPath, FileMode.Append, FileAccess.Write, FileShare.Read) ' Allow reading while writing
                Using sw As StreamWriter = New StreamWriter(fs)
                    sw.WriteLine("Title: " & title)
                    sw.WriteLine("Message: " & msg)
                    sw.WriteLine("StackTrace: " & stkTrace)
                    sw.WriteLine("Date/Time: " & DateTime.Now.ToString())
                    sw.WriteLine("===========================================================================================")
                End Using ' StreamWriter closed/disposed here
            End Using ' FileStream closed/disposed here

        Catch ex As Exception
            ' Error occurred while trying to log.
            ' Avoid infinite loops if logging itself fails.
            ' Consider logging to Debug console or Event Log in a real application.
            System.Diagnostics.Debug.WriteLine($"Failed to write to error log: {ex.Message}")
        End Try
    End Sub

End Class
