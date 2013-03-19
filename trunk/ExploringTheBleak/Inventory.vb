Public Class Inventory
#Region "Constants"
    Const Head As Short = 1
    Const Chest As Short = 2
    Const Arms As Short = 3
    Const Hands As Short = 4
    Const Legs As Short = 5
    Const Feet As Short = 6
    Const Weapon As Short = 7
    Const Gold As Short = 8
    Const TheEverspark As Short = 50
#End Region
    Function AddToInventory(ByVal VNUM As Short)
        Dim TestRoom As Short = 0
        Dim FoundRoom As Boolean = False
        If MainForm.ItemType(MainForm.MapLevel, VNUM) = TheEverspark Then 'player picked up the everspark, thus ending the game
            MainForm.Comment11.Visible = True
            MainForm.PlayerDead = True
            FoundRoom = True
        ElseIf MainForm.ItemType(MainForm.MapLevel, VNUM) = Gold Then 'set foundroom to true so gold isn't added the inventory instead of the total
            Dim GoldAmount As New Random
            Dim GoldAmt As Integer = GoldAmount.Next(1, 9)
            FoundRoom = True
            MainForm.PlayerGold += GoldAmt * MainForm.MapLevel
            MainForm.ItemNameType(MainForm.MapLevel, MainForm.PlayerPosX, MainForm.PlayerPosY) = LTrim(Str(GoldAmt * MainForm.MapLevel)) + " gold"
        End If
        If FoundRoom = False Then
            For TestRoom = 0 To 19 Step 1
                If MainForm.ItemInventoryType(TestRoom) = 0 Then
                    MainForm.ItemInventoryType(TestRoom) = MainForm.ItemType(MainForm.MapLevel, VNUM)
                    MainForm.ItemInventoryName(TestRoom) = MainForm.ItemNameType(MainForm.MapLevel, MainForm.PlayerPosX, MainForm.PlayerPosY)
                    '75%chance for no +, 25% to add +1 each round, but can't exceed level number in +
                    Dim FoundQuality As Boolean = False
                    Dim QualityRandom As New Random
                    Dim CurQuality As Integer = 0
                    Dim CurPercent As Short
                    For qualityrun = 0 To MainForm.MapLevel Step 1
                        CurPercent = QualityRandom.Next(1, 101)
                        If CurPercent <= 25 Then '25% chance to not add a +1 each whil
                            CurQuality -= 1
                        Else
                            CurQuality += 1
                        End If
                    Next
                    If CurQuality < 0 Then CurQuality = 0
                    MainForm.ItemInventoryQuality(TestRoom) = CurQuality
                    FoundRoom = True
                    Exit For
                End If
            Next
        End If
        If FoundRoom = False Then
            MainForm.SND("You don't have enough room.")
        Else
            MainForm.SND("You pick up " + MainForm.ItemNameType(MainForm.MapLevel, MainForm.PlayerPosX, MainForm.PlayerPosY) + ".")
            MainForm.ItemType(MainForm.MapLevel, MainForm.ItemOccupied(MainForm.MapLevel, MainForm.PlayerPosX, MainForm.PlayerPosY)) = 0
            MainForm.ItemOccupied(MainForm.MapLevel, MainForm.PlayerPosX, MainForm.PlayerPosY) = 0
            If MainForm.PlayerDead = True Then
                MainForm.SND("Game Over.")
                MainForm.SNDScores()
                MainForm.HighScores = MainForm.Output.Text
                MainForm.SaveTextToFile(MainForm.HighScores, CurDir() + "\HighScores.TG")
                Me.Text += " [Game Over] : Press f1 to view scores list"
            End If
        End If
        ResetQuickInventory()
        Return 0
    End Function
    Function ResetQuickInventory()
        Dim InventoryItem As Short
        Dim FoundSomething As Boolean
        MainForm.QuickInventory.Text = ""
        For InventoryItem = 0 To 9 Step 1
            If MainForm.QuickInventory.Text = "" And MainForm.ItemInventoryName(InventoryItem) <> "" Then
                MainForm.QuickInventory.Text = LTrim(Str(InventoryItem + 1)) + ". " + MainForm.ItemInventoryName(InventoryItem)
                FoundSomething = True
            ElseIf InventoryItem < 10 And MainForm.ItemInventoryName(InventoryItem) <> "" Then
                MainForm.QuickInventory.Text += " " + LTrim(Str(InventoryItem + 1)) + ". " + MainForm.ItemInventoryName(InventoryItem)
                FoundSomething = True
            ElseIf InventoryItem = 9 And FoundSomething = False Then
                MainForm.QuickInventory.Text = "Empty Inventory"
            End If
        Next
        Return 0
    End Function
    Function Processwear(ByVal Control As Short)
        Dim PreviousItemName As String = ""
        Dim PreviousItemType As Short = 0
        Dim PreviousItemQuality As Short = 0
        If MainForm.ItemInventoryType(Control - 1) = Head Then 'head armor
            PreviousItemName = MainForm.PlayerEquipNHead
            PreviousItemType = MainForm.PlayerEquipHead
            PreviousItemQuality = MainForm.PlayerEquipQHead
            If PreviousItemName <> "" Then 'you had something previously equiped, detail what it was
                MainForm.SND("You remove " + PreviousItemName + "+" + LTrim(Str(PreviousItemQuality)) + ".")
            End If
            HeadEquip.Text = MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(Control - 1) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))))  'head armor
            MainForm.SND("You equip " + MainForm.ItemInventoryName(Control - 1) + "+" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))) + ".")
            MainForm.PlayerEquipNHead = MainForm.ItemInventoryName(Control - 1)
            MainForm.PlayerEquipHead = Head
            MainForm.PlayerEquipQHead = MainForm.ItemInventoryQuality(Control - 1)
            MainForm.PlayerDefense = MainForm.PlayerDefense + MainForm.PlayerEquipQHead - PreviousItemQuality
        ElseIf MainForm.ItemInventoryType(Control - 1) = Chest Then 'chest armor
            PreviousItemName = MainForm.PlayerEquipNChest
            PreviousItemType = MainForm.PlayerEquipChest
            PreviousItemQuality = MainForm.PlayerEquipQChest
            If PreviousItemName <> "" Then 'you had something previously equiped, detail what it was
                MainForm.SND("You remove " + PreviousItemName + "+" + LTrim(Str(PreviousItemQuality)) + ".")
            End If
            ChestEquip.Text = MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(Control - 1) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))))  'you wield swords in your hand
            MainForm.SND("You equip " + MainForm.ItemInventoryName(Control - 1) + "+" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))) + ".")
            MainForm.PlayerEquipNChest = MainForm.ItemInventoryName(Control - 1)
            MainForm.PlayerEquipChest = Chest
            MainForm.PlayerEquipQChest = MainForm.ItemInventoryQuality(Control - 1)
            MainForm.PlayerDefense = MainForm.PlayerDefense + MainForm.PlayerEquipQChest - PreviousItemQuality
        ElseIf MainForm.ItemInventoryType(Control - 1) = Arms Then 'arms armor
            PreviousItemName = MainForm.PlayerEquipNArms
            PreviousItemType = MainForm.PlayerEquipArms
            PreviousItemQuality = MainForm.PlayerEquipQArms
            If PreviousItemName <> "" Then 'you had something previously equiped, detail what it was
                MainForm.SND("You remove " + PreviousItemName + "+" + LTrim(Str(PreviousItemQuality)) + ".")
            End If
            ArmsEquip.Text = MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(Control - 1) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1)))) 'you wield daggers in your hand
            MainForm.SND("You equip " + MainForm.ItemInventoryName(Control - 1) + "+" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))) + ".")
            MainForm.PlayerEquipNArms = MainForm.ItemInventoryName(Control - 1)
            MainForm.PlayerEquipArms = Arms
            MainForm.PlayerEquipQArms = MainForm.ItemInventoryQuality(Control - 1)
            MainForm.PlayerDefense = MainForm.PlayerDefense + MainForm.PlayerEquipQArms - PreviousItemQuality
        ElseIf MainForm.ItemInventoryType(Control - 1) = Hands Then
            PreviousItemName = MainForm.PlayerEquipNHands
            PreviousItemType = MainForm.PlayerEquipHands
            PreviousItemQuality = MainForm.PlayerEquipQHands
            If PreviousItemName <> "" Then 'you had something previously equiped, detail what it was
                MainForm.SND("You remove " + PreviousItemName + "+" + LTrim(Str(PreviousItemQuality)) + ".")
            End If
            HandsEquip.Text = MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(Control - 1) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1)))) 'you wear helmets on your head
            MainForm.SND("You equip " + MainForm.ItemInventoryName(Control - 1) + "+" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))) + ".")
            MainForm.PlayerEquipNHands = MainForm.ItemInventoryName(Control - 1)
            MainForm.PlayerEquipHands = Hands
            MainForm.PlayerEquipQHands = MainForm.ItemInventoryQuality(Control - 1)
            MainForm.PlayerDefense = MainForm.PlayerDefense + MainForm.PlayerEquipQHands - PreviousItemQuality
        ElseIf MainForm.ItemInventoryType(Control - 1) = Legs Then
            PreviousItemName = MainForm.PLayerEquipNLegs
            PreviousItemType = MainForm.PlayerEquipLegs
            PreviousItemQuality = MainForm.PLayerEquipQLegs
            If PreviousItemName <> "" Then 'you had something previously equiped, detail what it was
                MainForm.SND("You remove " + PreviousItemName + "+" + LTrim(Str(PreviousItemQuality)) + ".")
            End If
            LegsEquip.Text = MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(Control - 1) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1)))) 'you wear helmets on your head
            MainForm.SND("You equip " + MainForm.ItemInventoryName(Control - 1) + "+" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))) + ".")
            MainForm.PLayerEquipNLegs = MainForm.ItemInventoryName(Control - 1)
            MainForm.PlayerEquipLegs = Legs
            MainForm.PLayerEquipQLegs = MainForm.ItemInventoryQuality(Control - 1)
            MainForm.PlayerDefense = MainForm.PlayerDefense + MainForm.PLayerEquipQLegs - PreviousItemQuality
        ElseIf MainForm.ItemInventoryType(Control - 1) = feet Then
            PreviousItemName = MainForm.PlayerEquipNFeet
            PreviousItemType = MainForm.PlayerEquipFeet
            PreviousItemQuality = MainForm.PlayerEquipQFeet
            If PreviousItemName <> "" Then 'you had something previously equiped, detail what it was
                MainForm.SND("You remove " + PreviousItemName + "+" + LTrim(Str(PreviousItemQuality)) + ".")
            End If
            BootsEquip.Text = MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(Control - 1) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1)))) 'you wear helmets on your head
            MainForm.SND("You equip " + MainForm.ItemInventoryName(Control - 1) + "+" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))) + ".")
            MainForm.PlayerEquipNFeet = MainForm.ItemInventoryName(Control - 1)
            MainForm.PlayerEquipFeet = Feet
            MainForm.PlayerEquipQFeet = MainForm.ItemInventoryQuality(Control - 1)
            MainForm.PlayerDefense = MainForm.PlayerDefense + MainForm.PlayerEquipQFeet - PreviousItemQuality
        ElseIf MainForm.ItemInventoryType(Control - 1) = Weapon Then
            PreviousItemName = MainForm.PlayerEquipNHands
            PreviousItemType = MainForm.PlayerEquipHands
            PreviousItemQuality = MainForm.PlayerEquipQHands
            If PreviousItemName <> "" Then 'you had something previously equiped, detail what it was
                MainForm.SND("You remove " + PreviousItemName + "+" + LTrim(Str(PreviousItemQuality)) + ".")
            End If
            HandsEquip.Text = MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(Control - 1) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1)))) 'you wear helmets on your head
            MainForm.SND("You equip " + MainForm.ItemInventoryName(Control - 1) + "+" + LTrim(Str(MainForm.ItemInventoryQuality(Control - 1))) + ".")
            MainForm.PlayerEquipNHands = MainForm.ItemInventoryName(Control - 1)
            MainForm.PlayerEquipHands = Weapon
            MainForm.PlayerEquipQHands = MainForm.ItemInventoryQuality(Control - 1)
            MainForm.PlayerAttack = MainForm.PlayerAttack + MainForm.PlayerEquipQHands - PreviousItemQuality
        ElseIf MainForm.ItemInventoryType(Control - 1) = GenerateItem.Food Then
            Dim temphp As Integer = MainForm.PlayerCurHitpoints
            MainForm.PlayerCurHitpoints += (5 * (MainForm.ItemInventoryQuality(Control - 1) + 1))
            If MainForm.PlayerCurHitpoints > MainForm.PlayerHitpoints Then MainForm.PlayerCurHitpoints = MainForm.PlayerHitpoints
            MainForm.SND("You were healed for " + LTrim(Str(MainForm.PlayerCurHitpoints - temphp)) + ".")
        ElseIf MainForm.ItemInventoryType(Control - 1) = GenerateItem.Water Then
            Dim tempwp As Integer = MainForm.PlayerCurWillpower
            MainForm.PlayerCurWillpower += (5 * (MainForm.ItemInventoryQuality(Control - 1) + 1))
            If MainForm.PlayerCurWillpower > MainForm.PlayerWillpower Then MainForm.PlayerCurWillpower = MainForm.PlayerWillpower
            MainForm.SND("You were energized for " + LTrim(Str(MainForm.PlayerCurWillpower - tempwp)) + ".")
        ElseIf MainForm.ItemInventoryType(Control - 1) = GenerateItem.Potion Then
            Dim plural As String = "s"
            If MainForm.ItemInventoryName(Control - 1) = "Swim Potion" Then
                MainForm.WaterImmune = MainForm.ItemInventoryQuality(Control - 1) + 1
                If MainForm.WaterImmune = 1 Then plural = ""
                MainForm.SND("Drown immune for " + LTrim(Str(MainForm.WaterImmune)) + " round" + plural + ".")
            ElseIf MainForm.ItemInventoryName(Control - 1) = "Freeze Potion" Then
                MainForm.IceImmune = MainForm.ItemInventoryQuality(Control - 1) + 1
                If MainForm.IceImmune = 1 Then plural = ""
                MainForm.SND("Freeze immune for " + LTrim(Str(MainForm.IceImmune)) + " round" + plural + ".")
            ElseIf MainForm.ItemInventoryName(Control - 1) = "Burn Potion" Then
                MainForm.LavaImmune = MainForm.ItemInventoryQuality(Control - 1) + 1
                If MainForm.LavaImmune = 1 Then plural = ""
                MainForm.SND("Burn immune for " + LTrim(Str(MainForm.LavaImmune)) + " round" + plural + ".")
            End If
        End If
            MainForm.ReDraw() 'cause a tick every time something is drank, eaten, or worn
            MainForm.RefreshStats()
            MainForm.ItemInventoryType(Control - 1) = 0 'after item is worn, remove from inventory
            MainForm.ItemInventoryName(Control - 1) = "" 'that includes it's string
            MainForm.ItemInventoryQuality(Control - 1) = 0
            If PreviousItemType > 0 Then
                MainForm.ItemInventoryType(Control - 1) = PreviousItemType
                MainForm.ItemInventoryName(Control - 1) = PreviousItemName
                MainForm.ItemInventoryQuality(Control - 1) = PreviousItemQuality
        End If
        ResetQuickInventory()
            Return 0
    End Function
    Private Sub ProcessKeys(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyValue = Keys.I Then
            Me.Close()
        End If
    End Sub
    Public Sub DropItem(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Drop9.Click, Drop8.Click, Drop7.Click, Drop6.Click, Drop5.Click, Drop4.Click, Drop3.Click, Drop20.Click, Drop2.Click, Drop19.Click, Drop18.Click, Drop17.Click, Drop16.Click, Drop15.Click, Drop14.Click, Drop13.Click, Drop12.Click, Drop11.Click, Drop10.Click, Drop1.Click
        If sender.Equals(Drop1) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(0) + ".")
            MainForm.ItemInventoryName(0) = ""
            MainForm.ItemInventoryType(0) = 0
        ElseIf sender.Equals(Drop2) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(1) + ".")
            MainForm.ItemInventoryName(1) = ""
            MainForm.ItemInventoryType(1) = 0
        ElseIf sender.Equals(Drop3) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(2) + ".")
            MainForm.ItemInventoryName(2) = ""
            MainForm.ItemInventoryType(2) = 0
        ElseIf sender.Equals(Drop4) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(3) + ".")
            MainForm.ItemInventoryName(3) = ""
            MainForm.ItemInventoryType(3) = 0
        ElseIf sender.Equals(Drop5) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(4) + ".")
            MainForm.ItemInventoryName(4) = ""
            MainForm.ItemInventoryType(4) = 0
        ElseIf sender.Equals(Drop6) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(5) + ".")
            MainForm.ItemInventoryName(5) = ""
            MainForm.ItemInventoryType(5) = 0
        ElseIf sender.Equals(Drop7) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(6) + ".")
            MainForm.ItemInventoryName(6) = ""
            MainForm.ItemInventoryType(6) = 0
        ElseIf sender.Equals(Drop8) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(7) + ".")
            MainForm.ItemInventoryName(7) = ""
            MainForm.ItemInventoryType(7) = 0
        ElseIf sender.Equals(Drop9) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(8) + ".")
            MainForm.ItemInventoryName(8) = ""
            MainForm.ItemInventoryType(8) = 0
        ElseIf sender.Equals(Drop10) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(9) + ".")
            MainForm.ItemInventoryName(9) = ""
            MainForm.ItemInventoryType(9) = 0
        ElseIf sender.Equals(Drop11) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(10) + ".")
            MainForm.ItemInventoryName(10) = ""
            MainForm.ItemInventoryType(10) = 0
        ElseIf sender.Equals(Drop12) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(11) + ".")
            MainForm.ItemInventoryName(11) = ""
            MainForm.ItemInventoryType(11) = 0
        ElseIf sender.Equals(Drop13) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(12) + ".")
            MainForm.ItemInventoryName(12) = ""
            MainForm.ItemInventoryType(12) = 0
        ElseIf sender.Equals(Drop14) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(13) + ".")
            MainForm.ItemInventoryName(13) = ""
            MainForm.ItemInventoryType(13) = 0
        ElseIf sender.Equals(Drop15) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(14) + ".")
            MainForm.ItemInventoryName(14) = ""
            MainForm.ItemInventoryType(14) = 0
        ElseIf sender.Equals(Drop16) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(15) + ".")
            MainForm.ItemInventoryName(15) = ""
            MainForm.ItemInventoryType(15) = 0
        ElseIf sender.Equals(Drop17) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(16) + ".")
            MainForm.ItemInventoryName(16) = ""
            MainForm.ItemInventoryType(16) = 0
        ElseIf sender.Equals(Drop18) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(17) + ".")
            MainForm.ItemInventoryName(17) = ""
            MainForm.ItemInventoryType(17) = 0
        ElseIf sender.Equals(Drop19) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(18) + ".")
            MainForm.ItemInventoryName(18) = ""
            MainForm.ItemInventoryType(18) = 0
        ElseIf sender.Equals(Drop20) Then
            MainForm.SND("You destroy " + MainForm.ItemInventoryName(19) + ".")
            MainForm.ItemInventoryName(19) = ""
            MainForm.ItemInventoryType(19) = 0
        End If
        Dim Line1 As String
        If MainForm.ItemInventoryName(0) <> "" Then Line1 = "1. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(0)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(0))) + Chr(13) Else Line1 = "1." + Chr(13)
        If MainForm.ItemInventoryName(1) <> "" Then Line1 += "2. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(1)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(1))) + Chr(13) Else Line1 += "2." + Chr(13)
        If MainForm.ItemInventoryName(2) <> "" Then Line1 += "3. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(2)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(2))) + Chr(13) Else Line1 += "3." + Chr(13)
        If MainForm.ItemInventoryName(3) <> "" Then Line1 += "4. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(3)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(3))) + Chr(13) Else Line1 += "4." + Chr(13)
        If MainForm.ItemInventoryName(4) <> "" Then Line1 += "5. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(4)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(4))) + Chr(13) Else Line1 += "5." + Chr(13)
        If MainForm.ItemInventoryName(5) <> "" Then Line1 += "6. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(5)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(5))) + Chr(13) Else Line1 += "6." + Chr(13)
        If MainForm.ItemInventoryName(6) <> "" Then Line1 += "7. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(6)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(6))) + Chr(13) Else Line1 += "7." + Chr(13)
        If MainForm.ItemInventoryName(7) <> "" Then Line1 += "8. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(7)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(7))) + Chr(13) Else Line1 += "8." + Chr(13)
        If MainForm.ItemInventoryName(8) <> "" Then Line1 += "9. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(8)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(8))) + Chr(13) Else Line1 += "9." + Chr(13)
        If MainForm.ItemInventoryName(9) <> "" Then Line1 += "10. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(9)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(9))) + Chr(13) Else Line1 += "10." + Chr(13)
        Inventory1.Text = Line1 : Line1 = ""
        If MainForm.ItemInventoryName(10) <> "" Then Line1 = "11. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(10)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(10))) + Chr(13) Else Line1 = "11." + Chr(13)
        If MainForm.ItemInventoryName(11) <> "" Then Line1 += "12. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(11)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(11))) + Chr(13) Else Line1 += "12." + Chr(13)
        If MainForm.ItemInventoryName(12) <> "" Then Line1 += "13. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(12)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(12))) + Chr(13) Else Line1 += "13." + Chr(13)
        If MainForm.ItemInventoryName(13) <> "" Then Line1 += "14. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(13)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(13))) + Chr(13) Else Line1 += "14." + Chr(13)
        If MainForm.ItemInventoryName(14) <> "" Then Line1 += "15. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(14)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(14))) + Chr(13) Else Line1 += "15." + Chr(13)
        If MainForm.ItemInventoryName(15) <> "" Then Line1 += "16. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(15)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(15))) + Chr(13) Else Line1 += "16." + Chr(13)
        If MainForm.ItemInventoryName(16) <> "" Then Line1 += "17. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(16)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(16))) + Chr(13) Else Line1 += "17." + Chr(13)
        If MainForm.ItemInventoryName(17) <> "" Then Line1 += "18. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(17)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(17))) + Chr(13) Else Line1 += "18." + Chr(13)
        If MainForm.ItemInventoryName(18) <> "" Then Line1 += "19. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(18)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(18))) + Chr(13) Else Line1 += "19." + Chr(13)
        If MainForm.ItemInventoryName(19) <> "" Then Line1 += "20. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(19)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(19))) + Chr(13) Else Line1 += "20." + Chr(13)
        Inventory2.Text = Line1 : Line1 = ""
        ResetQuickInventory()
    End Sub
    Private Sub WearItem(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Wear9.Click, Wear8.Click, Wear7.Click, Wear6.Click, Wear5.Click, Wear4.Click, Wear3.Click, Wear20.Click, Wear2.Click, Wear19.Click, Wear18.Click, Wear17.Click, Wear16.Click, Wear15.Click, Wear14.Click, Wear13.Click, Wear12.Click, Wear11.Click, Wear10.Click, Wear1.Click
        If sender.Equals(Wear1) Then
            Processwear(1)
        ElseIf sender.Equals(Wear2) Then
            Processwear(2)
        ElseIf sender.Equals(Wear3) Then
            Processwear(3)
        ElseIf sender.Equals(Wear4) Then
            Processwear(4)
        ElseIf sender.Equals(Wear5) Then
            Processwear(5)
        ElseIf sender.Equals(Wear6) Then
            Processwear(6)
        ElseIf sender.Equals(Wear7) Then
            Processwear(7)
        ElseIf sender.Equals(Wear8) Then
            Processwear(8)
        ElseIf sender.Equals(Wear9) Then
            Processwear(9)
        ElseIf sender.Equals(Wear10) Then
            Processwear(10)
        ElseIf sender.Equals(Wear11) Then
            Processwear(11)
        ElseIf sender.Equals(Wear12) Then
            Processwear(12)
        ElseIf sender.Equals(Wear13) Then
            Processwear(13)
        ElseIf sender.Equals(Wear14) Then
            Processwear(14)
        ElseIf sender.Equals(Wear15) Then
            Processwear(15)
        ElseIf sender.Equals(Wear16) Then
            Processwear(16)
        ElseIf sender.Equals(Wear17) Then
            Processwear(17)
        ElseIf sender.Equals(Wear18) Then
            Processwear(18)
        ElseIf sender.Equals(Wear19) Then
            Processwear(19)
        ElseIf sender.Equals(Wear20) Then
            Processwear(20)
        End If
        Dim Line1 As String
        If MainForm.ItemInventoryName(0) <> "" Then Line1 = "1. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(0)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(0))) + Chr(13) Else Line1 = "1." + Chr(13)
        If MainForm.ItemInventoryName(1) <> "" Then Line1 += "2. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(1)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(1))) + Chr(13) Else Line1 += "2." + Chr(13)
        If MainForm.ItemInventoryName(2) <> "" Then Line1 += "3. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(2)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(2))) + Chr(13) Else Line1 += "3." + Chr(13)
        If MainForm.ItemInventoryName(3) <> "" Then Line1 += "4. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(3)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(3))) + Chr(13) Else Line1 += "4." + Chr(13)
        If MainForm.ItemInventoryName(4) <> "" Then Line1 += "5. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(4)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(4))) + Chr(13) Else Line1 += "5." + Chr(13)
        If MainForm.ItemInventoryName(5) <> "" Then Line1 += "6. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(5)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(5))) + Chr(13) Else Line1 += "6." + Chr(13)
        If MainForm.ItemInventoryName(6) <> "" Then Line1 += "7. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(6)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(6))) + Chr(13) Else Line1 += "7." + Chr(13)
        If MainForm.ItemInventoryName(7) <> "" Then Line1 += "8. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(7)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(7))) + Chr(13) Else Line1 += "8." + Chr(13)
        If MainForm.ItemInventoryName(8) <> "" Then Line1 += "9. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(8)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(8))) + Chr(13) Else Line1 += "9." + Chr(13)
        If MainForm.ItemInventoryName(9) <> "" Then Line1 += "10. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(9)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(9))) + Chr(13) Else Line1 += "10." + Chr(13)
        Inventory1.Text = Line1 : Line1 = ""
        If MainForm.ItemInventoryName(10) <> "" Then Line1 = "11. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(10)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(10))) + Chr(13) Else Line1 = "11." + Chr(13)
        If MainForm.ItemInventoryName(11) <> "" Then Line1 += "12. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(11)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(11))) + Chr(13) Else Line1 += "12." + Chr(13)
        If MainForm.ItemInventoryName(12) <> "" Then Line1 += "13. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(12)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(12))) + Chr(13) Else Line1 += "13." + Chr(13)
        If MainForm.ItemInventoryName(13) <> "" Then Line1 += "14. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(13)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(13))) + Chr(13) Else Line1 += "14." + Chr(13)
        If MainForm.ItemInventoryName(14) <> "" Then Line1 += "15. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(14)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(14))) + Chr(13) Else Line1 += "15." + Chr(13)
        If MainForm.ItemInventoryName(15) <> "" Then Line1 += "16. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(15)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(15))) + Chr(13) Else Line1 += "16." + Chr(13)
        If MainForm.ItemInventoryName(16) <> "" Then Line1 += "17. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(16)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(16))) + Chr(13) Else Line1 += "17." + Chr(13)
        If MainForm.ItemInventoryName(17) <> "" Then Line1 += "18. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(17)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(17))) + Chr(13) Else Line1 += "18." + Chr(13)
        If MainForm.ItemInventoryName(18) <> "" Then Line1 += "19. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(18)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(18))) + Chr(13) Else Line1 += "19." + Chr(13)
        If MainForm.ItemInventoryName(19) <> "" Then Line1 += "20. " + MainForm.CapitalizeFirstLetter(MainForm.ItemInventoryName(19)) + " +" + LTrim(Str(MainForm.ItemInventoryQuality(19))) + Chr(13) Else Line1 += "20." + Chr(13)
        Inventory2.Text = Line1 : Line1 = ""
        AttackScore.Text = "Attack: " + LTrim(Str(MainForm.PlayerAttack))
        DefenseScore.Text = "Defense: " + LTrim(Str(MainForm.PlayerDefense))
    End Sub
End Class