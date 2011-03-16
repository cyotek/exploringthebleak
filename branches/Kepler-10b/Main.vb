﻿Imports System.IO
Public Class MainForm
#Region "Constants"
    Public Const MapSize As Byte = 30 'original 25
    Public Const MaxDepthLevel As Byte = 28
    Public Const MaxMobiles As Byte = 20

    Const ASCII = False
    Const Tiled = True

    Const North As Short = 1
    Const East As Short = 2
    Const South As Short = 3
    Const West As Short = 4

    'mobiles can't walk on anything above 3
    Public Const Wall As Short = 0
    Public Const Floor As Short = 1
    Public Const SpecialFloor As Integer = 2
    Public Const StairsDown As Short = 3
    Public Const Item As Short = 4
    Public Const StairsUp As Short = 5
    Public Const Water As Short = 6
    Public Const Lava As Short = 7
    Public Const Ice As Short = 8

    Const Armor As Short = 6
    Const Weapon As Short = 7
    Const Gold As Short = 8
    Const TheEverspark As Short = 50

    Const Hidden As Short = 0 'used for MapDrawStatus, prevents recursive drawing on something already visible
    Const NotHidden As Short = 1 'ditto
    Const Shadowed As Short = 2 'ditto again

    Public Const TotalEnvironmentTypes As Short = 10

    Const Dungeon = 0
    Const Ruins = 1 'using spiral functions, perlins noise
    Const Tunnels = 2 'diffusion-limited aggregation (Cardinal)
    Const Tunnels2 = 3 'diffusion-limited aggregation (w/ Ordinal or diagonal)
    Const Catacombs = 4
    Const Swamps = 5
    Const Passage = 6 'single passage, stairs up start, stairs down ending
    Const Random = 10
#End Region
#Region "Form Enhancements"
    Public PAD As New Bitmap(1200, 1200)
    Public CANVAS As Graphics = Graphics.FromImage(PAD)
#End Region
#Region "Declarations and Dimensions"
    Public AdminVisible As Boolean = False 'admin mode, allows full map view without exploration. generally used to debug generation techniques
    Public GraphicalMode As Boolean = Tiled

    Public Screensaver As Boolean = True, ScreensaverFound As Boolean, ScreensaverPosition As Integer
    Private ScreensaverMap(MapSize, MapSize) As Integer
    Private ExitPosX, ExitPosY As Short
    Public Initialized As Boolean = False

    Public StandardColor As Color 'used for color types in fog display
    Public GenerateType As Short
    Public EnvironmentType As Byte = 0
    Public MapLevel As Byte = 1
    Public MapCreated(MaxDepthLevel) As Boolean
    Public Map(MaxDepthLevel, MapSize, MapSize) As Byte
    Public MapEntrances(MaxDepthLevel, 1, 1) As Short 'mapentrances(maxdepthlevel,exit type,axis type) exit types(0=down exit, 1=up exit locations) axis types(0=x,1=y)
    Public MapShown(MaxDepthLevel, MapSize, MapSize) As Boolean
    Public MapOccupied(MaxDepthLevel, MapSize, MapSize) As Byte
    Public MapBlur(MaxDepthLevel, MapSize, MapSize, 3) As Boolean
    Public WaterBlur(MaxDepthLevel, MapSize, MapSize, 3) As Boolean
    Public FogMap(MaxDepthLevel, MapSize, MapSize) As Byte
    Public RiverType As Short
    Public WaterImmune, IceImmune, LavaImmune As Short

    Public SKillGobalCooldown As Short 'prevents skills for a amount of time
    Public SkillType As String 'references a skillname if it's going to be used on next attack
    Public BoneShield, MagicShield, CounterAttack, Immolate, Fury, Block, Silence As Short

    Public PlayerPosX, PlayerPosY, PlayerLastPosX, PlayerLastPosY As Short
    Public PlayerExperience As Integer
    Public PlayerLevel As Byte = 1
    Public PlayerName As String
    Public PlayerHidden As Short
    Public PlayerClass As String
    Public PlayerRace As String
    Public PlayerDefense As Byte = 1
    Public PlayerAttack As Byte = 1
    Public PlayerSTR, PlayerDEX, PlayerINT, PlayerWIS, PlayerCON, PlayerCHA, PlayerLUC As Byte
    Public PlayerMaxSTR, PlayerMaxDEX, PlayerMaxINT, PlayerMaxWIS, PlayerMaxCON, PlayerMaxCHA, PlayerMaxLuc As Byte
    Public PlayerEquipHead, PlayerEquipChest, PlayerEquipArms, PlayerEquipHands, PlayerEquipLegs, PlayerEquipFeet As Byte
    Public PlayerEquipQHead, PlayerEquipQChest, PlayerEquipQArms, PlayerEquipQHands, PLayerEquipQLegs, PlayerEquipQFeet As Byte
    Public PlayerEquipNHead, PlayerEquipNChest, PlayerEquipNArms, PlayerEquipNHands, PLayerEquipNLegs, PlayerEquipNFeet As String
    Public PlayerHitpoints, PlayerEnergy As Short
    Public PlayerCurHitpoints, PlayerCurEnergy As Short
    Public PlayerLevelPoints, PlayerLevelRanks, CaptainCurLevel, ScoutCurLevel, TankCurLevel, BenefactorCurLevel, MuleCurLevel As Short
    Public PlayerRankModifier As Short = 1
    Public PlayerTurns As Long
    Public PlayerGold As Short
    Public PlayerDead As Boolean = False
    Public PlayerTargeting As Boolean = False
    Public HealthRejuv, EnergyRejuv As Short
    Public FearPerc As Short
    Public Aggroperc As Short
    Public PlayersPlusRange As UShort
    Public Playermaxitems As Short = 2

    Public HighScores As String

    Public PreviousAttack, PreviousDefense As Short

    Public MobilePosX(MaxDepthLevel, MaxMobiles), MobilePosY(MaxDepthLevel, MaxMobiles), MobilePrevX(MaxDepthLevel, MaxMobiles), MobilePrevY(MaxDepthLevel, MaxMobiles), MobileLastMove(MaxDepthLevel, MaxMobiles), MobileType(MaxDepthLevel, MaxMobiles) As Short
    Public MobileHealth(MaxDepthLevel, MaxMobiles), MobileFlee(MaxDepthLevel, MaxMobiles), MobileStun(MaxDepthLevel, MaxMobiles), MobileClumsiness(MaxDepthLevel, MaxMobiles) As Short
    Public MobileVisible(MaxDepthLevel, MaxMobiles, 3) As Short
    Public MobOccupied(MaxDepthLevel, MapSize, MapSize) As Short '0-9 mobile vnum loc
    Public MobileExists(MaxDepthLevel, MapSize, MapSize) As Boolean 'death flag
    Public MobilePresent As Boolean 'generic mobile is presently on screen to prevent erronerous targeting or skills w/o mobs present

    Public ItemNum(MaxDepthLevel, 39), ItemType(MaxDepthLevel, 39), ItemOccupied(MaxDepthLevel, MapSize, MapSize) As Short
    Public ItemBonusType(MaxDepthLevel, MapSize, MapSize) As Short
    Public ItemNameType(MaxDepthLevel, MapSize, MapSize), ItemShowType(MaxDepthLevel, MapSize, MapSize) As String
    Public InvType(9), InvStrength(9) As Short

    Public SNDLog(27) As String
    Public LogVisible As Boolean = True

    Public TheRoomWidth As Integer = 15 'original is 30
    Public TheRoomHeight As Integer = 15 'original is 30
    Public ColumnsSpace As Integer = 0
    Public RowSpace As Integer = 0

    Public CommentBoxOpen As Boolean = False

    Public MusicOn As Boolean = True
    Public Music_FileToPlay As String
    Public MusicBaseName(9) As String
    Public MusicDecrease(9) As Short
    Public MusicIncrease(9) As Short
    Public MusicIncreaseTar(9) As Short
    Public MusicIncreaseRatio(9) As Short
    Public MusicDecreaseName(9) As String
    Public MusicIncreaseName(9) As String
#End Region
#Region "Basic Functions"
    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Integer, ByVal hwndCallback As Integer) As Integer
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
#Region "Mobile Actions & Battle"
    Function Mv(ByVal Mobnum As Short, ByVal x As Short, ByVal y As Short, Optional ByVal NPC As Boolean = False) 'meta mobile move, all movement passed lastly through here
        Dim PreviousX As Short = MobilePosX(MapLevel, Mobnum)
        Dim PreviousY As Short = MobilePosY(MapLevel, Mobnum)
        If PreviousX >= 0 And PreviousY >= 0 Then
            If NPC = False Then MobileExists(MapLevel, PreviousX, PreviousY) = False
            If NPC = False Then MobOccupied(MapLevel, PreviousX, PreviousY) = 10
            If NPC = False Then MapOccupied(MapLevel, PreviousX, PreviousY) = 0
            MobilePosX(MapLevel, Mobnum) = PreviousX + x
            MobilePosY(MapLevel, Mobnum) = PreviousY + y
            If NPC = False Then MobileExists(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = True
            If NPC = False Then MobOccupied(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = Mobnum
            If NPC = False Then MapOccupied(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = MobileType(MapLevel, Mobnum)
        End If
        Return 0
    End Function
    Function MoveMobile(ByVal MobNum As Short, ByVal MvType As Short, Optional ByVal NPC As Boolean = False)
        Dim MobileDead As Boolean = False
        Dim x As Short = MobilePosX(MapLevel, MobNum)
        Dim y As Short = MobilePosY(MapLevel, MobNum)
        If MvType = North And MobilePosY(MapLevel, MobNum) > 0 Then 'North movement
            If Map(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) - 1) = Item Then 'mobile moves onto an item and picks it up
            ElseIf MobilePosX(MapLevel, MobNum) = PlayerPosX And MobilePosY(MapLevel, MobNum) - 1 = PlayerPosY And NPC = False Then 'mobile moves into player
                'if mobile is npc then obviously it can't move onto player, otherwise if mobile is screensaver player it...can't move on itself? hehe
                KillMob(MapLevel, MobNum)
                MobileDead = True
            End If
            If MobileDead = False Then
                Mv(MobNum, 0, -1, NPC)
                MobileLastMove(MapLevel, MobNum) = North
            End If
        ElseIf MvType = East And MobilePosX(MapLevel, MobNum) < MapSize Then 'East movement
            If Map(MapLevel, MobilePosX(MapLevel, MobNum) + 1, MobilePosY(MapLevel, MobNum)) = Item Then 'mobile moves onto a piece
            ElseIf MobilePosX(MapLevel, MobNum) + 1 = PlayerPosX And MobilePosY(MapLevel, MobNum) = PlayerPosY And NPC = False Then 'mobile moves into player
                'if mobile is npc then obviously it can't move onto player, otherwise if mobile is screensaver player it...can't move on itself? hehe
                KillMob(MapLevel, MobNum)
                MobileDead = True
            End If
            If MobileDead = False Then
                Mv(MobNum, 1, 0, NPC)
                MobileLastMove(MapLevel, MobNum) = East
            End If
        ElseIf MvType = South And MobilePosY(MapLevel, MobNum) < MapSize Then 'south movement
            If Map(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) + 1) = Item Then 'mobile moves onto a piece
            ElseIf MobilePosX(MapLevel, MobNum) = PlayerPosX And MobilePosY(MapLevel, MobNum) + 1 = PlayerPosY And NPC = False Then 'mobile moves into player
                'if mobile is npc then obviously it can't move onto player, otherwise if mobile is screensaver player it...can't move on itself? hehe
                KillMob(MapLevel, MobNum)
                MobileDead = True
            End If
            If MobileDead = False Then
                Mv(MobNum, 0, 1, NPC)
                MobileLastMove(MapLevel, MobNum) = South
            End If
        ElseIf MvType = West And MobilePosX(MapLevel, MobNum) > 0 Then 'west movement
            If Map(MapLevel, MobilePosX(MapLevel, MobNum) - 1, MobilePosY(MapLevel, MobNum)) = Item Then 'mobile moves onto a piece
            ElseIf MobilePosX(MapLevel, MobNum) - 1 = PlayerPosX And MobilePosY(MapLevel, MobNum) = PlayerPosY And NPC = False Then 'mobile moves into player
                'if mobile is npc then obviously it can't move onto player, otherwise if mobile is screensaver player it...can't move on itself? hehe
                KillMob(MapLevel, MobNum)
                MobileDead = True
            End If
            If MobileDead = False Then
                Mv(MobNum, -1, 0, NPC)
                MobileLastMove(MapLevel, MobNum) = West 'dictates the mobiles last movement direction for pattern-making movements
            End If
        End If
        Return 0
    End Function
    Function KillMob(ByVal Mobnum As Short, Optional ByVal MobString As String = "Enemy")
        If MobString <> "SILENCE MOB KILL" Then
            If Map(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = Floor Then
                Map(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = SpecialFloor
            End If
            PlayerExperience += 8  '+ MobileType(MapLevel, Mobnum)  'mobiletype distinguishes it's difficulty and therefor applys likewise to experience gained.
            If PlayerExperience >= 100 Then
                LevelUp()
            End If
            SND(UCase(Mid(MobString, 1, 1)) + Mid(MobString, 2, Len(MobString)) + " is dead.")
            '----------------Chance to drop items--------------
            '50% depth 1-2, 40% 3-4, 30% 5-6, 20% 7+
            If ItemOccupied(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = 0 Then 'ensures that items can't drop on items
                Dim ItemNumber As Short
                Dim ItemLocFound, DropSuccess As Boolean 'only make an item if there's room in item list, no more than 40 items created per map
                Dim RandomNumber As New Random
                Dim RandomValue As Short
                For ItemNumber = 1 To ItemNum.Length Step 1
                    If ItemNum(MapLevel, ItemNumber) = 0 Then
                        ItemNum(MapLevel, ItemNumber) = ItemNumber
                        ItemLocFound = True
                        Exit For
                    End If
                Next
                If ItemLocFound = True Then 'generate item possibility, there is a free item resource location
                    DropSuccess = False
                    RandomValue = RandomNumber.Next(1, 101)
                    If RandomValue <= PlayerLUC * 5 Then DropSuccess = True '5% per luck, (50% @ 10, 80% @ 16)
                    If DropSuccess = True Then 'item will be dropped, yay!
                        'Public NameType As String
                        'Public ItemType As Short
                        'Public ShowType As String
                        GenerateItem.GenerateRandomItem(ItemNumber)
                        ItemType(MapLevel, ItemNumber) = GenerateItem.ItemType
                        ItemShowType(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = GenerateItem.ShowType
                        ItemNameType(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = GenerateItem.NameType
                        ItemBonusType(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = GenerateItem.ItemStrength
                        If LTrim(GenerateItem.NameType) = "" Then 'this prevents stringless items which occur rarely.. remove when bug is found in generate item
                            Return 0
                        End If
                        SND(UCase(Mid(MobString, 1, 1)) + Mid(MobString, 2, Len(MobString)) + " drops " + GenerateItem.NameType + ".")
                        ItemOccupied(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = ItemNum(MapLevel, ItemNumber)
                        DrawingProcedures.LOSMap(MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = DrawingProcedures.Redraw
                    End If
                End If
                MobileHealth(MapLevel, Mobnum) = 0
                MapOccupied(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = False 'clears mob type
                MobileExists(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = False 'kills mob
                MobilePosX(MapLevel, Mobnum) = MapSize + 1 : MobilePosY(MapLevel, Mobnum) = MapSize + 1
            Else
                MobileHealth(MapLevel, Mobnum) = 0
                MapOccupied(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = False 'clears mob type
                MobileExists(MapLevel, MobilePosX(MapLevel, Mobnum), MobilePosY(MapLevel, Mobnum)) = False 'kills mob
                MobilePosX(MapLevel, Mobnum) = MapSize + 1 : MobilePosY(MapLevel, Mobnum) = MapSize + 1
            End If
        End If
        Return 0
    End Function
    Function AssignMobileString(ByVal MobNum As Short) As String
        If MobileType(MapLevel, MobNum) = 1 Then
            Return "Плутон" 'russian for pluto
        ElseIf MobileType(MapLevel, MobNum) = 2 Then
            Return "Нептун" 'russian for neptune
        ElseIf MobileType(MapLevel, MobNum) = 3 Then
            Return "Уран" 'russian for uranus
        ElseIf MobileType(MapLevel, MobNum) = 4 Then
            Return "Сатурн" 'saturn
        ElseIf MobileType(MapLevel, MobNum) = 5 Then
            Return "Юпитер" 'jupiter
        ElseIf MobileType(MapLevel, MobNum) = 6 Then
            Return "Марс" 'mars
        ElseIf MobileType(MapLevel, MobNum) = 7 Then
            Return "Земли" 'earth
        ElseIf MobileType(MapLevel, MobNum) = 8 Then
            Return "Венера" 'venus
        ElseIf MobileType(MapLevel, MobNum) = 9 Then
            Return "ртути" 'mercury
        ElseIf MobileType(MapLevel, MobNum) = 10 Then
            Return "Солнце" 'sun
        ElseIf MobileType(MapLevel, MobNum) = 11 Then
            Return "демон" 'demon in russian
        ElseIf MobileType(MapLevel, MobNum) = 12 Then
            Return "уничтожения" 'russian for destruction
        ElseIf MobileType(MapLevel, MobNum) = 13 Then
            Return "армагеддон" 'russian for armageddon
        Else
            Return "Unknown"
        End If
    End Function
    Function AssignMobileDamage(ByVal mobnum As Short) As Short
        If MobileType(MapLevel, mobnum) = 1 Then
            Return 3
        ElseIf MobileType(MapLevel, mobnum) = 2 Then
            Return 6
        ElseIf MobileType(MapLevel, mobnum) = 3 Then
            Return 9
        ElseIf MobileType(MapLevel, mobnum) = 4 Then
            Return 12
        ElseIf MobileType(MapLevel, mobnum) = 5 Then
            Return 15
        ElseIf MobileType(MapLevel, mobnum) = 6 Then
            Return 18
        ElseIf MobileType(MapLevel, mobnum) = 7 Then
            Return 21
        ElseIf MobileType(MapLevel, mobnum) = 8 Then
            Return 24
        ElseIf MobileType(MapLevel, mobnum) = 9 Then
            Return 27
        ElseIf MobileType(MapLevel, mobnum) = 10 Then
            Return 30
        ElseIf MobileType(MapLevel, mobnum) = 11 Then
            Return 33
        ElseIf MobileType(MapLevel, mobnum) = 12 Then
            Return 36
        ElseIf MobileType(MapLevel, mobnum) = 13 Then
            Return 39
        Else
            PlayerCurHitpoints -= 10
            SND("You trip and damage yourself.")
            Return 0
        End If
    End Function
    Function MobileFleeFail(ByVal Mobnum As Short)
        Dim MobileNameString As String = ""
        MobileNameString = AssignMobileString(Mobnum)
        SND(MobileNameString + " trips in its terror.")
        MobileHealth(MapLevel, Mobnum) -= 1
        If MobileHealth(MapLevel, Mobnum) <= 0 Then
            KillMob(Mobnum, MobileNameString)
            SND(MobileNameString + " falls in a heap dead.")
        End If
        Return 0
    End Function
    Function FleeMob(ByVal Mobnum As Short)
        Dim MobileNameString As String = ""
        MobileNameString = AssignMobileString(Mobnum)
        SND(MobileNameString + " turns to flee.")
        MobileFlee(MapLevel, Mobnum) = Math.Round(PlayerCHA / 10, 0)
        Return 0
    End Function
    Function HitMob(ByVal Mobnum As Short, Optional ByVal Counter As Boolean = False, Optional ByVal HideAttack As Boolean = False)
        Dim MobileNameString As String = ""
        Dim TestCriticalStrike As New Random
        Dim CritStrike As Short = 0
        MobileNameString = AssignMobileString(Mobnum)
        If HideAttack = False Then
            If SkillType = "" Then CritStrike = TestCriticalStrike.Next(0, 101) Else CritStrike = TestCriticalStrike.Next(0, 101)
            If CritStrike <= PlayerSTR And SkillType = "" Then 'player critically striked. chance to critically strike is the players strength
                MobileHealth(MapLevel, Mobnum) -= Val(WeaponBonus.Text) + PlayerAttack * 2
                If Counter = False Then
                    SND("You CRIT " + MobileNameString + ".")
                Else
                    SND("You counter CRITS " + MobileNameString + ".")
                End If
                PlayMusic("PlayerHit")
            ElseIf CritStrike <= PlayerINT * 2 And SkillType <> "" Then 'player critically striked with a skill.
                If SkillType = "Shoot" Then
                    'these are the basic +1 skilltypes, and all do the same
                    MobileHealth(MapLevel, Mobnum) -= Val(WeaponBonus.Text) + PlayerAttack * 2 + 2
                    SND("Your shot CRITS " + MobileNameString + ".")
                    PlayMusic("PlayerShoot")
                End If
            ElseIf SkillType = "" Then 'basic attack, test mobile dodge, then mobile miss
                CritStrike = TestCriticalStrike.Next(0, 101)
                If CritStrike <= 7 Then 'all mobs have 7% chance to dodge
                    If Counter = False Then
                        SND("Your attack is dodged.")
                    Else
                        SND("Your counter is dodged.")
                    End If
                    PlayMusic("PlayerMiss")
                ElseIf CritStrike <= 15 Then 'all attacks have 8% chance to miss
                    If Counter = False Then
                        SND("Your attack misses.")
                    Else
                        SND("Your counter misses.")
                    End If
                    PlayMusic("PlayerMiss")
                Else
                    If Fury > 0 Then 'fury increases regular attack strength by 1
                        MobileHealth(MapLevel, Mobnum) -= Val(WeaponBonus.Text) + PlayerAttack + 1
                    Else
                        MobileHealth(MapLevel, Mobnum) -= Val(WeaponBonus.Text) + PlayerAttack
                    End If
                    If Counter = False Then
                        SND("You hit " + MobileNameString + ".")
                    Else
                        SND("You counter " + MobileNameString + ".")
                    End If
                    PlayMusic("PlayerHit")
                End If
            ElseIf SkillType <> "" Then 'basic skill
                If SkillType = "Shoot" Then 'just a +1 attack
                    MobileHealth(MapLevel, Mobnum) -= Val(WeaponBonus.Text) + PlayerAttack + 1
                    SND("You shoot " + MobileNameString + ".")
                    PlayMusic("PlayerShoot")
                End If
                SkillType = "" 'makes sure you don't use the skill again for free ;0
            End If
        Else
            MobileHealth(MapLevel, Mobnum) -= 1
            SND("Immolation burns " + MobileNameString + ".")
        End If
        If MobileHealth(MapLevel, Mobnum) <= 0 Then
            KillMob(Mobnum, MobileNameString)
        End If
        Return 0
    End Function
    Function PlayerHitLocation(ByVal X As Short, ByVal Y As Short) 'This determines which mobile the player hits then sends it to function "hitmob" to determine damage
        Dim MobXVar As Short
        For MobXVar = 0 To 9 Step 1
            If X = MobilePosX(MapLevel, MobXVar) And Y = MobilePosY(MapLevel, MobXVar) Then
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
        MobileNameString = AssignMobileString(Mobnum)
        DamageAmount = AssignMobileDamage(Mobnum)
        If DamageAmount = 0 Then Return 0 'player trips, exit hitcharacter
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
            PlayMusic("PlayerMiss")
        End If
        TestDodge = TestDodgeRandom.Next(0, 100) 'test miss, 10%
        If TestDodge <= 10 And MobileClumsiness(MapLevel, Mobnum) <= 0 Then
            SupressDueToCriticalStrike = True
            SND(MobileNameString + "'s attack misses.")
            PlayMusic("PlayerMiss")
        ElseIf MobileClumsiness(MapLevel, Mobnum) > 0 And TestDodge <= 50 Then
            SupressDueToCriticalStrike = True
            SND(MobileNameString + "'s attack misses.")
            PlayMusic("PlayerMiss")
        End If
        If MobileClumsiness(MapLevel, Mobnum) > 0 Then MobileClumsiness(MapLevel, Mobnum) -= 1
        If SupressDueToCriticalStrike = False Then
            DamageAmount -= Val(ArmorBonus.Text) + PlayerDefense
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
            PlayMusic("ReceiveHit")
        End If
        HealthBar.Caption = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints)) + " HP"
        HealthBar.Value = PlayerCurHitpoints
        If Aggroperc > 0 Then 'counter percent chance
            TestDodge = TestDodgeRandom.Next(0, 100)
            If TestDodge <= Aggroperc Then 'counter is successful
                HitMob(Mobnum, True)
            End If
        End If
        If Immolate > 0 Then HitMob(Mobnum, False, True)
        Return 0
    End Function
    Function NPCgoStairs(ByVal mobnum As Short)
        Dim CurX As Integer = PlayerPosX, CurY As Integer = PlayerPosY
        Dim LastDirection, ThisDirection As Integer
        Dim CurDirection As Short
        Dim CurSector As Integer
        Dim VisitedSectorX(3), VisitedSectorY(3) As Integer
        Dim ScreensaverTempMap(MapSize, MapSize) As Integer
        '
        Dim CurSec = ScreensaverTempMap(ExitPosX, ExitPosY)
        Dim BestDirection As Short, LastDirectionAmount As Integer
        Dim CurPosX As Short = ExitPosX, CurPosY As Short = ExitPosY
        '
        If ScreensaverFound = False Then
            Array.Clear(ScreensaverMap, 0, ScreensaverMap.Length)
            ScreensaverFound = True
            Try
                Do
                    CurDirection = 0 : LastDirection = 0
                    'the follow details which direction is theoretically the best direction to go
                    If CurX + 1 <= MapSize Then 'east
                        If Map(MapLevel, CurX + 1, CurY) <> Wall And ScreensaverTempMap(CurX + 1, CurY) = 0 Then
                            LastDirection = 10 * (Math.Abs(CurX + 1 - ExitPosX) + Math.Abs(CurY - ExitPosY))
                            CurDirection = East
                        End If
                    End If
                    If CurY + 1 <= MapSize Then 'south
                        If Map(MapLevel, CurX, CurY + 1) <> Wall And ScreensaverTempMap(CurX, CurY + 1) = 0 Then
                            ThisDirection = 10 * (Math.Abs(CurX - ExitPosX) + Math.Abs(CurY + 1 - ExitPosY))
                            If ThisDirection < LastDirection Or LastDirection = 0 Then CurDirection = South
                            LastDirection = ThisDirection
                        End If
                    End If
                    If CurX - 1 >= 0 Then 'west
                        If Map(MapLevel, CurX - 1, CurY) <> Wall And ScreensaverTempMap(CurX - 1, CurY) = 0 Then
                            ThisDirection = 10 * (Math.Abs(CurX - 1 - ExitPosX) + Math.Abs(CurY - ExitPosY))
                            If ThisDirection < LastDirection Or LastDirection = 0 Then CurDirection = West
                            LastDirection = ThisDirection
                        End If
                    End If
                    If CurY - 1 >= 0 Then 'north
                        If Map(MapLevel, CurX, CurY - 1) <> Wall And ScreensaverTempMap(CurX, CurY - 1) = 0 Then
                            ThisDirection = 10 * (Math.Abs(CurX - ExitPosX) + Math.Abs(CurY - 1 - ExitPosY))
                            If ThisDirection < LastDirection Or LastDirection = 0 Then CurDirection = North
                        End If
                    End If
                    If CurDirection = 0 Then
                        If CurSector <= 1 Then
                            'shouldn't ever get here, this means that there isn't any stairs available to the user
                            MapLevel += 1
                            BuildNewMap(True)
                            ScreensaverFound = False
                            Return 0
                        End If
                        CurSector -= 1
                        CurX = VisitedSectorX(CurSector)
                        CurY = VisitedSectorY(CurSector)
                    End If
                    If CurDirection = East Then
                        CurSector += 1
                        CurX += 1
                    ElseIf CurDirection = South Then
                        CurSector += 1
                        CurY += 1
                    ElseIf CurDirection = West Then
                        CurSector += 1
                        CurX -= 1
                    ElseIf CurDirection = North Then
                        CurSector += 1
                        CurY -= 1
                    End If
                    If CurDirection <> 0 And ScreensaverTempMap(CurX, CurY) = 0 Then
                        Array.Resize(VisitedSectorX, VisitedSectorX.Length + 1)
                        Array.Resize(VisitedSectorY, VisitedSectorY.Length + 1)
                        VisitedSectorX(CurSector) = CurX
                        VisitedSectorY(CurSector) = CurY
                        ScreensaverTempMap(CurX, CurY) = CurSector
                    End If
                    'Check to see if you found the stairs
                    If CurX = ExitPosX And CurY = ExitPosY Then
                        ScreensaverTempMap(CurX, CurY) = CurSector
                        Exit Do
                    End If
                Loop
                'RUN IT BACKWARDS AND DELETE OTHERS
                ScreensaverMap(VisitedSectorX(CurSector), VisitedSectorY(CurSector)) = ScreensaverTempMap(VisitedSectorX(CurSector), VisitedSectorY(CurSector))
                CurSec = CurSector
                CurPosX = VisitedSectorX(CurSector)
                CurPosY = VisitedSectorY(CurSector)
                Do
                    BestDirection = 0
                    If CurPosX + 1 <= MapSize Then
                        If ScreensaverTempMap(CurPosX + 1, CurPosY) <> 0 Then
                            BestDirection = East
                            LastDirectionAmount = ScreensaverTempMap(CurPosX + 1, CurPosY)
                        End If
                    End If
                    If CurPosX - 1 >= 0 Then
                        If ScreensaverTempMap(CurPosX - 1, CurPosY) <> 0 Then
                            If ScreensaverTempMap(CurPosX - 1, CurPosY) < LastDirectionAmount Or LastDirectionAmount = 0 Then
                                BestDirection = West
                                LastDirectionAmount = ScreensaverTempMap(CurPosX - 1, CurPosY)
                            End If
                        End If
                    End If
                    If CurPosY + 1 <= MapSize Then
                        If ScreensaverTempMap(CurPosX, CurPosY + 1) <> 0 Then
                            If ScreensaverTempMap(CurPosX, CurPosY + 1) < LastDirectionAmount Or LastDirectionAmount = 0 Then
                                BestDirection = South
                                LastDirectionAmount = ScreensaverTempMap(CurPosX, CurPosY + 1)
                            End If
                        End If
                    End If
                    If CurPosY - 1 >= 0 Then
                        If ScreensaverTempMap(CurPosX, CurPosY - 1) <> 0 Then
                            If ScreensaverTempMap(CurPosX, CurPosY - 1) < LastDirectionAmount Or LastDirectionAmount = 0 Then
                                BestDirection = North
                                LastDirectionAmount = ScreensaverTempMap(CurPosX, CurPosY - 1)
                            End If
                        End If
                    End If
                    If BestDirection <> 0 Then
                        If BestDirection = East Then
                            CurPosX += 1
                        ElseIf BestDirection = South Then
                            CurPosY += 1
                        ElseIf BestDirection = West Then
                            CurPosX -= 1
                        ElseIf BestDirection = North Then
                            CurPosY -= 1
                        End If
                        ScreensaverMap(CurPosX, CurPosY) = ScreensaverTempMap(CurPosX, CurPosY)
                        CurSec -= 1
                    Else
                        Exit Do
                    End If
                Loop Until CurSec = 1
            Catch errorVariable As Exception
                MessageBox.Show(errorVariable.ToString)
            End Try
        End If
        'after or if the dijkstra map is generated formulate the correct direction to move
        BestDirection = 0
        If PlayerPosX + 1 <= MapSize Then
            If ScreensaverMap(PlayerPosX + 1, PlayerPosY) <> 0 And Map(MapLevel, PlayerPosX + 1, PlayerPosY) <> Wall Then
                BestDirection = East
                LastDirectionAmount = ScreensaverMap(PlayerPosX + 1, PlayerPosY)
            End If
        End If
        If PlayerPosX - 1 >= 0 Then
            If ScreensaverMap(PlayerPosX - 1, PlayerPosY) <> 0 And Map(MapLevel, PlayerPosX - 1, PlayerPosY) <> Wall Then
                If ScreensaverMap(PlayerPosX - 1, PlayerPosY) > LastDirectionAmount Or LastDirectionAmount = 0 Then
                    BestDirection = West
                    LastDirectionAmount = ScreensaverMap(PlayerPosX - 1, PlayerPosY)
                End If
            End If
        End If
        If PlayerPosY + 1 <= MapSize Then
            If ScreensaverMap(PlayerPosX, PlayerPosY + 1) <> 0 And Map(MapLevel, PlayerPosX, PlayerPosY + 1) <> Wall Then
                If ScreensaverMap(PlayerPosX, PlayerPosY + 1) > LastDirectionAmount Or LastDirectionAmount = 0 Then
                    BestDirection = South
                    LastDirectionAmount = ScreensaverMap(PlayerPosX, PlayerPosY + 1)
                End If
            End If
        End If
        If PlayerPosY - 1 >= 0 Then
            If ScreensaverMap(PlayerPosX, PlayerPosY - 1) <> 0 And Map(MapLevel, PlayerPosX, PlayerPosY - 1) <> Wall Then
                If ScreensaverMap(PlayerPosX, PlayerPosY - 1) > LastDirectionAmount Or LastDirectionAmount = 0 Then
                    BestDirection = North
                    LastDirectionAmount = ScreensaverMap(PlayerPosX, PlayerPosY - 1)
                End If
            End If
        End If
        If BestDirection <> 0 Then
            ScreensaverPosition += 1
            MoveMobile(mobnum, BestDirection, True)
        End If
        'after or if the dijkstra map is generated formulate the correct direction to move
        Return 0
    End Function
    Function EnsureLength(ByVal Text As String, ByVal Length As Integer) As String
        If Len(Text) >= Length Then
            Return Text
        Else
            Dim x As Integer = Len(Text)
            For x = x To Length - 1 Step 1
                Text = " " + Text
            Next
            Return Text
        End If
    End Function
    Function DetermineMobMov(ByVal MobNum As Short, Optional ByVal NPC As Boolean = False)
        'The way it works:
        '    Can mobile see character? If mobile can see character they race for him whether they have to swim or not. They lose 1 hp each round swimming
        '    If yes, mobile will walk towards him
        '    If no, mobile will walk around randomly but don't swim
        Dim Resolved As Boolean = True
        Dim StepNum As Short = 0
        Dim AlreadyMoved As Boolean = False
        Dim FleeinTerror As New Random
        Dim FleeResult As Short
        If MobilePosX(MapLevel, MobNum) = PlayerPosX And MobilePosY(MapLevel, MobNum) = PlayerPosY And NPC = False Then
            'player stepped on mob, mobile is dead, no path required. This sometimes happens on a bad spawn
            KillMob(MobNum, "SILENCE MOB KILL") 'send optional killmob text that supresses xp and message of mob dead
            Return 0
            Exit Function
        End If
        'if NPC then find path towards stairs
        If NPC = True Then
            NPCgoStairs(MobNum)
            Return 0
        End If
        'check to see if character is close
        If Math.Abs(MobilePosX(MapLevel, MobNum) - PlayerPosX) < 3 And Math.Abs(MobilePosY(MapLevel, MobNum) - PlayerPosY) < 3 And NPC = False Then '3 block radius of visibility or npc
            Resolved = False
        End If
        While Resolved = False And MobileFlee(MapLevel, MobNum) = 0 And PlayerHidden = 0 'this is mobile pathfinding straight to the player
            StepNum += 1
            If PlayerPosX > MobilePosX(MapLevel, MobNum) Then
                'if the variable isn't passed into this if statement, it's because mobile tried moving onto another mobile or wall
                If Map(MapLevel, MobilePosX(MapLevel, MobNum) + 1, MobilePosY(MapLevel, MobNum)) <> Wall Then
                    If MobilePosX(MapLevel, MobNum) + 1 = PlayerPosX And MobilePosY(MapLevel, MobNum) = PlayerPosY Then 'if mobile plans on moving east and character is to the east, hit character instead of move
                        FleeResult = FleeinTerror.Next(0, 101) + FearPerc 'add tanks fear percent to mobile fear percent
                        If FleeResult <= PlayerCHA Then
                            FleeMob(MobNum)
                            Resolved = True
                        Else
                            HitChar(MobNum)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    Else
                        If MapOccupied(MapLevel, MobilePosX(MapLevel, MobNum) + 1, MobilePosY(MapLevel, MobNum)) = 0 Then 'ensures that the sector isn't already occupied by another mobile
                            MoveMobile(MobNum, East)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    End If
                End If
            End If
            If PlayerPosX < MobilePosX(MapLevel, MobNum) And Resolved = False Then
                'if the variable isn't passed into this if statement, it's because mobile tried moving onto another mobile or wall
                If Map(MapLevel, MobilePosX(MapLevel, MobNum) - 1, MobilePosY(MapLevel, MobNum)) <> Wall Then
                    If MobilePosX(MapLevel, MobNum) - 1 = PlayerPosX And MobilePosY(MapLevel, MobNum) = PlayerPosY Then 'if mobile plans on moving west and character is to the west, hit character instead of moving
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
                        If MapOccupied(MapLevel, MobilePosX(MapLevel, MobNum) - 1, MobilePosY(MapLevel, MobNum)) = 0 Then 'ensures that the sector isn't already occupied by another mobile
                            MoveMobile(MobNum, West)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    End If
                End If
            End If
            If PlayerPosY > MobilePosY(MapLevel, MobNum) And Resolved = False Then
                'if the variable isn't passed into this if statement, it's because mobile tried moving onto another mobile or wall
                If Map(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) + 1) <> Wall Then
                    If MobilePosY(MapLevel, MobNum) + 1 = PlayerPosY And MobilePosX(MapLevel, MobNum) = PlayerPosX Then 'if mobile plans on moving south and character is to the south, hit character instead of moving
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
                        If MapOccupied(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) + 1) = 0 Then 'ensures that the sector isn't already occupied by another mobile
                            MoveMobile(MobNum, South)
                            Resolved = True
                            AlreadyMoved = True
                        End If
                    End If
                End If
            End If
            If PlayerPosY < MobilePosY(MapLevel, MobNum) And Resolved = False Then
                'if the variable isn't passed into this if statement, it's because mobile tried moving onto another mobile or wall
                If Map(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) - 1) <> Wall Then
                    If MobilePosY(MapLevel, MobNum) - 1 = PlayerPosY And MobilePosX(MapLevel, MobNum) = PlayerPosX Then 'if mobile plans on moving north and character is to the north, hit character instead of moving
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
                        If MapOccupied(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) - 1) = 0 Then 'ensures that the sector isn't already occupied by another mobile
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
        If MobileFlee(MapLevel, MobNum) > 0 Or PlayerHidden > 0 Then
            MobileFlee(MapLevel, MobNum) -= 1
            If MobileFlee(MapLevel, MobNum) < 0 Then MobileFlee(MapLevel, MobNum) = 0
            Resolved = True
        End If
        If Resolved = True And AlreadyMoved = False Then 'this is random mobile movement since the player isn't visible
            Dim FinishMovement As Boolean = False
            Dim RandomDirection As New Random
            Dim RandomPick As Short = RandomDirection.Next(1)
            If RandomPick = 0 Then RandomPick = MobileLastMove(MapLevel, MobNum) 'continues in same direction unless blocked, 50% chance
            If RandomPick = 1 Then RandomPick = RandomDirection.Next(1, 5) 'makes new path, 50% chance
            Dim Tries As Short = 1
            While FinishMovement = False
                If RandomPick = 1 And MobilePosY(MapLevel, MobNum) > 0 And MobileHealth(MapLevel, MobNum) > 0 Then 'north
                    If MobileLastMove(MapLevel, MobNum) <> South Then
                        If Map(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) - 1) <> Wall And Map(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) - 1) <> Water Then 'is there no walls to the north?
                            If MobilePosY(MapLevel, MobNum) - 1 = PlayerPosY And MobilePosX(MapLevel, MobNum) = PlayerPosX Then
                                If MobileFlee(MapLevel, MobNum) > 0 Then
                                    MobileFleeFail(MobNum)
                                End If
                                'mobile can't move into player, this is set incase the mobile is fleeing
                            Else
                                If MapOccupied(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) - 1) = 0 Then 'doesn't allow mobs to group up in a single sector
                                    FinishMovement = True
                                    MoveMobile(MobNum, North)
                                End If
                            End If
                        End If
                    End If
                ElseIf RandomPick = 2 And MobilePosX(MapLevel, MobNum) < 25 And MobileHealth(MapLevel, MobNum) > 0 Then 'east
                    If MobileLastMove(MapLevel, MobNum) <> West Then
                        'cannot allow mobiles to go back to spots they were just at in random direction.
                        If Map(MapLevel, MobilePosX(MapLevel, MobNum) + 1, MobilePosY(MapLevel, MobNum)) <> Wall And Map(MapLevel, MobilePosX(MapLevel, MobNum) + 1, MobilePosY(MapLevel, MobNum)) <> Water Then 'is there no walls to the east?
                            If MobilePosY(MapLevel, MobNum) = PlayerPosY And MobilePosX(MapLevel, MobNum) + 1 = PlayerPosX Then
                                If MobileFlee(MapLevel, MobNum) > 0 Then
                                    MobileFleeFail(MobNum)
                                End If
                                'mobile can't move into player, this is set incase the mobile is fleeing
                            Else
                                If MapOccupied(MapLevel, MobilePosX(MapLevel, MobNum) + 1, MobilePosY(MapLevel, MobNum)) = 0 Then 'doesn't allow mobs to group up in a single sector
                                    FinishMovement = True
                                    MoveMobile(MobNum, East)
                                End If
                            End If
                        End If
                    End If
                ElseIf RandomPick = 3 And MobilePosY(MapLevel, MobNum) < 25 And MobileHealth(MapLevel, MobNum) > 0 Then 'south
                    If MobileLastMove(MapLevel, MobNum) <> North Then
                        'cannot allow mobiles to go back to spots they were just at in random direction.
                        If Map(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) + 1) <> Wall And Map(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) + 1) <> Water Then 'is there no walls to the south?
                            If MobilePosY(MapLevel, MobNum) + 1 = PlayerPosY And MobilePosX(MapLevel, MobNum) = PlayerPosX Then
                                If MobileFlee(MapLevel, MobNum) > 0 Then
                                    MobileFleeFail(MobNum)
                                End If
                                'mobile can't move into player, this is set incase the mobile is fleeing
                            Else
                                If MapOccupied(MapLevel, MobilePosX(MapLevel, MobNum), MobilePosY(MapLevel, MobNum) + 1) = 0 Then 'doesn't allow mobs to group up in a single sector
                                    FinishMovement = True
                                    MoveMobile(MobNum, South)
                                End If
                            End If
                        End If
                    End If
                ElseIf RandomPick = 4 And MobilePosX(MapLevel, MobNum) > 0 And MobileHealth(MapLevel, MobNum) > 0 Then 'west
                    If MobileLastMove(MapLevel, MobNum) <> East Then
                        'cannot allow mobiles to go back to spots they were just at in random direction.
                        If Map(MapLevel, MobilePosX(MapLevel, MobNum) - 1, MobilePosY(MapLevel, MobNum)) <> Wall And Map(MapLevel, MobilePosX(MapLevel, MobNum) - 1, MobilePosY(MapLevel, MobNum)) <> Water Then 'is there no walls to the west?
                            If MobilePosY(MapLevel, MobNum) = PlayerPosY And MobilePosX(MapLevel, MobNum) - 1 = PlayerPosX Then
                                If MobileFlee(MapLevel, MobNum) > 0 Then
                                    MobileFleeFail(MobNum)
                                End If
                                'mobile can't move into player, this is set incase the mobile is fleeing
                            Else
                                If MapOccupied(MapLevel, MobilePosX(MapLevel, MobNum) - 1, MobilePosY(MapLevel, MobNum)) = 0 Then 'doesn't allow mobs to group up in a single sector
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
                    MobileLastMove(MapLevel, MobNum) = 5
                    Tries = 1
                End If
                RandomPick = RandomDirection.Next(1, 5)
            End While
        End If
        MobilePrevX(MapLevel, MobNum) = MobilePosX(MapLevel, MobNum)
        MobilePrevY(MapLevel, MobNum) = MobilePosY(MapLevel, MobNum)
        Return 0
    End Function
#End Region
#Region "Initialize"
    Private Sub InitWindowSize()
        'this will calculate the height and width that the form should be, check to see if it was already set, and then
        'set it if it wasn't. The reason we check before just setting it is to prevent the form from flickering during the
        'testing of hte width and height.
        Dim OldHeight As Integer = Me.Height
        Dim OldWidth As Integer = Me.Width
        Dim PossibleHeight As Integer
        Dim PossibleWidth As Integer
        Dim Oldscreenheight As Integer = Me.Height 'used to distinguish the correct layout of the width
        PossibleHeight = Screen.PrimaryScreen.WorkingArea.Height - 5 'arranges the height to the screen, assorting tiles to perspective size
        PossibleWidth = PossibleHeight - Oldscreenheight 'ensures the width is correspondant to the height
        If PossibleWidth < PossibleHeight Then PossibleWidth = PossibleHeight
        TheRoomHeight = Math.Round(PossibleHeight / (MapSize + 2), 0) - 4  'test the room height
        TheRoomWidth = Math.Round(PossibleWidth / (MapSize + 2), 0) - 4  'test the room width
        If TheRoomHeight > TheRoomWidth Then TheRoomHeight = TheRoomWidth 'ensures that the window is scaled to the smallest of the two
        If TheRoomWidth > TheRoomHeight Then TheRoomWidth = TheRoomHeight 'ensures that the window is scaled to the smallest of the two
        PossibleWidth = TheRoomWidth * MapSize + MapSize + 11
        If Screensaver = True Then
            PossibleHeight = TheRoomWidth * MapSize + MapSize + 56  'thelast 25 height is for the stat bars, remove that when they're unnecessary
        Else
            PossibleHeight = TheRoomWidth * MapSize + MapSize + 81 'thelast 25 height is for the stat bars, remove that when they're unnecessary
            HUDisplay.Text = "" : HUDisplay.Visible = False 'reset hud as singleplayer is initialized
        End If
        If OldHeight <> PossibleHeight Or OldWidth <> PossibleWidth Then
            Me.Width = PossibleWidth
            Me.Height = PossibleHeight
            Me.CenterToScreen() 'center to the screen
            Panel1.Left = Me.Width / 2 - Panel1.Width / 2
            HUDisplay.Top = 50
            HUDisplay.Left = Me.Width / 2 - HUDisplay.Width / 2
            displayfont = New Font("Arial", -3 + (TheRoomHeight + TheRoomWidth / 2) / 2)
        End If
    End Sub
    Private Sub InitScreensaver()
        If Screensaver = True Then
            HealthBar.Visible = False
            EnergyBar.Visible = False
        Else
            HealthBar.Visible = True
            EnergyBar.Visible = True
        End If
    End Sub
    Private Sub InitStatBar()
        HealthBar.Top = Me.Height - HealthBar.Height - 32
        HealthBar.Left = 0 'arranges the healthbar
        HealthBar.Width = Me.Width / 2
        HealthBar.Height = 25
        EnergyBar.Top = Me.Height - EnergyBar.Height - 32
        EnergyBar.Left = Me.Width / 2 'arrange the Energybar according to the panel
        EnergyBar.Width = Me.Width / 2
        EnergyBar.Height = 25
    End Sub
    Private Sub InitLog()
        Array.Clear(SNDLog, 0, SNDLog.Length)
        'SND("Press '?' or 'h' for help.")
        'SND("You descend to depth 1.")
    End Sub
    Private Sub InitCharacter()
        If Screensaver = False Then
            'Before player classes were implemented you would have to initiate the values
            'of the stats, now this isn't necessary as stats are set with the choosing
            'of a class at teh beginning of hte game
            'PlayerSTR = 10 : PlayerDEX = 10 : PlayerCON = 10 : PlayerINT = 10 : PlayerWIS = 10 : PlayerCHA = 10 : PlayerLUC = 10
            PlayerMaxSTR = PlayerSTR + 5 : PlayerMaxDEX = PlayerDEX + 5
            PlayerMaxINT = PlayerINT + 5 : PlayerMaxWIS = PlayerWIS + 5
            PlayerMaxCON = PlayerCON + 5 : PlayerMaxCHA = PlayerCHA + 5
            PlayerMaxLuc = PlayerLUC + 5
            PlayerDefense = Math.Round(PlayerCON / 5, 0)
            PlayerAttack = Math.Round(PlayerSTR / 5, 0)
            PreviousDefense = PlayerDefense
            PreviousAttack = PlayerAttack
            RefreshStats() 'updates Energy and health bar statistics
        End If
    End Sub
    Private Sub InitHighScores()
        If My.Computer.FileSystem.FileExists(CurDir() + "\HighScores.TG") Then 'this saves the location of the database for future reference if not found in correct spot
            HighScores = GetFileContents(CurDir() + "\HighScores.TG")
        Else
            SaveTextToFile("[Name]              [Race]        [Class]          [Level] [Experience] [Depth]      [Gold]    [Turns]" + Chr(13) + "Jarvis              Gnome         Gravedigger      4       45           6            141       1690", CurDir() + "\HighScores.TG", , True)
            HighScores = GetFileContents(CurDir() + "\HighScores.TG")
        End If
    End Sub
    Private Sub InitMaps()
        Array.Clear(MapCreated, 0, MapCreated.Length) 'set all to hidden
        Array.Clear(Map, 0, Map.Length)
        BuildNewMap()
    End Sub
    Private Sub InitSound()
        PlayMusic("Ambience")
    End Sub
    Public Sub Initialize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'resize windows
        InitWindowSize()
        InitStatBar()
        InitScreensaver()
        InitLog()
        InitCharacter()
        InitHighScores()
        InitMaps()
        InitSound()
        Initialized = True
        Me.Focus()
    End Sub
#End Region
#Region "Build Map"
    Private Sub BuildNewMap(Optional ByVal DirectionTraveled As Boolean = True, Optional ByVal ShowString As String = "")
        Dim RandomNumber As New Random
        Dim GenerateRiverChance As Short = RandomNumber.Next(0, 101)
        CANVAS.FillRectangle(Brushes.Black, 1, 1, 1200, 1200)
        'refresh line of sight for new map
        Array.Clear(LOSMap, 0, LOSMap.Length)
        Array.Clear(MapShown, 0, MapShown.Length) 'clear all shown sectors
        'check to see if the new map was one visited already
        If MapCreated(MapLevel) = False Then 'entering a new map, need to generate
            GenerateMap(8)
            DetermineEnvironment()
            If GenerateType <> Swamps And GenerateType <> Passage And GenerateType <> Catacombs Then 'don't generate a river in a swamp or catacombs
                If GenerateRiverChance < 71 Then '70% chance to draw a river
                    GenerateRiver()
                    RiverType = RandomNumber.Next(6, 9)
                    If EnvironmentType = 3 Then 'lava only
                        RiverType = Lava
                    ElseIf EnvironmentType = 5 Then 'ice only
                        RiverType = Ice
                    ElseIf EnvironmentType = 6 Then 'water only
                        RiverType = Water
                    End If
                End If
            Else 'it's a swamp must set water type to plain water
                RiverType = Water
            End If
            GenerateFog()
            If AdminVisible = False Then PopulateItems() 'don't spawn items on admin visible, admin mode dictates debugging of map variables
            If GenerateType <> Passage And GenerateType <> Catacombs Then 'passage renders beginning and end locations, as does the maze/catacomb
                PopulateEntrances()
            End If
            PopulateMobiles()
            MapCreated(MapLevel) = True
        Else
            DetermineEnvironment()
            If DirectionTraveled = False Then 'up traveled, show down exit
                PlayerPosX = MapEntrances(MapLevel, 0, 0)
                PlayerPosY = MapEntrances(MapLevel, 0, 1)
            Else 'down traveled
                PlayerPosX = MapEntrances(MapLevel, 1, 0)
                PlayerPosY = MapEntrances(MapLevel, 1, 1)
            End If
            'string must be shown here because HUD is @ playerlocation and playerlocation was just found
            If ShowString <> "" Then
                SND("You " + ShowString + " to depth " + LTrim(Str(MapLevel)) + ".")
            End If
        End If
        DrawingProcedures.ChangedMode = True
        ReDraw()
        DrawingProcedures.ChangedMode = False
    End Sub
    Sub GenerateRiver()
        Dim RandomNumber As New Random
        Dim Direction As Byte = RandomNumber.Next(1, 7)
        Dim XPos, XTar As Short 'xposition and x target
        Dim YPos, YTar As Short 'yposition and y target
        Dim LastAxis As Boolean = False 'determines whether x or y goes next
        Dim Shifter As Short 'after reaching target axis, shift water every now and then this way or that
        If Direction = 1 Then 'east-west
            YPos = RandomNumber.Next(0, MapSize + 1)
            Shifter = RandomNumber.Next(-3, 4)
            YTar = YPos + Shifter
            If YTar > MapSize Then YTar = MapSize
            If YTar < 0 Then YTar = 0
            XPos = 0
            While XPos <= MapSize
                Map(MapLevel, XPos, YPos) = Water
                If YPos < YTar And LastAxis = True Then
                    YPos += 1
                    LastAxis = False
                ElseIf YPos > YTar And LastAxis = True Then
                    YPos -= 1
                    LastAxis = False
                ElseIf YPos = YTar Then
                    Shifter = RandomNumber.Next(-5, 6)
                    YTar += Shifter
                    If YTar > MapSize Then YTar = MapSize
                    If YTar < 0 Then YTar = 0
                Else
                    XPos += 1
                    LastAxis = True
                End If
            End While
        ElseIf Direction = 2 Then 'north-south
            XPos = RandomNumber.Next(0, MapSize + 1)
            Shifter = RandomNumber.Next(-3, 4)
            XTar = XPos + Shifter
            If XTar > MapSize Then XTar = MapSize
            If XTar < 0 Then XTar = 0
            YPos = 0
            While YPos <= MapSize
                Map(MapLevel, XPos, YPos) = Water
                If XPos < XTar And LastAxis = True Then
                    XPos += 1
                    LastAxis = False
                ElseIf XPos > XTar And LastAxis = True Then
                    XPos -= 1
                    LastAxis = False
                ElseIf XPos = XTar Then
                    Shifter = RandomNumber.Next(-5, 6)
                    XTar += Shifter
                    If XTar > MapSize Then XTar = MapSize
                    If XTar < 0 Then XTar = 0
                Else
                    YPos += 1
                    LastAxis = True
                End If
            End While
        ElseIf Direction = 3 Then 'east-north
            XPos = RandomNumber.Next(0, MapSize / 2)
            Shifter = RandomNumber.Next(-3, 4)
            XTar = XPos + Shifter
            If XTar > MapSize Then XTar = MapSize
            If XTar < 0 Then XTar = 0
            YPos = 0
            YTar = MapSize 'not used until it reaches the center of map
            While YPos <= MapSize
                Map(MapLevel, XPos, YPos) = Water
                If XPos < XTar And LastAxis = True Then
                    XPos += 1
                    LastAxis = False
                ElseIf XPos > XTar And LastAxis = True Then
                    XPos -= 1
                    LastAxis = False
                ElseIf XPos = XTar Then
                    If XPos = MapSize Then
                        Exit While
                    End If
                    Shifter = RandomNumber.Next(-5, 6)
                    XTar += Shifter
                    If XTar > MapSize Then XTar = MapSize
                    If XTar < 0 Then XTar = 0
                ElseIf YPos = YTar Then
                    Shifter = RandomNumber.Next(-3, 4)
                    YTar += Shifter
                    If YTar > MapSize Then YTar = MapSize
                    If YTar < 0 Then YTar = 0
                Else
                    If YPos > YTar Then YPos -= 2
                    YPos += 1
                    LastAxis = True
                End If
                If YPos > MapSize / 2 Then
                    XTar = MapSize
                    Shifter = RandomNumber.Next(-3, 4)
                    YTar += Shifter
                    If YTar > MapSize Then YTar = MapSize
                    If YTar < 0 Then YTar = 0
                End If
            End While
        ElseIf Direction = 4 Then 'west-north
            XPos = RandomNumber.Next(MapSize / 2, MapSize + 1)
            Shifter = RandomNumber.Next(-3, 4)
            XTar = XPos + Shifter
            If XTar > MapSize Then XTar = MapSize
            If XTar < 0 Then XTar = 0
            YPos = 0
            YTar = MapSize 'not used until it reaches the center of map
            While YPos <= MapSize
                Map(MapLevel, XPos, YPos) = Water
                If XPos < XTar And LastAxis = True Then
                    XPos += 1
                    LastAxis = False
                ElseIf XPos > XTar And LastAxis = True Then
                    XPos -= 1
                    LastAxis = False
                ElseIf XPos = XTar Then
                    If XPos = 0 Then
                        Exit While
                    End If
                    Shifter = RandomNumber.Next(-5, 6)
                    XTar += Shifter
                    If XTar > MapSize Then XTar = MapSize
                    If XTar < 0 Then XTar = 0
                ElseIf YPos = YTar Then
                    Shifter = RandomNumber.Next(-3, 4)
                    YTar += Shifter
                    If YTar > MapSize Then YTar = MapSize
                    If YTar < 0 Then YTar = 0
                Else
                    If YPos > YTar Then YPos -= 2
                    YPos += 1
                    LastAxis = True
                End If
                If YPos > MapSize / 2 Then
                    XTar = 0
                    Shifter = RandomNumber.Next(-3, 4)
                    YTar += Shifter
                    If YTar > MapSize Then YTar = MapSize
                    If YTar < 0 Then YTar = 0
                End If
            End While
        ElseIf Direction = 5 Then 'west-south
            YPos = RandomNumber.Next(MapSize / 2, MapSize + 1)
            Shifter = RandomNumber.Next(-3, 4)
            YTar = YPos + Shifter
            If YTar > MapSize Then YTar = MapSize
            If YTar < 0 Then YTar = 0
            XPos = 0
            XTar = MapSize 'not used until it reaches the center of map
            While XPos <= MapSize
                Map(MapLevel, XPos, YPos) = Water
                If YPos < YTar And LastAxis = True Then
                    YPos += 1
                    LastAxis = False
                ElseIf YPos > YTar And LastAxis = True Then
                    YPos -= 1
                    LastAxis = False
                ElseIf YPos = YTar Then
                    If YPos = MapSize Then
                        Exit While
                    End If
                    Shifter = RandomNumber.Next(-5, 6)
                    YTar += Shifter
                    If YTar > MapSize Then YTar = MapSize
                    If YTar < 0 Then YTar = 0
                ElseIf XPos = XTar Then
                    Shifter = RandomNumber.Next(-3, 4)
                    XTar += Shifter
                    If XTar > MapSize Then XTar = MapSize
                    If XTar < 0 Then XTar = 0
                Else
                    If XPos > XTar Then XPos -= 2
                    XPos += 1
                    LastAxis = True
                End If
                If XPos > MapSize / 2 Then
                    YTar = MapSize
                    Shifter = RandomNumber.Next(-3, 4)
                    XTar += Shifter
                    If XTar > MapSize Then XTar = MapSize
                    If XTar < 0 Then XTar = 0
                End If
            End While
        ElseIf Direction = 6 Then 'east-south
            YPos = RandomNumber.Next(0, MapSize / 2)
            Shifter = RandomNumber.Next(-3, 4)
            YTar = YPos + Shifter
            If YTar > MapSize Then YTar = MapSize
            If YTar < 0 Then YTar = 0
            XPos = MapSize
            XTar = 0 'not used until it reaches the center of map
            While XPos >= 0
                Map(MapLevel, XPos, YPos) = Water
                If YPos < YTar And LastAxis = True Then
                    YPos += 1
                    LastAxis = False
                ElseIf YPos > YTar And LastAxis = True Then
                    YPos -= 1
                    LastAxis = False
                ElseIf YPos = YTar Then
                    If YPos = MapSize Then
                        Exit While
                    End If
                    Shifter = RandomNumber.Next(-5, 6)
                    YTar += Shifter
                    If YTar > MapSize Then YTar = MapSize
                    If YTar < 0 Then YTar = 0
                ElseIf XPos = XTar Then
                    Shifter = RandomNumber.Next(-3, 4)
                    XTar += Shifter
                    If XTar > MapSize Then XTar = MapSize
                    If XTar < 0 Then XTar = 0
                Else
                    If XPos > XTar Then XPos -= 2
                    XPos += 1
                    LastAxis = True
                End If
                If XPos > MapSize / 2 Then
                    YTar = MapSize
                    Shifter = RandomNumber.Next(-3, 4)
                    XTar += Shifter
                    If XTar > MapSize Then XTar = MapSize
                    If XTar < 0 Then XTar = 0
                End If
            End While
        End If
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
            If Map(MapLevel, RandomPosX, RandomPosY) = Floor Then
                Foundentrance = True
                If MapLevel >= 2 Then 'ensures that the player can't go to levels before 1
                    Map(MapLevel, RandomPosX, RandomPosY) = StairsUp 'uncomment this to allow stairs up
                    EntrancePosX = RandomPosX 'uncomment this to allow stairs up
                    EntrancePosY = RandomPosY 'uncomment this to allow stairs up
                    MapEntrances(MapLevel, 1, 0) = RandomPosX
                    MapEntrances(MapLevel, 1, 1) = RandomPosY
                End If
                PlayerPosX = RandomPosX : DrawingProcedures.PrevPlayerPosX(0) = PlayerPosX : DrawingProcedures.PrevPlayerPosX(2) = PlayerPosX : DrawingProcedures.PrevPlayerPosX(2) = PlayerPosX : DrawingProcedures.PrevPlayerPosX(3) = PlayerPosX
                PlayerPosY = RandomPosY : DrawingProcedures.PrevPlayerPosY(0) = PlayerPosY : DrawingProcedures.PrevPlayerPosY(2) = PlayerPosY : DrawingProcedures.PrevPlayerPosY(2) = PlayerPosY : DrawingProcedures.PrevPlayerPosY(3) = PlayerPosY
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
            If Map(MapLevel, RandomPosX, RandomPosY) = Floor Then
                If Math.Abs(RandomPosX - EntrancePosX) >= 5 Or Math.Abs(RandomPosY - EntrancePosY) >= 5 Then
                    Map(MapLevel, RandomPosX, RandomPosY) = StairsDown
                    MapEntrances(MapLevel, 0, 0) = RandomPosX
                    MapEntrances(MapLevel, 0, 1) = RandomPosY
                    ExitPosX = RandomPosX
                    ExitPosY = RandomPosY
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
    End Sub
    Sub PopulateItems()
        Dim RandomNum As New Random
        Dim RandomPosX As Short = RandomNum.Next(1, MapSize - 1)
        Dim RandomPosY As Short = RandomNum.Next(1, MapSize - 1)
        Dim RandomItemType As Short = RandomNum.Next(1, 6)
        Dim ItemNumber As Short 'otherwise known in other parts as ItemNum
        Dim FoundPosition As Boolean = False
        Dim MaxItems As Short = Math.Round(PlayerLUC - 8, 0)
        If MaxItems < 0 Then MaxItems = 0 'luck shouldn't be below 8 but if it is, just don't spawn items
        'clear previous map occupied first
        System.Array.Clear(ItemOccupied, 0, ItemOccupied.Length)
        System.Array.Clear(ItemNum, 0, ItemNum.Length) 'needs to be cleared if mobiles drop items
        'initiate population
        Dim Tries As Short = 0
        For ItemNumber = 0 To MaxItems Step 1
            FoundPosition = False
            ItemNum(MapLevel, ItemNumber) = ItemNumber
            Tries = 0
            While FoundPosition = False
                Tries += 1
                If Tries > 1000 Then
                    'no place to put the item, recursion too high, exit (catch)
                    Exit While
                End If
                RandomPosX = RandomNum.Next(1, MapSize - 1) 'don't want to start a mobile on the edge of the map... just because it doesn't look pretty
                RandomPosY = RandomNum.Next(1, MapSize - 1) 'don't want to start a mobile on the edge of the map... just because it doesn't look pretty
                If Map(MapLevel, RandomPosX, RandomPosY) = 1 Then 'if map sector is floor (can't draw items onto a wall.. that's just silly)
                    ItemOccupied(MapLevel, RandomPosX, RandomPosY) = ItemNum(MapLevel, ItemNumber)
                    GenerateItem.GenerateRandomItem(ItemNumber)
                    'Public NameType As String
                    'Public ItemType As Short
                    'Public ShowType As String
                    ItemType(MapLevel, ItemNumber) = GenerateItem.ItemType
                    ItemShowType(MapLevel, RandomPosX, RandomPosY) = GenerateItem.ShowType
                    ItemNameType(MapLevel, RandomPosX, RandomPosY) = GenerateItem.NameType
                    ItemBonusType(MapLevel, RandomPosX, RandomPosY) = GenerateItem.ItemStrength
                    If MapLevel = MaxDepthLevel And ItemNumber = MaxItems Then 'the reason we show it on item nine is because i allowed items to spawn over each other, this is
                        ItemType(MapLevel, ItemNumber) = TheEverspark 'an easy way to ensure that there's not always 10 items.
                    End If
                    FoundPosition = True
                    If LTrim(GenerateItem.NameType) = "" Then 'this prevents stringless items which occur rarely.. remove when bug is found in generate item
                        ItemOccupied(MapLevel, RandomPosX, RandomPosY) = 0
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
                MapOccupied(MapLevel, RandomPosX, RandomPosY) = 0
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
                If Map(MapLevel, RandomPosX, RandomPosY) = Floor Then
                    If RandomPosX = PlayerPosX And RandomPosY = PlayerPosY Then
                        'space for expansion if mobile falls on player. right now it's not allowed
                    Else
                        MapOccupied(MapLevel, RandomPosX, RandomPosY) = RandomMobType 'assign mobiles random type
                        MobOccupied(MapLevel, RandomPosX, RandomPosY) = MobileNumber
                        MobilePosX(MapLevel, MobileNumber) = RandomPosX : MobilePosY(MapLevel, MobileNumber) = RandomPosY
                        MobilePrevX(MapLevel, MobileNumber) = RandomPosX : MobilePrevY(MapLevel, MobileNumber) = RandomPosY
                        MobileType(MapLevel, MobileNumber) = RandomMobType
                        If RandomMobType = 1 Then 'assign the mobiles health depending on their type
                            MobileHealth(MapLevel, MobileNumber) = 2 + MapLevel
                        ElseIf RandomMobType = 2 Then
                            MobileHealth(MapLevel, MobileNumber) = 2 + MapLevel
                        ElseIf RandomMobType = 3 Then
                            MobileHealth(MapLevel, MobileNumber) = 3 + MapLevel
                        ElseIf RandomMobType = 4 Then
                            MobileHealth(MapLevel, MobileNumber) = 3 + MapLevel
                        ElseIf RandomMobType = 5 Then
                            MobileHealth(MapLevel, MobileNumber) = 3 + MapLevel
                        ElseIf RandomMobType = 6 Then
                            MobileHealth(MapLevel, MobileNumber) = 5 + MapLevel
                        ElseIf RandomMobType = 7 Then
                            MobileHealth(MapLevel, MobileNumber) = 5 + MapLevel
                        ElseIf RandomMobType = 8 Then
                            MobileHealth(MapLevel, MobileNumber) = 5 + MapLevel
                        ElseIf RandomMobType = 9 Then
                            MobileHealth(MapLevel, MobileNumber) = 5 + MapLevel
                        ElseIf RandomMobType = 10 Then
                            MobileHealth(MapLevel, MobileNumber) = 10 + MapLevel
                        ElseIf RandomMobType = 11 Then
                            MobileHealth(MapLevel, MobileNumber) = 10 + MapLevel
                        ElseIf RandomMobType = 12 Then
                            MobileHealth(MapLevel, MobileNumber) = 10 + MapLevel
                        ElseIf RandomMobType = 13 Then
                            MobileHealth(MapLevel, MobileNumber) = 10 + MapLevel
                        Else 'this is a catch to ensure that mobile types stay within known bounds in case environments are ever added, set all future mobiles
                            MobileHealth(MapLevel, MobileNumber) = 1 'to retain the same hitpoints as the rat (1)
                            MobileType(MapLevel, MobileNumber) = 1
                            MapOccupied(MapLevel, RandomPosX, RandomPosY) = 1
                        End If
                        MobileExists(MapLevel, RandomPosX, RandomPosY) = True 'set mobile to living
                        FoundPosition = True
                    End If
                End If
            End While
        Next
    End Sub
    Sub GenerateMap(ByVal Recursion As Short)
        Dim RepeatToRecursion As Short = 0
        Dim RandomNumber As New Random
        Dim BuilderDirection As Short = 0
        Dim BuilderLastDirection As Short = 0
        Dim BuilderPositionX As Short = RandomNumber.Next(2, MapSize)
        Dim BuilderPositionY As Short = RandomNumber.Next(2, MapSize)
        GenerateType = Ruins
        If GenerateType = Ruins Then
            Dim RandomRuin As New Random
            Dim RuinStrength As Short
            Dim MaximumRuins As Short = MapSize
            Dim CurrentRuin As Short = 0
            Dim RuinDispersion As Short
            Dim RuinMaxDispersion As Short = Math.Floor(MapSize / 3)
            'forest starts with a clean slate of floor instead of walls, must paint map first
            For BuilderPositionX = 0 To MapSize Step 1
                For BuilderPositionY = 0 To MapSize Step 1
                    Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
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
                            Map(MapLevel, Math.Floor(X), Math.Floor(Y)) = Wall
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
            Dim MapTrodAmount(9, MapSize, MapSize) As Short
            Dim MapGenLevel(9, MapSize, MapSize) As Short
            Dim TrodLevel As Short = 1
            Dim TrodTry As Boolean 'used in while statement to test in a circle , then +1, +2, up to +5 then exits the occupied space and goes to next space
            Dim TrodHead As Boolean
            Dim BuilderTestPosX, BuilderTestPosY As Short
            For BuilderPositionX = 0 To MapSize Step 1
                For BuilderPositionY = 0 To MapSize Step 1
                    If StartPositionFound = False And CurrentOccupied < 10 Then 'finding a location to start searching
                        If Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor And MapTrod(BuilderPositionX, BuilderPositionY) = 0 Then
                            StartPositionFound = True
                            MapTrod(BuilderPositionX, BuilderPositionY) += 1
                            BuilderTestPosX = BuilderPositionX
                            BuilderTestPosY = BuilderPositionY
                            MapGenLevel(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                            'MapOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                            'ItemOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1
                            'ItemShowType(MapLevel, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                        End If
                    Else 'already searching a location
                        TrodHead = True
                        While TrodHead = True
                            TrodTry = False : TrodLevel = 0
                            While TrodTry = False
                                'test up
                                If BuilderTestPosY > 0 Then 'ensure it's within bounds
                                    If Map(MapLevel, BuilderTestPosX, BuilderTestPosY - 1) <> Wall Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX, BuilderTestPosY - 1) = TrodLevel Then 'found next trod
                                            BuilderTestPosY -= 1
                                            If MapTrodAmount(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) < 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                                MapTrodAmount(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) += 1
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapGenLevel(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            'MapOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(MapLevel, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test right
                                If BuilderTestPosX < MapSize Then 'ensure it's within bounds
                                    If Map(MapLevel, BuilderTestPosX + 1, BuilderTestPosY) <> Wall Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX + 1, BuilderTestPosY) = TrodLevel Then 'found next trod
                                            BuilderTestPosX += 1
                                            If MapTrodAmount(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) < 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                                MapTrodAmount(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) += 1
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapGenLevel(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            'MapOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(MapLevel, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test down
                                If BuilderTestPosY < MapSize Then 'ensure it's within bounds
                                    If Map(MapLevel, BuilderTestPosX, BuilderTestPosY + 1) <> Wall Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX, BuilderTestPosY + 1) = TrodLevel Then 'found next trod
                                            BuilderTestPosY += 1
                                            If MapTrodAmount(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) < 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                                MapTrodAmount(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) += 1
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapGenLevel(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            'MapOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(MapLevel, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test left
                                If BuilderTestPosX > 0 Then 'ensure it's within bounds
                                    If Map(MapLevel, BuilderTestPosX - 1, BuilderTestPosY) <> Wall Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX - 1, BuilderTestPosY) = TrodLevel Then 'found next trod
                                            BuilderTestPosX -= 1
                                            If MapTrodAmount(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) < 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                                MapTrodAmount(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) += 1
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapGenLevel(CurrentOccupied, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            'MapOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(MapLevel, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
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
            Dim CurrentLargest = 1
            Dim Wallnumber As Short = 0
            For CurInt = 0 To 9 Step 1
                If OccupiedSquares(CurInt) > OccupiedSquares(CurrentLargest) Then CurrentLargest = CurInt
            Next
            For MapStepX = 0 To MapSize Step 1
                For MapStepY = 0 To MapSize Step 1
                    If MapGenLevel(CurrentLargest, MapStepX, MapStepY) = CurrentLargest Then
                        Map(MapLevel, MapStepX, MapStepY) = Floor
                        Wallnumber += 1
                    Else
                        Map(MapLevel, MapStepX, MapStepY) = Wall
                    End If
                Next
            Next
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
                FogMap(MapLevel, BuilderPositionX, BuilderPositionY) = noiseFloor(BuilderPositionX, BuilderPositionY) 'show the current state number, debug
            Next
        Next
    End Sub
#End Region
#Region "Tick"
    Sub ReDraw() 'also known as 'tick'
        'check to see if the player is in water and reduce their Energy
        If Map(MapLevel, PlayerPosX, PlayerPosY) = Water Then
            Dim Ignorewater = False
            If LavaImmune = 0 Then
                PlayerCurEnergy -= 30
            Else
                Ignorewater = True
            End If
            If PlayerCurEnergy <= 0 Then
                PlayerCurHitpoints += PlayerCurEnergy
                SND("Your shield depletes your energy.")
                SND("You burn for " + LTrim(Str(Math.Abs(PlayerCurEnergy))) + "HP.")
                PlayerCurEnergy = 0
            ElseIf Ignorewater = True Then
                SND("You remain immune.")
                If LavaImmune > 0 Then
                    LavaImmune -= 1
                    If LavaImmune = 0 Then
                        SND("Burn immunity wears off.")
                    End If
                End If
            Else
                SND("You burn reducing energy levels by 30.")
            End If
            RefreshStats() 'updates Energy and health bar statistics
        End If
        'Process the mobiles on the map and move them one at a time.
        Dim ProcessMobilePathNumber As Short = 0
        Dim MobileNameString As String
        For ProcessMobilePathNumber = 0 To 9 Step 1
            If MobileHealth(MapLevel, ProcessMobilePathNumber) > 0 Then
                If Silence <= 0 Then
                    If MobileStun(MapLevel, ProcessMobilePathNumber) <= 0 Then
                        If MobileHealth(MapLevel, ProcessMobilePathNumber) > 0 Then
                            DetermineMobMov(ProcessMobilePathNumber)
                        End If
                    Else
                        'mobile is stunned and can't move
                        SND("Stunned enemy struggles.")
                        'reduce the current time left on stun
                        MobileStun(MapLevel, ProcessMobilePathNumber) -= 1
                    End If
                End If
                'after mobile moves, check to see if its on water and then reduce its health as it drowns, no mobile can swim
                Try
                    If Map(MapLevel, MobilePosX(MapLevel, ProcessMobilePathNumber), MobilePosY(MapLevel, ProcessMobilePathNumber)) = Water Then
                        MobileNameString = AssignMobileString(MobileType(MapLevel, ProcessMobilePathNumber))
                        If RiverType = Water Then
                            MobileHealth(MapLevel, ProcessMobilePathNumber) -= 1
                            SND(MobileNameString + " is drowning.")
                        ElseIf RiverType = Ice Then
                            MobileHealth(MapLevel, ProcessMobilePathNumber) -= 2
                            SND(MobileNameString + " is freezing.")
                        ElseIf RiverType = Lava Then
                            MobileHealth(MapLevel, ProcessMobilePathNumber) -= 3
                            SND(MobileNameString + " is burning.")
                        End If
                        If MobileHealth(MapLevel, ProcessMobilePathNumber) <= 0 Then
                            KillMob(ProcessMobilePathNumber, MobileNameString)
                        End If
                    End If
                Catch
                End Try
            End If
        Next
        'reduce global cooldown of skill by one round, as a round has just passed
        SKillGobalCooldown -= 1
        If SKillGobalCooldown <= 0 Then
            SKillGobalCooldown = 0
        End If
        'reduce all cooldowns of skills that use cooldowns, cooldown can't go below 0
        Silence -= 1 : If Silence <= 0 Then Silence = 0 Else SND("The room remains pacified.")
        PlayerHidden -= 1 : If PlayerHidden <= 0 Then PlayerHidden = 0
        BoneShield -= 1 : If BoneShield <= 0 Then BoneShield = 0
        MagicShield -= 1 : If MagicShield <= 0 Then MagicShield = 0
        CounterAttack -= 1 : If CounterAttack <= 0 Then CounterAttack = 0
        Immolate -= 1 : If Immolate <= 0 Then Immolate = 0
        Fury -= 1 : If Fury <= 0 Then Fury = 0
        Block -= 1 : If Block <= 0 Then Block = 0
        'that ends a turn, add the turn to the players score
        PlayerTurns += 1
        'draw map last to complete the tick
        DrawingProcedures.DrawMap(GraphicalMode) 'graphical mode is either tiled (true) or ASCII (False)
    End Sub
#End Region
#Region "Level Up"
    Private Sub LevelUp()
        PlayerExperience -= 100
        If PlayerLevel < 100 Then
            PlayerLevel += 1
        End If
        strcur.Text = LTrim(Str(PlayerSTR)) : strmax.Text = LTrim(Str(PlayerMaxSTR)) : If PlayerSTR < PlayerMaxSTR Then stradd.Enabled = True
        dexcur.Text = LTrim(Str(PlayerDEX)) : dexmax.Text = LTrim(Str(PlayerMaxDEX)) : If PlayerDEX < PlayerMaxDEX Then dexadd.Enabled = True
        intcur.Text = LTrim(Str(PlayerINT)) : intmax.Text = LTrim(Str(PlayerMaxINT)) : If PlayerINT < PlayerMaxINT Then intadd.Enabled = True
        wiscur.Text = LTrim(Str(PlayerWIS)) : wismax.Text = LTrim(Str(PlayerMaxWIS)) : If PlayerWIS < PlayerMaxWIS Then wisadd.Enabled = True
        concur.Text = LTrim(Str(PlayerCON)) : conmax.Text = LTrim(Str(PlayerMaxCON)) : If PlayerCON < PlayerMaxCON Then conadd.Enabled = True
        chacur.Text = LTrim(Str(PlayerCHA)) : chamax.Text = LTrim(Str(PlayerMaxCHA)) : If PlayerCHA < PlayerMaxCHA Then chaadd.Enabled = True
        luccur.Text = LTrim(Str(PlayerLUC)) : lucmax.Text = LTrim(Str(PlayerMaxLuc)) : If PlayerLUC < PlayerMaxLuc Then lucadd.Enabled = True
        hpcur.Text = LTrim(Str(PlayerHitpoints)) : hpadd.Enabled = True
        wpcur.Text = LTrim(Str(PlayerEnergy)) : wpadd.Enabled = True
        PlayerLevelPoints += Math.Round(PlayerWIS / 4, 0)
        PlayerLevelRanks += PlayerRankModifier
        CurRanks.Text = LTrim(Str(PlayerLevelRanks))
        CurPoints.Text = LTrim(Str(PlayerLevelPoints))
        If CaptainCurLevel < 5 Then AddCaptain.Enabled = True
        If MuleCurLevel < 5 Then AddMule.Enabled = True
        If ScoutCurLevel < 5 Then AddScout.Enabled = True
        If BenefactorCurLevel < 5 Then AddBenefactor.Enabled = True
        If TankCurLevel < 5 Then AddTank.Enabled = True
        HelpInfo.Visible = False
        LevelUpPanel.Visible = True
    End Sub
    Private Sub ShowLevelUpStrip(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LevelStrip.Click
        If LevelUpPanel.Visible = False Then
            LevelUpPanel.Visible = True
        Else
            LevelUpPanel.Visible = False
        End If
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
    Private Sub captainadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCaptain.Click
        If Val(CurRanks.Text) > 0 And CaptainCurLevel < 5 Then
            PlayerLevelRanks -= 1
            CaptainCurLevel += 1
            PlayerRankModifier += 1 'captains beneficiary
            CurCaptain.Text = LTrim(Str(CaptainCurLevel))
            ResetRankNextDescriptions()
            CurRanks.Text = LTrim(Str(Val(CurRanks.Text) - 1))
            If Val(CurRanks.Text) = 0 Then DisableRankAdds()
        Else
            AddCaptain.Enabled = False
        End If
    End Sub
    Private Sub muleadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddMule.Click
        If Val(CurRanks.Text) > 0 And MuleCurLevel < 5 Then
            PlayerLevelRanks -= 1
            MuleCurLevel += 1
            Playermaxitems = MuleCurLevel * 2 'mules beneficiary
            CurMule.Text = LTrim(Str(MuleCurLevel))
            ResetRankNextDescriptions()
            CurRanks.Text = LTrim(Str(Val(CurRanks.Text) - 1))
            If Val(CurRanks.Text) = 0 Then DisableRankAdds()
        Else
            AddMule.Enabled = False
        End If
    End Sub
    Private Sub scoutadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddScout.Click
        If Val(CurRanks.Text) > 0 And ScoutCurLevel < 5 Then
            PlayerLevelRanks -= 1
            ScoutCurLevel += 1
            PlayersPlusRange = ScoutCurLevel - 1 'Scouts Beneficiary
            CurScout.Text = LTrim(Str(ScoutCurLevel))
            ResetRankNextDescriptions()
            CurRanks.Text = LTrim(Str(Val(CurRanks.Text) - 1))
            If Val(CurRanks.Text) = 0 Then DisableRankAdds()
        Else
            AddScout.Enabled = False
        End If
    End Sub
    Private Sub tankadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTank.Click
        If Val(CurRanks.Text) > 0 And TankCurLevel < 5 Then
            PlayerLevelRanks -= 1
            TankCurLevel += 1
            'tanks beneficiary
            If TankCurLevel = 1 Then
                Aggroperc = 0
            ElseIf TankCurLevel = 2 Then
                Aggroperc = 25
            ElseIf TankCurLevel = 3 Then
                Aggroperc = 50
            ElseIf TankCurLevel = 4 Then
                Aggroperc = 75
            ElseIf TankCurLevel = 5 Then
                FearPerc = 100
            End If
            CurTank.Text = LTrim(Str(TankCurLevel))
            ResetRankNextDescriptions()
            CurRanks.Text = LTrim(Str(Val(CurRanks.Text) - 1))
            If Val(CurRanks.Text) = 0 Then DisableRankAdds()
        Else
            AddTank.Enabled = False
        End If
    End Sub
    Private Sub benefactoradd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddBenefactor.Click
        If Val(CurRanks.Text) > 0 And BenefactorCurLevel < 5 Then
            PlayerLevelRanks -= 1
            BenefactorCurLevel += 1
            If BenefactorCurLevel = 1 Then
                EnergyRejuv = 0
            ElseIf BenefactorCurLevel = 2 Then
                EnergyRejuv = 1
            ElseIf BenefactorCurLevel = 3 Then
                EnergyRejuv = 3
            ElseIf BenefactorCurLevel = 4 Then
                EnergyRejuv = 1
            ElseIf BenefactorCurLevel = 5 Then
                HealthRejuv = 3
            End If
            CurBenefactor.Text = LTrim(Str(BenefactorCurLevel))
            ResetRankNextDescriptions()
            CurRanks.Text = LTrim(Str(Val(CurRanks.Text) - 1))
            If Val(CurRanks.Text) = 0 Then DisableRankAdds()
        Else
            AddBenefactor.Enabled = False
        End If
    End Sub
    Private Sub ResetRankNextDescriptions()
        If CaptainCurLevel = 1 Then
            CaptainNextBenefit.Text = "2 Ranks"
        ElseIf CaptainCurLevel = 2 Then
            CaptainNextBenefit.Text = "3 Ranks"
        ElseIf CaptainCurLevel = 3 Then
            CaptainNextBenefit.Text = "4 Ranks"
        ElseIf CaptainCurLevel = 4 Then
            CaptainNextBenefit.Text = "5 Ranks"
        ElseIf CaptainCurLevel = 5 Then
            CaptainNextBenefit.Text = "Max Rank Acquired"
        End If
        If MuleCurLevel = 1 Then
            MuleNextBenefit.Text = "4 Items"
        ElseIf MuleCurLevel = 2 Then
            MuleNextBenefit.Text = "6 Items"
        ElseIf MuleCurLevel = 3 Then
            MuleNextBenefit.Text = "8 Items"
        ElseIf MuleCurLevel = 4 Then
            MuleNextBenefit.Text = "10 Items"
        ElseIf MuleCurLevel = 5 Then
            MuleNextBenefit.Text = "Max Rank Acquired"
        End If
        If ScoutCurLevel = 1 Then
            ScoutNextBenefit.Text = "+1 Range"
        ElseIf ScoutCurLevel = 2 Then
            ScoutNextBenefit.Text = "+2 Range"
        ElseIf ScoutCurLevel = 3 Then
            ScoutNextBenefit.Text = "+3 Range"
        ElseIf ScoutCurLevel = 4 Then
            ScoutNextBenefit.Text = "+4 Range"
        ElseIf ScoutCurLevel = 5 Then
            ScoutNextBenefit.Text = "Max Rank Acquired"
        End If
        If TankCurLevel = 1 Then
            TankNextBenefit.Text = "25% Chance to counter"
        ElseIf TankCurLevel = 2 Then
            TankNextBenefit.Text = "50% Chance to counter"
        ElseIf TankCurLevel = 3 Then
            TankNextBenefit.Text = "75% Chance to counter"
        ElseIf TankCurLevel = 4 Then
            TankNextBenefit.Text = "100% Chance to counter"
        ElseIf TankCurLevel = 5 Then
            TankNextBenefit.Text = "Max Rank Acquired"
        End If
        If BenefactorCurLevel = 1 Then
            BenefactorNextBenefit.Text = "+1 Energy per round"
        ElseIf BenefactorCurLevel = 2 Then
            BenefactorNextBenefit.Text = "+3 Energy per round"
        ElseIf BenefactorCurLevel = 3 Then
            BenefactorNextBenefit.Text = "+1 Health per round"
        ElseIf BenefactorCurLevel = 4 Then
            BenefactorNextBenefit.Text = "+3 Health per round"
        ElseIf BenefactorCurLevel = 5 Then
            BenefactorNextBenefit.Text = "Max Rank Acquired"
        End If
    End Sub
    Private Sub DisableRankAdds()
        AddCaptain.Enabled = False
        AddMule.Enabled = False
        AddScout.Enabled = False
        AddTank.Enabled = False
        AddBenefactor.Enabled = False
    End Sub
    Private Sub DoneBttn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DoneBttn.Click
        'if you don't disable the buttons then it will ding as they will be selected even though the form isn't visible
        disableadds()
        DisableRankAdds()
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
        PlayerEnergy = Val(wpcur.Text)
        PlayerDefense += Math.Round(PlayerCON / 5, 0) - Math.Round(PrevCon / 5, 0)
        PlayerAttack += Math.Round(PlayerSTR / 5, 0) - Math.Round(PrevStr / 5, 0)
        LevelUpPanel.Visible = False
        RefreshStats()
    End Sub
    Public Sub RefreshStats()
        HealthBar.Caption = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints)) + " HP"
        HealthBar.Value = PlayerCurHitpoints
        HealthBar.Max = PlayerHitpoints
        EnergyBar.Caption = LTrim(Str(PlayerCurEnergy)) + " / " + LTrim(Str(PlayerEnergy)) + " EN"
        EnergyBar.Value = PlayerCurEnergy
        EnergyBar.Max = PlayerEnergy
    End Sub
#End Region
    Public Sub SND(ByVal Text As String, Optional ByVal Clear As Boolean = False) 'this displays the display text
        Dim pxish As Short = TheRoomWidth * PlayerPosX + ColumnsSpace * PlayerPosX 'current player sector x position
        Dim pyish As Short = TheRoomHeight * PlayerPosY + ColumnsSpace * PlayerPosY + 25 'current player sector y position
        If Clear = True Then
            HUDisplay.Text = ""
            HUDisplay.Visible = False
        ElseIf Screensaver = False And LogVisible = True Then
            If HUDisplay.Text <> "" Then HUDisplay.Text = HUDisplay.Text + Chr(13) + Text Else HUDisplay.Text = Text
            HUDisplay.Left = pxish + TheRoomWidth
            'make sure the display is still on screen and fix it if it isn't
            If HUDisplay.Left + HUDisplay.Width > Me.Width Then
                HUDisplay.Left = Me.Width - HUDisplay.Width
            End If
            HUDisplay.Top = pyish - TheRoomHeight
            'make sure hte display is still on screen and fix it if it isn't
            If HUDisplay.Top < 0 Then
                HUDisplay.Top = pyish + TheRoomHeight
            End If
            HUDisplay.Visible = True
        End If
    End Sub
    Private Sub Repaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        Me.CreateGraphics.DrawImage(PAD, 0, 0)
    End Sub
    Private Sub ProcessCommand(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        SND(0, True)
        If PlayerDead = False And Screensaver = False Then
            If e.KeyCode = Keys.Up And PlayerPosY > 0 And PlayerTargeting = False Or e.KeyCode = Keys.NumPad8 And PlayerPosY > 0 And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX, PlayerPosY - 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PositionNPCS()
                    PlayerPosY -= 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX, PlayerPosY - 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX, PlayerPosY - 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Down And PlayerPosY < MapSize And PlayerTargeting = False Or e.KeyCode = Keys.NumPad2 And PlayerPosY < MapSize And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX, PlayerPosY + 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PositionNPCS()
                    PlayerPosY += 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX, PlayerPosY + 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX, PlayerPosY + 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Right And PlayerPosX < MapSize And PlayerTargeting = False Or e.KeyCode = Keys.NumPad6 And PlayerPosX < MapSize And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX + 1, PlayerPosY) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PositionNPCS()
                    PlayerPosX += 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX + 1, PlayerPosY) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY) <> 0 Then
                    PlayerHitLocation(PlayerPosX + 1, PlayerPosY)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Left And PlayerPosX > 0 And PlayerTargeting = False Or e.KeyCode = Keys.NumPad4 And PlayerPosX > 0 And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX - 1, PlayerPosY) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PositionNPCS()
                    PlayerPosX -= 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX - 1, PlayerPosY) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY) <> 0 Then
                    PlayerHitLocation(PlayerPosX - 1, PlayerPosY)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad7 And PlayerPosX > 0 And PlayerPosY > 0 And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX - 1, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY - 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PositionNPCS()
                    PlayerPosX -= 1 : PlayerPosY -= 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX - 1, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY - 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX - 1, PlayerPosY - 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad9 And PlayerPosX < MapSize And PlayerPosY > 0 And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX + 1, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY - 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PositionNPCS()
                    PlayerPosX += 1 : PlayerPosY -= 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX + 1, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY - 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX + 1, PlayerPosY - 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad3 And PlayerPosX < MapSize And PlayerPosY < MapSize And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX + 1, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY + 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PositionNPCS()
                    PlayerPosX += 1 : PlayerPosY += 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX + 1, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY + 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX + 1, PlayerPosY + 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad1 And PlayerPosX > 0 And PlayerPosY < MapSize And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX - 1, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY + 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PositionNPCS()
                    PlayerPosX -= 1 : PlayerPosY += 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX - 1, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY + 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX - 1, PlayerPosY + 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Up And PlayerTargeting = True Then
                If MobileVisible(MapLevel, 0, 1) > 0 Then
                    DrawingProcedures.TargetEnemy(True)
                    DrawingProcedures.LOSMap(MobileVisible(MapLevel, 0, 0), MobileVisible(MapLevel, 0, 1)) = DrawingProcedures.Redraw 'make sure that tile is redrawn next round to remove black box
                    MobileVisible(MapLevel, 0, 1) -= 1
                    DrawingProcedures.TargetEnemy(False) 'just dictated false for visibility reasons
                End If
            ElseIf e.KeyCode = Keys.Down And PlayerTargeting = True Then
                If MobileVisible(MapLevel, 0, 1) < MapSize Then
                    DrawingProcedures.TargetEnemy(True)
                    DrawingProcedures.LOSMap(MobileVisible(MapLevel, 0, 0), MobileVisible(MapLevel, 0, 1)) = DrawingProcedures.Redraw 'make sure that tile is redrawn next round to remove black box
                    MobileVisible(MapLevel, 0, 1) += 1
                    DrawingProcedures.TargetEnemy(False) 'just dictated false for visibility reasons
                End If
            ElseIf e.KeyCode = Keys.Right And PlayerTargeting = True Then
                If MobileVisible(MapLevel, 0, 0) < MapSize Then
                    DrawingProcedures.TargetEnemy(True)
                    DrawingProcedures.LOSMap(MobileVisible(MapLevel, 0, 0), MobileVisible(MapLevel, 0, 1)) = DrawingProcedures.Redraw 'make sure that tile is redrawn next round to remove black box
                    MobileVisible(MapLevel, 0, 0) += 1
                    DrawingProcedures.TargetEnemy(False) 'just dictated false for visibility reasons
                End If
            ElseIf e.KeyCode = Keys.Left And PlayerTargeting = True Then
                If MobileVisible(MapLevel, 0, 0) > 0 Then
                    DrawingProcedures.TargetEnemy(True)
                    DrawingProcedures.LOSMap(MobileVisible(MapLevel, 0, 0), MobileVisible(MapLevel, 0, 1)) = DrawingProcedures.Redraw 'make sure that tile is redrawn next round to remove black box
                    MobileVisible(MapLevel, 0, 0) -= 1
                    DrawingProcedures.TargetEnemy(False) 'just dictated false for visibility reasons
                End If
            ElseIf e.KeyCode = Keys.Space And PlayerTargeting = True Then
                DrawingProcedures.TargetEnemy(True)
                PlayerHitLocation(MobileVisible(MapLevel, 0, 0), MobileVisible(MapLevel, 0, 1))
                DrawingProcedures.LOSMap(MobileVisible(MapLevel, 0, 0), MobileVisible(MapLevel, 0, 1)) = DrawingProcedures.Redraw 'make sure that tile is redrawn next round to remove black box
                PlayerTargeting = False
                ReDraw()
            ElseIf e.KeyCode = Keys.S Then
                If MobilePresent = True Then
                    If PlayerCurEnergy >= 5 Then
                        SND("Shoot in what direction?")
                        SkillType = "Shoot"
                        PlayerCurEnergy -= 5
                        DrawingProcedures.TargetEnemy()
                        SND("Press Spacebar to shoot.")
                        PlayerTargeting = True
                        EnergyBar.Caption = LTrim(Str(PlayerCurEnergy)) + " / " + LTrim(Str(PlayerEnergy)) + " EN"
                        EnergyBar.Value = PlayerCurEnergy
                    Else
                        SND("Not enough Energy.")
                    End If
                Else
                    SND("No enemies are around.")
                End If
            ElseIf e.KeyCode = Keys.Space And PlayerTargeting = False Then
                SND("You are not targeting anything.")
            ElseIf e.KeyCode = Keys.NumPad5 Then
                If PlayerCurHitpoints < PlayerHitpoints Then
                    PlayerCurHitpoints += 1
                    RefreshStats() 'updates Energy and health bar statistics
                End If
                If PlayerCurEnergy < PlayerEnergy Then
                    PlayerCurEnergy += 1
                    RefreshStats() 'updates Energy and health bar statistics
                End If
                ReDraw()
            ElseIf e.KeyCode = Keys.H Or e.KeyCode = Keys.OemQuestion And e.Shift = True Then
                HelpClick(0, EventArgs.Empty)
            ElseIf e.KeyCode = Keys.I Then
                InventoryClicked(0, EventArgs.Empty)
            ElseIf e.KeyCode = Keys.P Or e.KeyCode = Keys.T Then
                If ItemOccupied(MapLevel, PlayerPosX, PlayerPosY) > 0 Then
                    AddToInventory(ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                Else
                    SND("Nothing is here to pickup.")
                End If
            ElseIf e.KeyCode = Keys.OemPeriod And e.Shift = True Then 'go down
                If Map(MapLevel, PlayerPosX, PlayerPosY) = StairsDown Then 'exit
                    MapLevel += 1
                    BuildNewMap(True, "descend")
                End If
            ElseIf e.KeyCode = Keys.F6 And AdminVisible = True Then 'admin mode is enabled, allow force down exit no matter where player is
                If MapLevel < MaxDepthLevel Then
                    MapLevel += 1
                    BuildNewMap(True, "descend")
                Else
                    MapLevel = 1
                    BuildNewMap(True, "descend")
                End If
            ElseIf e.KeyCode = Keys.Q And e.Control = True Then 'quit
                ExitGameClick(0, EventArgs.Empty)
            ElseIf e.KeyCode = Keys.Oemcomma And e.Shift = True Then 'go up
                If Map(MapLevel, PlayerPosX, PlayerPosY) = StairsUp Then 'entrance
                    MapLevel -= 1
                    BuildNewMap(False, "ascend")
                End If
            End If
            If PlayerCurHitpoints <= 0 And InStr(Me.Text, "[Dead]") = False Then
                PlayerDead = True
                SNDScores()
                HighScores = Output.Text
                SaveTextToFile(HighScores, CurDir() + "\HighScores.TG")
                Me.Text += " [Dead] : Press f1 to view scores list"
                SND("Game Over.")
            End If
        End If
        'check rejuvination stats
        If EnergyRejuv > 0 Then
            PlayerCurEnergy += EnergyRejuv
            If PlayerCurEnergy > PlayerEnergy Then PlayerCurEnergy = PlayerEnergy
        End If
        If HealthRejuv > 0 Then
            PlayerCurHitpoints += HealthRejuv
            If PlayerCurHitpoints > PlayerHitpoints Then PlayerCurHitpoints = PlayerHitpoints
        End If
        'if the character stats panel is visble then recheck the stats and draw them after each round
        If CharStats.Visible = True Then
            CharacterStatsRefresh()
        End If
        RefreshStats()
    End Sub
    Sub PositionNPCS()
        PrevPlayerPosX(3) = PrevPlayerPosX(2) : PrevPlayerPosY(3) = PrevPlayerPosY(2)
        PrevPlayerPosX(2) = PrevPlayerPosX(1) : PrevPlayerPosY(2) = PrevPlayerPosY(1)
        PrevPlayerPosX(1) = PrevPlayerPosX(0) : PrevPlayerPosY(1) = PrevPlayerPosY(0)
        PrevPlayerPosX(0) = PlayerPosX : PrevPlayerPosY(0) = PlayerPosY
    End Sub
    Public Sub SNDScores()
        Output.Text = HighScores + Chr(13) + AddSpace(PlayerName, 20) + AddSpace(PlayerRace, 14) + AddSpace(PlayerClass, 17) + AddSpace(LTrim(Str(PlayerLevel)), 8) + AddSpace(LTrim(Str(PlayerExperience)), 13) + AddSpace(LTrim(Str(MapLevel)), 13) + AddSpace(LTrim(Str(PlayerGold)), 10) + LTrim(Str(PlayerTurns))
        HighScores = Output.Text 'incase character restarts game
    End Sub
#Region "Menu Click"
    Private Sub NewGameClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewGameToolStripMenuItem.Click, NewGameToolStripMenuItem1.Click
        Panel1.Visible = True
    End Sub
    Private Sub ExitGameClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitGameToolStripMenuItem.Click, ExitGameToolStripMenuItem1.Click
        Me.Close()
        Me.Dispose()
    End Sub
    Private Sub HelpClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem.Click, HelpToolStripMenuItem1.Click
        If GroupBox1.Visible = False Then
            GroupBox1.Visible = True
            HelpInfo.Visible = True
        ElseIf GroupBox1.Visible = True Then
            GroupBox1.Visible = False
            HelpInfo.Visible = False
        End If
    End Sub
    Private Sub ScoresClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolStripMenuItem.Click, HighScoresToolStripMenuItem.Click
        If ScoresBox.Visible = True Then
            ScoresBox.Visible = False
        ElseIf ScoresBox.Visible = False Then
            Output.Text = HighScores 'refresh scores
            CharStats.Visible = False
            ScoresBox.Visible = True
        End If
    End Sub
    Private Sub ToggleCharacterStatsClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToggleCharacterStatsToolStripMenuItem.Click, StatStrip.Click
        If CharStats.Visible = True Then
            CharStats.Visible = False
        ElseIf CharStats.Visible = False Then
            CharacterStatsRefresh()
        End If
    End Sub
    Private Sub CharacterStatsRefresh()
        StatBox.Text = "[Character Stats]" + Chr(13) + "Depth     : " + LTrim(Str(MapLevel)) + Chr(13) + "Level     : " + LTrim(Str(PlayerLevel)) + Chr(13) _
+ "Experience: " + LTrim(Str(PlayerExperience)) + Chr(13) _
+ "Minerals  : " + LTrim(Str(PlayerGold)) + Chr(13) _
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
    End Sub
    Private Sub ToggleActivityLogClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowHideActivityLogToolStripMenuItem.Click, LogStrip.Click
        If LogVisible = True Then
            LogStrip.Checked = False
            LogVisible = False
        ElseIf LogVisible = False Then
            LogStrip.Checked = True
            LogVisible = True
        End If
    End Sub
#End Region
    ' the following accepts a mouse click and highlights the area, this will be used in the future for mouse moving
    '    Private Sub MouseClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click
    '        Dim xish, xishPLUS, yish, yishPLUS As Integer
    '        Dim TheX As Integer = Cursor.Position.X - Me.Left - 20
    '        Dim TheY As Integer = Cursor.Position.Y - Me.Top - 40
    '        Dim X As Integer = TheX / TheRoomWidth
    '        Dim Y As Integer = TheY / TheRoomHeight
    '        xish = TheRoomWidth * X
    '        yish = TheRoomHeight * Y
    '        Me.CANVAS.DrawRectangle(Pens.IndianRed, xish, yish, TheRoomWidth - 2, TheRoomHeight - 2)
    '        Me.CreateGraphics.DrawImage(PAD, 0, 0)
    '    End Sub

    Private Sub ScreensaverActivate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer.Tick
        If Screensaver = True Then 'move character, it's the screensaver
            If PlayerPosX = ExitPosX And PlayerPosY = ExitPosY Then
                MapLevel += 1
                If MapLevel = MaxDepthLevel Then 'ensure that the screensaver restarts once it reaches the max depth level
                    Array.Clear(MapCreated, 0, MapCreated.Length) 'ensures the map is re-generated now that it's visited a prviously made map
                    MapLevel = 1
                End If
                BuildNewMap(False)
                ScreensaverFound = False
            Else
                MobilePosX(MapLevel, MaxMobiles) = PlayerPosX : MobilePosY(MapLevel, MaxMobiles) = PlayerPosY
                MobilePrevX(MapLevel, MaxMobiles) = PlayerPosX : MobilePrevY(MapLevel, MaxMobiles) = PlayerPosY
                MobileType(MapLevel, MaxMobiles) = 1
                MobileHealth(MapLevel, MaxMobiles) = 100
                PlayerHitpoints = 100 : PlayerCurHitpoints = 100
                PlayerEnergy = 100 : PlayerCurEnergy = 100
                'MobileExists(MapLevel, PlayerPosX, PlayerPosY) = True 'set mobile to living
                PlayerLastPosX = PlayerPosX
                PlayerLastPosY = PlayerPosY
                DetermineMobMov(MaxMobiles, True)
                PlayerPosX = MobilePosX(MapLevel, MaxMobiles)
                PlayerPosY = MobilePosY(MapLevel, MaxMobiles)
                PlayerHidden = 1
            End If
            ReDraw()
        End If
    End Sub
    Public Sub PlayMusic(ByVal Type As String)
        'allow just the ambience during the screensaver
        If MusicOn = True And Screensaver = False Or Type = "Ambience" Then
            'play the ambience and repeat it
            If Type = "Ambience" And MusicBaseName(0) = "" Then
                MusicBaseName(0) = "Ambience"
                Array.Clear(MusicDecrease, 0, MusicDecrease.Length)
                Array.Clear(MusicIncrease, 0, MusicDecrease.Length)
                Music_FileToPlay = (Chr(34) + ("Ambience.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(0), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(0) + " repeat", Nothing, 0, 0)
                'if the player hit file isn't load it, then load it and play it
            ElseIf Type = "PlayerHit" And MusicBaseName(1) = "" Then
                MusicBaseName(1) = "PlayerHit"
                Music_FileToPlay = (Chr(34) + ("PlayerHit.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(1), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(1), Nothing, 0, 0)
                'if the player hit file IS loaded then close it and open it and play it
            ElseIf Type = "PlayerHit" And MusicBaseName(1) <> "" Then
                mciSendString("close " + MusicBaseName(1), Nothing, 0, 0)
                Music_FileToPlay = (Chr(34) + ("PlayerHit.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(1), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(1), Nothing, 0, 0)
            ElseIf Type = "PlayerShoot" And MusicBaseName(2) = "" Then
                MusicBaseName(2) = "PlayerShoot"
                Music_FileToPlay = (Chr(34) + ("PlayerShoot.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(2), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(2), Nothing, 0, 0)
                'if the player hit file IS loaded then close it and open it and play it
            ElseIf Type = "PlayerShoot" And MusicBaseName(2) <> "" Then
                mciSendString("close " + MusicBaseName(2), Nothing, 0, 0)
                Music_FileToPlay = (Chr(34) + ("PlayerShoot.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(2), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(2), Nothing, 0, 0)
            ElseIf Type = "PlayerMiss" And MusicBaseName(3) = "" Then
                MusicBaseName(3) = "PlayerMiss"
                Music_FileToPlay = (Chr(34) + ("PlayerMiss.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(3), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(3), Nothing, 0, 0)
            ElseIf Type = "PlayerMiss" And MusicBaseName(3) <> "" Then
                mciSendString("close " + MusicBaseName(3), Nothing, 0, 0)
                Music_FileToPlay = (Chr(34) + ("PlayerMiss.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(3), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(3), Nothing, 0, 0)
            ElseIf Type = "ReceiveHit" And MusicBaseName(4) = "" Then
                MusicBaseName(4) = "ReceiveHit"
                Music_FileToPlay = (Chr(34) + ("ReceiveHit.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(4), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(4), Nothing, 0, 0)
            ElseIf Type = "ReceiveHit" And MusicBaseName(4) <> "" Then
                mciSendString("close " + MusicBaseName(4), Nothing, 0, 0)
                Music_FileToPlay = (Chr(34) + ("ReceiveHit.mp3") + Chr(34))
                mciSendString("open " & Music_FileToPlay & " alias " + MusicBaseName(4), Nothing, 0, 0)
                mciSendString("play " + MusicBaseName(4), Nothing, 0, 0)
            Else
                'MusicIncrease(2) = 1000
                'MusicIncreaseTar(2) = 300 '30% volume
                'MusicIncreaseRatio(2) = 2
                'MusicIncreaseName(2) = MusicBaseName(2)
                'here is the natural music location where one would assess the environment and play the correct music file
                'associated with that environment.
                'MusicDecrease(2) = 300
                'MusicDecreaseName(2) = "LoginCrowd"
                'MusicIncrease(2) = 1000
                'MusicIncrease(3) = 1000
                'MusicIncreaseTar(3) = 400 '30% volume
                'MusicIncreaseRatio(3) = 2
                'MusicIncreaseName(3) = MusicBaseName(3)
            End If
        End If
    End Sub
#Region "Class Creation Screen"
    Private Sub ScoutClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel2.Click, Label13.Click
        ClassIntroStatsBox.Text = "+Dodge"
        ClassIntroStatsBox.Text += Chr(13) + "+Feint"
        ClassIntroStatsBox.Text += Chr(13) + "+Range"
        ClassIntroStatsBox.Text += Chr(13)
        ClassIntroStatsBox.Text += Chr(13) + "8 Strength"
        ClassIntroStatsBox.Text += Chr(13) + "14 Dexterity"
        ClassIntroStatsBox.Text += Chr(13) + "10 Constitution"
        ClassIntroStatsBox.Text += Chr(13) + "9 Intelligence"
        ClassIntroStatsBox.Text += Chr(13) + "8 Wisdom"
        ClassIntroStatsBox.Text += Chr(13) + "11 Charisma"
        ClassIntroStatsBox.Text += Chr(13) + "10 Luck"
        PlayerSTR = 8
        PlayerDEX = 14
        PlayerCON = 10
        PlayerINT = 9
        PlayerWIS = 8
        PlayerCHA = 11
        PlayerLUC = 10
        Panel2.BackColor = Color.DarkGray
        Panel3.BackColor = Color.Gray
        Panel4.BackColor = Color.Gray
        SquadDescription.Text = "Stealth squad focuses on ranged attacks and" + Chr(13)
        SquadDescription.Text += "dealing damage before enemies can react." + Chr(13)
        SquadDescription.Text += "When forced into melee combat the stealth" + Chr(13)
        SquadDescription.Text += "squad can react quickly, dexterously, and dodge" + Chr(13)
        SquadDescription.Text += "attacks and run to ranged formations to deal" + Chr(13)
        SquadDescription.Text += "damage once again. They are deadly and feared" + Chr(13)
        SquadDescription.Text += "in later levels, their charismatic formation" + Chr(13)
        SquadDescription.Text += "causes enemies to flee."
    End Sub
    Private Sub SoldierClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel3.Click, Label14.Click
        ClassIntroStatsBox.Text = "+Counter"
        ClassIntroStatsBox.Text = "+Fear"
        ClassIntroStatsBox.Text += Chr(13)
        ClassIntroStatsBox.Text += Chr(13) + "14 Strength"
        ClassIntroStatsBox.Text += Chr(13) + "12 Dexterity"
        ClassIntroStatsBox.Text += Chr(13) + "14 Constitution"
        ClassIntroStatsBox.Text += Chr(13) + "8 Intelligence"
        ClassIntroStatsBox.Text += Chr(13) + "8 Wisdom"
        ClassIntroStatsBox.Text += Chr(13) + "16 Charisma"
        ClassIntroStatsBox.Text += Chr(13) + "10 Luck"
        PlayerSTR = 14
        PlayerDEX = 12
        PlayerCON = 14
        PlayerINT = 8
        PlayerWIS = 8
        PlayerCHA = 16
        PlayerLUC = 10
        Panel2.BackColor = Color.Gray
        Panel3.BackColor = Color.DarkGray
        Panel4.BackColor = Color.Gray
        SquadDescription.Text = "Munitions squad is a full-force combat squad" + Chr(13)
        SquadDescription.Text += "directed to be the first contact of" + Chr(13)
        SquadDescription.Text += "extraterrestrial resistance. Munitions squad" + Chr(13)
        SquadDescription.Text += "employs direct damage in both melee and ranged" + Chr(13)
        SquadDescription.Text += "scenarios and are equiped with more vitality" + Chr(13)
        SquadDescription.Text += "than other squads, allowing a higher percentage" + Chr(13)
        SquadDescription.Text += "of survival in stressed situations. Though they" + Chr(13)
        SquadDescription.Text += "are not versatile, they do enstill intense fear" + Chr(13)
        SquadDescription.Text += "in opposition.. rightfully so."
    End Sub
    Private Sub ScientistClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel4.Click, Label15.Click
        ClassIntroStatsBox.Text = "+Energy"
        ClassIntroStatsBox.Text += Chr(13) + "+Resistance"
        ClassIntroStatsBox.Text += Chr(13)
        ClassIntroStatsBox.Text += Chr(13) + "10 Strength"
        ClassIntroStatsBox.Text += Chr(13) + "10 Dexterity"
        ClassIntroStatsBox.Text += Chr(13) + "10 Constitution"
        ClassIntroStatsBox.Text += Chr(13) + "14 Intelligence"
        ClassIntroStatsBox.Text += Chr(13) + "14 Wisdom"
        ClassIntroStatsBox.Text += Chr(13) + "12 Charisma"
        ClassIntroStatsBox.Text += Chr(13) + "18 Luck"
        PlayerSTR = 10
        PlayerDEX = 10
        PlayerCON = 10
        PlayerINT = 14
        PlayerWIS = 14
        PlayerCHA = 12
        PlayerLUC = 18
        Panel2.BackColor = Color.Gray
        Panel3.BackColor = Color.Gray
        Panel4.BackColor = Color.DarkGray
        SquadDescription.Text = "Research squad is aimed to be primarily" + Chr(13)
        SquadDescription.Text += "focused on item retrieval and are incredibly" + Chr(13)
        SquadDescription.Text += "versatile in learning, progressing both in" + Chr(13)
        SquadDescription.Text += "damage or defense faster than any other squad" + Chr(13)
        SquadDescription.Text += "if it is their objective while gaining experience" + Chr(13)
        SquadDescription.Text += "during exploration. Research squad employs the" + Chr(13)
        SquadDescription.Text += "greatest ranged damage due to their precise strikes." + Chr(13)
        SquadDescription.Text += "This efficiency and the naturally higher energy" + Chr(13)
        SquadDescription.Text += "output of the group marks them quite useful."
    End Sub
    Private Sub HoverScout(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel2.MouseEnter, Label13.MouseEnter
        Panel2.BackColor = Color.MediumSeaGreen
    End Sub
    Private Sub HoverSoldier(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel3.MouseEnter, Label14.MouseEnter
        Panel3.BackColor = Color.MediumSeaGreen
    End Sub
    Private Sub HoverScientist(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel4.MouseEnter, Label15.MouseEnter
        Panel4.BackColor = Color.MediumSeaGreen
    End Sub
    Private Sub ReturnButtons(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel4.MouseLeave, Panel2.MouseLeave, Label15.MouseLeave, Label14.MouseLeave, Label13.MouseLeave, Label16.MouseLeave, Panel3.MouseLeave, Panel5.MouseLeave
        If Panel2.BackColor = Color.DarkGray Then 'scout selected, return to right colors
            Panel2.BackColor = Color.DarkGray
            Panel3.BackColor = Color.Gray
            Panel4.BackColor = Color.Gray
        ElseIf Panel3.BackColor = Color.DarkGray Then 'soldier selected, return to right colors
            Panel2.BackColor = Color.Gray
            Panel3.BackColor = Color.DarkGray
            Panel4.BackColor = Color.Gray
        ElseIf Panel4.BackColor = Color.DarkGray Then 'scientist selected, return to right colors
            Panel2.BackColor = Color.Gray
            Panel3.BackColor = Color.Gray
            Panel4.BackColor = Color.DarkGray
        ElseIf Panel2.BackColor = Color.MediumSeaGreen Then 'scout selected, return to right colors
            Panel2.BackColor = Color.DarkGray
            Panel3.BackColor = Color.Gray
            Panel4.BackColor = Color.Gray
        ElseIf Panel3.BackColor = Color.MediumSeaGreen Then 'soldier selected, return to right colors
            Panel2.BackColor = Color.Gray
            Panel3.BackColor = Color.DarkGray
            Panel4.BackColor = Color.Gray
        ElseIf Panel4.BackColor = Color.MediumSeaGreen Then 'scientist selected, return to right colors
            Panel2.BackColor = Color.Gray
            Panel3.BackColor = Color.Gray
            Panel4.BackColor = Color.DarkGray
        End If
        Panel5.BackColor = Color.Gray
    End Sub
    Private Sub AcceptHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label16.MouseEnter, Panel5.MouseEnter
        Panel5.BackColor = Color.MediumSeaGreen
    End Sub
    Private Sub AcceptClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label16.Click, Panel5.Click
        'Hide New Character Panel
        Panel1.Visible = False
        'enable the toggle strip on the file menu
        StatStrip.Enabled = True
        ToggleStrip.Enabled = True
        LogStrip.Enabled = True
        LevelStrip.Enabled = True
        InvStrip.Enabled = True
        'disable screensaver variables
        Screensaver = False
        Timer.Enabled = False
        ScreensaverFound = False
        Screensaver = False
        'setup character variables
        PlayerExperience = 0
        PlayerGold = 0
        PlayerTurns = 0
        PlayerLevel = 0
        PlayerLevelPoints = 0
        PlayerDead = False
        MapLevel = 1
        'set squad stats
        CaptainCurLevel = 1
        ScoutCurLevel = 1
        TankCurLevel = 1
        BenefactorCurLevel = 1
        MuleCurLevel = 1
        'setup window
        Initialize(0, EventArgs.Empty)
        Array.Clear(MapCreated, 0, MapCreated.Length)
        'inventory shizzy
        For tmp0 = 0 To 19 Step 1
            Inv1.Text = "" : Inv2.Text = "" : Inv3.Text = "" : Inv4.Text = "" : Inv5.Text = "" : Inv6.Text = "" : Inv7.Text = "" : Inv8.Text = "" : Inv9.Text = "" : Inv10.Text = ""
            WeaponName.Text = "Nothing Equiped" : ArmorName.Text = "Nothing Equiped"
            WeaponBonus.Text = "No Bonuses" : ArmorBonus.Text = "No Bonuses"
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
    End Sub
#End Region
#Region "Inventory"
    Private Sub InventoryClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvStrip.Click
        If InventoryBox.Visible = False Then
            Inv1.Enabled = False : Inv2.Enabled = False : Inv3.Enabled = False : Inv4.Enabled = False : Inv5.Enabled = False : Inv6.Enabled = False : Inv7.Enabled = False : Inv8.Enabled = False : Inv9.Enabled = False : Inv10.Enabled = False
            Drop1.Enabled = False : Drop2.Enabled = False : Drop3.Enabled = False : Drop4.Enabled = False : Drop5.Enabled = False : Drop6.Enabled = False : Drop7.Enabled = False : Drop8.Enabled = False : Drop9.Enabled = False : Drop10.Enabled = False
            Equip1.Enabled = False : Equip2.Enabled = False : Equip3.Enabled = False : Equip4.Enabled = False : Equip5.Enabled = False : Equip6.Enabled = False : Equip7.Enabled = False : Equip8.Enabled = False : Equip9.Enabled = False : Equip10.Enabled = False
            If Playermaxitems = 2 Then
                Inv1.Enabled = True : Inv2.Enabled = True
                Drop1.Enabled = True : Drop2.Enabled = True
                Equip1.Enabled = True : Equip2.Enabled = True
            ElseIf Playermaxitems = 4 Then
                Inv1.Enabled = True : Inv2.Enabled = True : Inv3.Enabled = True : Inv4.Enabled = True
                Drop1.Enabled = True : Drop2.Enabled = True : Drop3.Enabled = True : Drop4.Enabled = True
                Equip1.Enabled = True : Equip2.Enabled = True : Equip3.Enabled = True : Equip4.Enabled = True
            ElseIf Playermaxitems = 6 Then
                Inv1.Enabled = True : Inv2.Enabled = True : Inv3.Enabled = True : Inv4.Enabled = True : Inv5.Enabled = True : Inv6.Enabled = True
                Drop1.Enabled = True : Drop2.Enabled = True : Drop3.Enabled = True : Drop4.Enabled = True : Drop5.Enabled = True : Drop6.Enabled = True
                Equip1.Enabled = True : Equip2.Enabled = True : Equip3.Enabled = True : Equip4.Enabled = True : Equip5.Enabled = True : Equip6.Enabled = True
            ElseIf Playermaxitems = 8 Then
                Inv1.Enabled = True : Inv2.Enabled = True : Inv3.Enabled = True : Inv4.Enabled = True : Inv5.Enabled = True : Inv6.Enabled = True : Inv7.Enabled = True : Inv8.Enabled = True
                Drop1.Enabled = True : Drop2.Enabled = True : Drop3.Enabled = True : Drop4.Enabled = True : Drop5.Enabled = True : Drop6.Enabled = True : Drop7.Enabled = True : Drop8.Enabled = True
                Equip1.Enabled = True : Equip2.Enabled = True : Equip3.Enabled = True : Equip4.Enabled = True : Equip5.Enabled = True : Equip6.Enabled = True : Equip7.Enabled = True : Equip8.Enabled = True
            ElseIf Playermaxitems = 9 Then
                Inv1.Enabled = True : Inv2.Enabled = True : Inv3.Enabled = True : Inv4.Enabled = True : Inv5.Enabled = True : Inv6.Enabled = True : Inv7.Enabled = True : Inv8.Enabled = True : Inv9.Enabled = True : Inv10.Enabled = True
                Drop1.Enabled = True : Drop2.Enabled = True : Drop3.Enabled = True : Drop4.Enabled = True : Drop5.Enabled = True : Drop6.Enabled = True : Drop7.Enabled = True : Drop8.Enabled = True : Drop9.Enabled = True : Drop10.Enabled = True
                Equip1.Enabled = True : Equip2.Enabled = True : Equip3.Enabled = True : Equip4.Enabled = True : Equip5.Enabled = True : Equip6.Enabled = True : Equip7.Enabled = True : Equip8.Enabled = True : Equip9.Enabled = True : Equip10.Enabled = True
            End If
            InventoryBox.Top = 0
            InventoryBox.Left = 0
            InventoryBox.Visible = True
        Else
            Inv1.Enabled = False : Inv2.Enabled = False : Inv3.Enabled = False : Inv4.Enabled = False : Inv5.Enabled = False : Inv6.Enabled = False : Inv7.Enabled = False : Inv8.Enabled = False : Inv9.Enabled = False : Inv10.Enabled = False
            Drop1.Enabled = False : Drop2.Enabled = False : Drop3.Enabled = False : Drop4.Enabled = False : Drop5.Enabled = False : Drop6.Enabled = False : Drop7.Enabled = False : Drop8.Enabled = False : Drop9.Enabled = False : Drop10.Enabled = False
            Equip1.Enabled = False : Equip2.Enabled = False : Equip3.Enabled = False : Equip4.Enabled = False : Equip5.Enabled = False : Equip6.Enabled = False : Equip7.Enabled = False : Equip8.Enabled = False : Equip9.Enabled = False : Equip10.Enabled = False
            Me.Focus()
            InventoryBox.Visible = False
        End If
    End Sub
    Sub AddToInventory(ByVal VNUM As Short)
        Dim TestRoom As Short = 0
        Dim FoundRoom As Boolean = False
        If ItemType(MapLevel, VNUM) = TheEverspark Then 'player picked up the everspark, thus ending the game
            SND("You win.")
            SND("Game over")
            PlayerDead = True
            FoundRoom = True
        ElseIf ItemType(MapLevel, VNUM) = Gold Then 'set foundroom to true so gold isn't added the inventory instead of the total
            Dim GoldAmount As New Random
            Dim GoldAmt As Integer = GoldAmount.Next(1, 9)
            FoundRoom = True
            PlayerGold += GoldAmt * MapLevel
            ItemNameType(MapLevel, PlayerPosX, PlayerPosY) = LTrim(Str(GoldAmt * MapLevel)) + " precious minerals"
        End If
        If FoundRoom = False Then
            If Inv1.Text = "" And Playermaxitems > 1 Then
                FoundRoom = True
                Inv1.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(0) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(0) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv2.Text = "" And Playermaxitems >= 2 Then
                FoundRoom = True
                Inv2.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(1) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(1) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv3.Text = "" And Playermaxitems >= 3 Then
                FoundRoom = True
                Inv3.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(2) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(2) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv4.Text = "" And Playermaxitems >= 4 Then
                FoundRoom = True
                Inv4.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(3) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(3) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv5.Text = "" And Playermaxitems >= 5 Then
                FoundRoom = True
                Inv5.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(4) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(4) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv6.Text = "" And Playermaxitems >= 6 Then
                FoundRoom = True
                Inv6.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(5) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(5) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv7.Text = "" And Playermaxitems >= 7 Then
                FoundRoom = True
                Inv7.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(6) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(6) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv8.Text = "" And Playermaxitems >= 8 Then
                FoundRoom = True
                Inv8.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(7) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(7) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv9.Text = "" And Playermaxitems >= 9 Then
                FoundRoom = True
                Inv9.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(8) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(8) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            ElseIf Inv10.Text = "" And Playermaxitems >= 10 Then
                FoundRoom = True
                Inv10.Text = ItemNameType(MapLevel, PlayerPosX, PlayerPosY)
                InvType(9) = ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY))
                InvStrength(9) = ItemBonusType(MapLevel, PlayerPosX, PlayerPosY)
            End If
        End If
        If FoundRoom = False Then
            SND("You don't have enough room.")
        Else
            SND("You pick up " + ItemNameType(MapLevel, PlayerPosX, PlayerPosY) + ".")
            ItemType(MapLevel, ItemOccupied(MapLevel, PlayerPosX, PlayerPosY)) = 0
            ItemOccupied(MapLevel, PlayerPosX, PlayerPosY) = 0
            ReDraw()
        End If
    End Sub
    Private Sub Drop1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop1.Click
        Inv1.Text = ""
    End Sub
    Private Sub Drop2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop2.Click
        Inv2.Text = ""
    End Sub
    Private Sub Drop3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop3.Click
        Inv3.Text = ""
    End Sub
    Private Sub Drop4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop4.Click
        Inv4.Text = ""
    End Sub
    Private Sub Drop5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop5.Click
        Inv5.Text = ""
    End Sub
    Private Sub Drop6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop6.Click
        Inv6.Text = ""
    End Sub
    Private Sub Drop7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop7.Click
        Inv7.Text = ""
    End Sub
    Private Sub Drop8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop8.Click
        Inv8.Text = ""
    End Sub
    Private Sub Drop9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop9.Click
        Inv9.Text = ""
    End Sub
    Private Sub Drop10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop10.Click
        Inv10.Text = ""
    End Sub
    Private Sub Equip1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip1.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(0) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv1.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(0)))
            If TempName <> "Nothing Equiped" Then
                Inv1.Text = TempName
                InvStrength(0) = TempStrength
                InvType(0) = Armor
            Else
                Inv1.Text = ""
            End If
        ElseIf InvType(0) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv1.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(0)))
            If TempName <> "Nothing Equiped" Then
                Inv1.Text = TempName
                InvStrength(0) = TempStrength
                InvType(0) = Weapon
            Else
                Inv1.Text = ""
            End If
        ElseIf InvType(0) = Food Then
            PlayerCurHitpoints += 25
            Inv1.Text = ""
            RefreshStats()
        ElseIf InvType(0) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv1.Text = ""
            RefreshStats()
        ElseIf InvType(0) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv1.Text = ""
        End If
    End Sub
    Private Sub Equip2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip2.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(1) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv2.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(1)))
            If TempName <> "Nothing Equiped" Then
                Inv2.Text = TempName
                InvStrength(1) = TempStrength
                InvType(1) = Armor
            Else
                Inv2.Text = ""
            End If
        ElseIf InvType(1) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv2.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(1)))
            If TempName <> "Nothing Equiped" Then
                Inv2.Text = TempName
                InvStrength(1) = TempStrength
                InvType(1) = Weapon
            Else
                Inv2.Text = ""
            End If
        ElseIf InvType(1) = Food Then
            PlayerCurHitpoints += 25
            Inv2.Text = ""
            RefreshStats()
        ElseIf InvType(1) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv2.Text = ""
            RefreshStats()
        ElseIf InvType(1) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv2.Text = ""
        End If
    End Sub
    Private Sub Equip3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip3.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(2) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv3.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(2)))
            If TempName <> "Nothing Equiped" Then
                Inv3.Text = TempName
                InvStrength(2) = TempStrength
                InvType(2) = Armor
            Else
                Inv3.Text = ""
            End If
        ElseIf InvType(2) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv3.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(2)))
            If TempName <> "Nothing Equiped" Then
                Inv3.Text = TempName
                InvStrength(2) = TempStrength
                InvType(2) = Weapon
            Else
                Inv3.Text = ""
            End If
        ElseIf InvType(2) = Food Then
            PlayerCurHitpoints += 25
            Inv3.Text = ""
            RefreshStats()
        ElseIf InvType(2) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv3.Text = ""
            RefreshStats()
        ElseIf InvType(2) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv3.Text = ""
        End If
    End Sub
    Private Sub Equip4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip4.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(3) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv4.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(3)))
            If TempName <> "Nothing Equiped" Then
                Inv4.Text = TempName
                InvStrength(3) = TempStrength
                InvType(3) = Armor
            Else
                Inv4.Text = ""
            End If
        ElseIf InvType(3) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv4.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(3)))
            If TempName <> "Nothing Equiped" Then
                Inv4.Text = TempName
                InvStrength(3) = TempStrength
                InvType(3) = Weapon
            Else
                Inv4.Text = ""
            End If
        ElseIf InvType(3) = Food Then
            PlayerCurHitpoints += 25
            Inv4.Text = ""
            RefreshStats()
        ElseIf InvType(3) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv4.Text = ""
            RefreshStats()
        ElseIf InvType(3) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv4.Text = ""
        End If
    End Sub
    Private Sub Equip5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip5.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(4) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv5.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(4)))
            If TempName <> "Nothing Equiped" Then
                Inv5.Text = TempName
                InvStrength(4) = TempStrength
                InvType(4) = Armor
            Else
                Inv5.Text = ""
            End If
        ElseIf InvType(4) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv5.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(4)))
            If TempName <> "Nothing Equiped" Then
                Inv5.Text = TempName
                InvStrength(4) = TempStrength
                InvType(4) = Weapon
            Else
                Inv5.Text = ""
            End If
        ElseIf InvType(4) = Food Then
            PlayerCurHitpoints += 25
            Inv5.Text = ""
            RefreshStats()
        ElseIf InvType(4) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv5.Text = ""
            RefreshStats()
        ElseIf InvType(4) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv5.Text = ""
        End If
    End Sub
    Private Sub Equip6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip6.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(5) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv6.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(5)))
            If TempName <> "Nothing Equiped" Then 'Nothing Equiped
                Inv6.Text = TempName
                InvStrength(5) = TempStrength
                InvType(5) = Armor
            Else
                Inv6.Text = ""
            End If
        ElseIf InvType(5) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv6.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(5)))
            If TempName <> "Nothing Equiped" Then
                Inv6.Text = TempName
                InvStrength(5) = TempStrength
                InvType(5) = Weapon
            Else
                Inv6.Text = ""
            End If
        ElseIf InvType(5) = Food Then
            PlayerCurHitpoints += 25
            Inv6.Text = ""
            RefreshStats()
        ElseIf InvType(5) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv6.Text = ""
            RefreshStats()
        ElseIf InvType(5) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv6.Text = ""
        End If
    End Sub
    Private Sub Equip7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip7.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(6) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv7.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(6)))
            If TempName <> "Nothing Equiped" Then
                Inv7.Text = TempName
                InvStrength(6) = TempStrength
                InvType(6) = Armor
            Else
                Inv7.Text = ""
            End If
        ElseIf InvType(6) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv7.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(6)))
            If TempName <> "Nothing Equiped" Then
                Inv7.Text = TempName
                InvStrength(6) = TempStrength
                InvType(6) = Weapon
            Else
                Inv7.Text = ""
            End If
        ElseIf InvType(6) = Food Then
            PlayerCurHitpoints += 25
            Inv7.Text = ""
            RefreshStats()
        ElseIf InvType(6) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv7.Text = ""
            RefreshStats()
        ElseIf InvType(6) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv7.Text = ""
        End If
    End Sub
    Private Sub Equip8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip8.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(7) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv8.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(7)))
            If TempName <> "Nothing Equiped" Then
                Inv8.Text = TempName
                InvStrength(7) = TempStrength
                InvType(7) = Armor
            Else
                Inv8.Text = ""
            End If
        ElseIf InvType(7) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv8.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(7)))
            If TempName <> "Nothing Equiped" Then
                Inv8.Text = TempName
                InvStrength(7) = TempStrength
                InvType(7) = Weapon
            Else
                Inv8.Text = ""
            End If
        ElseIf InvType(7) = Food Then
            PlayerCurHitpoints += 25
            Inv8.Text = ""
            RefreshStats()
        ElseIf InvType(7) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv8.Text = ""
            RefreshStats()
        ElseIf InvType(7) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv8.Text = ""
        End If
    End Sub
    Private Sub Equip9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip9.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(8) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv9.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(8)))
            If TempName <> "Nothing Equiped" Then
                Inv9.Text = TempName
                InvStrength(8) = TempStrength
                InvType(8) = Armor
            Else
                Inv9.Text = ""
            End If
        ElseIf InvType(8) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv9.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(8)))
            If TempName <> "Nothing Equiped" Then
                Inv9.Text = TempName
                InvStrength(8) = TempStrength
                InvType(8) = Weapon
            Else
                Inv9.Text = ""
            End If
        ElseIf InvType(8) = Food Then
            PlayerCurHitpoints += 25
            Inv9.Text = ""
            RefreshStats()
        ElseIf InvType(8) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv9.Text = ""
            RefreshStats()
        ElseIf InvType(8) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv9.Text = ""
        End If
    End Sub
    Private Sub Equip10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Equip10.Click
        Dim TempName As String
        Dim TempStrength As String
        If InvType(9) = Armor Then
            TempName = ArmorName.Text
            TempStrength = ArmorBonus.Text
            ArmorName.Text = Inv10.Text
            ArmorBonus.Text = LTrim(Str(InvStrength(9)))
            If TempName <> "Nothing Equiped" Then
                Inv10.Text = TempName
                InvStrength(9) = TempStrength
                InvType(9) = Armor
            Else
                Inv10.Text = ""
            End If
        ElseIf InvType(9) = Weapon Then
            TempName = WeaponName.Text
            TempStrength = WeaponBonus.Text
            WeaponName.Text = Inv10.Text
            WeaponBonus.Text = LTrim(Str(InvStrength(9)))
            If TempName <> "Nothing Equiped" Then
                Inv10.Text = TempName
                InvStrength(9) = TempStrength
                InvType(9) = Weapon
            Else
                Inv10.Text = ""
            End If
        ElseIf InvType(9) = Food Then
            PlayerCurHitpoints += 25
            Inv10.Text = ""
            RefreshStats()
        ElseIf InvType(9) = generateitem.Water Then
            PlayerCurEnergy += 25
            Inv10.Text = ""
            RefreshStats()
        ElseIf InvType(9) = Potion Then
            If Inv5.Text = "Fire Shield Type 1" Then
                LavaImmune = 2
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 2" Then
                LavaImmune = 4
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            ElseIf Inv5.Text = "Fire Shield Type 3" Then
                LavaImmune = 6
                SND("Fire immunity for " + LTrim(Str(LavaImmune)) + " rounds.")
            End If
            Inv10.Text = ""
        End If
    End Sub
#End Region
End Class