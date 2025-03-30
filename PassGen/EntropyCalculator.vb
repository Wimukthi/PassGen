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
    ''' Calculates the password entropy in bits using the standard formula.
    ''' Entropy = L * log2(N)
    ''' Where L is the password length and N is the size of the character set pool used for generation.
    ''' </summary>
    ''' <param name="passwordLength">The length of the password (L).</param>
    ''' <param name="characterSetSize">The total number of possible characters in the pool (N).</param>
    ''' <returns>The calculated entropy in bits, or 0.0 if inputs are invalid or an error occurs.</returns>
    Public Function CalculateEntropy(passwordLength As Integer, characterSetSize As Integer) As Double
        ' Validate inputs: Length and character set size must be positive.
        ' A character set size of 1 means log2(1) = 0 entropy, which is valid but results in 0.
        If passwordLength <= 0 OrElse characterSetSize <= 0 Then
            Return 0.0
        End If

        Try
            ' Handle the edge case where characterSetSize is 1 (log2(1) = 0)
            If characterSetSize = 1 Then Return 0.0

            ' Calculate entropy using Log base 2: L * Log2(N) = L * (Log(N) / Log(2))
            Dim entropy As Double = CDbl(passwordLength) * Math.Log(CDbl(characterSetSize)) / Math.Log(2.0)
            Return entropy
        Catch ex As Exception
            _errorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error calculating entropy in EntropyCalculator")
            Return 0.0 ' Return 0.0 on error
        End Try
    End Function

End Class