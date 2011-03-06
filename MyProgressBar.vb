Public Class MyProgBar
    Inherits System.Windows.Forms.UserControl
    Protected pMin As Integer
    Protected pMax As Integer
    Protected pValue As Integer
    Protected pCaption As String
    Public Property Min() As Integer
        Get
            Return pMin
        End Get
        Set(ByVal Value As Integer)
            pMin = Value
        End Set
    End Property
    Public Property Max() As Integer
        Get
            Return pMax
        End Get
        Set(ByVal Value As Integer)
            pMax = Value
        End Set
    End Property
    Public Property Caption() As String
        Get
            Return pCaption
        End Get
        Set(ByVal Value As String)
            pCaption = Value
        End Set
    End Property
    Public Property Value() As Integer
        Get
            Return pValue
        End Get
        Set(ByVal Value As Integer)
            pValue = Value
            Me.Invalidate(New Rectangle(0, 0, Me.Width, Me.Height))
            Application.DoEvents()
        End Set
    End Property
    Public Sub StepOne()
        Me.Value += 1
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Drawbar(e)
    End Sub
    Protected Sub Drawbar(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim Pct As Single = pValue / pMax * 100
        'If Pct > 0 Then
        Dim Len As Single = pValue / pMax * Me.Width
        Dim b As Brush = New System.Drawing.SolidBrush(Color.Black)
        e.Graphics.DrawLine(New Pen(Me.ForeColor, Me.Height * 2), 0, 0, Len, 0)
        e.Graphics.DrawString(pCaption, Me.Font, b, Me.Width / 2 - 25, Me.Height / 2 - 5)
        b.Dispose()
        'End If
    End Sub
End Class
