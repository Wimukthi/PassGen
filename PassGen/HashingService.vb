Imports System.Security.Cryptography
Imports System.Text

Public Class HashingService

    Private _errorLogger As ErrorLogger

    ''' <summary>
    ''' Initializes a new instance of the HashingService class.
    ''' </summary>
    ''' <param name="logger">An instance of ErrorLogger for logging potential errors.</param>
    Public Sub New(logger As ErrorLogger)
        If logger Is Nothing Then Throw New ArgumentNullException("logger")
        _errorLogger = logger
    End Sub

    ''' <summary>
    ''' Generates an unsalted MD5 hash of the input string.
    ''' </summary>
    ''' <param name="plainText">The string to hash.</param>
    ''' <returns>A Base64 encoded string representing the MD5 hash, or an empty string on error.</returns>
    Public Function GenerateMD5Hash(plainText As String) As String
        If plainText Is Nothing Then plainText = "" ' Handle null input gracefully

        Try
            Using md5Provider As New MD5CryptoServiceProvider()
                Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
                Dim hashBytes As Byte() = md5Provider.ComputeHash(plainTextBytes)
                Return Convert.ToBase64String(hashBytes)
            End Using
        Catch ex As Exception
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error generating MD5 hash in HashingService")
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Generates an unsalted SHA256 hash of the input string.
    ''' </summary>
    ''' <param name="plainText">The string to hash.</param>
    ''' <returns>A Base64 encoded string representing the SHA256 hash, or an empty string on error.</returns>
    Public Function GenerateSHA256Hash(plainText As String) As String
        If plainText Is Nothing Then plainText = "" ' Handle null input gracefully

        Try
            Using sha256Provider As New SHA256Managed()
                Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
                Dim hashBytes As Byte() = sha256Provider.ComputeHash(plainTextBytes)
                Return Convert.ToBase64String(hashBytes)
            End Using
        Catch ex As Exception
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error generating SHA256 hash in HashingService")
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Generates an unsalted SHA512 hash of the input string.
    ''' </summary>
    ''' <param name="plainText">The string to hash.</param>
    ''' <returns>A Base64 encoded string representing the SHA512 hash, or an empty string on error.</returns>
    Public Function GenerateSHA512Hash(plainText As String) As String
        If plainText Is Nothing Then plainText = "" ' Handle null input gracefully

        Try
            Using sha512Provider As New SHA512Managed()
                Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
                Dim hashBytes As Byte() = sha512Provider.ComputeHash(plainTextBytes)
                Return Convert.ToBase64String(hashBytes)
            End Using
        Catch ex As Exception
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error generating SHA512 hash in HashingService")
            Return String.Empty
        End Try
    End Function

End Class