Module DrawingProcedures
#Region "Constants"
    'Mapsize constant has been changed a lot, go ahead and pass that from the main form where necessary instead of creating a
    'constant for it here.

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

    Public Const Hidden As Short = 0 'used for MapDrawStatus, prevents recursive drawing on something already visible
    Public Const Visible As Short = 1 'ditto
    Public Const Shadowed As Short = 2 'ditto again
    Public Const Redraw As Short = 3 'forces redraw no matter what

    Const TotalEnvironmentTypes As Short = 10

    Const Dungeon = 0
    Const Ruins = 1
    Const Classic = 2
    Const Random = 3

    'drawing constants only
    Const MobXPosition = 0
    Const MobYPosition = 1
    Const MobType = 2
    Const MobNumber = 3
#End Region
#Region "Dimensions and Declarations"
    Public displayfont As New Font("Arial", 24)
    Public displayfont2 As New Font("Arial", 12)
    Public ChangedMode As Boolean = False
    Public LOSMap(MainForm.MapSize, MainForm.MapSize) As Short
    Private CurrentlyDisplayedMobile As Byte
    Private FloorGraphic(3) As Image
    Private WallGraphic As Image
    Private DrawnGraphics(TotalEnvironmentTypes, 4) As Boolean '4 is wall
#End Region
#Region "Image Filters"
    Function FilterImageRed(ByVal TheObject As Image) As System.Object
        If MainForm.ImageFilterOn.Checked = True Then
            Dim i, j, a As Integer
            Dim c As System.Drawing.Color
            Dim c2 As System.Drawing.Color
            Dim pic1 As System.Drawing.Bitmap
            Dim pic2 As System.Drawing.Bitmap
            Dim r1, b1, g1, r2, b2, g2 As Integer
            pic1 = My.Resources.BloodSplatter
            pic2 = TheObject
            For j = 1 To TheObject.Height - 2
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
            Return pic2
        End If
        Return TheObject
    End Function
    Function FilterImageFog(ByVal TheObject As Image, ByVal TheColor As Byte) As System.Object
        If MainForm.ImageFilterOn.Checked = True Then
            Dim i, j, a As Integer
            Dim c2 As System.Drawing.Color
            Dim pic2 As System.Drawing.Bitmap
            Dim r1, b1, g1, r2, b2, g2 As Integer
            Dim ResultColor As Color
            pic2 = TheObject
            For j = 0 To TheObject.Height - 1
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
        End If
        Return TheObject
    End Function
    Function FilterImageWall(ByVal TheObject As Image, ByVal TheFloor As Image, ByVal N As Boolean, ByVal E As Boolean, ByVal S As Boolean, ByVal W As Boolean) As System.Object
        If MainForm.ImageFilterOn.Checked = True Then
            'right now it's a test function to make transparent outsides -> fade into opaque insides
            Dim x, y, a As Integer
            Dim c1, c2 As System.Drawing.Color
            Dim Floor As System.Drawing.Bitmap
            Dim BG As System.Drawing.Bitmap
            Dim r1, b1, g1, r2, b2, g2 As Integer
            Dim ResultColor As Color
            Dim GoDirection As Short
            Dim AlphaBG As Double
            Dim AlphaFloor As Double
            Dim FadePercent As Double = 0.2 'change this, .1-.3
            Dim NegPercent As Double = 1 - FadePercent
            'The fade will last for 10% of the tiles width or height
            BG = TheObject
            Floor = TheFloor
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
                            AlphaFloor = 1
                        End If
                        If GoDirection = South And N = True Then
                            AlphaBG = ((TheObject.Height * FadePercent) - y) / (TheObject.Height * FadePercent)
                            AlphaFloor = y / (TheObject.Height * FadePercent)
                        ElseIf GoDirection = East And W = True Then
                            AlphaBG = ((TheObject.Width * FadePercent) - x) / (TheObject.Width * FadePercent)
                            AlphaFloor = x / (TheObject.Width * FadePercent)
                        ElseIf GoDirection = North And S = True Then
                            AlphaBG = ((TheObject.Height - (TheObject.Height * NegPercent)) - (TheObject.Height - y)) / (TheObject.Height - (TheObject.Height * NegPercent))
                            AlphaFloor = (TheObject.Height - y) / (TheObject.Height - (TheObject.Height * NegPercent))
                        ElseIf GoDirection = West And E = True Then
                            AlphaBG = ((TheObject.Width - (TheObject.Width * NegPercent)) - (TheObject.Width - x)) / (TheObject.Width - (TheObject.Width * NegPercent))
                            AlphaFloor = (TheObject.Width - x) / (TheObject.Width - (TheObject.Width * NegPercent))
                        Else
                            AlphaBG = 1
                            AlphaFloor = 1
                        End If
                        c2 = BG.GetPixel(x, y)
                        c1 = Floor.GetPixel(x, y)
                        r1 = c1.R : r2 = c2.R
                        g1 = c1.G : g2 = c2.G
                        b1 = c1.B : b2 = c2.B
                        If r1 = 255 And g1 = 255 And b1 = 255 Then
                            ResultColor = Color.FromArgb(a, r2, g2, b2)
                            BG.SetPixel(x, y, ResultColor)
                        Else
                            r2 = (r1 * AlphaFloor + r2 * AlphaBG) / 2 : g2 = (g1 * AlphaFloor + g2 * AlphaBG) / 2 : b2 = (b1 * AlphaFloor + b2 * AlphaBG) / 2
                            ResultColor = Color.FromArgb(a, r2, g2, b2)
                            BG.SetPixel(x, y, ResultColor)
                        End If
                    End If
                Next
            Next
            Return BG 'returns a modified background
        End If
        Return TheFloor
    End Function
    Function FilterImageWater(ByVal TheObject As Image, ByVal TheWater As Image) As System.Object
        If MainForm.ImageFilterOn.Checked = True Then
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
        End If
        Return TheWater
    End Function
#End Region
#Region "Player FoV / LoS"
    Function IsVisible(ByVal x As Short, ByVal y As Short, ByVal playerposx As Short, ByVal playerposy As Short) As Short
        Dim TestVarX As Short = playerposx
        Dim TestVarY As Short = playerposy
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
            If MainForm.Map(MainForm.MapLevel, TestVarX, TestVarY) = 0 Then
                TestResult += 1
            ElseIf TestResult = 1 Then
                TestResult += 1 'this ensures you can't see through one wall
            End If
        End While
        Return TestResult
    End Function
    Function IsVisible2(ByVal x As Short, ByVal y As Short, ByVal PlayerPosX As Short, ByVal PlayerPosY As Short) As Short
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
            If MainForm.Map(MainForm.MapLevel, TestVarX, TestVarY) = 0 Then
                TestResult += 1
            ElseIf TestResult = 1 Then
                TestResult += 1 'this ensures you can't see through one wall
            End If
        End While
        Return TestResult
    End Function
#End Region
    Public Sub DrawMap(ByVal GraphicalMode As Boolean)
        'Here are variables that are passed from the mainform, re-stated here to distinguish easier which are passed and allow
        'better mobility for future changes.
        '
        Dim Mapsize As Short = MainForm.MapSize
        Dim PlayerPosX As Short = MainForm.PlayerPosX
        Dim PlayerposY As Short = MainForm.PlayerPosY
        Dim EnvironmentType As Short = MainForm.EnvironmentType 'different looking sets of tiles, like jungle scene, ice scene etc.
        Dim TheRoomWidth As Short = MainForm.TheRoomWidth
        Dim TheRoomHeight As Short = MainForm.TheRoomHeight
        Dim ColumnsSpace As Short = MainForm.ColumnsSpace
        Dim RowSpace As Short = MainForm.RowSpace
        '
        Dim xish, xishPLUS, yish, yishPLUS As Integer
        Dim RandomNum As New Random
        'Mobile placed reduced the need to check for mobile targeting if nothings on screen, less overhead
        Dim MobilePlaced As Boolean = False
        CurrentlyDisplayedMobile = 1 'reset to 1, this allows targeting to auto-sort list and clear else each time
        'wallart and floor art is assigned depending on environmenttype which is dependant on the current depth or level
        Dim WallArt As Bitmap
        Dim FloorArt As Bitmap
        'define bounds, start x visible area -1 to x visible area +1, same with y, no need to check whole map, it's already written
        'just make sure you check where player is moving and where he could have moved to see if tiles need to be replaced.
        Dim StartX As Short = PlayerPosX - 5 : If StartX < 0 Then StartX = 0
        Dim StartY As Short = PlayerposY - 5 : If StartY < 0 Then StartY = 0
        Dim FinishX As Short = PlayerPosX + 5 : If FinishX > Mapsize Then FinishX = Mapsize
        Dim FinishY As Short = PlayerposY + 5 : If FinishY > Mapsize Then FinishY = Mapsize
        If ChangedMode = True Then 'required to redraw entire map, used when grahpical mode is changed
            StartX = 0 : StartY = 0 : FinishX = Mapsize : FinishY = Mapsize
        End If
        'start at top left and go to bottom right
        For x = StartX To FinishX Step 1
            For y = StartY To FinishY Step 1
                'sets wall and floor art dependant on environment type which is dependant on current depth or dungeon level
                If GraphicalMode = Tiled Then
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
                End If
                'xish and yish is the x and y location on the screen that the tile will be placed upon
                xish = TheRoomWidth * x + ColumnsSpace * x + 1
                yish = TheRoomHeight * y + RowSpace * y + 25
                xishPLUS = Val(TheRoomWidth) * x + Val(ColumnsSpace) * x + 10 + Val(TheRoomWidth)
                yishPLUS = Val(TheRoomHeight) * y + Val(RowSpace) * y + 10 + Val(TheRoomHeight)
                'MainForm.CANVAS.DrawRectangle(Pens.Pink, xish, yish, TheRoomWidth, TheRoomHeight) 'used to test canvas size
                'draws a circle around player 4 wide on each side for visibility, remember that x and y are passed starting -5 to 5 of characters current position.
                If ((Math.Pow(PlayerPosX - x, 2) + Math.Pow(PlayerposY - y, 2)) < (Math.Pow(4, 2))) Or MainForm.AdminVisible = True Then 'admin visible shows all
                    'within range of player, only process isvisible routines if it's within 4 of character so it doesn't process unnecessary squares too far from player
                    If IsVisible(x, y, PlayerPosX, PlayerposY) <= 1 Or IsVisible2(x, y, PlayerPosX, PlayerposY) <= 1 Or MainForm.AdminVisible = True Or ChangedMode = True And MainForm.MapShown(MainForm.MapLevel, x, y) = True Then
                        'within range of player and is visible
                        If LOSMap(x, y) <> Visible Then 'should be visible, tile not currently visible, change then set map as visible
                            LOSMap(x, y) = Visible
                            'draw wall
                            DrawTile(x, y, xish, yish, TheRoomWidth, TheRoomHeight, GraphicalMode, WallArt, FloorArt)
                        End If
                        'if map is occupied, show enemy
                        If MainForm.MapOccupied(MainForm.MapLevel, x, y) > 0 Then
                            LOSMap(x, y) = Redraw
                            MobilePlaced = True
                            ShowEnemy(MainForm.MapOccupied(MainForm.MapLevel, x, y), xish, yish, x, y)
                            CurrentlyDisplayedMobile += 1
                        Else
                            'if map isn't occupied by mobiles, is it occupied by items, if so show items (prioritize enemy showing over items)
                            If MainForm.ItemOccupied(MainForm.MapLevel, x, y) > 0 Then
                                LOSMap(x, y) = Redraw
                                ShowItem(xish, yish, x, y)
                            End If
                        End If
                        'player can be shown over items for better visibility
                        If x = PlayerPosX And y = PlayerposY Then
                            LOSMap(x, y) = Redraw
                            MainForm.CANVAS.DrawString("@", displayfont, Brushes.LimeGreen, xish, yish)
                        End If
                        MainForm.MapShown(MainForm.MapLevel, x, y) = True
                    ElseIf MainForm.MapShown(MainForm.MapLevel, x, y) = True Then
                        'not within the visual sight of the player, but was visited, so should just be fogged
                        Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                        If LOSMap(x, y) <> Shadowed Then
                            LOSMap(x, y) = Shadowed
                            'draw wall
                            DrawTile(x, y, xish, yish, TheRoomWidth, TheRoomHeight, GraphicalMode, WallArt, FloorArt)
                            'shadow the area
                            MainForm.MapShown(MainForm.MapLevel, x, y) = True
                            MainForm.CANVAS.FillRectangle(semiTransBrush, xish, yish, TheRoomWidth, TheRoomHeight)
                        End If
                    End If
                ElseIf MainForm.MapShown(MainForm.MapLevel, x, y) = True Then
                    'not within the visual sight of the player, but was visited, so should just be fogged
                    Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                    If LOSMap(x, y) <> Shadowed Then
                        LOSMap(x, y) = Shadowed
                        DrawTile(x, y, xish, yish, TheRoomWidth, TheRoomHeight, GraphicalMode, WallArt, FloorArt)
                        'shadow the area
                        MainForm.MapShown(MainForm.MapLevel, x, y) = True
                        MainForm.CANVAS.FillRectangle(semiTransBrush, xish, yish, TheRoomWidth, TheRoomHeight)
                    End If
                End If
            Next
        Next
        If MobilePlaced = True Then
            MainForm.MobilePresent = True
            SortEnemyRangeList() 'puts the closest enemy on 0
        Else
            MainForm.MobilePresent = False
        End If
        MainForm.CreateGraphics.DrawImage(MainForm.PAD, 0, 0)
    End Sub
    Sub DrawTile(ByVal x As Short, ByVal y As Short, ByVal xish As Short, ByVal yish As Short, ByVal TheRoomWidth As Short, ByVal TheRoomHeight As Short, ByVal GraphicalMode As Boolean, ByVal wallart As Bitmap, ByVal floorart As Bitmap)
        If MainForm.Map(MainForm.MapLevel, x, y) = Wall Then
            If GraphicalMode = Tiled Then
                If MainForm.ImageFilterOn.Checked = True Then
                    MainForm.CANVAS.DrawImage(FilterImageWall(floorart, wallart, MainForm.MapBlur(MainForm.MapLevel, x, y, 2), MainForm.MapBlur(MainForm.MapLevel, x, y, 1), MainForm.MapBlur(MainForm.MapLevel, x, y, 0), MainForm.MapBlur(MainForm.MapLevel, x, y, 3)), xish, yish, TheRoomWidth, TheRoomHeight)
                Else
                    MainForm.CANVAS.DrawImage(wallart, xish, yish, TheRoomWidth, TheRoomHeight)
                End If
            ElseIf GraphicalMode = ASCII Then
                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                MainForm.CANVAS.DrawString("#", displayfont, Brushes.DarkGray, xish, yish)
            End If
                'draw floor
            ElseIf MainForm.Map(MainForm.MapLevel, x, y) = Floor Then
                If GraphicalMode = Tiled Then
                If MainForm.ImageFilterOn.Checked = True Then
                    ShowFog(xish, yish, x, y, floorart)
                Else
                    MainForm.CANVAS.DrawImage(floorart, xish, yish, TheRoomWidth, TheRoomHeight)
                End If
            ElseIf GraphicalMode = ASCII Then
                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                MainForm.CANVAS.DrawString(".", displayfont, Brushes.DarkGray, xish, yish)
            End If
                'draw floor with blood
            ElseIf MainForm.Map(MainForm.MapLevel, x, y) = SpecialFloor Then
                If GraphicalMode = Tiled Then
                If MainForm.ImageFilterOn.Checked = True Then
                    ShowFog(xish, yish, x, y, FilterImageRed(floorart), True)
                Else
                    MainForm.CANVAS.DrawImage(floorart, xish, yish, TheRoomWidth, TheRoomHeight)
                    MainForm.CANVAS.DrawString("x", displayfont, Brushes.DarkRed, xish, yish)
                End If
            ElseIf GraphicalMode = ASCII Then
                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                MainForm.CANVAS.DrawString("x", displayfont, Brushes.DarkRed, xish, yish)
            End If
                'draw stairs up
            ElseIf MainForm.Map(MainForm.MapLevel, x, y) = StairsUp Then
                If GraphicalMode = Tiled Then
                    MainForm.CANVAS.DrawImage(My.Resources.StairsUp, xish, yish, TheRoomWidth, TheRoomHeight)
                ElseIf GraphicalMode = ASCII Then
                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                    MainForm.CANVAS.DrawString("<", displayfont, Brushes.DarkGray, xish, yish)
                End If
                'draw stairs down
            ElseIf MainForm.Map(MainForm.MapLevel, x, y) = StairsDown Then
                If GraphicalMode = Tiled Then
                    MainForm.CANVAS.DrawImage(My.Resources.StairsDown, xish, yish, TheRoomWidth, TheRoomHeight)
                ElseIf GraphicalMode = ASCII Then
                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                    MainForm.CANVAS.DrawString(">", displayfont, Brushes.DarkGray, xish, yish)
                End If
            ElseIf MainForm.Map(MainForm.MapLevel, x, y) = Lava Then
                If GraphicalMode = Tiled Then
                    MainForm.CANVAS.DrawImage(FilterImageWall(floorart, My.Resources.Lava, True, True, False, False), xish, yish, TheRoomWidth, TheRoomHeight)
                ElseIf GraphicalMode = ASCII Then
                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                    MainForm.CANVAS.DrawString("~", displayfont, Brushes.DarkRed, xish, yish)
                End If
            ElseIf MainForm.Map(MainForm.MapLevel, x, y) = Water Then
                If GraphicalMode = Tiled Then
                    MainForm.CANVAS.DrawImage(FilterImageWater(floorart, My.Resources.Water), xish, yish, TheRoomWidth, TheRoomHeight)
                ElseIf GraphicalMode = ASCII Then
                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                    MainForm.CANVAS.DrawString("~", displayfont, Brushes.DarkBlue, xish, yish)
                End If
            ElseIf MainForm.Map(MainForm.MapLevel, x, y) = Ice Then
                If GraphicalMode = Tiled Then
                    MainForm.CANVAS.DrawImage(FilterImageWater(floorart, My.Resources.Ice), xish, yish, TheRoomWidth, TheRoomHeight)
                ElseIf GraphicalMode = ASCII Then
                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                    MainForm.CANVAS.DrawString("#", displayfont, Brushes.DarkCyan, xish, yish)
                End If
            End If
    End Sub
    Sub SortEnemyRangeList()
        Dim MobileNumber As Short = 0
        MainForm.MobileVisible(MainForm.MapLevel, 0, MobType) = 0 'Clear the first targetable option before sort
        CurrentlyDisplayedMobile -= 1 'is always 1 too many as it's added after displayed
        For MobileNumber = 1 To CurrentlyDisplayedMobile Step 1
            'if the mobile is within range of player or current target is dead (Prioritizes next option before declaring a target fault)
            If Math.Abs(MainForm.MobileVisible(MainForm.MapLevel, MobileNumber, MobXPosition) - MainForm.PlayerPosX) + Math.Abs(MainForm.MobileVisible(MainForm.MapLevel, MobileNumber, MobYPosition) - MainForm.PlayerPosY) < Math.Abs(MainForm.MobileVisible(MainForm.MapLevel, 0, MobXPosition) - MainForm.PlayerPosX) + Math.Abs(MainForm.MobileVisible(MainForm.MapLevel, 0, MobYPosition) - MainForm.PlayerPosY) And MainForm.MobileVisible(MainForm.MapLevel, MobileNumber, MobType) Or MainForm.MobileHealth(MainForm.MapLevel, MainForm.MobileVisible(MainForm.MapLevel, 0, MobNumber)) <= 0 Then
                If MainForm.MobileHealth(MainForm.MapLevel, MainForm.MobileVisible(MainForm.MapLevel, MobileNumber, MobNumber)) > 0 Then 'ensures that the new target mob is alive! lol, no need
                    MainForm.MobileVisible(MainForm.MapLevel, 0, MobXPosition) = MainForm.MobileVisible(MainForm.MapLevel, MobileNumber, MobXPosition)
                    MainForm.MobileVisible(MainForm.MapLevel, 0, MobYPosition) = MainForm.MobileVisible(MainForm.MapLevel, MobileNumber, MobYPosition)
                    MainForm.MobileVisible(MainForm.MapLevel, 0, MobNumber) = MainForm.MobileVisible(MainForm.MapLevel, MobileNumber, MobNumber)
                    MainForm.MobileVisible(MainForm.MapLevel, MobileNumber, MobType) = 0 'shows that the mobile was processed, basically clears it for future processes
                End If
            End If
        Next
    End Sub
    Sub ShowEnemy(ByVal EnemyNum As Short, ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short)
        If EnemyNum = 1 Then 'creatures are currently being hidden in the shadows.
            MainForm.CANVAS.DrawString("r", displayfont, Brushes.Red, xish, yish) 'rat
        ElseIf EnemyNum = 2 Then
            MainForm.CANVAS.DrawString("b", displayfont, Brushes.Red, xish, yish) 'bat
        ElseIf EnemyNum = 3 Then
            MainForm.CANVAS.DrawString("i", displayfont, Brushes.Red, xish, yish) 'imp
        ElseIf EnemyNum = 4 Then
            MainForm.CANVAS.DrawString("g", displayfont, Brushes.Red, xish, yish) 'goblin
        ElseIf EnemyNum = 5 Then
            MainForm.CANVAS.DrawString("t", displayfont, Brushes.Red, xish, yish) 'troll
        ElseIf EnemyNum = 6 Then
            MainForm.CANVAS.DrawString("o", displayfont, Brushes.Red, xish, yish) 'ogre
        ElseIf EnemyNum = 7 Then
            MainForm.CANVAS.DrawString("c", displayfont, Brushes.Red, xish, yish) 'catoblepas
        ElseIf EnemyNum = 8 Then
            MainForm.CANVAS.DrawString("p", displayfont, Brushes.Red, xish, yish) 'parandrus
        ElseIf EnemyNum = 9 Then
            MainForm.CANVAS.DrawString("C", displayfont, Brushes.Red, xish, yish) 'Clurichuan
        ElseIf EnemyNum = 10 Then
            MainForm.CANVAS.DrawString("d", displayfont, Brushes.Red, xish, yish) 'Dullahan
        ElseIf EnemyNum = 11 Then
            MainForm.CANVAS.DrawString("G", displayfont, Brushes.Red, xish, yish) 'Golem
        ElseIf EnemyNum = 12 Then
            MainForm.CANVAS.DrawString("s", displayfont, Brushes.Red, xish, yish) 'sceadugengan
        ElseIf EnemyNum = 13 Then
            MainForm.CANVAS.DrawString("S", displayfont, Brushes.Red, xish, yish) 'Schilla
        End If
        'since map contains an enemy, throw that enemy and their details into a list that can be back-traced
        'this list is used for ranged weapons.
        MainForm.MobileVisible(MainForm.MapLevel, CurrentlyDisplayedMobile, MobXPosition) = x
        MainForm.MobileVisible(MainForm.MapLevel, CurrentlyDisplayedMobile, MobYPosition) = y
        MainForm.MobileVisible(MainForm.MapLevel, CurrentlyDisplayedMobile, MobType) = EnemyNum
        MainForm.MobileVisible(MainForm.MapLevel, CurrentlyDisplayedMobile, MobNumber) = MainForm.MobOccupied(MainForm.MapLevel, x, y)
    End Sub
    Sub TargetEnemy(Optional ByVal clear As Boolean = False)
        'to ensure all enemies within target list are still viable and alive, sort through list and null else
        '
        Dim Mapsize As Short = MainForm.MapSize
        Dim PlayerPosX As Short = MainForm.PlayerPosX
        Dim PlayerposY As Short = MainForm.PlayerPosY
        Dim EnvironmentType As Short = MainForm.EnvironmentType
        Dim TheRoomWidth As Short = MainForm.TheRoomWidth
        Dim TheRoomHeight As Short = MainForm.TheRoomHeight
        Dim ColumnsSpace As Short = MainForm.ColumnsSpace
        Dim RowSpace As Short = MainForm.RowSpace
        Dim x As Short = MainForm.MobileVisible(MainForm.MapLevel, 0, MobXPosition)
        Dim y As Short = MainForm.MobileVisible(MainForm.MapLevel, 0, MobYPosition)
        Dim xish As Short = TheRoomWidth * x + ColumnsSpace * x + 1
        Dim yish As Short = TheRoomHeight * y + RowSpace * y + 25
        '
        If clear = False Then
            MainForm.CANVAS.DrawRectangle(Pens.IndianRed, xish, yish, TheRoomWidth - 2, TheRoomHeight - 2)
        ElseIf clear = True Then
            MainForm.CANVAS.DrawRectangle(Pens.Black, xish, yish, TheRoomWidth - 2, TheRoomHeight - 2)
        End If
        MainForm.CreateGraphics.DrawImage(MainForm.PAD, 0, 0)
    End Sub
    Sub ShowItem(ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short)
        If MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = Gold Then
            MainForm.CANVAS.DrawString("g", displayfont, Brushes.Yellow, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = TheEverspark Then
            MainForm.CANVAS.DrawString("E", displayfont, Brushes.White, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = Weapon Then
            MainForm.CANVAS.DrawString("g", displayfont, Brushes.DarkCyan, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = GenerateItem.Food Then
            MainForm.CANVAS.DrawString("f", displayfont, Brushes.LightYellow, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = GenerateItem.Water Then
            MainForm.CANVAS.DrawString("w", displayfont, Brushes.Aqua, xish, yish)
        Else
            MainForm.CANVAS.DrawString(MainForm.ItemShowType(MainForm.MapLevel, x, y), displayfont, Brushes.Green, xish, yish)
        End If
    End Sub
    Sub ShowFog(ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short, ByVal FloorArt As Image, Optional ByVal ForceRedraw As Boolean = False)
        'Here are variables that are passed from the mainform, re-stated here to distinguish easier which are passed and allow
        'better mobility for future changes.
        '
        Dim TheRoomWidth As Short = MainForm.TheRoomWidth
        Dim TheRoomHeight As Short = MainForm.TheRoomHeight
        Dim CurrentFogDepth = MainForm.FogMap(MainForm.MapLevel, x, y) : If CurrentFogDepth > 3 Then CurrentFogDepth = 0
        If DrawnGraphics(MainForm.EnvironmentType, CurrentFogDepth) = False Then
            FloorGraphic(CurrentFogDepth) = FilterImageFog(FloorArt, CurrentFogDepth * 10)
            DrawnGraphics(MainForm.EnvironmentType, CurrentFogDepth) = True
        End If
        '
        If ForceRedraw = False Then
            If CurrentFogDepth = 1 Then
                MainForm.CANVAS.DrawImage(FloorGraphic(CurrentFogDepth), xish, yish, TheRoomWidth, TheRoomHeight)
            ElseIf CurrentFogDepth = 2 Then
                MainForm.CANVAS.DrawImage(FloorGraphic(CurrentFogDepth), xish, yish, TheRoomWidth, TheRoomHeight)
            ElseIf CurrentFogDepth = 3 Then
                MainForm.CANVAS.DrawImage(FloorGraphic(CurrentFogDepth), xish, yish, TheRoomWidth, TheRoomHeight)
            Else
                MainForm.CANVAS.DrawImage(FloorGraphic(CurrentFogDepth), xish, yish, TheRoomWidth, TheRoomHeight)
            End If
        Else 'bloodsplatter, force draw
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, CurrentFogDepth * 10), xish, yish, TheRoomWidth, TheRoomHeight)
        End If
    End Sub
End Module
