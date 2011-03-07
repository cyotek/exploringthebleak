Imports System.IO
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
    Public PlayerExperience As Integer = 0
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
    Public PlayerHitpoints, PlayerAmmunition As Short
    Public PlayerCurHitpoints, PlayerCurAmmunition As Short
    Public PlayerLevelPoints As Short
    Public PlayerTurns As Long
    Public PlayerGold As Short
    Public PlayerDead As Boolean = False
    Public PlayerTargeting As Boolean = False

    Public HighScores As String

    Public PreviousAttack, PreviousDefense As Short

    Public MobilePosX(MaxDepthLevel, MaxMobiles), MobilePosY(MaxDepthLevel, MaxMobiles), MobilePrevX(MaxDepthLevel, MaxMobiles), MobilePrevY(MaxDepthLevel, MaxMobiles), MobileLastMove(MaxDepthLevel, MaxMobiles), MobileType(MaxDepthLevel, MaxMobiles) As Short
    Public MobileHealth(MaxDepthLevel, MaxMobiles), MobileFlee(MaxDepthLevel, MaxMobiles), MobileStun(MaxDepthLevel, MaxMobiles), MobileClumsiness(MaxDepthLevel, MaxMobiles) As Short
    Public MobileVisible(MaxDepthLevel, MaxMobiles, 3) As Short
    Public MobOccupied(MaxDepthLevel, MapSize, MapSize) As Short '0-9 mobile vnum loc
    Public MobileExists(MaxDepthLevel, MapSize, MapSize) As Boolean 'death flag
    Public MobilePresent As Boolean 'generic mobile is presently on screen to prevent erronerous targeting or skills w/o mobs present

    Public ItemNum(MaxDepthLevel, 39), ItemType(MaxDepthLevel, 39), ItemOccupied(MaxDepthLevel, MapSize, MapSize) As Short
    Public ItemNameType(MaxDepthLevel, MapSize, MapSize), ItemShowType(MaxDepthLevel, MapSize, MapSize)
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
    Function MobileFleeFail(ByVal Mobnum As Short)
        Dim MobileNameString As String = ""
        If MobileType(MapLevel, Mobnum) = 1 Then
            MobileNameString = "A rat"
        ElseIf MobileType(MapLevel, Mobnum) = 2 Then
            MobileNameString = "A bat"
        ElseIf MobileType(MapLevel, Mobnum) = 3 Then
            MobileNameString = "An imp"
        ElseIf MobileType(MapLevel, Mobnum) = 4 Then
            MobileNameString = "A goblin"
        ElseIf MobileType(MapLevel, Mobnum) = 5 Then
            MobileNameString = "A troll"
        ElseIf MobileType(MapLevel, Mobnum) = 6 Then
            MobileNameString = "An ogre"
        ElseIf MobileType(MapLevel, Mobnum) = 7 Then
            MobileNameString = "A catoblepas"
        ElseIf MobileType(MapLevel, Mobnum) = 8 Then
            MobileNameString = "A parandrus"
        ElseIf MobileType(MapLevel, Mobnum) = 9 Then
            MobileNameString = "A clurichuan"
        ElseIf MobileType(MapLevel, Mobnum) = 10 Then
            MobileNameString = "A dullahan"
        ElseIf MobileType(MapLevel, Mobnum) = 11 Then
            MobileNameString = "A golem"
        ElseIf MobileType(MapLevel, Mobnum) = 12 Then
            MobileNameString = "A sceadugengan"
        ElseIf MobileType(MapLevel, Mobnum) = 13 Then
            MobileNameString = "A schilla"
        End If
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
        If MobileType(MapLevel, Mobnum) = 1 Then
            MobileNameString = "a rat"
        ElseIf MobileType(MapLevel, Mobnum) = 2 Then
            MobileNameString = "a bat"
        ElseIf MobileType(MapLevel, Mobnum) = 3 Then
            MobileNameString = "an imp"
        ElseIf MobileType(MapLevel, Mobnum) = 4 Then
            MobileNameString = "a goblin"
        ElseIf MobileType(MapLevel, Mobnum) = 5 Then
            MobileNameString = "a troll"
        ElseIf MobileType(MapLevel, Mobnum) = 6 Then
            MobileNameString = "an ogre"
        ElseIf MobileType(MapLevel, Mobnum) = 7 Then
            MobileNameString = "a catoblepas"
        ElseIf MobileType(MapLevel, Mobnum) = 8 Then
            MobileNameString = "a parandrus"
        ElseIf MobileType(MapLevel, Mobnum) = 9 Then
            MobileNameString = "a clurichuan"
        ElseIf MobileType(MapLevel, Mobnum) = 10 Then
            MobileNameString = "a dullahan"
        ElseIf MobileType(MapLevel, Mobnum) = 11 Then
            MobileNameString = "a golem"
        ElseIf MobileType(MapLevel, Mobnum) = 12 Then
            MobileNameString = "a sceadugengan"
        ElseIf MobileType(MapLevel, Mobnum) = 13 Then
            MobileNameString = "a schilla"
        End If
        SND(MobileNameString + " turns to flee.")
        MobileFlee(MapLevel, Mobnum) = Math.Round(PlayerCHA / 10, 0)
        Return 0
    End Function
    Function HitMob(ByVal Mobnum As Short, Optional ByVal Counter As Boolean = False, Optional ByVal HideAttack As Boolean = False)
        Dim MobileNameString As String = ""
        Dim TestCriticalStrike As New Random
        Dim CritStrike As Short = 0
        If MobileType(MapLevel, Mobnum) = 1 Then
            MobileNameString = "a rat"
        ElseIf MobileType(MapLevel, Mobnum) = 2 Then
            MobileNameString = "a bat"
        ElseIf MobileType(MapLevel, Mobnum) = 3 Then
            MobileNameString = "an imp"
        ElseIf MobileType(MapLevel, Mobnum) = 4 Then
            MobileNameString = "a goblin"
        ElseIf MobileType(MapLevel, Mobnum) = 5 Then
            MobileNameString = "a troll"
        ElseIf MobileType(MapLevel, Mobnum) = 6 Then
            MobileNameString = "an ogre"
        ElseIf MobileType(MapLevel, Mobnum) = 7 Then
            MobileNameString = "a catoblepas"
        ElseIf MobileType(MapLevel, Mobnum) = 8 Then
            MobileNameString = "a parandrus"
        ElseIf MobileType(MapLevel, Mobnum) = 9 Then
            MobileNameString = "a clurichuan"
        ElseIf MobileType(MapLevel, Mobnum) = 10 Then
            MobileNameString = "a dullahan"
        ElseIf MobileType(MapLevel, Mobnum) = 11 Then
            MobileNameString = "a golem"
        ElseIf MobileType(MapLevel, Mobnum) = 12 Then
            MobileNameString = "a sceadugengan"
        ElseIf MobileType(MapLevel, Mobnum) = 13 Then
            MobileNameString = "a schilla"
        End If
        If HideAttack = False Then
            If SkillType = "" Then CritStrike = TestCriticalStrike.Next(0, 101) Else CritStrike = TestCriticalStrike.Next(0, 101)
            If CritStrike <= PlayerSTR And SkillType = "" Then 'player critically striked. chance to critically strike is the players strength
                MobileHealth(MapLevel, Mobnum) -= PlayerAttack * 2
                If Counter = False Then
                    SND("You CRIT " + MobileNameString + ".")
                Else
                    SND("You counter CRITS " + MobileNameString + ".")
                End If
            ElseIf CritStrike <= PlayerINT * 2 And SkillType <> "" Then 'player critically striked with a skill.
                If SkillType = "Punch" Or SkillType = "Kick" Or SkillType = "Hit" Or SkillType = "Strike" Or SkillType = "Slice" Or SkillType = "Stab" Or SkillType = "Shoot" Then
                    'these are the basic +1 skilltypes, and all do the same
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack * 2 + 2
                    SND("Your skill CRITS " + MobileNameString + ".")
                ElseIf SkillType = "Wound" Then
                    MobileHealth(MapLevel, Mobnum) -= 40
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
                        MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 1
                    Else
                        MobileHealth(MapLevel, Mobnum) -= PlayerAttack
                    End If
                    If Counter = False Then
                        SND("You hit " + MobileNameString + ".")
                    Else
                        SND("You counter " + MobileNameString + ".")
                    End If
                End If
            ElseIf SkillType <> "" Then 'basic skill
                If SkillType = "Punch" Then 'just a +1 attack
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 1
                    SND("You punch " + MobileNameString + ".")
                ElseIf SkillType = "Kick" Then 'just a +1 attack
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 1
                    SND("You kick " + MobileNameString + ".")
                ElseIf SkillType = "Hit" Then 'just a +1 attack
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 1
                    SND("You hit " + MobileNameString + ".")
                ElseIf SkillType = "Strike" Then 'just a +1 attack
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 1
                    SND("You strike " + MobileNameString + ".")
                ElseIf SkillType = "Slice" Then 'just a +1 attack
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 1
                    SND("You slice " + MobileNameString + ".")
                ElseIf SkillType = "Stab" Then 'just a +1 attack
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 1
                    SND("You stab " + MobileNameString + ".")
                ElseIf SkillType = "Shoot" Then 'just a +1 attack
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 1
                    SND("You shoot " + MobileNameString + ".")
                ElseIf SkillType = "Wound" Then '20 attack
                    MobileHealth(MapLevel, Mobnum) -= 20
                    SND("You decimate " + MobileNameString + ".")
                ElseIf SkillType = "Stun" Then 'stun enemy preventing movement and attacks for 3 rounds
                    MobileStun(MapLevel, Mobnum) = 3
                    SND("You stun " + MobileNameString + ".")
                ElseIf SkillType = "Double Slice" Then 'double slice the enemy
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack * 2
                    SND("You slice " + MobileNameString + ".")
                    SND("You slice " + MobileNameString + ".")
                ElseIf SkillType = "Trip" Then 'stun enemy for 2 rounds
                    MobileStun(MapLevel, Mobnum) = 2
                    SND("You trip " + MobileNameString + ".")
                ElseIf SkillType = "Runestrike" Then 'runstrike enemy
                    MobileStun(MapLevel, Mobnum) = 2
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack
                    SND("You runstrike " + MobileNameString + ".")
                ElseIf SkillType = "Fireball" Then 'fireball enemy
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 10
                    SND("A fireball mutilates " + MobileNameString + ".")
                ElseIf SkillType = "Clumsiness" Then 'clumsiness enemy
                    SND("You clumsiness " + MobileNameString + ".")
                    MobileClumsiness(MapLevel, Mobnum) = 5
                ElseIf SkillType = "Holy Bolt" Then 'holy bolt enemy
                    SND("Holy Bolt erradicates " + MobileNameString + ".")
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 10
                ElseIf SkillType = "Fire Arrow" Then 'fire arrow enemy
                    SND("Fire Arrow sears " + MobileNameString + ".")
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 3
                ElseIf SkillType = "Sacrifice" Then 'sacrifice hp for extra damage attack
                    SND("You demolish " + MobileNameString + ".")
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack + 10
                ElseIf SkillType = "Triple Slice" Then 'double slice the enemy
                    MobileHealth(MapLevel, Mobnum) -= PlayerAttack * 3
                    SND("You slice " + MobileNameString + ".")
                    SND("You slice " + MobileNameString + ".")
                    SND("You slice " + MobileNameString + ".")
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
        If MobileType(MapLevel, Mobnum) = 1 Then
            DamageAmount = 3
            MobileNameString = "A rat"
        ElseIf MobileType(MapLevel, Mobnum) = 2 Then
            DamageAmount = 6
            MobileNameString = "A bat"
        ElseIf MobileType(MapLevel, Mobnum) = 3 Then
            DamageAmount = 9
            MobileNameString = "An imp"
        ElseIf MobileType(MapLevel, Mobnum) = 4 Then
            DamageAmount = 12
            MobileNameString = "A goblin"
        ElseIf MobileType(MapLevel, Mobnum) = 5 Then
            DamageAmount = 15
            MobileNameString = "A troll"
        ElseIf MobileType(MapLevel, Mobnum) = 6 Then
            DamageAmount = 18
            MobileNameString = "An ogre"
        ElseIf MobileType(MapLevel, Mobnum) = 7 Then
            DamageAmount = 21
            MobileNameString = "A catoblepas"
        ElseIf MobileType(MapLevel, Mobnum) = 8 Then
            DamageAmount = 24
            MobileNameString = "A parandrus"
        ElseIf MobileType(MapLevel, Mobnum) = 9 Then
            DamageAmount = 27
            MobileNameString = "A clurichuan"
        ElseIf MobileType(MapLevel, Mobnum) = 10 Then
            DamageAmount = 30
            MobileNameString = "A dullahan"
        ElseIf MobileType(MapLevel, Mobnum) = 11 Then
            DamageAmount = 33
            MobileNameString = "A golem"
        ElseIf MobileType(MapLevel, Mobnum) = 12 Then
            DamageAmount = 36
            MobileNameString = "A sceadugengan"
        ElseIf MobileType(MapLevel, Mobnum) = 13 Then
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
        If TestDodge <= 10 And MobileClumsiness(MapLevel, Mobnum) <= 0 Then
            SupressDueToCriticalStrike = True
            SND(MobileNameString + "'s attack misses.")
        ElseIf MobileClumsiness(MapLevel, Mobnum) > 0 And TestDodge <= 50 Then
            SupressDueToCriticalStrike = True
            SND(MobileNameString + "'s attack misses.")
        End If
        If MobileClumsiness(MapLevel, Mobnum) > 0 Then MobileClumsiness(MapLevel, Mobnum) -= 1
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
        PossibleWidth = TheRoomWidth * MapSize + MapSize + 20
        PossibleHeight = TheRoomWidth * MapSize + MapSize + 60 + 25 'thelast 25 height is for the stat bars, remove that when they're unnecessary
        If OldHeight <> PossibleHeight Or OldWidth <> PossibleWidth Then
            Me.Width = PossibleWidth
            Me.Height = PossibleHeight
            Me.CenterToScreen() 'center to the screen
            displayfont = New Font("Arial", -4 + (TheRoomHeight + TheRoomWidth / 2) / 2)
        End If
    End Sub
    Private Sub InitScreensaver()
        If Screensaver = True Then
            HealthBar.Visible = False
            AmmunitionBar.Visible = False
        Else
            HealthBar.Visible = True
            AmmunitionBar.Visible = True
        End If
    End Sub
    Private Sub InitStatBar()
        HealthBar.Top = Me.Height - HealthBar.Height - 32
        HealthBar.Left = 0 'arranges the healthbar
        HealthBar.Width = Me.Width / 2
        HealthBar.Height = 25
        AmmunitionBar.Top = Me.Height - AmmunitionBar.Height - 32
        AmmunitionBar.Left = Me.Width / 2 'arrange the Ammunitionbar according to the panel
        AmmunitionBar.Width = Me.Width / 2
        AmmunitionBar.Height = 25
    End Sub
    Private Sub InitLog()
        Array.Clear(SNDLog, 0, SNDLog.Length)
        'SND("Press '?' or 'h' for help.")
        'SND("You descend to depth 1.")
    End Sub
    Private Sub InitCharacter()
        If Screensaver = False Then
            PlayerSTR = 10 : PlayerDEX = 10 : PlayerCON = 10 : PlayerINT = 10 : PlayerWIS = 10 : PlayerCHA = 10 : PlayerLUC = 10
            PlayerDefense = Math.Round(PlayerCON / 5, 0)
            PlayerAttack = Math.Round(PlayerSTR / 5, 0)
            PreviousDefense = PlayerDefense
            PreviousAttack = PlayerAttack
            RefreshStats() 'updates Ammunition and health bar statistics
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
    Public Sub Initialize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'resize windows
        InitWindowSize()
        InitStatBar()
        InitScreensaver()
        InitLog()
        InitCharacter()
        InitHighScores()
        InitMaps()
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
        Array.Clear(MapShown, 0, MapShown.Length) 'clear all shown sectors
        'check to see if the new map was one visited already
        If MapCreated(MapLevel) = False Then 'entering a new map, need to generate
            GenerateMap(8)
            GenerateBlur() 'walls blur
            DetermineEnvironment()
            If GenerateType <> Swamps And GenerateType <> Passage And GenerateType <> Catacombs Then 'don't generate a river in a swamp or catacombs
                If GenerateRiverChance < 71 Then '70% chance to draw a river
                    GenerateRiver()
                    GenerateBlur(True) 'water blur
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
                GenerateBlur(True)
                RiverType = Water
            End If
            GenerateFog()
            PopulateItems()
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
    Sub GenerateBlur(Optional ByVal Type As Boolean = False)
        If Type = False Then 'regular test for walls
            For x = 0 To MapSize Step 1
                For y = 0 To MapSize Step 1
                    If Map(MapLevel, x, y) = Wall Then
                        If x > 0 Then
                            If Map(MapLevel, x - 1, y) <> Wall Then MapBlur(MapLevel, x, y, 3) = True Else MapBlur(MapLevel, x, y, 3) = False
                        End If
                        If x < MapSize Then
                            If Map(MapLevel, x + 1, y) <> Wall Then MapBlur(MapLevel, x, y, 1) = True Else MapBlur(MapLevel, x, y, 1) = False
                        End If
                        If y > 0 Then
                            If Map(MapLevel, x, y - 1) <> Wall Then MapBlur(MapLevel, x, y, 2) = True Else MapBlur(MapLevel, x, y, 2) = False
                        End If
                        If y < MapSize Then
                            If Map(MapLevel, x, y + 1) <> Wall Then MapBlur(MapLevel, x, y, 0) = True Else MapBlur(MapLevel, x, y, 0) = False
                        End If
                    End If
                Next
            Next
        Else 'test for water
            For x = 0 To MapSize Step 1
                For y = 0 To MapSize Step 1
                    If Map(MapLevel, x, y) = Water Then
                        If x > 0 Then
                            If Map(MapLevel, x - 1, y) <> Water Then WaterBlur(MapLevel, x, y, 3) = True Else WaterBlur(MapLevel, x, y, 3) = False
                        End If
                        If x < MapSize Then
                            If Map(MapLevel, x + 1, y) <> Water Then WaterBlur(MapLevel, x, y, 1) = True Else WaterBlur(MapLevel, x, y, 1) = False
                        End If
                        If y > 0 Then
                            If Map(MapLevel, x, y - 1) <> Water Then WaterBlur(MapLevel, x, y, 2) = True Else WaterBlur(MapLevel, x, y, 2) = False
                        End If
                        If y < MapSize Then
                            If Map(MapLevel, x, y + 1) <> Water Then WaterBlur(MapLevel, x, y, 0) = True Else WaterBlur(MapLevel, x, y, 0) = False
                        End If
                    End If
                Next
            Next
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
        Dim BuilderGrowthAmount As Short 'this is the actual amount that will be grown, not the potential
        Dim BuilderPositionX As Short = RandomNumber.Next(2, MapSize)
        Dim BuilderPositionY As Short = RandomNumber.Next(2, MapSize)
        Dim Turns As Short = 7
        Dim Stops = 0
        Dim StopWhile = 0
        Dim PotentialGrowth As Short = 0 'this number is the potential size the room or corridor can be in that direction
        Dim PotentialSides As Short = 0
        'generate a random map type
        GenerateType = RandomNumber.Next(0, 7) 'returns 0:Dungeon,1:Ruins,2:Tunnels(nesw-dir),3:TunnelsExpanded(nesw+ne,se,sw,nw-dir),4:Catacombs(mazes),5:Swamps,6:Passages
        If MapLevel = MaxDepthLevel Then
            GenerateType = Catacombs
        ElseIf GenerateType = Catacombs Then
            GenerateType = Ruins 'favors ruins when catacombs is picked and it's not the last level.
        ElseIf GenerateType = Swamps Then
            GenerateType = Dungeon 'favors dungeon when swamps is picked. swamps is only allowed on one dungeon, 22 (entrance to grassy area)
        ElseIf MapLevel = 22 And GenerateType <> Swamps Then
            GenerateType = Swamps
        ElseIf Screensaver = True And GenerateType = Tunnels2 Then 'don't allow diagonal tunnels during screensaver because pathing doesn't support it
            GenerateType = Tunnels
        End If
        If GenerateType = Dungeon Then
            For RepeatToRecursion = 1 To Recursion Step 1
                Map(MapLevel, BuilderPositionX, BuilderPositionY) = 1
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
                        If Map(MapLevel, BuilderPositionX, BuilderPositionY - 1) = 1 Then 'already carved, no need to proceed
                            PotentialGrowth = -1 : Stops += 1
                        End If
                    ElseIf BuilderDirection = East And BuilderPositionX < MapSize - 2 Then 'Veryifying there is room for growth in that direction, at least 2 spaces
                        PotentialGrowth = MapSize - BuilderPositionX - 1
                        If Map(MapLevel, BuilderPositionX + 1, BuilderPositionY) = 1 Then 'already carved, no need to proceed
                            PotentialGrowth = -1 : Stops += 1
                        End If
                    ElseIf BuilderDirection = South And BuilderPositionY < MapSize - 2 Then 'Verifying there is room for growth in that direction, at least 2 spaces
                        PotentialGrowth = MapSize - BuilderPositionY - 1
                        If Map(MapLevel, BuilderPositionX, BuilderPositionY + 1) = 1 Then 'already carved, no need to proceed
                            PotentialGrowth = -1 : Stops += 1
                        End If
                    ElseIf BuilderDirection = West And BuilderPositionX > 2 Then 'Verifying there is room for growth in that direction, at least 2 spaces
                        PotentialGrowth = BuilderPositionX - 1
                        If Map(MapLevel, BuilderPositionX - 1, BuilderPositionY) = 1 Then 'already carved, no need to proceed
                            PotentialGrowth = -1 : Stops += 1
                        End If
                    End If
                    If PotentialGrowth > 4 Then
                        BuilderLastDirection = BuilderDirection
                        BuilderGrowthAmount = RandomNumber.Next(1, PotentialGrowth) 'growth includes wall, can't draw to end of map so just use potential growth since it's exclusive instead of inclusive
                        Map(MapLevel, BuilderPositionX + 1, BuilderPositionY) = Floor
                        Map(MapLevel, BuilderPositionX - 1, BuilderPositionY) = Floor
                        Map(MapLevel, BuilderPositionX, BuilderPositionY + 1) = Floor
                        Map(MapLevel, BuilderPositionX, BuilderPositionY - 1) = Floor
                        PotentialSides = RandomNumber.Next(1, 3)
                        If PotentialSides = 1 Then
                            Map(MapLevel, BuilderPositionX + 1, BuilderPositionY + 1) = Floor
                        End If
                        PotentialSides = RandomNumber.Next(1, 3)
                        If PotentialSides = 1 Then
                            Map(MapLevel, BuilderPositionX - 1, BuilderPositionY - 1) = Floor
                        End If
                        PotentialSides = RandomNumber.Next(1, 3)
                        If PotentialSides = 1 Then
                            Map(MapLevel, BuilderPositionX - 1, BuilderPositionY + 1) = Floor
                        End If
                        PotentialSides = RandomNumber.Next(1, 3)
                        If PotentialSides = 1 Then
                            Map(MapLevel, BuilderPositionX + 1, BuilderPositionY - 1) = Floor
                        End If
                        If BuilderDirection = North Then
                            For BuilderPositionY = BuilderPositionY To BuilderPositionY - BuilderGrowthAmount Step -1
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                PotentialSides = RandomNumber.Next(1, 11)
                                If PotentialSides = 1 Then 'random chance to draw a floor to the right 10%
                                    Map(MapLevel, BuilderPositionX + 1, BuilderPositionY) = Floor
                                ElseIf PotentialSides = 2 Then 'randomchance to draw a floor to the left 10%
                                    Map(MapLevel, BuilderPositionX - 1, BuilderPositionY) = Floor
                                End If
                            Next
                        ElseIf BuilderDirection = East Then
                            For BuilderPositionX = BuilderPositionX To BuilderPositionX + BuilderGrowthAmount Step 1
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                PotentialSides = RandomNumber.Next(1, 11)
                                If PotentialSides = 1 Then 'random chance to draw a floor to the north 10%
                                    Map(MapLevel, BuilderPositionX, BuilderPositionY - 1) = Floor
                                ElseIf PotentialSides = 2 Then 'randomchance to draw a floor to the south 10%
                                    Map(MapLevel, BuilderPositionX, BuilderPositionY + 1) = Floor
                                End If
                            Next
                        ElseIf BuilderDirection = South Then
                            For BuilderPositionY = BuilderPositionY To BuilderPositionY + BuilderGrowthAmount Step 1
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                PotentialSides = RandomNumber.Next(1, 11)
                                If PotentialSides = 1 Then 'random chance to draw a floor to the right 10%
                                    Map(MapLevel, BuilderPositionX + 1, BuilderPositionY) = Floor
                                ElseIf PotentialSides = 2 Then 'randomchance to draw a floor to the left 10%
                                    Map(MapLevel, BuilderPositionX - 1, BuilderPositionY) = Floor
                                End If
                            Next
                        ElseIf BuilderDirection = West Then
                            For BuilderPositionX = BuilderPositionX To BuilderPositionX - BuilderGrowthAmount Step -1
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                PotentialSides = RandomNumber.Next(1, 11)
                                If PotentialSides = 1 Then 'random chance to draw a floor to the north 10%
                                    Map(MapLevel, BuilderPositionX, BuilderPositionY - 1) = Floor
                                ElseIf PotentialSides = 2 Then 'randomchance to draw a floor to the south 10%
                                    Map(MapLevel, BuilderPositionX, BuilderPositionY + 1) = Floor
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
        ElseIf GenerateType = Ruins Then
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
            Dim MapGenLevel(MapSize, MapSize) As Short
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
                            MapGenLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                            ItemOccupied(MapLevel, BuilderTestPosX, BuilderTestPosY) = 1
                            ItemShowType(MapLevel, BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                        End If
                    ElseIf CurrentOccupied = 20 Then
                        Map(MapLevel, BuilderPositionX, BuilderPositionY) = Wall 'positions filled, no more searching, fill rest with walls
                    Else 'already searching a location
                        TrodHead = True
                        While TrodHead = True
                            TrodTry = False : TrodLevel = 0
                            While TrodTry = False
                                'test up
                                If BuilderTestPosY > 0 Then 'ensure it's within bounds
                                    If Map(MapLevel, BuilderTestPosX, BuilderTestPosY - 1) = Floor Or Map(MapLevel, BuilderTestPosX, BuilderTestPosY - 1) = StairsUp Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX, BuilderTestPosY - 1) = TrodLevel Then 'found next trod
                                            BuilderTestPosY -= 1
                                            If MapTrod(BuilderTestPosX, BuilderTestPosY) <= 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapGenLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            'ItemOccupied(BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test right
                                If BuilderTestPosX < MapSize Then 'ensure it's within bounds
                                    If Map(MapLevel, BuilderTestPosX + 1, BuilderTestPosY) = Floor Or Map(MapLevel, BuilderTestPosX + 1, BuilderTestPosY) = StairsUp Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX + 1, BuilderTestPosY) = TrodLevel Then 'found next trod
                                            BuilderTestPosX += 1
                                            If MapTrod(BuilderTestPosX, BuilderTestPosY) <= 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapGenLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            Map(MapLevel, BuilderTestPosX, BuilderTestPosY) = StairsUp
                                            'ItemOccupied(BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test down
                                If BuilderTestPosY < MapSize Then 'ensure it's within bounds
                                    If Map(MapLevel, BuilderTestPosX, BuilderTestPosY + 1) = Floor Or Map(MapLevel, BuilderTestPosX, BuilderTestPosY + 1) = StairsUp Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX, BuilderTestPosY + 1) = TrodLevel Then 'found next trod
                                            BuilderTestPosY += 1
                                            If MapTrod(BuilderTestPosX, BuilderTestPosY) <= 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapGenLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            Map(MapLevel, BuilderTestPosX, BuilderTestPosY) = StairsUp
                                            'ItemOccupied(BuilderTestPosX, BuilderTestPosY) = 1 'show the current state number, debug
                                            'ItemShowType(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied 'show the current state number, debug
                                            Exit While
                                        End If
                                    End If
                                End If
                                'test left
                                If BuilderTestPosX > 0 Then 'ensure it's within bounds
                                    If Map(MapLevel, BuilderTestPosX - 1, BuilderTestPosY) = Floor Or Map(MapLevel, BuilderTestPosX - 1, BuilderTestPosY) = StairsUp Then 'make sure it's a floor
                                        If MapTrod(BuilderTestPosX - 1, BuilderTestPosY) = TrodLevel Then 'found next trod
                                            BuilderTestPosX -= 1
                                            If MapTrod(BuilderTestPosX, BuilderTestPosY) <= 1 Then
                                                OccupiedSquares(CurrentOccupied) += 1 'increase the total occupied squares of current occupied set to compare w/ others
                                            End If
                                            MapTrod(BuilderTestPosX, BuilderTestPosY) += 1
                                            MapGenLevel(BuilderTestPosX, BuilderTestPosY) = CurrentOccupied
                                            Map(MapLevel, BuilderTestPosX, BuilderTestPosY) = StairsUp
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
                    If MapGenLevel(MapStepX, MapStepY) = CurrentLargest Then
                        Map(MapLevel, MapStepX, MapStepY) = Floor
                        Wallnumber += 1
                    Else
                        Map(MapLevel, MapStepX, MapStepY) = Wall
                    End If
                Next
            Next
        ElseIf GenerateType = Passage Then
            Dim CurX As Short = RandomNumber.Next(1, MapSize - 1)
            Dim CurY As Short = RandomNumber.Next(1, MapSize - 1)
            Dim CellsMade As Short = 1
            Dim CurrentCell As Short = 1
            Dim CellOrderX(1) As Short
            Dim CellOrderY(1) As Short
            Dim CellMade(MapSize, MapSize) As Boolean
            Dim FoundDirection As Boolean = False
            Dim TryDirection As Short
            Dim TestDirection As Boolean = False
            Dim TMapSize As Short = MapSize
            'set start location as entrance or up stairs if maplevel is greater than 1
            PlayerPosX = CurX
            PlayerPosY = CurY
            If MapLevel >= 2 Then
                Map(MapLevel, CurX, CurY) = StairsUp
            End If
            While CellsMade < TMapSize * TMapSize
                If Map(MapLevel, CurX, CurY) <> StairsUp Then Map(MapLevel, CurX, CurY) = Floor 'make sure you don't draw over the stairs up
                CellMade(CurX, CurY) = True
                TryDirection = RandomNumber.Next(1, 5)
                TestDirection = False
                If CurX + 2 <= MapSize Then 'check boundaries
                    If Map(MapLevel, CurX + 2, CurY) = Wall Then TestDirection = True 'allowed to build to the right? if so then we can proceed
                End If
                If CurY + 2 <= MapSize Then 'check boundaries
                    If Map(MapLevel, CurX, CurY + 2) = Wall Then TestDirection = True 'allowed to build to the south? if so then we can proceed
                End If
                If CurX - 2 >= 0 Then 'check boundaries
                    If Map(MapLevel, CurX - 2, CurY) = Wall Then TestDirection = True 'allowed to build to the west? if so then we can proceed
                End If
                If CurY - 2 >= 0 Then 'check boundaries
                    If Map(MapLevel, CurX, CurY - 2) = Wall Then TestDirection = True 'allowed to build to the north? if so then we can proceed
                End If
                If TestDirection = False Then 'there are no directions to build from this point, dead end
                    If MapLevel < 28 Then
                        ExitPosX = CurX
                        ExitPosY = CurY
                        Map(MapLevel, CurX, CurY) = StairsDown 'don't draw stairs down on last level
                    End If
                    Exit While
                End If
                If CellOrderX.Length < CurrentCell + 1 Then Array.Resize(CellOrderX, CurrentCell + 1) 'ensure the array is long enough
                If CellOrderY.Length < CurrentCell + 1 Then Array.Resize(CellOrderY, CurrentCell + 1) 'ensure the array is long enough
                If TryDirection = East Then
                    'test right
                    If CurX + 2 <= MapSize Then 'ensures the target is within boundaries
                        If Map(MapLevel, CurX + 2, CurY) = Wall Then
                            CellOrderX(CurrentCell) = CurX
                            CellOrderY(CurrentCell) = CurY
                            CurrentCell += 1
                            Map(MapLevel, CurX + 1, CurY) = Floor 'build to the right
                            CurX += 2
                            CellsMade += 2
                        End If
                    End If
                ElseIf TryDirection = South Then
                    'test down
                    If CurY + 2 <= MapSize Then 'ensures the target is within boundaries
                        If Map(MapLevel, CurX, CurY + 2) = Wall Then
                            CellOrderX(CurrentCell) = CurX
                            CellOrderY(CurrentCell) = CurY
                            CurrentCell += 1
                            Map(MapLevel, CurX, CurY + 1) = Floor 'build to the south
                            CurY += 2
                            CellsMade += 2
                        End If
                    End If
                ElseIf TryDirection = West Then
                    'test left
                    If CurX - 2 >= 0 Then 'ensures the target is within boundaries
                        If Map(MapLevel, CurX - 2, CurY) = Wall Then
                            CellOrderX(CurrentCell) = CurX
                            CellOrderY(CurrentCell) = CurY
                            CurrentCell += 1
                            Map(MapLevel, CurX - 1, CurY) = Floor 'build to the left
                            CurX -= 2
                            CellsMade += 2
                        End If
                    End If
                ElseIf TryDirection = North Then
                    'test up
                    If CurY - 2 >= 0 Then 'ensures the target is within boundaries
                        If Map(MapLevel, CurX, CurY - 2) = Wall Then
                            CellOrderX(CurrentCell) = CurX
                            CellOrderY(CurrentCell) = CurY
                            CurrentCell += 1
                            Map(MapLevel, CurX, CurY - 1) = Floor 'build to the north
                            CurY -= 2
                            CellsMade += 2
                        End If
                    End If
                End If
            End While
        ElseIf GenerateType = Catacombs Then
            Dim CurX As Short = RandomNumber.Next(1, MapSize - 1)
            Dim CurY As Short = RandomNumber.Next(1, MapSize - 1)
            Dim CellsMade As Short = 1
            Dim CurrentCell As Short = 1
            Dim CellOrderX(1) As Short
            Dim CellOrderY(1) As Short
            Dim CellMade(MapSize, MapSize) As Boolean
            Dim FoundDirection As Boolean = False
            Dim TryDirection As Short
            Dim TestDirection As Boolean = False
            Dim TMapSize As Short = MapSize
            'set start location as entrance or up stairs if maplevel is greater than 1
            PlayerPosX = CurX
            PlayerPosY = CurY
            If MapLevel >= 2 Then
                Map(MapLevel, CurX, CurY) = StairsUp
            End If
            While CellsMade < TMapSize * TMapSize
                If Map(MapLevel, CurX, CurY) <> StairsUp Then Map(MapLevel, CurX, CurY) = Floor 'make sure you don't draw over the stairs up
                CellMade(CurX, CurY) = True
                TryDirection = RandomNumber.Next(1, 5)
                TestDirection = False
                If CurX + 2 <= MapSize Then 'check boundaries
                    If Map(MapLevel, CurX + 2, CurY) = Wall Then TestDirection = True 'allowed to build to the right? if so then we can proceed
                End If
                If CurY + 2 <= MapSize Then 'check boundaries
                    If Map(MapLevel, CurX, CurY + 2) = Wall Then TestDirection = True 'allowed to build to the south? if so then we can proceed
                End If
                If CurX - 2 >= 0 Then 'check boundaries
                    If Map(MapLevel, CurX - 2, CurY) = Wall Then TestDirection = True 'allowed to build to the west? if so then we can proceed
                End If
                If CurY - 2 >= 0 Then 'check boundaries
                    If Map(MapLevel, CurX, CurY - 2) = Wall Then TestDirection = True 'allowed to build to the north? if so then we can proceed
                End If
                If TestDirection = False Then 'there are no directions to build from this point, dead end
                    CurrentCell -= 1
                    CurX = CellOrderX(CurrentCell)
                    CurY = CellOrderY(CurrentCell)
                    If CurrentCell = 1 Then
                        If MapLevel < 28 Then
                            ExitPosX = CellOrderX(CellOrderX.Length - 1)
                            ExitPosY = CellOrderY(CellOrderY.Length - 1)
                            Map(MapLevel, CellOrderX(CellOrderX.Length - 1), CellOrderY(CellOrderY.Length - 1)) = StairsDown 'don't draw stairs down on last level
                        End If
                        Exit While
                    End If
                End If
                If CellOrderX.Length < CurrentCell + 1 Then Array.Resize(CellOrderX, CurrentCell + 1) 'ensure the array is long enough
                If CellOrderY.Length < CurrentCell + 1 Then Array.Resize(CellOrderY, CurrentCell + 1) 'ensure the array is long enough
                If TryDirection = East Then
                    'test right
                    If CurX + 2 <= MapSize Then 'ensures the target is within boundaries
                        If Map(MapLevel, CurX + 2, CurY) = Wall Then
                            CellOrderX(CurrentCell) = CurX
                            CellOrderY(CurrentCell) = CurY
                            CurrentCell += 1
                            Map(MapLevel, CurX + 1, CurY) = Floor 'build to the right
                            CurX += 2
                            CellsMade += 2
                        End If
                    End If
                ElseIf TryDirection = South Then
                    'test down
                    If CurY + 2 <= MapSize Then 'ensures the target is within boundaries
                        If Map(MapLevel, CurX, CurY + 2) = Wall Then
                            CellOrderX(CurrentCell) = CurX
                            CellOrderY(CurrentCell) = CurY
                            CurrentCell += 1
                            Map(MapLevel, CurX, CurY + 1) = Floor 'build to the south
                            CurY += 2
                            CellsMade += 2
                        End If
                    End If
                ElseIf TryDirection = West Then
                    'test left
                    If CurX - 2 >= 0 Then 'ensures the target is within boundaries
                        If Map(MapLevel, CurX - 2, CurY) = Wall Then
                            CellOrderX(CurrentCell) = CurX
                            CellOrderY(CurrentCell) = CurY
                            CurrentCell += 1
                            Map(MapLevel, CurX - 1, CurY) = Floor 'build to the left
                            CurX -= 2
                            CellsMade += 2
                        End If
                    End If
                ElseIf TryDirection = North Then
                    'test up
                    If CurY - 2 >= 0 Then 'ensures the target is within boundaries
                        If Map(MapLevel, CurX, CurY - 2) = Wall Then
                            CellOrderX(CurrentCell) = CurX
                            CellOrderY(CurrentCell) = CurY
                            CurrentCell += 1
                            Map(MapLevel, CurX, CurY - 1) = Floor 'build to the north
                            CurY -= 2
                            CellsMade += 2
                        End If
                    End If
                End If
            End While
        ElseIf GenerateType = Swamps Then
            Dim TMapSize As Short = MapSize 'can't multiply constants, so declaring a temp variable
            Dim MapFilled As Short = 0
            Dim BadPos As Boolean = False
            Dim RetryPos As Boolean = True
            Dim RoomWidthSize, RoomHeightSize As Short
            Dim PosX, PosY, CurX, CurY As Integer
            Dim FailedTimes As Short = 0
            While MapFilled < (TMapSize * TMapSize) * 5 / 6
                While RetryPos = True
                    RoomWidthSize = RandomNumber.Next(1, 6) '2-5 roomsize
                    RoomHeightSize = RandomNumber.Next(1, 6)
                    PosX = RandomNumber.Next(0 - RoomWidthSize, MapSize + RoomWidthSize)
                    PosY = RandomNumber.Next(0 - RoomHeightSize, MapSize + RoomHeightSize)
                    For CurX = PosX To PosX + RoomWidthSize Step 1
                        For CurY = PosY To PosY + RoomHeightSize Step 1
                            If CurX >= 0 And CurX <= MapSize And CurY >= 0 And CurY <= MapSize Then
                                If Map(MapLevel, CurX, CurY) = Floor Then
                                    BadPos = True
                                    Exit For
                                End If
                            End If
                        Next
                        If BadPos = True Then
                            Exit For
                        End If
                    Next
                    If BadPos = False Then 'draw floor and walls
                        For CurX = PosX - 1 To PosX + RoomWidthSize + 1 Step 1
                            For CurY = PosY - 1 To PosY + RoomHeightSize + 1 Step 1
                                If CurX < PosX Or CurX > PosX + RoomWidthSize Or CurY < PosY Or CurY > PosY + RoomHeightSize Then 'walls
                                    If CurX >= 0 And CurX <= MapSize And CurY >= 0 And CurY <= MapSize Then 'make sure it stays within the bound of the map
                                        If Map(MapLevel, CurX, CurY) <> Water Then
                                            Map(MapLevel, CurX, CurY) = Water : MapFilled += 1
                                        End If
                                    End If
                                Else
                                    If CurX >= 0 And CurX <= MapSize And CurY >= 0 And CurY <= MapSize Then  'make sure it stays within the bound of the map
                                        If Map(MapLevel, CurX, CurY) <> Floor Then
                                            Map(MapLevel, CurX, CurY) = Floor : MapFilled += 1
                                        End If
                                    End If
                                End If
                            Next
                        Next
                        RetryPos = False
                    Else
                        FailedTimes += 1
                        If FailedTimes > 100 Then
                            RetryPos = False
                            MapFilled = TMapSize * TMapSize
                        End If
                        BadPos = False
                    End If
                End While
                RetryPos = True
            End While
            'now set all walls to floor
            For CurX = 0 To MapSize Step 1
                For CurY = 0 To MapSize Step 1
                    If Map(MapLevel, CurX, CurY) = Wall Then
                        Map(MapLevel, CurX, CurY) = Floor
                    End If
                Next
            Next
        ElseIf GenerateType = Tunnels Or GenerateType = Tunnels2 Then
            Dim AllocatedBlocks As Short = 0
            Dim FailedBlocks As Short = 0
            Dim RandomPosition As New Random
            Dim BuilderSpawned As Boolean = False
            Dim BuilderMoveDirection As Short = 0
            Dim PawnLocationX As Short = Math.Floor(MapSize / 2)
            Dim PawnLocationY As Short = Math.Floor(MapSize / 2)
            While AllocatedBlocks < MapSize * (MapSize / 3) And FailedBlocks < 500
                If BuilderSpawned = False Then
                    'spawn at random position
                    BuilderPositionX = RandomPosition.Next(1, MapSize)
                    BuilderPositionY = RandomPosition.Next(1, MapSize)
                    'see if spawn is within 1 block of pawn after spawn
                    If Math.Abs(PawnLocationX - BuilderPositionX) <= 1 And Math.Abs(PawnLocationY - BuilderPositionY) <= 1 Then
                        'builder was spawned too close to spawn, clear that floor and respawn
                        If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                            Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                            AllocatedBlocks += 1
                        Else
                            FailedBlocks += 1
                        End If
                    ElseIf Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor Then
                        FailedBlocks += 1
                    Else
                        BuilderSpawned = True
                        BuilderMoveDirection = RandomPosition.Next(1, 9)
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
                    ElseIf BuilderMoveDirection = 5 And BuilderPositionX < MapSize And BuilderPositionY > 0 And GenerateType = Tunnels2 Then 'northeast
                        BuilderPositionY -= 1
                        BuilderPositionX += 1
                    ElseIf BuilderMoveDirection = 6 And BuilderPositionX < MapSize And BuilderPositionY < MapSize And GenerateType = Tunnels2 Then 'southeast
                        BuilderPositionY += 1
                        BuilderPositionX += 1
                    ElseIf BuilderMoveDirection = 7 And BuilderPositionX > 0 And BuilderPositionY < MapSize And GenerateType = Tunnels2 Then 'southwest
                        BuilderPositionY += 1
                        BuilderPositionX -= 1
                    ElseIf BuilderMoveDirection = 8 And BuilderPositionX > 0 And BuilderPositionY > 0 And GenerateType = Tunnels2 Then
                        BuilderPositionY -= 1
                        BuilderPositionX -= 1
                    Else
                        'if it wasn't passed it must either be an error or near the side of the map
                        'so go ahead and respawn
                        BuilderSpawned = False
                    End If
                    'see whether the builder is near an existing spot
                    'see whether the builder is near an exit
                    If BuilderPositionX < MapSize And BuilderPositionY < MapSize And BuilderPositionX > 0 And BuilderPositionY > 0 Then
                        If Map(MapLevel, BuilderPositionX + 1, BuilderPositionY) = Floor Then
                            If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                AllocatedBlocks += 1
                            Else
                                FailedBlocks += 1
                            End If
                        ElseIf Map(MapLevel, BuilderPositionX - 1, BuilderPositionY) = Floor Then
                            If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                AllocatedBlocks += 1
                            Else
                                FailedBlocks += 1
                            End If
                        ElseIf Map(MapLevel, BuilderPositionX, BuilderPositionY + 1) = Floor Then
                            If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                AllocatedBlocks += 1
                            Else
                                FailedBlocks += 1
                            End If
                        ElseIf Map(MapLevel, BuilderPositionX, BuilderPositionY - 1) = Floor Then
                            If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                AllocatedBlocks += 1
                            Else
                                FailedBlocks += 1
                            End If
                        ElseIf Map(MapLevel, BuilderPositionX + 1, BuilderPositionY + 1) = Floor Then
                            If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                AllocatedBlocks += 1
                            Else
                                FailedBlocks += 1
                            End If
                        ElseIf Map(MapLevel, BuilderPositionX + 1, BuilderPositionY - 1) = Floor Then
                            If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                AllocatedBlocks += 1
                            Else
                                FailedBlocks += 1
                            End If
                        ElseIf Map(MapLevel, BuilderPositionX - 1, BuilderPositionY - 1) = Floor Then
                            If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                AllocatedBlocks += 1
                            Else
                                FailedBlocks += 1
                            End If
                        ElseIf Map(MapLevel, BuilderPositionX - 1, BuilderPositionY + 1) = Floor Then
                            If Map(MapLevel, BuilderPositionX, BuilderPositionY) <> Floor Then
                                Map(MapLevel, BuilderPositionX, BuilderPositionY) = Floor
                                AllocatedBlocks += 1
                            Else
                                FailedBlocks += 1
                            End If
                        End If
                    Else
                        BuilderSpawned = False
                    End If
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
                FogMap(MapLevel, BuilderPositionX, BuilderPositionY) = noiseFloor(BuilderPositionX, BuilderPositionY) 'show the current state number, debug
            Next
        Next
    End Sub
#End Region
#Region "Tick"
    Sub ReDraw() 'also known as 'tick'
        'check to see if the player is in water and reduce their Ammunition
        If Map(MapLevel, PlayerPosX, PlayerPosY) = Water Then
            Dim Ignorewater = False
            If RiverType = Water And WaterImmune = 0 Then
                PlayerCurAmmunition -= 10
            ElseIf RiverType = Ice And IceImmune = 0 Then
                PlayerCurAmmunition -= 20
            ElseIf RiverType = Lava And LavaImmune = 0 Then
                PlayerCurAmmunition -= 30
            Else
                Ignorewater = True
            End If
            If PlayerCurAmmunition <= 0 Then
                PlayerCurHitpoints += PlayerCurAmmunition
                SND("You use up all your WP.")
                If RiverType = Water And PlayerCurAmmunition <> 0 Then
                    SND("You drown for " + LTrim(Str(Math.Abs(PlayerCurAmmunition))) + "HP.")
                ElseIf RiverType = Ice And PlayerCurAmmunition <> 0 Then
                    SND("You freeze for " + LTrim(Str(Math.Abs(PlayerCurAmmunition))) + "HP.")
                ElseIf RiverType = Lava And PlayerCurAmmunition <> 0 Then
                    SND("You burn for " + LTrim(Str(Math.Abs(PlayerCurAmmunition))) + "HP.")
                End If
                PlayerCurAmmunition = 0
            ElseIf Ignorewater = True Then
                SND("You remain immune.")
                If WaterImmune > 0 Then
                    WaterImmune -= 1
                    If WaterImmune = 0 Then
                        SND("Drown immunity wears off.")
                    End If
                End If
                If IceImmune > 0 Then
                    IceImmune -= 1
                    If IceImmune = 0 Then
                        SND("Freeze immunity wears off.")
                    End If
                End If
                If LavaImmune > 0 Then
                    LavaImmune -= 1
                    If LavaImmune = 0 Then
                        SND("Burn immunity wears off.")
                    End If
                End If
            Else
                If RiverType = Water Then
                    SND("You swim reducing WP by 10.")
                ElseIf RiverType = Ice Then
                    SND("You freeze reducing WP by 20.")
                ElseIf RiverType = Lava Then
                    SND("You burn reducing WP by 30.")
                End If
            End If
            RefreshStats() 'updates Ammunition and health bar statistics
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
                        If MobileType(MapLevel, ProcessMobilePathNumber) = 1 Then
                            MobileNameString = "A rat"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 2 Then
                            MobileNameString = "A bat"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 3 Then
                            MobileNameString = "An imp"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 4 Then
                            MobileNameString = "A goblin"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 5 Then
                            MobileNameString = "A troll"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 6 Then
                            MobileNameString = "An ogre"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 7 Then
                            MobileNameString = "A catoblepas"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 8 Then
                            MobileNameString = "A parandrus"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 9 Then
                            MobileNameString = "A clurichuan"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 10 Then
                            MobileNameString = "A dullahan"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 11 Then
                            MobileNameString = "A golem"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 12 Then
                            MobileNameString = "A sceadugengan"
                        ElseIf MobileType(MapLevel, ProcessMobilePathNumber) = 13 Then
                            MobileNameString = "A schilla"
                        End If
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
        If PlayerLevel < 100 Then PlayerLevel += 1
        strcur.Text = LTrim(Str(PlayerSTR)) : strmax.Text = LTrim(Str(PlayerMaxSTR)) : If PlayerSTR < PlayerMaxSTR Then stradd.Enabled = True
        dexcur.Text = LTrim(Str(PlayerDEX)) : dexmax.Text = LTrim(Str(PlayerMaxDEX)) : If PlayerDEX < PlayerMaxDEX Then dexadd.Enabled = True
        intcur.Text = LTrim(Str(PlayerINT)) : intmax.Text = LTrim(Str(PlayerMaxINT)) : If PlayerINT < PlayerMaxINT Then intadd.Enabled = True
        wiscur.Text = LTrim(Str(PlayerWIS)) : wismax.Text = LTrim(Str(PlayerMaxWIS)) : If PlayerWIS < PlayerMaxWIS Then wisadd.Enabled = True
        concur.Text = LTrim(Str(PlayerCON)) : conmax.Text = LTrim(Str(PlayerMaxCON)) : If PlayerCON < PlayerMaxCON Then conadd.Enabled = True
        chacur.Text = LTrim(Str(PlayerCHA)) : chamax.Text = LTrim(Str(PlayerMaxCHA)) : If PlayerCHA < PlayerMaxCHA Then chaadd.Enabled = True
        luccur.Text = LTrim(Str(PlayerLUC)) : lucmax.Text = LTrim(Str(PlayerMaxLuc)) : If PlayerLUC < PlayerMaxLuc Then lucadd.Enabled = True
        hpcur.Text = LTrim(Str(PlayerHitpoints)) : hpadd.Enabled = True
        wpcur.Text = LTrim(Str(PlayerAmmunition)) : wpadd.Enabled = True
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
        PlayerAmmunition = Val(wpcur.Text)
        PlayerDefense += Math.Round(PlayerCON / 5, 0) - Math.Round(PrevCon / 5, 0)
        PlayerAttack += Math.Round(PlayerSTR / 5, 0) - Math.Round(PrevStr / 5, 0)
        LevelUpPanel.Visible = False
        RefreshStats()
    End Sub
    Public Sub RefreshStats()
        HealthBar.Caption = LTrim(Str(PlayerCurHitpoints)) + " / " + LTrim(Str(PlayerHitpoints)) + " HP"
        HealthBar.Value = PlayerCurHitpoints
        HealthBar.Max = PlayerHitpoints
        AmmunitionBar.Caption = LTrim(Str(PlayerCurAmmunition)) + " / " + LTrim(Str(PlayerAmmunition)) + " WP"
        AmmunitionBar.Value = PlayerCurAmmunition
        AmmunitionBar.Max = PlayerAmmunition
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
    Public Sub SND(ByVal Text As String, Optional ByVal Clear As Boolean = False) 'this displays the display text
        Dim pxish As Short = TheRoomWidth * PlayerPosX + ColumnsSpace * PlayerPosX 'current player sector x position
        Dim pyish As Short = TheRoomHeight * PlayerPosY + ColumnsSpace * PlayerPosY + 25 'current player sector y position
        If Clear = True Then
            HUDisplay.Text = ""
            HUDisplay.Visible = False
        ElseIf Screensaver = False Then
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
        If PlayerDead = False Then
            If e.KeyCode = Keys.Up And PlayerPosY > 0 And PlayerTargeting = False Or e.KeyCode = Keys.NumPad8 And PlayerPosY > 0 And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX, PlayerPosY - 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosY -= 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX, PlayerPosY - 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX, PlayerPosY - 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Down And PlayerPosY < MapSize And PlayerTargeting = False Or e.KeyCode = Keys.NumPad2 And PlayerPosY < MapSize And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX, PlayerPosY + 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosY += 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX, PlayerPosY + 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX, PlayerPosY + 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Right And PlayerPosX < MapSize And PlayerTargeting = False Or e.KeyCode = Keys.NumPad6 And PlayerPosX < MapSize And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX + 1, PlayerPosY) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosX += 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX + 1, PlayerPosY) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY) <> 0 Then
                    PlayerHitLocation(PlayerPosX + 1, PlayerPosY)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.Left And PlayerPosX > 0 And PlayerTargeting = False Or e.KeyCode = Keys.NumPad4 And PlayerPosX > 0 And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX - 1, PlayerPosY) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosX -= 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX - 1, PlayerPosY) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY) <> 0 Then
                    PlayerHitLocation(PlayerPosX - 1, PlayerPosY)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad7 And PlayerPosX > 0 And PlayerPosY > 0 And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX - 1, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY - 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosX -= 1 : PlayerPosY -= 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX - 1, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY - 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX - 1, PlayerPosY - 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad9 And PlayerPosX < MapSize And PlayerPosY > 0 And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX + 1, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY - 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosX += 1 : PlayerPosY -= 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX + 1, PlayerPosY - 1) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY - 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX + 1, PlayerPosY - 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad3 And PlayerPosX < MapSize And PlayerPosY < MapSize And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX + 1, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY + 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
                    PlayerPosX += 1 : PlayerPosY += 1
                    ReDraw()
                ElseIf Map(MapLevel, PlayerPosX + 1, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX + 1, PlayerPosY + 1) <> 0 Then
                    PlayerHitLocation(PlayerPosX + 1, PlayerPosY + 1)
                    ReDraw()
                End If
            ElseIf e.KeyCode = Keys.NumPad1 And PlayerPosX > 0 And PlayerPosY < MapSize And PlayerTargeting = False Then
                If Map(MapLevel, PlayerPosX - 1, PlayerPosY + 1) > 0 And MapOccupied(MapLevel, PlayerPosX - 1, PlayerPosY + 1) = 0 Then
                    PlayerLastPosX = PlayerPosX : PlayerLastPosY = PlayerPosY
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
                    If PlayerCurAmmunition >= 5 Then
                        SND("Shoot in what direction?")
                        SkillType = "Shoot"
                        PlayerCurAmmunition -= 5
                        DrawingProcedures.TargetEnemy()
                        SND("Press Spacebar to shoot.")
                        PlayerTargeting = True
                    Else
                        SND("Not enough Ammunition.")
                    End If
                Else
                    SND("No enemies are around.")
                End If
            ElseIf e.KeyCode = Keys.Space And PlayerTargeting = False Then
                SND("You are not targeting anything.")
            ElseIf e.KeyCode = Keys.NumPad5 Then
                If PlayerCurHitpoints < PlayerHitpoints Then
                    PlayerCurHitpoints += 1
                    RefreshStats() 'updates Ammunition and health bar statistics
                End If
                If PlayerCurAmmunition < PlayerAmmunition Then
                    PlayerCurAmmunition += 1
                    RefreshStats() 'updates Ammunition and health bar statistics
                End If
                ReDraw()
            ElseIf e.KeyCode = Keys.H Or e.KeyCode = Keys.OemQuestion And e.Shift = True Then
                HelpClick(0, EventArgs.Empty)
            ElseIf e.KeyCode = Keys.OemPeriod And e.Shift = True Then 'go down
                If Map(MapLevel, PlayerPosX, PlayerPosY) = StairsDown Then 'exit
                    MapLevel += 1
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
        'if the character stats panel is visble then recheck the stats and draw them after each round
        If CharStats.Visible = True Then
            CharacterStatsRefresh()
        End If
    End Sub
    Public Sub SNDScores()
        Output.Text = HighScores + Chr(13) + AddSpace(PlayerName, 20) + AddSpace(PlayerRace, 14) + AddSpace(PlayerClass, 17) + AddSpace(LTrim(Str(PlayerLevel)), 8) + AddSpace(LTrim(Str(PlayerExperience)), 13) + AddSpace(LTrim(Str(MapLevel)), 13) + AddSpace(LTrim(Str(PlayerGold)), 10) + LTrim(Str(PlayerTurns))
        HighScores = Output.Text 'incase character restarts game
    End Sub
#Region "Menu Click"
    Private Sub NewGameClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewGameToolStripMenuItem.Click, NewGameToolStripMenuItem1.Click
        'enable the toggle strip on the file menu
        StatStrip.Enabled = True
        ToggleStrip.Enabled = True
        LogStrip.Enabled = True
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
        'setup window
        Initialize(0, EventArgs.Empty)
        Array.Clear(LOSMap, 0, LOSMap.Length) 'set all to hidden
        Array.Clear(MapCreated, 0, MapCreated.Length)
        'inventory shizzy
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
    End Sub
    Private Sub ToggleActivityLogClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowHideActivityLogToolStripMenuItem.Click, LogStrip.Click
        If LogVisible = True Then
            LogVisible = False
        ElseIf LogVisible = False Then
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
        SND(0, True) 'mobile moves each tick, go ahead and clear snd logs if visible
        If Screensaver = True Then 'move character, it's the screensaver
            If PlayerPosX = ExitPosX And PlayerPosY = ExitPosY Then
                MapLevel += 1
                BuildNewMap(False)
                ScreensaverFound = False
            Else
                MobilePosX(MapLevel, MaxMobiles) = PlayerPosX : MobilePosY(MapLevel, MaxMobiles) = PlayerPosY
                MobilePrevX(MapLevel, MaxMobiles) = PlayerPosX : MobilePrevY(MapLevel, MaxMobiles) = PlayerPosY
                MobileType(MapLevel, MaxMobiles) = 1
                MobileHealth(MapLevel, MaxMobiles) = 100
                PlayerHitpoints = 100 : PlayerCurHitpoints = 100
                PlayerAmmunition = 100 : PlayerCurAmmunition = 100
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
End Class
