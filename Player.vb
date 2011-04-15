Public Class Player
    Public Name As String
    Public Hidden As Short
    Public PlayerClass As String
    Public PlayerRace As String
    Public Defense As Byte
    Public Attack As Byte
    Public Alive As Boolean
    Public Targeting As Boolean
    Public PlusRange As Short
    Public PlusItems As Short
#Region "Position"
    Public X, Y As Short
    Public LastX, LastY As Short
#End Region
#Region "Progress"
    Public Turns As Long
    Public Gold As Short
#End Region
#Region "Statistics"
    Public HealthRejuv, EnergyRejuv As Short
    Public FearPercent, AggroPercent As Short
    Public CurHitpoints, Hitpoints As Short
    Public CurEnergy, Energy As Short
    Public Strength, MaxStrength As Byte
    Public Intelligence, MaxIntelligence As Byte
    Public Wisdom, MaxWisdom As Byte
    Public Constitution, MaxConstitution As Byte
    Public Charisma, MaxCharisma As Byte
    Public Luck, MaxLuck As Byte
#End Region
#Region "Level"
    Public CurrentLevel As Byte
    Public Experience As Integer
    Public Points As Short
    Public Ranks As Short
    Public RankModifier As Short
    Public Captain As Short
    Public Scout As Short
    Public Tank As Short
    Public Benefactor As Short
    Public Mule As Short
#End Region
#Region "Equipment"
    Public Head, HeadQ, HeadN As Byte
    Public Chest, ChestQ, ChestN As Byte
    Public Arms, ArmsQ, ArmsN As Byte
    Public Legs, LegsQ, LegsN As Byte
    Public Feet, FeetQ, FeetN As Byte
#End Region
End Class
