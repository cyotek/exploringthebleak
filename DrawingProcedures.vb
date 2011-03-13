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
    Public displayfont As New Font("Times New Roman", 24)
    Public displayfont2 As New Font("Lucida Sans Unicode", 12)
    Public ChangedMode As Boolean = False
    Public LOSMap(MainForm.MapSize, MainForm.MapSize) As Short
    Public PrevPlayerPosX(3), PrevPlayerPosY(3) As Integer
    Private CurrentlyDisplayedMobile As Byte
    Private FloorGraphic(3) As Image
    Private WallGraphic As Image
    Private LineGraphic As Image
    Private DrawnGraphics(TotalEnvironmentTypes, 4) As Boolean '4 is wall
#End Region
    ' Copies a part of the bitmap.
    Function CopyBitmap(ByVal source As Bitmap, ByVal part As Rectangle) As Bitmap
        Dim bmp As New Bitmap(part.Width, part.Height)

        Dim g As Graphics = Graphics.FromImage(bmp)
        g.DrawImage(source, 0, 0, part, GraphicsUnit.Pixel)
        g.Dispose()

        Return bmp
    End Function
#Region "Player FoV / LoS"
    Function IsDiagonal(ByVal x As Short, ByVal y As Short, ByVal playerposx As Short, ByVal playerposy As Short) As Boolean
        Dim IsDiagonalTest As Boolean
        'if this happens, that means that diagonally from bottom left to top right is where the sector is.
        'example:
        '23456
        '34567
        '45678 <-- middle 6 is player
        '56789
        '67890
        If playerposx + playerposy = x + y Then
            IsDiagonalTest = True
        End If
        'if this happens, that means that diagonally from bottom right to top left is where the sector is.
        'example:
        '65432
        '76543
        '87654 <-- middle 6 is player
        '98765
        '09876
        If playerposx + playerposy = 2 * playerposx - x + y Then
            IsDiagonalTest = True
        End If
        'it wasn't successfully read as diagonal
        If IsDiagonalTest = False Then
            Return False
        Else
            Dim CurX, CurStep As Integer
            If x > playerposx Then
                If y > playerposy Then 'bottom right
                    For CurX = playerposx + 1 To x Step 1
                        CurStep += 1
                        If MainForm.Map(MainForm.MapLevel, CurX, playerposy + CurStep) = Wall Then
                            If CurX = x And playerposy + CurStep = y Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    Next
                Else 'top right
                    For CurX = playerposx + 1 To x Step 1
                        CurStep -= 1
                        If MainForm.Map(MainForm.MapLevel, CurX, playerposy + CurStep) = Wall Then
                            If CurX = x And playerposy + CurStep = y Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    Next
                End If
            Else
                If y > playerposy Then 'bottom left
                    For CurX = playerposx - 1 To x Step -1
                        CurStep += 1
                        If MainForm.Map(MainForm.MapLevel, CurX, playerposy + CurStep) = Wall Then
                            If CurX = x And playerposy + CurStep = y Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    Next
                Else 'top left
                    For CurX = playerposx - 1 To x Step -1
                        CurStep -= 1
                        If MainForm.Map(MainForm.MapLevel, CurX, playerposy + CurStep) = Wall Then
                            If CurX = x And playerposy + CurStep = y Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    Next
                End If
            End If
            Return True
        End If
    End Function
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
        Dim StartX As Short = PlayerPosX - 4 - MainForm.PlayersPlusRange : If StartX < 0 Then StartX = 0
        Dim StartY As Short = PlayerposY - 4 - MainForm.PlayersPlusRange : If StartY < 0 Then StartY = 0
        Dim FinishX As Short = PlayerPosX + 4 + MainForm.PlayersPlusRange : If FinishX > Mapsize Then FinishX = Mapsize
        Dim FinishY As Short = PlayerposY + 4 + MainForm.PlayersPlusRange : If FinishY > Mapsize Then FinishY = Mapsize
        If ChangedMode = True Then 'required to redraw entire map, used when grahpical mode is changed
            StartX = 0 : StartY = 0 : FinishX = Mapsize : FinishY = Mapsize
        End If
        'start at top left and go to bottom right
        For x = StartX To FinishX Step 1
            For y = StartY To FinishY Step 1
                'sets wall and floor art dependant on environment type which is dependant on current depth or dungeon level
                'xish and yish is the x and y location on the screen that the tile will be placed upon
                xish = TheRoomWidth * x + ColumnsSpace * x + 1
                yish = TheRoomHeight * y + RowSpace * y + 25
                xishPLUS = Val(TheRoomWidth) * x + Val(ColumnsSpace) * x + 10 + Val(TheRoomWidth)
                yishPLUS = Val(TheRoomHeight) * y + Val(RowSpace) * y + 10 + Val(TheRoomHeight)
                'MainForm.CANVAS.DrawRectangle(Pens.Pink, xish, yish, TheRoomWidth, TheRoomHeight) 'used to test canvas size
                'draws a circle around player 4 wide on each side for visibility, remember that x and y are passed starting -5 to 5 of characters current position.
                If ((Math.Pow(PlayerPosX - x, 2) + Math.Pow(PlayerposY - y, 2)) < (Math.Pow(4 + MainForm.PlayersPlusRange, 2))) Or MainForm.AdminVisible = True Then 'admin visible shows all
                    'within range of player, only process isvisible routines if it's within 4(+playersplusrange) of character so it doesn't process unnecessary squares too far from player
                    If IsVisible(x, y, PlayerPosX, PlayerposY) <= 1 Or IsVisible2(x, y, PlayerPosX, PlayerposY) <= 1 Or MainForm.AdminVisible = True Or ChangedMode = True And MainForm.MapShown(MainForm.MapLevel, x, y) = True Or IsDiagonal(x, y, PlayerPosX, PlayerposY) = True Then
                        'within range of player and is visible
                        If LOSMap(x, y) <> Visible Then 'should be visible, tile not currently visible, change then set map as visible
                            LOSMap(x, y) = Visible
                            'draw wall
                            DrawTile(x, y, xish, yish, TheRoomWidth, TheRoomHeight, GraphicalMode, WallArt, FloorArt)
                        End If
                        'if map is occupied, show enemy
                        If MainForm.MapOccupied(MainForm.MapLevel, x, y) > 0 Then
                            If x = PlayerPosX And y = PlayerposY Then
                                'this is the screensaver person
                            Else
                                LOSMap(x, y) = Redraw
                                MobilePlaced = True
                                ShowEnemy(MainForm.MapOccupied(MainForm.MapLevel, x, y), xish, yish, x, y)
                                CurrentlyDisplayedMobile += 1
                            End If
                        Else
                            'if map isn't occupied by mobiles, is it occupied by items, if so show items (prioritize enemy showing over items)
                            If MainForm.ItemOccupied(MainForm.MapLevel, x, y) > 0 Then
                                LOSMap(x, y) = Redraw
                                ShowItem(xish, yish, x, y)
                            End If
                        End If
                        'player can be shown over items for better visibility
                        If x = PlayerPosX And y = PlayerposY And MainForm.Screensaver = False Then
                            LOSMap(x, y) = Redraw
                            MainForm.CANVAS.DrawString("@", displayfont, Brushes.LimeGreen, xish, yish)
                        ElseIf x = PlayerPosX And y = PlayerposY And MainForm.Screensaver = True Then
                            LOSMap(x, y) = Redraw
                            MainForm.CANVAS.DrawString("@", displayfont, Brushes.Red, xish, yish)
                        ElseIf x = PrevPlayerPosX(0) And y = PrevPlayerPosY(0) And MainForm.Screensaver = False Then
                            LOSMap(x, y) = Redraw
                            MainForm.CANVAS.DrawString("@", New Font("Arial", displayfont.Size / 3 * 2.2), Brushes.Yellow, xish, yish)
                            MainForm.CANVAS.DrawString("T", New Font("Arial", displayfont.Size / 3), Brushes.White, xish + TheRoomWidth / 3 * 2, yish + TheRoomHeight / 3 * 2)
                        ElseIf x = PrevPlayerPosX(1) And y = PrevPlayerPosY(1) And MainForm.Screensaver = False Then
                            LOSMap(x, y) = Redraw
                            MainForm.CANVAS.DrawString("@", New Font("Arial", displayfont.Size / 3 * 2.2), Brushes.Yellow, xish, yish)
                            MainForm.CANVAS.DrawString("S", New Font("Arial", displayfont.Size / 3), Brushes.White, xish + TheRoomWidth / 3 * 2, yish + TheRoomHeight / 3 * 2)
                        ElseIf x = PrevPlayerPosX(2) And y = PrevPlayerPosY(2) And MainForm.Screensaver = False Then
                            LOSMap(x, y) = Redraw
                            MainForm.CANVAS.DrawString("@", New Font("Arial", displayfont.Size / 3 * 2.2), Brushes.Yellow, xish, yish)
                            MainForm.CANVAS.DrawString("B", New Font("Arial", displayfont.Size / 3), Brushes.White, xish + TheRoomWidth / 3 * 2, yish + TheRoomHeight / 3 * 2)
                        ElseIf x = PrevPlayerPosX(3) And y = PrevPlayerPosY(3) And MainForm.Screensaver = False Then
                            LOSMap(x, y) = Redraw
                            MainForm.CANVAS.DrawString("@", New Font("Arial", displayfont.Size / 3 * 2.2), Brushes.Yellow, xish, yish)
                            MainForm.CANVAS.DrawString("M", New Font("Arial", displayfont.Size / 3), Brushes.White, xish + TheRoomWidth / 3 * 2, yish + TheRoomHeight / 3 * 2)
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
            Try
                SortEnemyRangeList() 'puts the closest enemy on 0
            Catch
            End Try
        Else
            MainForm.MobilePresent = False
        End If
        MainForm.CreateGraphics.DrawImage(MainForm.PAD, 0, 0)
    End Sub
    Sub DrawTile(ByVal x As Short, ByVal y As Short, ByVal xish As Short, ByVal yish As Short, ByVal TheRoomWidth As Short, ByVal TheRoomHeight As Short, ByVal GraphicalMode As Boolean, ByVal wallart As Bitmap, ByVal floorart As Bitmap)
        Dim BGColor As Brush
        If MainForm.EnvironmentType = 1 Then
            BGColor = Brushes.BurlyWood
        ElseIf MainForm.EnvironmentType = 2 Then
            BGColor = Brushes.Chartreuse
        ElseIf MainForm.EnvironmentType = 3 Then
            BGColor = Brushes.DarkRed
        ElseIf MainForm.EnvironmentType = 4 Then
            BGColor = Brushes.Cornsilk
        ElseIf MainForm.EnvironmentType = 5 Then
            BGColor = Brushes.DarkKhaki
        ElseIf MainForm.EnvironmentType = 6 Then
            BGColor = Brushes.DarkOrange
        ElseIf MainForm.EnvironmentType = 7 Then
            BGColor = Brushes.DarkGreen
        ElseIf MainForm.EnvironmentType = 8 Then
            BGColor = Brushes.DarkViolet
        ElseIf MainForm.EnvironmentType = 9 Then
            BGColor = Brushes.DeepPink
        Else
            BGColor = New SolidBrush(Color.FromArgb(35, 0, 0))
        End If
        If MainForm.Map(MainForm.MapLevel, x, y) = Wall Then
            MainForm.CANVAS.FillRectangle(Brushes.DarkGray, xish, yish, TheRoomWidth, TheRoomHeight)
            MainForm.CANVAS.DrawString("#", displayfont, Brushes.Black, xish, yish)
            'draw floor
        ElseIf MainForm.Map(MainForm.MapLevel, x, y) = Floor Then
            MainForm.CANVAS.FillRectangle(BGColor, xish, yish, TheRoomWidth, TheRoomHeight)
            MainForm.CANVAS.DrawString(".", displayfont, Brushes.DarkGray, xish, yish)
            'draw floor with blood
        ElseIf MainForm.Map(MainForm.MapLevel, x, y) = SpecialFloor Then
            MainForm.CANVAS.FillRectangle(BGColor, xish, yish, TheRoomWidth, TheRoomHeight)
            MainForm.CANVAS.DrawString("x", displayfont, Brushes.DarkRed, xish, yish)
            'draw stairs up
        ElseIf MainForm.Map(MainForm.MapLevel, x, y) = StairsUp Then
            MainForm.CANVAS.FillRectangle(BGColor, xish, yish, TheRoomWidth, TheRoomHeight)
            MainForm.CANVAS.DrawString("<", displayfont, Brushes.DarkGray, xish, yish)
            'draw stairs down
        ElseIf MainForm.Map(MainForm.MapLevel, x, y) = StairsDown Then
            MainForm.CANVAS.FillRectangle(BGColor, xish, yish, TheRoomWidth, TheRoomHeight)
            MainForm.CANVAS.DrawString(">", displayfont, Brushes.DarkGray, xish, yish)
        ElseIf MainForm.Map(MainForm.MapLevel, x, y) = Water Then
            If MainForm.RiverType = Water Then
                MainForm.CANVAS.FillRectangle(New SolidBrush(Color.FromArgb(150, 0, 0)), xish, yish, TheRoomWidth, TheRoomHeight)
                MainForm.CANVAS.DrawString("~", displayfont, Brushes.Red, xish, yish)
            ElseIf MainForm.RiverType = Lava Then
                MainForm.CANVAS.FillRectangle(New SolidBrush(Color.FromArgb(175, 0, 0)), xish, yish, TheRoomWidth, TheRoomHeight)
                MainForm.CANVAS.DrawString("~", displayfont, Brushes.Yellow, xish, yish)
            ElseIf MainForm.RiverType = Ice Then
                MainForm.CANVAS.FillRectangle(New SolidBrush(Color.FromArgb(175, 0, 0)), xish, yish, TheRoomWidth, TheRoomHeight)
                MainForm.CANVAS.DrawString("~", displayfont, Brushes.Black, xish, yish)
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
            MainForm.CANVAS.DrawString("Ѫ", displayfont, Brushes.Red, xish, yish) 'rat
        ElseIf EnemyNum = 2 Then
            MainForm.CANVAS.DrawString("ѩ", displayfont, Brushes.Red, xish, yish) 'bat
        ElseIf EnemyNum = 3 Then
            MainForm.CANVAS.DrawString("ѧ", displayfont, Brushes.Red, xish, yish) 'imp
        ElseIf EnemyNum = 4 Then
            MainForm.CANVAS.DrawString("җ", displayfont, Brushes.Red, xish, yish) 'goblin
        ElseIf EnemyNum = 5 Then
            MainForm.CANVAS.DrawString("ҕ", displayfont, Brushes.Red, xish, yish) 'troll
        ElseIf EnemyNum = 6 Then
            MainForm.CANVAS.DrawString("ҹ", displayfont, Brushes.Red, xish, yish) 'ogre
        ElseIf EnemyNum = 7 Then
            MainForm.CANVAS.DrawString("ҙ", displayfont, Brushes.Red, xish, yish) 'catoblepas
        ElseIf EnemyNum = 8 Then
            MainForm.CANVAS.DrawString("Ӄ", displayfont, Brushes.Red, xish, yish) 'parandrus
        ElseIf EnemyNum = 9 Then
            MainForm.CANVAS.DrawString("Җ", displayfont, Brushes.Red, xish, yish) 'Clurichuan
        ElseIf EnemyNum = 10 Then
            MainForm.CANVAS.DrawString("Ҿ", displayfont, Brushes.Red, xish, yish) 'Dullahan
        ElseIf EnemyNum = 11 Then
            MainForm.CANVAS.DrawString("Ҩ", displayfont, Brushes.Red, xish, yish) 'Golem
        ElseIf EnemyNum = 12 Then
            MainForm.CANVAS.DrawString("Ҵ", displayfont, Brushes.Red, xish, yish) 'sceadugengan
        ElseIf EnemyNum = 13 Then
            MainForm.CANVAS.DrawString("Ѡ", displayfont, Brushes.Red, xish, yish) 'Schilla
        End If
        'since map contains an enemy, throw that enemy and their details into a list that can be back-traced
        'this list is used for ranged weapons.
        Try
            MainForm.MobileVisible(MainForm.MapLevel, CurrentlyDisplayedMobile, MobXPosition) = x
            MainForm.MobileVisible(MainForm.MapLevel, CurrentlyDisplayedMobile, MobYPosition) = y
            MainForm.MobileVisible(MainForm.MapLevel, CurrentlyDisplayedMobile, MobType) = EnemyNum
            MainForm.MobileVisible(MainForm.MapLevel, CurrentlyDisplayedMobile, MobNumber) = MainForm.MobOccupied(MainForm.MapLevel, x, y)
        Catch
        End Try
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
        Dim x As Short = MainForm.MobileVisible(MainForm.MapLevel, 0, MobXPosition) 'current mobile x position
        Dim y As Short = MainForm.MobileVisible(MainForm.MapLevel, 0, MobYPosition) 'current mobile y position
        Dim xish As Short = TheRoomWidth * x + ColumnsSpace * x + 1 'current mobile sector x position
        Dim yish As Short = TheRoomHeight * y + RowSpace * y + 25 'current mobile sector y position
        Dim pxish As Short = TheRoomWidth * PlayerPosX + ColumnsSpace * PlayerPosX + 1 'current player sector x position
        Dim pyish As Short = TheRoomHeight * PlayerposY + ColumnsSpace * PlayerposY + 25 'current player sector y position
        Dim halfroom As Short = TheRoomWidth / 2 'half of the room in pixels
        Dim CurX, TarX, CurY, TarY As Integer 'make sure that the rectangle starts from the lowest x and ends at the largest x, likewise with y variables
        Dim CurAmtX, CurAmtY As Integer 'details whether the line will be on the right or left side of the square, 
        If pxish >= xish Then : CurX = xish : TarX = pxish : CurAmtX = TheRoomWidth - 2 : Else : CurX = pxish : TarX = xish : CurAmtX = 0 : End If
        If PlayerPosX = x Then CurAmtX = halfroom
        If pyish >= yish Then : CurY = yish : TarY = pyish : CurAmtY = TheRoomHeight - 2 : Else : CurY = pyish : TarY = yish : CurAmtY = 0 : End If
        If PlayerposY = y Then CurAmtY = halfroom
        Dim Fromrectangle As New Rectangle(CurX, CurY, TarX, TarY) 'copy the original image before drawing on that image so it can be restored after finished.
        If clear = False Then
            LineGraphic = CopyBitmap(MainForm.PAD, Fromrectangle)
            MainForm.CANVAS.DrawRectangle(Pens.IndianRed, xish, yish, TheRoomWidth - 2, TheRoomHeight - 2)
            MainForm.CANVAS.DrawLine(Pens.IndianRed, pxish + halfroom, pyish + halfroom, xish + CurAmtX, yish + CurAmtY)
        ElseIf clear = True Then
            MainForm.CANVAS.DrawImage(LineGraphic, Fromrectangle) 'this paints the original image before the drawing of the line and box as it is to be cleared.
        End If
        MainForm.CreateGraphics.DrawImage(MainForm.PAD, 0, 0)
    End Sub
    Sub ShowItem(ByVal xish As Short, ByVal yish As Short, ByVal x As Short, ByVal y As Short)
        If MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = Gold Then
            MainForm.CANVAS.DrawString(MainForm.ItemShowType(MainForm.MapLevel, x, y), displayfont, Brushes.Yellow, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = TheEverspark Then
            MainForm.CANVAS.DrawString("E", displayfont, Brushes.White, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = Weapon Then
            MainForm.CANVAS.DrawString(MainForm.ItemShowType(MainForm.MapLevel, x, y), displayfont, Brushes.DarkCyan, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = GenerateItem.Food Then
            MainForm.CANVAS.DrawString(MainForm.ItemShowType(MainForm.MapLevel, x, y), displayfont, Brushes.LightYellow, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = GenerateItem.Water Then
            MainForm.CANVAS.DrawString(MainForm.ItemShowType(MainForm.MapLevel, x, y), displayfont, Brushes.Aqua, xish, yish)
        ElseIf MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, x, y)) = GenerateItem.Potion Then
            MainForm.CANVAS.DrawString(MainForm.ItemShowType(MainForm.MapLevel, x, y), displayfont, Brushes.Magenta, xish, yish)
        Else
            MainForm.CANVAS.DrawString(MainForm.ItemShowType(MainForm.MapLevel, x, y), displayfont, Brushes.Green, xish, yish)
        End If
    End Sub
End Module
