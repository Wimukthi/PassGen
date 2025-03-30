Namespace PassGen
    Public Class Program ' Changed from Friend NotInheritable
        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        Private Sub New()
        End Sub
        <STAThread()>
        Public Shared Sub Main() ' Ensure Main is Public
            ' Application.EnableVisualStyles() ' Often enabled by default or via manifest/project settings now
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New FrmMain()) ' Use the correct class name FrmMain
        End Sub
    End Class
End Namespace
