<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CreditNote
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Label2 As System.Windows.Forms.Label
        Dim Label20 As System.Windows.Forms.Label
        Dim Label1 As System.Windows.Forms.Label
        Me.cmbAgent = New System.Windows.Forms.ComboBox
        Me.cmbTerms = New System.Windows.Forms.ComboBox
        Me.dtpDate = New System.Windows.Forms.DateTimePicker
        Me.btnPrint = New System.Windows.Forms.Button
        Label2 = New System.Windows.Forms.Label
        Label20 = New System.Windows.Forms.Label
        Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmbAgent
        '
        Me.cmbAgent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAgent.FormattingEnabled = True
        Me.cmbAgent.Location = New System.Drawing.Point(147, 35)
        Me.cmbAgent.Name = "cmbAgent"
        Me.cmbAgent.Size = New System.Drawing.Size(109, 21)
        Me.cmbAgent.TabIndex = 117
        '
        'Label2
        '
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(3, 38)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(151, 13)
        Label2.TabIndex = 116
        Label2.Text = "Agent ID . . . . . . . . . . . . . . "
        '
        'cmbTerms
        '
        Me.cmbTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTerms.FormattingEnabled = True
        Me.cmbTerms.Location = New System.Drawing.Point(147, 35)
        Me.cmbTerms.Name = "cmbTerms"
        Me.cmbTerms.Size = New System.Drawing.Size(303, 21)
        Me.cmbTerms.TabIndex = 114
        Me.cmbTerms.Visible = False
        '
        'Label20
        '
        Label20.AutoSize = True
        Label20.Location = New System.Drawing.Point(3, 38)
        Label20.Name = "Label20"
        Label20.Size = New System.Drawing.Size(163, 13)
        Label20.TabIndex = 113
        Label20.Text = "Product. . . . . . . . . . . . . . . . . "
        Label20.Visible = False
        '
        'dtpDate
        '
        Me.dtpDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDate.Location = New System.Drawing.Point(147, 9)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(109, 21)
        Me.dtpDate.TabIndex = 112
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(92, 78)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint.TabIndex = 111
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(3, 9)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(156, 13)
        Label1.TabIndex = 115
        Label1.Text = "Date. . . . . . . . . . . . . . . . . . "
        '
        'CreditNote
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(265, 115)
        Me.Controls.Add(Me.cmbAgent)
        Me.Controls.Add(Label2)
        Me.Controls.Add(Me.cmbTerms)
        Me.Controls.Add(Label20)
        Me.Controls.Add(Me.dtpDate)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "CreditNote"
        Me.Text = "Credit Note"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbAgent As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTerms As System.Windows.Forms.ComboBox
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnPrint As System.Windows.Forms.Button
End Class
