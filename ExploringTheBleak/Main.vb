Imports System.IO
Public Class MainForm
#Region "Constants"
    Const MapSize As Short = 50 'original 25

    Const North As Short = 1
    Const East As Short = 2
    Const South As Short = 3
    Const West As Short = 4

    'mobiles can't walk on anything above 3
    Const Wall As Short = 0
    Const Floor As Short = 1
    Const SpecialFloor As Integer = 2
    Const StairsDown As Short = 3
    Const Item As Short = 4
    Const StairsUp As Short = 5
    Const Water As Short = 6
    Const Lava As Short = 7
    Const Ice As Short = 8

    Const Head As Short = 1
    Const Chest As Short = 2
    Const Arms As Short = 3
    Const Hands As Short = 4
    Const Legs As Short = 5
    Const Feet As Short = 6
    Const Weapon As Short = 7
    Const Gold As Short = 8
    Const TheEverspark As Short = 50

    Const Hidden As Short = 0 'used for MapDrawStatus, prevents recursive drawing on something already visible
    Const NotHidden As Short = 1 'ditto
    Const Shadowed As Short = 2 'ditto again

    Const TotalEnvironmentTypes As Short = 10

    Const Dungeon = 0
    Const Ruins = 1
    Const Classic = 2
    Const Random = 3
#End Region
#Region "Form Enhancements"
    Public PAD As New Bitmap(1200, 1200)
    Public CANVAS As Graphics = Graphics.FromImage(PAD)
#End Region
#Region "Declarations and Dimensions"
    Public AdminVisible As Boolean = False 'admin mode, allows full map view without exploration. generally used to debug generation techniques

    Public StandardColor As Color 'used for color types in fog display
    Public GenerateType As Short = Random
    Public EnvironmentType As Short = 0
    Public MapLevel As Short = 0
    Public Map(MapSize, MapSize) As Short
    Public MapShown(MapSize, MapSize) As Boolean
    Public MapOccupied(MapSize, MapSize) As Short
    Public MapDrawStatus(MapSize, MapSize) As Short 'draw status is an attempt to reduce overhead by preventing redraws on something already visible
    Public MapDrawStatusPlus(MapSize, MapSize) As Short 'draw status plus is an attempt to replace the original with something b3tter
    Public MapBlur(MapSize, MapSize, 3) As Boolean
    Public FogMap(MapSize, MapSize) As Short

    Public SKillGobalCooldown As Short 'prevents skills for a amount of time
    Public SkillType As String 'references a skillname if it's going to be used on next attack
    Public BoneShield, MagicShield, CounterAttack, Immolate, Fury, Block, Silence As Short

    Public PlayerPosX, PlayerPosY, PlayerLastPosX, PlayerLastPosY As Short
    Public PlayerExperience As Integer = 0
    Public PlayerLevel As Short = 1
    Public PlayerName As String
    Public PlayerHidden As Short
    Public PlayerClass As String
    Public PlayerRace As String
    Public PlayerDefense As Short = 1
    Public PlayerAttack As Short = 1
    Public PlayerSTR, PlayerDEX, PlayerINT, PlayerWIS, PlayerCON, PlayerCHA, PlayerLUC As Short
    Public PlayerMaxSTR, PlayerMaxDEX, PlayerMaxINT, PlayerMaxWIS, PlayerMaxCON, PlayerMaxCHA, PlayerMaxLuc As Short
    Public PlayerEquipHead, PlayerEquipChest, PlayerEquipArms, PlayerEquipHands, PlayerEquipLegs, PlayerEquipFeet As Short
    Public PlayerEquipQHead, PlayerEquipQChest, PlayerEquipQArms, PlayerEquipQHands, PLayerEquipQLegs, PlayerEquipQFeet As Short
    Public PlayerEquipNHead, PlayerEquipNChest, PlayerEquipNArms, PlayerEquipNHands, PLayerEquipNLegs, PlayerEquipNFeet As String
    Public PlayerHitpoints, PlayerWillpower As Short
    Public PlayerCurHitpoints, PlayerCurWillpower As Short
    Public PlayerLevelPoints As Short
    Public PlayerTurns As Long
    Public PlayerGold As Short
    Public PlayerDead As Boolean = False

    Public HighScores As String

    Public PreviousAttack, PreviousDefense As Short

    Public MobilePosX(9), MobilePosY(9), MobilePrevX(9), MobilePrevY(9), MobileLastMove(9), MobileType(9) As Short
    Public MobileHealth(9), MobileFlee(9), MobileStun(9), MobileClumsiness(9) As Short

    Public ItemNum(9), ItemType(9), ItemOccupied(MapSize, MapSize) As Short
    Public ItemNameType(MapSize, MapSize), ItemShowType(MapSize, MapSize)
    Public ItemInventoryType(19) As Short
    Public ItemInventoryName(19) As String
    Public ItemInventoryQuality(19) As Short

    Public SNDLog(27) As String
    Public LogVisible As Boolean = True

    Public TheRoomWidth As Integer = 15 'original is 30
    Public TheRoomHeight As Integer = 15 'original is 30
    Public ColumnsSpace As Integer = 0
    Public RowSpace As Integer = 0

    Public CommentBoxOpen As Boolean = False

    Dim displayfont As New Font("Arial", 24)
    Dim displayfont2 As New Font("Arial", 12)
#End Region
#Region "Basic Functions"
    Function CapitalizeFirstLetter(ByVal TheString As String) As String
        TheString = UCase(Mid(TheString, 1, 1)) + Mid(TheString, 2, Len(TheString))
        Return TheString
    End Function
    Public Function AddSpace(ByVal thestring As String, ByVal length As Integer, Optional ByVal Type As Short = 0) As String
        Dim tmp As Integer                                'this function will add characters to the right side given the specified LENGTH that THESTRING needs to be. Optionally you can specify
        If length - Len(thestring) > 0 Then               'a TYPE where if none is specified it just adds spaces to the right, with 1 it adds 0's and any other number will add
            For tmp = Len(thestring) To length - 1 Step 1 'astrisks
                If Type = 0 Then
                    thestring += " "
                ElseIf Type = 1 Then
                    thestring += "0"
                Else
                    thestring += "*"
                End If
            Next
            Return thestring
        Else 'the length of THESTRING is longer than the specified length that the string needs to be, therefor no 'spaces' are added
            Return thestring
        End If
    End Function
    Public Function AddSpaceL(ByVal thestring As String, ByVal length As Integer, Optional ByVal type As Short = 0) As String
        Dim tmp As Integer                                    'this function will add characters to the left side given the specified LENGTH that
        If length - Len(thestring) > 0 Then                   'THESTRING needs to be. Optionally you can specify a TYPE where if none is specified
            For tmp = Len(thestring) To length - 1 Step 1     'it just adds spaces, if 1 it adds 0's, and any other number it adds astrisks. this
                If type = 0 Then                              'can be helpful if you need a 3 digit long number and a number is only 8, it therefor
                    thestring = " " + thestring               'returns '008' if called: AddSpaceL("8",3,1)
                ElseIf type = 1 Then
                    thestring = "0" + thestring
                Else
                    thestring = "*" + thestring
                End If
            Next
            Return thestring
        Else 'the length of THESTRING is longer than the specified length that the strings needs to be, therefor no 'spaces' are added
            Return thestring
        End If
    End Function
    Public Function FileExists(ByVal FileFullPath As String) As Boolean  'does file exist? returns as boolean, yes/no (0,1)
        Dim f As New IO.FileInfo(FileFullPath)
        Return f.Exists
    End Function
    Public Function GetFileContents(ByVal FullPath As String, Optional ByRef ErrInfo As String = "") As String 'gets specified filenames data
        Dim strContents As String                                                  'by FULLPATH and assigns it as a string. 
        Dim objReader As StreamReader
        If FileExists(FullPath) = True Then
            Try
                objReader = New StreamReader(FullPath)
                strContents = objReader.ReadToEnd()
                objReader.Close()
                objReader.Dispose()
                Return strContents
            Catch Ex As Exception
                ErrInfo = Ex.Message
                MsgBox(ErrInfo, MsgBoxStyle.Critical, "ERROR!")
                Return ""
            End Try
        Else
            MsgBox(FullPath + " doesn't exist.", MsgBoxStyle.Critical, "ERROR!")
            Return ""
        End If
    End Function
    Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "", Optional ByVal MakeFile As Boolean = False) As Boolean
        Dim bAns As Boolean = False                  'assigns strdata to FULLPATH as string and closes
        Dim trynum As Short = 1
        Dim objReader As StreamWriter
        If FileExists(FullPath) = True Or MakeFile = True Then
            While bAns = False
                Try
                    objReader = New StreamWriter(FullPath)
                    objReader.Write(strData)
                    objReader.Close()
                    bAns = True
                Catch Ex As Exception
                    ErrInfo = Ex.Message
                    MsgBox(ErrInfo, MsgBoxStyle.Critical, "ERROR!")
                    bAns = True
                End Try
            End While
        Else
            MsgBox(FullPath + " doesn't exist.", MsgBoxStyle.Critical, "ERROR!")
        End If
        Return bAns
    End Function
#End Region
    Function FilterImageRed(ByVal TheObject As Image) As System.Object
        Dim i, j, a As Integer
        Dim c As System.Drawing.Color
        Dim c2 As System.Drawing.Color
        Dim pic1 As System.Drawing.Bitmap
        Dim pic2 As System.Drawing.Bitmap
        Dim r1, b1, g1, r2, b2, g2 As Integer
        pic1 = My.Resources.BloodSplatter
        pic2 = TheObject
        For j = 1 To TheObject.height - 2
            For i = 1 To TheObject.Width - 2
                c = pic1.GetPixel(i, j)
                c2 = pic2.GetPixel(i, j)
                r1 = c.R : r2 = c2.R
                g1 = c.G : g2 = c2.G
                b1 = c.B : b2 = c2.B
                If r1 = 255 And g1 = 255 And b1 = 255 Then
                    c = Color.FromArgb(a, r2, g2, b2)
                    pic2.SetPixel(i, j, c)
                Else
                    r1 = 255 : g1 = 0 : b1 = 0
                    r2 = (r1 + r2) / 2 : g2 = (g1 + g2) / 2 : b2 = (b1 + b2) / 2
                    c = Color.FromArgb(a, r2, g2, b2)
                    pic2.SetPixel(i, j, c)
                End If
            Next
        Next
        Return TheObject
    End Function
    Function FilterImageFog(ByVal TheObject As Image, ByVal TheColor As Short) As System.Object
        Dim i, j, a As Integer
        Dim c2 As System.Drawing.Color
        Dim pic2 As System.Drawing.Bitmap
        Dim r1, b1, g1, r2, b2, g2 As Integer
        Dim ResultColor As Color
        pic2 = TheObject
        For j = 0 To TheObject.height - 1
            For i = 0 To TheObject.Width - 1
                c2 = pic2.GetPixel(i, j)
                r1 = TheColor : r2 = c2.R
                g1 = TheColor : g2 = c2.G
                b1 = TheColor : b2 = c2.B
                If r1 = 255 And g1 = 255 And b1 = 255 Then
                    ResultColor = Color.FromArgb(a, r2, g2, b2)
                    pic2.SetPixel(i, j, ResultColor)
                Else
                    r2 = (r1 + r2) / 2 : g2 = (g1 + g2) / 2 : b2 = (b1 + b2) / 2
                    ResultColor = Color.FromArgb(a, r2, g2, b2)
                    pic2.SetPixel(i, j, ResultColor)
                End If
            Next
        Next
        Return pic2
    End Function
    Function FilterImageWall(ByVal TheObject As Image, ByVal TheWater As Image, ByVal N As Boolean, ByVal E As Boolean, ByVal S As Boolean, ByVal W As Boolean) As System.Object
        'right now it's a test function to make transparent outsides -> fade into opaque insides
        Dim x, y, a As Integer
        Dim c1, c2 As System.Drawing.Color
        Dim Water As System.Drawing.Bitmap
        Dim BG As System.Drawing.Bitmap
        Dim r1, b1, g1, r2, b2, g2 As Integer
        Dim ResultColor As Color
        Dim GoDirection As Short
        Dim AlphaBG As Double
        Dim AlphaWater As Double
        Dim FadePercent As Double = 0.2 'change this, .1-.3
        Dim NegPercent As Double = 1 - FadePercent
        'The fade will last for 10% of the tiles width or height
        BG = TheObject
        Water = TheWater
        For y = 0 To TheObject.Height - 1
            For x = 0 To TheObject.Width - 1
                'only process transparency if it's in the outer 10% of the tile
                If x / TheObject.Height < TheObject.Height * FadePercent Or y / TheObject.Width < TheObject.Width * FadePercent Or x / TheObject.Height > TheObject.Height * NegPercent Or y / TheObject.Width > TheObject.Width * NegPercent Then
                    'We need to grab the percentage of transparency based on 0-100% of closeness to the inner square
                    'first we need to grab a direction that falls into our transparency formula
                    'if two directions fall into it, favor the smallest direction to remove inconsistancies
                    'note, go direction is always TOWARDS the opaque
                    If x < TheObject.Width * FadePercent And y < TheObject.Height * FadePercent Then 'top leftcorner
                        If x < y Then GoDirection = East Else GoDirection = South
                    ElseIf x < TheObject.Width * FadePercent And y > TheObject.Height * NegPercent Then 'top right corner
                        If x < (TheObject.Height * NegPercent) - y + (TheObject.Height * FadePercent) Then GoDirection = East Else GoDirection = North
                    ElseIf x > TheObject.Width * NegPercent And y < TheObject.Height * FadePercent Then 'bottom left corner
                        If (TheObject.Width * NegPercent) - x + (TheObject.Width * FadePercent) < y Then GoDirection = West Else GoDirection = South
                    ElseIf x > TheObject.Width * NegPercent And y > TheObject.Width * NegPercent Then 'bottom right corner
                        If (TheObject.Width * NegPercent) - x + (TheObject.Width * FadePercent) < (TheObject.Height * NegPercent) - y + (TheObject.Height * FadePercent) Then GoDirection = West Else GoDirection = North
                    ElseIf y < TheObject.Height * FadePercent Then 'top side
                        GoDirection = South
                    ElseIf y > TheObject.Height * NegPercent Then 'bottom side
                        GoDirection = North
                    ElseIf x < TheObject.Width * FadePercent Then 'left side
                        GoDirection = East
                    ElseIf x > TheObject.Width * NegPercent Then 'right side
                        GoDirection = West
                    Else
                        GoDirection = 10 'none
                        AlphaBG = 1
                        AlphaWater = 1
                    End If
                    If GoDirection = South And N = True Then
                        AlphaBG = ((TheObject.Height * FadePercent) - y) / (TheObject.Height * FadePercent)
                        AlphaWater = y / (TheObject.Height * FadePercent)
                    ElseIf GoDirection = East And W = True Then
                        AlphaBG = ((TheObject.Width * FadePercent) - x) / (TheObject.Width * FadePercent)
                        AlphaWater = x / (TheObject.Width * FadePercent)
                    ElseIf GoDirection = North And S = True Then
                        AlphaBG = ((TheObject.Height - (TheObject.Height * NegPercent)) - (TheObject.Height - y)) / (TheObject.Height - (TheObject.Height * NegPercent))
                        AlphaWater = (TheObject.Height - y) / (TheObject.Height - (TheObject.Height * NegPercent))
                    ElseIf GoDirection = West And E = True Then
                        AlphaBG = ((TheObject.Width - (TheObject.Width * NegPercent)) - (TheObject.Width - x)) / (TheObject.Width - (TheObject.Width * NegPercent))
                        AlphaWater = (TheObject.Width - x) / (TheObject.Width - (TheObject.Width * NegPercent))
                    Else
                        AlphaBG = 1
                        AlphaWater = 1
                    End If
                    c2 = BG.GetPixel(x, y)
                    c1 = Water.GetPixel(x, y)
                    r1 = c1.R : r2 = c2.R
                    g1 = c1.G : g2 = c2.G
                    b1 = c1.B : b2 = c2.B
                    If r1 = 255 And g1 = 255 And b1 = 255 Then
                        ResultColor = Color.FromArgb(a, r2, g2, b2)
                        BG.SetPixel(x, y, ResultColor)
                    Else
                        r2 = (r1 * AlphaWater + r2 * AlphaBG) / 2 : g2 = (g1 * AlphaWater + g2 * AlphaBG) / 2 : b2 = (b1 * AlphaWater + b2 * AlphaBG) / 2
                        ResultColor = Color.FromArgb(a, r2, g2, b2)
                        BG.SetPixel(x, y, ResultColor)
                    End If
                End If
            Next
        Next
        Return BG 'returns a modified background
    End Function
    Function FilterImageWater(ByVal TheObject As Image, ByVal TheWater As Image) As System.Object
        'right now it's a test function to make transparent outsides -> fade into opaque insides
        Dim x, y, a As Integer
        Dim c1, c2 As System.Drawing.Color
        Dim Water As System.Drawing.Bitmap
        Dim BG As System.Drawing.Bitmap
        Dim r1, b1, g1, r2, b2, g2 As Integer
        Dim ResultColor As Color
        Dim GoDirection As Short
        Dim AlphaBG As Double
        Dim AlphaWater As Double
        Dim FadePercent As Double = 0.2 'change this, .1-.3
        Dim NegPercent As Double = 1 - FadePercent
        'The fade will last for 10% of the tiles width or height
        BG = TheObject
        Water = TheWater
        For y = 0 To TheObject.Height - 1
            For x = 0 To TheObject.Width - 1
                'only process transparency if it's in the outer 10% of the tile
                If x / TheObject.Height < TheObject.Height * FadePercent Or y / TheObject.Width < TheObject.Width * FadePercent Or x / TheObject.Height > TheObject.Height * NegPercent Or y / TheObject.Width > TheObject.Width * NegPercent Then
                    'We need to grab the percentage of transparency based on 0-100% of closeness to the inner square
                    'first we need to grab a direction that falls into our transparency formula
                    'if two directions fall into it, favor the smallest direction to remove inconsistancies
                    'note, go direction is always TOWARDS the opaque
                    If x < TheObject.Width * FadePercent And y < TheObject.Height * FadePercent Then 'top leftcorner
                        If x < y Then GoDirection = East Else GoDirection = South
                    ElseIf x < TheObject.Width * FadePercent And y > TheObject.Height * NegPercent Then 'top right corner
                        If x < (TheObject.Height * NegPercent) - y + (TheObject.Height * FadePercent) Then GoDirection = East Else GoDirection = North
                    ElseIf x > TheObject.Width * NegPercent And y < TheObject.Height * FadePercent Then 'bottom left corner
                        If (TheObject.Width * NegPercent) - x + (TheObject.Width * FadePercent) < y Then GoDirection = West Else GoDirection = South
                    ElseIf x > TheObject.Width * NegPercent And y > TheObject.Width * NegPercent Then 'bottom right corner
                        If (TheObject.Width * NegPercent) - x + (TheObject.Width * FadePercent) < (TheObject.Height * NegPercent) - y + (TheObject.Height * FadePercent) Then GoDirection = West Else GoDirection = North
                    ElseIf y < TheObject.Height * FadePercent Then 'top side
                        GoDirection = South
                    ElseIf y > TheObject.Height * NegPercent Then 'bottom side
                        GoDirection = North
                    ElseIf x < TheObject.Width * FadePercent Then 'left side
                        GoDirection = East
                    ElseIf x > TheObject.Width * NegPercent Then 'right side
                        GoDirection = West
                    Else
                        GoDirection = 10 'none
                        AlphaBG = 1
                        AlphaWater = 1
                    End If
                    If GoDirection = South Then
                        AlphaBG = ((TheObject.Height * FadePercent) - y) / (TheObject.Height * FadePercent)
                        AlphaWater = y / (TheObject.Height * FadePercent)
                    ElseIf GoDirection = East Then
                        AlphaBG = ((TheObject.Width * FadePercent) - x) / (TheObject.Width * FadePercent)
                        AlphaWater = x / (TheObject.Width * FadePercent)
                    ElseIf GoDirection = North Then
                        AlphaBG = ((TheObject.Height - (TheObject.Height * NegPercent)) - (TheObject.Height - y)) / (TheObject.Height - (TheObject.Height * NegPercent))
                        AlphaWater = (TheObject.Height - y) / (TheObject.Height - (TheObject.Height * NegPercent))
                    ElseIf GoDirection = West Then
                        AlphaBG = ((TheObject.Width - (TheObject.Width * NegPercent)) - (TheObject.Width - x)) / (TheObject.Width - (TheObject.Width * NegPercent))
                        AlphaWater = (TheObject.Width - x) / (TheObject.Width - (TheObject.Width * NegPercent))
                    Else
                        AlphaBG = 1
                        AlphaWater = 1
                    End If
                    c2 = BG.GetPixel(x, y)
                    c1 = Water.GetPixel(x, y)
                    r1 = c1.R : r2 = c2.R
                    g1 = c1.G : g2 = c2.G
                    b1 = c1.B : b2 = c2.B
                    If r1 = 255 And g1 = 255 And b1 = 255 Then
                        ResultColor = Color.FromArgb(a, r2, g2, b2)
                        BG.SetPixel(x, y, ResultColor)
                    Else
                        r2 = (r1 * AlphaWater + r2 * AlphaBG) / 2 : g2 = (g1 * AlphaWater + g2 * AlphaBG) / 2 : b2 = (b1 * AlphaWater + b2 * AlphaBG) / 2
                        ResultColor = Color.FromArgb(a, r2, g2, b2)
                        BG.SetPixel(x, y, ResultColor)
                    End If
                    End If
            Next
        Next
        Return BG 'returns a modified background
    End Function
#Region "Mobile Actions & Battle"
    Function MoveMobile(ByVal MobNum As Short, ByVal MvType As Short)
        Dim MobileDead As Boolean = False
        Dim x As Short = MobilePosX(MobNum)
        Dim y As Short = MobilePosY(MobNum)
        If MvType = North And MobilePosY(MobNum) > 0 Then 'North movement
            If Map(MobilePosX(MobNum), MobilePosY(MobNum) - 1) = Item Then 'mobile moves onto an item and picks it up
            ElseIf MobilePosX(MobNum) = PlayerPosX And MobilePosY(MobNum) - 1 = PlayerPosY Then 'mobile moves into player
                KillMob(MobNum)
                MobileDead = True
            End If
            If MobileDead = False Then
                MobilePosY(MobNum) -= 1
                MapOccupied(MobilePosX(MobNum), MobilePosY(MobNum)) = MobileType(MobNum)
                MapOccupied(MobilePrevX(MobNum), MobilePrevY(MobNum)) = 0
                MobileLastMove(MobNum) = North
            End If
        ElseIf MvType = East And MobilePosX(MobNum) < MapSize Then 'East movement
            If Map(MobilePosX(MobNum) + 1, MobilePosY(MobNum)) = Item Then 'mobile moves onto a piece
            ElseIf MobilePosX(MobNum) + 1 = PlayerPosX And MobilePosY(MobNum) = PlayerPosY Then 'mobile moves into player
                KillMob(MobNum)
                MobileDead = True
            End If
            If MobileDead = False Then
                MobilePosX(MobNum) += 1
                MapOccupied(MobilePosX(MobNum), MobilePosY(MobNum)) = MobileType(MobNum)
                MapOccupied(MobilePrevX(MobNum), MobilePrevY(MobNum)) = 0
                MobileLastMove(MobNum) = East
            End If
        ElseIf MvType = South And MobilePosY(MobNum) < MapSize Then 'south movement
            If Map(MobilePosX(MobNum), MobilePosY(MobNum) + 1) = Item Then 'mobile moves onto a piece
            ElseIf MobilePosX(MobNum) = PlayerPosX And MobilePosY(MobNum) + 1 = PlayerPosY Then 'mobile moves into player
                KillMob(MobNum)
                MobileDead = True
            End If
            If MobileDead = False Then
                MobilePosY(MobNum) += 1
                MapOccupied(MobilePosX(MobNum), MobilePosY(MobNum)) = MobileType(MobNum)
                MapOccupied(MobilePrevX(MobNum), MobilePrevY(MobNum)) = 0
                MobileLastMove(MobNum) = South
            End If
        ElseIf MvType = West And MobilePosX(MobNum) > 0 Then 'west movement
            If Map(MobilePosX(MobNum) - 1, MobilePosY(MobNum)) = Item Then 'mobile moves onto a piece
            ElseIf MobilePosX(MobNum) - 1 = PlayerPosX And MobilePosY(MobNum) = PlayerPosY Then 'mobile moves into player
                KillMob(MobNum)
                MobileDead = True
            End If
            If MobileDead = False Then
                MobilePosX(MobNum) -= 1
                MapOccupied(MobilePosX(MobNum), MobilePosY(MobNum)) = MobileType(MobNum)
                MapOccupied(MobilePrevX(MobNum), MobilePrevY(MobNum)) = 0 'mobile moved, previous position is blank
                MobileLastMove(MobNum) = West 'dictates the mobiles last movement direction for pattern-making movements
            End If
        End If
        Return 0
    End Function
    Function KillMob(ByVal Mobnum As Short, Optional ByVal MobString As String = "Enemy")
        If MobilePosX(Mobnum) < 26 And MobilePosY(Mobnum) < 26 Then 'prevent index errors, we set off-map mobiles when they're dead
            If Map(MobilePosX(Mobnum), MobilePosY(Mobnum)) = Floor Then
                Map(MobilePosX(Mobnum), MobilePosY(Mobnum)) = SpecialFloor
            End If
        End If
        If MobString <> "SILENCE MOB KILL" Then
            MapOccupied(MobilePosX(Mobnum), MobilePosY(Mobnum)) = False
            MobilePosX(Mobnum) = MapSize + 1 : MobilePosY(Mobnum) = MapSize + 1
            MobileHealth(Mobnum) = 0
            PlayerExperience += MobileType(Mobnum)  'mobiletype distinguishes it's difficulty and therefor applys likewise to experience gained.
            If PlayerExperience >= 100 Then
                LevelUp()
            End If
            SND(UCase(Mid(MobString, 1, 1)) + Mid(MobString, 2, Len(MobString)) + " is dead.")
        Else
            MobileHealth(Mobnum) = 0
            MapOccupied(MobilePosX(Mobnum), MobilePosY(Mobnum)) = False
            MobilePosX(Mobnum) = MapSize + 1 : MobilePosY(Mobnum) = MapSize + 1
        End If
        Return 0
    End Function
    Function MobileFleeFail(ByVal Mobnum As Short)
        Dim MobileNameString As String = ""
        If MobileType(Mobnum) = 1 Then
            MobileNameString = "A rat"
        ElseIf MobileType(Mobnum) = 2 Then
            MobileNameString = "A bat"
        ElseIf MobileType(Mobnum) = 3 Then
            MobileNameString = "An imp"
        ElseIf MobileType(Mobnum) = 4 Then
            MobileNameString = "A goblin"
        ElseIf MobileType(Mobnum) = 5 Then
            MobileNameString = "A troll"
        ElseIf MobileType(Mobnum) = 6 Then
            MobileNameString = "An ogre"
        ElseIf MobileType(Mobnum) = 7 Then
            MobileNameString = "A catoblepas"
        ElseIf MobileType(Mobnum) = 8 Then
            MobileNameString = "A parandrus"
        ElseIf MobileType(Mobnum) = 9 Then
            MobileNameString = "A clurichuan"
        ElseIf MobileType(Mobnum) = 10 Then
            MobileNameString = "A dullahan"
        ElseIf MobileType(Mobnum) = 11 Then
            MobileNameString = "A golem"
        ElseIf MobileType(Mobnum) = 12 Then
            MobileNameString = "A sceadugengan"
        ElseIf MobileType(Mobnum) = 13 Then
            MobileNameString = "A schilla"
        End If
        SND(MobileNameString + " trips in its terror.")
        MobileHealth(Mobnum) -= 1
        If MobileHealth(Mobnum) <= 0 Then
            KillMob(Mobnum, MobileNameString)
            SND(MobileNameString + " falls in a heap dead.")
        End If
        Return 0
    End Function
    Function FleeMob(ByVal Mobnum As Short)
        Dim MobileNameString As String = ""
        If MobileType(Mobnum) = 1 Then
            MobileNameString = "a rat"
        ElseIf MobileType(Mobnum) = 2 Then
            MobileNameString = "a bat"
        ElseIf MobileType(Mobnum) = 3 Then
            MobileNameString = "an imp"
        ElseIf MobileType(Mobnum) = 4 Then
            MobileNameString = "a goblin"
        ElseIf MobileType(Mobnum) = 5 Then
            MobileNameString = "a troll"
        ElseIf MobileType(Mobnum) = 6 Then
            MobileNameString = "an ogre"
        ElseIf MobileType(Mobnum) = 7 Then
            MobileNameString = "a catoblepas"
        ElseIf MobileType(Mobnum) = 8 Then
            MobileNameString = "a parandrus"
        ElseIf MobileType(Mobnum) = 9 Then
            MobileNameString = "a clurichuan"
        ElseIf MobileType(Mobnum) = 10 Then
            MobileNameString = "a dullahan"
        ElseIf MobileType(Mobnum) = 11 Then
            MobileNameString = "a golem"
        ElseIf MobileType(Mobnum) = 12 Then
            MobileNameString = "a sceadugengan"
        ElseIf MobileType(Mobnum) = 13 Then
            MobileNameString = "a schilla"
        End If
        SND(MobileNameString + " turns to flee.")
        MobileFlee(Mobnum) = Math.Round(PlayerCHA / 10, 0)
        Return 0
    End Function
    Function HitMob(ByVal Mobnum As Short, Optional ByVal Counter As Boolean = False, Optional ByVal HideAttack As Boolean = False)
        Dim MobileNameString As String = ""
        Dim TestCriticalStrike As New Random
        Dim CritStrike As Short = 0
        If MobileType(Mobnum) = 1 Then
            MobileNameString = "a rat"
        ElseIf MobileType(Mobnum) = 2 Then
            MobileNameString = "a bat"
        ElseIf MobileType(Mobnum) = 3 Then
            MobileNameString = "an imp"
        ElseIf MobileType(Mobnum) = 4 Then
            MobileNameString = "a goblin"
        ElseIf MobileType(Mobnum) = 5 Then
            MobileNameString = "a troll"
        ElseIf MobileType(Mobnum) = 6 Then
            MobileNameString = "an ogre"
        ElseIf MobileType(Mobnum) = 7 Then
            MobileNameString = "a catoblepas"
        ElseIf MobileType(Mobnum) = 8 Then
            MobileNameString = "a parandrus"
        ElseIf MobileType(Mobnum) = 9 Then
            MobileNameString = "a clurichuan"
        ElseIf MobileType(Mobnum) = 10 Then
            MobileNameString = "a dullahan"
        ElseIf MobileType(Mobnum) = 11 Then
            MobileNameString = "a golem"
        ElseIf MobileType(Mobnum) = 12 Then
            MobileNameString = "a sceadugengan"
        ElseIf MobileType(Mobnum) = 13 Then
            MobileNameString = "a schilla"
        End If
        If HideAttack = False Then
            If SkillType = "" Then CritStrike = TestCriticalStrike.Next(0, 101) Else CritStrike = TestCriticalStrike.Next(0, 101)
            If CritStrike <= PlayerSTR And SkillType = "" Then 'player critically striked. chance to critically strike is the players strength
                MobileHealth(Mobnum) -= PlayerAttack * 2
                If Counter = False Then
                    SND("You CRIT " + MobileNameString + ".")
                Else
                    SND("You counter CRITS " + MobileNameString + ".")
                End If
            ElseIf CritStrike <= PlayerINT And SkillType <> "" Then 'player critically striked with a skill.
                If SkillType = "Punch" Or SkillType = "Kick" Or SkillType = "Hit" Or SkillType = "Strike" Or SkillType = "Slice" Or SkillType = "Stab" Or SkillType = "Shoot" Then
                    'these are the basic +1 skilltypes, and all do the same
                    MobileHealth(Mobnum) -= PlayerAttack * 2 + 2
                    SND("Your skill CRITS " + MobileNameString + ".")
                ElseIf SkillType = "Wound" Then
                    MobileHealth(Mobnum) -= 40
                    SND("Your skill CRITS " + MobileNameString + ".")
                End If
            ElseIf SkillType = "" Then 'basic attack, test mobile dodge, then mobile miss
                CritStrike = TestCriticalStrike.Next(0, 101)
                If CritStrike <= 7 Then 'all mobs have 7% chance to dodge
                    If Counter = False Then
                        SND("Your attack is dodged.")
                    Else
                        SND("Your counter is dodged.")
                    End If
                ElseIf CritStrike <= 15 Then 'all attacks have 8% chance to miss
                    If Counter = False Then
                        SND("Your attack misses.")
                    Else
                        SND("Your counter misses.")
                    End If
                Else
                    If Fury > 0 Then 'fury increases regular attack strength by 1
                        MobileHealth(Mobnum) -= PlayerAttack + 1
                    Else
                        MobileHealth(Mobnum) -= PlayerAttack
                    End If
                    If Counter = False Then
                        SND("You hit " + MobileNameString + ".")
                    Else
                        SND("You counter " + MobileNameString + ".")
                    End If
                End If
            ElseIf SkillType <> "" Then 'basic skill
                If SkillType = "Punch" Then 'just a +1 attack
                    MobileHealth(Mobnum) -= PlayerAttack + 1
                    SND("You punch " + MobileNameString + ".")
                ElseIf SkillType = "Kick" Then 'just a +1 attack
                    MobileHealth(Mobnum) -= PlayerAttack + 1
                    SND("You kick " + MobileNameString + ".")
                ElseIf SkillType = "Hit" Then 'just a +1 attack
                    MobileHealth(Mobnum) -= PlayerAttack + 1
                    SND("You hit " + MobileNameString + ".")
                ElseIf SkillType = "Strike" Then 'just a +1 attack
                    MobileHealth(Mobnum) -= PlayerAttack + 1
                    SND("You strike " + MobileNameString + ".")
                ElseIf SkillType = "Slice" Then 'just a +1 attack
                    MobileHealth(Mobnum) -= PlayerAttack + 1
                    SND("You slice " + MobileNameString + ".")
                ElseIf SkillType = "Stab" Then 'just a +1 attack
                    MobileHealth(Mobnum) -= PlayerAttack + 1
                    SND("You stab " + MobileNameString + ".")
                ElseIf SkillType = "Shoot" Then 'just a +1 attack
                    MobileHealth(Mobnum) -= PlayerAttack + 1
                    SND("You shoot " + MobileNameString + ".")
                ElseIf SkillType = "Wound" Then '20 attack
                    MobileHealth(Mobnum) -= 20
                    SND("You decimate " + MobileNameString + ".")
                ElseIf SkillType = "Stun" Then 'stun enemy preventing movement and attacks for 3 rounds
                    MobileStun(Mobnum) = 3
                    SND("You stun " + MobileNameString + ".")
                ElseIf SkillType = "Double Slice" Then 'double slice the enemy
                    MobileHealth(Mobnum) -= PlayerAttack * 2
                    SND("You slice " + MobileNameString + ".")
                    SND("You slice " + MobileNameString + ".")
                ElseIf SkillType = "Trip" Then 'stun enemy for 2 rounds
                    MobileStun(Mobnum) = 2
                    SND("You trip " + MobileNameString + ".")
                ElseIf SkillType = "Runestrike" Then 'runstrike enemy
                    MobileStun(Mobnum) = 2
                    MobileHealth(Mobnum) -= PlayerAttack
                    SND("You runstrike " + MobileNameString + ".")
                ElseIf SkillType = "Fireball" Then 'fireball enemy
                    MobileHealth(Mobnum) -= PlayerAttack + 10
                    SND("A fireball mutilates " + MobileNameString + ".")
                ElseIf SkillType = "Clumsiness" Then 'clumsiness enemy
                    SND("You clumsiness " + MobileNameString + ".")
                    MobileClumsiness(Mobnum) = 5
                ElseIf SkillType = "Holy Bolt" Then 'holy bolt enemy
                    SND("Holy Bolt erradicates " + MobileNameString + ".")
                    MobileHealth(Mobnum) -= PlayerAttack + 10
                ElseIf SkillType = "Fire Arrow" Then 'fire arrow enemy
                    SND("Fire Arrow sears " + MobileNameString + ".")
                    MobileHealth(Mobnum) -= PlayerAttack + 3
                ElseIf SkillType = "Sacrifice" Then 'sacrifice hp for extra damage attack
                    SND("You demolish " + MobileNameString + ".")
                    MobileHealth(Mobnum) -= PlayerAttack + 10
                ElseIf SkillType = "Triple Slice" Then 'double slice the enemy
                    MobileHealth(Mobnum) -= PlayerAttack * 3
                    SND("You slice " + MobileNameString + ".")
                    SND("You slice " + MobileNameString + ".")
                    SND("You slice " + MobileNameString + ".")
                End If
                SkillType = "" 'makes sure you don't use the skill again for free ;0
            End If
        Else
            MobileHealth(Mobnum) -= 1
            SND("Immolation burns " + MobileNameString + ".")
        End If
        If MobileHealth(Mobnum) <= 0 Then
            KillMob(Mobnum, MobileNameString)
        End If
        Return 0
    End Function
    Function PlayerHitLocation(ByVal X As Short, ByVal Y As Short) 'This determines which mobile the player hits then sends it to function "hitmob" to determine damage
        Dim MobXVar As Short
        For MobXVar = 0 To 9 Step 1
            If X = MobilePosX(MobXVar) And Y = MobilePosY(MobXVar) Then
                HitMob(MobXVar)
                Exit For
            End If
        Next
        Return 0
    End Function
    Function HitChar(ByVal Mobnum As Short) 'mobile damage depends on the mobile type
        Dim MobileNameString As String = ""
        Dim DamageAmount As Short
        Dim SupressDueToCriticalStrike As Boolean = False
        Dim TestDodgeRandom As New Random
        Dim TestDodge As Short = 0
        If MobileType(Mobnum) = 1 Then
            DamageAmount = 3
            MobileNameString = "A rat"
        ElseIf MobileType(Mobnum) = 2 Then
            DamageAmount = 6
            MobileNameString = "A bat"
        ElseIf MobileType(Mobnum) = 3 Then
            DamageAmount = 9
            MobileNameString = "An imp"
        ElseIf MobileType(Mobnum) = 4 Then
            DamageAmount = 12
            MobileNameString = "A goblin"
        ElseIf MobileType(Mobnum) = 5 Then
            DamageAmount = 15
            MobileNameString = "A troll"
        ElseIf MobileType(Mobnum) = 6 Then
            DamageAmount = 18
            MobileNameString = "An ogre"
        ElseIf MobileType(Mobnum) = 7 Then
            DamageAmount = 21
            MobileNameString = "A catoblepas"
        ElseIf MobileType(Mobnum) = 8 Then
            DamageAmount = 24
            MobileNameString = "A parandrus"
        ElseIf MobileType(Mobnum) = 9 Then
            DamageAmount = 27
            MobileNameString = "A clurichuan"
        ElseIf MobileType(Mobnum) = 10 Then
            DamageAmount = 30
            MobileNameString = "A dullahan"
        ElseIf MobileType(Mobnum) = 11 Then
            DamageAmount = 33
            MobileNameString = "A golem"
        ElseIf MobileType(Mobnum) = 12 Then
            DamageAmount = 36
            MobileNameString = "A sceadugengan"
        ElseIf MobileType(Mobnum) = 13 Then
            DamageAmount = 39
            MobileNameString = "A schilla"
        Else
            PlayerCurHitpoints -= 10
            SND("You trip and damage yourself.")
            SupressDueToCriticalStrike = True
        End If
        If BoneShield > 0 Then
            SupressDueToCriticalStrike = True
            SND(MobileNameString + " attacks bone shield.")
        End If
        If PlayerHidden > 0 Then
            SupressDueToCriticalStrike = True
            SND(MobileNameString + " searches for you.")
        End If
        TestDodge = TestDodgeRandom.Next(0, 100)
        If PlayerDEX >= TestDodge Then 'player dodged the attack due to their dexterity score
            SupressDueToCriticalStrike = True
            SND("You dodge an attack.")
        End If
        TestDodge = TestDodgeRandom.Next(0, 100) 'test miss, 10%
        If TestDodge <= 10 And MobileClumsiness(Mobnum) <= 0 Then
            SupressDueToCriticalStrike = True
            SND(MobileNameString + "'s attack misses.")
        ElseIf MobileClumsiness(Mobnum) > 0 And TestDodge <= 50 Then
            SupressDueToCriticalStrike = True
            SND(MobileNameString + "'s attack misses.")
        End If
        If MobileClumsiness(Mobnum) > 0 Then MobileClumsiness(Mobnum) -= 1
        If SupressDueToCriticalStrike = False Then
            DamageAmount -= PlayerDefense
            If Block > 0 Then 'block skill is active, reduce all damage by 2
                DamageAmount -= 2
            End If
            If MagicShield > 0 Then DamageAmount -= 1 'magic shield reduces 1 damage
            If DamageAmount <= 0 Then
                SND(MobileNameString + " hits you too weak.")
            Else
                PlayerCurHitpoints -= DamageAmount
                SND(MobileNameString + " hits you for " + LTrim(Str(DamageAmount)) + ".")
            End If
        End If
        HealthBar.Caption = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints)) + " HP"
        HealthBar.Value = PlayerCurHitpoints
        If CounterAttack > 0 Then
            HitMob(Mobnum, True)
        End If
        If Immolate > 0 Then HitMob(Mobnum, False, True)
        Return 0
    End Function
    Function DetermineMobMov(ByVal MobNum As Short)
        'The way it works:
        '    Can mobile see character?
        '    If yes, mobile will walk towards him
        '    If no, mobile will walk around randomly
        Dim Resolved As Boolean = True
        Dim StepNum As Short = 0
        Dim AlreadyMoved As Boolean = False
        Dim FleeinTerror As New Random
        Dim FleeResult As Short
        If MobilePosX(MobNum) = PlayerPosX And MobilePosY(MobNum) = PlayerPosY Then
            'player stepped on mob, mobile is dead, no path required. This sometimes happens on a bad spawn
            KillMob(MobNum, "SILENCE MOB KILL") 'send optional killmob text that supresses xp and message of mob dead
            Return 0
            Exit Function
        End If
        'check to see if character is close
        If Math.Abs(MobilePosX(MobNum) - PlayerPosX) < 3 And Math.Abs(MobilePosY(MobNum) - PlayerPosY) < 3 Then '3 block radius of visibility
            Resolved = False
        End If
        While Resolved = False And MobileFlee(MobNum) = 0 And PlayerHidden = 0 'this is mobile pathfinding straight to the player
            StepNum += 1
            If PlayerPosX > MobilePosX(MobNum) Then
                'if the variable isn't passed into this if statement, it's because mobile tried moving onto another mobile or wall
                If Map(MobilePosX(MobNum) + 1, MobilePosY(MobNum)) > 0 And Map(MobilePosX(MobNum) + 1, MobilePosY(MobNum)) < 4 Then
                    If MobilePosX(MobNum) + 1 = PlayerPosX And MobilePosY(MobNum) = PlayerPosY Then 'if mobile plans on moving east and character is to the east, hit character instead of move
                        FleeResult = FleeinTerror.Next(0, 101)
                        If FleeResult <= PlayerCHA Then
                            FleeMob(MobNum)
                            Resolved = True
                        Else
                            HitChar(MobNum)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    Else
                        If MapOccupied(MobilePosX(MobNum) + 1, MobilePosY(MobNum)) = 0 Then 'ensures that the sector isn't already occupied by another mobile
                            MoveMobile(MobNum, East)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    End If
                End If
            End If
            If PlayerPosX < MobilePosX(MobNum) And Resolved = False Then
                'if the variable isn't passed into this if statement, it's because mobile tried moving onto another mobile or wall
                If Map(MobilePosX(MobNum) - 1, MobilePosY(MobNum)) > 0 And Map(MobilePosX(MobNum) - 1, MobilePosY(MobNum)) < 4 Then
                    If MobilePosX(MobNum) - 1 = PlayerPosX And MobilePosY(MobNum) = PlayerPosY Then 'if mobile plans on moving west and character is to the west, hit character instead of moving
                        FleeResult = FleeinTerror.Next(0, 101)
                        If FleeResult <= PlayerCHA Then
                            FleeMob(MobNum)
                            Resolved = True
                        Else
                            HitChar(MobNum)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    Else
                        If MapOccupied(MobilePosX(MobNum) - 1, MobilePosY(MobNum)) = 0 Then 'ensures that the sector isn't already occupied by another mobile
                            MoveMobile(MobNum, West)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    End If
                End If
            End If
            If PlayerPosY > MobilePosY(MobNum) And Resolved = False Then
                'if the variable isn't passed into this if statement, it's because mobile tried moving onto another mobile or wall
                If Map(MobilePosX(MobNum), MobilePosY(MobNum) + 1) > 0 And Map(MobilePosX(MobNum), MobilePosY(MobNum) + 1) < 4 Then
                    If MobilePosY(MobNum) + 1 = PlayerPosY And MobilePosX(MobNum) = PlayerPosX Then 'if mobile plans on moving south and character is to the south, hit character instead of moving
                        FleeResult = FleeinTerror.Next(0, 101)
                        If FleeResult <= PlayerCHA Then
                            FleeMob(MobNum)
                            Resolved = True
                        Else
                            HitChar(MobNum)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    Else
                        If MapOccupied(MobilePosX(MobNum), MobilePosY(MobNum) + 1) = 0 Then 'ensures that the sector isn't already occupied by another mobile
                            MoveMobile(MobNum, South)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    End If
                End If
            End If
            If PlayerPosY < MobilePosY(MobNum) And Resolved = False Then
                'if the variable isn't passed into this if statement, it's because mobile tried moving onto another mobile or wall
                If Map(MobilePosX(MobNum), MobilePosY(MobNum) - 1) > 0 And Map(MobilePosX(MobNum), MobilePosY(MobNum) - 1) < 4 Then
                    If MobilePosY(MobNum) - 1 = PlayerPosY And MobilePosX(MobNum) = PlayerPosX Then 'if mobile plans on moving north and character is to the north, hit character instead of moving
                        FleeResult = FleeinTerror.Next(0, 101)
                        If FleeResult <= PlayerCHA Then
                            FleeMob(MobNum)
                            Resolved = True
                        Else
                            HitChar(MobNum)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    Else
                        If MapOccupied(MobilePosX(MobNum), MobilePosY(MobNum) - 1) = 0 Then 'ensures that the sector isn't already occupied by another mobile
                            MoveMobile(MobNum, North)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    End If
                End If
            End If
            If StepNum = 6 Then 'this prevents the recursion from continuing forever if the mobile has nowhere to move
                Resolved = True
            End If
        End While
        If MobileFlee(MobNum) > 0 Or PlayerHidden > 0 Then
            MobileFlee(MobNum) -= 1
            If MobileFlee(MobNum) < 0 Then MobileFlee(MobNum) = 0
            Resolved = True
        End If
        If Resolved = True And AlreadyMoved = False Then 'this is random mobile movement since the player isn't visible
            Dim FinishMovement As Boolean = False
            Dim RandomDirection As New Random
            Dim RandomPick As Short = RandomDirection.Next(1)
            If RandomPick = 0 Then RandomPick = MobileLastMove(MobNum) 'continues in same direction unless blocked, 50% chance
            If RandomPick = 1 Then RandomPick = RandomDirection.Next(1, 5) 'makes new path, 50% chance
            Dim Tries As Short = 1
            While FinishMovement = False
                If RandomPick = 1 And MobilePosY(MobNum) > 0 And MobileHealth(MobNum) > 0 Then 'north
                    If MobileLastMove(MobNum) <> South Then
                        If Map(MobilePosX(MobNum), MobilePosY(MobNum) - 1) > 0 And Map(MobilePosX(MobNum), MobilePosY(MobNum) - 1) < 4 Then 'is there no walls to the north?
                            If MobilePosY(MobNum) - 1 = PlayerPosY And MobilePosX(MobNum) = PlayerPosX Then
                                If MobileFlee(MobNum) > 0 Then
                                    MobileFleeFail(MobNum)
                                End If
                                'mobile can't move into player, this is set incase the mobile is fleeing
                            Else
                                If MapOccupied(MobilePosX(MobNum), MobilePosY(MobNum) - 1) = 0 Then 'doesn't allow mobs to group up in a single sector
                                    FinishMovement = True
                                    MoveMobile(MobNum, North)
                                End If
                            End If
                        End If
                    End If
                ElseIf RandomPick = 2 And MobilePosX(MobNum) < 25 And MobileHealth(MobNum) > 0 Then 'east
                    If MobileLastMove(MobNum) <> West Then
                        'cannot allow mobiles to go back to spots they were just at in random direction.
                        If Map(MobilePosX(MobNum) + 1, MobilePosY(MobNum)) > 0 And Map(MobilePosX(MobNum) + 1, MobilePosY(MobNum)) < 4 Then 'is there no walls to the east?
                            If MobilePosY(MobNum) = PlayerPosY And MobilePosX(MobNum) + 1 = PlayerPosX Then
                                If MobileFlee(MobNum) > 0 Then
                                    MobileFleeFail(MobNum)
                                End If
                                'mobile can't move into player, this is set incase the mobile is fleeing
                            Else
                                If MapOccupied(MobilePosX(MobNum) + 1, MobilePosY(MobNum)) = 0 Then 'doesn't allow mobs to group up in a single sector
                                    FinishMovement = True
                                    MoveMobile(MobNum, East)
                                End If
                            End If
                        End If
                    End If
                ElseIf RandomPick = 3 And MobilePosY(MobNum) < 25 And MobileHealth(MobNum) > 0 Then 'south
                    If MobileLastMove(MobNum) <> North Then
                        'cannot allow mobiles to go back to spots they were just at in random direction.
                        If Map(MobilePosX(MobNum), MobilePosY(MobNum) + 1) > 0 And Map(MobilePosX(MobNum), MobilePosY(MobNum) + 1) < 4 Then 'is there no walls to the south?
                            If MobilePosY(MobNum) + 1 = PlayerPosY And MobilePosX(MobNum) = PlayerPosX Then
                                If MobileFlee(MobNum) > 0 Then
                                    MobileFleeFail(MobNum)
                                End If
                                'mobile can't move into player, this is set incase the mobile is fleeing
                            Else
                                If MapOccupied(MobilePosX(MobNum), MobilePosY(MobNum) + 1) = 0 Then 'doesn't allow mobs to group up in a single sector
                                    FinishMovement = True
                                    MoveMobile(MobNum, South)
                                End If
                            End If
                        End If
                    End If
                ElseIf RandomPick = 4 And MobilePosX(MobNum) > 0 And MobileHealth(MobNum) > 0 Then 'west
                    If MobileLastMove(MobNum) <> East Then
                        'cannot allow mobiles to go back to spots they were just at in random direction.
                        If Map(MobilePosX(MobNum) - 1, MobilePosY(MobNum)) > 0 And Map(MobilePosX(MobNum) - 1, MobilePosY(MobNum)) < 4 Then 'is there no walls to the west?
                            If MobilePosY(MobNum) = PlayerPosY And MobilePosX(MobNum) - 1 = PlayerPosX Then
                                If MobileFlee(MobNum) > 0 Then
                                    MobileFleeFail(MobNum)
                                End If
                                'mobile can't move into player, this is set incase the mobile is fleeing
                            Else
                                If MapOccupied(MobilePosX(MobNum) - 1, MobilePosY(MobNum)) = 0 Then 'doesn't allow mobs to group up in a single sector
                                    FinishMovement = True
                                    MoveMobile(MobNum, West)
                                End If
                            End If
                        End If
                    End If
                End If
                Tries += 1
                If Tries >= 8 Then 'stay in same spot... boorrrring... really rare
                    FinishMovement = True
                    MobileLastMove(MobNum) = 5
                    Tries = 1
                End If
                RandomPick = RandomDirection.Next(1, 5)
            End While
        End If
        MobilePrevX(MobNum) = MobilePosX(MobNum)
        MobilePrevY(MobNum) = MobilePosY(MobNum)
        Return 0
    End Function
#End Region
#Region "Player FoV / LoS"
    Function IsVisible(ByVal x As Short, ByVal y As Short) As Short
        Dim TestVarX As Short = PlayerPosX
        Dim TestVarY As Short = PlayerPosY
        Dim TestResult As Short = 0
        While TestVarX <> x Or TestVarY <> y
            If Math.Abs(TestVarX - x) - Math.Abs(TestVarY - y) >= 0 Then 'will change x first, it is the greatest off
                If TestVarX < x Then
                    TestVarX += 1
                ElseIf TestVarX > x Then
                    TestVarX -= 1
                End If
            Else
                If TestVarY < y Then
                    TestVarY += 1
                ElseIf TestVarY > y Then
                    TestVarY -= 1
                End If
            End If
            If Map(TestVarX, TestVarY) = 0 Then
                TestResult += 1
            ElseIf TestResult = 1 Then
                TestResult += 1 'this ensures you can't see through one wall
            End If
        End While
        Return TestResult
    End Function
    Function IsVisible2(ByVal x As Short, ByVal y As Short) As Short
        Dim TestVarX As Short = PlayerPosX
        Dim TestVarY As Short = PlayerPosY
        Dim TestResult As Short = 0
        While TestVarX <> x Or TestVarY <> y
            If Math.Abs(TestVarY - y) - Math.Abs(TestVarX - x) >= 0 Then 'will change x first, it is the greatest off
                If TestVarY < y Then
                    TestVarY += 1
                ElseIf TestVarY > y Then
                    TestVarY -= 1
                End If
            Else
                If TestVarX < x Then
                    TestVarX += 1
                ElseIf TestVarX > x Then
                    TestVarX -= 1
                End If
            End If
            If Map(TestVarX, TestVarY) = 0 Then
                TestResult += 1
            ElseIf TestResult = 1 Then
                TestResult += 1 'this ensures you can't see through one wall
            End If
        End While
        Return TestResult
    End Function
#End Region
#Region "Initialize"
    Private Sub Initialize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'resize windows
        Dim Oldscreenheight As Integer = Me.Height 'used to distinguish the correct layout of the width
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height - 50 'arranges the height to the screen, assorting tiles to perspective size
        Me.Width += Me.Height - Oldscreenheight 'ensures the width is correspondant to the height
        Me.CenterToScreen() 'center to the screen
        TheRoomHeight = Math.Round(Me.Height / (MapSize + 2), 0)  'test the room height
        TheRoomWidth = Math.Round(Me.Width / (MapSize + 2), 0)  'test the room width
        If TheRoomHeight > TheRoomWidth Then TheRoomHeight = TheRoomWidth 'ensures that the window is scaled to the smallest of the two
        If TheRoomWidth > TheRoomHeight Then TheRoomWidth = TheRoomHeight 'ensures that the window is scaled to the smallest of the two
        Panel1.Left = Me.Width - Panel1.Width - 15 'sorts the panels width to the width of the window
        HealthBar.Left = Me.Width - Panel1.Width - 10 'arranges the healthbar
        WillpowerBar.Left = Me.Width - Panel1.Width - 10 'arrange the willpowerbar according to the panel
        StatBox.Left = Me.Width - Panel1.Width - 10 'arrange the statbox according tot he panel
        Comment1.Left = (Me.Width / 2) - (Comment1.Width / 2) : Comment1.Top = Me.Height - Comment1.Height - 30
        Comment2.Left = (Me.Width / 2) - (Comment2.Width / 2) : Comment2.Top = Me.Height - Comment2.Height - 30
        Comment3.Left = (Me.Width / 2) - (Comment3.Width / 2) : Comment3.Top = Me.Height - Comment3.Height - 30
        Comment4.Left = (Me.Width / 2) - (Comment4.Width / 2) : Comment4.Top = Me.Height - Comment4.Height - 30
        Comment5.Left = (Me.Width / 2) - (Comment5.Width / 2) : Comment5.Top = Me.Height - Comment5.Height - 30
        Comment6.Left = (Me.Width / 2) - (Comment6.Width / 2) : Comment6.Top = Me.Height - Comment6.Height - 30
        Comment7.Left = (Me.Width / 2) - (Comment7.Width / 2) : Comment7.Top = Me.Height - Comment7.Height - 30
        Comment8.Left = (Me.Width / 2) - (Comment8.Width / 2) : Comment8.Top = Me.Height - Comment8.Height - 30
        Comment9.Left = (Me.Width / 2) - (Comment9.Width / 2) : Comment9.Top = Me.Height - Comment9.Height - 30
        Comment10.Left = (Me.Width / 2) - (Comment10.Width / 2) : Comment10.Top = Me.Height - Comment10.Height - 30
        Comment11.Left = (Me.Width / 2) - (Comment11.Width / 2) : Comment11.Top = Me.Height - Comment11.Height - 30
        'test display fonts
        displayfont = New Font("Arial", -4 + (TheRoomHeight + TheRoomWidth / 2) / 2)
        'end resize windows
        UpdateSkills()
        PlayerDefense = Math.Round(PlayerCON / 5, 0)
        PlayerAttack = Math.Round(PlayerSTR / 5, 0)
        PreviousDefense = PlayerDefense
        PreviousAttack = PlayerAttack
        HealthBar.Caption = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints)) + " HP"
        HealthBar.Value = PlayerCurHitpoints
        HealthBar.Max = PlayerHitpoints
        WillpowerBar.Caption = LTrim(Str(PlayerCurWillpower)) + " / " + LTrim(Str(PlayerWillpower)) + " WP"
        WillpowerBar.Value = PlayerCurWillpower
        WillpowerBar.Max = PlayerWillpower
        SkillInfoBox.Left = Panel1.Left - SkillInfoBox.Width
        BuildNewMap()
        SND("Press '?' or 'h' for help.")
        SND("You ascend to depth 1.")
        If My.Computer.FileSystem.FileExists(CurDir() + "\HighScores.TG") Then 'this saves the location of the database for future reference if not found in correct spot
            HighScores = GetFileContents(CurDir() + "\HighScores.TG")
        Else
            SaveTextToFile("[Name]              [Race]        [Class]          [Level] [Experience] [Depth]      [Gold]    [Turns]" + Chr(13) + "Jarvis              Gnome         Gravedigger      4       45           6            141       1690", CurDir() + "\HighScores.TG", , True)
            HighScores = GetFileContents(CurDir() + "\HighScores.TG")
        End If
    End Sub
#End Region
#Region "Build Map"
    Private Sub BuildNewMap()
        CANVAS.FillRectangle(Brushes.Black, 1, 1, 1200, 1200)
        Dim RefreshShownMapX, RefreshShownMapY As Short
        For RefreshShownMapX = 0 To MapSize Step 1
            For RefreshShownMapY = 0 To MapSize Step 1
                MapShown(RefreshShownMapX, RefreshShownMapY) = False
                MapDrawStatus(RefreshShownMapX, RefreshShownMapY) = Hidden
                MapDrawStatusPlus(RefreshShownMapX, RefreshShownMapY) = 0
            Next
        Next
        DetermineEnvironment()
        GenerateMap(8)
        GenerateFog()
        GenerateBlur()
        PopulateItems()
        PopulateMobiles()
        PopulateEntrances()
        ReDraw()
    End Sub
    Sub GenerateBlur()
        For x = 0 To MapSize Step 1
            For y = 0 To MapSize Step 1
                If Map(x, y) = Wall Then
                    If x > 0 Then
                        If Map(x - 1, y) <> Wall Then MapBlur(x, y, 3) = True Else MapBlur(x, y, 3) = False
                    End If
                    If x < MapSize Then
                        If Map(x + 1, y) <> Wall Then MapBlur(x, y, 1) = True Else MapBlur(x, y, 1) = False
                    End If
                    If y > 0 Then
                        If Map(x, y - 1) <> Wall Then MapBlur(x, y, 2) = True Else MapBlur(x, y, 2) = False
                    End If
                    If y < MapSize Then
                        If Map(x, y + 1) <> Wall Then MapBlur(x, y, 0) = True Else MapBlur(x, y, 0) = False
                    End If
                End If
            Next
        Next
    End Sub
    Sub PopulateEntrances()
        Dim RandomNum As New Random
        Dim RandomPosX As Short = RandomNum.Next(1, MapSize - 1)
        Dim RandomPosY As Short = RandomNum.Next(1, MapSize - 1)
        Dim EntrancePosX, EntrancePosY As Short
        Dim Foundentrance, Foundexit As Boolean
        Dim Tries As Short = 0
        While Foundentrance = False
            Tries += 1
            If Tries > 1000 Then
                'no place to put the item, recursion too high, exit (catch)
                Exit While
            End If
            If Map(RandomPosX, RandomPosY) = 1 Then
                Foundentrance = True
                If MapLevel >= 2 Then 'ensures that the player can't go to levels before 1
                    'Map(RandomPosX, RandomPosY) = 2 'uncomment this to allow stairs up
                    'EntrancePosX = RandomPosX 'uncomment this to allow stairs up
                    'EntrancePosY = RandomPosY 'uncomment this to allow stairs up
                End If
                PlayerPosX = RandomPosX
                PlayerPosY = RandomPosY
                RandomPosX = RandomNum.Next(1, MapSize - 1) 'not necessary if stairs up is allowed, prevents stairs down from spawning on player
                RandomPosY = RandomNum.Next(1, MapSize - 1) 'not necessary if stairs up is allowed, prevents stairs down from spawning on player
            Else
                RandomPosX = RandomNum.Next(1, MapSize - 1)
                RandomPosY = RandomNum.Next(1, MapSize - 1)
            End If
        End While
        If MapLevel = 28 Then Foundexit = True 'don't allow stairs on the last level
        Tries = 0
        While Foundexit = False
            Tries += 1
            If Tries > 1000 Then
                'no place to put the item, recursion too high, exit (catch)
                Exit While
            End If
            If Map(RandomPosX, RandomPosY) = 1 Then
                If Math.Abs(RandomPosX - EntrancePosX) >= 5 Or Math.Abs(RandomPosY - EntrancePosY) >= 5 Then
                    Map(RandomPosX, RandomPosY) = 3
                    Foundexit = True
                Else
                    RandomPosX = RandomNum.Next(1, MapSize - 1)
                    RandomPosY = RandomNum.Next(1, MapSize - 1)
                End If
            Else
                RandomPosX = RandomNum.Next(1, MapSize - 1)
                RandomPosY = RandomNum.Next(1, MapSize - 1)
            End If
        End While
    End Sub
    Sub DetermineEnvironment()
        Dim RandomNum As New Random
        Dim RandomEnvironment As Short = RandomNum.Next(0, 10)
        MapLevel += 1
        If MapLevel < 4 Then
            EnvironmentType = 0
        ElseIf MapLevel < 7 Then
            EnvironmentType = 1
        ElseIf MapLevel < 10 Then
            EnvironmentType = 2
        ElseIf MapLevel < 13 Then
            EnvironmentType = 3
        ElseIf MapLevel < 16 Then
            EnvironmentType = 4
        ElseIf MapLevel < 19 Then
            EnvironmentType = 5
        ElseIf MapLevel < 22 Then
            EnvironmentType = 6
        ElseIf MapLevel < 25 Then
            EnvironmentType = 7
        ElseIf MapLevel < 28 Then
            EnvironmentType = 8
        ElseIf MapLevel < 31 Then
            EnvironmentType = 9
        Else
            EnvironmentType = RandomEnvironment
        End If
        If MapLevel = 1 Then
            Comment1.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 4 Then
            Comment2.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 7 Then
            Comment3.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 10 Then
            Comment4.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 13 Then
            Comment5.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 16 Then
            Comment6.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 19 Then
            Comment7.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 22 Then
            Comment8.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 25 Then
            Comment9.Visible = True : CommentBoxOpen = True
        ElseIf MapLevel = 28 Then
            Comment10.Visible = True : CommentBoxOpen = True
        End If
    End Sub
    Sub PopulateItems()
        Dim RandomNum As New Random
        Dim RandomPosX As Short = RandomNum.Next(1, MapSize - 1)
        Dim RandomPosY As Short = RandomNum.Next(1, MapSize - 1)
        Dim RandomItemType As Short = RandomNum.Next(1, 6)
        Dim ItemNumber As Short 'otherwise known in other parts as ItemNum
        Dim FoundPosition As Boolean = False
        Dim MaxItems As Short = Math.Round(PlayerLUC / 2, 0)
        'clear previous map occupied first
        For RandomPosX = 0 To MapSize Step 1
            For RandomPosY = 0 To MapSize Step 1
                ItemOccupied(RandomPosX, RandomPosY) = 0
            Next
        Next
        'initiate population
        Dim Tries As Short = 0
        For ItemNumber = 0 To MaxItems Step 1
            FoundPosition = False
            ItemNum(ItemNumber) = ItemNumber
            Tries = 0
            While FoundPosition = False
                Tries += 1
                If Tries > 1000 Then
                    'no place to put the item, recursion too high, exit (catch)
                    Exit While
                End If
                RandomPosX = RandomNum.Next(1, MapSize - 1) 'don't want to start a mobile on the edge of the map... just because it doesn't look pretty
                RandomPosY = RandomNum.Next(1, MapSize - 1) 'don't want to start a mobile on the edge of the map... just because it doesn't look pretty
                If Map(RandomPosX, RandomPosY) = 1 Then 'if map sector is floor (can't draw items onto a wall.. that's just silly)
                    ItemOccupied(RandomPosX, RandomPosY) = ItemNum(ItemNumber)
                    GenerateItem.GenerateRandomItem(ItemNumber)
                    'Public NameType As String
                    'Public ItemType As Short
                    'Public ShowType As String
                    ItemType(ItemNumber) = GenerateItem.ItemType
                    ItemShowType(RandomPosX, RandomPosY) = GenerateItem.ShowType
                    ItemNameType(RandomPosX, RandomPosY) = GenerateItem.NameType
                    If MapLevel = 28 And ItemNumber = MaxItems Then 'the reason we show it on item nine is because i allowed items to spawn over each other, this is
                        ItemType(ItemNumber) = TheEverspark 'an easy way to ensure that there's not always 10 items.
                    End If
                    FoundPosition = True
                    If LTrim(GenerateItem.NameType) = "" Then 'this prevents stringless items which occur rarely.. remove when bug is found in generate item
                        ItemOccupied(RandomPosX, RandomPosY) = 0
                    End If
                End If
            End While
        Next
    End Sub
    Sub PopulateMobiles()
        Dim RandomNum As New Random
        Dim RandomPosX As Short = RandomNum.Next(1, MapSize - 1) 'don't want to start a mobile on the edge of the map... just because it doesn't look pretty
        Dim RandomPosY As Short = RandomNum.Next(1, MapSize - 1) 'don't want to start a mobile on the edge of the map... just because it doesn't look pretty
        Dim RandomMobType As Short = RandomNum.Next(EnvironmentType, EnvironmentType + 4) 'there are three possibilities of mobiles for each environment type, getting progressively harder
        Dim MobileNumber As Short 'otherwise known in other parts as MobNum
        Dim FoundPosition As Boolean = False
        'clear previous map occupied first
        For RandomPosX = 0 To MapSize Step 1
            For RandomPosY = 0 To MapSize Step 1
                MapOccupied(RandomPosX, RandomPosY) = 0
            Next
        Next
        'initiate population
        Dim Tries As Short = 0
        For MobileNumber = 0 To 9 Step 1
            FoundPosition = False
            RandomMobType = RandomNum.Next(EnvironmentType, EnvironmentType + 4)
            Tries = 0
            While FoundPosition = False
                Tries += 1
                If Tries > 1000 Then
                    'no place to put the item, recursion too high, exit (catch)
                    Exit While
                End If
                RandomPosX = RandomNum.Next(1, MapSize - 1) 'don't want to start a mobile on the edge of the map... just because it doesn't look pretty
                RandomPosY = RandomNum.Next(1, MapSize - 1) 'don't want to start a mobile on the edge of the map... just because it doesn't look pretty
                If Map(RandomPosX, RandomPosY) = 1 Then
                    MapOccupied(RandomPosX, RandomPosY) = RandomMobType 'assign mobiles random type
                    MobilePosX(MobileNumber) = RandomPosX : MobilePosY(MobileNumber) = RandomPosY
                    MobilePrevX(MobileNumber) = RandomPosX : MobilePrevY(MobileNumber) = RandomPosY
                    MobileType(MobileNumber) = RandomMobType
                    If RandomMobType = 1 Then 'assign the mobiles health depending on their type
                        MobileHealth(MobileNumber) = 2 + MapLevel
                    ElseIf RandomMobType = 2 Then
                        MobileHealth(MobileNumber) = 2 + MapLevel
                    ElseIf RandomMobType = 3 Then
                        MobileHealth(MobileNumber) = 3 + MapLevel
                    ElseIf RandomMobType = 4 Then
                        MobileHealth(MobileNumber) = 3 + MapLevel
                    ElseIf RandomMobType = 5 Then
                        MobileHealth(MobileNumber) = 3 + MapLevel
                    ElseIf RandomMobType = 6 Then
                        MobileHealth(MobileNumber) = 5 + MapLevel
                    ElseIf RandomMobType = 7 Then
                        MobileHealth(MobileNumber) = 5 + MapLevel
                    ElseIf RandomMobType = 8 Then
                        MobileHealth(MobileNumber) = 5 + MapLevel
                    ElseIf RandomMobType = 9 Then
                        MobileHealth(MobileNumber) = 5 + MapLevel
                    ElseIf RandomMobType = 10 Then
                        MobileHealth(MobileNumber) = 10 + MapLevel
                    ElseIf RandomMobType = 11 Then
                        MobileHealth(MobileNumber) = 10 + MapLevel
                    ElseIf RandomMobType = 12 Then
                        MobileHealth(MobileNumber) = 10 + MapLevel
                    ElseIf RandomMobType = 13 Then
                        MobileHealth(MobileNumber) = 10 + MapLevel
                    Else 'this is a catch to ensure that mobile types stay within known bounds in case environments are ever added, set all future mobiles
                        MobileHealth(MobileNumber) = 1 'to retain the same hitpoints as the rat (1)
                        MobileType(MobileNumber) = 1
                        MapOccupied(RandomPosX, RandomPosY) = 1
                    End If
                    FoundPosition = True
                End If
            End While
        Next
    End Sub
    Sub GenerateMap(ByVal Recursion As Short)
        Dim RepeatToRecursion As Short = 0
        Dim RandomNumber As New Random
        Dim BuilderDirection As Short = 0
        Dim BuilderLastDirection As Short = 0
        Dim BuilderGrowthAmount As Short 'this is the actual amount that will be grown, not the potential
        Dim BuilderPositionX As Short = RandomNumber.Next(2, MapSize)
        Dim BuilderPositionY As Short = RandomNumber.Next(2, MapSize)
        Dim Turns As Short = 7
        Dim Stops = 0
        Dim StopWhile = 0
        Dim PotentialGrowth As Short = 0 'this number is the potential size the room or corridor can be in that direction
        Dim PotentialSides As Short = 0
        Dim GenerateRandomType = GenerateType
        If GenerateRandomType = 3 Then
            GenerateRandomType = RandomNumber.Next(0, 2) 'returns 0:Dungeon,1:Ruins..alternatively once finished 2:Classic
        End If
        If GenerateRandomType = Dungeon Then
            For RepeatToRecursion = 1 To Recursion Step 1
                Map(BuilderPositionX, BuilderPositionY) = 1
                While Turns > 0
                    PotentialGrowth = 0
                    '-------Pick Random Direction---------
                    BuilderDirection = RandomNumber.Next(1, 5) 'There are 4 directions
                    While BuilderDirection = BuilderLastDirection
                        BuilderDirection = RandomNumber.Next(1, 5) 'Sorry can't pick the last direction you grew
                        StopWhile += 1
                        If StopWhile = 10 Then
                            Stops += 1
                            If Stops > 100 Then Exit While
                            Exit While
                        End If
                    End While
                    StopWhile = 0
                    If BuilderDirection = North And BuilderPositionY > 2 Then 'Verifying there is room for growth in that direction, at least 2 spaces
                        PotentialGrowth = BuilderPositionY - 1
                        If Map(BuilderPositionX, BuilderPositionY - 1) = 1 Then 'already carved, no need to proceed
                            PotentialGrowth = -1 : Stops += 1
                        End If
                    ElseIf BuilderDirection = East And BuilderPositionX < MapSize - 2 Then 'Veryifying there is room for growth in that direction, at least 2 spaces
                        PotentialGrowth = MapSize - BuilderPositionX - 1
                        If Map(BuilderPositionX + 1, BuilderPositionY) = 1 Then 'already carved, no need to proceed
                            PotentialGrowth = -1 : Stops += 1
                        End If
                    ElseIf BuilderDirection = South And BuilderPositionY < MapSize - 2 Then 'Verifying there is room for growth in that direction, at least 2 spaces
                        PotentialGrowth = MapSize - BuilderPositionY - 1
                        If Map(BuilderPositionX, BuilderPositionY + 1) = 1 Then 'already carved, no need to proceed
                            PotentialGrowth = -1 : Stops += 1
                        End If
                    ElseIf BuilderDirection = West And BuilderPositionX > 2 Then 'Verifying there is room for growth in that direction, at least 2 spaces
                        PotentialGrowth = BuilderPositionX - 1
                        If Map(BuilderPositionX - 1, BuilderPositionY) = 1 Then 'already carved, no need to proceed
                            PotentialGrowth = -1 : Stops += 1
                        End If
                    End If
                    If PotentialGrowth > 4 Then
                        BuilderLastDirection = BuilderDirection
                        BuilderGrowthAmount = RandomNumber.Next(1, PotentialGrowth) 'growth includes wall, can't draw to end of map so just use potential growth since it's exclusive instead of inclusive
                        Map(BuilderPositionX + 1, BuilderPositionY) = Floor
                        Map(BuilderPositionX - 1, BuilderPositionY) = Floor
                        Map(BuilderPositionX, BuilderPositionY + 1) = Floor
                        Map(BuilderPositionX, BuilderPositionY - 1) = Floor
                        PotentialSides = RandomNumber.Next(1, 3)
                        If PotentialSides = 1 Then
                            Map(BuilderPositionX + 1, BuilderPositionY + 1) = Floor
                        End If
                        PotentialSides = RandomNumber.Next(1, 3)
                        If PotentialSides = 1 Then
                            Map(BuilderPositionX - 1, BuilderPositionY - 1) = Floor
                        End If
                        PotentialSides = RandomNumber.Next(1, 3)
                        If PotentialSides = 1 Then
                            Map(BuilderPositionX - 1, BuilderPositionY + 1) = Floor
                        End If
                        PotentialSides = RandomNumber.Next(1, 3)
                        If PotentialSides = 1 Then
                            Map(BuilderPositionX + 1, BuilderPositionY - 1) = Floor
                        End If
                        If BuilderDirection = North Then
                            For BuilderPositionY = BuilderPositionY To BuilderPositionY - BuilderGrowthAmount Step -1
                                Map(BuilderPositionX, BuilderPositionY) = Floor
                                PotentialSides = RandomNumber.Next(1, 11)
                                If PotentialSides = 1 Then 'random chance to draw a floor to the right 10%
                                    Map(BuilderPositionX + 1, BuilderPositionY) = Floor
                                ElseIf PotentialSides = 2 Then 'randomchance to draw a floor to the left 10%
                                    Map(BuilderPositionX - 1, BuilderPositionY) = Floor
                                End If
                            Next
                        ElseIf BuilderDirection = East Then
                            For BuilderPositionX = BuilderPositionX To BuilderPositionX + BuilderGrowthAmount Step 1
                                Map(BuilderPositionX, BuilderPositionY) = Floor
                                PotentialSides = RandomNumber.Next(1, 11)
                                If PotentialSides = 1 Then 'random chance to draw a floor to the north 10%
                                    Map(BuilderPositionX, BuilderPositionY - 1) = Floor
                                ElseIf PotentialSides = 2 Then 'randomchance to draw a floor to the south 10%
                                    Map(BuilderPositionX, BuilderPositionY + 1) = Floor
                                End If
                            Next
                        ElseIf BuilderDirection = South Then
                            For BuilderPositionY = BuilderPositionY To BuilderPositionY + BuilderGrowthAmount Step 1
                                Map(BuilderPositionX, BuilderPositionY) = Floor
                                PotentialSides = RandomNumber.Next(1, 11)
                                If PotentialSides = 1 Then 'random chance to draw a floor to the right 10%
                                    Map(BuilderPositionX + 1, BuilderPositionY) = Floor
                                ElseIf PotentialSides = 2 Then 'randomchance to draw a floor to the left 10%
                                    Map(BuilderPositionX - 1, BuilderPositionY) = Floor
                                End If
                            Next
                        ElseIf BuilderDirection = West Then
                            For BuilderPositionX = BuilderPositionX To BuilderPositionX - BuilderGrowthAmount Step -1
                                Map(BuilderPositionX, BuilderPositionY) = Floor
                                PotentialSides = RandomNumber.Next(1, 11)
                                If PotentialSides = 1 Then 'random chance to draw a floor to the north 10%
                                    Map(BuilderPositionX, BuilderPositionY - 1) = Floor
                                ElseIf PotentialSides = 2 Then 'randomchance to draw a floor to the south 10%
                                    Map(BuilderPositionX, BuilderPositionY + 1) = Floor
                                End If
                            Next
                        End If
                        Turns -= 1
                    Else
                        Stops += 1
                        If Stops > 100 Then
                            Turns = 0
                        End If
                    End If
                    If Stops > 100 Then
                        Turns = 0
                    End If
                End While
                Stops = 0
                StopWhile = 0
                BuilderDirection = 0
                BuilderLastDirection = 0
                Turns = 7
            Next
        ElseIf GenerateRandomType = Ruins Then
            Dim RandomRuin As New Random
            Dim RuinStrength As Short
            Dim MaximumRuins As Short = MapSize
            Dim CurrentRuin As Short = 0
            Dim RuinDispersion As Short
            Dim RuinMaxDispersion As Short = Math.Floor(MapSize / 3)
            'forest starts with a clean slate of floor instead of walls, must paint map first
            For BuilderPositionX = 0 To MapSize Step 1
                For BuilderPositionY = 0 To MapSize Step 1
                    Map(BuilderPositionX, BuilderPositionY) = Floor
                Next
            Next
            For CurrentRuin = 0 To MaximumRuins Step 1
                BuilderPositionX = RandomRuin.Next(0, MapSize)
                BuilderPositionY = RandomRuin.Next(0, MapSize)
                'spiral outwards
                Dim i As Short = 0
                Dim ang As Double = 0
                Dim cX As Long, X As Long
                Dim cy As Long, Y As Long
                Dim e As Double = 2.718281828
                cX = BuilderPositionX
                cy = BuilderPositionY
                RuinStrength = RandomRuin.Next(1, 20)
                RuinDispersion = RandomRuin.Next(0, 24)
                Dim a As Single, b As Single
                a = 0.03 : b = 0.03 'angles, lower is tighter
                For i = 0 To 4 + RuinDispersion 'recursions
                    ang = (Math.PI / 720) * i * 1000
                    X = cX + (a * (Math.Cos(ang)) * (e ^ (b * ang))) * RuinStrength 'Ruin strength increases the distance between diversion the larger it gets
                    Y = cy - (a * (Math.Sin(ang)) * (e ^ (b * ang))) * RuinStrength 'Ruin strength increases the distance between diversion the larger it gets
                    If Math.Floor(X) >= 0 And Math.Floor(X) <= MapSize Then
                        If Math.Floor(Y) >= 0 And Math.Floor(Y) <= MapSize Then
                            Map(Math.Floor(X), Math.Floor(Y)) = Wall
                        End If
                    End If
                Next
                'end spiral outwards
            Next
            'Pathing Closed Squares, keeping consistant space open and closing unaccessible space, required to path square
            'this can take immense cpu in LARGE maps, carefully trod here.
            Dim OccupiedSquares(9) As Short 'might need to be integer if it's a LARGE map, highly suggested against though
            Dim CurrentOccupied As Short = 1
            Dim StartPositionFound As Boolean = False
            Dim MapTrod(MapSize, MapSize) As Short
            Dim MapLevel(MapSize, MapSize) As Short
            Dim TrodLevel As Short = 1
            Dim TrodTry As Boolean 'used in while statement to test in a circle , then +1, +2, up to +5 then exits the occupied space and goes to next space
            Dim TrodHead As Boolean
            Dim BuilderTestPosX, BuilderTestPosY As Short
            For BuilderPositionX = 0 To MapSize Step 1
                For BuilderPositionY = 0 To MapSize Step 1
                    If StartPositionFound = False And CurrentOccupied < 10 Then 'finding a location to start searching
                        If Map(BuilderPositionX, BuilderPositionY) = Floor And MapTrod(BuilderPositionX, BuilderPositionY) = 0 Then
                            StartPositionFound = True
                            MapTrod(BuilderPositionX, BuilderPositionY) += 1
                            BuilderTestPosX = BuilderPositionX
                            BuilderTestPosY = BuilderPositionY
                            MapLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                            ItemOccupied(BuilderTestPosX, BuilderTestPosY) = 1
                            ItemShowType(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                        End If
                    ElseIf CurrentOccupied = 20 Then
                        Map(BuilderPositionX, BuilderPositionY) = Wall 'positions filled, no more searching, fill rest with walls
                    Else 'already searching a location
                        TrodHead = True
                        While TrodHead = True
                            TrodTry = False : TrodLevel = 0
                            While TrodTry = False
                                'test up
                                If BuilderTestPosY > 0 Then 'ensure it's within bounds
                                    If Map(BuilderTestPosX, BuilderTestPosY - 1) = Floor Or Map(BuilderTestPosX, BuilderTestPosY - 1) = StairsUp Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX, BuilderTestPosY - 1) = TrodLevel Then 'found next trod
                                            BuilderTestPosY -= 1
                                            If MapTrod(BuilderTestPosX, BuilderTestPosY) <= 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            'ItemOccupied(BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test right
                                If BuilderTestPosX < MapSize Then 'ensure it's within bounds
                                    If Map(BuilderTestPosX + 1, BuilderTestPosY) = Floor Or Map(BuilderTestPosX + 1, BuilderTestPosY) = StairsUp Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX + 1, BuilderTestPosY) = TrodLevel Then 'found next trod
                                            BuilderTestPosX += 1
                                            If MapTrod(BuilderTestPosX, BuilderTestPosY) <= 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            Map(BuilderTestPosX, BuilderTestPosY) = StairsUp
                                            'ItemOccupied(BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test down
                                If BuilderTestPosY < MapSize Then 'ensure it's within bounds
                                    If Map(BuilderTestPosX, BuilderTestPosY + 1) = Floor Or Map(BuilderTestPosX, BuilderTestPosY + 1) = StairsUp Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX, BuilderTestPosY + 1) = TrodLevel Then 'found next trod
                                            BuilderTestPosY += 1
                                            If MapTrod(BuilderTestPosX, BuilderTestPosY) <= 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            Map(BuilderTestPosX, BuilderTestPosY) = StairsUp
                                            'ItemOccupied(BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test left
                                If BuilderTestPosX > 0 Then 'ensure it's within bounds
                                    If Map(BuilderTestPosX - 1, BuilderTestPosY) = Floor Or Map(BuilderTestPosX - 1, BuilderTestPosY) = StairsUp Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX - 1, BuilderTestPosY) = TrodLevel Then 'found next trod
                                            BuilderTestPosX -= 1
                                            If MapTrod(BuilderTestPosX, BuilderTestPosY) <= 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            Map(BuilderTestPosX, BuilderTestPosY) = StairsUp
                                            'ItemOccupied(BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'none found that's untrodden, rinse and repeat with trodden +1
                                If TrodLevel = 20 Then 'trodlevel of occupied space is too high, exit current space
                                    CurrentOccupied += 1
                                    TrodHead = False
                                    TrodTry = True
                                    StartPositionFound = False
                                End If
                                TrodLevel += 1
                            End While
                            If CurrentOccupied = 20 Then
                                Exit While 'occupied levels are too high, exit all spaces
                            End If
                        End While
                    End If
                Next
            Next
            'now clear all states that aren't the largest and fill them with walls
            Dim CurrentLargest = 0
            Dim Wallnumber As Short = 0
            For CurInt = 1 To 9 Step 1
                If OccupiedSquares(CurInt) > OccupiedSquares(CurrentLargest) Then CurrentLargest = CurInt
            Next
            For MapStepX = 0 To MapSize Step 1
                For MapStepY = 0 To MapSize Step 1
                    If MapLevel(MapStepX, MapStepY) = CurrentLargest Then
                        Map(MapStepX, MapStepY) = Floor
                        Wallnumber += 1
                    Else
                        Map(MapStepX, MapStepY) = Wall
                    End If
                Next
            Next
        ElseIf GenerateRandomType = Classic Then
            Dim AllocatedBlocks As Short = 0
            Dim RandomPosition As New Random
            Dim BuilderSpawned As Boolean = False
            Dim BuilderMoveDirection As Short = 0
            Dim PawnLocationX As Short = Math.Floor(MapSize / 2)
            Dim PawnLocationY As Short = Math.Floor(MapSize / 2)
            While AllocatedBlocks < MapSize / 2
                If BuilderSpawned = False Then
                    'spawn at random position
                    BuilderPositionX = RandomPosition.Next(0, MapSize)
                    BuilderPositionY = RandomPosition.Next(0, MapSize)
                    'see if spawn is within 1 block of pawn after spawn
                    If Math.Abs(PawnLocationX - BuilderPositionX) <= 1 And Math.Abs(PawnLocationY - BuilderPositionY) <= 1 Then
                        'builder was spawned too close to spawn, clear that floor and respawn
                        Map(BuilderPositionX, BuilderPositionY) = Floor
                    Else
                        BuilderSpawned = True
                        BuilderMoveDirection = RandomPosition.Next(1, 5)
                    End If
                Else 'builderalready spawned and knows it's direction, move the builder
                    'move the builder
                    If BuilderMoveDirection = North And BuilderPositionY > 0 Then
                        BuilderPositionY -= 1
                    ElseIf BuilderMoveDirection = East And BuilderPositionX < MapSize Then
                        BuilderPositionX += 1
                    ElseIf BuilderMoveDirection = South And BuilderPositionY < MapSize Then
                        BuilderPositionY += 1
                    ElseIf BuilderMoveDirection = West And BuilderPositionX > 0 Then
                        BuilderPositionX -= 1
                    Else
                        'if it wasn't passed it must either be an error or near the side of the map
                        'so go ahead and respawn
                        BuilderSpawned = False
                    End If
                    'see whether the builder is near an exit or near a existing spot

                End If
            End While
        End If
    End Sub
    Sub GenerateFog()
        'declare noise dimensions
        Dim noiseWidth As Integer = MapSize
        Dim noiseHeight As Integer = MapSize
        Dim noiseDepth As Integer = 8
        Dim RandomNoise As New Random
        Dim noiseData(noiseWidth, noiseHeight, noiseDepth) As Short
        Dim noiseFloor(noiseWidth, noiseHeight) As Short
        'Generate Noise
        Dim CurDepth As Short
        For BuilderPositionX = 0 To MapSize
            For BuilderPositionY = 0 To MapSize
                For CurDepth = 0 To noiseDepth
                    noiseData(BuilderPositionX, BuilderPositionY, CurDepth) = (RandomNoise.Next(0, 5) Mod 5)
                    If noiseData(BuilderPositionX, BuilderPositionY, CurDepth) = 0 Then
                        noiseFloor(BuilderPositionX, BuilderPositionY) += 1
                    End If
                Next
            Next
        Next
        'Smooth Noise                 
        'function SmoothNoise_2D(x, y)
        '     corners = (Noise(x - 1, y - 1) + Noise(x + 1, y - 1) + Noise(x - 1, y + 1) + Noise(x + 1, y + 1)) / 16
        '     sides =   (Noise(x - 1, y) + Noise(x + 1, y) + Noise(x, y - 1) + Noise(x, y + 1)) / 8
        '     center =   Noise(x, y) / 4
        'Return corners + sides + center
        Dim Corner(3) As Short
        Dim Side(3) As Short
        Dim CurNoise As Short
        Dim AddNoise As Short
        For BuilderPositionX = 0 To MapSize
            For BuilderPositionY = 0 To MapSize
                CurNoise = 0
                'determine whether it's high priority block, if so add 1 to all surrounding squares, alternative interpolation, prioritize height
                AddNoise = 0
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 0 Then AddNoise = -3
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 1 Then AddNoise = 0
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 2 Then AddNoise = -1
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 3 Then AddNoise = 0
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 4 Then AddNoise = 0
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 5 Then AddNoise = 1
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 6 Then AddNoise = 1
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 7 Then AddNoise = 2
                If noiseFloor(BuilderPositionX, BuilderPositionY) = 8 Then AddNoise = 3
                'calculate the corners
                If BuilderPositionX > 0 And BuilderPositionY > 0 Then 'topleft corner
                    Corner(0) = noiseFloor(BuilderPositionX - 1, BuilderPositionY - 1)
                    If AddNoise > 0 Then noiseFloor(BuilderPositionX - 1, BuilderPositionY - 1) += AddNoise
                Else 'if already on topleft, take current spot
                    Corner(0) = noiseFloor(BuilderPositionX, BuilderPositionY)
                End If
                If BuilderPositionX < MapSize And BuilderPositionY > 0 Then 'topright corner
                    Corner(1) = noiseFloor(BuilderPositionX + 1, BuilderPositionY - 1)
                    If AddNoise > 0 Then noiseFloor(BuilderPositionX + 1, BuilderPositionY - 1) += AddNoise
                Else 'if already on topleft, take current spot
                    Corner(1) = noiseFloor(BuilderPositionX, BuilderPositionY)
                End If
                If BuilderPositionX > 0 And BuilderPositionY < MapSize Then 'bottom left corner
                    Corner(2) = noiseFloor(BuilderPositionX - 1, BuilderPositionY + 1)
                    If AddNoise > 0 Then noiseFloor(BuilderPositionX - 1, BuilderPositionY + 1) += AddNoise
                Else
                    Corner(2) = noiseFloor(BuilderPositionX, BuilderPositionY)
                End If
                If BuilderPositionX < MapSize And BuilderPositionY < MapSize Then 'bottomright corner
                    Corner(3) = noiseFloor(BuilderPositionX + 1, BuilderPositionY + 1)
                    If AddNoise > 0 Then noiseFloor(BuilderPositionX + 1, BuilderPositionY + 1) += AddNoise
                Else
                    Corner(3) = noiseFloor(BuilderPositionX, BuilderPositionY)
                End If
                'calculate the sides
                If BuilderPositionX > 0 Then 'left side
                    Side(0) = noiseFloor(BuilderPositionX - 1, BuilderPositionY)
                    If AddNoise > 0 Then noiseFloor(BuilderPositionX - 1, BuilderPositionY) += AddNoise
                Else
                    Side(0) = noiseFloor(BuilderPositionX, BuilderPositionY)
                End If
                If BuilderPositionY > 0 Then 'top side
                    Side(1) = noiseFloor(BuilderPositionX, BuilderPositionY - 1)
                    If AddNoise > 0 Then noiseFloor(BuilderPositionX, BuilderPositionY - 1) += AddNoise
                Else
                    Side(1) = noiseFloor(BuilderPositionX, BuilderPositionY)
                End If
                If BuilderPositionX < MapSize Then 'right side
                    Side(2) = noiseFloor(BuilderPositionX + 1, BuilderPositionY)
                    If AddNoise > 0 Then noiseFloor(BuilderPositionX + 1, BuilderPositionY) += AddNoise
                Else
                    Side(2) = noiseFloor(BuilderPositionX, BuilderPositionY)
                End If
                If BuilderPositionY < MapSize Then 'bottom side
                    Side(3) = noiseFloor(BuilderPositionX, BuilderPositionY + 1)
                    If AddNoise > 0 Then noiseFloor(BuilderPositionX, BuilderPositionY + 1) += AddNoise
                Else
                    Side(3) = noiseFloor(BuilderPositionX, BuilderPositionY)
                End If
                'get the average noise of all sides, corners, and current spot
                CurNoise = (Corner(0) + Corner(1) + Corner(2) + Corner(3)) / 16
                CurNoise += (Side(0) + Side(1) + Side(2) + Side(3)) / 8
                CurNoise += noiseFloor(BuilderPositionX, BuilderPositionY) / 4
                'apply new noise average
                noiseFloor(BuilderPositionX, BuilderPositionY) = CurNoise
                FogMap(BuilderPositionX, BuilderPositionY) = noiseFloor(BuilderPositionX, BuilderPositionY) 'show the current state number, debug
            Next
        Next
    End Sub
#End Region
#Region "Tick"
    Sub ReDraw() 'also known as 'tick'
        Dim ProcessMobilePathNumber As Short = 0
        For ProcessMobilePathNumber = 0 To 9 Step 1
            If Silence <= 0 Then
                If MobileStun(ProcessMobilePathNumber) <= 0 Then
                    If MobileHealth(ProcessMobilePathNumber) > 0 Then
                        DetermineMobMov(ProcessMobilePathNumber)
                    End If
                Else
                    SND("Stunned enemy struggles.")
                    MobileStun(ProcessMobilePathNumber) -= 1
                End If
            End If
        Next
        SKillGobalCooldown -= 1
        If SKillGobalCooldown <= 0 Then
            SKillGobalCooldown = 0
            UpdateSkills()
        End If
        Silence -= 1 : If Silence <= 0 Then Silence = 0 Else SND("The room remains pacified.")
        PlayerHidden -= 1 : If PlayerHidden <= 0 Then PlayerHidden = 0
        BoneShield -= 1 : If BoneShield <= 0 Then BoneShield = 0
        MagicShield -= 1 : If MagicShield <= 0 Then MagicShield = 0
        CounterAttack -= 1 : If CounterAttack <= 0 Then CounterAttack = 0
        Immolate -= 1 : If Immolate <= 0 Then Immolate = 0
        Fury -= 1 : If Fury <= 0 Then Fury = 0
        Block -= 1 : If Block <= 0 Then Block = 0
        Dim xish, xishPLUS, yish, yishPLUS As Integer
        Dim RandomNum As New Random
        'lets reduce redundancy
        Dim WallArt As Bitmap
        Dim FloorArt As Bitmap
        'define bounds, start x visible area -1 to x visible area +1, same with y, no need to check whole map, it's already written
        'just make sure you check where player is moving and where he could have moved to see if tiles need to be replaced.
        Dim StartX As Short = PlayerPosX - 5 : If StartX < 0 Then StartX = 0
        Dim StartY As Short = PlayerPosY - 5 : If StartY < 0 Then StartY = 0
        Dim FinishX As Short = PlayerPosX + 5 : If FinishX > MapSize Then FinishX = MapSize
        Dim FinishY As Short = PlayerPosY + 5 : If FinishY > MapSize Then FinishY = MapSize
        'start at top left and go to bottom right
        For x = StartX To FinishX Step 1
            For y = StartY To FinishY Step 1
                'sets wall and floor art to prevent redundancy later, easier to read
                If EnvironmentType = 0 Then : WallArt = My.Resources._1 : FloorArt = My.Resources._1floor()
                ElseIf EnvironmentType = 1 Then : WallArt = My.Resources._2 : FloorArt = My.Resources._2floor()
                ElseIf EnvironmentType = 2 Then : WallArt = My.Resources._3 : FloorArt = My.Resources._3floor()
                ElseIf EnvironmentType = 3 Then : WallArt = My.Resources._4 : FloorArt = My.Resources._4floor()
                ElseIf EnvironmentType = 4 Then : WallArt = My.Resources._5 : FloorArt = My.Resources._5floor()
                ElseIf EnvironmentType = 5 Then : WallArt = My.Resources._6 : FloorArt = My.Resources._6floor()
                ElseIf EnvironmentType = 6 Then : WallArt = My.Resources._7 : FloorArt = My.Resources._7floor()
                ElseIf EnvironmentType = 7 Then : WallArt = My.Resources._8 : FloorArt = My.Resources._8floor()
                ElseIf EnvironmentType = 8 Then : WallArt = My.Resources._9 : FloorArt = My.Resources._9floor()
                ElseIf EnvironmentType = 9 Then : WallArt = My.Resources._10 : FloorArt = My.Resources._10floor()
                End If
                xish = TheRoomWidth * x + ColumnsSpace * x + 10
                yish = TheRoomHeight * y + RowSpace * y + 10
                xishPLUS = Val(TheRoomWidth) * x + Val(ColumnsSpace) * x + 10 + Val(TheRoomWidth)
                yishPLUS = Val(TheRoomHeight) * y + Val(RowSpace) * y + 10 + Val(TheRoomHeight)
                If Math.Abs((PlayerPosX + PlayerPosY) - (x + y)) <= 4 And Math.Abs((PlayerPosX - PlayerPosY) - (x - y)) <= 4 Or AdminVisible = True Then 'admin visible shows all
                    If IsVisible(x, y) <= 1 Or IsVisible2(x, y) <= 1 Or AdminVisible = True Then 'admin visible shows all, only process isvisible routines if it's within 4 of character so it doesn't process unnecessary squares too far from player
                        If MapDrawStatus(x, y) = Shadowed Then
                            MapDrawStatusPlus(x, y) = 0
                        End If
                        If MapDrawStatusPlus(x, y) = 0 Then
                            MapDrawStatusPlus(x, y) += 1
                            'draw wall
                            If Map(x, y) = Wall And MapDrawStatus(x, y) <> NotHidden Then
                                CANVAS.DrawImage(FilterImageWall(FloorArt, WallArt, MapBlur(x, y, 2), MapBlur(x, y, 1), MapBlur(x, y, 0), MapBlur(x, y, 3)), xish, yish, TheRoomWidth, TheRoomHeight)
                                MapDrawStatus(x, y) = NotHidden
                                'draw floor
                            ElseIf Map(x, y) = Floor And MapDrawStatus(x, y) <> NotHidden Then
                                ShowFog(xish, yish, x, y, FloorArt)
                                MapDrawStatus(x, y) = NotHidden
                                'draw floor with blood
                            ElseIf Map(x, y) = SpecialFloor And MapDrawStatus(x, y) <> NotHidden Then
                                ShowFog(xish, yish, x, y, FilterImageRed(FloorArt))
                                MapDrawStatus(x, y) = NotHidden
                                'draw stairs up
                            ElseIf Map(x, y) = StairsUp And MapDrawStatus(x, y) <> NotHidden Then
                                CANVAS.DrawImage(My.Resources.StairsUp, xish, yish, TheRoomWidth, TheRoomHeight)
                                MapDrawStatus(x, y) = NotHidden
                                'draw stairs down
                            ElseIf Map(x, y) = StairsDown And MapDrawStatus(x, y) <> NotHidden Then
                                CANVAS.DrawImage(My.Resources.StairsDown, xish, yish, TheRoomWidth, TheRoomHeight)
                                MapDrawStatus(x, y) = NotHidden
                            ElseIf Map(x, y) = Lava And MapDrawStatus(x, y) <> NotHidden Then
                                CANVAS.DrawImage(FilterImageWall(FloorArt, My.Resources.Lava, True, True, False, False), xish, yish, TheRoomWidth, TheRoomHeight)
                                MapDrawStatus(x, y) = NotHidden
                            ElseIf Map(x, y) = Water And MapDrawStatus(x, y) <> NotHidden Then
                                CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Water), xish, yish, TheRoomWidth, TheRoomHeight)
                                MapDrawStatus(x, y) = NotHidden
                            ElseIf Map(x, y) = Ice And MapDrawStatus(x, y) <> NotHidden Then
                                CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Ice), xish, yish, TheRoomWidth, TheRoomHeight)
                                MapDrawStatus(x, y) = NotHidden
                            End If
                        End If
                        'if map is occupied, show enemy
                        If MapOccupied(x, y) > 0 Then
                            ShowEnemy(MapOccupied(x, y), xish, yish, x, y)
                            MapDrawStatus(x, y) = Hidden
                            MapDrawStatusPlus(x, y) = 0
                        Else
                            'if map isn't occupied by mobiles, is it occupied by items, if so show items (prioritize enemy showing over items)
                            If ItemOccupied(x, y) > 0 Then
                                ShowItem(xish, yish, x, y)
                                MapDrawStatusPlus(x, y) = 0
                            End If
                            MapDrawStatus(x, y) = Hidden
                        End If
                        'player can be shown over items for better visibility
                        If x = PlayerPosX And y = PlayerPosY Then
                            CANVAS.DrawString("@", displayfont, Brushes.LimeGreen, xish, yish)
                            MapDrawStatus(x, y) = Hidden
                            MapDrawStatusPlus(x, y) = 0
                        End If
                        MapShown(x, y) = True
                    End If
                ElseIf MapShown(x, y) = True And MapDrawStatus(x, y) <> Shadowed Then 'not within the visual sight of the player, but was visited, so should just be fogged
                    Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                    If MapDrawStatus(x, y) = Hidden Then
                        MapDrawStatusPlus(x, y) = 0
                    End If
                    If MapDrawStatusPlus(x, y) = 0 Then
                        MapDrawStatusPlus(x, y) += 1
                        'draw wall
                        If Map(x, y) = Wall Then
                            CANVAS.DrawImage(FilterImageWall(FloorArt, WallArt, MapBlur(x, y, 2), MapBlur(x, y, 1), MapBlur(x, y, 0), MapBlur(x, y, 3)), xish, yish, TheRoomWidth, TheRoomHeight)
                            MapDrawStatus(x, y) = Shadowed
                            'draw floor
                        ElseIf Map(x, y) = Floor Then
                            ShowFog(xish, yish, x, y, FloorArt)
                            MapDrawStatus(x, y) = Shadowed
                            'draw floor with blood
                        ElseIf Map(x, y) = SpecialFloor Then
                            ShowFog(xish, yish, x, y, FilterImageRed(FloorArt))
                            MapDrawStatus(x, y) = Shadowed
                            'draw stairs up
                        ElseIf Map(x, y) = StairsUp Then
                            CANVAS.DrawImage(My.Resources.StairsUp, xish, yish, TheRoomWidth, TheRoomHeight)
                            MapDrawStatus(x, y) = Shadowed
                            'draw stairs down
                        ElseIf Map(x, y) = StairsDown Then
                            CANVAS.DrawImage(My.Resources.StairsDown, xish, yish, TheRoomWidth, TheRoomHeight)
                            MapDrawStatus(x, y) = Shadowed
                            'water
                        ElseIf Map(x, y) = Water And MapDrawStatus(x, y) <> NotHidden Then
                            CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Water), xish, yish, TheRoomWidth, TheRoomHeight)
                            MapDrawStatus(x, y) = Shadowed
                            'water
                        ElseIf Map(x, y) = Lava And MapDrawStatus(x, y) <> NotHidden Then
                            CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Lava), xish, yish, TheRoomWidth, TheRoomHeight)
                            MapDrawStatus(x, y) = Shadowed
                            'water
                        ElseIf Map(x, y) = Ice And MapDrawStatus(x, y) <> NotHidden Then
                            CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Ice), xish, yish, TheRoomWidth, TheRoomHeight)
                            MapDrawStatus(x, y) = Shadowed
                        End If
                        'shadow the area
                        MapShown(x, y) = True
                        CANVAS.FillRectangle(semiTransBrush, xish, yish, TheRoomWidth, TheRoomHeight)
                    End If
                End If
            Next
        Next
        Me.CreateGraphics.DrawImage(PAD, 0, 0)
        PlayerTurns += 1
    End Sub
    Sub ShowEnemy(ByVal EnemyNum As Short, ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short)
        If MapOccupied(x, y) = 1 Then 'creatures are currently being hidden in the shadows.
            CANVAS.DrawString("r", displayfont, Brushes.Red, xish, yish) 'rat
        ElseIf MapOccupied(x, y) = 2 Then
            CANVAS.DrawString("b", displayfont, Brushes.Red, xish, yish) 'bat
        ElseIf MapOccupied(x, y) = 3 Then
            CANVAS.DrawString("i", displayfont, Brushes.Red, xish, yish) 'imp
        ElseIf MapOccupied(x, y) = 4 Then
            CANVAS.DrawString("g", displayfont, Brushes.Red, xish, yish) 'goblin
        ElseIf MapOccupied(x, y) = 5 Then
            CANVAS.DrawString("t", displayfont, Brushes.Red, xish, yish) 'troll
        ElseIf MapOccupied(x, y) = 6 Then
            CANVAS.DrawString("o", displayfont, Brushes.Red, xish, yish) 'ogre
        ElseIf MapOccupied(x, y) = 7 Then
            CANVAS.DrawString("c", displayfont, Brushes.Red, xish, yish) 'catoblepas
        ElseIf MapOccupied(x, y) = 8 Then
            CANVAS.DrawString("p", displayfont, Brushes.Red, xish, yish) 'parandrus
        ElseIf MapOccupied(x, y) = 9 Then
            CANVAS.DrawString("C", displayfont, Brushes.Red, xish, yish) 'Clurichuan
        ElseIf MapOccupied(x, y) = 10 Then
            CANVAS.DrawString("d", displayfont, Brushes.Red, xish, yish) 'Dullahan
        ElseIf MapOccupied(x, y) = 11 Then
            CANVAS.DrawString("G", displayfont, Brushes.Red, xish, yish) 'Golem
        ElseIf MapOccupied(x, y) = 12 Then
            CANVAS.DrawString("s", displayfont, Brushes.Red, xish, yish) 'sceadugengan
        ElseIf MapOccupied(x, y) = 13 Then
            CANVAS.DrawString("S", displayfont, Brushes.Red, xish, yish) 'Schilla
        End If
    End Sub
    Sub ShowItem(ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short)
        If ItemType(ItemOccupied(x, y)) = Gold Then
            CANVAS.DrawString("g", displayfont, Brushes.Yellow, xish, yish)
        ElseIf ItemType(ItemOccupied(x, y)) = TheEverspark Then
            CANVAS.DrawString("E", displayfont, Brushes.White, xish, yish)
        ElseIf ItemType(ItemOccupied(x, y)) = Weapon Then
            CANVAS.DrawString("g", displayfont, Brushes.DarkCyan, xish, yish)
        Else
            CANVAS.DrawString(ItemShowType(x, y), displayfont, Brushes.Green, xish, yish)
        End If
    End Sub
    Sub ShowFog(ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short, ByVal FloorArt As Image)
        CANVAS.DrawImage(FloorArt, xish, yish, TheRoomWidth, TheRoomHeight)
        If FogMap(x, y) = 1 Then
            CANVAS.DrawImage(FilterImageFog(FloorArt, 10), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf FogMap(x, y) = 2 Then
            CANVAS.DrawImage(FilterImageFog(FloorArt, 20), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf FogMap(x, y) = 3 Then
            CANVAS.DrawImage(FilterImageFog(FloorArt, 30), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf FogMap(x, y) = 4 Then
            CANVAS.DrawImage(FilterImageFog(FloorArt, 40), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf FogMap(x, y) = 5 Then
            CANVAS.DrawImage(FilterImageFog(FloorArt, 50), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf FogMap(x, y) = 6 Then
            CANVAS.DrawImage(FilterImageFog(FloorArt, 60), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf FogMap(x, y) = 7 Then
            CANVAS.DrawImage(FilterImageFog(FloorArt, 70), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf FogMap(x, y) = 8 Then
            CANVAS.DrawImage(FilterImageFog(FloorArt, 80), xish, yish, TheRoomWidth, TheRoomHeight)
        Else
            CANVAS.DrawImage(FilterImageFog(FloorArt, 0), xish, yish, TheRoomWidth, TheRoomHeight)
        End If
    End Sub
#End Region
#Region "Level Up"
    Private Sub LevelUp()
        PlayerExperience -= 100
        If PlayerLevel < 100 Then PlayerLevel += 1
        strcur.Text = LTrim(Str(PlayerSTR)) : strmax.Text = LTrim(Str(PlayerMaxSTR)) : If PlayerSTR < PlayerMaxSTR Then stradd.Enabled = True
        dexcur.Text = LTrim(Str(PlayerDEX)) : dexmax.Text = LTrim(Str(PlayerMaxDEX)) : If PlayerDEX < PlayerMaxDEX Then dexadd.Enabled = True
        intcur.Text = LTrim(Str(PlayerINT)) : intmax.Text = LTrim(Str(PlayerMaxINT)) : If PlayerINT < PlayerMaxINT Then intadd.Enabled = True
        wiscur.Text = LTrim(Str(PlayerWIS)) : wismax.Text = LTrim(Str(PlayerMaxWIS)) : If PlayerWIS < PlayerMaxWIS Then wisadd.Enabled = True
        concur.Text = LTrim(Str(PlayerCON)) : conmax.Text = LTrim(Str(PlayerMaxCON)) : If PlayerCON < PlayerMaxCON Then conadd.Enabled = True
        chacur.Text = LTrim(Str(PlayerCHA)) : chamax.Text = LTrim(Str(PlayerMaxCHA)) : If PlayerCHA < PlayerMaxCHA Then chaadd.Enabled = True
        luccur.Text = LTrim(Str(PlayerLUC)) : lucmax.Text = LTrim(Str(PlayerMaxLuc)) : If PlayerLUC < PlayerMaxLuc Then lucadd.Enabled = True
        hpcur.Text = LTrim(Str(PlayerHitpoints)) : hpadd.Enabled = True
        wpcur.Text = LTrim(Str(PlayerWillpower)) : wpadd.Enabled = True
        PlayerLevelPoints += Math.Round(PlayerWIS / 4, 0)
        CurPoints.Text = LTrim(Str(PlayerLevelPoints))
        HelpInfo.Visible = False
        LevelUpPanel.Visible = True
    End Sub
    Private Sub stradd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stradd.Click
        If Val(strcur.Text) < Val(strmax.Text) And Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            strcur.Text = LTrim(Str(Val(strcur.Text) + 1))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(strcur.Text) = Val(strmax.Text) Then stradd.Enabled = False
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            stradd.Enabled = False
        End If
    End Sub
    Private Sub disableadds()
        stradd.Enabled = False
        dexadd.Enabled = False
        intadd.Enabled = False
        wisadd.Enabled = False
        conadd.Enabled = False
        chaadd.Enabled = False
        lucadd.Enabled = False
        hpadd.Enabled = False
        wpadd.Enabled = False
    End Sub
    Private Sub dexadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dexadd.Click
        If Val(dexcur.Text) < Val(dexmax.Text) And Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            dexcur.Text = LTrim(Str(Val(dexcur.Text) + 1))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(dexcur.Text) = Val(dexmax.Text) Then dexadd.Enabled = False
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            dexadd.Enabled = False
        End If
    End Sub
    Private Sub intadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles intadd.Click
        If Val(intcur.Text) < Val(intmax.Text) And Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            intcur.Text = LTrim(Str(Val(intcur.Text) + 1))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(intcur.Text) = Val(intmax.Text) Then intadd.Enabled = False
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            intadd.Enabled = False
        End If
    End Sub
    Private Sub wisadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wisadd.Click
        If Val(wiscur.Text) < Val(wismax.Text) And Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            wiscur.Text = LTrim(Str(Val(wiscur.Text) + 1))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(wiscur.Text) = Val(wismax.Text) Then wisadd.Enabled = False
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            wisadd.Enabled = False
        End If
    End Sub
    Private Sub conadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles conadd.Click
        If Val(concur.Text) < Val(conmax.Text) And Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            concur.Text = LTrim(Str(Val(concur.Text) + 1))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(concur.Text) = Val(conmax.Text) Then conadd.Enabled = False
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            conadd.Enabled = False
        End If
    End Sub
    Private Sub chaadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chaadd.Click
        If Val(chacur.Text) < Val(chamax.Text) And Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            chacur.Text = LTrim(Str(Val(chacur.Text) + 1))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(chacur.Text) = Val(chamax.Text) Then chaadd.Enabled = False
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            chaadd.Enabled = False
        End If
    End Sub
    Private Sub lucadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lucadd.Click
        If Val(luccur.Text) < Val(lucmax.Text) And Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            luccur.Text = LTrim(Str(Val(luccur.Text) + 1))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(luccur.Text) = Val(lucmax.Text) Then lucadd.Enabled = False
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            lucadd.Enabled = False
        End If
    End Sub
    Private Sub hpadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hpadd.Click
        If Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            hpcur.Text = LTrim(Str(Val(hpcur.Text) + 10))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            hpadd.Enabled = False
        End If
    End Sub
    Private Sub wpadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wpadd.Click
        If Val(CurPoints.Text) > 0 Then
            PlayerLevelPoints -= 1
            wpcur.Text = LTrim(Str(Val(wpcur.Text) + 10))
            CurPoints.Text = LTrim(Str(Val(CurPoints.Text) - 1))
            If Val(CurPoints.Text) = 0 Then disableadds()
        Else
            wpadd.Enabled = False
        End If
    End Sub
    Private Sub DoneBttn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DoneBttn.Click
        Dim PrevCon As Short = PlayerCON
        Dim PrevStr As Short = PlayerSTR
        PlayerSTR = Val(strcur.Text)
        PlayerDEX = Val(dexcur.Text)
        PlayerINT = Val(intcur.Text)
        PlayerWIS = Val(wiscur.Text)
        PlayerCON = Val(concur.Text)
        PlayerCHA = Val(chacur.Text)
        PlayerLUC = Val(luccur.Text)
        PlayerHitpoints = Val(hpcur.Text)
        PlayerWillpower = Val(wpcur.Text)
        PlayerCurHitpoints = PlayerHitpoints
        PlayerCurWillpower = PlayerWillpower
        HealthBar.Caption = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints)) + " HP"
        HealthBar.Value = PlayerCurHitpoints
        HealthBar.Max = PlayerHitpoints
        WillpowerBar.Caption = LTrim(Str(PlayerCurWillpower)) + " / " + LTrim(Str(PlayerWillpower)) + " WP"
        WillpowerBar.Value = PlayerCurWillpower
        WillpowerBar.Max = PlayerWillpower
        PlayerDefense += Math.Round(PlayerCON / 5, 0) - Math.Round(PrevCon / 5, 0)
        PlayerAttack += Math.Round(PlayerSTR / 5, 0) - Math.Round(PrevStr / 5, 0)
        LevelUpPanel.Visible = False
    End Sub
#End Region
#Region "Skill Button Events"
    Private Sub Skill2Over(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If SKillGobalCooldown <= 0 Then
            FilterBevel(Skill2)
        End If
    End Sub
    Private Sub Skill2Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If SKillGobalCooldown <= 0 Then
            UpdateSkills()
        End If
    End Sub
    Private Sub Skill3Over(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If SKillGobalCooldown <= 0 Then
            FilterBevel(Skill3)
        End If
    End Sub
    Private Sub Skill3Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If SKillGobalCooldown <= 0 Then
            UpdateSkills()
        End If
    End Sub
    Private Sub Skill1Press(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Skill1.Click
        If SKillGobalCooldown <= 0 Then
            DecipherSkill(1)
        Else
            SND("Skills on cooldown.")
        End If
    End Sub
    Private Sub Skill2Press(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Skill2.Click
        If SKillGobalCooldown <= 0 Then
            DecipherSkill(2)
        Else
            SND("Skills on cooldown.")
        End If
    End Sub
    Private Sub Skill3Press(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Skill3.Click
        If SKillGobalCooldown <= 0 Then
            DecipherSkill(3)
        Else
            SND("Skills on cooldown.")
        End If
    End Sub
    Private Sub DecipherSkill(ByVal skillnum As Short)
        Dim SkillName As String = ""
        If skillnum = 1 Then
            SkillName = Skill1Name.Text
        ElseIf skillnum = 2 Then
            SkillName = Skill2Name.Text
        ElseIf skillnum = 3 Then
            SkillName = Skill3Name.Text
        End If
        ProcessSKill(SkillName)
    End Sub
    Private Sub ProcessSKill(ByVal Skillname As String)
        If Skillname = "Fire Shield" Then
        ElseIf Skillname = "Wound" Then
            If PlayerCurWillpower >= 20 Then
                SND("Wound in what direction?")
                SkillType = "Wound"
                SKillGobalCooldown = 3
                SetSkillsToCooldown()
                PlayerCurWillpower -= 20
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Fury" Then
            If PlayerCurWillpower >= 20 Then
                PlayerCurWillpower -= 20
                SND("You're empowered with fury.")
                SKillGobalCooldown = 5
                SetSkillsToCooldown()
                Fury = 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Trip" Then
            If PlayerCurWillpower >= 10 Then
                PlayerCurWillpower -= 10
                SND("Trip in what direction?")
                SkillType = "Trip"
                SKillGobalCooldown = 2
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Block" Then
            If PlayerCurWillpower >= 15 Then
                PlayerCurWillpower -= 15
                SND("You prepare to block attacks.")
                Block = 2
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Double Slice" Then
            If PlayerCurWillpower >= 15 Then
                PlayerCurWillpower -= 15
                SND("Double Slice which direction?")
                SkillType = "Double Slice"
                SKillGobalCooldown = 2
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Runestrike" Then
            If PlayerCurWillpower >= 20 Then
                PlayerCurWillpower -= 20
                SND("Runestrike which direction?")
                SkillType = "Runestrike"
                SKillGobalCooldown = 3
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Empower" Then
            If PlayerCurHitpoints >= 31 Then
                PlayerCurHitpoints -= 30
                PlayerCurWillpower += 30
                SND("Empower which direction?")
            Else
                SND("That would kill you.")
            End If
        ElseIf Skillname = "Holy Bolt" Then
            If PlayerCurWillpower >= 20 Then
                PlayerCurWillpower -= 20
                SkillType = "Holy Bolt"
                SKillGobalCooldown = 4
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Fire Arrow" Then
            If PlayerCurWillpower >= 15 Then
                PlayerCurWillpower -= 15
                SkillType = "Fire Arrow"
                SKillGobalCooldown = 3
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Leech" Then
            If PlayerCurWillpower >= PlayerAttack Then
                PlayerCurWillpower -= PlayerAttack
                PlayerCurHitpoints += PlayerAttack
                SKillGobalCooldown = 3
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Punch" Then
            If PlayerCurWillpower >= 5 Then
                SND("Punch in what direction?")
                SkillType = "Punch"
                PlayerCurWillpower -= 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Kick" Then
            If PlayerCurWillpower >= 5 Then
                SND("Kick in what direction?")
                SkillType = "Kick"
                PlayerCurWillpower -= 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Hit" Then
            If PlayerCurWillpower >= 5 Then
                SND("Hit in what direction?")
                SkillType = "Hit"
                PlayerCurWillpower -= 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Strike" Then
            If PlayerCurWillpower >= 5 Then
                SND("Strike in what direction?")
                SkillType = "Strike"
                PlayerCurWillpower -= 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Stab" Then
            If PlayerCurWillpower >= 5 Then
                SND("Stab in what direction?")
                SkillType = "Stab"
                PlayerCurWillpower -= 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Slice" Then
            If PlayerCurWillpower >= 5 Then
                SND("Stab in what direction?")
                SkillType = "Stab"
                PlayerCurWillpower -= 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Shoot" Then
            If PlayerCurWillpower >= 5 Then
                SND("Shoot in what direction?")
                SkillType = "Shoot"
                PlayerCurWillpower -= 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Heal" Then
            If PlayerCurWillpower >= 20 Then
                SND("You heal yourself.")
                PlayerCurWillpower -= 20
                PlayerCurHitpoints += 20
                If PlayerCurHitpoints >= PlayerHitpoints Then
                    PlayerCurHitpoints = PlayerHitpoints
                End If
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Hide" Then
            If PlayerCurWillpower >= 10 Then
                SND("You hide. Don't move.")
                PlayerCurWillpower -= 10
                SkillType = "Hide"
                PlayerHidden = 6
                SKillGobalCooldown = 6
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Bone Shield" Then
            If PlayerCurWillpower >= 30 Then
                SND("You cast bone shield.")
                PlayerCurWillpower -= 30
                BoneShield = 5
                SKillGobalCooldown = 10
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Magic Shield" Then
            If PlayerCurWillpower >= 30 Then
                SND("You cast magic shield.")
                PlayerCurWillpower -= 30
                MagicShield = 10
                SKillGobalCooldown = 10
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Counter" Then
            If PlayerCurWillpower >= 15 Then
                SND("You cast counter")
                PlayerCurWillpower -= 15
                CounterAttack = 3
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Fireball" Then
            If PlayerCurWillpower >= 30 Then
                SND("Fireball in which direction?")
                PlayerCurWillpower -= 30
                SkillType = "Fireball"
                SKillGobalCooldown = 3
                SetSkillsToCooldown()
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Clumsiness" Then
            If PlayerCurWillpower >= 20 Then
                PlayerCurWillpower -= 20
                SND("Clumsiness in what direction?")
                SKillGobalCooldown = 10
                SetSkillsToCooldown()
                SkillType = "Clumsiness"
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Immolate" Then
            If PlayerCurWillpower >= 20 Then
                PlayerCurWillpower -= 20
                SND("You cast Immolate.")
                SKillGobalCooldown = 10
                SetSkillsToCooldown()
                Immolate = 5
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Stun" Then
            If PlayerCurWillpower >= 25 Then
                SND("Stun in what direction?")
                SkillType = "Stun"
                PlayerCurWillpower -= 25
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Sacrifice" Then
            If PlayerCurHitpoints >= 11 Then
                PlayerCurHitpoints -= 10
                SKillGobalCooldown = 5
                SetSkillsToCooldown()
                SkillType = "Sacrifice"
            Else
                SND("You would die.")
            End If
        ElseIf Skillname = "Backstab" Then
            If PlayerCurWillpower >= 25 Then
                PlayerCurWillpower -= 25
                SKillGobalCooldown = 5
                SetSkillsToCooldown()
                SkillType = "Sacrifice" 'does same amount of damage
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Triple Slice" Then
            If PlayerCurWillpower >= 30 Then
                PlayerCurWillpower -= 30
                SKillGobalCooldown = 4
                SetSkillsToCooldown()
                SkillType = "Triple Slice"
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Whisper" Then
            If PlayerCurWillpower >= 50 Then
                PlayerCurWillpower -= 50
                SKillGobalCooldown = 10
                SetSkillsToCooldown()
                SkillType = "Whisper"
            Else
                SND("Not enough willpower.")
            End If
        ElseIf Skillname = "Silence" Then
            If PlayerCurWillpower >= 50 Then
                PlayerCurWillpower -= 50
                SKillGobalCooldown = 10
                SetSkillsToCooldown()
                SkillType = "Silence"
                SND("You cast silence.")
                Silence = 5
            Else
                SND("Not enough willpower.")
            End If
        End If
        HealthBar.Caption = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints)) + " HP"
        HealthBar.Value = PlayerCurHitpoints
        WillpowerBar.Caption = LTrim(Str(PlayerCurWillpower)) + " / " + LTrim(Str(PlayerWillpower)) + " WP"
        WillpowerBar.Value = PlayerCurWillpower
    End Sub
    Private Sub SetSkillsToCooldown()
        FilterRed(Skill1)
        FilterRed(Skill2)
        FilterRed(Skill3)
    End Sub
    Private Sub Skill1Over(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If SKillGobalCooldown <= 0 Then
            FilterBevel(Skill1)
        End If
    End Sub
    Private Sub Skill1leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If SKillGobalCooldown <= 0 Then
            UpdateSkills()
        End If
    End Sub
    Private Sub UpdateSkills()
        If PlayerClass = "Priest" Then
            Skill1.Image = My.Resources.Punch : Skill1Name.Text = "Punch"
            Skill2.Image = My.Resources.Heal : Skill2Name.Text = "Heal"
            Skill3.Image = My.Resources.HolyBolt : Skill3Name.Text = "Holy Bolt"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Woodsman" Then
            Skill1.Image = My.Resources.Shoot : Skill1Name.Text = "Shoot"
            Skill2.Image = My.Resources.Hide : Skill2Name.Text = "Hide"
            Skill3.Image = My.Resources.FireArrow : Skill3Name.Text = "Fire Arrow"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Gravedigger" Then
            Skill1.Image = My.Resources.Kick : Skill1Name.Text = "Kick"
            Skill2.Image = My.Resources.BoneShield : Skill2Name.Text = "Bone Shield"
            Skill3.Image = My.Resources.Leech : Skill3Name.Text = "Leech"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Mageling" Then
            Skill1.Image = My.Resources.Punch : Skill1Name.Text = "Punch"
            Skill2.Image = My.Resources.MagicShield : Skill2Name.Text = "Magic Shield"
            Skill3.Image = My.Resources.Fireball : Skill3Name.Text = "Fireball"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Hermit" Then
            Skill1.Image = My.Resources.Hit : Skill1Name.Text = "Hit"
            Skill2.Image = My.Resources.Counter : Skill2Name.Text = "Counter"
            Skill3.Image = My.Resources.Clumsiness : Skill3Name.Text = "Clumsiness"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Elementalist" Then
            Skill1.Image = My.Resources.Strike : Skill1Name.Text = "Strike"
            Skill2.Image = My.Resources.FireShield : Skill2Name.Text = "Magic Shield"
            Skill3.Image = My.Resources.Immolate : Skill3Name.Text = "Immolate"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Headhunter" Then
            Skill1.Image = My.Resources.Stab : Skill1Name.Text = "Stab"
            Skill2.Image = My.Resources.Wound : Skill2Name.Text = "Wound"
            Skill3.Image = My.Resources.Stun : Skill3Name.Text = "Stun"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Plainsman" Then
            Skill1.Image = My.Resources.Slice : Skill1Name.Text = "Slice"
            Skill2.Image = My.Resources.Fury : Skill2Name.Text = "Fury"
            Skill3.Image = My.Resources.Sacrifice : Skill3Name.Text = "Sacrifice"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Pickpocket" Then
            Skill1.Image = My.Resources.Punch : Skill1Name.Text = "Punch"
            Skill2.Image = My.Resources.Trip : Skill2Name.Text = "Trip"
            Skill3.Image = My.Resources.Backstab : Skill3Name.Text = "Backstab"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Page" Then
            Skill1.Image = My.Resources.Strike : Skill1Name.Text = "Strike"
            Skill2.Image = My.Resources.Block : Skill2Name.Text = "Block"
            Skill3.Image = My.Resources.Stun : Skill3Name.Text = "Stun"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Scout" Then
            Skill1.Image = My.Resources.Slice : Skill1Name.Text = "Slice"
            Skill2.Image = My.Resources.DoubleSlice : Skill2Name.Text = "Double Slice"
            Skill3.Image = My.Resources.TripleSlice : Skill3Name.Text = "Triple Slice"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Runescribe" Then
            Skill1.Image = My.Resources.Kick : Skill1Name.Text = "Kick"
            Skill2.Image = My.Resources.Runestrike : Skill2Name.Text = "Runestrike"
            Skill3.Image = My.Resources.Whisper : Skill3Name.Text = "Whisper"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Monk" Then
            Skill1.Image = My.Resources.Punch : Skill1Name.Text = "Punch"
            Skill2.Image = My.Resources.Kick : Skill2Name.Text = "Kick"
            Skill3.Image = My.Resources.Stun : Skill3Name.Text = "Stun"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        ElseIf PlayerClass = "Minstrel" Then
            Skill1.Image = My.Resources.Stab : Skill1Name.Text = "Stab"
            Skill2.Image = My.Resources.Empower : Skill2Name.Text = "Empower"
            Skill3.Image = My.Resources.Silence : Skill3Name.Text = "Silence"
            Skill1Name.Left = 17 + (Skill1.Image.Width / 2) - Skill1Name.Width / 2 'center skill name on skill image
            Skill2Name.Left = 80 + (Skill2.Image.Width / 2) - Skill2Name.Width / 2 'center skill name on skill image
            Skill3Name.Left = 144 + (Skill3.Image.Width / 2) - Skill3Name.Width / 2 'center skill name on skill image
        End If
    End Sub
#End Region
#Region "Image Filters"
    Private Sub FilterBevel(ByVal TheObject As System.Object)
        Dim i, j, rr, gg, bb, a As Integer
        Dim c, cc As System.Drawing.Color
        Dim pic1 As System.Drawing.Bitmap
        Dim r1, r2, b1, b2, g1, g2 As Integer
        pic1 = TheObject.Image
        For j = 0 To TheObject.Image.Height - 2
            For i = 0 To TheObject.Image.Width - 2
                c = pic1.GetPixel(i, j)
                r1 = c.R
                g1 = c.G
                b1 = c.B
                cc = pic1.GetPixel(i + 1, j + 1)
                r2 = cc.R
                g2 = cc.G
                b2 = cc.B
                rr = r2 - r1 + 128
                gg = g2 - g1 + 128
                bb = b2 - b1 + 128
                If rr < 0 Then rr = 0
                If rr > 255 Then rr = 255
                If gg < 0 Then gg = 0
                If gg > 255 Then gg = 255
                If bb < 0 Then bb = 0
                If bb > 255 Then bb = 255
                c = Color.FromArgb(a, rr, gg, bb)
                pic1.SetPixel(i, j, c)
            Next
            TheObject.Refresh()
        Next
    End Sub
    Private Sub FilterRed(ByVal TheObject As System.Object)
        Dim i, j, a As Integer
        Dim c As System.Drawing.Color
        Dim pic1 As System.Drawing.Bitmap
        Dim r1, b1, g1 As Integer
        pic1 = TheObject.Image
        For j = 0 To TheObject.Image.Height - 2
            For i = 0 To TheObject.Image.Width - 2
                c = pic1.GetPixel(i, j)
                r1 = c.R + 75
                g1 = c.G - 25
                b1 = c.B - 25
                If r1 < 0 Then r1 = 0
                If r1 > 255 Then r1 = 255
                If g1 < 0 Then g1 = 0
                If b1 < 0 Then b1 = 0
                c = Color.FromArgb(a, r1, g1, b1)
                pic1.SetPixel(i, j, c)
            Next
            TheObject.Refresh()
        Next
    End Sub
#End Region
    Public Sub SND(ByVal Text As String) 'this displays the display text
        Dim LengthTo0 As Short
        DisplayText.Text = ""
        For LengthTo0 = 27 To 0 Step -1
            If LengthTo0 > 0 Then
                SNDLog(LengthTo0) = SNDLog(LengthTo0 - 1) 'moves all old text to make room
            Else
                SNDLog(LengthTo0) = Text 'for the new text
            End If
        Next
        For LengthTo0 = 0 To 27 Step 1
            If LengthTo0 < 27 And SNDLog(LengthTo0) <> "" Then
                DisplayText.Text += SNDLog(LengthTo0) + Chr(13) 'add a paragraph character after each line sent
            Else
                DisplayText.Text += SNDLog(LengthTo0) + Chr(13) 'except for the last
            End If
        Next
    End Sub
    Private Sub Repaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        Me.CreateGraphics.DrawImage(PAD, 0, 0)
    End Sub
    Private Sub ProcessCommand(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If PlayerDead = False Then
            If CommentBoxOpen = True Then
                CloseCommentBox()
            End If
            If e.KeyCode = Keys.Up And PlayerPosY > 0 Or e.KeyCode = Keys.NumPad8 And PlayerPosY > 0 Then
                If Map(PlayerPosX, PlayerPosY - 1) > 0 And MapOccupied(PlayerPosX, PlayerPosY - 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosY -= 1
                    ReDraw()
                ElseIf Map(PlayerPosX, PlayerPosY - 1) > 0 And MapOccupied(PlayerPosX, PlayerPosY - 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX, PlayerPosY - 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Down And PlayerPosY < MapSize Or e.KeyCode = Keys.NumPad2 And PlayerPosY < MapSize Then
                If Map(PlayerPosX, PlayerPosY + 1) > 0 And MapOccupied(PlayerPosX, PlayerPosY + 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosY += 1
                    ReDraw()
                ElseIf Map(PlayerPosX, PlayerPosY + 1) > 0 And MapOccupied(PlayerPosX, PlayerPosY + 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX, PlayerPosY + 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Right And PlayerPosX < MapSize Or e.KeyCode = Keys.NumPad6 And PlayerPosX < MapSize Then
                If Map(PlayerPosX + 1, PlayerPosY) > 0 And MapOccupied(PlayerPosX + 1, PlayerPosY) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosX += 1
                    ReDraw()
                ElseIf Map(PlayerPosX + 1, PlayerPosY) > 0 And MapOccupied(PlayerPosX + 1, PlayerPosY) <> 0 Then
                    PlayerHitLocation(PlayerPosX + 1, PlayerPosY)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Left And PlayerPosX > 0 Or e.KeyCode = Keys.NumPad4 And PlayerPosX > 0 Then
                If Map(PlayerPosX - 1, PlayerPosY) > 0 And MapOccupied(PlayerPosX - 1, PlayerPosY) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosX -= 1
                    ReDraw()
                ElseIf Map(PlayerPosX - 1, PlayerPosY) > 0 And MapOccupied(PlayerPosX - 1, PlayerPosY) <> 0 Then
                    PlayerHitLocation(PlayerPosX - 1, PlayerPosY)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad5 Then
                If PlayerCurHitpoints < PlayerHitpoints Or PlayerCurWillpower < PlayerWillpower Then
                    PlayerCurHitpoints += 1
                    PlayerCurWillpower += 1
                    HealthBar.Caption = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints)) + " HP"
                    HealthBar.Value = PlayerCurHitpoints
                    WillpowerBar.Caption = LTrim(Str(PlayerCurWillpower)) + " / " + LTrim(Str(PlayerWillpower)) + " WP"
                    WillpowerBar.Value = PlayerCurWillpower
                End If
                ReDraw()
            ElseIf e.KeyCode = Keys.P Or e.KeyCode = Keys.T Then
                If ItemOccupied(PlayerPosX, PlayerPosY) > 0 Then
                    Inventory.AddToInventory(ItemOccupied(PlayerPosX, PlayerPosY))
                Else
                    SND("Nothing is here to pickup.")
                End If
                ReDraw()
            ElseIf e.KeyCode = Keys.D1 Then
                DecipherSkill(1)
            ElseIf e.KeyCode = Keys.D2 Then
                DecipherSkill(2)
            ElseIf e.KeyCode = Keys.D3 Then
                DecipherSkill(3)
            ElseIf e.KeyCode = Keys.F1 Then
                If ScoresBox.Visible = True Then
                    ScoresBox.Visible = False
                ElseIf ScoresBox.Visible = False Then
                    SNDScores()
                    CharStats.Visible = False
                    ScoresBox.Visible = True
                End If
            ElseIf e.KeyCode = Keys.F2 Then
                If CharStats.Visible = True Then
                    CharStats.Visible = False
                ElseIf CharStats.Visible = False Then
                    StatBox.Text = "[Character Stats]" + Chr(13) + "Depth     : " + LTrim(Str(MapLevel)) + Chr(13) + "Level     : " + LTrim(Str(PlayerLevel)) + Chr(13) _
+ "Experience: " + LTrim(Str(PlayerExperience)) + Chr(13) _
+ "Gold      : " + LTrim(Str(PlayerGold)) + Chr(13) _
+ "Turns     : " + LTrim(Str(PlayerTurns)) + Chr(13) + Chr(13) _
+ "Strength    : " + LTrim(Str(PlayerSTR)) + Chr(13) _
+ "Dexterity   : " + LTrim(Str(PlayerDEX)) + Chr(13) _
+ "Intelligence: " + LTrim(Str(PlayerINT)) + Chr(13) _
+ "Wisdom      : " + LTrim(Str(PlayerWIS)) + Chr(13) _
+ "Constitution: " + LTrim(Str(PlayerCON)) + Chr(13) _
+ "Charisma    : " + LTrim(Str(PlayerCHA)) + Chr(13) _
+ "Luck        : " + LTrim(Str(PlayerLUC))
                    StatBox.Visible = True
                    ScoresBox.Visible = False
                    CharStats.Visible = True
                End If
            ElseIf e.KeyCode = Keys.F3 Then
                If LogVisible = True Then
                    LogVisible = False
                    Panel1.Height -= DisplayText.Height + 10
                    Skill1Name.Top = 99 : Skill2Name.Top = 99 : Skill3Name.Top = 99
                ElseIf LogVisible = False Then
                    LogVisible = True
                    Panel1.Height += DisplayText.Height + 10
                    Skill1Name.Top = 99 : Skill2Name.Top = 99 : Skill3Name.Top = 99
                End If
            ElseIf e.KeyCode = Keys.F4 Then
                PlayerExperience = 0
                PlayerGold = 0
                PlayerTurns = 0
                PlayerLevel = 0
                PlayerLevelPoints = 0
                PlayerDead = False
                MapLevel = 0
                Initialize(0, EventArgs.Empty)
                Comment11.Visible = False
                For tmp0 = 0 To 19 Step 1
                    ItemInventoryName(tmp0) = ""
                    ItemInventoryQuality(tmp0) = 0
                    ItemInventoryType(tmp0) = 0
                    PlayerEquipArms = 0
                    PlayerEquipChest = 0
                    PlayerEquipFeet = 0
                    PlayerEquipHands = 0
                    PlayerEquipHead = 0
                    PlayerEquipLegs = 0
                    PlayerEquipNArms = ""
                    PlayerEquipNChest = ""
                    PlayerEquipNFeet = ""
                    PlayerEquipNHands = ""
                    PlayerEquipNHead = ""
                    PLayerEquipNLegs = ""
                Next
                Me.Hide()
                ChooseCharacter.TabControl1.SelectedTab = ChooseCharacter.BasicTab
                ChooseCharacter.CharacterName.Text = "Mykil Ironfist"
                ChooseCharacter.TopMost = True
                ChooseCharacter.Show()
            ElseIf e.KeyCode = Keys.OemPeriod And e.Shift = True Then 'go down
                If Map(PlayerPosX, PlayerPosY) = 3 Then 'exit
                    Dim tmp1 As Short
                    For tmp0 = 0 To MapSize Step 1
                        For tmp1 = 0 To MapSize Step 1
                            Map(tmp0, tmp1) = 0
                        Next
                    Next
                    BuildNewMap()
                End If
            ElseIf e.KeyCode = Keys.I Then 'inventory
                Inventory.NameStat.Text = PlayerName
                Inventory.HealthStat.Text = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints))
                Inventory.GoldStat.Text = LTrim(Str(PlayerGold))
                Inventory.ExperienceStat.Text = LTrim(Str(PlayerExperience))
                Inventory.PointsStat.Text = LTrim(Str(PlayerLevelPoints))
                Inventory.AttackScore.Text = "Attack: " + LTrim(Str(PlayerAttack))
                Inventory.DefenseScore.Text = "Defense: " + LTrim(Str(PlayerDefense))
                If PlayerEquipHead <> 0 Then
                    Inventory.HeadEquip.Text = CapitalizeFirstLetter(PlayerEquipNHead + " +" + LTrim(Str(PlayerEquipQHead)))
                End If
                If PlayerEquipArms <> 0 Then
                    Inventory.ArmsEquip.Text = CapitalizeFirstLetter(PlayerEquipNArms + " +" + LTrim(Str(PlayerEquipQArms)))
                End If
                If PlayerEquipChest <> 0 Then
                    Inventory.ChestEquip.Text = CapitalizeFirstLetter(PlayerEquipNChest + " +" + LTrim(Str(PlayerEquipQChest)))
                End If
                If PlayerEquipFeet <> 0 Then
                    Inventory.BootsEquip.Text = CapitalizeFirstLetter(PlayerEquipNFeet + " +" + LTrim(Str(PlayerEquipQFeet)))
                End If
                If PlayerEquipHands <> 0 Then
                    Inventory.HandsEquip.Text = CapitalizeFirstLetter(PlayerEquipNHands + " +" + LTrim(Str(PlayerEquipQHands)))
                End If
                If PlayerEquipLegs <> 0 Then
                    Inventory.LegsEquip.Text = CapitalizeFirstLetter(PLayerEquipNLegs + " +" + LTrim(Str(PLayerEquipQLegs)))
                End If
                'inventory 1 is the left column, 2 is the right column
                Dim Line1 As String
                If ItemInventoryName(0) <> "" Then Line1 = "1. " + CapitalizeFirstLetter(ItemInventoryName(0)) + " +" + LTrim(Str(ItemInventoryQuality(0))) + Chr(13) Else Line1 = "1." + Chr(13)
                If ItemInventoryName(1) <> "" Then Line1 += "2. " + CapitalizeFirstLetter(ItemInventoryName(1)) + " +" + LTrim(Str(ItemInventoryQuality(1))) + Chr(13) Else Line1 += "2." + Chr(13)
                If ItemInventoryName(2) <> "" Then Line1 += "3. " + CapitalizeFirstLetter(ItemInventoryName(2)) + " +" + LTrim(Str(ItemInventoryQuality(2))) + Chr(13) Else Line1 += "3." + Chr(13)
                If ItemInventoryName(3) <> "" Then Line1 += "4. " + CapitalizeFirstLetter(ItemInventoryName(3)) + " +" + LTrim(Str(ItemInventoryQuality(3))) + Chr(13) Else Line1 += "4." + Chr(13)
                If ItemInventoryName(4) <> "" Then Line1 += "5. " + CapitalizeFirstLetter(ItemInventoryName(4)) + " +" + LTrim(Str(ItemInventoryQuality(4))) + Chr(13) Else Line1 += "5." + Chr(13)
                If ItemInventoryName(5) <> "" Then Line1 += "6. " + CapitalizeFirstLetter(ItemInventoryName(5)) + " +" + LTrim(Str(ItemInventoryQuality(5))) + Chr(13) Else Line1 += "6." + Chr(13)
                If ItemInventoryName(6) <> "" Then Line1 += "7. " + CapitalizeFirstLetter(ItemInventoryName(6)) + " +" + LTrim(Str(ItemInventoryQuality(6))) + Chr(13) Else Line1 += "7." + Chr(13)
                If ItemInventoryName(7) <> "" Then Line1 += "8. " + CapitalizeFirstLetter(ItemInventoryName(7)) + " +" + LTrim(Str(ItemInventoryQuality(7))) + Chr(13) Else Line1 += "8." + Chr(13)
                If ItemInventoryName(8) <> "" Then Line1 += "9. " + CapitalizeFirstLetter(ItemInventoryName(8)) + " +" + LTrim(Str(ItemInventoryQuality(8))) + Chr(13) Else Line1 += "9." + Chr(13)
                If ItemInventoryName(9) <> "" Then Line1 += "10. " + CapitalizeFirstLetter(ItemInventoryName(9)) + " +" + LTrim(Str(ItemInventoryQuality(9))) + Chr(13) Else Line1 += "10." + Chr(13)
                Inventory.Inventory1.Text = Line1 : Line1 = ""
                If ItemInventoryName(10) <> "" Then Line1 = "11. " + CapitalizeFirstLetter(ItemInventoryName(10)) + " +" + LTrim(Str(ItemInventoryQuality(10))) + Chr(13) Else Line1 = "11." + Chr(13)
                If ItemInventoryName(11) <> "" Then Line1 += "12. " + CapitalizeFirstLetter(ItemInventoryName(11)) + " +" + LTrim(Str(ItemInventoryQuality(11))) + Chr(13) Else Line1 += "12." + Chr(13)
                If ItemInventoryName(12) <> "" Then Line1 += "13. " + CapitalizeFirstLetter(ItemInventoryName(12)) + " +" + LTrim(Str(ItemInventoryQuality(12))) + Chr(13) Else Line1 += "13." + Chr(13)
                If ItemInventoryName(13) <> "" Then Line1 += "14. " + CapitalizeFirstLetter(ItemInventoryName(13)) + " +" + LTrim(Str(ItemInventoryQuality(13))) + Chr(13) Else Line1 += "14." + Chr(13)
                If ItemInventoryName(14) <> "" Then Line1 += "15. " + CapitalizeFirstLetter(ItemInventoryName(14)) + " +" + LTrim(Str(ItemInventoryQuality(14))) + Chr(13) Else Line1 += "15." + Chr(13)
                If ItemInventoryName(15) <> "" Then Line1 += "16. " + CapitalizeFirstLetter(ItemInventoryName(15)) + " +" + LTrim(Str(ItemInventoryQuality(15))) + Chr(13) Else Line1 += "16." + Chr(13)
                If ItemInventoryName(16) <> "" Then Line1 += "17. " + CapitalizeFirstLetter(ItemInventoryName(16)) + " +" + LTrim(Str(ItemInventoryQuality(16))) + Chr(13) Else Line1 += "17." + Chr(13)
                If ItemInventoryName(17) <> "" Then Line1 += "18. " + CapitalizeFirstLetter(ItemInventoryName(17)) + " +" + LTrim(Str(ItemInventoryQuality(17))) + Chr(13) Else Line1 += "18." + Chr(13)
                If ItemInventoryName(18) <> "" Then Line1 += "19. " + CapitalizeFirstLetter(ItemInventoryName(18)) + " +" + LTrim(Str(ItemInventoryQuality(18))) + Chr(13) Else Line1 += "19." + Chr(13)
                If ItemInventoryName(19) <> "" Then Line1 += "20. " + CapitalizeFirstLetter(ItemInventoryName(19)) + " +" + LTrim(Str(ItemInventoryQuality(19))) + Chr(13) Else Line1 += "20." + Chr(13)
                Inventory.Inventory2.Text = Line1 : Line1 = ""
                Inventory.Show()
            ElseIf e.KeyCode = Keys.Oemcomma And e.Shift = True Then 'go up
                If Map(PlayerPosX, PlayerPosY) = 2 Then 'entrance
                    Dim tmp1 As Short
                    For tmp0 = 0 To MapSize Step 1
                        For tmp1 = 0 To MapSize Step 1
                            Map(tmp0, tmp1) = 0
                        Next
                    Next
                    MapLevel -= 2
                    BuildNewMap()
                End If
            ElseIf e.KeyCode = Keys.Escape Then 'exit inventory, exit game
                Me.Close()
                Me.Dispose()
            ElseIf e.KeyCode = Keys.H Or e.KeyCode = Keys.OemQuestion And e.Shift = True Then
                If HelpInfo.Visible = False Then
                    HelpInfo.Visible = True
                ElseIf HelpInfo.Visible = True Then
                    HelpInfo.Visible = False
                End If
            End If
            If PlayerCurHitpoints <= 0 Then
                PlayerDead = True
                SNDScores()
                HighScores = Output.Text
                SaveTextToFile(HighScores, CurDir() + "\HighScores.TG")
                Me.Text += " [Dead] : Press f1 to view scores list"
                SND("Game Over.")
            End If
        Else 'game is over input controls
            If e.KeyCode = Keys.F1 Then
                If ScoresBox.Visible = True Then
                    ScoresBox.Visible = False
                ElseIf ScoresBox.Visible = False Then
                    CharStats.Visible = False
                    ScoresBox.Visible = True
                End If
            ElseIf e.KeyCode = Keys.H Or e.KeyCode = Keys.OemQuestion And e.Shift = True Then
                If HelpInfo.Visible = False Then
                    HelpInfo.Visible = True
                ElseIf HelpInfo.Visible = True Then
                    HelpInfo.Visible = False
                End If
            ElseIf e.KeyCode = Keys.F4 Then
                PlayerExperience = 0
                PlayerGold = 0
                PlayerTurns = 0
                PlayerLevel = 0
                PlayerLevelPoints = 0
                PlayerDead = False
                MapLevel = 0
                Initialize(0, EventArgs.Empty)
                Comment11.Visible = False
                For tmp0 = 0 To 19 Step 1
                    ItemInventoryName(tmp0) = ""
                    ItemInventoryQuality(tmp0) = 0
                    ItemInventoryType(tmp0) = 0
                    PlayerEquipArms = 0
                    PlayerEquipChest = 0
                    PlayerEquipFeet = 0
                    PlayerEquipHands = 0
                    PlayerEquipHead = 0
                    PlayerEquipLegs = 0
                    PlayerEquipNArms = ""
                    PlayerEquipNChest = ""
                    PlayerEquipNFeet = ""
                    PlayerEquipNHands = ""
                    PlayerEquipNHead = ""
                    PLayerEquipNLegs = ""
                Next
                Me.Hide()
                ChooseCharacter.TabControl1.SelectedTab = ChooseCharacter.BasicTab
                ChooseCharacter.CharacterName.Text = "Mykil Ironfist"
                ChooseCharacter.Show()
            ElseIf e.KeyCode = Keys.F2 Then
                If CharStats.Visible = True Then
                    CharStats.Visible = False
                ElseIf CharStats.Visible = False Then
                    StatBox.Text = "[Character Stats]" + Chr(13) + "Depth     : " + LTrim(Str(MapLevel)) + Chr(13) + "Level     : " + LTrim(Str(PlayerLevel)) + Chr(13) _
+ "Experience: " + LTrim(Str(PlayerExperience)) + Chr(13) _
+ "Gold      : " + LTrim(Str(PlayerGold)) + Chr(13) _
+ "Turns     : " + LTrim(Str(PlayerTurns)) + Chr(13) + Chr(13) _
+ "Strength    : " + LTrim(Str(PlayerSTR)) + Chr(13) _
+ "Dexterity   : " + LTrim(Str(PlayerDEX)) + Chr(13) _
+ "Intelligence: " + LTrim(Str(PlayerINT)) + Chr(13) _
+ "Wisdom      : " + LTrim(Str(PlayerWIS)) + Chr(13) _
+ "Constitution: " + LTrim(Str(PlayerCON)) + Chr(13) _
+ "Charisma    : " + LTrim(Str(PlayerCHA)) + Chr(13) _
+ "Luck        : " + LTrim(Str(PlayerLUC))
                    StatBox.Visible = True
                    ScoresBox.Visible = False
                    CharStats.Visible = True
                End If
            End If
        End If
    End Sub
    Private Sub CloseCommentBox()
        Comment1.Visible = False
        Comment2.Visible = False
        Comment3.Visible = False
        Comment4.Visible = False
        Comment5.Visible = False
        Comment6.Visible = False
        Comment7.Visible = False
        Comment8.Visible = False
        Comment9.Visible = False
        Comment10.Visible = False
        Comment11.Visible = False
    End Sub
    Private Sub CloseInventory(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Inventory.Close()
        ChooseCharacter.Close()
    End Sub
    Public Sub SNDScores()
        Output.Text = HighScores + Chr(13) + AddSpace(PlayerName, 20) + AddSpace(PlayerRace, 14) + AddSpace(PlayerClass, 17) + AddSpace(LTrim(Str(PlayerLevel)), 8) + AddSpace(LTrim(Str(PlayerExperience)), 13) + AddSpace(LTrim(Str(MapLevel)), 13) + AddSpace(LTrim(Str(PlayerGold)), 10) + LTrim(Str(PlayerTurns))
    End Sub
    Private Sub SkillRollOver(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Skill3.MouseEnter, Skill2.MouseEnter, Skill1.MouseEnter
        SkillInfoBox.Visible = True
        If sender Is Skill1 Then
            ShowInfoboxText(Skill1Name.Text)
        ElseIf sender Is Skill2 Then
            ShowInfoboxText(Skill2Name.Text)
        ElseIf sender Is Skill3 Then
            ShowInfoboxText(Skill3Name.Text)
        End If
    End Sub
    Private Sub ShowInfoboxText(ByVal Skill As String)
        If Skill = "Punch" Then
            SkillInfoBox.Text = "Punch"
            SkillInfo.Text = "Basic attack plus 1 damage. Costs 5 willpower."
        ElseIf Skill = "Heal" Then
            SkillInfoBox.Text = "Heal"
            SkillInfo.Text = "Heals 20 hitpoints. Costs 20 willpower. Causes a 5 round global cooldown of skills."
        ElseIf Skill = "Fire Arrow" Then
            SkillInfoBox.Text = "Fire Arrow"
            SkillInfo.Text = "Strike target for 3 damage in addition to a regular attack. Costs 15 willpower. Causes a 3 round global cooldown of skills."
        ElseIf Skill = "Kick" Then
            SkillInfoBox.Text = "Kick"
            SkillInfo.Text = "Basic attack plus 1 damage. Costs 5 willpower."
        ElseIf Skill = "Bone Shield" Then
            SkillInfoBox.Text = "Bone Shield"
            SkillInfo.Text = "Protects gravedigger from all damage for 5 rounds. Costs 30 willpower. Causes a 10 round global cooldown of skills."
        ElseIf Skill = "Leech" Then
            SkillInfoBox.Text = "Leech"
            SkillInfo.Text = "Gravedigger receives hitpoints in the amount of his attack score, leeched from the enemy. Costs the gravediggers attack score in amount of willpower."
        ElseIf Skill = "Holy Bolt" Then
            SkillInfoBox.Text = "Holy Bolt"
            SkillInfo.Text = "Strikes target for 10 damage in addition to a normal attack. Costs 20 willpower. Causes a 4 round global cooldown of skills."
        ElseIf Skill = "Magic Shield" Then
            SkillInfoBox.Text = "Magic Shield"
            SkillInfo.Text = "Reduces 1 damage from all attacks for 10 rounds. Costs 30 willpower. Causes a 10 round global cooldown of skills."
        ElseIf Skill = "Fireball" Then
            SkillInfoBox.Text = "Fireball"
            SkillInfo.Text = "Attacks target for 10 damage in addition to a regular attack. Costs 30 willpower. Causes a 3 round global cooldown of skills."
        ElseIf Skill = "Hit" Then
            SkillInfoBox.Text = "Hit"
            SkillInfo.Text = "Basic attack plus 1 damage. Costs 5 willpower."
        ElseIf Skill = "Counter" Then
            SkillInfoBox.Text = "Counter"
            SkillInfo.Text = "Counters all attacks from all enemies for 3 rounds. Counters can miss or be dodged. Costs 25 willpower. Causes a 5 round global cooldown of skills."
        ElseIf Skill = "Clumsiness" Then
            SkillInfoBox.Text = "Clumsiness"
            SkillInfo.Text = "Reduces a targets chance to hit by 50 percent. Costs 20 willpower. Causes a 10 round global cooldown of skills."
        ElseIf Skill = "Strike" Then
            SkillInfoBox.Text = "Strike"
            SkillInfo.Text = "Basic attack plus 1 damage. Costs 5 willpower."
        ElseIf Skill = "Immolate" Then
            SkillInfoBox.Text = "Immolate"
            SkillInfo.Text = "Elementalist immolates a fire shield that causes 1 damage to all attackers for 5 rounds. Costs 20 willpower. Causes a 10 round global cooldown of skills."
        ElseIf Skill = "Stab" Then
            SkillInfoBox.Text = "Stab"
            SkillInfo.Text = "Basic attack plus 1 damage. Costs 5 willpower."
        ElseIf Skill = "Wound" Then
            SkillInfoBox.Text = "Wound"
            SkillInfo.Text = "Attack an enemy causing 5 damage in addition to a normal attack. Costs 20 willpower. Causes a 5 round global cooldown of skills."
        ElseIf Skill = "Stun" Then
            SkillInfoBox.Text = "Stun"
            SkillInfo.Text = "Stun target for 3 rounds. Costs 25 willpower. Causes a 5 round global cooldown of skills."
        ElseIf Skill = "Slice" Then
            SkillInfoBox.Text = "Slice"
            SkillInfo.Text = "Basic attack plus 1 damage. Costs 5 willpower."
        ElseIf Skill = "Fury" Then
            SkillInfoBox.Text = "Fury"
            SkillInfo.Text = "Increases damage in regular attacks by 1 for 5 rounds. Costs 20 willpower. Causes a 5 round global cooldown of skills."
        ElseIf Skill = "Sacrifice" Then
            SkillInfoBox.Text = "Sacrifice"
            SkillInfo.Text = "Sacrifice 10 HP and deal regular damage plus 10. Causes a 5 round global cooldown of skills."
        ElseIf Skill = "Trip" Then
            SkillInfoBox.Text = "Trip"
            SkillInfo.Text = "Trip target preventing them from moving or attacking for 2 rounds. Costs 10 willpower. Causes a 2 round global cooldown of skills."
        ElseIf Skill = "Backstab" Then
            SkillInfoBox.Text = "Backstab"
            SkillInfo.Text = "Attack target for regular attack damage plus 10. Costs 25 willpower. Causes a 5 round global cooldown of skills."
        ElseIf Skill = "Block" Then
            SkillInfoBox.Text = "Block"
            SkillInfo.Text = "Reduces all damage for 2 rounds by 2. Costs 15 willpower. Causes a 2 round global cooldown of skills."
        ElseIf Skill = "Slice" Then
            SkillInfoBox.Text = "Slice"
            SkillInfo.Text = "Basic attack plus 1 damage. Costs 5 willpower."
        ElseIf Skill = "Double Slice" Then
            SkillInfoBox.Text = "Double Slice"
            SkillInfo.Text = "Attack the target twice in 1 round. Costs 15 willpower. Causes a 2 round global cooldown of skills."
        ElseIf Skill = "Triple Slice" Then
            SkillInfoBox.Text = "Triple Slice"
            SkillInfo.Text = "Attack the target 3 times in 1 round. Costs 30 willpower. Causes a 4 round global cooldown of skills."
        ElseIf Skill = "Runestrike" Then
            SkillInfoBox.Text = "Runestrike"
            SkillInfo.Text = "Attack the target normally and additionally prevent target from attacking for 2 rounds. Costs 20 willpower. Causes a 3 round global cooldown of skills."
        ElseIf Skill = "Whisper" Then
            SkillInfoBox.Text = "Whisper"
            SkillInfo.Text = "Causes target to die immediately. Costs 50 willpower. Causes a 10 round global cooldown of skills."
        ElseIf Skill = "Empower" Then
            SkillInfoBox.Text = "Empower"
            SkillInfo.Text = "Converts 30 hitpoints into 30 willpower and attacks a target with a basic attack."
        ElseIf Skill = "Silence" Then
            SkillInfoBox.Text = "Silence"
            SkillInfo.Text = "Causes all targets attacking the minstrel to stop attacking for 5 rounds. Costs 50 willpower. Causes a 10 round global cooldown of skills."
        End If
    End Sub
    Private Sub SkillRollOut(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Skill3.MouseLeave, Skill2.MouseLeave, Skill1.MouseLeave
        SkillInfoBox.Visible = False
    End Sub
End Class
