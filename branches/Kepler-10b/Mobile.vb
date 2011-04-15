Public Class Mobile
    Public Visible(,,) As Short '3 visible settings (maxdepthlevel,maxmobiles,3)
    Public Alive As Boolean 'death flag
    Public Type As Short 'identifier type, or creature type
#Region "Position"
    Public X, Y As Short
    Public LastMovement As Short
#End Region
End Class
