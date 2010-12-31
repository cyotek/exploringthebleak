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

    Const Hidden As Short = 0 'used for MapDrawStatus, prevents recursive drawing on something already visible
    Const NotHidden As Short = 1 'ditto
    Const Shadowed As Short = 2 'ditto again

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
#End Region
#Region "Image Filters"
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
            If MainForm.Map(TestVarX, TestVarY) = 0 Then
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
            If MainForm.Map(TestVarX, TestVarY) = 0 Then
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
        Dim EnvironmentType As Short = MainForm.EnvironmentType
        Dim TheRoomWidth As Short = MainForm.TheRoomWidth
        Dim TheRoomHeight As Short = MainForm.TheRoomHeight
        Dim ColumnsSpace As Short = MainForm.ColumnsSpace
        Dim RowSpace As Short = MainForm.RowSpace
        '
        Dim xish, xishPLUS, yish, yishPLUS As Integer
        Dim RandomNum As New Random
        'Mobile placed reduced the need to check for mobile targeting if nothings on screen, less overhead
        Dim MobilePlaced As Boolean = False
        'lets reduce redundancy
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
                'sets wall and floor art to prevent redundancy later, easier to read
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
                xish = TheRoomWidth * x + ColumnsSpace * x + 10
                yish = TheRoomHeight * y + RowSpace * y + 10
                xishPLUS = Val(TheRoomWidth) * x + Val(ColumnsSpace) * x + 10 + Val(TheRoomWidth)
                yishPLUS = Val(TheRoomHeight) * y + Val(RowSpace) * y + 10 + Val(TheRoomHeight)
                If Math.Abs((PlayerPosX + PlayerposY) - (x + y)) <= 4 And Math.Abs((PlayerPosX - PlayerposY) - (x - y)) <= 4 Or MainForm.AdminVisible = True Then 'admin visible shows all
                    'within range of player, only process isvisible routines if it's within 4 of character so it doesn't process unnecessary squares too far from player
                    If IsVisible(x, y, PlayerPosX, PlayerposY) <= 1 Or IsVisible2(x, y, PlayerPosX, PlayerposY) <= 1 Or MainForm.AdminVisible = True Or ChangedMode = True And MainForm.MapShown(x, y) = True Then
                        'within range of player and is visible
                        If MainForm.MapDrawStatus(x, y) = Shadowed Then
                            MainForm.MapDrawStatusPlus(x, y) = 0
                        End If
                        If MainForm.MapDrawStatusPlus(x, y) = 0 Then
                            MainForm.MapDrawStatusPlus(x, y) += 1
                            'draw wall
                            If MainForm.Map(x, y) = Wall And MainForm.MapDrawStatus(x, y) <> NotHidden Then
                                If GraphicalMode = Tiled Then
                                    MainForm.CANVAS.DrawImage(FilterImageWall(FloorArt, WallArt, MainForm.MapBlur(x, y, 2), MainForm.MapBlur(x, y, 1), MainForm.MapBlur(x, y, 0), MainForm.MapBlur(x, y, 3)), xish, yish, TheRoomWidth, TheRoomHeight)
                                ElseIf GraphicalMode = ASCII Then
                                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                    MainForm.CANVAS.DrawString("#", displayfont, Brushes.LightGray, xish, yish)
                                End If
                                MainForm.MapDrawStatus(x, y) = NotHidden
                                'draw floor
                            ElseIf MainForm.Map(x, y) = Floor And MainForm.MapDrawStatus(x, y) <> NotHidden Then
                                If GraphicalMode = Tiled Then
                                    ShowFog(xish, yish, x, y, FloorArt)
                                ElseIf GraphicalMode = ASCII Then
                                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                    MainForm.CANVAS.DrawString(".", displayfont, Brushes.DarkGray, xish, yish)
                                End If
                                MainForm.MapDrawStatus(x, y) = NotHidden
                                'draw floor with blood
                            ElseIf MainForm.Map(x, y) = SpecialFloor And MainForm.MapDrawStatus(x, y) <> NotHidden Then
                                If GraphicalMode = Tiled Then
                                    ShowFog(xish, yish, x, y, FilterImageRed(FloorArt))
                                ElseIf GraphicalMode = ASCII Then
                                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                    MainForm.CANVAS.DrawString("x", displayfont, Brushes.Red, xish, yish)
                                End If
                                MainForm.MapDrawStatus(x, y) = NotHidden
                                'draw stairs up
                            ElseIf MainForm.Map(x, y) = StairsUp And MainForm.MapDrawStatus(x, y) <> NotHidden Then
                                If GraphicalMode = Tiled Then
                                    MainForm.CANVAS.DrawImage(My.Resources.StairsUp, xish, yish, TheRoomWidth, TheRoomHeight)
                                ElseIf GraphicalMode = ASCII Then
                                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                    MainForm.CANVAS.DrawString("<", displayfont, Brushes.DarkGray, xish, yish)
                                End If
                                MainForm.MapDrawStatus(x, y) = NotHidden
                                'draw stairs down
                            ElseIf MainForm.Map(x, y) = StairsDown And MainForm.MapDrawStatus(x, y) <> NotHidden Then
                                If GraphicalMode = Tiled Then
                                    MainForm.CANVAS.DrawImage(My.Resources.StairsDown, xish, yish, TheRoomWidth, TheRoomHeight)
                                ElseIf GraphicalMode = ASCII Then
                                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                    MainForm.CANVAS.DrawString(">", displayfont, Brushes.DarkGray, xish, yish)
                                End If
                                MainForm.MapDrawStatus(x, y) = NotHidden
                            ElseIf MainForm.Map(x, y) = Lava And MainForm.MapDrawStatus(x, y) <> NotHidden Then
                                If GraphicalMode = Tiled Then
                                    MainForm.CANVAS.DrawImage(FilterImageWall(FloorArt, My.Resources.Lava, True, True, False, False), xish, yish, TheRoomWidth, TheRoomHeight)
                                ElseIf GraphicalMode = ASCII Then
                                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                    MainForm.CANVAS.DrawString("~", displayfont, Brushes.Red, xish, yish)
                                End If
                                MainForm.MapDrawStatus(x, y) = NotHidden
                            ElseIf MainForm.Map(x, y) = Water And MainForm.MapDrawStatus(x, y) <> NotHidden Then
                                If GraphicalMode = Tiled Then
                                    MainForm.CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Water), xish, yish, TheRoomWidth, TheRoomHeight)
                                ElseIf GraphicalMode = ASCII Then
                                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                    MainForm.CANVAS.DrawString("~", displayfont, Brushes.Blue, xish, yish)
                                End If
                                MainForm.MapDrawStatus(x, y) = NotHidden
                            ElseIf MainForm.Map(x, y) = Ice And MainForm.MapDrawStatus(x, y) <> NotHidden Then
                                If GraphicalMode = Tiled Then
                                    MainForm.CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Ice), xish, yish, TheRoomWidth, TheRoomHeight)
                                ElseIf GraphicalMode = ASCII Then
                                    MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                    MainForm.CANVAS.DrawString("#", displayfont, Brushes.Cyan, xish, yish)
                                End If
                                MainForm.MapDrawStatus(x, y) = NotHidden
                            End If
                        End If
                        'if map is occupied, show enemy
                        If MainForm.MapOccupied(x, y) > 0 Then
                            ShowEnemy(MainForm.MapOccupied(x, y), xish, yish, x, y)
                            MobilePlaced = True
                            MainForm.MapDrawStatus(x, y) = Hidden
                            MainForm.MapDrawStatusPlus(x, y) = 0
                        Else
                            'if map isn't occupied by mobiles, is it occupied by items, if so show items (prioritize enemy showing over items)
                            If MainForm.ItemOccupied(x, y) > 0 Then
                                ShowItem(xish, yish, x, y)
                                MainForm.MapDrawStatusPlus(x, y) = 0
                            End If
                            MainForm.MapDrawStatus(x, y) = Hidden
                        End If
                        'player can be shown over items for better visibility
                        If x = PlayerPosX And y = PlayerposY Then
                            MainForm.CANVAS.DrawString("@", displayfont, Brushes.LimeGreen, xish, yish)
                            MainForm.MapDrawStatus(x, y) = Hidden
                            MainForm.MapDrawStatusPlus(x, y) = 0
                        End If
                        MainForm.MapShown(x, y) = True
                    End If
                ElseIf MainForm.MapShown(x, y) = True And MainForm.MapDrawStatus(x, y) <> Shadowed Or ChangedMode = True And MainForm.MapShown(x, y) = True Then
                    'not within the visual sight of the player, but was visited, so should just be fogged
                    Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                    If MainForm.MapDrawStatus(x, y) = 0 Then
                        MainForm.MapDrawStatusPlus(x, y) = 0
                    End If
                    If MainForm.MapDrawStatusPlus(x, y) = 0 Then
                        MainForm.MapDrawStatusPlus(x, y) += 2
                        'draw wall
                        If MainForm.Map(x, y) = Wall And MainForm.MapDrawStatus(x, y) <> Shadowed Then
                            If GraphicalMode = Tiled Then
                                MainForm.CANVAS.DrawImage(FilterImageWall(FloorArt, WallArt, MainForm.MapBlur(x, y, 2), MainForm.MapBlur(x, y, 1), MainForm.MapBlur(x, y, 0), MainForm.MapBlur(x, y, 3)), xish, yish, TheRoomWidth, TheRoomHeight)
                            ElseIf GraphicalMode = ASCII Then
                                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                MainForm.CANVAS.DrawString("#", displayfont, Brushes.DarkGray, xish, yish)
                            End If
                            MainForm.MapDrawStatus(x, y) = Shadowed
                            'draw floor
                        ElseIf MainForm.Map(x, y) = Floor And MainForm.MapDrawStatus(x, y) <> Shadowed Then
                            If GraphicalMode = Tiled Then
                                ShowFog(xish, yish, x, y, FloorArt)
                            ElseIf GraphicalMode = ASCII Then
                                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                MainForm.CANVAS.DrawString(".", displayfont, Brushes.DarkGray, xish, yish)
                            End If
                            MainForm.MapDrawStatus(x, y) = Shadowed
                            'draw floor with blood
                        ElseIf MainForm.Map(x, y) = SpecialFloor And MainForm.MapDrawStatus(x, y) <> Shadowed Then
                            If GraphicalMode = Tiled Then
                                ShowFog(xish, yish, x, y, FilterImageRed(FloorArt))
                            ElseIf GraphicalMode = ASCII Then
                                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                MainForm.CANVAS.DrawString("x", displayfont, Brushes.DarkRed, xish, yish)
                            End If
                            MainForm.MapDrawStatus(x, y) = Shadowed
                            'draw stairs up
                        ElseIf MainForm.Map(x, y) = StairsUp And MainForm.MapDrawStatus(x, y) <> Shadowed Then
                            If GraphicalMode = Tiled Then
                                MainForm.CANVAS.DrawImage(My.Resources.StairsUp, xish, yish, TheRoomWidth, TheRoomHeight)
                            ElseIf GraphicalMode = ASCII Then
                                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                MainForm.CANVAS.DrawString("<", displayfont, Brushes.DarkGray, xish, yish)
                            End If
                            MainForm.MapDrawStatus(x, y) = Shadowed
                            'draw stairs down
                        ElseIf MainForm.Map(x, y) = StairsDown And MainForm.MapDrawStatus(x, y) <> Shadowed Then
                            If GraphicalMode = Tiled Then
                                MainForm.CANVAS.DrawImage(My.Resources.StairsDown, xish, yish, TheRoomWidth, TheRoomHeight)
                            ElseIf GraphicalMode = ASCII Then
                                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                MainForm.CANVAS.DrawString(">", displayfont, Brushes.DarkGray, xish, yish)
                            End If
                            MainForm.MapDrawStatus(x, y) = Shadowed
                        ElseIf MainForm.Map(x, y) = Lava And MainForm.MapDrawStatus(x, y) <> Shadowed Then
                            If GraphicalMode = Tiled Then
                                MainForm.CANVAS.DrawImage(FilterImageWall(FloorArt, My.Resources.Lava, True, True, False, False), xish, yish, TheRoomWidth, TheRoomHeight)
                            ElseIf GraphicalMode = ASCII Then
                                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                MainForm.CANVAS.DrawString("~", displayfont, Brushes.DarkRed, xish, yish)
                            End If
                            MainForm.MapDrawStatus(x, y) = Shadowed
                        ElseIf MainForm.Map(x, y) = Water And MainForm.MapDrawStatus(x, y) <> Shadowed Then
                            If GraphicalMode = Tiled Then
                                MainForm.CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Water), xish, yish, TheRoomWidth, TheRoomHeight)
                            ElseIf GraphicalMode = ASCII Then
                                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                MainForm.CANVAS.DrawString("~", displayfont, Brushes.DarkBlue, xish, yish)
                            End If
                            MainForm.MapDrawStatus(x, y) = Shadowed
                        ElseIf MainForm.Map(x, y) = Ice And MainForm.MapDrawStatus(x, y) <> Shadowed Then
                            If GraphicalMode = Tiled Then
                                MainForm.CANVAS.DrawImage(FilterImageWater(FloorArt, My.Resources.Ice), xish, yish, TheRoomWidth, TheRoomHeight)
                            ElseIf GraphicalMode = ASCII Then
                                MainForm.CANVAS.FillRectangle(Brushes.Black, xish, yish, TheRoomWidth, TheRoomHeight)
                                MainForm.CANVAS.DrawString("#", displayfont, Brushes.DarkCyan, xish, yish)
                            End If
                            MainForm.MapDrawStatus(x, y) = Shadowed
                        End If
                        'shadow the area
                        MainForm.MapShown(x, y) = True
                        MainForm.CANVAS.FillRectangle(semiTransBrush, xish, yish, TheRoomWidth, TheRoomHeight)
                    End If
                End If
            Next
        Next
        If MobilePlaced = True Then
            MainForm.MobilePresent = True
            SortEnemyRangeList() 'puts the closest enemy on 0
        Else
            MainForm.MobilePresent=false
        End If
        MainForm.CreateGraphics.DrawImage(MainForm.PAD, 0, 0)
    End Sub
    Sub SortEnemyRangeList()
        Dim MobileNumber As Short = 0
        MainForm.MobileVisible(0, MobType) = 0 'Clear the first targetable option before sort
        For MobileNumber = 0 To 9 Step 1
            'if the mobile is within range of player or current target is dead (Prioritizes next option before declaring a target fault)
            If Math.Abs(MainForm.MobileVisible(MobileNumber, MobXPosition) - MainForm.PlayerPosX) + Math.Abs(MainForm.MobileVisible(MobileNumber, MobYPosition) - MainForm.PlayerPosY) < Math.Abs(MainForm.MobileVisible(0, MobXPosition) - MainForm.PlayerPosX) + Math.Abs(MainForm.MobileVisible(0, MobYPosition) - MainForm.PlayerPosY) And MainForm.MobileVisible(MobileNumber, MobType) Or MainForm.MobileHealth(MainForm.MobileVisible(0, MobNumber)) <= 0 Then
                If MainForm.MobileHealth(MainForm.MobileVisible(MobileNumber, MobNumber)) > 0 Then 'ensures that the new target mob is alive! lol, no need
                    MainForm.MobileVisible(0, MobXPosition) = MainForm.MobileVisible(MobileNumber, MobXPosition)
                    MainForm.MobileVisible(0, MobYPosition) = MainForm.MobileVisible(MobileNumber, MobYPosition)
                    MainForm.MobileVisible(0, MobNumber) = MainForm.MobileVisible(MobileNumber, MobNumber)
                    MainForm.MobileVisible(MobileNumber, MobType) = 0 'shows that the mobile was processed, basically clears it for future processes
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
        MainForm.MobileVisible(EnemyNum, MobXPosition) = x
        MainForm.MobileVisible(EnemyNum, MobYPosition) = y
        MainForm.MobileVisible(EnemyNum, MobType) = EnemyNum
        MainForm.MobileVisible(EnemyNum, MobNumber) = MainForm.MobOccupied(x, y)
    End Sub
    Sub TargetEnemy()
        'to ensure all enemies within target list are still viable and alive, sort through list and null else
        SortEnemyRangeList()
        '
        Dim Mapsize As Short = MainForm.MapSize
        Dim PlayerPosX As Short = MainForm.PlayerPosX
        Dim PlayerposY As Short = MainForm.PlayerPosY
        Dim EnvironmentType As Short = MainForm.EnvironmentType
        Dim TheRoomWidth As Short = MainForm.TheRoomWidth
        Dim TheRoomHeight As Short = MainForm.TheRoomHeight
        Dim ColumnsSpace As Short = MainForm.ColumnsSpace
        Dim RowSpace As Short = MainForm.RowSpace
        Dim x As Short = MainForm.MobileVisible(0, MobXPosition)
        Dim y As Short = MainForm.MobileVisible(0, MobYPosition)
        Dim xish As Short = TheRoomWidth * x + ColumnsSpace * x + 10
        Dim yish As Short = TheRoomHeight * y + RowSpace * y + 10
        '
        MainForm.CANVAS.DrawRectangle(Pens.IndianRed, xish, yish, TheRoomWidth - 2, TheRoomHeight - 2)
        MainForm.CreateGraphics.DrawImage(MainForm.PAD, 0, 0)
    End Sub
    Sub ShowItem(ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short)
        If MainForm.ItemType(MainForm.ItemOccupied(x, y)) = Gold Then
            MainForm.CANVAS.DrawString("g", displayfont, Brushes.Yellow, xish, yish)
        ElseIf MainForm.ItemType(MainForm.ItemOccupied(x, y)) = TheEverspark Then
            MainForm.CANVAS.DrawString("E", displayfont, Brushes.White, xish, yish)
        ElseIf MainForm.ItemType(MainForm.ItemOccupied(x, y)) = Weapon Then
            MainForm.CANVAS.DrawString("g", displayfont, Brushes.DarkCyan, xish, yish)
        Else
            MainForm.CANVAS.DrawString(MainForm.ItemShowType(x, y), displayfont, Brushes.Green, xish, yish)
        End If
    End Sub
    Sub ShowFog(ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short, ByVal FloorArt As Image)
        'Here are variables that are passed from the mainform, re-stated here to distinguish easier which are passed and allow
        'better mobility for future changes.
        '
        Dim TheRoomWidth As Short = MainForm.TheRoomWidth
        Dim TheRoomHeight As Short = MainForm.TheRoomHeight
        '
        MainForm.CANVAS.DrawImage(FloorArt, xish, yish, TheRoomWidth, TheRoomHeight)
        If MainForm.FogMap(x, y) = 1 Then
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 10), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf MainForm.FogMap(x, y) = 2 Then
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 20), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf MainForm.FogMap(x, y) = 3 Then
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 30), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf MainForm.FogMap(x, y) = 4 Then
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 40), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf MainForm.FogMap(x, y) = 5 Then
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 50), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf MainForm.FogMap(x, y) = 6 Then
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 60), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf MainForm.FogMap(x, y) = 7 Then
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 70), xish, yish, TheRoomWidth, TheRoomHeight)
        ElseIf MainForm.FogMap(x, y) = 8 Then
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 80), xish, yish, TheRoomWidth, TheRoomHeight)
        Else
            MainForm.CANVAS.DrawImage(FilterImageFog(FloorArt, 0), xish, yish, TheRoomWidth, TheRoomHeight)
        End If
    End Sub
End Module
