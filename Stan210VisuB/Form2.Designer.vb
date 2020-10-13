<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.components = New System.ComponentModel.Container()
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("")
        Me.StopUpdate = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.eucount = New System.Windows.Forms.Label()
        Me.DeleteIngotID = New System.Windows.Forms.TextBox()
        Me.btnDeleteIngot = New System.Windows.Forms.Button()
        Me.cr_th = New System.Windows.Forms.ComboBox()
        Me.f2_thshow = New System.Windows.Forms.CheckBox()
        Me.DntShowMP = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StopUpdate
        '
        Me.StopUpdate.AutoSize = True
        Me.StopUpdate.Location = New System.Drawing.Point(0, 3)
        Me.StopUpdate.Name = "StopUpdate"
        Me.StopUpdate.Size = New System.Drawing.Size(149, 17)
        Me.StopUpdate.TabIndex = 38
        Me.StopUpdate.Text = "Остановить обновление"
        Me.StopUpdate.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.ListView1)
        Me.Panel1.Location = New System.Drawing.Point(0, 26)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1034, 481)
        Me.Panel1.TabIndex = 40
        '
        'ListView1
        '
        Me.ListView1.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1})
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(1034, 481)
        Me.ListView1.TabIndex = 38
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.List
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(162, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 41
        Me.Label1.Text = "Количество ЕУ"
        '
        'eucount
        '
        Me.eucount.AutoSize = True
        Me.eucount.Location = New System.Drawing.Point(252, 4)
        Me.eucount.Name = "eucount"
        Me.eucount.Size = New System.Drawing.Size(28, 13)
        Me.eucount.TabIndex = 42
        Me.eucount.Text = "XXX"
        '
        'DeleteIngotID
        '
        Me.DeleteIngotID.Location = New System.Drawing.Point(494, 5)
        Me.DeleteIngotID.Name = "DeleteIngotID"
        Me.DeleteIngotID.Size = New System.Drawing.Size(76, 20)
        Me.DeleteIngotID.TabIndex = 47
        '
        'btnDeleteIngot
        '
        Me.btnDeleteIngot.Location = New System.Drawing.Point(576, 3)
        Me.btnDeleteIngot.Name = "btnDeleteIngot"
        Me.btnDeleteIngot.Size = New System.Drawing.Size(65, 22)
        Me.btnDeleteIngot.TabIndex = 48
        Me.btnDeleteIngot.Text = "Удалить"
        Me.btnDeleteIngot.UseVisualStyleBackColor = True
        '
        'cr_th
        '
        Me.cr_th.FormattingEnabled = True
        Me.cr_th.Items.AddRange(New Object() {"0 - резерв", "1 - передаточный стол", "2 - конвейер перед печами", "3 - печь", "4 - резерв", "5 - прокатка", "6 - вертикальный конвейер приемный", "7 - вертикальный конвейер нижний", "8 - резерв", "9 -вертикальный конвейер выходной", "10 - вертикальный конвейер верхний", "11- тележка сьема с кантователя", "12 - горизонтальный конвейер приемный", "13- горизонтальный конвейер верхний", "14- горизонтальный конвейер обвзяки", "15 - горизонтальный конвейер выходной", "16 - подвесная тележка", "17 - складирующий цепной шлеппер"})
        Me.cr_th.Location = New System.Drawing.Point(726, 2)
        Me.cr_th.MaxDropDownItems = 30
        Me.cr_th.Name = "cr_th"
        Me.cr_th.Size = New System.Drawing.Size(308, 21)
        Me.cr_th.TabIndex = 186
        Me.cr_th.Text = "выберите"
        '
        'f2_thshow
        '
        Me.f2_thshow.AutoSize = True
        Me.f2_thshow.Location = New System.Drawing.Point(669, 6)
        Me.f2_thshow.Name = "f2_thshow"
        Me.f2_thshow.Size = New System.Drawing.Size(51, 17)
        Me.f2_thshow.TabIndex = 187
        Me.f2_thshow.Text = "Нить"
        Me.f2_thshow.UseVisualStyleBackColor = True
        '
        'DntShowMP
        '
        Me.DntShowMP.AutoSize = True
        Me.DntShowMP.Location = New System.Drawing.Point(286, 3)
        Me.DntShowMP.Name = "DntShowMP"
        Me.DntShowMP.Size = New System.Drawing.Size(130, 17)
        Me.DntShowMP.TabIndex = 46
        Me.DntShowMP.Text = "Не показывать печь"
        Me.DntShowMP.UseVisualStyleBackColor = True
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1034, 507)
        Me.Controls.Add(Me.f2_thshow)
        Me.Controls.Add(Me.cr_th)
        Me.Controls.Add(Me.btnDeleteIngot)
        Me.Controls.Add(Me.DeleteIngotID)
        Me.Controls.Add(Me.DntShowMP)
        Me.Controls.Add(Me.eucount)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.StopUpdate)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StopUpdate As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents eucount As System.Windows.Forms.Label
    Friend WithEvents DeleteIngotID As System.Windows.Forms.TextBox
    Friend WithEvents btnDeleteIngot As System.Windows.Forms.Button
    Friend WithEvents cr_th As System.Windows.Forms.ComboBox
    Friend WithEvents f2_thshow As System.Windows.Forms.CheckBox
    Friend WithEvents DntShowMP As System.Windows.Forms.CheckBox
End Class
