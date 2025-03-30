Imports System.Security.Cryptography
Imports System.Text

Public Class PasswordGenerator

    Private _errorLogger As ErrorLogger

    ''' <summary>
    ''' Initializes a new instance of the PasswordGenerator class.
    ''' </summary>
    ''' <param name="logger">An instance of ErrorLogger for logging potential errors.</param>
    Public Sub New(logger As ErrorLogger)
        If logger Is Nothing Then Throw New ArgumentNullException("logger")
        _errorLogger = logger
    End Sub

    ''' <summary>
    ''' Builds the string of allowed characters based on the specified options.
    ''' </summary>
    ''' <param name="useUpperCase">Include uppercase letters (A-Z).</param>
    ''' <param name="useLowerCase">Include lowercase letters (a-z).</param>
    ''' <param name="useNumbers">Include numbers (0-9).</param>
    ''' <param name="useSpecial">Include special characters (~`!@#$%^&amp;*()_+=-{[}]|;:'<,>.?).</param>
    ''' <param name="useSpace">Include the space character.</param>
    ''' <param name="useCustom">Include custom characters specified in customChars.</param>
    ''' <param name="customChars">A string containing custom characters to include.</param>
    ''' <returns>A string containing all allowed characters for password generation.</returns>
    Public Function BuildCharacterSet(useUpperCase As Boolean, useLowerCase As Boolean, useNumbers As Boolean, useSpecial As Boolean, useSpace As Boolean, useCustom As Boolean, customChars As String) As String
        Dim allowedChars As New StringBuilder()

        Try
            If useUpperCase Then allowedChars.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            If useLowerCase Then allowedChars.Append("abcdefghijklmnopqrstuvwxyz")
            If useNumbers Then allowedChars.Append("0123456789")
            If useSpecial Then allowedChars.Append("~`!@#$%^&amp;*()_+=-{[}]|;:'<,>.?")
            If useSpace Then allowedChars.Append(" ")
            If useCustom AndAlso Not String.IsNullOrWhiteSpace(customChars) Then
                ' Append only unique custom characters not already present
                For Each c As Char In customChars
                    If allowedChars.ToString().IndexOf(c) = -1 Then
                        allowedChars.Append(c)
                    End If
                Next
            End If
        Catch ex As Exception
             _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error building character set in PasswordGenerator")
             Return String.Empty ' Return empty set on error
        End Try

        Return allowedChars.ToString()
    End Function

    ''' <summary>
    ''' Generates a random password string using a cryptographically secure random number generator.
    ''' </summary>
    ''' <param name="characterSet">The string of allowed characters to use for generation.</param>
    ''' <param name="length">The desired length of the password.</param>
    ''' <returns>The generated password string, or an empty string if the character set is empty, length is invalid, or an error occurs.</returns>
    Public Function GeneratePassword(characterSet As String, length As Integer) As String
        If String.IsNullOrEmpty(characterSet) OrElse length <= 0 Then
            Return "" ' Cannot generate password with empty set or invalid length
        End If

        Dim passwordBuilder As New StringBuilder()
        Try
            Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
                Dim randomBytes(3) As Byte ' 4 bytes for a UInt32
                Dim rangeUpperBound As UInteger = CUInt(characterSet.Length) ' Exclusive upper bound for index
                ' Calculate maxMultiple outside the loop for efficiency
                Dim maxMultiple As UInteger = UInt32.MaxValue - (UInt32.MaxValue Mod rangeUpperBound) ' For unbiased sampling

                For i As Integer = 0 To (length - 1)
                    Dim randomIndex As Integer
                    Do
                        rng.GetBytes(randomBytes) ' Fill with random bytes
                        Dim randomUInt32 As UInt32 = BitConverter.ToUInt32(randomBytes, 0)

                        ' Rejection sampling to avoid bias: ensures uniform distribution
                        If randomUInt32 >= maxMultiple Then Continue Do ' Value is in the biased range, retry

                        ' Value is acceptable, map it to the desired range
                        randomIndex = CInt(randomUInt32 Mod rangeUpperBound)
                        Exit Do ' Found a suitable index
                    Loop

                    passwordBuilder.Append(characterSet(randomIndex)) ' Append character using 0-based index
                Next
            End Using ' Dispose the RNG
        Catch ex As Exception
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error during password generation in PasswordGenerator")
            Return String.Empty ' Return empty on error
        End Try

        Return passwordBuilder.ToString()
    End Function

End Class