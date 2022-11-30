Imports System.ComponentModel
Imports System.Security.Permissions
    Public Class MetroComboBox
        Inherits ComboBox

        '<SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.UnmanagedCode)>
        'Public Class CustomComboBox
        '    Inherits ComboBox

        Public Sub New()
            MyBase.New()
            m_sizeCombo = New Size(MyBase.DropDownWidth, MyBase.DropDownHeight)
            AddHandler Me.m_dropDown.Closing, New ToolStripDropDownClosingEventHandler(AddressOf m_dropDown_Closing)
        End Sub

        Private Sub m_dropDown_Closing(ByVal sender As Object, ByVal e As ToolStripDropDownClosingEventArgs)
            Me.Text = m_dropDownCtrl.Text
            m_lastHideTime = DateTime.Now
        End Sub

        Public Sub New(ByVal dropControl As Control)
            Me.New()
            DropDownControl = dropControl
        End Sub

    'Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    '    If disposing Then

    '        If m_dropDown IsNot Nothing Then
    '            m_dropDown.Dispose()
    '            m_dropDown = Nothing
    '        End If

    '        If m_dropDownHost IsNot Nothing Then
    '            m_dropDownHost.Dispose()
    '            m_dropDownHost = Nothing
    '        End If

    '        If m_timerAutoFocus IsNot Nothing Then
    '            m_timerAutoFocus.Dispose()
    '            m_timerAutoFocus = Nothing
    '        End If
    '    End If

    '    MyBase.Dispose(disposing)
    'End Sub

    Private Sub timerAutoFocus_Tick(ByVal sender As Object, ByVal e As EventArgs)
            If m_dropDownHost.IsOnDropDown AndAlso Not DropDownControl.Focused Then
                DropDownControl.Focus()
                m_timerAutoFocus.Enabled = False
            End If
        End Sub

        Private Sub m_dropDown_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
            m_lastHideTime = DateTime.Now
        End Sub

        Protected Overridable Function PrepareDropDown(ByVal control As Control) As Control
            If AllowResizeDropDown Then
                Return New ResizableDropDownContainer(control)
            Else
                Return control
            End If
        End Function

        Public Overridable Sub ShowDropDown()
            m_dropDownCtrl.Text = Me.Text
            If DropDownContainer IsNot Nothing AndAlso m_dropDownHost IsNot Nothing AndAlso Not IsDroppedDown Then

                AutoSizeDropDown()
                m_dropDown.Show(Me, 0, Me.Height)
                m_bDroppedDown = True

                If m_timerAutoFocus Is Nothing Then
                    m_timerAutoFocus = New Timer()
                    m_timerAutoFocus.Interval = 10
                    AddHandler Me.m_timerAutoFocus.Tick, New EventHandler(AddressOf timerAutoFocus_Tick)
                End If

                m_timerAutoFocus.Enabled = True
                m_sShowTime = DateTime.Now
            End If
        End Sub

        Public Overridable Sub HideDropDown()

            If DropDownContainer IsNot Nothing AndAlso m_dropDownHost IsNot Nothing AndAlso IsDroppedDown Then
                m_dropDown.Hide()
                m_bDroppedDown = False
                If m_timerAutoFocus IsNot Nothing AndAlso m_timerAutoFocus.Enabled Then m_timerAutoFocus.Enabled = False
            End If
        End Sub

        Protected Sub AutoSizeDropDown()
            If m_dropDownHost IsNot Nothing Then
                m_dropDownHost.Margin = CSharpImpl.__Assign(m_dropDownHost.Padding, New Padding(0))

                Select Case DropDownSizeMode
                    Case SizeMode.UseComboSize
                        DropDownContainer.Size = New Size(Width, m_sizeCombo.Height)
                    Case SizeMode.UseControlSize
                        DropDownContainer.Size = New Size(m_sizeOriginal.Width, m_sizeOriginal.Height)
                    Case SizeMode.UseDropDownSize
                        DropDownContainer.Size = m_sizeCombo
                End Select
            End If
        End Sub

        Protected Overridable Sub AssignControl(ByVal control As Control)
            If control IsNot DropDownControl Then
                m_sizeOriginal = control.Size
                m_dropDownCtrl = control
                UpdateDropDownHost(control)
            End If
            'If TypeOf control Is RichTextBox Then
            '    Dim rtb As RichTextBox = control
            '    AddHandler rtb.TextChanged, AddressOf Me.OnRTBTextChanged
            'End If
        End Sub

        'Private Sub OnRTBTextChanged(sender As Object, e As EventArgs)
        '    Me.Text = m_dropDownCtrl.Text
        'End Sub

        Protected Sub UpdateDropDownHost(ByVal control As Control)
            m_dropDown.Items.Clear()
            Dim prevControlSize As Size = control.Size

            If DropDownContainer IsNot Nothing AndAlso DropDownContainer IsNot DropDownControl Then
                m_dropDownCnt.Dispose()
                m_dropDownCnt = Nothing
            End If

            m_dropDownCnt = PrepareDropDown(control)

            If m_dropDownHost IsNot Nothing Then
                m_dropDownHost.Dispose()
                m_dropDownHost = Nothing
            End If

            m_dropDownHost = New ToolStripControlHost(DropDownContainer)
            AddHandler Me.m_dropDownHost.LostFocus, New EventHandler(AddressOf m_dropDown_LostFocus)
            m_dropDownHost.AutoSize = False
            m_dropDown.Items.Add(m_dropDownHost)
            Dim dropDownCnt As IDropDownContainer = TryCast(DropDownContainer, IDropDownContainer)

            If dropDownCnt IsNot Nothing Then
                m_sizeOriginal = dropDownCnt.CalculateContainerSize(prevControlSize)
            Else
                m_sizeOriginal = prevControlSize
            End If

            AutoSizeDropDown()
        End Sub

        Public Const WM_COMMAND As UInteger = &H111
        Public Const WM_USER As UInteger = &H400
        Public Const WM_REFLECT As UInteger = WM_USER + &H1C00
        Public Const WM_LBUTTONDOWN As UInteger = &H201
        Public Const CBN_DROPDOWN As UInteger = 7
        Public Const CBN_CLOSEUP As UInteger = 8

        Public Shared Function HIWORD(ByVal n As Integer) As UInteger
            ''' Cannot convert ReturnStatementSyntax, System.ArgumentOutOfRangeException: Exception of type 'System.ArgumentOutOfRangeException' was thrown.
            ''' Parameter name: op
            ''' Actual value was RightShiftExpression.
            '''    at ICSharpCode.CodeConverter.Util.VBUtil.GetExpressionOperatorTokenKind(SyntaxKind op)
            '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitBinaryExpression(BinaryExpressionSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
            '''    at ICSharpCode.CodeConverter.VB.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.VisitBinaryExpression(BinaryExpressionSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
            '''    at ICSharpCode.CodeConverter.VB.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitCastExpression(CastExpressionSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
            '''    at ICSharpCode.CodeConverter.VB.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.VisitCastExpression(CastExpressionSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitBinaryExpression(BinaryExpressionSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
            '''    at ICSharpCode.CodeConverter.VB.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.VisitBinaryExpression(BinaryExpressionSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at ICSharpCode.CodeConverter.VB.MethodBodyVisitor.VisitReturnStatement(ReturnStatementSyntax node)
            '''    at Microsoft.CodeAnalysis.CSharp.Syntax.ReturnStatementSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
            '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
            '''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.ConvertWithTrivia(SyntaxNode node)
            '''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
            ''' 
            ''' Input: 
            '''             return (uint)(n >> 16) & 0xffff;
        End Function
        Public Overrides Function PreProcessMessage(ByRef m As Message) As Boolean
            If m.Msg = (WM_REFLECT + WM_COMMAND) Then
                If HIWORD(CInt(m.WParam)) = CBN_DROPDOWN Then Return False
            End If

            Return MyBase.PreProcessMessage(m)
        End Function

        Private Shared m_sShowTime As DateTime = DateTime.Now

        Private Sub AutoDropDown()
            If m_dropDown IsNot Nothing AndAlso m_dropDown.Visible Then
                HideDropDown()
            ElseIf (DateTime.Now - m_lastHideTime).Milliseconds > 50 Then
                ShowDropDown()
            End If
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            If m.Msg = WM_LBUTTONDOWN Then
                AutoDropDown()
                Return
            End If
            If m.Msg = (WM_REFLECT + WM_COMMAND) Then

                Select Case HIWORD(CInt(m.WParam))

                    Case CBN_DROPDOWN
                        AutoDropDown()
                        Return
                    Case CBN_CLOSEUP
                        If (DateTime.Now - m_sShowTime).Seconds > 1 Then
                            HideDropDown()
                        End If
                        Return
                End Select
            End If

            MyBase.WndProc(m)
        End Sub

        Public Enum SizeMode
            UseComboSize
            UseControlSize
            UseDropDownSize
        End Enum

        <Browsable(False)>
        Public ReadOnly Property DropDownContainer As Control
            Get
                Return Me.m_dropDownCnt
            End Get
        End Property

        <Browsable(False)>
        Public Property DropDownControl As Control
            Get
                Return m_dropDownCtrl
            End Get
            Set(ByVal value As Control)
                AssignControl(value)
            End Set
        End Property

        <Browsable(False)>
        Public ReadOnly Property IsDroppedDown As Boolean
            Get
                Return Me.m_bDroppedDown AndAlso DropDownContainer.Visible
            End Get
        End Property

        <Category("Custom Drop-Down"), Description("Indicates if drop-down is resizable.")>
        Public Property AllowResizeDropDown As Boolean
            Get
                Return Me.m_bIsResizable
            End Get
            Set(ByVal value As Boolean)

                If value <> Me.m_bIsResizable Then
                    Me.m_bIsResizable = value
                    If m_dropDownHost IsNot Nothing Then UpdateDropDownHost(DropDownControl)
                End If
            End Set
        End Property

        <Category("Custom Drop-Down"), Description("Indicates current sizing mode."), DefaultValue(SizeMode.UseComboSize)>
        Public Property DropDownSizeMode As SizeMode
            Get
                Return Me.m_sizeMode
            End Get
            Set(ByVal value As SizeMode)

                If value <> Me.m_sizeMode Then
                    Me.m_sizeMode = value
                    AutoSizeDropDown()
                End If
            End Set
        End Property

        <Category("Custom Drop-Down")>
        Public Property DropSize As Size
            Get
                Return m_sizeCombo
            End Get
            Set(ByVal value As Size)
                m_sizeCombo = value
                If DropDownSizeMode = SizeMode.UseDropDownSize Then AutoSizeDropDown()
            End Set
        End Property

        <Category("Custom Drop-Down"), Browsable(False)>
        Public Property ControlSize As Size
            Get
                Return m_sizeOriginal
            End Get
            Set(ByVal value As Size)
                m_sizeOriginal = value
                If DropDownSizeMode = SizeMode.UseControlSize Then AutoSizeDropDown()
            End Set
        End Property

        <Browsable(False)>
        Public Overloads ReadOnly Property Items As ObjectCollection
            Get
                Return MyBase.Items
            End Get
        End Property

        <Browsable(False)>
        Public Overloads Property ItemHeight As Integer
            Get
                Return MyBase.ItemHeight
            End Get
            Set(ByVal value As Integer)
                MyBase.ItemHeight = value
            End Set
        End Property

        Private m_dropDownHost As ToolStripControlHost
        Private m_dropDown As ToolStripDropDown = New ToolStripDropDown()
        Private m_dropDownCnt As Control
        Private m_dropDownCtrl As Control
        Private m_bDroppedDown As Boolean = False
        Private m_sizeMode As SizeMode = SizeMode.UseComboSize
        Private m_lastHideTime As DateTime = DateTime.Now
        Private m_timerAutoFocus As Timer
        Private m_sizeOriginal As Size = New Size(1, 1)
        Private m_sizeCombo As Size
        Private m_bIsResizable As Boolean = True

    Public Sub Hide_Click()
            If m_dropDownCtrl.Visible Then
                HideDropDown()
            End If
        End Sub
        Public Sub Show_Click()
            If m_dropDownCtrl.Visible = False Then
                ShowDropDown()
            End If
        End Sub
        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class

    Interface IDropDownContainer
        Function CalculateContainerSize(ByVal controlSize As Size) As Size
    End Interface

    Public Class ResizableDropDownContainer
        Inherits Control
        Implements IDropDownContainer

        Public Sub New(ByVal childControl As Control)
            If childControl Is Nothing Then Throw New NullReferenceException()
            Me.Controls.Add(childControl)
            m_ctrlChild = childControl
            Me.MinimumSize = New Size(childControl.MinimumSize.Width, childControl.MinimumSize.Height + 18)
        End Sub

        Public Function CalculateContainerSize(ByVal controlSize As Size) As Size
            controlSize.Height += 18
            Return controlSize
        End Function

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            MyBase.OnPaint(e)
            Dim rectGrip As Rectangle = New Rectangle(Right - 16, Bottom - 16, 16, 16)
            ControlPaint.DrawSizeGrip(e.Graphics, BackColor, rectGrip)
        End Sub

        Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
            MyBase.OnSizeChanged(e)
            ChildControl.Left = CSharpImpl.__Assign(ChildControl.Top, 0)
            ChildControl.Width = DisplayRectangle.Width
            ChildControl.Height = DisplayRectangle.Height - 18
        End Sub

        Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            If e.Button = MouseButtons.Left Then
                Dim rectGrip As Rectangle = CalculateGripRectangle()

                If rectGrip.Contains(e.Location) Then
                    m_bDragGrip = CSharpImpl.__Assign(Capture, True)
                    UpdateCursor(e.Location)
                    m_ptAnchor = e.Location
                End If
            End If
        End Sub

        Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
            MyBase.OnMouseMove(e)

            If e.Button = MouseButtons.Left Then

                If IsDraggingGrip Then
                    Size = New Size(Width + e.X - m_ptAnchor.X, Height + e.Y - m_ptAnchor.Y)
                    m_ptAnchor = e.Location
                    Invalidate()
                End If
            End If

            If Not IsDraggingGrip Then UpdateCursor(e.Location)
        End Sub

        Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            m_bDragGrip = CSharpImpl.__Assign(Capture, False)
            UpdateCursor(e.Location)
        End Sub

        Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
            MyBase.OnMouseLeave(e)
            Cursor = Cursors.[Default]
        End Sub

        Protected Sub UpdateCursor(ByVal location As Point)
            Dim rectGrip As Rectangle = CalculateGripRectangle()

            If rectGrip.Contains(location) Then
                Cursor = Cursors.SizeNWSE
            Else
                Cursor = Cursors.[Default]
            End If
        End Sub

        Protected Function CalculateGripRectangle() As Rectangle
            Return New Rectangle(Right - 16, Bottom - 16, 16, 16)
        End Function

        Private Function IDropDownContainer_CalculateContainerSize(controlSize As Size) As Size Implements IDropDownContainer.CalculateContainerSize
            'Throw New NotImplementedException()
        End Function

        <Browsable(False)>
        Public ReadOnly Property ChildControl As Control
            Get
                Return Me.m_ctrlChild
            End Get
        End Property

        Public ReadOnly Property IsDraggingGrip As Boolean
            Get
                Return Me.m_bDragGrip
            End Get
        End Property

        Private m_ctrlChild As Control
        Private m_ptAnchor As Point
        Private m_bDragGrip As Boolean = False

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
        'End Class
    End Class

