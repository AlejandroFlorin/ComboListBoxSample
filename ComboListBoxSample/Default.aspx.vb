Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack() Then

            clb01.IsSequencedList = True
            clb01.AllowReorder = True
            clb01.Height = 200
            clb01.MaxListItemLength = 10
            clb02.PrimaryKeyOption = ComboListBox.PrimaryKeyOptions.ItemText

            clb01.SetLists(
                Nothing,
                ComboListBox.GetEmptyFormatedTable(ComboListBox.PrimaryKeyOptions.ItemText, "ItemValue", "ItemValue"),
                "ItemSequence",
                "ItemValue",
                "ItemValue")


            clb02.SortMode = ComboListBox.SortModes.Sequential
            clb02.EntryMode = ComboListBox.EntryModes.DropDownList
            clb02.IsSequencedList = False
            clb02.AllowReorder = True
            clb02.Height = 200
            clb02.PrimaryKeyOption = ComboListBox.PrimaryKeyOptions.ItemValue

            Dim dtSourceList As DataTable = ComboListBox.GetEmptyFormatedTable(ComboListBox.PrimaryKeyOptions.ItemValue, "ItemText", "ItemValue", "ItemSequence")

            Dim dr As DataRow

            dr = dtSourceList.NewRow
            dr("ItemText") = "Item Text 1"
            dr("ItemValue") = "Item Value 1"
            dr("ItemSequence") = 1
            dtSourceList.Rows.Add(dr)

            dr = dtSourceList.NewRow
            dr("ItemText") = "Item Text 2"
            dr("ItemValue") = "Item Value 2"
            dr("ItemSequence") = 2
            dtSourceList.Rows.Add(dr)

            dr = dtSourceList.NewRow
            dr("ItemText") = "Item Text 3"
            dr("ItemValue") = "Item Value 3"
            dr("ItemSequence") = 3
            dtSourceList.Rows.Add(dr)

            dtSourceList.AcceptChanges()

            clb02.SetLists(
                dtSourceList,
                ComboListBox.GetEmptyFormatedTable(ComboListBox.PrimaryKeyOptions.ItemValue, "ItemText", "ItemValue"),
                "ItemSequence",
                "ItemText",
                "ItemValue")

        End If

    End Sub

End Class