Public Class Mobile
    Public Health, MaxHealth As UShort
    Public Visible(,,) As Short '3 visible settings (maxdepthlevel,maxmobiles,3)
    Public Alive As Boolean 'death flag
    Public Flee As Short
    Public Type As Short 'identifier type, or creature type
    Public X, Y As Short
    Public LastX, LastY As Short
    Public LastMovement As Short
End Class
