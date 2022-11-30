
Imports System.ComponentModel

Public Class MetroTextBox
    Inherits TextBox

    Public Sub New()
        SetStyle(ControlStyles.UserPaint, True)
    End Sub

    Private m_borderColor As Color
    Private m_borderThickness As Integer
    Private m_shadowColor As Color
    Private m_shadowThickness As Integer
    Private m_borderRound As Integer


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


    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Dim g As Graphics = Parent.CreateGraphics
        Dim p As Pen = New Pen(m_borderColor, m_borderThickness)
        If (m_borderThickness > 0) Then
            If (m_borderRound > 0) Then
                DrawRoundedRectangle(
                    New Pen(m_borderColor, m_borderThickness),
                    g,
                    CSng(Location.X - m_borderThickness / 2),
                    CSng(Location.Y - m_borderThickness / 2),
                    Width + m_borderThickness,
                    Height + m_borderThickness,
                    m_borderRound
                )
            Else
                g.DrawRectangle(
                    New Pen(m_borderColor, m_borderThickness),
                    CSng(Location.X - m_borderThickness / 2),
                    CSng(Location.Y - m_borderThickness / 2),
                    Width + m_borderThickness,
                    Height + m_borderThickness
                )
            End If
        End If
        MyBase.OnPaint(e)
    End Sub

    Public Sub DrawRoundedRectangle(ByVal p As Pen,
                                ByVal objGraphics As Graphics,
                                ByVal m_intxAxis As Single,
                                ByVal m_intyAxis As Single,
                                ByVal m_intWidth As Single,
                                ByVal m_intHeight As Single,
                                ByVal m_diameter As Single)
        'Dim g As Graphics
        Dim BaseRect As New RectangleF(m_intxAxis, m_intyAxis, m_intWidth,
                                      m_intHeight)

        m_diameter = Math.Min(m_diameter, Math.Min(m_intHeight, m_intWidth))

        objGraphics.DrawLine(p, m_intxAxis + CSng(m_diameter / 2),
                             m_intyAxis,
                             m_intxAxis + m_intWidth - CSng(m_diameter / 2),
                             m_intyAxis)
        objGraphics.DrawLine(p, m_intxAxis + m_intWidth,
                             m_intyAxis + CSng(m_diameter / 2),
                             m_intxAxis + m_intWidth,
                             m_intyAxis + m_intHeight - CSng(m_diameter / 2))
        objGraphics.DrawLine(p, m_intxAxis + CSng(m_diameter / 2),
                             m_intyAxis + m_intHeight,
                             m_intxAxis + m_intWidth - CSng(m_diameter / 2),
                             m_intyAxis + m_intHeight)
        objGraphics.DrawLine(p,
                             m_intxAxis, m_intyAxis + CSng(m_diameter / 2),
                             m_intxAxis,
                             m_intyAxis + m_intHeight - CSng(m_diameter / 2))
        Dim ArcRect As New RectangleF(BaseRect.Location,
                                      New SizeF(m_diameter, m_diameter))
        'top left Arc
        objGraphics.DrawArc(p, ArcRect, 180, 90)
        ' top right arc
        ArcRect.X = BaseRect.Right - m_diameter
        objGraphics.DrawArc(p, ArcRect, 270, 90)
        ' bottom right arc
        ArcRect.Y = BaseRect.Bottom - m_diameter
        objGraphics.DrawArc(p, ArcRect, 0, 90)
        ' bottom left arc
        ArcRect.X = BaseRect.Left
        objGraphics.DrawArc(p, ArcRect, 90, 90)
    End Sub
End Class
