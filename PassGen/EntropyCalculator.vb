Imports System.Text
Imports System.Collections.Generic ' Required for HashSet

Public Class EntropyCalculator

    Private _errorLogger As ErrorLogger

    ''' <summary>
    ''' Initializes a new instance of the EntropyCalculator class.
    ''' </summary>
    ''' <param name="logger">An instance of ErrorLogger for logging potential errors.</param>
    Public Sub New(logger As ErrorLogger)
        If logger Is Nothing Then Throw New ArgumentNullException("logger")
        _errorLogger = logger
    End Sub

    ''' <summary>
    ''' Counts the number of unique characters within a given password string.
    ''' </summary>
    ''' <param name="password">The password string to analyze.</param>
    ''' <returns>The count of unique characters, or 0 if the password is null/empty or an error occurs.</returns>
    Public Function CountUniqueCharacters(password As String) As Integer
        If String.IsNullOrEmpty(password) Then Return 0

        Dim uniqueCount As Integer = 0
        ' Use a HashSet for efficient tracking of unique characters
        Dim uniqueChars As New HashSet(Of Char)()

        Try
            For Each c As Char In password
                ' Add returns true if the character was not already present
                If uniqueChars.Add(c) Then
                    uniqueCount += 1
                End If
            Next
        Catch ex As Exception
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error counting unique characters in EntropyCalculator")
            Return 0 ' Return 0 on error
        End Try

        Return uniqueCount
    End Function

    ''' <summary>
    ''' Calculates the password entropy in bits.
    ''' Entropy = L * log2(N)
    ''' Where L is the password length and N is the number of unique characters in the pool (size of the character set used).
    ''' Note: The original implementation calculated entropy based on unique characters *in the generated password*, 
    ''' which is different from the standard definition based on the *pool* of possible characters.
    ''' This implementation retains the original logic for consistency, using uniqueCharacterCount from the generated password.
    ''' </summary>
    ''' <param name="passwordLength">The length of the password.</param>
    ''' <param name="uniqueCharacterCount">The number of unique characters found *within* the generated password.</param>
    ''' <returns>The calculated entropy in bits, or 0 if inputs are invalid or an error occurs.</returns>
    Public Function CalculateEntropy(passwordLength As Integer, uniqueCharacterCount As Integer) As Double
        If passwordLength <= 0 OrElse uniqueCharacterCount <= 0 Then
            Return 0.0
        End If

        Try
            ' Handle the edge case where uniqueCharacterCount is 1 (log2(1) = 0)
            If uniqueCharacterCount = 1 Then Return 0.0

            ' Calculate entropy using Log base 2: L * Log2(N) = L * (Log(N) / Log(2))
            Dim entropy As Double = CDbl(passwordLength) * Math.Log(CDbl(uniqueCharacterCount)) / Math.Log(2.0)
            Return entropy
        Catch ex As Exception
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error calculating entropy in EntropyCalculator")
            Return 0.0 ' Return 0 on error
        End Try
    End Function

End Class