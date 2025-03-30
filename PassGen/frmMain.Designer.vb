<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.txtoutput = New System.Windows.Forms.TextBox()
        Me.txtMaxLength = New System.Windows.Forms.NumericUpDown()
        Me.chkUpperCase = New System.Windows.Forms.CheckBox()
        Me.chkLowerCase = New System.Windows.Forms.CheckBox()
        Me.chkNumbers = New System.Windows.Forms.CheckBox()
        Me.chkSpecialCharacters = New System.Windows.Forms.CheckBox()
        Me.bwgen = New System.ComponentModel.BackgroundWorker()
        Me.progGen = New System.Windows.Forms.ProgressBar()
        Me.chkSpace = New System.Windows.Forms.CheckBox()
        Me.tooltips = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkCustomChars = New System.Windows.Forms.CheckBox()
        Me.txtCustomChars = New System.Windows.Forms.TextBox()
        Me.lblEntropy = New System.Windows.Forms.Label()
        Me.btnAbout = New System.Windows.Forms.Button()
        Me.btnCopySHA512 = New System.Windows.Forms.Button()
        Me.btnCopySHA256 = New System.Windows.Forms.Button()
        Me.btnCopyMD5 = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.txtPassAmount = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.progEntropy = New System.Windows.Forms.ProgressBar()
        Me.threadEntropy = New System.ComponentModel.BackgroundWorker()
        Me.txtMD5 = New System.Windows.Forms.Label()
        Me.txtSHA256 = New System.Windows.Forms.Label()
        Me.txtSHA512 = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lstvKeys = New System.Windows.Forms.ListView()
        Me.clmnNumber = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmnKey = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmnEntropy = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmnLength = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmMD5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmSHA256 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmSHA512 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.txtMaxLength, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPassAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtoutput
        '
        Me.txtoutput.BackColor = System.Drawing.Color.White
        Me.txtoutput.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtoutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtoutput.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtoutput.ForeColor = System.Drawing.Color.Black
        Me.txtoutput.Location = New System.Drawing.Point(0, 0)
        Me.txtoutput.Multiline = True
        Me.txtoutput.Name = "txtoutput"
        Me.txtoutput.ReadOnly = True
        Me.txtoutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtoutput.Size = New System.Drawing.Size(234, 268)
        Me.txtoutput.TabIndex = 0
        '
        'txtMaxLength
        '
        Me.txtMaxLength.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMaxLength.Location = New System.Drawing.Point(125, 441)
        Me.txtMaxLength.Maximum = New Decimal(New Integer() {20000, 0, 0, 0})
        Me.txtMaxLength.Name = "txtMaxLength"
        Me.txtMaxLength.Size = New System.Drawing.Size(91, 18)
        Me.txtMaxLength.TabIndex = 8
        Me.txtMaxLength.ThousandsSeparator = True
        Me.tooltips.SetToolTip(Me.txtMaxLength, "Set the Length of the password, you can use up to 20000" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "characters, however on s" &
        "lower computers this may cause" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the password generation to take some time.")
        Me.txtMaxLength.Value = New Decimal(New Integer() {35, 0, 0, 0})
        '
        'chkUpperCase
        '
        Me.chkUpperCase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkUpperCase.AutoSize = True
        Me.chkUpperCase.Checked = True
        Me.chkUpperCase.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUpperCase.Location = New System.Drawing.Point(7, 380)
        Me.chkUpperCase.Name = "chkUpperCase"
        Me.chkUpperCase.Size = New System.Drawing.Size(150, 15)
        Me.chkUpperCase.TabIndex = 1
        Me.chkUpperCase.Text = "Upper Case Letters"
        Me.tooltips.SetToolTip(Me.chkUpperCase, "Use Upper Case characters :" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        Me.chkUpperCase.UseVisualStyleBackColor = True
        '
        'chkLowerCase
        '
        Me.chkLowerCase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkLowerCase.AutoSize = True
        Me.chkLowerCase.Checked = True
        Me.chkLowerCase.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLowerCase.Location = New System.Drawing.Point(163, 380)
        Me.chkLowerCase.Name = "chkLowerCase"
        Me.chkLowerCase.Size = New System.Drawing.Size(150, 15)
        Me.chkLowerCase.TabIndex = 2
        Me.chkLowerCase.Text = "Lower Case Letters"
        Me.tooltips.SetToolTip(Me.chkLowerCase, "Use Lower Case characters :" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "abcdefghijklmnopqrstuvwxyz")
        Me.chkLowerCase.UseVisualStyleBackColor = True
        '
        'chkNumbers
        '
        Me.chkNumbers.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkNumbers.AutoSize = True
        Me.chkNumbers.Checked = True
        Me.chkNumbers.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNumbers.Location = New System.Drawing.Point(319, 380)
        Me.chkNumbers.Name = "chkNumbers"
        Me.chkNumbers.Size = New System.Drawing.Size(73, 15)
        Me.chkNumbers.TabIndex = 3
        Me.chkNumbers.Text = "Numbers"
        Me.tooltips.SetToolTip(Me.chkNumbers, "Use Numbers :" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "0123456789")
        Me.chkNumbers.UseVisualStyleBackColor = True
        '
        'chkSpecialCharacters
        '
        Me.chkSpecialCharacters.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSpecialCharacters.AutoSize = True
        Me.chkSpecialCharacters.Checked = True
        Me.chkSpecialCharacters.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSpecialCharacters.Location = New System.Drawing.Point(409, 380)
        Me.chkSpecialCharacters.Name = "chkSpecialCharacters"
        Me.chkSpecialCharacters.Size = New System.Drawing.Size(150, 15)
        Me.chkSpecialCharacters.TabIndex = 4
        Me.chkSpecialCharacters.Text = "Special Characters"
        Me.tooltips.SetToolTip(Me.chkSpecialCharacters, "Use Special Characters :" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "~`!@#$%^&*()_+=-{[}]|;:'<,>.?" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        Me.chkSpecialCharacters.UseVisualStyleBackColor = True
        '
        'bwgen
        '
        Me.bwgen.WorkerReportsProgress = True
        Me.bwgen.WorkerSupportsCancellation = True
        '
        'progGen
        '
        Me.progGen.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.progGen.Location = New System.Drawing.Point(0, 467)
        Me.progGen.Name = "progGen"
        Me.progGen.Size = New System.Drawing.Size(1084, 10)
        Me.progGen.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.progGen.TabIndex = 14
        Me.progGen.Visible = False
        '
        'chkSpace
        '
        Me.chkSpace.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSpace.AutoSize = True
        Me.chkSpace.Location = New System.Drawing.Point(565, 380)
        Me.chkSpace.Name = "chkSpace"
        Me.chkSpace.Size = New System.Drawing.Size(94, 15)
        Me.chkSpace.TabIndex = 5
        Me.chkSpace.Text = "Use Spaces"
        Me.tooltips.SetToolTip(Me.chkSpace, "Use Spaces When Generating the Password")
        Me.chkSpace.UseVisualStyleBackColor = True
        '
        'chkCustomChars
        '
        Me.chkCustomChars.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkCustomChars.AutoSize = True
        Me.chkCustomChars.Location = New System.Drawing.Point(7, 406)
        Me.chkCustomChars.Name = "chkCustomChars"
        Me.chkCustomChars.Size = New System.Drawing.Size(192, 15)
        Me.chkCustomChars.TabIndex = 6
        Me.chkCustomChars.Text = "Inject Custom Characters"
        Me.tooltips.SetToolTip(Me.chkCustomChars, "You can use the adjacent text field to inject any other character you like to the" &
        " password" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "for example you can use High ANSI characters like these : ĊøúüþĦĤĦŐŞå" &
        "æÊÈÆ" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        Me.chkCustomChars.UseVisualStyleBackColor = True
        '
        'txtCustomChars
        '
        Me.txtCustomChars.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCustomChars.Enabled = False
        Me.txtCustomChars.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCustomChars.Location = New System.Drawing.Point(205, 401)
        Me.txtCustomChars.Name = "txtCustomChars"
        Me.txtCustomChars.Size = New System.Drawing.Size(868, 20)
        Me.txtCustomChars.TabIndex = 7
        Me.txtCustomChars.Text = "ĊøúüþĦĤĦŐŞåæÊÈÆ"
        Me.tooltips.SetToolTip(Me.txtCustomChars, "Enter any Custom Characters here, Given is only a Example." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Use these with cautio" &
        "n, because most web sites and programs" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "wont accept High ANSI characters for pas" &
        "swords.")
        '
        'lblEntropy
        '
        Me.lblEntropy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEntropy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblEntropy.Location = New System.Drawing.Point(845, 279)
        Me.lblEntropy.Name = "lblEntropy"
        Me.lblEntropy.Size = New System.Drawing.Size(230, 17)
        Me.lblEntropy.TabIndex = 22
        Me.lblEntropy.Text = "Password Entropy : 0 bits"
        Me.tooltips.SetToolTip(Me.lblEntropy, resources.GetString("lblEntropy.ToolTip"))
        '
        'btnAbout
        '
        Me.btnAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbout.Image = Global.PassGen.My.Resources.Resources._1683_Lightbulb_16x16
        Me.btnAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAbout.Location = New System.Drawing.Point(946, 427)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(129, 40)
        Me.btnAbout.TabIndex = 33
        Me.btnAbout.Text = "About"
        Me.tooltips.SetToolTip(Me.btnAbout, "Save the Password and the HASH values to a text" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "file.")
        Me.btnAbout.UseVisualStyleBackColor = True
        '
        'btnCopySHA512
        '
        Me.btnCopySHA512.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopySHA512.Location = New System.Drawing.Point(991, 353)
        Me.btnCopySHA512.Name = "btnCopySHA512"
        Me.btnCopySHA512.Size = New System.Drawing.Size(84, 17)
        Me.btnCopySHA512.TabIndex = 32
        Me.btnCopySHA512.Text = "SHA512"
        Me.tooltips.SetToolTip(Me.btnCopySHA512, "Copy to Clipboard")
        Me.btnCopySHA512.UseVisualStyleBackColor = True
        '
        'btnCopySHA256
        '
        Me.btnCopySHA256.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopySHA256.Location = New System.Drawing.Point(991, 329)
        Me.btnCopySHA256.Name = "btnCopySHA256"
        Me.btnCopySHA256.Size = New System.Drawing.Size(84, 17)
        Me.btnCopySHA256.TabIndex = 31
        Me.btnCopySHA256.Text = "SHA256"
        Me.tooltips.SetToolTip(Me.btnCopySHA256, "Copy to Clipboard")
        Me.btnCopySHA256.UseVisualStyleBackColor = True
        '
        'btnCopyMD5
        '
        Me.btnCopyMD5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopyMD5.Location = New System.Drawing.Point(991, 307)
        Me.btnCopyMD5.Name = "btnCopyMD5"
        Me.btnCopyMD5.Size = New System.Drawing.Size(84, 17)
        Me.btnCopyMD5.TabIndex = 30
        Me.btnCopyMD5.Text = "MD5"
        Me.tooltips.SetToolTip(Me.btnCopyMD5, "Copy to Clipboard")
        Me.btnCopyMD5.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Image = Global.PassGen.My.Resources.Resources.script_key
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(618, 427)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(129, 40)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save"
        Me.tooltips.SetToolTip(Me.btnSave, "Save the Password and the HASH values to a text" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "file.")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnGenerate
        '
        Me.btnGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenerate.Image = Global.PassGen.My.Resources.Resources.EntityDataModel_property_with_key_16x16
        Me.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerate.Location = New System.Drawing.Point(483, 427)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(129, 40)
        Me.btnGenerate.TabIndex = 9
        Me.btnGenerate.Text = "Generate"
        Me.tooltips.SetToolTip(Me.btnGenerate, "Click to generate password.")
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'txtPassAmount
        '
        Me.txtPassAmount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtPassAmount.Location = New System.Drawing.Point(376, 440)
        Me.txtPassAmount.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.txtPassAmount.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.txtPassAmount.Name = "txtPassAmount"
        Me.txtPassAmount.Size = New System.Drawing.Size(91, 18)
        Me.txtPassAmount.TabIndex = 36
        Me.txtPassAmount.ThousandsSeparator = True
        Me.tooltips.SetToolTip(Me.txtPassAmount, "Set the Length of the password, you can use up to 20000" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "characters, however on s" &
        "lower computers this may cause" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the password generation to take some time.")
        Me.txtPassAmount.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Location = New System.Drawing.Point(7, 279)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(130, 17)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Password Strength"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.Location = New System.Drawing.Point(5, 443)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(114, 17)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "Password Length"
        '
        'progEntropy
        '
        Me.progEntropy.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.progEntropy.Location = New System.Drawing.Point(143, 279)
        Me.progEntropy.Maximum = 256
        Me.progEntropy.Name = "progEntropy"
        Me.progEntropy.Size = New System.Drawing.Size(695, 17)
        Me.progEntropy.TabIndex = 20
        '
        'threadEntropy
        '
        Me.threadEntropy.WorkerReportsProgress = True
        Me.threadEntropy.WorkerSupportsCancellation = True
        '
        'txtMD5
        '
        Me.txtMD5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMD5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtMD5.Location = New System.Drawing.Point(7, 307)
        Me.txtMD5.Name = "txtMD5"
        Me.txtMD5.Size = New System.Drawing.Size(978, 17)
        Me.txtMD5.TabIndex = 27
        '
        'txtSHA256
        '
        Me.txtSHA256.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSHA256.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtSHA256.Location = New System.Drawing.Point(7, 329)
        Me.txtSHA256.Name = "txtSHA256"
        Me.txtSHA256.Size = New System.Drawing.Size(978, 17)
        Me.txtSHA256.TabIndex = 28
        '
        'txtSHA512
        '
        Me.txtSHA512.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSHA512.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtSHA512.Location = New System.Drawing.Point(7, 353)
        Me.txtSHA512.Name = "txtSHA512"
        Me.txtSHA512.Size = New System.Drawing.Size(978, 17)
        Me.txtSHA512.TabIndex = 29
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Location = New System.Drawing.Point(7, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstvKeys)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtoutput)
        Me.SplitContainer1.Size = New System.Drawing.Size(1068, 270)
        Me.SplitContainer1.SplitterDistance = 828
        Me.SplitContainer1.TabIndex = 35
        '
        'lstvKeys
        '
        Me.lstvKeys.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstvKeys.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmnNumber, Me.clmnKey, Me.clmnEntropy, Me.clmnLength, Me.clmMD5, Me.clmSHA256, Me.clmSHA512})
        Me.lstvKeys.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstvKeys.FullRowSelect = True
        Me.lstvKeys.GridLines = True
        Me.lstvKeys.HideSelection = False
        Me.lstvKeys.Location = New System.Drawing.Point(0, 0)
        Me.lstvKeys.Name = "lstvKeys"
        Me.lstvKeys.Size = New System.Drawing.Size(826, 268)
        Me.lstvKeys.TabIndex = 0
        Me.lstvKeys.UseCompatibleStateImageBehavior = False
        Me.lstvKeys.View = System.Windows.Forms.View.Details
        '
        'clmnNumber
        '
        Me.clmnNumber.Text = "#"
        Me.clmnNumber.Width = 30
        '
        'clmnKey
        '
        Me.clmnKey.Text = "Password"
        Me.clmnKey.Width = 300
        '
        'clmnEntropy
        '
        Me.clmnEntropy.Text = "Password Entropy"
        Me.clmnEntropy.Width = 130
        '
        'clmnLength
        '
        Me.clmnLength.Text = "Password Length"
        Me.clmnLength.Width = 130
        '
        'clmMD5
        '
        Me.clmMD5.Text = "MD5"
        '
        'clmSHA256
        '
        Me.clmSHA256.Text = "SHA256"
        '
        'clmSHA512
        '
        Me.clmSHA512.Text = "SHA512"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.Location = New System.Drawing.Point(227, 443)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(147, 17)
        Me.Label5.TabIndex = 37
        Me.Label5.Text = "Number of Passwords"
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 11.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1084, 477)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtPassAmount)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnAbout)
        Me.Controls.Add(Me.btnCopySHA512)
        Me.Controls.Add(Me.btnCopySHA256)
        Me.Controls.Add(Me.btnCopyMD5)
        Me.Controls.Add(Me.txtSHA512)
        Me.Controls.Add(Me.txtSHA256)
        Me.Controls.Add(Me.txtMD5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblEntropy)
        Me.Controls.Add(Me.progEntropy)
        Me.Controls.Add(Me.txtCustomChars)
        Me.Controls.Add(Me.chkCustomChars)
        Me.Controls.Add(Me.chkSpace)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.chkSpecialCharacters)
        Me.Controls.Add(Me.chkNumbers)
        Me.Controls.Add(Me.chkLowerCase)
        Me.Controls.Add(Me.chkUpperCase)
        Me.Controls.Add(Me.txtMaxLength)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.progGen)
        Me.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(980, 468)
        Me.Name = "FrmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.txtMaxLength, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPassAmount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtoutput As System.Windows.Forms.TextBox
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents txtMaxLength As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkUpperCase As System.Windows.Forms.CheckBox
    Friend WithEvents chkLowerCase As System.Windows.Forms.CheckBox
    Friend WithEvents chkNumbers As System.Windows.Forms.CheckBox
    Friend WithEvents chkSpecialCharacters As System.Windows.Forms.CheckBox
    Friend WithEvents bwgen As System.ComponentModel.BackgroundWorker
    Friend WithEvents progGen As System.Windows.Forms.ProgressBar
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents chkSpace As System.Windows.Forms.CheckBox
    Friend WithEvents tooltips As System.Windows.Forms.ToolTip
    Friend WithEvents chkCustomChars As System.Windows.Forms.CheckBox
    Friend WithEvents txtCustomChars As System.Windows.Forms.TextBox
    Friend WithEvents progEntropy As System.Windows.Forms.ProgressBar
    Friend WithEvents lblEntropy As System.Windows.Forms.Label
    Friend WithEvents threadEntropy As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtMD5 As System.Windows.Forms.Label
    Friend WithEvents txtSHA256 As System.Windows.Forms.Label
    Friend WithEvents txtSHA512 As System.Windows.Forms.Label
    Friend WithEvents btnCopyMD5 As System.Windows.Forms.Button
    Friend WithEvents btnCopySHA256 As System.Windows.Forms.Button
    Friend WithEvents btnCopySHA512 As System.Windows.Forms.Button
    Friend WithEvents btnAbout As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Label5 As Label
    Friend WithEvents txtPassAmount As NumericUpDown
    Friend WithEvents lstvKeys As ListView
    Friend WithEvents clmnKey As ColumnHeader
    Friend WithEvents clmnEntropy As ColumnHeader
    Friend WithEvents clmnLength As ColumnHeader
    Friend WithEvents clmnNumber As ColumnHeader
    Friend WithEvents clmMD5 As ColumnHeader
    Friend WithEvents clmSHA256 As ColumnHeader
    Friend WithEvents clmSHA512 As ColumnHeader
End Class
