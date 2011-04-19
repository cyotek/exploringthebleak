Public Class Map
    Public DepthLevel As Short
    Public MapLevel As Integer 'keeps track of maps generated
    Public CurrentSeed As Short
    Public GenerateType As Short
    Public Environment As Byte
    Public MapData(,) As Short
    Public MapCreated() As Boolean 'use seed reference tokens to pre-made maps
    Public MapEntrances(,,) As Short 'mapentrances(maxdepthlevel,exit type, axis type) exit types(0=down exit, 1=up exit locations) axis types(0=x,1=y)
    Public MapShown(,) As Boolean 'use seed reference tokens to pre-made maps, saves x & y shown types on map
    Public MapOccupied(,) As Byte 'use seed reference tokens to pre-made maps, saves x & y occupied types on map
    Public ItemOccupied(,) As Byte
    Public MapBlur(,) As Boolean 'use seed reference tokens to pre-made maps, saves x & y blur types
    Public WaterBlur(,) As Boolean 'use seed reference tokens to pre-made maps, saves x & y blur types
    Public FogMap(,) As Boolean 'use seed reference tokens to pre-made maps, saves x & y fog types
    Public RiverType As SByte 'used to define the river water type on a map
End Class
