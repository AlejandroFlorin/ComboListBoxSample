Public Class ComboListBoxEventArgs
    Inherits EventArgs

    Public AffectedDataRow As DataRow
    Public SelectedValue As String
    Public SelectedText As String

    Public Sub New(ByVal AffectedDataRow As DataRow)
        Me.AffectedDataRow = AffectedDataRow
    End Sub

    Public Sub New(selectedValue As String, selectedText As String)
        Me.SelectedValue = selectedValue
        Me.SelectedText = selectedText
    End Sub

    Public Sub New(affectedDataRow As DataRow, selectedText As String, selectedValue As String)
        Me.AffectedDataRow = affectedDataRow
        Me.SelectedText = selectedText
        Me.SelectedValue = selectedValue
    End Sub

End Class
