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

Namespace PassGen
    Friend NotInheritable Class Program
        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        Private Sub New()
        End Sub
        <STAThread()>
        Shared Sub Main()
            'Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New frmMain())
        End Sub
    End Class
End Namespace

