<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.DisplayText = New System.Windows.Forms.RichTextBox()
        Me.Skill3Name = New System.Windows.Forms.Label()
        Me.Skill2Name = New System.Windows.Forms.Label()
        Me.Skill1Name = New System.Windows.Forms.Label()
        Me.Skill3 = New System.Windows.Forms.PictureBox()
        Me.Skill2 = New System.Windows.Forms.PictureBox()
        Me.Skill1 = New System.Windows.Forms.PictureBox()
        Me.HelpInfo = New System.Windows.Forms.Label()
        Me.LevelUpPanel = New System.Windows.Forms.GroupBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.stradd = New System.Windows.Forms.Button()
        Me.strcur = New System.Windows.Forms.TextBox()
        Me.DoneBttn = New System.Windows.Forms.Button()
        Me.dexcur = New System.Windows.Forms.TextBox()
        Me.lucmax = New System.Windows.Forms.TextBox()
        Me.intcur = New System.Windows.Forms.TextBox()
        Me.chamax = New System.Windows.Forms.TextBox()
        Me.wiscur = New System.Windows.Forms.TextBox()
        Me.conmax = New System.Windows.Forms.TextBox()
        Me.concur = New System.Windows.Forms.TextBox()
        Me.wismax = New System.Windows.Forms.TextBox()
        Me.chacur = New System.Windows.Forms.TextBox()
        Me.intmax = New System.Windows.Forms.TextBox()
        Me.luccur = New System.Windows.Forms.TextBox()
        Me.dexmax = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.strmax = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.wpadd = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.hpadd = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.wpcur = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.hpcur = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lucadd = New System.Windows.Forms.Button()
        Me.CurPoints = New System.Windows.Forms.TextBox()
        Me.chaadd = New System.Windows.Forms.Button()
        Me.dexadd = New System.Windows.Forms.Button()
        Me.conadd = New System.Windows.Forms.Button()
        Me.intadd = New System.Windows.Forms.Button()
        Me.wisadd = New System.Windows.Forms.Button()
        Me.ScoresBox = New System.Windows.Forms.GroupBox()
        Me.Output = New System.Windows.Forms.RichTextBox()
        Me.CharStats = New System.Windows.Forms.GroupBox()
        Me.StatBox = New System.Windows.Forms.RichTextBox()
        Me.SkillInfoBox = New System.Windows.Forms.GroupBox()
        Me.SkillInfo = New System.Windows.Forms.RichTextBox()
        Me.Comment10 = New System.Windows.Forms.PictureBox()
        Me.Comment9 = New System.Windows.Forms.PictureBox()
        Me.Comment8 = New System.Windows.Forms.PictureBox()
        Me.Comment7 = New System.Windows.Forms.PictureBox()
        Me.Comment6 = New System.Windows.Forms.PictureBox()
        Me.Comment5 = New System.Windows.Forms.PictureBox()
        Me.Comment4 = New System.Windows.Forms.PictureBox()
        Me.Comment3 = New System.Windows.Forms.PictureBox()
        Me.Comment2 = New System.Windows.Forms.PictureBox()
        Me.Comment1 = New System.Windows.Forms.PictureBox()
        Me.Comment11 = New System.Windows.Forms.PictureBox()
        Me.HealthBar = New Exploring_The_Bleak.MyProgBar()
        Me.WillpowerBar = New Exploring_The_Bleak.MyProgBar()
        Me.Panel1.SuspendLayout()
        CType(Me.Skill3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Skill2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Skill1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LevelUpPanel.SuspendLayout()
        Me.ScoresBox.SuspendLayout()
        Me.CharStats.SuspendLayout()
        Me.SkillInfoBox.SuspendLayout()
        CType(Me.Comment10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Comment11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel1.Controls.Add(Me.DisplayText)
        Me.Panel1.Controls.Add(Me.Skill3Name)
        Me.Panel1.Controls.Add(Me.Skill2Name)
        Me.Panel1.Controls.Add(Me.Skill1Name)
        Me.Panel1.Controls.Add(Me.Skill3)
        Me.Panel1.Controls.Add(Me.Skill2)
        Me.Panel1.Controls.Add(Me.Skill1)
        Me.Panel1.Location = New System.Drawing.Point(892, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(207, 458)
        Me.Panel1.TabIndex = 14
        '
        'DisplayText
        '
        Me.DisplayText.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.DisplayText.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DisplayText.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DisplayText.ForeColor = System.Drawing.Color.White
        Me.DisplayText.Location = New System.Drawing.Point(7, 114)
        Me.DisplayText.Name = "DisplayText"
        Me.DisplayText.ReadOnly = True
        Me.DisplayText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.DisplayText.Size = New System.Drawing.Size(193, 337)
        Me.DisplayText.TabIndex = 96
        Me.DisplayText.TabStop = False
        Me.DisplayText.Text = ""
        '
        'Skill3Name
        '
        Me.Skill3Name.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Skill3Name.AutoSize = True
        Me.Skill3Name.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Skill3Name.Font = New System.Drawing.Font("Courier New", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Skill3Name.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Skill3Name.Location = New System.Drawing.Point(149, 99)
        Me.Skill3Name.Name = "Skill3Name"
        Me.Skill3Name.Size = New System.Drawing.Size(35, 12)
        Me.Skill3Name.TabIndex = 95
        Me.Skill3Name.Text = "Label1"
        Me.Skill3Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Skill2Name
        '
        Me.Skill2Name.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Skill2Name.AutoSize = True
        Me.Skill2Name.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Skill2Name.Font = New System.Drawing.Font("Courier New", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Skill2Name.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Skill2Name.Location = New System.Drawing.Point(85, 99)
        Me.Skill2Name.Name = "Skill2Name"
        Me.Skill2Name.Size = New System.Drawing.Size(35, 12)
        Me.Skill2Name.TabIndex = 94
        Me.Skill2Name.Text = "Label1"
        Me.Skill2Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Skill1Name
        '
        Me.Skill1Name.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Skill1Name.AutoSize = True
        Me.Skill1Name.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Skill1Name.Font = New System.Drawing.Font("Courier New", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Skill1Name.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Skill1Name.Location = New System.Drawing.Point(21, 99)
        Me.Skill1Name.Name = "Skill1Name"
        Me.Skill1Name.Size = New System.Drawing.Size(35, 12)
        Me.Skill1Name.TabIndex = 93
        Me.Skill1Name.Text = "Label1"
        Me.Skill1Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Skill3
        '
        Me.Skill3.Image = Global.Exploring_The_Bleak.My.Resources.Resources.DoubleSlice
        Me.Skill3.Location = New System.Drawing.Point(140, 47)
        Me.Skill3.Name = "Skill3"
        Me.Skill3.Size = New System.Drawing.Size(52, 51)
        Me.Skill3.TabIndex = 19
        Me.Skill3.TabStop = False
        '
        'Skill2
        '
        Me.Skill2.Image = Global.Exploring_The_Bleak.My.Resources.Resources.DoubleSlice
        Me.Skill2.Location = New System.Drawing.Point(76, 47)
        Me.Skill2.Name = "Skill2"
        Me.Skill2.Size = New System.Drawing.Size(52, 51)
        Me.Skill2.TabIndex = 18
        Me.Skill2.TabStop = False
        '
        'Skill1
        '
        Me.Skill1.Image = Global.Exploring_The_Bleak.My.Resources.Resources.DoubleSlice
        Me.Skill1.Location = New System.Drawing.Point(13, 47)
        Me.Skill1.Name = "Skill1"
        Me.Skill1.Size = New System.Drawing.Size(52, 51)
        Me.Skill1.TabIndex = 17
        Me.Skill1.TabStop = False
        '
        'HelpInfo
        '
        Me.HelpInfo.AutoSize = True
        Me.HelpInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.HelpInfo.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.HelpInfo.Location = New System.Drawing.Point(262, 32)
        Me.HelpInfo.Name = "HelpInfo"
        Me.HelpInfo.Size = New System.Drawing.Size(455, 364)
        Me.HelpInfo.TabIndex = 93
        Me.HelpInfo.Text = resources.GetString("HelpInfo.Text")
        Me.HelpInfo.Visible = False
        '
        'LevelUpPanel
        '
        Me.LevelUpPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LevelUpPanel.Controls.Add(Me.Label11)
        Me.LevelUpPanel.Controls.Add(Me.Label12)
        Me.LevelUpPanel.Controls.Add(Me.stradd)
        Me.LevelUpPanel.Controls.Add(Me.strcur)
        Me.LevelUpPanel.Controls.Add(Me.DoneBttn)
        Me.LevelUpPanel.Controls.Add(Me.dexcur)
        Me.LevelUpPanel.Controls.Add(Me.lucmax)
        Me.LevelUpPanel.Controls.Add(Me.intcur)
        Me.LevelUpPanel.Controls.Add(Me.chamax)
        Me.LevelUpPanel.Controls.Add(Me.wiscur)
        Me.LevelUpPanel.Controls.Add(Me.conmax)
        Me.LevelUpPanel.Controls.Add(Me.concur)
        Me.LevelUpPanel.Controls.Add(Me.wismax)
        Me.LevelUpPanel.Controls.Add(Me.chacur)
        Me.LevelUpPanel.Controls.Add(Me.intmax)
        Me.LevelUpPanel.Controls.Add(Me.luccur)
        Me.LevelUpPanel.Controls.Add(Me.dexmax)
        Me.LevelUpPanel.Controls.Add(Me.Label1)
        Me.LevelUpPanel.Controls.Add(Me.strmax)
        Me.LevelUpPanel.Controls.Add(Me.Label2)
        Me.LevelUpPanel.Controls.Add(Me.wpadd)
        Me.LevelUpPanel.Controls.Add(Me.Label3)
        Me.LevelUpPanel.Controls.Add(Me.hpadd)
        Me.LevelUpPanel.Controls.Add(Me.Label4)
        Me.LevelUpPanel.Controls.Add(Me.Label10)
        Me.LevelUpPanel.Controls.Add(Me.Label5)
        Me.LevelUpPanel.Controls.Add(Me.Label9)
        Me.LevelUpPanel.Controls.Add(Me.Label6)
        Me.LevelUpPanel.Controls.Add(Me.wpcur)
        Me.LevelUpPanel.Controls.Add(Me.Label7)
        Me.LevelUpPanel.Controls.Add(Me.hpcur)
        Me.LevelUpPanel.Controls.Add(Me.Label8)
        Me.LevelUpPanel.Controls.Add(Me.lucadd)
        Me.LevelUpPanel.Controls.Add(Me.CurPoints)
        Me.LevelUpPanel.Controls.Add(Me.chaadd)
        Me.LevelUpPanel.Controls.Add(Me.dexadd)
        Me.LevelUpPanel.Controls.Add(Me.conadd)
        Me.LevelUpPanel.Controls.Add(Me.intadd)
        Me.LevelUpPanel.Controls.Add(Me.wisadd)
        Me.LevelUpPanel.ForeColor = System.Drawing.Color.White
        Me.LevelUpPanel.Location = New System.Drawing.Point(403, 110)
        Me.LevelUpPanel.Name = "LevelUpPanel"
        Me.LevelUpPanel.Size = New System.Drawing.Size(283, 310)
        Me.LevelUpPanel.TabIndex = 94
        Me.LevelUpPanel.TabStop = False
        Me.LevelUpPanel.Text = "Level Up!"
        Me.LevelUpPanel.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(71, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(41, 13)
        Me.Label11.TabIndex = 39
        Me.Label11.Text = "Current"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(135, 16)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(51, 13)
        Me.Label12.TabIndex = 40
        Me.Label12.Text = "Maximum"
        '
        'stradd
        '
        Me.stradd.ForeColor = System.Drawing.Color.Black
        Me.stradd.Location = New System.Drawing.Point(203, 30)
        Me.stradd.Name = "stradd"
        Me.stradd.Size = New System.Drawing.Size(75, 23)
        Me.stradd.TabIndex = 0
        Me.stradd.Text = "+"
        Me.stradd.UseVisualStyleBackColor = True
        '
        'strcur
        '
        Me.strcur.Enabled = False
        Me.strcur.Location = New System.Drawing.Point(56, 32)
        Me.strcur.Name = "strcur"
        Me.strcur.Size = New System.Drawing.Size(66, 20)
        Me.strcur.TabIndex = 1
        '
        'DoneBttn
        '
        Me.DoneBttn.ForeColor = System.Drawing.Color.Black
        Me.DoneBttn.Location = New System.Drawing.Point(203, 282)
        Me.DoneBttn.Name = "DoneBttn"
        Me.DoneBttn.Size = New System.Drawing.Size(75, 23)
        Me.DoneBttn.TabIndex = 38
        Me.DoneBttn.Text = "Done"
        Me.DoneBttn.UseVisualStyleBackColor = True
        '
        'dexcur
        '
        Me.dexcur.Enabled = False
        Me.dexcur.Location = New System.Drawing.Point(56, 58)
        Me.dexcur.Name = "dexcur"
        Me.dexcur.Size = New System.Drawing.Size(66, 20)
        Me.dexcur.TabIndex = 2
        '
        'lucmax
        '
        Me.lucmax.Enabled = False
        Me.lucmax.Location = New System.Drawing.Point(128, 188)
        Me.lucmax.Name = "lucmax"
        Me.lucmax.Size = New System.Drawing.Size(66, 20)
        Me.lucmax.TabIndex = 35
        '
        'intcur
        '
        Me.intcur.Enabled = False
        Me.intcur.Location = New System.Drawing.Point(56, 84)
        Me.intcur.Name = "intcur"
        Me.intcur.Size = New System.Drawing.Size(66, 20)
        Me.intcur.TabIndex = 3
        '
        'chamax
        '
        Me.chamax.Enabled = False
        Me.chamax.Location = New System.Drawing.Point(128, 162)
        Me.chamax.Name = "chamax"
        Me.chamax.Size = New System.Drawing.Size(66, 20)
        Me.chamax.TabIndex = 34
        '
        'wiscur
        '
        Me.wiscur.Enabled = False
        Me.wiscur.Location = New System.Drawing.Point(56, 110)
        Me.wiscur.Name = "wiscur"
        Me.wiscur.Size = New System.Drawing.Size(66, 20)
        Me.wiscur.TabIndex = 4
        '
        'conmax
        '
        Me.conmax.Enabled = False
        Me.conmax.Location = New System.Drawing.Point(128, 136)
        Me.conmax.Name = "conmax"
        Me.conmax.Size = New System.Drawing.Size(66, 20)
        Me.conmax.TabIndex = 33
        '
        'concur
        '
        Me.concur.Enabled = False
        Me.concur.Location = New System.Drawing.Point(56, 136)
        Me.concur.Name = "concur"
        Me.concur.Size = New System.Drawing.Size(66, 20)
        Me.concur.TabIndex = 5
        '
        'wismax
        '
        Me.wismax.Enabled = False
        Me.wismax.Location = New System.Drawing.Point(128, 110)
        Me.wismax.Name = "wismax"
        Me.wismax.Size = New System.Drawing.Size(66, 20)
        Me.wismax.TabIndex = 32
        '
        'chacur
        '
        Me.chacur.Enabled = False
        Me.chacur.Location = New System.Drawing.Point(56, 162)
        Me.chacur.Name = "chacur"
        Me.chacur.Size = New System.Drawing.Size(66, 20)
        Me.chacur.TabIndex = 6
        '
        'intmax
        '
        Me.intmax.Enabled = False
        Me.intmax.Location = New System.Drawing.Point(128, 84)
        Me.intmax.Name = "intmax"
        Me.intmax.Size = New System.Drawing.Size(66, 20)
        Me.intmax.TabIndex = 31
        '
        'luccur
        '
        Me.luccur.Enabled = False
        Me.luccur.Location = New System.Drawing.Point(56, 188)
        Me.luccur.Name = "luccur"
        Me.luccur.Size = New System.Drawing.Size(66, 20)
        Me.luccur.TabIndex = 7
        '
        'dexmax
        '
        Me.dexmax.Enabled = False
        Me.dexmax.Location = New System.Drawing.Point(128, 58)
        Me.dexmax.Name = "dexmax"
        Me.dexmax.Size = New System.Drawing.Size(66, 20)
        Me.dexmax.TabIndex = 30
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 288)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Points Left: "
        '
        'strmax
        '
        Me.strmax.Enabled = False
        Me.strmax.Location = New System.Drawing.Point(128, 32)
        Me.strmax.Name = "strmax"
        Me.strmax.Size = New System.Drawing.Size(66, 20)
        Me.strmax.TabIndex = 29
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "STR: "
        '
        'wpadd
        '
        Me.wpadd.ForeColor = System.Drawing.Color.Black
        Me.wpadd.Location = New System.Drawing.Point(203, 238)
        Me.wpadd.Name = "wpadd"
        Me.wpadd.Size = New System.Drawing.Size(75, 23)
        Me.wpadd.TabIndex = 28
        Me.wpadd.Text = "+"
        Me.wpadd.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "DEX: "
        '
        'hpadd
        '
        Me.hpadd.ForeColor = System.Drawing.Color.Black
        Me.hpadd.Location = New System.Drawing.Point(203, 212)
        Me.hpadd.Name = "hpadd"
        Me.hpadd.Size = New System.Drawing.Size(75, 23)
        Me.hpadd.TabIndex = 27
        Me.hpadd.Text = "+"
        Me.hpadd.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 87)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "INT: "
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(15, 243)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(31, 13)
        Me.Label10.TabIndex = 26
        Me.Label10.Text = "WP: "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 113)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "WIS: "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(15, 217)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(28, 13)
        Me.Label9.TabIndex = 25
        Me.Label9.Text = "HP: "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(15, 139)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(36, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "CON: "
        '
        'wpcur
        '
        Me.wpcur.Enabled = False
        Me.wpcur.Location = New System.Drawing.Point(56, 240)
        Me.wpcur.Name = "wpcur"
        Me.wpcur.Size = New System.Drawing.Size(138, 20)
        Me.wpcur.TabIndex = 24
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 165)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "CHA: "
        '
        'hpcur
        '
        Me.hpcur.Enabled = False
        Me.hpcur.Location = New System.Drawing.Point(56, 214)
        Me.hpcur.Name = "hpcur"
        Me.hpcur.Size = New System.Drawing.Size(138, 20)
        Me.hpcur.TabIndex = 23
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(15, 191)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(34, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "LUC: "
        '
        'lucadd
        '
        Me.lucadd.ForeColor = System.Drawing.Color.Black
        Me.lucadd.Location = New System.Drawing.Point(203, 186)
        Me.lucadd.Name = "lucadd"
        Me.lucadd.Size = New System.Drawing.Size(75, 23)
        Me.lucadd.TabIndex = 22
        Me.lucadd.Text = "+"
        Me.lucadd.UseVisualStyleBackColor = True
        '
        'CurPoints
        '
        Me.CurPoints.Enabled = False
        Me.CurPoints.Location = New System.Drawing.Point(80, 284)
        Me.CurPoints.Name = "CurPoints"
        Me.CurPoints.Size = New System.Drawing.Size(114, 20)
        Me.CurPoints.TabIndex = 16
        '
        'chaadd
        '
        Me.chaadd.ForeColor = System.Drawing.Color.Black
        Me.chaadd.Location = New System.Drawing.Point(203, 160)
        Me.chaadd.Name = "chaadd"
        Me.chaadd.Size = New System.Drawing.Size(75, 23)
        Me.chaadd.TabIndex = 21
        Me.chaadd.Text = "+"
        Me.chaadd.UseVisualStyleBackColor = True
        '
        'dexadd
        '
        Me.dexadd.ForeColor = System.Drawing.Color.Black
        Me.dexadd.Location = New System.Drawing.Point(203, 56)
        Me.dexadd.Name = "dexadd"
        Me.dexadd.Size = New System.Drawing.Size(75, 23)
        Me.dexadd.TabIndex = 17
        Me.dexadd.Text = "+"
        Me.dexadd.UseVisualStyleBackColor = True
        '
        'conadd
        '
        Me.conadd.ForeColor = System.Drawing.Color.Black
        Me.conadd.Location = New System.Drawing.Point(203, 134)
        Me.conadd.Name = "conadd"
        Me.conadd.Size = New System.Drawing.Size(75, 23)
        Me.conadd.TabIndex = 20
        Me.conadd.Text = "+"
        Me.conadd.UseVisualStyleBackColor = True
        '
        'intadd
        '
        Me.intadd.ForeColor = System.Drawing.Color.Black
        Me.intadd.Location = New System.Drawing.Point(203, 82)
        Me.intadd.Name = "intadd"
        Me.intadd.Size = New System.Drawing.Size(75, 23)
        Me.intadd.TabIndex = 18
        Me.intadd.Text = "+"
        Me.intadd.UseVisualStyleBackColor = True
        '
        'wisadd
        '
        Me.wisadd.ForeColor = System.Drawing.Color.Black
        Me.wisadd.Location = New System.Drawing.Point(203, 108)
        Me.wisadd.Name = "wisadd"
        Me.wisadd.Size = New System.Drawing.Size(75, 23)
        Me.wisadd.TabIndex = 19
        Me.wisadd.Text = "+"
        Me.wisadd.UseVisualStyleBackColor = True
        '
        'ScoresBox
        '
        Me.ScoresBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ScoresBox.BackgroundImage = CType(resources.GetObject("ScoresBox.BackgroundImage"), System.Drawing.Image)
        Me.ScoresBox.Controls.Add(Me.Output)
        Me.ScoresBox.ForeColor = System.Drawing.Color.White
        Me.ScoresBox.Location = New System.Drawing.Point(68, 58)
        Me.ScoresBox.Name = "ScoresBox"
        Me.ScoresBox.Size = New System.Drawing.Size(772, 520)
        Me.ScoresBox.TabIndex = 95
        Me.ScoresBox.TabStop = False
        Me.ScoresBox.Text = "Top Scores - Characters are saved to this list when they die."
        Me.ScoresBox.Visible = False
        '
        'Output
        '
        Me.Output.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Output.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Output.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Output.Location = New System.Drawing.Point(7, 19)
        Me.Output.Name = "Output"
        Me.Output.ReadOnly = True
        Me.Output.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.Output.Size = New System.Drawing.Size(759, 495)
        Me.Output.TabIndex = 0
        Me.Output.Text = resources.GetString("Output.Text")
        Me.Output.WordWrap = False
        '
        'CharStats
        '
        Me.CharStats.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.CharStats.Controls.Add(Me.StatBox)
        Me.CharStats.ForeColor = System.Drawing.Color.White
        Me.CharStats.Location = New System.Drawing.Point(253, 36)
        Me.CharStats.Name = "CharStats"
        Me.CharStats.Size = New System.Drawing.Size(193, 291)
        Me.CharStats.TabIndex = 96
        Me.CharStats.TabStop = False
        Me.CharStats.Text = "Character Statistics"
        Me.CharStats.Visible = False
        '
        'StatBox
        '
        Me.StatBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.StatBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.StatBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.StatBox.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StatBox.Location = New System.Drawing.Point(3, 16)
        Me.StatBox.Name = "StatBox"
        Me.StatBox.ReadOnly = True
        Me.StatBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.StatBox.Size = New System.Drawing.Size(180, 272)
        Me.StatBox.TabIndex = 0
        Me.StatBox.Text = "test"
        Me.StatBox.WordWrap = False
        '
        'SkillInfoBox
        '
        Me.SkillInfoBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.SkillInfoBox.Controls.Add(Me.SkillInfo)
        Me.SkillInfoBox.ForeColor = System.Drawing.Color.White
        Me.SkillInfoBox.Location = New System.Drawing.Point(846, 140)
        Me.SkillInfoBox.Name = "SkillInfoBox"
        Me.SkillInfoBox.Size = New System.Drawing.Size(240, 172)
        Me.SkillInfoBox.TabIndex = 97
        Me.SkillInfoBox.TabStop = False
        Me.SkillInfoBox.Text = "Character Statistics"
        Me.SkillInfoBox.Visible = False
        '
        'SkillInfo
        '
        Me.SkillInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.SkillInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SkillInfo.Dock = System.Windows.Forms.DockStyle.Left
        Me.SkillInfo.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SkillInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.SkillInfo.Location = New System.Drawing.Point(3, 16)
        Me.SkillInfo.Name = "SkillInfo"
        Me.SkillInfo.ReadOnly = True
        Me.SkillInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.SkillInfo.Size = New System.Drawing.Size(227, 153)
        Me.SkillInfo.TabIndex = 0
        Me.SkillInfo.Text = "test"
        '
        'Comment10
        '
        Me.Comment10.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox10_01
        Me.Comment10.Location = New System.Drawing.Point(142, 672)
        Me.Comment10.Name = "Comment10"
        Me.Comment10.Size = New System.Drawing.Size(544, 168)
        Me.Comment10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment10.TabIndex = 92
        Me.Comment10.TabStop = False
        Me.Comment10.Visible = False
        '
        'Comment9
        '
        Me.Comment9.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox9_01
        Me.Comment9.Location = New System.Drawing.Point(142, 672)
        Me.Comment9.Name = "Comment9"
        Me.Comment9.Size = New System.Drawing.Size(544, 168)
        Me.Comment9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment9.TabIndex = 91
        Me.Comment9.TabStop = False
        Me.Comment9.Visible = False
        '
        'Comment8
        '
        Me.Comment8.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox8_01
        Me.Comment8.Location = New System.Drawing.Point(142, 672)
        Me.Comment8.Name = "Comment8"
        Me.Comment8.Size = New System.Drawing.Size(544, 168)
        Me.Comment8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment8.TabIndex = 90
        Me.Comment8.TabStop = False
        Me.Comment8.Visible = False
        '
        'Comment7
        '
        Me.Comment7.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox7_01
        Me.Comment7.Location = New System.Drawing.Point(142, 672)
        Me.Comment7.Name = "Comment7"
        Me.Comment7.Size = New System.Drawing.Size(544, 168)
        Me.Comment7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment7.TabIndex = 89
        Me.Comment7.TabStop = False
        Me.Comment7.Visible = False
        '
        'Comment6
        '
        Me.Comment6.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox6_01
        Me.Comment6.Location = New System.Drawing.Point(142, 672)
        Me.Comment6.Name = "Comment6"
        Me.Comment6.Size = New System.Drawing.Size(544, 168)
        Me.Comment6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment6.TabIndex = 88
        Me.Comment6.TabStop = False
        Me.Comment6.Visible = False
        '
        'Comment5
        '
        Me.Comment5.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox5_01
        Me.Comment5.Location = New System.Drawing.Point(142, 672)
        Me.Comment5.Name = "Comment5"
        Me.Comment5.Size = New System.Drawing.Size(544, 168)
        Me.Comment5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment5.TabIndex = 87
        Me.Comment5.TabStop = False
        Me.Comment5.Visible = False
        '
        'Comment4
        '
        Me.Comment4.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox4_01
        Me.Comment4.Location = New System.Drawing.Point(142, 672)
        Me.Comment4.Name = "Comment4"
        Me.Comment4.Size = New System.Drawing.Size(544, 168)
        Me.Comment4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment4.TabIndex = 86
        Me.Comment4.TabStop = False
        Me.Comment4.Visible = False
        '
        'Comment3
        '
        Me.Comment3.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox3_01
        Me.Comment3.Location = New System.Drawing.Point(142, 672)
        Me.Comment3.Name = "Comment3"
        Me.Comment3.Size = New System.Drawing.Size(544, 168)
        Me.Comment3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment3.TabIndex = 85
        Me.Comment3.TabStop = False
        Me.Comment3.Visible = False
        '
        'Comment2
        '
        Me.Comment2.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox2_01
        Me.Comment2.Location = New System.Drawing.Point(142, 672)
        Me.Comment2.Name = "Comment2"
        Me.Comment2.Size = New System.Drawing.Size(544, 168)
        Me.Comment2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment2.TabIndex = 84
        Me.Comment2.TabStop = False
        Me.Comment2.Visible = False
        '
        'Comment1
        '
        Me.Comment1.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox1_01
        Me.Comment1.Location = New System.Drawing.Point(142, 672)
        Me.Comment1.Name = "Comment1"
        Me.Comment1.Size = New System.Drawing.Size(544, 168)
        Me.Comment1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment1.TabIndex = 83
        Me.Comment1.TabStop = False
        Me.Comment1.Visible = False
        '
        'Comment11
        '
        Me.Comment11.Image = Global.Exploring_The_Bleak.My.Resources.Resources.CommentBox11_01
        Me.Comment11.Location = New System.Drawing.Point(142, 458)
        Me.Comment11.Name = "Comment11"
        Me.Comment11.Size = New System.Drawing.Size(544, 382)
        Me.Comment11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Comment11.TabIndex = 11
        Me.Comment11.TabStop = False
        Me.Comment11.Visible = False
        '
        'HealthBar
        '
        Me.HealthBar.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.HealthBar.Caption = "Health Bar"
        Me.HealthBar.Enabled = False
        Me.HealthBar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.HealthBar.Location = New System.Drawing.Point(899, 18)
        Me.HealthBar.Max = 100
        Me.HealthBar.Min = 0
        Me.HealthBar.Name = "HealthBar"
        Me.HealthBar.Size = New System.Drawing.Size(193, 15)
        Me.HealthBar.TabIndex = 12
        Me.HealthBar.Value = 70
        '
        'WillpowerBar
        '
        Me.WillpowerBar.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.WillpowerBar.Caption = "Mana Bar"
        Me.WillpowerBar.Enabled = False
        Me.WillpowerBar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.WillpowerBar.Location = New System.Drawing.Point(899, 36)
        Me.WillpowerBar.Max = 100
        Me.WillpowerBar.Min = 0
        Me.WillpowerBar.Name = "WillpowerBar"
        Me.WillpowerBar.Size = New System.Drawing.Size(193, 16)
        Me.WillpowerBar.TabIndex = 13
        Me.WillpowerBar.Value = 30
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(1111, 852)
        Me.Controls.Add(Me.SkillInfoBox)
        Me.Controls.Add(Me.CharStats)
        Me.Controls.Add(Me.ScoresBox)
        Me.Controls.Add(Me.LevelUpPanel)
        Me.Controls.Add(Me.HelpInfo)
        Me.Controls.Add(Me.Comment10)
        Me.Controls.Add(Me.Comment9)
        Me.Controls.Add(Me.Comment8)
        Me.Controls.Add(Me.Comment7)
        Me.Controls.Add(Me.HealthBar)
        Me.Controls.Add(Me.WillpowerBar)
        Me.Controls.Add(Me.Comment6)
        Me.Controls.Add(Me.Comment5)
        Me.Controls.Add(Me.Comment4)
        Me.Controls.Add(Me.Comment3)
        Me.Controls.Add(Me.Comment2)
        Me.Controls.Add(Me.Comment1)
        Me.Controls.Add(Me.Comment11)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Exploring The Bleak"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.Skill3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Skill2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Skill1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LevelUpPanel.ResumeLayout(False)
        Me.LevelUpPanel.PerformLayout()
        Me.ScoresBox.ResumeLayout(False)
        Me.CharStats.ResumeLayout(False)
        Me.SkillInfoBox.ResumeLayout(False)
        CType(Me.Comment10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Comment11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Comment11 As System.Windows.Forms.PictureBox
    Friend WithEvents HealthBar As Exploring_The_Bleak.MyProgBar
    Friend WithEvents WillpowerBar As Exploring_The_Bleak.MyProgBar
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Comment10 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment9 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment8 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment7 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment6 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment5 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment4 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment3 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment2 As System.Windows.Forms.PictureBox
    Friend WithEvents Comment1 As System.Windows.Forms.PictureBox
    Friend WithEvents Skill3 As System.Windows.Forms.PictureBox
    Friend WithEvents Skill2 As System.Windows.Forms.PictureBox
    Friend WithEvents Skill1 As System.Windows.Forms.PictureBox
    Friend WithEvents Skill3Name As System.Windows.Forms.Label
    Friend WithEvents Skill2Name As System.Windows.Forms.Label
    Friend WithEvents Skill1Name As System.Windows.Forms.Label
    Friend WithEvents HelpInfo As System.Windows.Forms.Label
    Friend WithEvents LevelUpPanel As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents stradd As System.Windows.Forms.Button
    Friend WithEvents strcur As System.Windows.Forms.TextBox
    Friend WithEvents DoneBttn As System.Windows.Forms.Button
    Friend WithEvents dexcur As System.Windows.Forms.TextBox
    Friend WithEvents lucmax As System.Windows.Forms.TextBox
    Friend WithEvents intcur As System.Windows.Forms.TextBox
    Friend WithEvents chamax As System.Windows.Forms.TextBox
    Friend WithEvents wiscur As System.Windows.Forms.TextBox
    Friend WithEvents conmax As System.Windows.Forms.TextBox
    Friend WithEvents concur As System.Windows.Forms.TextBox
    Friend WithEvents wismax As System.Windows.Forms.TextBox
    Friend WithEvents chacur As System.Windows.Forms.TextBox
    Friend WithEvents intmax As System.Windows.Forms.TextBox
    Friend WithEvents luccur As System.Windows.Forms.TextBox
    Friend WithEvents dexmax As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents strmax As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents wpadd As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents hpadd As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents wpcur As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents hpcur As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lucadd As System.Windows.Forms.Button
    Friend WithEvents CurPoints As System.Windows.Forms.TextBox
    Friend WithEvents chaadd As System.Windows.Forms.Button
    Friend WithEvents dexadd As System.Windows.Forms.Button
    Friend WithEvents conadd As System.Windows.Forms.Button
    Friend WithEvents intadd As System.Windows.Forms.Button
    Friend WithEvents wisadd As System.Windows.Forms.Button
    Friend WithEvents ScoresBox As System.Windows.Forms.GroupBox
    Friend WithEvents Output As System.Windows.Forms.RichTextBox
    Friend WithEvents CharStats As System.Windows.Forms.GroupBox
    Friend WithEvents StatBox As System.Windows.Forms.RichTextBox
    Friend WithEvents DisplayText As System.Windows.Forms.RichTextBox
    Friend WithEvents SkillInfoBox As System.Windows.Forms.GroupBox
    Friend WithEvents SkillInfo As System.Windows.Forms.RichTextBox

End Class
