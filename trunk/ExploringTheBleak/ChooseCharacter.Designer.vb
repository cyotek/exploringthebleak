<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChooseCharacter
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChooseCharacter))
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.BasicTab = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CharacterName = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Sex_Male = New System.Windows.Forms.RadioButton()
        Me.Sex_Female = New System.Windows.Forms.RadioButton()
        Me.RaceTab = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SecondaryStat = New System.Windows.Forms.Label()
        Me.PrimaryStat = New System.Windows.Forms.Label()
        Me.weight = New System.Windows.Forms.Label()
        Me.height = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.luc = New System.Windows.Forms.Label()
        Me.cha = New System.Windows.Forms.Label()
        Me.con = New System.Windows.Forms.Label()
        Me.wis = New System.Windows.Forms.Label()
        Me.int = New System.Windows.Forms.Label()
        Me.dex = New System.Windows.Forms.Label()
        Me.str = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.description = New System.Windows.Forms.RichTextBox()
        Me.Race_Sprite = New System.Windows.Forms.RadioButton()
        Me.Race_Pixie = New System.Windows.Forms.RadioButton()
        Me.Race_Quickling = New System.Windows.Forms.RadioButton()
        Me.Race_Halforc = New System.Windows.Forms.RadioButton()
        Me.Race_Orc = New System.Windows.Forms.RadioButton()
        Me.Race_Troll = New System.Windows.Forms.RadioButton()
        Me.Race_Goblin = New System.Windows.Forms.RadioButton()
        Me.Race_Halfling = New System.Windows.Forms.RadioButton()
        Me.Race_Halfelf = New System.Windows.Forms.RadioButton()
        Me.Race_Elf = New System.Windows.Forms.RadioButton()
        Me.Race_Gnome = New System.Windows.Forms.RadioButton()
        Me.Race_Dwarf = New System.Windows.Forms.RadioButton()
        Me.Race_Human = New System.Windows.Forms.RadioButton()
        Me.ClassTab = New System.Windows.Forms.TabPage()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Skill3Desc = New System.Windows.Forms.RichTextBox()
        Me.Skill2Desc = New System.Windows.Forms.RichTextBox()
        Me.Skill1Desc = New System.Windows.Forms.RichTextBox()
        Me.Skill3 = New System.Windows.Forms.PictureBox()
        Me.Skill2 = New System.Windows.Forms.PictureBox()
        Me.Skill1 = New System.Windows.Forms.PictureBox()
        Me.ClassDescription = New System.Windows.Forms.RichTextBox()
        Me.Minstrel = New System.Windows.Forms.RadioButton()
        Me.Monk = New System.Windows.Forms.RadioButton()
        Me.Runescribe = New System.Windows.Forms.RadioButton()
        Me.Scout = New System.Windows.Forms.RadioButton()
        Me.Page = New System.Windows.Forms.RadioButton()
        Me.Pickpocket = New System.Windows.Forms.RadioButton()
        Me.PLainsman = New System.Windows.Forms.RadioButton()
        Me.Headhunter = New System.Windows.Forms.RadioButton()
        Me.Elementalist = New System.Windows.Forms.RadioButton()
        Me.Hermit = New System.Windows.Forms.RadioButton()
        Me.Mageling = New System.Windows.Forms.RadioButton()
        Me.Gravedigger = New System.Windows.Forms.RadioButton()
        Me.Woodsman = New System.Windows.Forms.RadioButton()
        Me.Priest = New System.Windows.Forms.RadioButton()
        Me.HealthValue = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.WillpowerValue = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.BasicTab.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.RaceTab.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ClassTab.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.Skill3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Skill2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Skill1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(397, 24)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(52, 345)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Next"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.BasicTab)
        Me.TabControl1.Controls.Add(Me.RaceTab)
        Me.TabControl1.Controls.Add(Me.ClassTab)
        Me.TabControl1.Location = New System.Drawing.Point(2, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(386, 367)
        Me.TabControl1.TabIndex = 39
        '
        'BasicTab
        '
        Me.BasicTab.Controls.Add(Me.GroupBox2)
        Me.BasicTab.Controls.Add(Me.GroupBox1)
        Me.BasicTab.Location = New System.Drawing.Point(4, 22)
        Me.BasicTab.Name = "BasicTab"
        Me.BasicTab.Padding = New System.Windows.Forms.Padding(3)
        Me.BasicTab.Size = New System.Drawing.Size(378, 341)
        Me.BasicTab.TabIndex = 0
        Me.BasicTab.Text = "Basics"
        Me.BasicTab.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.CharacterName)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(366, 47)
        Me.GroupBox2.TabIndex = 19
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Name Your Character"
        '
        'CharacterName
        '
        Me.CharacterName.Location = New System.Drawing.Point(6, 19)
        Me.CharacterName.MaxLength = 19
        Me.CharacterName.Name = "CharacterName"
        Me.CharacterName.Size = New System.Drawing.Size(354, 20)
        Me.CharacterName.TabIndex = 16
        Me.CharacterName.Text = "Mykil Ironfist"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Sex_Male)
        Me.GroupBox1.Controls.Add(Me.Sex_Female)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 59)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(366, 62)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Choose Your Gender"
        '
        'Sex_Male
        '
        Me.Sex_Male.AutoSize = True
        Me.Sex_Male.Checked = True
        Me.Sex_Male.Location = New System.Drawing.Point(6, 19)
        Me.Sex_Male.Name = "Sex_Male"
        Me.Sex_Male.Size = New System.Drawing.Size(48, 17)
        Me.Sex_Male.TabIndex = 0
        Me.Sex_Male.TabStop = True
        Me.Sex_Male.Text = "Male"
        Me.Sex_Male.UseVisualStyleBackColor = True
        '
        'Sex_Female
        '
        Me.Sex_Female.AutoSize = True
        Me.Sex_Female.Location = New System.Drawing.Point(6, 42)
        Me.Sex_Female.Name = "Sex_Female"
        Me.Sex_Female.Size = New System.Drawing.Size(59, 17)
        Me.Sex_Female.TabIndex = 1
        Me.Sex_Female.Text = "Female"
        Me.Sex_Female.UseVisualStyleBackColor = True
        '
        'RaceTab
        '
        Me.RaceTab.Controls.Add(Me.Panel1)
        Me.RaceTab.Location = New System.Drawing.Point(4, 22)
        Me.RaceTab.Name = "RaceTab"
        Me.RaceTab.Padding = New System.Windows.Forms.Padding(3)
        Me.RaceTab.Size = New System.Drawing.Size(378, 341)
        Me.RaceTab.TabIndex = 1
        Me.RaceTab.Text = "Race"
        Me.RaceTab.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.SecondaryStat)
        Me.Panel1.Controls.Add(Me.PrimaryStat)
        Me.Panel1.Controls.Add(Me.weight)
        Me.Panel1.Controls.Add(Me.height)
        Me.Panel1.Controls.Add(Me.Label18)
        Me.Panel1.Controls.Add(Me.Label17)
        Me.Panel1.Controls.Add(Me.Label16)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.luc)
        Me.Panel1.Controls.Add(Me.cha)
        Me.Panel1.Controls.Add(Me.con)
        Me.Panel1.Controls.Add(Me.wis)
        Me.Panel1.Controls.Add(Me.int)
        Me.Panel1.Controls.Add(Me.dex)
        Me.Panel1.Controls.Add(Me.str)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.description)
        Me.Panel1.Controls.Add(Me.Race_Sprite)
        Me.Panel1.Controls.Add(Me.Race_Pixie)
        Me.Panel1.Controls.Add(Me.Race_Quickling)
        Me.Panel1.Controls.Add(Me.Race_Halforc)
        Me.Panel1.Controls.Add(Me.Race_Orc)
        Me.Panel1.Controls.Add(Me.Race_Troll)
        Me.Panel1.Controls.Add(Me.Race_Goblin)
        Me.Panel1.Controls.Add(Me.Race_Halfling)
        Me.Panel1.Controls.Add(Me.Race_Halfelf)
        Me.Panel1.Controls.Add(Me.Race_Elf)
        Me.Panel1.Controls.Add(Me.Race_Gnome)
        Me.Panel1.Controls.Add(Me.Race_Dwarf)
        Me.Panel1.Controls.Add(Me.Race_Human)
        Me.Panel1.Location = New System.Drawing.Point(6, 6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(367, 304)
        Me.Panel1.TabIndex = 40
        '
        'SecondaryStat
        '
        Me.SecondaryStat.AutoSize = True
        Me.SecondaryStat.Location = New System.Drawing.Point(303, 212)
        Me.SecondaryStat.Name = "SecondaryStat"
        Me.SecondaryStat.Size = New System.Drawing.Size(29, 13)
        Me.SecondaryStat.TabIndex = 35
        Me.SecondaryStat.Text = "DEX"
        '
        'PrimaryStat
        '
        Me.PrimaryStat.AutoSize = True
        Me.PrimaryStat.Location = New System.Drawing.Point(303, 189)
        Me.PrimaryStat.Name = "PrimaryStat"
        Me.PrimaryStat.Size = New System.Drawing.Size(29, 13)
        Me.PrimaryStat.TabIndex = 34
        Me.PrimaryStat.Text = "STR"
        '
        'weight
        '
        Me.weight.AutoSize = True
        Me.weight.Location = New System.Drawing.Point(303, 166)
        Me.weight.Name = "weight"
        Me.weight.Size = New System.Drawing.Size(56, 13)
        Me.weight.TabIndex = 33
        Me.weight.Text = "120' - 240'"
        '
        'height
        '
        Me.height.AutoSize = True
        Me.height.Location = New System.Drawing.Point(303, 145)
        Me.height.Name = "height"
        Me.height.Size = New System.Drawing.Size(32, 13)
        Me.height.TabIndex = 32
        Me.height.Text = "4' - 6'"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(216, 212)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(83, 13)
        Me.Label18.TabIndex = 31
        Me.Label18.Text = "Secondary Stat:"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(216, 189)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(66, 13)
        Me.Label17.TabIndex = 30
        Me.Label17.Text = "Primary Stat:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(216, 166)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(44, 13)
        Me.Label16.TabIndex = 29
        Me.Label16.Text = "Weight:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(216, 143)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(41, 13)
        Me.Label15.TabIndex = 28
        Me.Label15.Text = "Height:"
        '
        'luc
        '
        Me.luc.AutoSize = True
        Me.luc.Location = New System.Drawing.Point(155, 281)
        Me.luc.Name = "luc"
        Me.luc.Size = New System.Drawing.Size(19, 13)
        Me.luc.TabIndex = 27
        Me.luc.Text = "10"
        '
        'cha
        '
        Me.cha.AutoSize = True
        Me.cha.Location = New System.Drawing.Point(155, 260)
        Me.cha.Name = "cha"
        Me.cha.Size = New System.Drawing.Size(19, 13)
        Me.cha.TabIndex = 26
        Me.cha.Text = "10"
        '
        'con
        '
        Me.con.AutoSize = True
        Me.con.Location = New System.Drawing.Point(155, 237)
        Me.con.Name = "con"
        Me.con.Size = New System.Drawing.Size(19, 13)
        Me.con.TabIndex = 25
        Me.con.Text = "10"
        '
        'wis
        '
        Me.wis.AutoSize = True
        Me.wis.Location = New System.Drawing.Point(155, 214)
        Me.wis.Name = "wis"
        Me.wis.Size = New System.Drawing.Size(19, 13)
        Me.wis.TabIndex = 24
        Me.wis.Text = "10"
        '
        'int
        '
        Me.int.AutoSize = True
        Me.int.Location = New System.Drawing.Point(155, 191)
        Me.int.Name = "int"
        Me.int.Size = New System.Drawing.Size(19, 13)
        Me.int.TabIndex = 23
        Me.int.Text = "10"
        '
        'dex
        '
        Me.dex.AutoSize = True
        Me.dex.Location = New System.Drawing.Point(155, 166)
        Me.dex.Name = "dex"
        Me.dex.Size = New System.Drawing.Size(19, 13)
        Me.dex.TabIndex = 22
        Me.dex.Text = "10"
        '
        'str
        '
        Me.str.AutoSize = True
        Me.str.Location = New System.Drawing.Point(155, 143)
        Me.str.Name = "str"
        Me.str.Size = New System.Drawing.Size(19, 13)
        Me.str.TabIndex = 21
        Me.str.Text = "10"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(117, 281)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(31, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "LUC:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(117, 260)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 13)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "CHA:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(117, 237)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(33, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "CON:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(117, 214)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "WIS:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(117, 191)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(28, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "INT:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(117, 166)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "DEX:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(117, 143)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "STR:"
        '
        'description
        '
        Me.description.BackColor = System.Drawing.Color.White
        Me.description.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.description.Location = New System.Drawing.Point(110, 3)
        Me.description.Name = "description"
        Me.description.Size = New System.Drawing.Size(254, 132)
        Me.description.TabIndex = 13
        Me.description.Text = "Most of the continent of Sedia contains humans, the newest race, and are breeding" & _
            " out the rest. Humans can be any class as they are most versatile. "
        '
        'Race_Sprite
        '
        Me.Race_Sprite.AutoSize = True
        Me.Race_Sprite.Location = New System.Drawing.Point(3, 279)
        Me.Race_Sprite.Name = "Race_Sprite"
        Me.Race_Sprite.Size = New System.Drawing.Size(52, 17)
        Me.Race_Sprite.TabIndex = 12
        Me.Race_Sprite.Text = "Sprite"
        Me.Race_Sprite.UseVisualStyleBackColor = True
        '
        'Race_Pixie
        '
        Me.Race_Pixie.AutoSize = True
        Me.Race_Pixie.Location = New System.Drawing.Point(3, 256)
        Me.Race_Pixie.Name = "Race_Pixie"
        Me.Race_Pixie.Size = New System.Drawing.Size(47, 17)
        Me.Race_Pixie.TabIndex = 11
        Me.Race_Pixie.Text = "Pixie"
        Me.Race_Pixie.UseVisualStyleBackColor = True
        '
        'Race_Quickling
        '
        Me.Race_Quickling.AutoSize = True
        Me.Race_Quickling.Location = New System.Drawing.Point(3, 233)
        Me.Race_Quickling.Name = "Race_Quickling"
        Me.Race_Quickling.Size = New System.Drawing.Size(69, 17)
        Me.Race_Quickling.TabIndex = 10
        Me.Race_Quickling.Text = "Quickling"
        Me.Race_Quickling.UseVisualStyleBackColor = True
        '
        'Race_Halforc
        '
        Me.Race_Halforc.AutoSize = True
        Me.Race_Halforc.Checked = True
        Me.Race_Halforc.Location = New System.Drawing.Point(3, 210)
        Me.Race_Halforc.Name = "Race_Halforc"
        Me.Race_Halforc.Size = New System.Drawing.Size(62, 17)
        Me.Race_Halforc.TabIndex = 9
        Me.Race_Halforc.TabStop = True
        Me.Race_Halforc.Text = "Half-orc"
        Me.Race_Halforc.UseVisualStyleBackColor = True
        '
        'Race_Orc
        '
        Me.Race_Orc.AutoSize = True
        Me.Race_Orc.Location = New System.Drawing.Point(3, 187)
        Me.Race_Orc.Name = "Race_Orc"
        Me.Race_Orc.Size = New System.Drawing.Size(42, 17)
        Me.Race_Orc.TabIndex = 8
        Me.Race_Orc.Text = "Orc"
        Me.Race_Orc.UseVisualStyleBackColor = True
        '
        'Race_Troll
        '
        Me.Race_Troll.AutoSize = True
        Me.Race_Troll.Location = New System.Drawing.Point(3, 164)
        Me.Race_Troll.Name = "Race_Troll"
        Me.Race_Troll.Size = New System.Drawing.Size(45, 17)
        Me.Race_Troll.TabIndex = 7
        Me.Race_Troll.Text = "Troll"
        Me.Race_Troll.UseVisualStyleBackColor = True
        '
        'Race_Goblin
        '
        Me.Race_Goblin.AutoSize = True
        Me.Race_Goblin.Location = New System.Drawing.Point(3, 141)
        Me.Race_Goblin.Name = "Race_Goblin"
        Me.Race_Goblin.Size = New System.Drawing.Size(55, 17)
        Me.Race_Goblin.TabIndex = 6
        Me.Race_Goblin.Text = "Goblin"
        Me.Race_Goblin.UseVisualStyleBackColor = True
        '
        'Race_Halfling
        '
        Me.Race_Halfling.AutoSize = True
        Me.Race_Halfling.Location = New System.Drawing.Point(3, 118)
        Me.Race_Halfling.Name = "Race_Halfling"
        Me.Race_Halfling.Size = New System.Drawing.Size(60, 17)
        Me.Race_Halfling.TabIndex = 5
        Me.Race_Halfling.Text = "Halfling"
        Me.Race_Halfling.UseVisualStyleBackColor = True
        '
        'Race_Halfelf
        '
        Me.Race_Halfelf.AutoSize = True
        Me.Race_Halfelf.Location = New System.Drawing.Point(3, 95)
        Me.Race_Halfelf.Name = "Race_Halfelf"
        Me.Race_Halfelf.Size = New System.Drawing.Size(58, 17)
        Me.Race_Halfelf.TabIndex = 4
        Me.Race_Halfelf.Text = "Half-elf"
        Me.Race_Halfelf.UseVisualStyleBackColor = True
        '
        'Race_Elf
        '
        Me.Race_Elf.AutoSize = True
        Me.Race_Elf.Location = New System.Drawing.Point(3, 72)
        Me.Race_Elf.Name = "Race_Elf"
        Me.Race_Elf.Size = New System.Drawing.Size(37, 17)
        Me.Race_Elf.TabIndex = 3
        Me.Race_Elf.Text = "Elf"
        Me.Race_Elf.UseVisualStyleBackColor = True
        '
        'Race_Gnome
        '
        Me.Race_Gnome.AutoSize = True
        Me.Race_Gnome.Location = New System.Drawing.Point(3, 49)
        Me.Race_Gnome.Name = "Race_Gnome"
        Me.Race_Gnome.Size = New System.Drawing.Size(59, 17)
        Me.Race_Gnome.TabIndex = 2
        Me.Race_Gnome.Text = "Gnome"
        Me.Race_Gnome.UseVisualStyleBackColor = True
        '
        'Race_Dwarf
        '
        Me.Race_Dwarf.AutoSize = True
        Me.Race_Dwarf.Location = New System.Drawing.Point(3, 26)
        Me.Race_Dwarf.Name = "Race_Dwarf"
        Me.Race_Dwarf.Size = New System.Drawing.Size(53, 17)
        Me.Race_Dwarf.TabIndex = 1
        Me.Race_Dwarf.Text = "Dwarf"
        Me.Race_Dwarf.UseVisualStyleBackColor = True
        '
        'Race_Human
        '
        Me.Race_Human.AutoSize = True
        Me.Race_Human.Location = New System.Drawing.Point(3, 3)
        Me.Race_Human.Name = "Race_Human"
        Me.Race_Human.Size = New System.Drawing.Size(59, 17)
        Me.Race_Human.TabIndex = 0
        Me.Race_Human.Text = "Human"
        Me.Race_Human.UseVisualStyleBackColor = True
        '
        'ClassTab
        '
        Me.ClassTab.BackColor = System.Drawing.Color.White
        Me.ClassTab.Controls.Add(Me.Panel3)
        Me.ClassTab.Location = New System.Drawing.Point(4, 22)
        Me.ClassTab.Name = "ClassTab"
        Me.ClassTab.Size = New System.Drawing.Size(378, 341)
        Me.ClassTab.TabIndex = 2
        Me.ClassTab.Text = "Class"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Skill3Desc)
        Me.Panel3.Controls.Add(Me.Skill2Desc)
        Me.Panel3.Controls.Add(Me.Skill1Desc)
        Me.Panel3.Controls.Add(Me.Skill3)
        Me.Panel3.Controls.Add(Me.Skill2)
        Me.Panel3.Controls.Add(Me.Skill1)
        Me.Panel3.Controls.Add(Me.ClassDescription)
        Me.Panel3.Controls.Add(Me.Minstrel)
        Me.Panel3.Controls.Add(Me.Monk)
        Me.Panel3.Controls.Add(Me.Runescribe)
        Me.Panel3.Controls.Add(Me.Scout)
        Me.Panel3.Controls.Add(Me.Page)
        Me.Panel3.Controls.Add(Me.Pickpocket)
        Me.Panel3.Controls.Add(Me.PLainsman)
        Me.Panel3.Controls.Add(Me.Headhunter)
        Me.Panel3.Controls.Add(Me.Elementalist)
        Me.Panel3.Controls.Add(Me.Hermit)
        Me.Panel3.Controls.Add(Me.Mageling)
        Me.Panel3.Controls.Add(Me.Gravedigger)
        Me.Panel3.Controls.Add(Me.Woodsman)
        Me.Panel3.Controls.Add(Me.Priest)
        Me.Panel3.Location = New System.Drawing.Point(2, 8)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(374, 325)
        Me.Panel3.TabIndex = 2
        '
        'Skill3Desc
        '
        Me.Skill3Desc.BackColor = System.Drawing.Color.White
        Me.Skill3Desc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Skill3Desc.Location = New System.Drawing.Point(178, 275)
        Me.Skill3Desc.Name = "Skill3Desc"
        Me.Skill3Desc.Size = New System.Drawing.Size(192, 51)
        Me.Skill3Desc.TabIndex = 44
        Me.Skill3Desc.Text = resources.GetString("Skill3Desc.Text")
        '
        'Skill2Desc
        '
        Me.Skill2Desc.BackColor = System.Drawing.Color.White
        Me.Skill2Desc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Skill2Desc.Location = New System.Drawing.Point(178, 218)
        Me.Skill2Desc.Name = "Skill2Desc"
        Me.Skill2Desc.Size = New System.Drawing.Size(192, 51)
        Me.Skill2Desc.TabIndex = 43
        Me.Skill2Desc.Text = resources.GetString("Skill2Desc.Text")
        '
        'Skill1Desc
        '
        Me.Skill1Desc.BackColor = System.Drawing.Color.White
        Me.Skill1Desc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Skill1Desc.Location = New System.Drawing.Point(178, 161)
        Me.Skill1Desc.Name = "Skill1Desc"
        Me.Skill1Desc.Size = New System.Drawing.Size(192, 51)
        Me.Skill1Desc.TabIndex = 42
        Me.Skill1Desc.Text = resources.GetString("Skill1Desc.Text")
        '
        'Skill3
        '
        Me.Skill3.Image = Global.Exploring_The_Bleak.My.Resources.Resources.DoubleSlice
        Me.Skill3.Location = New System.Drawing.Point(120, 274)
        Me.Skill3.Name = "Skill3"
        Me.Skill3.Size = New System.Drawing.Size(52, 51)
        Me.Skill3.TabIndex = 41
        Me.Skill3.TabStop = False
        '
        'Skill2
        '
        Me.Skill2.Image = Global.Exploring_The_Bleak.My.Resources.Resources.DoubleSlice
        Me.Skill2.Location = New System.Drawing.Point(120, 218)
        Me.Skill2.Name = "Skill2"
        Me.Skill2.Size = New System.Drawing.Size(52, 51)
        Me.Skill2.TabIndex = 40
        Me.Skill2.TabStop = False
        '
        'Skill1
        '
        Me.Skill1.Image = Global.Exploring_The_Bleak.My.Resources.Resources.DoubleSlice
        Me.Skill1.Location = New System.Drawing.Point(120, 161)
        Me.Skill1.Name = "Skill1"
        Me.Skill1.Size = New System.Drawing.Size(52, 51)
        Me.Skill1.TabIndex = 39
        Me.Skill1.TabStop = False
        '
        'ClassDescription
        '
        Me.ClassDescription.BackColor = System.Drawing.Color.White
        Me.ClassDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ClassDescription.Location = New System.Drawing.Point(120, 5)
        Me.ClassDescription.Name = "ClassDescription"
        Me.ClassDescription.Size = New System.Drawing.Size(254, 153)
        Me.ClassDescription.TabIndex = 15
        Me.ClassDescription.Text = resources.GetString("ClassDescription.Text")
        '
        'Minstrel
        '
        Me.Minstrel.AutoSize = True
        Me.Minstrel.Location = New System.Drawing.Point(3, 302)
        Me.Minstrel.Name = "Minstrel"
        Me.Minstrel.Size = New System.Drawing.Size(61, 17)
        Me.Minstrel.TabIndex = 14
        Me.Minstrel.Text = "Minstrel"
        Me.Minstrel.UseVisualStyleBackColor = True
        '
        'Monk
        '
        Me.Monk.AutoSize = True
        Me.Monk.Location = New System.Drawing.Point(3, 279)
        Me.Monk.Name = "Monk"
        Me.Monk.Size = New System.Drawing.Size(52, 17)
        Me.Monk.TabIndex = 13
        Me.Monk.Text = "Monk"
        Me.Monk.UseVisualStyleBackColor = True
        '
        'Runescribe
        '
        Me.Runescribe.AutoSize = True
        Me.Runescribe.Location = New System.Drawing.Point(3, 254)
        Me.Runescribe.Name = "Runescribe"
        Me.Runescribe.Size = New System.Drawing.Size(79, 17)
        Me.Runescribe.TabIndex = 12
        Me.Runescribe.Text = "Runescribe"
        Me.Runescribe.UseVisualStyleBackColor = True
        '
        'Scout
        '
        Me.Scout.AutoSize = True
        Me.Scout.Location = New System.Drawing.Point(3, 233)
        Me.Scout.Name = "Scout"
        Me.Scout.Size = New System.Drawing.Size(53, 17)
        Me.Scout.TabIndex = 11
        Me.Scout.Text = "Scout"
        Me.Scout.UseVisualStyleBackColor = True
        '
        'Page
        '
        Me.Page.AutoSize = True
        Me.Page.Location = New System.Drawing.Point(3, 212)
        Me.Page.Name = "Page"
        Me.Page.Size = New System.Drawing.Size(50, 17)
        Me.Page.TabIndex = 10
        Me.Page.Text = "Page"
        Me.Page.UseVisualStyleBackColor = True
        '
        'Pickpocket
        '
        Me.Pickpocket.AutoSize = True
        Me.Pickpocket.Location = New System.Drawing.Point(3, 189)
        Me.Pickpocket.Name = "Pickpocket"
        Me.Pickpocket.Size = New System.Drawing.Size(79, 17)
        Me.Pickpocket.TabIndex = 9
        Me.Pickpocket.Text = "Pickpocket"
        Me.Pickpocket.UseVisualStyleBackColor = True
        '
        'PLainsman
        '
        Me.PLainsman.AutoSize = True
        Me.PLainsman.Location = New System.Drawing.Point(3, 166)
        Me.PLainsman.Name = "PLainsman"
        Me.PLainsman.Size = New System.Drawing.Size(73, 17)
        Me.PLainsman.TabIndex = 8
        Me.PLainsman.Text = "Plainsman"
        Me.PLainsman.UseVisualStyleBackColor = True
        '
        'Headhunter
        '
        Me.Headhunter.AutoSize = True
        Me.Headhunter.Location = New System.Drawing.Point(3, 141)
        Me.Headhunter.Name = "Headhunter"
        Me.Headhunter.Size = New System.Drawing.Size(81, 17)
        Me.Headhunter.TabIndex = 7
        Me.Headhunter.Text = "Headhunter"
        Me.Headhunter.UseVisualStyleBackColor = True
        '
        'Elementalist
        '
        Me.Elementalist.AutoSize = True
        Me.Elementalist.Location = New System.Drawing.Point(3, 118)
        Me.Elementalist.Name = "Elementalist"
        Me.Elementalist.Size = New System.Drawing.Size(81, 17)
        Me.Elementalist.TabIndex = 6
        Me.Elementalist.Text = "Elementalist"
        Me.Elementalist.UseVisualStyleBackColor = True
        '
        'Hermit
        '
        Me.Hermit.AutoSize = True
        Me.Hermit.Location = New System.Drawing.Point(3, 95)
        Me.Hermit.Name = "Hermit"
        Me.Hermit.Size = New System.Drawing.Size(55, 17)
        Me.Hermit.TabIndex = 5
        Me.Hermit.Text = "Hermit"
        Me.Hermit.UseVisualStyleBackColor = True
        '
        'Mageling
        '
        Me.Mageling.AutoSize = True
        Me.Mageling.Location = New System.Drawing.Point(3, 72)
        Me.Mageling.Name = "Mageling"
        Me.Mageling.Size = New System.Drawing.Size(68, 17)
        Me.Mageling.TabIndex = 4
        Me.Mageling.Text = "Mageling"
        Me.Mageling.UseVisualStyleBackColor = True
        '
        'Gravedigger
        '
        Me.Gravedigger.AutoSize = True
        Me.Gravedigger.Location = New System.Drawing.Point(3, 49)
        Me.Gravedigger.Name = "Gravedigger"
        Me.Gravedigger.Size = New System.Drawing.Size(83, 17)
        Me.Gravedigger.TabIndex = 3
        Me.Gravedigger.Text = "Gravedigger"
        Me.Gravedigger.UseVisualStyleBackColor = True
        '
        'Woodsman
        '
        Me.Woodsman.AutoSize = True
        Me.Woodsman.Checked = True
        Me.Woodsman.Location = New System.Drawing.Point(3, 26)
        Me.Woodsman.Name = "Woodsman"
        Me.Woodsman.Size = New System.Drawing.Size(79, 17)
        Me.Woodsman.TabIndex = 2
        Me.Woodsman.TabStop = True
        Me.Woodsman.Text = "Woodsman"
        Me.Woodsman.UseVisualStyleBackColor = True
        '
        'Priest
        '
        Me.Priest.AutoSize = True
        Me.Priest.Location = New System.Drawing.Point(3, 3)
        Me.Priest.Name = "Priest"
        Me.Priest.Size = New System.Drawing.Size(51, 17)
        Me.Priest.TabIndex = 1
        Me.Priest.Text = "Priest"
        Me.Priest.UseVisualStyleBackColor = True
        '
        'HealthValue
        '
        Me.HealthValue.AutoSize = True
        Me.HealthValue.ForeColor = System.Drawing.Color.Red
        Me.HealthValue.Location = New System.Drawing.Point(270, 6)
        Me.HealthValue.Name = "HealthValue"
        Me.HealthValue.Size = New System.Drawing.Size(19, 13)
        Me.HealthValue.TabIndex = 41
        Me.HealthValue.Text = "40"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(223, 6)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(41, 13)
        Me.Label8.TabIndex = 40
        Me.Label8.Text = "Health:"
        '
        'WillpowerValue
        '
        Me.WillpowerValue.AutoSize = True
        Me.WillpowerValue.ForeColor = System.Drawing.Color.Blue
        Me.WillpowerValue.Location = New System.Drawing.Point(369, 6)
        Me.WillpowerValue.Name = "WillpowerValue"
        Me.WillpowerValue.Size = New System.Drawing.Size(19, 13)
        Me.WillpowerValue.TabIndex = 45
        Me.WillpowerValue.Text = "60"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(309, 6)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(56, 13)
        Me.Label9.TabIndex = 44
        Me.Label9.Text = "Willpower:"
        '
        'ChooseCharacter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(453, 372)
        Me.ControlBox = False
        Me.Controls.Add(Me.WillpowerValue)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.HealthValue)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "ChooseCharacter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Make Your Character - Click ""Ready"" or hit the ""Enter"" key when done."
        Me.TabControl1.ResumeLayout(False)
        Me.BasicTab.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.RaceTab.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ClassTab.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.Skill3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Skill2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Skill1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents BasicTab As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents CharacterName As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Sex_Male As System.Windows.Forms.RadioButton
    Friend WithEvents Sex_Female As System.Windows.Forms.RadioButton
    Friend WithEvents RaceTab As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SecondaryStat As System.Windows.Forms.Label
    Friend WithEvents PrimaryStat As System.Windows.Forms.Label
    Friend WithEvents weight As System.Windows.Forms.Label
    Friend WithEvents height As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents luc As System.Windows.Forms.Label
    Friend WithEvents cha As System.Windows.Forms.Label
    Friend WithEvents con As System.Windows.Forms.Label
    Friend WithEvents wis As System.Windows.Forms.Label
    Friend WithEvents int As System.Windows.Forms.Label
    Friend WithEvents dex As System.Windows.Forms.Label
    Friend WithEvents str As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents description As System.Windows.Forms.RichTextBox
    Friend WithEvents Race_Sprite As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Pixie As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Quickling As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Halforc As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Orc As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Troll As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Goblin As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Halfling As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Halfelf As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Elf As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Gnome As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Dwarf As System.Windows.Forms.RadioButton
    Friend WithEvents Race_Human As System.Windows.Forms.RadioButton
    Friend WithEvents ClassTab As System.Windows.Forms.TabPage
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents WillpowerValue As System.Windows.Forms.Label
    Friend WithEvents Skill3Desc As System.Windows.Forms.RichTextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Skill2Desc As System.Windows.Forms.RichTextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Skill1Desc As System.Windows.Forms.RichTextBox
    Friend WithEvents HealthValue As System.Windows.Forms.Label
    Friend WithEvents Skill3 As System.Windows.Forms.PictureBox
    Friend WithEvents Skill2 As System.Windows.Forms.PictureBox
    Friend WithEvents Skill1 As System.Windows.Forms.PictureBox
    Friend WithEvents ClassDescription As System.Windows.Forms.RichTextBox
    Friend WithEvents Minstrel As System.Windows.Forms.RadioButton
    Friend WithEvents Monk As System.Windows.Forms.RadioButton
    Friend WithEvents Runescribe As System.Windows.Forms.RadioButton
    Friend WithEvents Scout As System.Windows.Forms.RadioButton
    Friend WithEvents Page As System.Windows.Forms.RadioButton
    Friend WithEvents Pickpocket As System.Windows.Forms.RadioButton
    Friend WithEvents PLainsman As System.Windows.Forms.RadioButton
    Friend WithEvents Headhunter As System.Windows.Forms.RadioButton
    Friend WithEvents Elementalist As System.Windows.Forms.RadioButton
    Friend WithEvents Hermit As System.Windows.Forms.RadioButton
    Friend WithEvents Mageling As System.Windows.Forms.RadioButton
    Friend WithEvents Gravedigger As System.Windows.Forms.RadioButton
    Friend WithEvents Woodsman As System.Windows.Forms.RadioButton
    Friend WithEvents Priest As System.Windows.Forms.RadioButton
End Class
