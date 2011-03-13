Module GenerateItem
#Region "Dimensions and Variables"
    Const Armor As Short = 6
    Const Weapon As Short = 7
    Const Gold As Short = 8
    Public Const Food As Short = 9
    Public Const Water As Short = 10
    Public Const Potion As Short = 11
    Public NameType As String
    Public ItemType As Short
    Public ShowType As String
    Public ItemStrength As Short
#End Region
    Public Function GenerateRandomItem(ByVal seed As Short) As String
        Dim RandomItem As New Random
        Dim Randomnum As Integer = -1
        Dim RandomType As Integer = -1
        Dim Ensure As Short = 0
        NameType = "" 'need to set to zero so we can check to make sure it's not repeated twice in a row
        For ensure = 1 To seed Step 1
            RandomType = RandomItem.Next(1, 101)
        Next
        If RandomType > 85 Then
            For Ensure = 1 To seed Step 1
                Randomnum = RandomItem.Next(0, 10)
            Next
            If Randomnum = 0 Then : NameType = "Disruptor Type 1"
                ItemType = Weapon : ShowType = "d" : ItemStrength = 1
            ElseIf Randomnum = 1 Then : NameType = "Disruptor Type 2"
                ItemType = Weapon : ShowType = "D" : ItemStrength = 2
            ElseIf Randomnum = 2 Then : NameType = "Disruptor Type 3"
                ItemType = Weapon : ShowType = "r" : ItemStrength = 3
            ElseIf Randomnum = 3 Then : NameType = "Varon-T Disruptor"
                ItemType = Weapon : ShowType = "R" : ItemStrength = 4
            ElseIf Randomnum = 4 Then : NameType = "Laser Pistol"
                ItemType = Weapon : ShowType = "p" : ItemStrength = 3
            ElseIf Randomnum = 5 Then : NameType = "Pulse Cannon"
                ItemType = Weapon : ShowType = "C" : ItemStrength = 7
            ElseIf Randomnum = 6 Then : NameType = "Phase Cannon"
                ItemType = Weapon : ShowType = "P" : ItemStrength = 6
            ElseIf Randomnum = 7 Then : NameType = "Phaser Type 1"
                ItemType = Weapon : ShowType = "a" : ItemStrength = 3
            ElseIf Randomnum = 8 Then : NameType = "Phaser Type 2"
                ItemType = Weapon : ShowType = "A" : ItemStrength = 4
            ElseIf Randomnum = 9 Then : NameType = "Phaser Type 3"
                ItemType = Weapon : ShowType = "t" : ItemStrength = 5
            End If
        ElseIf RandomType > 60 Then
            ItemType = Armor
            For Ensure = 1 To seed Step 1
                Randomnum = RandomItem.Next(0, 140)
            Next
            If Randomnum = 0 Then
                NameType = "Navy Mark IV" : ItemStrength = 1 : ShowType = "n"
            ElseIf Randomnum = 1 Then
                NameType = "Gemini" : ItemStrength = 2 : ShowType = "g"
            ElseIf Randomnum = 2 Then
                NameType = "MOL MH-7" : ItemStrength = 3 : ShowType = "m"
            ElseIf Randomnum = 3 Then
                NameType = "Skylab A7L" : ItemStrength = 4 : ShowType = "s"
            ElseIf Randomnum = 4 Then
                NameType = "SEE Suit" : ItemStrength = 5 : ShowType = "S"
            ElseIf Randomnum = 5 Then
                NameType = "Launch Entry Suit" : ItemStrength = 6 : ShowType = "e"
            ElseIf Randomnum = 6 Then
                NameType = "ACE Suit" : ItemStrength = 8 : ShowType = "a"
            ElseIf Randomnum = 7 Then
                NameType = "EMU" : ItemStrength = 9 : ShowType = "E"
            ElseIf Randomnum = 8 Then
                NameType = "EVA" : ItemStrength = 9 : ShowType = "A"
            ElseIf Randomnum = 9 Then
                NameType = "Mark III" : ItemStrength = 10 : ShowType = "M"
            ElseIf Randomnum = 10 Then
                NameType = "I-Suit" : ItemStrength = 8 : ShowType = "i"
            ElseIf Randomnum = 11 Then
                NameType = "Bio-Suit" : ItemStrength = 9 : ShowType = "b"
            ElseIf Randomnum = 12 Then
                NameType = "MX-2" : ItemStrength = 9 : ShowType = "X"
            ElseIf Randomnum = 13 Then
                NameType = "Aouda.x" : ItemStrength = 7 : ShowType = "x"
            ElseIf Randomnum = 14 Then
                NameType = "NASA CSSS" : ItemStrength = 9 : ShowType = "N"
            End If
        ElseIf RandomType > 38 Then 'food,water, and potions
            For Ensure = 1 To seed Step 1
                Randomnum = RandomItem.Next(0, 11)
            Next
            If Randomnum < 4 Then : NameType = "Energy Cell"
                ItemType = Water : ShowType = "e"
            ElseIf Randomnum < 8 Then : NameType = "Stim Pack"
                ItemType = Food : ShowType = "s"
            ElseIf Randomnum = 8 Then : NameType = "Fire Shield Type 1"
                ItemType = Potion : ShowType = "f"
            ElseIf Randomnum = 9 Then : NameType = "Fire Shield Type 2"
                ItemType = Potion : ShowType = "F"
            ElseIf Randomnum = 10 Then : NameType = "Fire Shield Type 3"
                ItemType = Potion : ShowType = "B"
            End If
        Else
            NameType = "Precious Minerals" : ItemType = Gold : ShowType = "p"
        End If
        Return 0
    End Function
End Module

