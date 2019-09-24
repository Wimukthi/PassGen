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
Module PasswordStrength
    Public Function PasswordEntropy(passwordLength As String, Randomness As String) As String
        Try


            Dim Entropy As New mcCalc
            Return Entropy.evaluate(passwordLength.ToString & "*log(" & Randomness.ToString & ")/log(2)")
        Catch ex As Exception
            Dim el As New ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, My.Application.Info.AssemblyName.ToString & " Encountered an Error")
            Return ""
        End Try
    End Function
End Module
