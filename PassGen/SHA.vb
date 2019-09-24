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

Imports System.Security.Cryptography
Imports System.Text

Public Class SHA
    Public Shared Function GenerateHash(ByVal plainText As String, ByVal hashAlgorithm As String, ByVal saltBytes() As Byte) As String
        Try
            ' If salt is not specified, generate it on the fly.
            If (saltBytes Is Nothing) Then

                ' Define min and max salt sizes.
                Dim minSaltSize As Integer
                Dim maxSaltSize As Integer

                minSaltSize = 4
                maxSaltSize = 8

                ' Generate a random number for the size of the salt.
                Dim random As Random
                random = New Random()

                Dim saltSize As Integer
                saltSize = random.Next(minSaltSize, maxSaltSize)

                ' Allocate a byte array, which will hold the salt.
                saltBytes = New Byte(saltSize - 1) {}

                ' Initialize a random number generator.
                Dim rng As RNGCryptoServiceProvider
                rng = New RNGCryptoServiceProvider()

                ' Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes)
            End If

            ' Convert plain text into a byte array.
            Dim plainTextBytes As Byte()
            plainTextBytes = Encoding.UTF8.GetBytes(plainText)

            ' Allocate array, which will hold plain text and salt.
            Dim plainTextWithSaltBytes() As Byte = _
                New Byte(plainTextBytes.Length + saltBytes.Length - 1) {}

            ' Copy plain text bytes into resulting array.
            Dim I As Integer
            For I = 0 To plainTextBytes.Length - 1
                plainTextWithSaltBytes(I) = plainTextBytes(I)
            Next I

            ' Append salt bytes to the resulting array.
            For I = 0 To saltBytes.Length - 1
                plainTextWithSaltBytes(plainTextBytes.Length + I) = saltBytes(I)
            Next I

            ' Because we support multiple hashing algorithms, we must define
            ' hash object as a common (abstract) base class. We will specify the
            ' actual hashing algorithm class later during object creation.
            Dim hash As HashAlgorithm

            ' Make sure hashing algorithm name is specified.
            If (hashAlgorithm Is Nothing) Then
                hashAlgorithm = ""
            End If

            ' Initialize appropriate hashing algorithm class.
            Select Case hashAlgorithm.ToUpper()

                Case "SHA1"
                    hash = New SHA1Managed()

                Case "SHA256"
                    hash = New SHA256Managed()

                Case "SHA384"
                    hash = New SHA384Managed()

                Case "SHA512"
                    hash = New SHA512Managed()

                Case Else
                    hash = New MD5CryptoServiceProvider()

            End Select

            ' Compute hash value of our plain text with appended salt.
            Dim hashBytes As Byte()
            hashBytes = hash.ComputeHash(plainTextWithSaltBytes)

            ' Create array which will hold hash and original salt bytes.
            Dim hashWithSaltBytes() As Byte = _
                                       New Byte(hashBytes.Length + _
                                                saltBytes.Length - 1) {}

            ' Copy hash bytes into resulting array.
            For I = 0 To hashBytes.Length - 1
                hashWithSaltBytes(I) = hashBytes(I)
            Next I

            ' Append salt bytes to the result.
            For I = 0 To saltBytes.Length - 1
                hashWithSaltBytes(hashBytes.Length + I) = saltBytes(I)
            Next I

            ' Convert result into a base64-encoded string.
            Dim hashValue As String
            hashValue = Convert.ToBase64String(hashWithSaltBytes)

            ' Return the result.
            GenerateHash = hashValue
        Catch ex As Exception
            MsgBox("Error", ex.Message)
            Return ""
        End Try
    End Function
End Class
