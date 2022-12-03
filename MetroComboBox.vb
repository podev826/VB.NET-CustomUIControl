Public Class MetroComboBox
    Inherits ComboBox
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        FlatStyle = FlatStyle.Flat
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)
        Select Case m.Msg
            Case &HF

                Dim g As Graphics = Me.CreateGraphics

                If ComboBoxRenderer.IsSupported Then
                    CustomPaint()
                End If
        End Select
    End Sub

    Private m_borderColor As Color = Color.Black
    Private m_borderThickness As UInteger = 1
    Private m_shadowColor As Color = Color.Gray
    Private m_shadowThickness As Integer = 0
    Private m_borderRound As UInteger = 0

    Public Property BorderColor As Color
        Get
            Return m_borderColor
        End Get
        Set(value As Color)
            m_borderColor = value
            Me.Invalidate()
        End Set
    End Property
    Public Property BorderThickness As Integer
        Get
            Return m_borderThickness
        End Get
        Set(value As Integer)
            m_borderThickness = value
            Me.Invalidate()
        End Set
    End Property
    Public Property ShadowColor As Color
        Get
            Return m_shadowColor
        End Get
        Set(value As Color)
            m_shadowColor = value
            Me.Invalidate()
        End Set
    End Property
    Public Property ShadowThickness As Integer
        Get
            Return m_shadowThickness
        End Get
        Set(value As Integer)
            m_shadowThickness = value
            Me.Invalidate()
        End Set
    End Property
    Public Property BorderRound As Integer
        Get
            Return m_borderRound
        End Get
        Set(value As Integer)
            m_borderRound = value
            Me.Invalidate()
        End Set
    End Property

    Protected Sub CustomPaint()
        Dim g As Graphics = Parent.CreateGraphics
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        Dim diam As Single = Math.Min(m_borderRound, Math.Min(Height, Width))
        Dim r As New Rectangle(
                CSng(Location.X - m_borderThickness / 2 - 1),
                CSng(Location.Y - m_borderThickness / 2 - 1),
                Width + m_borderThickness + 1,
                Height + m_borderThickness + 1)
        Dim path As Drawing2D.GraphicsPath = RoundedRectangle(r, diam)

        Dim shadow As New Rectangle(
                CSng(Location.X - m_borderThickness + m_shadowThickness),
                CSng(Location.Y - m_borderThickness + m_shadowThickness),
                Width + 2 * m_borderThickness,
                Height + 2 * m_borderThickness)
        Dim shadowPath As Drawing2D.GraphicsPath = RoundedRectangle(shadow, 2 * diam)
        g.FillPath(New SolidBrush(m_shadowColor), shadowPath)
        g.DrawPath(New Pen(m_borderColor, m_borderThickness), path)

    End Sub
    Private Function RoundedRectangle(rect As RectangleF, diam As Single) As Drawing2D.GraphicsPath
        Dim path As New Drawing2D.GraphicsPath
        If (diam > 0) Then
            path.AddArc(rect.Left, rect.Top, diam, diam, 180, 90)
            path.AddArc(rect.Right - diam, rect.Top, diam, diam, 270, 90)
            path.AddArc(rect.Right - diam, rect.Bottom - diam, diam, diam, 0, 90)
            path.AddArc(rect.Left, rect.Bottom - diam, diam, diam, 90, 90)
            path.CloseFigure()
        Else
            path.AddRectangle(rect)
        End If
        Return path
    End Function

End Class
