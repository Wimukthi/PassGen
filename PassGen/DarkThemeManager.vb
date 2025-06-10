Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Class DarkThemeManager
    ' Windows API declarations for native theming
    <DllImport("dwmapi.dll", PreserveSig:=False)>
    Private Shared Sub DwmSetWindowAttribute(hwnd As IntPtr, attr As Integer, ByRef attrValue As Integer, attrSize As Integer)
    End Sub

    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function SetWindowTheme(hwnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetWindowDC(hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
    End Function

    ' Color scheme definition
    Public Structure DarkColorScheme
        Public BackgroundPrimary As Color
        Public BackgroundSecondary As Color
        Public BackgroundTertiary As Color
        Public TextPrimary As Color
        Public TextSecondary As Color
        Public Border As Color
        Public Accent As Color
        Public AccentHover As Color
        Public AccentPressed As Color
        Public Success As Color
        Public Warning As Color
        Public ErrorColor As Color
    End Structure

    Private Shared ReadOnly DefaultDark As DarkColorScheme = New DarkColorScheme With {
        .BackgroundPrimary = Color.FromArgb(32, 32, 32),
        .BackgroundSecondary = Color.FromArgb(45, 45, 45),
        .BackgroundTertiary = Color.FromArgb(60, 60, 60),
        .TextPrimary = Color.FromArgb(255, 255, 255),
        .TextSecondary = Color.FromArgb(200, 200, 200),
        .Border = Color.FromArgb(80, 80, 80),
        .Accent = Color.FromArgb(0, 120, 215),
        .AccentHover = Color.FromArgb(70, 70, 70),
        .AccentPressed = Color.FromArgb(90, 90, 90),
        .Success = Color.FromArgb(16, 124, 16),
        .Warning = Color.FromArgb(255, 140, 0),
        .ErrorColor = Color.FromArgb(232, 17, 35)
    }

    Private _colorScheme As DarkColorScheme
    Private _appliedForms As New HashSet(Of Form)
    Private _customPaintHandlers As New Dictionary(Of Control, List(Of PaintEventHandler))

    Public Sub New(Optional colorScheme As DarkColorScheme? = Nothing)
        _colorScheme = If(colorScheme, DefaultDark)
    End Sub

    Public Sub ApplyDarkTheme(form As Form)
        If _appliedForms.Contains(form) Then Return
        _appliedForms.Add(form)

        ' Apply native dark title bar if supported (Windows 10 1809+)
        Try
            Dim darkMode As Integer = 1
            DwmSetWindowAttribute(form.Handle, 20, darkMode, 4) ' DWMWA_USE_IMMERSIVE_DARK_MODE
        Catch
            ' Fallback for older Windows versions
        End Try

        ' Set form properties
        form.BackColor = _colorScheme.BackgroundPrimary
        form.ForeColor = _colorScheme.TextPrimary

        ' Handle form events
        AddHandler form.FormClosed, AddressOf OnFormClosed

        ' Apply theme to all controls recursively
        ApplyThemeToControl(form)

        ' Handle dynamic control additions
        AddHandler form.ControlAdded, AddressOf OnControlAdded
    End Sub

    Private Sub OnFormClosed(sender As Object, e As FormClosedEventArgs)
        Dim form = DirectCast(sender, Form)
        _appliedForms.Remove(form)
        RemoveHandler form.FormClosed, AddressOf OnFormClosed
        RemoveHandler form.ControlAdded, AddressOf OnControlAdded
    End Sub

    Private Sub OnControlAdded(sender As Object, e As ControlEventArgs)
        ApplyThemeToControl(e.Control)
    End Sub

    Private Sub ApplyThemeToControl(control As Control)
        If control Is Nothing Then Return

        Select Case True
            Case TypeOf control Is Form
                ApplyFormTheme(DirectCast(control, Form))
            Case TypeOf control Is Button
                ApplyButtonTheme(DirectCast(control, Button))
            Case TypeOf control Is TextBox
                ApplyTextBoxTheme(DirectCast(control, TextBox))
            Case TypeOf control Is ComboBox
                ApplyComboBoxTheme(DirectCast(control, ComboBox))
            Case TypeOf control Is ListBox
                ApplyListBoxTheme(DirectCast(control, ListBox))
            Case TypeOf control Is TreeView
                ApplyTreeViewTheme(DirectCast(control, TreeView))
            Case TypeOf control Is ListView
                ApplyListViewTheme(DirectCast(control, ListView))
            Case TypeOf control Is DataGridView
                ApplyDataGridViewTheme(DirectCast(control, DataGridView))
            Case TypeOf control Is Panel
                ApplyPanelTheme(DirectCast(control, Panel))
            Case TypeOf control Is GroupBox
                ApplyGroupBoxTheme(DirectCast(control, GroupBox))
            Case TypeOf control Is TabControl
                ApplyTabControlTheme(DirectCast(control, TabControl))
            Case TypeOf control Is MenuStrip
                ApplyMenuStripTheme(DirectCast(control, MenuStrip))
            Case TypeOf control Is StatusStrip
                ApplyStatusStripTheme(DirectCast(control, StatusStrip))
            Case TypeOf control Is ToolStrip
                ApplyToolStripTheme(DirectCast(control, ToolStrip))
            Case TypeOf control Is CheckBox
                ApplyCheckBoxTheme(DirectCast(control, CheckBox))
            Case TypeOf control Is RadioButton
                ApplyRadioButtonTheme(DirectCast(control, RadioButton))
            Case TypeOf control Is Label
                ApplyLabelTheme(DirectCast(control, Label))
            Case TypeOf control Is NumericUpDown
                ApplyNumericUpDownTheme(DirectCast(control, NumericUpDown))
            Case TypeOf control Is DateTimePicker
                ApplyDateTimePickerTheme(DirectCast(control, DateTimePicker))
            Case TypeOf control Is ProgressBar
                ApplyProgressBarTheme(DirectCast(control, ProgressBar))
            Case TypeOf control Is RichTextBox
                ApplyRichTextBoxTheme(DirectCast(control, RichTextBox))
            Case TypeOf control Is LinkLabel
                ApplyLinkLabelTheme(DirectCast(control, LinkLabel))
            Case TypeOf control Is CheckedListBox
                ApplyCheckedListBoxTheme(DirectCast(control, CheckedListBox))
            Case TypeOf control Is TrackBar
                ApplyTrackBarTheme(DirectCast(control, TrackBar))
            Case Else
                ApplyGenericControlTheme(control)
        End Select

        ' Recursively apply to child controls
        For Each childControl As Control In control.Controls
            ApplyThemeToControl(childControl)
        Next
    End Sub

    Private Sub ApplyFormTheme(form As Form)
        form.BackColor = _colorScheme.BackgroundPrimary
        form.ForeColor = _colorScheme.TextPrimary
    End Sub

    Private Sub ApplyButtonTheme(button As Button)
        button.BackColor = _colorScheme.BackgroundSecondary
        button.ForeColor = _colorScheme.TextPrimary
        button.FlatStyle = FlatStyle.Flat
        button.FlatAppearance.BorderColor = _colorScheme.Border
        button.FlatAppearance.BorderSize = 1
        button.FlatAppearance.MouseOverBackColor = _colorScheme.AccentHover
        button.FlatAppearance.MouseDownBackColor = _colorScheme.AccentPressed
        button.UseVisualStyleBackColor = False
    End Sub

    Private Sub ApplyTextBoxTheme(textBox As TextBox)
        textBox.BackColor = _colorScheme.BackgroundSecondary
        textBox.ForeColor = _colorScheme.TextPrimary
        textBox.BorderStyle = BorderStyle.FixedSingle

        SetCustomPaint(textBox, AddressOf PaintTextBox)
    End Sub

    Private Sub ApplyComboBoxTheme(comboBox As ComboBox)
        comboBox.BackColor = _colorScheme.BackgroundSecondary
        comboBox.ForeColor = _colorScheme.TextPrimary
        comboBox.FlatStyle = FlatStyle.Flat

        SetCustomPaint(comboBox, AddressOf PaintComboBox)
    End Sub

    Private Sub ApplyListBoxTheme(listBox As ListBox)
        listBox.BackColor = _colorScheme.BackgroundSecondary
        listBox.ForeColor = _colorScheme.TextPrimary
        listBox.BorderStyle = BorderStyle.FixedSingle

        SetCustomPaint(listBox, AddressOf PaintListBox)
    End Sub

    Private Sub ApplyTreeViewTheme(treeView As TreeView)
        treeView.BackColor = _colorScheme.BackgroundSecondary
        treeView.ForeColor = _colorScheme.TextPrimary
        treeView.BorderStyle = BorderStyle.FixedSingle
        treeView.LineColor = _colorScheme.Border

        ' Disable visual styles to prevent white artifacts
        SetWindowTheme(treeView.Handle, "", "")
    End Sub

    Private Sub ApplyListViewTheme(listView As ListView)
        listView.BackColor = _colorScheme.BackgroundSecondary
        listView.ForeColor = _colorScheme.TextPrimary
        listView.BorderStyle = BorderStyle.FixedSingle

        SetWindowTheme(listView.Handle, "", "")
    End Sub

    Private Sub ApplyDataGridViewTheme(dgv As DataGridView)
        dgv.BackgroundColor = _colorScheme.BackgroundPrimary
        dgv.GridColor = _colorScheme.Border
        dgv.BorderStyle = BorderStyle.FixedSingle

        ' Style default cell style
        With dgv.DefaultCellStyle
            .BackColor = _colorScheme.BackgroundSecondary
            .ForeColor = _colorScheme.TextPrimary
            .SelectionBackColor = _colorScheme.Accent
            .SelectionForeColor = _colorScheme.TextPrimary
        End With

        ' Style headers
        With dgv.ColumnHeadersDefaultCellStyle
            .BackColor = _colorScheme.BackgroundTertiary
            .ForeColor = _colorScheme.TextPrimary
            .SelectionBackColor = _colorScheme.BackgroundTertiary
            .SelectionForeColor = _colorScheme.TextPrimary
        End With

        With dgv.RowHeadersDefaultCellStyle
            .BackColor = _colorScheme.BackgroundTertiary
            .ForeColor = _colorScheme.TextPrimary
            .SelectionBackColor = _colorScheme.BackgroundTertiary
            .SelectionForeColor = _colorScheme.TextPrimary
        End With



        dgv.EnableHeadersVisualStyles = False
    End Sub

    Private Sub ApplyPanelTheme(panel As Panel)
        panel.BackColor = _colorScheme.BackgroundPrimary
        panel.ForeColor = _colorScheme.TextPrimary
    End Sub

    Private Sub ApplyGroupBoxTheme(groupBox As GroupBox)
        groupBox.BackColor = _colorScheme.BackgroundPrimary
        groupBox.ForeColor = _colorScheme.TextPrimary

        SetCustomPaint(groupBox, AddressOf PaintGroupBox)
    End Sub

    Private Sub ApplyTabControlTheme(tabControl As TabControl)
        tabControl.BackColor = _colorScheme.BackgroundPrimary
        tabControl.ForeColor = _colorScheme.TextPrimary

        ' Apply theme to tab pages
        For Each tabPage As TabPage In tabControl.TabPages
            tabPage.BackColor = _colorScheme.BackgroundPrimary
            tabPage.ForeColor = _colorScheme.TextPrimary
        Next

        SetCustomPaint(tabControl, AddressOf PaintTabControl)
    End Sub

    Private Sub ApplyMenuStripTheme(menuStrip As MenuStrip)
        menuStrip.BackColor = _colorScheme.BackgroundSecondary
        menuStrip.ForeColor = _colorScheme.TextPrimary
        menuStrip.RenderMode = ToolStripRenderMode.Professional
        menuStrip.Renderer = New DarkToolStripRenderer(_colorScheme)

        ' Apply theme to all menu items recursively
        ApplyMenuItemsTheme(menuStrip.Items)
    End Sub

    Private Sub ApplyMenuItemsTheme(items As ToolStripItemCollection)
        For Each item As ToolStripItem In items
            item.BackColor = _colorScheme.BackgroundSecondary
            item.ForeColor = _colorScheme.TextPrimary

            If TypeOf item Is ToolStripMenuItem Then
                Dim menuItem = DirectCast(item, ToolStripMenuItem)
                If menuItem.HasDropDownItems Then
                    menuItem.DropDown.BackColor = _colorScheme.BackgroundSecondary
                    menuItem.DropDown.ForeColor = _colorScheme.TextPrimary
                    ApplyMenuItemsTheme(menuItem.DropDownItems)
                End If
            End If
        Next
    End Sub

    Private Sub ApplyStatusStripTheme(statusStrip As StatusStrip)
        statusStrip.BackColor = _colorScheme.BackgroundSecondary
        statusStrip.ForeColor = _colorScheme.TextPrimary
        statusStrip.RenderMode = ToolStripRenderMode.Professional
        statusStrip.Renderer = New DarkToolStripRenderer(_colorScheme)
    End Sub

    Private Sub ApplyToolStripTheme(toolStrip As ToolStrip)
        toolStrip.BackColor = _colorScheme.BackgroundSecondary
        toolStrip.ForeColor = _colorScheme.TextPrimary
        toolStrip.RenderMode = ToolStripRenderMode.Professional
        toolStrip.Renderer = New DarkToolStripRenderer(_colorScheme)
    End Sub

    Private Sub ApplyCheckBoxTheme(checkBox As CheckBox)
        checkBox.BackColor = _colorScheme.BackgroundPrimary
        checkBox.ForeColor = _colorScheme.TextPrimary
        checkBox.FlatStyle = FlatStyle.Flat
        checkBox.FlatAppearance.BorderColor = _colorScheme.Border
        checkBox.FlatAppearance.CheckedBackColor = _colorScheme.Accent
    End Sub

    Private Sub ApplyRadioButtonTheme(radioButton As RadioButton)
        radioButton.BackColor = _colorScheme.BackgroundPrimary
        radioButton.ForeColor = _colorScheme.TextPrimary
        radioButton.FlatStyle = FlatStyle.Flat
        radioButton.FlatAppearance.BorderColor = _colorScheme.Border
        radioButton.FlatAppearance.CheckedBackColor = _colorScheme.Accent
    End Sub

    Private Sub ApplyLabelTheme(label As Label)
        If label.BackColor = SystemColors.Control Then
            label.BackColor = Color.Transparent
        End If
        label.ForeColor = _colorScheme.TextPrimary
    End Sub

    Private Sub ApplyGenericControlTheme(control As Control)
        If control.BackColor = SystemColors.Control Then
            control.BackColor = _colorScheme.BackgroundPrimary
        End If
        If control.ForeColor = SystemColors.ControlText Then
            control.ForeColor = _colorScheme.TextPrimary
        End If
    End Sub

    Private Sub ApplyNumericUpDownTheme(numericUpDown As NumericUpDown)
        numericUpDown.BackColor = _colorScheme.BackgroundSecondary
        numericUpDown.ForeColor = _colorScheme.TextPrimary
        numericUpDown.BorderStyle = BorderStyle.FixedSingle
        ' Custom paint for buttons and border
        SetCustomPaint(numericUpDown, AddressOf PaintNumericUpDown)
    End Sub

    Private Sub ApplyDateTimePickerTheme(dateTimePicker As DateTimePicker)
        dateTimePicker.BackColor = _colorScheme.BackgroundSecondary
        dateTimePicker.ForeColor = _colorScheme.TextPrimary
        ' Calendar theming might be complex and require more specific handling
        ' For now, basic background/foreground.
        ' Custom paint for dropdown button and border
        SetCustomPaint(dateTimePicker, AddressOf PaintDateTimePicker)
    End Sub

    Private Sub ApplyProgressBarTheme(progressBar As ProgressBar)
        progressBar.BackColor = _colorScheme.BackgroundSecondary
        ' ProgressBar styling is often OS-dependent or requires full custom drawing.
        ' We'll use custom paint for a basic dark theme look.
        SetCustomPaint(progressBar, AddressOf PaintProgressBar)
    End Sub

    Private Sub ApplyRichTextBoxTheme(richTextBox As RichTextBox)
        richTextBox.BackColor = _colorScheme.BackgroundSecondary
        richTextBox.ForeColor = _colorScheme.TextPrimary
        richTextBox.BorderStyle = BorderStyle.FixedSingle
        SetCustomPaint(richTextBox, AddressOf PaintRichTextBoxBorder) ' For border consistency
    End Sub

    Private Sub ApplyLinkLabelTheme(linkLabel As LinkLabel)
        If linkLabel.BackColor = SystemColors.Control Then
            linkLabel.BackColor = Color.Transparent
        End If
        linkLabel.ForeColor = _colorScheme.TextPrimary ' General text
        linkLabel.LinkColor = _colorScheme.Accent
        linkLabel.ActiveLinkColor = _colorScheme.AccentPressed
        linkLabel.VisitedLinkColor = _colorScheme.AccentHover ' Or a slightly dimmer accent
    End Sub

    Private Sub ApplyCheckedListBoxTheme(checkedListBox As CheckedListBox)
        checkedListBox.BackColor = _colorScheme.BackgroundSecondary
        checkedListBox.ForeColor = _colorScheme.TextPrimary
        checkedListBox.BorderStyle = BorderStyle.FixedSingle
        ' Custom drawing for items and check boxes
        SetCustomPaint(checkedListBox, AddressOf PaintCheckedListBox)
    End Sub

    Private Sub ApplyTrackBarTheme(trackBar As TrackBar)
        trackBar.BackColor = _colorScheme.BackgroundPrimary ' Or BackgroundSecondary depending on desired look
        ' TrackBar theming is notoriously difficult without full custom drawing.
        SetCustomPaint(trackBar, AddressOf PaintTrackBar)
    End Sub

    Private Sub SetCustomPaint(control As Control, handler As PaintEventHandler)
        If Not _customPaintHandlers.ContainsKey(control) Then
            _customPaintHandlers(control) = New List(Of PaintEventHandler)
        End If

        _customPaintHandlers(control).Add(handler)
        AddHandler control.Paint, handler
        AddHandler control.Disposed, Sub() RemoveCustomPaint(control)
    End Sub

    Private Sub RemoveCustomPaint(control As Control)
        If _customPaintHandlers.ContainsKey(control) Then
            For Each handler In _customPaintHandlers(control)
                RemoveHandler control.Paint, handler
            Next
            _customPaintHandlers.Remove(control)
        End If
    End Sub

    ' Custom paint methods
    Private Sub PaintNumericUpDown(sender As Object, e As PaintEventArgs)
        Dim numericUpDown = DirectCast(sender, NumericUpDown)
        e.Graphics.Clear(numericUpDown.BackColor) ' Clear background

        ' Border
        Using borderPen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(borderPen, 0, 0, numericUpDown.Width - 1, numericUpDown.Height - 1)
        End Using

        ' Buttons (simplified representation)
        Dim buttonWidth As Integer = SystemInformation.VerticalScrollBarWidth
        Dim upButtonRect As New Rectangle(numericUpDown.Width - buttonWidth - 1, 1, buttonWidth, (numericUpDown.Height \ 2) - 2)
        Dim downButtonRect As New Rectangle(numericUpDown.Width - buttonWidth - 1, numericUpDown.Height \ 2, buttonWidth, (numericUpDown.Height \ 2) - 2)

        Using buttonBrush As New SolidBrush(_colorScheme.BackgroundTertiary), arrowBrush As New SolidBrush(_colorScheme.TextPrimary)
            e.Graphics.FillRectangle(buttonBrush, upButtonRect)
            e.Graphics.FillRectangle(buttonBrush, downButtonRect)

            ' Draw arrows (simple triangles)
            Dim arrowSize As Integer = 5
            Dim upArrowPoints As Point() = {
                New Point(upButtonRect.X + upButtonRect.Width \ 2, upButtonRect.Y + (upButtonRect.Height - arrowSize) \ 2),
                New Point(upButtonRect.X + (upButtonRect.Width - arrowSize) \ 2, upButtonRect.Y + (upButtonRect.Height + arrowSize) \ 2),
                New Point(upButtonRect.X + (upButtonRect.Width + arrowSize) \ 2, upButtonRect.Y + (upButtonRect.Height + arrowSize) \ 2)
            }
            e.Graphics.FillPolygon(arrowBrush, upArrowPoints)

            Dim downArrowPoints As Point() = {
                New Point(downButtonRect.X + downButtonRect.Width \ 2, downButtonRect.Y + (downButtonRect.Height + arrowSize) \ 2),
                New Point(downButtonRect.X + (downButtonRect.Width - arrowSize) \ 2, downButtonRect.Y + (downButtonRect.Height - arrowSize) \ 2),
                New Point(downButtonRect.X + (downButtonRect.Width + arrowSize) \ 2, downButtonRect.Y + (downButtonRect.Height - arrowSize) \ 2)
            }
            e.Graphics.FillPolygon(arrowBrush, downArrowPoints)
        End Using

        ' Redraw text (important for NumericUpDown)
        Using textBrush As New SolidBrush(numericUpDown.ForeColor)
            Dim textRect As New Rectangle(2, 2, numericUpDown.Width - buttonWidth - 4, numericUpDown.Height - 4)
            TextRenderer.DrawText(e.Graphics, numericUpDown.Text, numericUpDown.Font, textRect, numericUpDown.ForeColor, TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)
        End Using
    End Sub

    Private Sub PaintDateTimePicker(sender As Object, e As PaintEventArgs)
        Dim dateTimePicker = DirectCast(sender, DateTimePicker)
        e.Graphics.Clear(dateTimePicker.BackColor) ' Clear background

        ' Border
        Using borderPen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(borderPen, 0, 0, dateTimePicker.Width - 1, dateTimePicker.Height - 1)
        End Using

        ' Dropdown button (simplified representation)
        Dim buttonWidth As Integer = SystemInformation.VerticalScrollBarWidth
        Dim buttonRect As New Rectangle(dateTimePicker.Width - buttonWidth - 1, 1, buttonWidth, dateTimePicker.Height - 2)

        Using buttonBrush As New SolidBrush(_colorScheme.BackgroundTertiary), arrowBrush As New SolidBrush(_colorScheme.TextPrimary)
            e.Graphics.FillRectangle(buttonBrush, buttonRect)
            ' Draw dropdown arrow (simple triangle)
            Dim arrowSize As Integer = 5
            Dim arrowPoints As Point() = {
                New Point(buttonRect.X + buttonRect.Width \ 2, buttonRect.Y + (buttonRect.Height + arrowSize) \ 2),
                New Point(buttonRect.X + (buttonRect.Width - arrowSize) \ 2, buttonRect.Y + (buttonRect.Height - arrowSize) \ 2),
                New Point(buttonRect.X + (buttonRect.Width + arrowSize) \ 2, buttonRect.Y + (buttonRect.Height - arrowSize) \ 2)
            }
            e.Graphics.FillPolygon(arrowBrush, arrowPoints)
        End Using

        ' Redraw text (important for DateTimePicker)
        Using textBrush As New SolidBrush(dateTimePicker.ForeColor)
            Dim textRect As New Rectangle(2, 2, dateTimePicker.Width - buttonWidth - 4, dateTimePicker.Height - 4)
            TextRenderer.DrawText(e.Graphics, dateTimePicker.Text, dateTimePicker.Font, textRect, dateTimePicker.ForeColor, TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)
        End Using
    End Sub

    Private Sub PaintProgressBar(sender As Object, e As PaintEventArgs)
        Dim progressBar = DirectCast(sender, ProgressBar)
        e.Graphics.Clear(progressBar.BackColor) ' Clear background

        ' Border
        Using borderPen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(borderPen, 0, 0, progressBar.Width - 1, progressBar.Height - 1)
        End Using

        ' Progress fill
        Dim fillWidth As Integer = CInt(Math.Truncate((progressBar.Width - 2) * (progressBar.Value - progressBar.Minimum) / (progressBar.Maximum - progressBar.Minimum)))
        If fillWidth > 0 Then
            Using fillBrush As New SolidBrush(_colorScheme.Accent)
                e.Graphics.FillRectangle(fillBrush, 1, 1, fillWidth, progressBar.Height - 2)
            End Using
        End If
    End Sub

    Private Sub PaintRichTextBoxBorder(sender As Object, e As PaintEventArgs)
        Dim richTextBox = DirectCast(sender, RichTextBox)
        Using pen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(pen, 0, 0, richTextBox.Width - 1, richTextBox.Height - 1)
        End Using
    End Sub

    Private Sub PaintCheckedListBox(sender As Object, e As PaintEventArgs)
        Dim checkedListBox = DirectCast(sender, CheckedListBox)
        e.Graphics.Clear(checkedListBox.BackColor) ' Clear background

        ' Border
        Using borderPen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(borderPen, 0, 0, checkedListBox.Width - 1, checkedListBox.Height - 1)
        End Using

        ' Custom draw items for better control over checkbox and text
        For i As Integer = 0 To checkedListBox.Items.Count - 1
            Dim itemRect = checkedListBox.GetItemRectangle(i)
            Dim isSelected = (checkedListBox.SelectedIndex = i)
            Dim isChecked = checkedListBox.GetItemChecked(i)
            Dim itemText = checkedListBox.GetItemText(checkedListBox.Items(i))

            ' Background
            Using bgBrush As New SolidBrush(If(isSelected, _colorScheme.AccentHover, checkedListBox.BackColor))
                e.Graphics.FillRectangle(bgBrush, itemRect)
            End Using

            ' CheckBox
            Dim checkSize As Integer = 13 ' Standard checkbox size
            Dim checkRect As New Rectangle(itemRect.X + 2, itemRect.Y + (itemRect.Height - checkSize) \ 2, checkSize, checkSize)

            Using checkBrush As New SolidBrush(_colorScheme.BackgroundTertiary), checkMarkPen As New Pen(_colorScheme.TextPrimary, 2)
                e.Graphics.FillRectangle(checkBrush, checkRect)
                Using checkBorderPen As New Pen(_colorScheme.Border)
                    e.Graphics.DrawRectangle(checkBorderPen, checkRect)
                End Using

                If isChecked Then
                    ' Draw check mark (simple 'V')
                    e.Graphics.DrawLines(checkMarkPen, {
                        New Point(checkRect.X + 3, checkRect.Y + checkSize \ 2),
                        New Point(checkRect.X + checkSize \ 2 - 1, checkRect.Y + checkSize - 3),
                        New Point(checkRect.X + checkSize - 3, checkRect.Y + 3)
                    })
                End If
            End Using

            ' Text
            Dim textRect As New Rectangle(itemRect.X + checkSize + 6, itemRect.Y, itemRect.Width - checkSize - 8, itemRect.Height)
            TextRenderer.DrawText(e.Graphics, itemText, checkedListBox.Font, textRect, _colorScheme.TextPrimary, TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)
        Next
    End Sub

    Private Sub PaintTrackBar(sender As Object, e As PaintEventArgs)
        Dim trackBar = DirectCast(sender, TrackBar)
        e.Graphics.Clear(trackBar.BackColor) ' Clear background

        Dim trackRect As Rectangle
        Dim thumbRect As Rectangle
        Dim thumbSize As Integer = 10 ' Approximate thumb width/height

        If trackBar.Orientation = Orientation.Horizontal Then
            Dim trackHeight As Integer = 4
            trackRect = New Rectangle(thumbSize \ 2, (trackBar.Height - trackHeight) \ 2, trackBar.Width - thumbSize, trackHeight)
            Dim thumbX As Integer = CInt(Math.Truncate(trackRect.X + (trackRect.Width - thumbSize) * (trackBar.Value - trackBar.Minimum) / (trackBar.Maximum - trackBar.Minimum)))
            thumbRect = New Rectangle(thumbX, (trackBar.Height - thumbSize * 2) \ 2, thumbSize, thumbSize * 2)
        Else ' Vertical
            Dim trackWidth As Integer = 4
            trackRect = New Rectangle((trackBar.Width - trackWidth) \ 2, thumbSize \ 2, trackWidth, trackBar.Height - thumbSize)
            Dim thumbY As Integer = CInt(Math.Truncate(trackRect.Y + (trackRect.Height - thumbSize) * (trackBar.Value - trackBar.Minimum) / (trackBar.Maximum - trackBar.Minimum)))
            thumbRect = New Rectangle((trackBar.Width - thumbSize * 2) \ 2, thumbY, thumbSize * 2, thumbSize)
        End If

        ' Draw Track
        Using trackBrush As New SolidBrush(_colorScheme.BackgroundTertiary)
            e.Graphics.FillRectangle(trackBrush, trackRect)
        End Using

        ' Draw Thumb
        Using thumbBrush As New SolidBrush(_colorScheme.Accent)
            e.Graphics.FillEllipse(thumbBrush, thumbRect) ' Use Ellipse for a rounder thumb
        End Using

        ' Draw Ticks (optional, basic implementation)
        If trackBar.TickStyle <> TickStyle.None Then
            Using tickPen As New Pen(_colorScheme.Border)
                Dim numTicks As Integer = trackBar.Maximum - trackBar.Minimum
                If numTicks > 0 AndAlso trackBar.TickFrequency > 0 Then
                    For i As Integer = 0 To numTicks Step trackBar.TickFrequency
                        Dim tickValue As Integer = trackBar.Minimum + i
                        Dim tickPos As Single
                        If trackBar.Orientation = Orientation.Horizontal Then
                            tickPos = trackRect.X + (trackRect.Width - thumbSize) * (tickValue - trackBar.Minimum) / (trackBar.Maximum - trackBar.Minimum) + thumbSize / 2
                            If trackBar.TickStyle = TickStyle.BottomRight OrElse trackBar.TickStyle = TickStyle.Both Then
                                e.Graphics.DrawLine(tickPen, tickPos, trackRect.Bottom + 1, tickPos, trackRect.Bottom + 5)
                            End If
                            If trackBar.TickStyle = TickStyle.TopLeft OrElse trackBar.TickStyle = TickStyle.Both Then
                                e.Graphics.DrawLine(tickPen, tickPos, trackRect.Top - 1, tickPos, trackRect.Top - 5)
                            End If
                        Else ' Vertical
                            tickPos = trackRect.Y + (trackRect.Height - thumbSize) * (tickValue - trackBar.Minimum) / (trackBar.Maximum - trackBar.Minimum) + thumbSize / 2
                            If trackBar.TickStyle = TickStyle.BottomRight OrElse trackBar.TickStyle = TickStyle.Both Then
                                e.Graphics.DrawLine(tickPen, trackRect.Right + 1, tickPos, trackRect.Right + 5, tickPos)
                            End If
                            If trackBar.TickStyle = TickStyle.TopLeft OrElse trackBar.TickStyle = TickStyle.Both Then
                                e.Graphics.DrawLine(tickPen, trackRect.Left - 1, tickPos, trackRect.Left - 5, tickPos)
                            End If
                        End If
                    Next
                End If
            End Using
        End If
    End Sub

    Private Sub PaintTextBox(sender As Object, e As PaintEventArgs)
        Dim textBox = DirectCast(sender, TextBox)
        Using pen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(pen, 0, 0, textBox.Width - 1, textBox.Height - 1)
        End Using
    End Sub

    Private Sub PaintComboBox(sender As Object, e As PaintEventArgs)
        Dim comboBox = DirectCast(sender, ComboBox)
        Using pen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(pen, 0, 0, comboBox.Width - 1, comboBox.Height - 1)
        End Using
    End Sub

    Private Sub PaintListBox(sender As Object, e As PaintEventArgs)
        Dim listBox = DirectCast(sender, ListBox)
        Using pen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(pen, 0, 0, listBox.Width - 1, listBox.Height - 1)
        End Using
    End Sub

    Private Sub PaintGroupBox(sender As Object, e As PaintEventArgs)
        Dim groupBox = DirectCast(sender, GroupBox)
        Dim textSize = TextRenderer.MeasureText(groupBox.Text, groupBox.Font)
        Dim rect = New Rectangle(0, textSize.Height \ 2, groupBox.Width - 1, groupBox.Height - textSize.Height \ 2 - 1)

        Using pen As New Pen(_colorScheme.Border)
            e.Graphics.DrawRectangle(pen, rect)
        End Using

        If Not String.IsNullOrEmpty(groupBox.Text) Then
            Using brush As New SolidBrush(_colorScheme.BackgroundPrimary)
                e.Graphics.FillRectangle(brush, 8, 0, textSize.Width + 4, textSize.Height)
            End Using
            TextRenderer.DrawText(e.Graphics, groupBox.Text, groupBox.Font, New Point(10, 0), groupBox.ForeColor)
        End If
    End Sub

    Private Sub PaintTabControl(sender As Object, e As PaintEventArgs)
        Dim tabControl = DirectCast(sender, TabControl)
        For i As Integer = 0 To tabControl.TabCount - 1
            Dim tabRect = tabControl.GetTabRect(i)
            Dim isSelected = (i = tabControl.SelectedIndex)

            Using brush As New SolidBrush(If(isSelected, _colorScheme.BackgroundSecondary, _colorScheme.BackgroundTertiary))
                e.Graphics.FillRectangle(brush, tabRect)
            End Using

            Using pen As New Pen(_colorScheme.Border)
                e.Graphics.DrawRectangle(pen, tabRect)
            End Using

            TextRenderer.DrawText(e.Graphics, tabControl.TabPages(i).Text, tabControl.Font, tabRect, _colorScheme.TextPrimary, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        Next
    End Sub

    ' Custom ToolStripRenderer for dark theme with gradient fix
    Private Class DarkToolStripRenderer
        Inherits ToolStripProfessionalRenderer

        Private _colorScheme As DarkColorScheme

        Public Sub New(colorScheme As DarkColorScheme)
            MyBase.New(New DarkToolStripColorTable(colorScheme))
            _colorScheme = colorScheme
        End Sub

        ' FIX: Explicitly render image margin with solid color
        Protected Overrides Sub OnRenderImageMargin(e As ToolStripRenderEventArgs)
            Using brush As New SolidBrush(_colorScheme.BackgroundSecondary)
                e.Graphics.FillRectangle(brush, e.AffectedBounds)
            End Using
        End Sub

        Protected Overrides Sub OnRenderToolStripBackground(e As ToolStripRenderEventArgs)
            Using brush As New SolidBrush(_colorScheme.BackgroundSecondary)
                e.Graphics.FillRectangle(brush, e.AffectedBounds)
            End Using
        End Sub

        Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
            If e.Item.Selected OrElse e.Item.Pressed Then
                Using brush As New SolidBrush(Color.FromArgb(65, 65, 65))
                    e.Graphics.FillRectangle(brush, New Rectangle(1, 0, e.Item.Size.Width - 2, e.Item.Size.Height))
                End Using

                Using pen As New Pen(Color.FromArgb(100, 100, 100))
                    e.Graphics.DrawRectangle(pen, New Rectangle(1, 0, e.Item.Size.Width - 3, e.Item.Size.Height - 1))
                End Using
            End If
        End Sub

        Protected Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
            Dim rect = New Rectangle(30, 3, e.Item.Width - 30, 1)
            Using pen As New Pen(_colorScheme.Border)
                e.Graphics.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top)
            End Using
        End Sub

        Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
            e.TextColor = _colorScheme.TextPrimary
            MyBase.OnRenderItemText(e)
        End Sub

        Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
            e.ArrowColor = _colorScheme.TextPrimary
            MyBase.OnRenderArrow(e)
        End Sub

        Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
            If TypeOf e.ToolStrip Is ToolStripDropDown Then
                Using pen As New Pen(_colorScheme.Border)
                    e.Graphics.DrawRectangle(pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1)
                End Using
            End If
        End Sub
    End Class

    Private Class DarkToolStripColorTable
        Inherits ProfessionalColorTable

        Private _colorScheme As DarkColorScheme

        Public Sub New(colorScheme As DarkColorScheme)
            _colorScheme = colorScheme
        End Sub

        Public Overrides ReadOnly Property MenuStripGradientBegin As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientEnd As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        ' FIX: All gradient properties set to solid color
        Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientBegin As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientMiddle As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientEnd As Color
            Get
                Return _colorScheme.BackgroundSecondary
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected As Color
            Get
                Return _colorScheme.AccentHover
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder As Color
            Get
                Return _colorScheme.Border
            End Get
        End Property
    End Class

    Public Sub RemoveDarkTheme(form As Form)
        If Not _appliedForms.Contains(form) Then Return

        _appliedForms.Remove(form)

        ' Reset to system colors
        form.BackColor = SystemColors.Control
        form.ForeColor = SystemColors.ControlText

        ' Remove custom paint handlers
        RemoveThemeFromControl(form)

        ' Reset title bar
        Try
            Dim lightMode As Integer = 0
            DwmSetWindowAttribute(form.Handle, 20, lightMode, 4)
        Catch
        End Try
    End Sub

    Private Sub RemoveThemeFromControl(control As Control)
        RemoveCustomPaint(control)

        ' Reset control colors
        If TypeOf control Is Button Then
            Dim btn = DirectCast(control, Button)
            btn.BackColor = SystemColors.Control
            btn.ForeColor = SystemColors.ControlText
            btn.FlatStyle = FlatStyle.Standard
            btn.UseVisualStyleBackColor = True
        End If

        ' Recursively remove from children
        For Each childControl As Control In control.Controls
            RemoveThemeFromControl(childControl)
        Next
    End Sub

    Public ReadOnly Property ColorScheme As DarkColorScheme
        Get
            Return _colorScheme
        End Get
    End Property
End Class