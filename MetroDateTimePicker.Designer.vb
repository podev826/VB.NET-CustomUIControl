<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MetroDateTimePicker
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MetroDateTimePicker))
        Me.DomainUpDown1 = New System.Windows.Forms.DomainUpDown()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.SuspendLayout()
        '
        'DomainUpDown1
        '
        Me.DomainUpDown1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DomainUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DomainUpDown1.Location = New System.Drawing.Point(277, 1)
        Me.DomainUpDown1.Margin = New System.Windows.Forms.Padding(0)
        Me.DomainUpDown1.Name = "DomainUpDown1"
        Me.DomainUpDown1.Size = New System.Drawing.Size(21, 19)
        Me.DomainUpDown1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(260, 1)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(17, 17)
        Me.Button1.TabIndex = 1
        Me.Button1.TabStop = False
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Location = New System.Drawing.Point(-1, -1)
        Me.DateTimePicker2.Margin = New System.Windows.Forms.Padding(0)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(302, 23)
        Me.DateTimePicker2.TabIndex = 1
        '
        'MetroDateTimePicker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DomainUpDown1)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Name = "MetroDateTimePicker"
        Me.Size = New System.Drawing.Size(300, 21)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DomainUpDown1 As DomainUpDown
    Friend WithEvents Button1 As Button
    Friend WithEvents DateTimePicker2 As DateTimePicker
End Class
