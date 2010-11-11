Module GenerateItem
#Region "Dimensions and Variables"
    Const Head As Short = 1
    Const Chest As Short = 2
    Const Arms As Short = 3
    Const Hands As Short = 4
    Const Legs As Short = 5
    Const Boots As Short = 6
    Const Weapon As Short = 7
    Const Gold As Short = 8
    Public NameType As String
    Public ItemType As Short
    Public ShowType As String
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
        If RandomType > 80 Then
            For Ensure = 1 To seed Step 1
                Randomnum = RandomItem.Next(0, 68)
            Next
            If Randomnum = 0 Then : NameType = "Seax"
                ItemType = Weapon : ShowType = "s"
            ElseIf Randomnum = 1 Then : NameType = "Stiletto"
                ItemType = Weapon : ShowType = "S"
            ElseIf Randomnum = 2 Then : NameType = "Pugio"
                ItemType = Weapon : ShowType = "p"
            ElseIf Randomnum = 3 Then : NameType = "Poniard"
                ItemType = Weapon : ShowType = "P"
            ElseIf Randomnum = 4 Then : NameType = "Dirk"
                ItemType = Weapon : ShowType = "d"
            ElseIf Randomnum = 5 Then : NameType = "Dagger"
                ItemType = Weapon : ShowType = "D"
            ElseIf Randomnum = 6 Then : NameType = "Pickaxe"
                ItemType = Weapon : ShowType = "i"
            ElseIf Randomnum = 7 Then : NameType = "Katar"
                ItemType = Weapon : ShowType = "k"
            ElseIf Randomnum = 8 Then : NameType = "Cestus"
                ItemType = Weapon : ShowType = "c"
            ElseIf Randomnum = 17 Then : NameType = "Spear"
                ItemType = Weapon : ShowType = "e"
            ElseIf Randomnum = 18 Then : NameType = "Pike"
                ItemType = Weapon : ShowType = "I"
            ElseIf Randomnum = 19 Then : NameType = "Doloire"
                ItemType = Weapon : ShowType = "o"
            ElseIf Randomnum = 20 Then : NameType = "Ranseur"
                ItemType = Weapon : ShowType = "r"
            ElseIf Randomnum = 21 Then : NameType = "Spetum"
                ItemType = Weapon : ShowType = "E"
            ElseIf Randomnum = 22 Then : NameType = "Hasta"
                ItemType = Weapon : ShowType = "h"
            ElseIf Randomnum = 23 Then : NameType = "Tepoztopilli"
                ItemType = Weapon : ShowType = "z"
            ElseIf Randomnum = 24 Then : NameType = "Falcata"
                ItemType = Weapon : ShowType = "f"
            ElseIf Randomnum = 25 Then : NameType = "Gladius"
                ItemType = Weapon : ShowType = "g"
            ElseIf Randomnum = 26 Then : NameType = "Kopis"
                ItemType = Weapon : ShowType = "K"
            ElseIf Randomnum = 27 Then : NameType = "Shortsword"
                ItemType = Weapon : ShowType = "w"
            ElseIf Randomnum = 28 Then : NameType = "Broadsword"
                ItemType = Weapon : ShowType = "W"
            ElseIf Randomnum = 29 Then : NameType = "Masakari"
                ItemType = Weapon : ShowType = "m"
            ElseIf Randomnum = 30 Then : NameType = "Sappara"
                ItemType = Weapon : ShowType = "a"
            ElseIf Randomnum = 31 Then : NameType = "Khopesh"
                ItemType = Weapon : ShowType = "H"
            ElseIf Randomnum = 32 Then : NameType = "Longsword"
                ItemType = Weapon : ShowType = "O"
            ElseIf Randomnum = 33 Then : NameType = "Flamberge"
                ItemType = Weapon : ShowType = "M"
            ElseIf Randomnum = 34 Then : NameType = "Falchion"
                ItemType = Weapon : ShowType = "C"
            ElseIf Randomnum = 35 Then : NameType = "Claymore"
                ItemType = Weapon : ShowType = "y"
            ElseIf Randomnum = 36 Then : NameType = "Hatchet"
                ItemType = Weapon : ShowType = "h"
            ElseIf Randomnum = 37 Then : NameType = "Axe"
                ItemType = Weapon : ShowType = "x"
            ElseIf Randomnum = 38 Then : NameType = "Field Axe"
                ItemType = Weapon : ShowType = "X"
            ElseIf Randomnum = 39 Then : NameType = "Rapier"
                ItemType = Weapon : ShowType = "R"
            ElseIf Randomnum = 40 Then : NameType = "Scythe"
                ItemType = Weapon : ShowType = "S"
            ElseIf Randomnum = 41 Then : NameType = "Chakrum"
                ItemType = Weapon : ShowType = "K"
            ElseIf Randomnum = 42 Then : NameType = "Scimitar"
                ItemType = Weapon : ShowType = "s"
            ElseIf Randomnum = 43 Then : NameType = "Cleaver"
                ItemType = Weapon : ShowType = "C"
            ElseIf Randomnum = 44 Then : NameType = "Falx"
                ItemType = Weapon : ShowType = "x"
            ElseIf Randomnum = 45 Then : NameType = "Rhomphaia"
                ItemType = Weapon : ShowType = "r"
            ElseIf Randomnum = 46 Then : NameType = "Bardiche"
                ItemType = Weapon : ShowType = "B"
            ElseIf Randomnum = 47 Then : NameType = "Bill"
                ItemType = Weapon : ShowType = "b"
            ElseIf Randomnum = 48 Then : NameType = "Glaive"
                ItemType = Weapon : ShowType = "g"
            ElseIf Randomnum = 49 Then : NameType = "Guisarme"
                ItemType = Weapon : ShowType = "u"
            ElseIf Randomnum = 50 Then : NameType = "Voulge"
                ItemType = Weapon : ShowType = "v"
            ElseIf Randomnum = 51 Then : NameType = "Staff"
                ItemType = Weapon : ShowType = "s"
            ElseIf Randomnum = 52 Then : NameType = "Morning Star"
                ItemType = Weapon : ShowType = "M"
            ElseIf Randomnum = 53 Then : NameType = "Macuahuiti"
                ItemType = Weapon : ShowType = "M"
            ElseIf Randomnum = 54 Then : NameType = "Mace"
                ItemType = Weapon : ShowType = "m"
            ElseIf Randomnum = 55 Then : NameType = "Flail"
                ItemType = Weapon : ShowType = "f"
            ElseIf Randomnum = 56 Then : NameType = "Hammer"
                ItemType = Weapon : ShowType = "m"
            ElseIf Randomnum = 57 Then : NameType = "War Hammer"
                ItemType = Weapon : ShowType = "W"
            ElseIf Randomnum = 58 Then : NameType = "Scepter"
                ItemType = Weapon : ShowType = "s"
            ElseIf Randomnum = 59 Then : NameType = "Club"
                ItemType = Weapon : ShowType = "c"
            ElseIf Randomnum = 60 Then : NameType = "Blackjack"
                ItemType = Weapon : ShowType = "b"
            ElseIf Randomnum = 61 Then : NameType = "Eku"
                ItemType = Weapon : ShowType = "e"
            ElseIf Randomnum = 62 Then : NameType = "Sai"
                ItemType = Weapon : ShowType = "s"
            ElseIf Randomnum = 63 Then : NameType = "Halberd"
                ItemType = Weapon : ShowType = "H"
            ElseIf Randomnum = 64 Then : NameType = "Trident"
                ItemType = Weapon : ShowType = "T"
            ElseIf Randomnum = 65 Then : NameType = "Kukri"
                ItemType = Weapon : ShowType = "K"
            ElseIf Randomnum = 66 Then : NameType = "Lathi"
                ItemType = Weapon : ShowType = "L"
            ElseIf Randomnum = 67 Then : NameType = "Whip"
                ItemType = Weapon : ShowType = "w"
            End If
        ElseIf RandomType > 45 Then
            For Ensure = 1 To seed Step 1
                Randomnum = RandomItem.Next(0, 140)
            Next
            If Randomnum = 0 Then
                NameType = "Crown" : ItemType = Head : ShowType = "c"
            ElseIf Randomnum = 1 Then
                NameType = "Chapeau" : ItemType = Head : ShowType = "C"
            ElseIf Randomnum = 2 Then
                NameType = "Chaplet" : ItemType = Head : ShowType = "h"
            ElseIf Randomnum = 3 Then
                NameType = "Coif" : ItemType = Head : ShowType = "o"
            ElseIf Randomnum = 4 Then
                NameType = "Coronet" : ItemType = Head : ShowType = "O"
            ElseIf Randomnum = 5 Then
                NameType = "Cowl" : ItemType = Head : ShowType = "w"
            ElseIf Randomnum = 6 Then
                NameType = "Spectacles" : ItemType = Head : ShowType = "S"
            ElseIf Randomnum = 7 Then
                NameType = "Tiara" : ItemType = Head : ShowType = "t"
            ElseIf Randomnum = 8 Then
                NameType = "Eyeglasses" : ItemType = Head : ShowType = "e"
            ElseIf Randomnum = 9 Then
                NameType = "Monocle" : ItemType = Head : ShowType = "m"
            ElseIf Randomnum = 10 Then
                NameType = "Wreath" : ItemType = Head : ShowType = "w"
            ElseIf Randomnum = 11 Then
                NameType = "Circlet" : ItemType = Head : ShowType = "i"
            ElseIf Randomnum = 12 Then
                NameType = "Mask" : ItemType = Head : ShowType = "m"
            ElseIf Randomnum = 13 Then
                NameType = "Headdress" : ItemType = Head : ShowType = "H"
            ElseIf Randomnum = 14 Then
                NameType = "Hood" : ItemType = Head : ShowType = "o"
            ElseIf Randomnum = 15 Then
                NameType = "Cap" : ItemType = Head : ShowType = "c"
            ElseIf Randomnum = 16 Then
                NameType = "Helm" : ItemType = Head : ShowType = "e"
            ElseIf Randomnum = 17 Then
                NameType = "Full Helm" : ItemType = Head : ShowType = "f"
            ElseIf Randomnum = 18 Then
                NameType = "Horned Helm" : ItemType = Head : ShowType = "H"
            ElseIf Randomnum = 19 Then
                NameType = "Skull Cap" : ItemType = Head : ShowType = "k"
            ElseIf Randomnum = 20 Then
                NameType = "Face Guard" : ItemType = Head : ShowType = "a"
            ElseIf Randomnum = 21 Then
                NameType = "Face Plate" : ItemType = Head : ShowType = "C"
            ElseIf Randomnum = 22 Then
                NameType = "Gorget" : ItemType = Head : ShowType = "G"
            ElseIf Randomnum = 23 Then
                NameType = "Amulet" : ItemType = Head : ShowType = "m"
            ElseIf Randomnum = 24 Then
                NameType = "Choker" : ItemType = Head : ShowType = "K"
            ElseIf Randomnum = 25 Then
                NameType = "Locket" : ItemType = Head : ShowType = "c"
            ElseIf Randomnum = 26 Then
                NameType = "Medallion" : ItemType = Head : ShowType = "D"
            ElseIf Randomnum = 27 Then
                NameType = "Neckband" : ItemType = Head : ShowType = "B"
            ElseIf Randomnum = 28 Then
                NameType = "Necklace" : ItemType = Head : ShowType = "n"
            ElseIf Randomnum = 29 Then
                NameType = "Mark" : ItemType = Head : ShowType = "K"
            ElseIf Randomnum = 30 Then
                NameType = "Pendant" : ItemType = Head : ShowType = "n"
            ElseIf Randomnum = 31 Then
                NameType = "Icon" : ItemType = Head : ShowType = "c"
            ElseIf Randomnum = 32 Then
                NameType = "Talisman" : ItemType = Head : ShowType = "L"
            ElseIf Randomnum = 33 Then
                NameType = "Amice" : ItemType = Chest : ShowType = "m"
            ElseIf Randomnum = 34 Then
                NameType = "Pauldrons" : ItemType = Chest : ShowType = "p"
            ElseIf Randomnum = 35 Then
                NameType = "Mantle" : ItemType = Chest : ShowType = "M"
            ElseIf Randomnum = 36 Then
                NameType = "Studded Mantle" : ItemType = Chest : ShowType = "S"
            ElseIf Randomnum = 37 Then
                NameType = "Shoulder Pads" : ItemType = Chest : ShowType = "h"
            ElseIf Randomnum = 38 Then
                NameType = "Spaulders" : ItemType = Chest : ShowType = "p"
            ElseIf Randomnum = 39 Then
                NameType = "Scaled Shoulders" : ItemType = Chest : ShowType = "s"
            ElseIf Randomnum = 40 Then
                NameType = "Splint Shoulders" : ItemType = Chest : ShowType = "S"
            ElseIf Randomnum = 41 Then
                NameType = "Half-plate Shoulders" : ItemType = Chest : ShowType = "H"
            ElseIf Randomnum = 42 Then
                NameType = "Plate Shoulders" : ItemType = Chest : ShowType = "p"
            ElseIf Randomnum = 43 Then
                NameType = "Armlets" : ItemType = Arms : ShowType = "a"
            ElseIf Randomnum = 44 Then
                NameType = "Armband" : ItemType = Arms : ShowType = "A"
            ElseIf Randomnum = 45 Then
                NameType = "Arm-guards" : ItemType = Arms : ShowType = "a"
            ElseIf Randomnum = 46 Then
                NameType = "Arm Wraps" : ItemType = Arms : ShowType = "A"
            ElseIf Randomnum = 47 Then
                NameType = "Cuffs" : ItemType = Arms : ShowType = "c"
            ElseIf Randomnum = 48 Then
                NameType = "Wristband" : ItemType = Arms : ShowType = "w"
            ElseIf Randomnum = 49 Then
                NameType = "Bracers" : ItemType = Arms : ShowType = "b"
            ElseIf Randomnum = 50 Then
                NameType = "Bracelet" : ItemType = Arms : ShowType = "r"
            ElseIf Randomnum = 51 Then
                NameType = "Shackles" : ItemType = Hands : ShowType = "s"
            ElseIf Randomnum = 52 Then
                NameType = "Bindings" : ItemType = Hands : ShowType = "b"
            ElseIf Randomnum = 53 Then
                NameType = "Gloves" : ItemType = Hands : ShowType = "g"
            ElseIf Randomnum = 54 Then
                NameType = "Mittens" : ItemType = Hands : ShowType = "m"
            ElseIf Randomnum = 55 Then
                NameType = "Handwraps" : ItemType = Hands : ShowType = "H"
            ElseIf Randomnum = 56 Then
                NameType = "Handguards" : ItemType = Hands : ShowType = "g"
            ElseIf Randomnum = 57 Then
                NameType = "Gauntlets" : ItemType = Hands : ShowType = "G"
            ElseIf Randomnum = 58 Then
                NameType = "Chainmail Gauntlets" : ItemType = Hands : ShowType = "c"
            ElseIf Randomnum = 59 Then
                NameType = "Scalemail Gauntlets" : ItemType = Hands : ShowType = "C"
            ElseIf Randomnum = 60 Then
                NameType = "Platemail Gauntlets" : ItemType = Hands : ShowType = "p"
            ElseIf Randomnum = 61 Then
                NameType = "Platemail" : ItemType = Chest : ShowType = "P"
            ElseIf Randomnum = 62 Then
                NameType = "Ringmail" : ItemType = Chest : ShowType = "r"
            ElseIf Randomnum = 63 Then
                NameType = "Chain Cuirass" : ItemType = Chest : ShowType = "C"
            ElseIf Randomnum = 64 Then
                NameType = "Half=plate" : ItemType = Chest : ShowType = "h"
            ElseIf Randomnum = 65 Then
                NameType = "Chestplate" : ItemType = Chest : ShowType = "c"
            ElseIf Randomnum = 66 Then
                NameType = "Chestguard" : ItemType = Chest : ShowType = "g"
            ElseIf Randomnum = 67 Then
                NameType = "Scalemail" : ItemType = Chest : ShowType = "C"
            ElseIf Randomnum = 68 Then
                NameType = "Splintmail" : ItemType = Chest : ShowType = "S"
            ElseIf Randomnum = 69 Then
                NameType = "Studded Tunic" : ItemType = Chest : ShowType = "d"
            ElseIf Randomnum = 70 Then
                NameType = "Tunic" : ItemType = Chest : ShowType = "t"
            ElseIf Randomnum = 71 Then
                NameType = "Vestment" : ItemType = Chest : ShowType = "v"
            ElseIf Randomnum = 72 Then
                NameType = "Hauberk" : ItemType = Chest : ShowType = "h"
            ElseIf Randomnum = 73 Then
                NameType = "Field Plate" : ItemType = Chest : ShowType = "f"
            ElseIf Randomnum = 74 Then
                NameType = "Banded Mail" : ItemType = Chest : ShowType = "b"
            ElseIf Randomnum = 75 Then
                NameType = "Brigandine Armor" : ItemType = Chest : ShowType = "B"
            ElseIf Randomnum = 76 Then
                NameType = "Robe" : ItemType = Chest : ShowType = "r"
            ElseIf Randomnum = 77 Then
                NameType = "Raiment" : ItemType = Chest : ShowType = "R"
            ElseIf Randomnum = 78 Then
                NameType = "Tabard" : ItemType = Chest : ShowType = "T"
            ElseIf Randomnum = 79 Then
                NameType = "Doublet" : ItemType = Chest : ShowType = "D"
            ElseIf Randomnum = 80 Then
                NameType = "Chemise" : ItemType = Chest : ShowType = "m"
            ElseIf Randomnum = 81 Then
                NameType = "Lorica Segmentata" : ItemType = Chest : ShowType = "L"
            ElseIf Randomnum = 82 Then
                NameType = "Lamellar" : ItemType = Chest : ShowType = "m"
            ElseIf Randomnum = 83 Then
                NameType = "Shawl" : ItemType = Chest : ShowType = "w"
            ElseIf Randomnum = 84 Then
                NameType = "Cape" : ItemType = Chest : ShowType = "p"
            ElseIf Randomnum = 85 Then
                NameType = "Capelet" : ItemType = Chest : ShowType = "c"
            ElseIf Randomnum = 86 Then
                NameType = "Cloak" : ItemType = Chest : ShowType = "k"
            ElseIf Randomnum = 87 Then
                NameType = "Heavy Cloak" : ItemType = Chest : ShowType = "v"
            ElseIf Randomnum = 88 Then
                NameType = "Battle Cloak" : ItemType = Chest : ShowType = "b"
            ElseIf Randomnum = 89 Then
                NameType = "Royal Cloak" : ItemType = Chest : ShowType = "r"
            ElseIf Randomnum = 90 Then
                NameType = "Girdle" : ItemType = Chest : ShowType = "R"
            ElseIf Randomnum = 91 Then
                NameType = "Belt" : ItemType = Chest : ShowType = "B"
            ElseIf Randomnum = 92 Then
                NameType = "Cord" : ItemType = Chest : ShowType = "c"
            ElseIf Randomnum = 93 Then
                NameType = "Waistwrap" : ItemType = Chest : ShowType = "w"
            ElseIf Randomnum = 94 Then
                NameType = "Sash" : ItemType = Chest : ShowType = "s"
            ElseIf Randomnum = 95 Then
                NameType = "Genouillere" : ItemType = Legs : ShowType = "g"
            ElseIf Randomnum = 96 Then
                NameType = "Bloomers" : ItemType = Legs : ShowType = "b"
            ElseIf Randomnum = 97 Then
                NameType = "Breeches" : ItemType = Legs : ShowType = "B"
            ElseIf Randomnum = 98 Then
                NameType = "Trousers" : ItemType = Legs : ShowType = "t"
            ElseIf Randomnum = 99 Then
                NameType = "Leggings" : ItemType = Legs : ShowType = "l"
            ElseIf Randomnum = 100 Then
                NameType = "Legguards" : ItemType = Legs : ShowType = "L"
            ElseIf Randomnum = 101 Then
                NameType = "Skirt" : ItemType = Legs : ShowType = "k"
            ElseIf Randomnum = 102 Then
                NameType = "Split Skirt" : ItemType = Legs : ShowType = "p"
            ElseIf Randomnum = 103 Then
                NameType = "Tights" : ItemType = Legs : ShowType = "t"
            ElseIf Randomnum = 104 Then
                NameType = "Pantaloons" : ItemType = Legs : ShowType = "p"
            ElseIf Randomnum = 105 Then
                NameType = "Scale Leggings" : ItemType = Legs : ShowType = "c"
            ElseIf Randomnum = 106 Then
                NameType = "Splint Leggings" : ItemType = Legs : ShowType = "S"
            ElseIf Randomnum = 107 Then
                NameType = "Half-Plate Leggings" : ItemType = Legs : ShowType = "P"
            ElseIf Randomnum = 108 Then
                NameType = "Plate Leggings" : ItemType = Legs : ShowType = "P"
            ElseIf Randomnum = 109 Then
                NameType = "Chainmail leggings" : ItemType = Legs : ShowType = "c"
            ElseIf Randomnum = 110 Then
                NameType = "Greaves" : ItemType = Legs : ShowType = "g"
            ElseIf Randomnum = 111 Then
                NameType = "Slippers" : ItemType = Boots : ShowType = "S"
            ElseIf Randomnum = 112 Then
                NameType = "Sandals" : ItemType = Boots : ShowType = "s"
            ElseIf Randomnum = 113 Then
                NameType = "Stalkers" : ItemType = Boots : ShowType = "S"
            ElseIf Randomnum = 114 Then
                NameType = "Footguards" : ItemType = Boots : ShowType = "g"
            ElseIf Randomnum = 115 Then
                NameType = "Scale Boots" : ItemType = Boots : ShowType = "b"
            ElseIf Randomnum = 116 Then
                NameType = "Splint Boots" : ItemType = Boots : ShowType = "B"
            ElseIf Randomnum = 117 Then
                NameType = "Half-plate Boots" : ItemType = Boots : ShowType = "h"
            ElseIf Randomnum = 118 Then
                NameType = "Plate Boots" : ItemType = Boots : ShowType = "P"
            ElseIf Randomnum = 119 Then
                NameType = "Anklet" : ItemType = Boots : ShowType = "k"
            ElseIf Randomnum = 120 Then
                NameType = "Boots" : ItemType = Boots : ShowType = "b"
            ElseIf Randomnum = 121 Then
                NameType = "Long Boots" : ItemType = Boots : ShowType = "B"
            ElseIf Randomnum = 122 Then
                NameType = "Footguards" : ItemType = Boots : ShowType = "f"
            ElseIf Randomnum = 123 Then
                NameType = "Ring" : ItemType = Hands : ShowType = "r"
            ElseIf Randomnum = 124 Then
                NameType = "Band" : ItemType = Hands : ShowType = "r"
            ElseIf Randomnum = 125 Then
                NameType = "Thumb Ring" : ItemType = Hands : ShowType = "r"
            ElseIf Randomnum = 126 Then
                NameType = "Wedding Ring" : ItemType = Hands : ShowType = "r"
            ElseIf Randomnum = 127 Then
                NameType = "Engagement Ring" : ItemType = Hands : ShowType = "r"
            ElseIf Randomnum = 128 Then
                NameType = "Signet Ring" : ItemType = Hands : ShowType = "r"
            ElseIf Randomnum = 129 Then
                NameType = "Blood Ring" : ItemType = Hands : ShowType = "r"
            ElseIf Randomnum = 130 Then
                NameType = "Aegis" : ItemType = Arms : ShowType = "A"
            ElseIf Randomnum = 131 Then
                NameType = "Scutum" : ItemType = Arms : ShowType = "S"
            ElseIf Randomnum = 132 Then
                NameType = "Targe" : ItemType = Arms : ShowType = "g"
            ElseIf Randomnum = 133 Then
                NameType = "Roundel" : ItemType = Arms : ShowType = "R"
            ElseIf Randomnum = 134 Then
                NameType = "Buckler" : ItemType = Arms : ShowType = "b"
            ElseIf Randomnum = 135 Then
                NameType = "Disc Shield" : ItemType = Arms : ShowType = "c"
            ElseIf Randomnum = 136 Then
                NameType = "Heater Shield" : ItemType = Arms : ShowType = "s"
            ElseIf Randomnum = 137 Then
                NameType = "Bulwark" : ItemType = Arms : ShowType = "b"
            ElseIf Randomnum = 138 Then
                NameType = "Tower Shield" : ItemType = Arms : ShowType = "t"
            ElseIf Randomnum = 139 Then
                NameType = "Kite Shield" : ItemType = Arms : ShowType = "k"
            End If
        Else
            NameType = "Gold" : ItemType = Gold : ShowType = "g"
        End If
        Return 0
    End Function
End Module

