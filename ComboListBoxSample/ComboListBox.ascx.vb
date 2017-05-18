Partial Public Class ComboListBox
    Inherits System.Web.UI.UserControl

#Region "Privates"

    Private _dtSourceListItems As DataTable
    Private _dtListBoxItems As DataTable

#End Region

#Region "Constants"

    Private Const DEFAULT_HEIGHT_IN_ROWS As Integer = 6
    Private Const DEFAULT_ROW_HEIGHT As Integer = 20
    Private Const DEFAULT_MAX_LIST_ITEMS As Integer = 100
    Private Const DEFAULT_HEIGHT_IN_PIXELS As Integer = 80
    Private Const MIN_HEIGHT_IN_PIXELS As Integer = 60

#End Region

#Region "Properties"

    Public Property IsClosing() As Boolean
        'Use to manage session storage of the controls data. Set to True when the parent page is exiting/unloading so the data for the control is cleared from the session
        Get
            If Not ViewState("IsClosing") Is Nothing Then
                Return CBool(ViewState("IsClosing"))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsClosing") = Value
        End Set
    End Property

    Public WriteOnly Property CssClass() As String
        Set(Value As String)
            txtSourceEntry.CssClass = String.Join(" ", {txtSourceEntry.CssClass, Value})
            ddlSourceList.CssClass = String.Join(" ", {ddlSourceList.CssClass, Value})
            lbListBox.CssClass = String.Join(" ", {lbListBox.CssClass, Value})
        End Set
    End Property

    Private Property SequenceFieldName() As String
        'Defines the column name of the data table which holds the records integer sequence. Used for sorting
        Get
            If ViewState("SequenceFieldName") IsNot Nothing Then
                Return ViewState("SequenceFieldName").ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(Value As String)
            ViewState("SequenceFieldName") = Value
        End Set
    End Property

    Private Property TextFieldName() As String
        'Defines the column name of the data table which holds Text data. Corresponds with listitem.text 
        Get
            If ViewState("TextFieldName") IsNot Nothing Then
                Return ViewState("TextFieldName").ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(Value As String)
            ViewState("TextFieldName") = Value
        End Set
    End Property

    Private Property ValueFieldName() As String
        'Defines the column name of the data table which holds Value data. Corresponds with listitem.value 
        Get
            If ViewState("ValueFieldName") IsNot Nothing Then
                Return ViewState("ValueFieldName").ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(Value As String)
            ViewState("ValueFieldName") = Value
        End Set
    End Property

    Public Property IsSequencedList() As Boolean
        'Used to tell the control whether the list is a sequenced list. The data tables will need to contain a sequence column
        Get
            If ViewState("IsSequencedList") IsNot Nothing Then
                Return CBool(ViewState("IsSequencedList"))
            Else
                Return False
            End If
        End Get
        Set(Value As Boolean)
            ViewState("IsSequencedList") = Value
        End Set
    End Property

    Public Property PrimaryKeyOption() As PrimaryKeyOptions
        'Defines which is the primary key for the control's lists and associated tables.
        'It will be either a listitem's TEXT or its VALUE
        Get
            If ViewState("PrimaryKeyOption") IsNot Nothing Then
                Return CType(ViewState("PrimaryKeyOption"), PrimaryKeyOptions)
            Else
                Return PrimaryKeyOptions.ItemValue
            End If
        End Get
        Set(Value As PrimaryKeyOptions)
            ViewState("PrimaryKeyOption") = Value
        End Set
    End Property

    Public Property AutoResizeMode() As AutoResizeModes
        'Sets how the control resizes itself vertically.
        'It can be set to resize based on the screen size, based on how many items the list has or no resize at all
        Get
            If ViewState("AutoResizeMode") IsNot Nothing Then
                Return CType(ViewState("AutoResizeMode"), AutoResizeModes)
            Else
                Return AutoResizeModes.NoResize
            End If
        End Get
        Set(Value As AutoResizeModes)
            ViewState("AutoResizeMode") = Value
        End Set
    End Property

    Public Property CanBeEmpty() As Boolean
        'Determines if the delete button is disabled when the last item is attempted to be deleted from the list
        Get
            If ViewState("CanBeEmpty") IsNot Nothing Then
                Return CBool(ViewState("CanBeEmpty"))
            Else
                Return True
            End If
        End Get
        Set(Value As Boolean)
            ViewState("CanBeEmpty") = Value
        End Set
    End Property

    Public Property Enabled() As Boolean
        'Used to enable or disable the control
        Get
            If ViewState("Enabled") IsNot Nothing Then
                Return CBool(ViewState("Enabled"))
            Else
                Return True
            End If
        End Get
        Set(Value As Boolean)
            ViewState("Enabled") = Value
        End Set
    End Property

    Public Property AllowReorder() As Boolean
        'Determines if the up and down reorder buttons are displayed
        'The buttons will do nothing if IsSequencedList is true
        Get
            If ViewState("AllowReorder") IsNot Nothing Then
                Return CBool(ViewState("AllowReorder"))
            Else
                Return False
            End If
        End Get
        Set(Value As Boolean)
            ViewState("AllowReorder") = Value
        End Set
    End Property

    Public Property EntryMode() As EntryModes
        'Used to set the main entry mode.
        'The control can be set to DropdownList with a source list ddl where the user selects items from a finite list.
        'The control can alternately be set to TextBox with an open textbox entry control where the user adds new items to the list
        Get
            If ViewState("EntryMode") IsNot Nothing Then
                Return CType(ViewState("EntryMode"), EntryModes)
            Else
                Return EntryModes.TextBox
            End If
        End Get
        Set(Value As EntryModes)
            ViewState("EntryMode") = Value
        End Set
    End Property

    Public Property SortMode() As SortModes
        'Used to set if the control will sort items in the list box automatically.
        'They may be sorted alphabetically, by sequence number (if its a sequenced list) or not sorted at all
        'If set to none, the allowreorder may be used to let the user reorder items manually
        Get
            If ViewState("SortMode") IsNot Nothing Then
                Return CType(ViewState("SortMode"), SortModes)
            Else
                Return SortModes.None
            End If
        End Get
        Set(Value As SortModes)
            ViewState("SortMode") = Value
        End Set
    End Property

    Public Property MaxListItemsCount() As Integer
        Get
            If ViewState("MaxListItemsCount") IsNot Nothing Then
                Return CInt(ViewState("MaxListItemsCount"))
            Else
                Return Integer.MaxValue
            End If
        End Get
        Set(Value As Integer)
            ViewState("MaxListItemsCount") = Value
        End Set
    End Property

    Public Property MaxListItemLength() As Integer
        'Defines the maximum character length of items before a warning is displayed
        Get
            If ViewState("MaxListItemLength") IsNot Nothing Then
                Return CInt(ViewState("MaxListItemLength"))
            Else
                Return DEFAULT_MAX_LIST_ITEMS
            End If
        End Get
        Set(Value As Integer)
            ViewState("MaxListItemLength") = Value
        End Set
    End Property

    Public Property Height() As Integer
        'This property sets the default initial height of the control in pixels
        'If the control is allowed to resize, it will do so when appropriate
        Get
            If ViewState("Height") IsNot Nothing Then
                Return CInt(ViewState("Height"))
            Else
                Return DEFAULT_HEIGHT_IN_PIXELS

            End If
        End Get
        Set(Value As Integer)
            If (Value < MIN_HEIGHT_IN_PIXELS) Then
                Throw New System.Exception("The Height property for a ComboListBox control must be >= " & MIN_HEIGHT_IN_PIXELS.ToString)
            Else
                ViewState("Height") = Value
            End If
        End Set
    End Property

    Public Property MinHeight() As Integer
        'This property sets the default initial height of the control in pixels
        'If the control is allowed to resize, it will do so when appropriate
        Get
            If ViewState("MinHeight") IsNot Nothing Then
                Return CInt(ViewState("MinHeight"))
            Else
                Return MIN_HEIGHT_IN_PIXELS
            End If
        End Get
        Set(Value As Integer)
            If (Value < MIN_HEIGHT_IN_PIXELS) Then
                Throw New System.Exception("The MinHeight property for a ComboListBox control must be >= " & MIN_HEIGHT_IN_PIXELS.ToString)
            Else
                ViewState("MinHeight") = Value
            End If
        End Set
    End Property

    Public Property Width() As Unit
        Get
            Return pnlComboListBoxWrapper.Width
        End Get
        Set(value As Unit)
            pnlComboListBoxWrapper.Width = value
        End Set
    End Property

    Public Property AutoResizeScaleFactor() As Double
        'Used if autoresize is set to ResizeToScreen and determines what percentage of
        'a screen vertical resize should be applied to the control
        Get
            If Not (ViewState("AutoResizeScaleFactor") Is Nothing) Then
                Return CommonUtilities.ConvertStringToDouble(ViewState("AutoResizeScaleFactor").ToString)
            Else
                Return 1
            End If
        End Get
        Set(Value As Double)
            If (Value <= 0 Or Value > 1) Then
                Throw New System.Exception("The AutoResizeScaleFactor property for a ComboListBox control must be a Double > 0 and <= 1")
            Else
                ViewState("AutoResizeScaleFactor") = Value
            End If
        End Set
    End Property

    Public Property Displayed() As Boolean
        Get
            If Not (ViewState("Displayed") Is Nothing) Then
                Return CBool(ViewState("Displayed"))
            Else
                Return True
            End If
        End Get
        Set(value As Boolean)
            ViewState("Displayed") = value
            If value Then
                pnlComboListBoxWrapper.Style("Display") = "block"
            Else
                pnlComboListBoxWrapper.Style("Display") = "none"
            End If
        End Set
    End Property

    Public ReadOnly Property ListItems() As ListItemCollection
        Get
            Return Me.lbListBox.Items
        End Get
    End Property

    Public ReadOnly Property SourceListItems() As ListItemCollection
        Get
            Return Me.ddlSourceList.Items
        End Get
    End Property

    Public ReadOnly Property UnassignedListItems() As ListItemCollection
        Get
            Dim UnassignedItems As New ListItemCollection
            For Each li As ListItem In ddlSourceList.Items
                If li.Text <> String.Empty AndAlso lbListBox.Items.FindByText(li.Text) Is Nothing Then
                    UnassignedItems.Add(New ListItem(li.Text, li.Value))
                End If
            Next
            Return UnassignedItems
        End Get
    End Property

    Public ReadOnly Property SelectedItemRow() As DataRow
        Get
            If ViewState("SelectedItemKey") IsNot Nothing Then
                Return _dtListBoxItems.Rows.Find(ViewState("SelectedItemKey"))
            Else
                Return Nothing
            End If
        End Get

    End Property

    Public Property IsDirty() As Boolean
        Get
            If ViewState("IsDirty") IsNot Nothing Then
                Return CBool(ViewState("IsDirty"))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("IsDirty") = value
        End Set
    End Property

    Public ReadOnly Property SourceTable() As DataTable
        Get
            LoadPersistedDataFromSession()
            Return _dtSourceListItems
        End Get
    End Property

    Public ReadOnly Property SelectedTable() As DataTable
        Get
            LoadPersistedDataFromSession()
            Return _dtListBoxItems
        End Get
    End Property

    Public ReadOnly Property TextBoxClientID() As String
        Get
            Return Me.txtSourceEntry.ClientID
        End Get
    End Property

    Public ReadOnly Property DropDownListClientID() As String
        Get
            Return Me.ddlSourceList.ClientID
        End Get
    End Property

    Public Property DeleteButtonEnabled() As Boolean
        Get
            If Not (ViewState("DeleteButtonEnabled") Is Nothing) Then
                Return CBool(ViewState("DeleteButtonEnabled"))
            Else
                Return True
            End If
        End Get
        Set(Value As Boolean)
            ViewState("DeleteButtonEnabled") = Value
        End Set
    End Property

    Public Property AddConfirmButtonEnabled() As Boolean
        Get
            If Not (ViewState("AddConfirmButtonEnabled") Is Nothing) Then
                Return CBool(ViewState("AddConfirmButtonEnabled"))
            Else
                Return True
            End If
        End Get
        Set(Value As Boolean)
            ViewState("AddConfirmButtonEnabled") = Value
        End Set
    End Property

    Public Property SourceListDDLEnabled() As Boolean
        Get
            If Not (ViewState("SourceListDDLEnabled") Is Nothing) Then
                Return CBool(ViewState("SourceListDDLEnabled"))
            Else
                Return True
            End If
        End Get
        Set(Value As Boolean)
            ViewState("SourceListDDLEnabled") = Value
        End Set
    End Property

#End Region

#Region "Enums"

    Public Enum EntryModes
        TextBox
        DropDownList
    End Enum

    Public Enum AutoResizeModes 'Used for vertical resizing (or not)
        ResizeToScreen
        ResizeToItems
        NoResize
    End Enum

    Public Enum SortModes
        Sequential
        Alphabetical
        None
    End Enum

    Public Enum PrimaryKeyOptions
        ItemText
        ItemValue
    End Enum

#End Region

#Region "Events"

    'These events are available to any page the uses the ComboListBox control
    'They all pass as arguments a reference to the CombListBox control as well as
    'the row of the datatable bound to the list box that was affected by the event
    Public Event ItemMovedUp As ComboListBoxEventHandler
    Public Event ItemMovedDown As ComboListBoxEventHandler
    Public Event ItemDeleted As ComboListBoxEventHandler
    Public Event ItemAdded As ComboListBoxEventHandler
    Public Event ItemModified As ComboListBoxEventHandler
    Public Event ItemSelected As ComboListBoxEventHandler

    Public Delegate Sub ComboListBoxEventHandler(sender As Object, e As ComboListBoxEventArgs)

#End Region

#Region "Helper Subs"
    Public Sub SetLists(dtSourceListItems As DataTable,
        dtInitialListBoxItems As DataTable,
        sequenceFieldName As String,
        textFieldName As String,
        Optional valueFieldName As String = "ListItemValue")

        'Used to initialize the tables and lists in the control. Should be called after the controls properties have been set
        'by the parent page and before the user interacts with the control.

        'The dtSourceListItems table is used if EntryMode = DropDownList (it may be NOTHING if not needed)
        'It needs to contain all the items that may be assigned to the listbox (including any that are already assigned).

        'The dtInitialListBoxItems table should contain all the items that are already assigned to the
        'list box.  If there are no current items, an empty (but instantiated) table should be passed with
        'its columns already defined.

        'The SequenceColumnName is used if the list is sequenced.  It is used to tell the control
        'what column in both the dtSourceListItems and dtInitialListBoxItems tables contains the sequence number
        'of the items.  The column name needs to be the same in both tables

        'The ItemTextColumnName is used to tell the control what column in the tables contain the text values
        'that will be displayed in the sourcelist ddl and the listbox.  The column name needs to be the same in
        'both tables

        'The ItemValueColumnName is optional and is used to tell the control what column in the tables contain
        'the listitem VALUEs (as opposed to the listItem TEXTs) that will be set for sourcelist ddl and the listbox.
        'The column name needs to be the same in both tables.  If the parameter is omited, the control will set
        'the listitem VALUEs = to the listitem TEXT's

        Dim dr As DataRow

        'If no sequenceColumnName was passed, make sure the IsSequenceList property is set to false
        IsSequencedList = Not String.IsNullOrEmpty(sequenceFieldName)

        Me.SequenceFieldName = sequenceFieldName

        If dtSourceListItems Is Nothing Then
            dtSourceListItems = GetEmptyFormatedTable()
        End If

        If SortMode = SortModes.Sequential AndAlso Not IsSequencedList Then
            Throw New System.Exception("Must pass a sequenceFieldName if the SortMode is set to Sequential")
        End If

        If valueFieldName = Nothing Then
            valueFieldName = textFieldName
        End If

        Me.TextFieldName = textFieldName
        Me.ValueFieldName = valueFieldName

        'If the dtSourceListItems table does not have a primary key,
        'then create one based on the primarykeyoptions property
        If EntryMode = EntryModes.DropDownList AndAlso dtSourceListItems.PrimaryKey.Length = 0 Then
            Dim primaryKeyColumn(0) As DataColumn
            If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                primaryKeyColumn(0) = dtSourceListItems.Columns(textFieldName)
                dtSourceListItems.PrimaryKey = primaryKeyColumn
            Else
                primaryKeyColumn(0) = dtSourceListItems.Columns(valueFieldName)
                dtSourceListItems.PrimaryKey = primaryKeyColumn
            End If
        End If

        'If the dtInitialListBoxItems table does not have a primary key,
        'then create one based on the primarykeyoptions property
        If dtInitialListBoxItems.PrimaryKey.Length = 0 Then
            Dim primaryKeyColumn(0) As DataColumn
            If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                primaryKeyColumn(0) = dtInitialListBoxItems.Columns(textFieldName)
                dtInitialListBoxItems.PrimaryKey = primaryKeyColumn
            Else
                primaryKeyColumn(0) = dtInitialListBoxItems.Columns(valueFieldName)
                dtInitialListBoxItems.PrimaryKey = primaryKeyColumn
            End If
        End If

        If EntryMode = EntryModes.DropDownList Then

            'Check to see if the table does not already have a blank row.
            'If it doesn't, then add one to the top of the table
            Dim HasBlank As Boolean = False

            For Each drSourceRow As DataRow In dtSourceListItems.Rows
                If CStr(drSourceRow(textFieldName)).Trim.Length = 0 Then
                    HasBlank = True
                    Exit For
                End If
            Next

            If Not HasBlank Then
                'Add a new row to the SourceList Table and set its values to blank
                dr = dtSourceListItems.NewRow
                'Row does not allow null values
                dr(textFieldName) = ""
                If valueFieldName <> textFieldName Then
                    dr(valueFieldName) = "-1"
                End If

                'Insert the blank row into the table as the first row
                If IsSequencedList Then
                    dr(sequenceFieldName) = -1
                    CommonUtilities.InsertRow(dtSourceListItems, sequenceFieldName, dr)
                Else
                    dtSourceListItems.Rows.InsertAt(dr, 0)
                End If

            End If

            If IsSequencedList AndAlso SortMode = SortModes.Sequential Then
                ResequenceSourceList(dtSourceListItems, dtInitialListBoxItems)
            End If

            dtSourceListItems.AcceptChanges()

        End If

        Me._dtSourceListItems = dtSourceListItems
        Me._dtListBoxItems = dtInitialListBoxItems

        'Bind the ListBox items if the passed initial list is not empty
        If dtInitialListBoxItems IsNot Nothing Then
            BindListBox()
        End If

        'Bind the SourceListItems ddl if the passed initial source list is not nothing
        If EntryMode = EntryModes.DropDownList AndAlso dtSourceListItems IsNot Nothing Then
            BindSourceList()
        End If

        PersistDataToSession()

    End Sub

    Public Function GetDelimitedListItems() As String
        'Returns all the current items in the ListBox as a double delimited string.
        'The individual items are delimited by pipes, and each item's text and value are delimited by a comma

        Dim sbItems As New StringBuilder

        If _dtListBoxItems IsNot Nothing Then

            For Each li As ListItem In lbListBox.Items
                sbItems.Append(li.Text)
                sbItems.Append(",")
                sbItems.Append(li.Value)
                sbItems.Append("|")
            Next

            If sbItems.Length > 0 Then
                sbItems.Remove(sbItems.Length - 1, 1)
            End If

        End If

        Return sbItems.ToString

    End Function

    Private Sub ResequenceSourceList(dtSourceListItems As DataTable, dtListBoxItems As DataTable)
        'Used when the mode is set to dropdownlist. It is called to resequence the source list when an item is
        'moved from the source list or when an item already in the list box is deleted  (moving it back to the source list)

        CommonUtilities.RequenceTable(dtSourceListItems, SequenceFieldName, 0) 'A1, B2, C3, D4'
        CommonUtilities.RequenceTable(dtListBoxItems, SequenceFieldName, 1) 'C1, D2'

        Dim sbInitialItems As New StringBuilder
        For Each drInitialListItem As DataRow In dtListBoxItems.Rows

            If drInitialListItem.RowState <> DataRowState.Deleted Then
                If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                    sbInitialItems.Append(String.Format("'{0}',", drInitialListItem(TextFieldName)))
                    dtSourceListItems.Rows.Find(drInitialListItem(TextFieldName))(SequenceFieldName) = drInitialListItem(SequenceFieldName)
                Else
                    sbInitialItems.Append(String.Format("'{0}',", drInitialListItem(ValueFieldName)))
                    dtSourceListItems.Rows.Find(drInitialListItem(ValueFieldName))(SequenceFieldName) = drInitialListItem(SequenceFieldName)
                End If
                'source now = A1, B2, C1, D2
            End If
        Next

        If sbInitialItems.Length > 0 Then

            Dim draFilteredSourceListItems As DataRow()

            If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                draFilteredSourceListItems = dtSourceListItems.Select(String.Format("{0} not in ({1})", TextFieldName, sbInitialItems.ToString.TrimEnd(","c)), SequenceFieldName)
            Else
                draFilteredSourceListItems = dtSourceListItems.Select(String.Format("{0} not in ({1})", ValueFieldName, sbInitialItems.ToString.TrimEnd(","c)), SequenceFieldName)
            End If

            Dim i As Integer = 1
            For Each drSourceRow As DataRow In draFilteredSourceListItems  'filtered = A1, B2
                If drSourceRow(TextFieldName).ToString <> String.Empty Then
                    drSourceRow(SequenceFieldName) = i + dtListBoxItems.Rows.Count 'Source now = A3, B4, C1, D2
                    i += 1
                End If
            Next

        End If

    End Sub

    Public Sub ClearEntry()
        'Called by the parent page to clear the textbox and selected item in the list box
        txtSourceEntry.Text = String.Empty
        lbListBox.SelectedIndex = -1
    End Sub

    Public Sub ClearLists()

        'Called by the parent page to reset the control
        _dtSourceListItems = Nothing
        _dtListBoxItems = Nothing
        RemoveSessionItems()
        lbListBox.Items.Clear()
        ddlSourceList.Items.Clear()
        txtSourceEntry.Text = String.Empty

    End Sub

    Private Sub LoadPersistedDataFromSession()

        _dtSourceListItems = CType(Session("dtSourceListItems" & UniqueID), DataTable)
        _dtListBoxItems = CType(Session("dtListBoxItems" & UniqueID), DataTable)

    End Sub

    Private Sub PersistDataToSession()

        Session("dtSourceListItems" & UniqueID) = _dtSourceListItems
        Session("dtListBoxItems" & UniqueID) = _dtListBoxItems

    End Sub

    Private Sub RemoveSessionItems()

        Session.Remove("dtSourceListItems" & UniqueID)
        Session.Remove("dtListBoxItems" & UniqueID)

    End Sub

    Private Sub SetupImages()

        lbListBox.Enabled = Me.Enabled

        If ibtnAddConfirm.Enabled Then
            If EntryMode = EntryModes.TextBox Then
                If lbListBox.SelectedIndex = -1 Then
                    ibtnAddConfirm.ImageUrl = "~/images/Plus16.png"
                Else
                    ibtnAddConfirm.ImageUrl = "~/images/ok16.png"
                End If
            Else
                ibtnAddConfirm.ImageUrl = "~/images/AddAll16.png"
            End If

        Else
            If EntryMode = EntryModes.TextBox Then
                ibtnAddConfirm.ImageUrl = "~/images/PlusBW16.png"
            Else
                ibtnAddConfirm.ImageUrl = "~/images/AddAllBW16.png"
            End If
        End If

        If ibtnMoveDown.Enabled Then
            ibtnMoveDown.ImageUrl = "~/images/down16.png"
        Else
            ibtnMoveDown.ImageUrl = "~/images/DownBW16.png"
        End If

        If ibtnMoveUp.Enabled Then
            ibtnMoveUp.ImageUrl = "~/images/up16.png"
        Else
            ibtnMoveUp.ImageUrl = "~/images/UpBW16.png"
        End If

        If ibtnDelete.Enabled Then
            ibtnDelete.ImageUrl = "~/images/Delete16.png"
        Else
            ibtnDelete.ImageUrl = "~/images/DeleteBW16.png"
        End If

    End Sub

    Private Sub SetHeight()
        'Set the listbox control's height based on the auto resize mode
        Select Case AutoResizeMode

            Case AutoResizeModes.ResizeToItems
                If lbListBox.Items IsNot Nothing AndAlso lbListBox.Items.Count > DEFAULT_HEIGHT_IN_ROWS Then
                    lbListBox.Rows = lbListBox.Items.Count
                Else
                    lbListBox.Rows = DEFAULT_HEIGHT_IN_ROWS
                End If
            Case AutoResizeModes.NoResize
                lbListBox.Height = Unit.Pixel(Height - DEFAULT_ROW_HEIGHT)
        End Select
    End Sub

    Private Sub SetControlDisplays()

        If EntryMode = EntryModes.TextBox Then
            txtSourceEntry.Visible = True
            ddlSourceList.Visible = False
        Else
            txtSourceEntry.Visible = False
            ddlSourceList.Visible = True
        End If

        If AllowReorder Then
            tdArrowsCell.Style.Item("display") = ""
        Else
            tdArrowsCell.Style.Item("display") = "none"
        End If

    End Sub

    Private Sub SetControlEnables()

        txtSourceEntry.Enabled = Me.Enabled
        ddlSourceList.Enabled = Me.Enabled
        ibtnMoveDown.Enabled = Me.Enabled
        ibtnMoveUp.Enabled = Me.Enabled

        ibtnAddConfirm.Enabled =
            AddConfirmButtonEnabled AndAlso
            Me.Enabled AndAlso
            ((EntryMode = EntryModes.TextBox AndAlso (lbListBox.SelectedIndex <> -1 OrElse lbListBox.Items.Count < MaxListItemsCount)) OrElse
             (ddlSourceList.Items.Count > 1 AndAlso lbListBox.Items.Count < MaxListItemsCount))

        ibtnDelete.Enabled = Me.Enabled AndAlso DeleteButtonEnabled

        ddlSourceList.Enabled = Me.Enabled AndAlso SourceListDDLEnabled

        If Not CanBeEmpty AndAlso lbListBox.Items.Count = 1 Then
            ibtnDelete.Enabled = False
        End If

        lbListBox.Enabled = True

        If Me.Enabled Then
            lbListBox.Attributes.Remove("readonly")
        Else
            lbListBox.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Private Function GetSourceListTableFilteredWithoutListItems() As DataTable

        'Returns the source list data table minus all the items already added to the list box

        Dim dtFilteredSourceList As New DataTable()
        dtFilteredSourceList = _dtSourceListItems.Copy

        Dim dr As DataRow
        Dim primaryKey As String

        For Each dr In _dtListBoxItems.Rows

            If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                primaryKey = dr(TextFieldName).ToString
            Else
                primaryKey = dr(ValueFieldName).ToString
            End If

            If _dtSourceListItems.Rows.Find(primaryKey) IsNot Nothing Then
                dtFilteredSourceList.Rows.Remove(dtFilteredSourceList.Rows.Find(primaryKey))
            End If

        Next

        Return dtFilteredSourceList

    End Function

    Private Sub BindSourceList()
        'Bind the sourcelist ddl to the a dataview created from the sorted and filtered sourcelist table

        ddlSourceList.DataSource = New DataView(GetSourceListTableFilteredWithoutListItems, "", TextFieldName, DataViewRowState.CurrentRows)
        ddlSourceList.DataTextField = TextFieldName
        ddlSourceList.DataValueField = ValueFieldName
        ddlSourceList.DataBind()

    End Sub

    Private Sub BindListBox()

        Dim sortExpression As String = String.Empty

        If SortMode = SortModes.Alphabetical Then
            sortExpression = TextFieldName
        ElseIf SortMode = SortModes.Sequential Then
            sortExpression = SequenceFieldName
        End If

        lbListBox.DataSource = New DataView(_dtListBoxItems, "", sortExpression, DataViewRowState.CurrentRows)
        lbListBox.DataTextField = TextFieldName
        lbListBox.DataValueField = ValueFieldName
        lbListBox.DataBind()

    End Sub

    Private Sub MoveItem(direction As CommonEnums.MoveDirections)

        If lbListBox.SelectedIndex <> -1 Then

            Dim dr As DataRow
            Dim sourceDataText As String = lbListBox.SelectedItem.Text
            Dim sourceDataValue As String = lbListBox.SelectedValue
            Dim primaryKey As String

            If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                primaryKey = sourceDataText
            Else
                primaryKey = sourceDataValue
            End If

            If EntryMode = EntryModes.DropDownList Then
                dr = _dtListBoxItems.Rows.Find(primaryKey)
                CommonUtilities.MoveRow(_dtListBoxItems, SequenceFieldName, dr, direction)
                BindSourceList()
                For Each dr In _dtSourceListItems.Rows
                    If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                        dr(SequenceFieldName) = _dtSourceListItems.Rows.Find(dr(TextFieldName))(SequenceFieldName)
                    Else
                        dr(SequenceFieldName) = _dtSourceListItems.Rows.Find(dr(ValueFieldName))(SequenceFieldName)
                    End If
                Next
            Else
                dr = _dtListBoxItems.Rows.Find(primaryKey)
                CommonUtilities.MoveRow(_dtListBoxItems, SequenceFieldName, dr, direction)
            End If

            BindListBox()

            If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                lbListBox.SelectedIndex = lbListBox.Items.IndexOf(lbListBox.Items.FindByText(primaryKey))
            Else
                lbListBox.SelectedIndex = lbListBox.Items.IndexOf(lbListBox.Items.FindByValue(primaryKey))
            End If

            IsDirty = True

            If direction = CommonEnums.MoveDirections.Down Then
                RaiseEvent ItemMovedDown(Me, New ComboListBoxEventArgs(dr))
            Else
                RaiseEvent ItemMovedUp(Me, New ComboListBoxEventArgs(dr))
            End If

        End If

    End Sub

    Private Sub SetJavaScripts()

        If EntryMode = EntryModes.TextBox Then

            Dim txtSourceEntryOnKeyPress As New System.Text.StringBuilder
            Dim txtSourceEntryOnPaste As New System.Text.StringBuilder

            With txtSourceEntryOnKeyPress
                .Append("handleTextBoxOnKeyPress(")
                .Append(MaxListItemLength.ToString)
                .Append(",'")
                .Append(ibtnAddConfirm.ClientID)
                .Append("', event);")
            End With
            With txtSourceEntryOnPaste
                .Append("handleTextBoxOnPaste(")
                .Append(MaxListItemLength.ToString)
                .Append(",'")
                .Append(txtSourceEntry.ClientID)
                .Append("','")
                .Append("', event);")
            End With

            txtSourceEntry.MaxLength = MaxListItemLength
            txtSourceEntry.Attributes.Add("onkeypress", txtSourceEntryOnKeyPress.ToString)
            txtSourceEntry.Attributes.Add("onpaste", txtSourceEntryOnPaste.ToString)

        End If

    End Sub

    Private Sub RemoveItemRow(dr As DataRow)

        'If the list is sequenced and the entry mode is TextBox, delete the row from the table
        'using the DeleteRow utility function to keep the sequence numbers in sync.
        'Otherwise, just remove the row from the table
        If IsSequencedList AndAlso EntryMode = EntryModes.TextBox Then
            CommonUtilities.DeleteRow(_dtListBoxItems, SequenceFieldName, dr)
        Else
            _dtListBoxItems.Rows.Remove(dr)
        End If

        If IsSequencedList AndAlso EntryMode = EntryModes.DropDownList AndAlso SortMode = SortModes.Sequential Then
            ResequenceSourceList(_dtSourceListItems, _dtListBoxItems)
        End If
        _dtListBoxItems.AcceptChanges()
        lbListBox.SelectedIndex = -1

        If ViewState("SelectedItemKey") IsNot Nothing Then
            ViewState("SelectedItemKey") = Nothing
        End If

        'Bind the modified table to the listbox
        BindListBox()

    End Sub

    Public Sub RemoveItemByItemKey(ItemKey As String)

        Dim dr As DataRow

        If CommonUtilities.IsValidInteger(ItemKey) Then
            dr = _dtListBoxItems.Rows.Find(CInt(ItemKey))
        Else
            dr = _dtListBoxItems.Rows.Find(ItemKey)
        End If

        RemoveItemRow(dr)

        'After the row is deleted above, if the entry mode = dropdownlist, bind the source list
        'to filter out the (one item smaller) listboxitems
        If EntryMode = EntryModes.DropDownList Then
            BindSourceList()
        End If

    End Sub

    Private Sub RemoveSelectedItem()

        Dim dr As DataRow

        'Get the appropriate primary search key (Text or Value) based on the PrimaryKeyOption set for the control
        If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
            dr = _dtListBoxItems.Rows.Find(lbListBox.SelectedItem.Text.Trim)
        Else
            If CommonUtilities.IsValidInteger(lbListBox.SelectedValue) Then
                dr = _dtListBoxItems.Rows.Find(CInt(lbListBox.SelectedValue))
            Else
                dr = _dtListBoxItems.Rows.Find(CStr(lbListBox.SelectedValue).Trim)
            End If
        End If

        RemoveItemRow(dr)

        'After the row is deleted above, if the entry mode = dropdownlist, bind the source list
        'to filter out the (one item smaller) listboxitems. Otherwise, just clear the source text box's text.
        If EntryMode = EntryModes.DropDownList Then
            BindSourceList()
        Else
            txtSourceEntry.Text = String.Empty
        End If

    End Sub

    Public Shared Function GetEmptyFormatedTable(
        Optional primaryKeyOption As PrimaryKeyOptions = PrimaryKeyOptions.ItemValue,
        Optional textFieldName As String = "ListItemText",
        Optional valueFieldName As String = "ListItemValue",
        Optional sequenceFieldName As String = "ItemSequence") As DataTable

        'Utility function returning an empty datatable structured for use by the control

        Dim dtCurrentItems As New DataTable
        CommonUtilities.AddTableColumn(dtCurrentItems, textFieldName, GetType(String))
        CommonUtilities.AddTableColumn(dtCurrentItems, valueFieldName, GetType(String))
        CommonUtilities.AddTableColumn(dtCurrentItems, sequenceFieldName, GetType(Integer))

        dtCurrentItems.Columns(1).AllowDBNull = True

        'Add a primary Key to the Table
        Dim PrimaryKey(0) As DataColumn
        If primaryKeyOption = PrimaryKeyOptions.ItemValue Then
            PrimaryKey(0) = dtCurrentItems.Columns(valueFieldName)
            primaryKeyOption = PrimaryKeyOptions.ItemValue
        Else
            PrimaryKey(0) = dtCurrentItems.Columns(textFieldName)
            primaryKeyOption = PrimaryKeyOptions.ItemText
        End If
        dtCurrentItems.PrimaryKey = PrimaryKey
        Return dtCurrentItems
    End Function

    Public Shared Function ConvertDelimitedStringToRequiredFormat(
        currentValue As String,
        delimiterChar As Char,
        Optional subDelimiterChar As Char = Char.MinValue) As String

        'Utility function to convert a delimited string into a format compatible with the control

        If delimiterChar = "" OrElse currentValue = String.Empty Then
            Return currentValue
        End If

        Dim sbFormatted As New StringBuilder
        Dim items() As String = currentValue.Split(delimiterChar)
        Dim iCount As Integer = 0

        For Each item As String In items
            With sbFormatted
                If subDelimiterChar = Char.MinValue Then
                    .Append(item)
                    .Append(",")
                    .Append(item)
                    .Append(",")
                Else
                    .Append(item.Split(subDelimiterChar)(0))
                    .Append(",")
                    .Append(item.Split(subDelimiterChar)(1))
                    .Append(",")
                End If
                .Append(iCount.ToString)
                .Append(Chr(27))
            End With
            iCount += 1
        Next

        Return sbFormatted.ToString

    End Function

    Public Shared Function GetFormatedTableFromString(
        formattedValues As String,
        Optional primaryKeyOption As PrimaryKeyOptions = PrimaryKeyOptions.ItemValue,
        Optional textFieldName As String = "ListItemText",
        Optional valueFieldName As String = "ListItemValue") As DataTable

        'Utility function that converts a double delimited string containing text, value and sequence numbers into a data table structered for use by the control

        Dim currentValues() As String
        Dim dr As DataRow

        Dim dtTable As DataTable = GetEmptyFormatedTable(primaryKeyOption, textFieldName, valueFieldName)

        'If there are current values, populate the dtCurrentItems with the parsed information currentvalues
        'If there is only one undelimited item, then fill a single row of the table using that item

        If Not String.IsNullOrEmpty(formattedValues) Then
            currentValues = formattedValues.Split(Chr(27))
            For Each s As String In currentValues
                If s.Length > 0 Then
                    Dim Items() As String = s.Split(","c)
                    dr = dtTable.NewRow
                    dr.Item(textFieldName) = Items(0)
                    dr.Item(valueFieldName) = Items(1)
                    dr.Item("ItemSequence") = Items(2)
                    dtTable.Rows.Add(dr)
                End If
            Next
        End If

        Return dtTable

    End Function

    Public Overloads Shared Function GetFormatedTable(
        dtSource As DataTable,
        textFieldName As String,
        valueFieldName As String,
        Optional sequenceFieldName As String = "",
        Optional primaryKeyOption As PrimaryKeyOptions = PrimaryKeyOptions.ItemValue,
        Optional addBlank As Boolean = False,
        Optional allowDuplicates As Boolean = False) As DataTable

        'Utility function that returns a data table structed for use by the control using an already filled data table as its source

        Return ComboListBox.GetFormatedTable(dtSource.Select(), textFieldName, valueFieldName, sequenceFieldName, primaryKeyOption, addBlank, allowDuplicates)

    End Function

    Public Overloads Shared Function GetFormatedTable(
        draSource As DataRow(),
        textFieldName As String,
        valueFieldName As String,
        Optional sequenceFieldName As String = "",
        Optional primaryKeyOption As PrimaryKeyOptions = PrimaryKeyOptions.ItemValue,
        Optional addBlank As Boolean = False,
        Optional allowDuplicates As Boolean = False) As DataTable

        'Utility function that returns a data table structed for use by the control using an already filled datarow array as its source

        Dim dtSource As DataTable = GetEmptyFormatedTable(primaryKeyOption, textFieldName, valueFieldName)
        Dim drNewRow As DataRow
        Dim iCount As Integer = 0

        If addBlank Then
            drNewRow = dtSource.NewRow
            drNewRow(textFieldName) = ""
            drNewRow(valueFieldName) = -1
            drNewRow("ItemSequence") = -1
            dtSource.Rows.Add(drNewRow)
        End If

        For Each dr As DataRow In draSource
            If Not IsDBNull(dr(textFieldName)) AndAlso CStr(dr(textFieldName)).Length > 0 Then

                If allowDuplicates OrElse dtSource.Select(String.Format("{0}='{1}'", textFieldName, dr(textFieldName).ToString.Replace("'", "''"), "", DataViewRowState.CurrentRows), "", DataViewRowState.CurrentRows).Length = 0 Then
                    drNewRow = dtSource.NewRow
                    drNewRow(textFieldName) = dr(textFieldName)
                    drNewRow(valueFieldName) = dr(valueFieldName)
                    If sequenceFieldName.Length > 0 Then
                        drNewRow("ItemSequence") = dr(sequenceFieldName)
                    Else
                        drNewRow("ItemSequence") = iCount
                        iCount += 1
                    End If
                    dtSource.Rows.Add(drNewRow)
                End If

            End If
        Next

        'Add a primary Key to the Table
        Dim PrimaryKey(0) As DataColumn
        If primaryKeyOption = PrimaryKeyOptions.ItemValue Then
            PrimaryKey(0) = dtSource.Columns(valueFieldName)
            primaryKeyOption = PrimaryKeyOptions.ItemValue
        Else
            PrimaryKey(0) = dtSource.Columns(textFieldName)
            primaryKeyOption = PrimaryKeyOptions.ItemText
        End If
        dtSource.PrimaryKey = PrimaryKey

        Return dtSource

    End Function
#End Region

#Region "Page and Element Event Handlers"

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack Then
            LoadPersistedDataFromSession()
        End If

    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles MyBase.PreRender

        If ViewState("SelectedItemKey") IsNot Nothing AndAlso lbListBox.SelectedIndex = -1 Then
            lbListBox.SelectedIndex = lbListBox.Items.IndexOf(lbListBox.Items.FindByValue(ViewState("SelectedItemKey").ToString))
        End If

        'Setup the display and position styles of the controls
        SetControlDisplays()

        'Had to do this here cuase setting the enable states in the property wasn't sticking due to the page lifecycle
        SetControlEnables()

        'Setup the control's images
        SetupImages()

        SetHeight()

        SetJavaScripts()

        If IsClosing Then
            RemoveSessionItems()
        Else
            PersistDataToSession()
        End If

    End Sub

    Protected Sub ibtnAddConfirm_Click(sender As System.Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnAddConfirm.Click

        If EntryMode = EntryModes.TextBox Then

            Dim dr As DataRow
            Dim primaryKey As String
            Dim selItemKey As String = String.Empty

            'Set the Text and Value data that will be used for the new list item
            'to the text entered in the entry box
            Dim sourceDataText As String = txtSourceEntry.Text.Trim
            Dim sourceDataValue As String = txtSourceEntry.Text.Trim
            Dim selectedText As String = String.Empty
            Dim selectedValue As String = String.Empty

            'Only add/change the item if the source data text box is not empty
            If sourceDataText <> String.Empty Then

                'Get the appropriate primary search key (Text or Value) based on the
                'PrimaryKeyOption set for the control
                If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                    primaryKey = sourceDataText
                Else
                    primaryKey = sourceDataValue
                End If

                'Get the selected item's key Text or Value) based on the
                'PrimaryKeyOption set for the control
                If lbListBox.SelectedIndex <> -1 Then
                    selectedText = lbListBox.SelectedItem.Text
                    selectedValue = lbListBox.SelectedItem.Value
                    If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                        selItemKey = lbListBox.SelectedItem.Text
                    Else
                        selItemKey = lbListBox.SelectedValue
                    End If
                End If

                'Find the datarow in the table bound to the ListBox using the primary key
                dr = _dtListBoxItems.Rows.Find(primaryKey)

                'Warn to prevent adding the duplicate row by first checking if the new item (primary key)
                'already exists in the table (i.e. dr is not nothing).
                'If a row was found, check if an item is selected in the listbox.  If there is
                'no item selected then have to warn. If an item IS selected, 
                'then check if the primary key matches the selected items key.
                'If it does not match, it means that the item already exists in the listbox and is one
                'other than the selected item so will need to warn.
                If dr IsNot Nothing AndAlso (lbListBox.SelectedIndex = -1 OrElse primaryKey.ToUpper <> selItemKey.ToUpper) Then
                    'SetErrorMessage("List may not contain duplicate items")

                Else
                    'If a deleted row was found in the list box, delete it completely (the new data will
                    'replace the old row).  This will happen if a row is deleted and a new row with
                    'identical data is added again
                    If dr IsNot Nothing AndAlso dr.RowState = DataRowState.Deleted Then
                        dr.AcceptChanges()
                    End If

                    'Create a row from the Listbox's data table
                    dr = _dtListBoxItems.NewRow

                    'If there is no selected item in the listbox, ADD a new row to the end of the table
                    'Otherwise, CHANGE the values of the selected  item

                    Dim AddingNewItem As Boolean

                    If lbListBox.SelectedIndex = -1 Then

                        AddingNewItem = True

                        'Set the new row's data based on the entered info in the text box
                        dr(TextFieldName) = sourceDataText
                        dr(ValueFieldName) = sourceDataValue

                        'If the list is sequenced, add the row to the end of the table
                        'using the InsertRow utility function to keep the sequence numbers in sync.
                        'Otherwise, just add the row to the table
                        If IsSequencedList Then
                            If lbListBox.Items.Count > 0 Then
                                Dim drLastRow As DataRow
                                If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                                    drLastRow = _dtListBoxItems.Rows.Find(lbListBox.Items(lbListBox.Items.Count - 1).Text)
                                Else
                                    drLastRow = _dtListBoxItems.Rows.Find(lbListBox.Items(lbListBox.Items.Count - 1).Value)
                                End If
                                dr(SequenceFieldName) = drLastRow(SequenceFieldName)
                            Else
                                dr(SequenceFieldName) = -1
                            End If
                            CommonUtilities.InsertRow(_dtListBoxItems, SequenceFieldName, dr)
                        Else
                            _dtListBoxItems.Rows.Add(dr)
                        End If
                    Else
                        AddingNewItem = False
                        'If there was an item selected, find the appropriate row and change its values based
                        'on the entered info in the text box
                        If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                            dr = _dtListBoxItems.Rows.Find(lbListBox.Items.FindByText(lbListBox.SelectedItem.Text).Text)
                            dr(TextFieldName) = sourceDataText
                            dr(ValueFieldName) = sourceDataValue
                        Else
                            dr = _dtListBoxItems.Rows.Find(lbListBox.Items.FindByValue(lbListBox.SelectedValue).Value)
                            dr(TextFieldName) = sourceDataText
                        End If
                    End If

                    'Clear the entry text box
                    txtSourceEntry.Text = String.Empty

                    'Bind the modified table to the listbox
                    BindListBox()

                    If ViewState("SelectedItemKey") IsNot Nothing Then
                        ViewState("SelectedItemKey") = Nothing
                    End If

                    lbListBox.SelectedIndex = -1

                    IsDirty = True

                    'Raise the appropriate event, passing the datarow that was added/changed
                    If AddingNewItem Then
                        RaiseEvent ItemAdded(Me, New ComboListBoxEventArgs(dr))
                    Else
                        RaiseEvent ItemModified(Me, New ComboListBoxEventArgs(dr, selectedText, selectedValue))
                    End If
                End If

            End If

        Else

            For i As Integer = 1 To ddlSourceList.Items.Count
                If ddlSourceList.Items.Count > 1 Then
                    ddlSourceList.SelectedIndex = 1
                    ddlSourceList_SelectedIndexChanged(sender, e)
                End If
            Next

        End If

    End Sub

    Private Sub ddlSourceList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSourceList.SelectedIndexChanged
        'This function adds the selected item in the source list to the listbox items
        'After the item is added, the source ddl is rebound to filter out the (one item bigger) listbox items

        Dim sourceDataText As String = ddlSourceList.SelectedItem.Text
        Dim sourceDataValue As String = ddlSourceList.SelectedValue

        Dim dr As DataRow = Nothing
        Dim primaryKey As String

        'Get the appropriate primary search key (Text or Value) based on the
        'PrimaryKeyOption set for the control
        If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
            primaryKey = sourceDataText
        Else
            primaryKey = sourceDataValue
        End If

        'If the item selected in the source ddl is not blank, add a row to the listboxitem's table
        'with the same info as the selected source ddl row (text, value)
        'May need to resquence the source list
        'bind the lists to the tables
        If sourceDataText <> String.Empty AndAlso sourceDataValue <> String.Empty Then

            dr = _dtListBoxItems.NewRow

            dr(TextFieldName) = sourceDataText
            dr(ValueFieldName) = sourceDataValue

            _dtListBoxItems.Rows.Add(dr)

            If IsSequencedList Then

                If SortMode = SortModes.Alphabetical Then
                    dr(SequenceFieldName) = _dtSourceListItems.Rows.Find(primaryKey)(SequenceFieldName)
                End If

                If SortMode = SortModes.Sequential Then
                    dr(SequenceFieldName) = Integer.MaxValue
                    ResequenceSourceList(_dtSourceListItems, _dtListBoxItems)
                End If

            End If

            ddlSourceList.SelectedIndex = -1

            BindListBox()
            BindSourceList()

        End If

        If sourceDataText <> String.Empty AndAlso sourceDataValue <> String.Empty Then
            IsDirty = True
            RaiseEvent ItemAdded(Me, New ComboListBoxEventArgs(dr))
        End If


    End Sub

    Private Sub lbListBox_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbListBox.SelectedIndexChanged

        If lbListBox.SelectedItem IsNot Nothing Then

            Dim dr As DataRow

            'If the entry mode is set to TextBox and a non-blank item was selected in the list box
            'Set the entry box's text to the value of the selected item
            If EntryMode = EntryModes.TextBox AndAlso lbListBox.SelectedItem.Text <> String.Empty Then
                txtSourceEntry.Text = lbListBox.SelectedItem.Text
            End If

            'Get the row of the table bound to the list box based on the primary key option set for the control
            If PrimaryKeyOption = PrimaryKeyOptions.ItemText Then
                dr = _dtListBoxItems.Rows.Find(lbListBox.Items.FindByText(lbListBox.SelectedItem.Text).Text)
                ViewState("SelectedItemKey") = lbListBox.SelectedItem.Text
            Else
                dr = _dtListBoxItems.Rows.Find(lbListBox.Items.FindByValue(lbListBox.SelectedValue).Value)
                ViewState("SelectedItemKey") = lbListBox.SelectedValue
            End If


            'Raise the ItemSelected event, passing the datarow corresponding to the selected item
            RaiseEvent ItemSelected(Me, New ComboListBoxEventArgs(dr))

        End If

    End Sub

    Protected Sub ibtnDelete_Click(sender As System.Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click

        'This function handle's the delete button click.
        'If an item is selected it will be deleted from the list box as well as its bound data table.
        If lbListBox.SelectedIndex <> -1 Then
            Dim DeletedDataText As String = lbListBox.SelectedItem.Text
            Dim DeletedDataValue As String = lbListBox.SelectedValue

            RemoveSelectedItem()

            IsDirty = True

            If ViewState("SelectedItemKey") IsNot Nothing Then
                ViewState("SelectedItemKey") = Nothing
            End If

            RaiseEvent ItemDeleted(Me, New ComboListBoxEventArgs(DeletedDataValue, DeletedDataText))

            'Prevent deleting of the last item in the listbox if the control as been set to do so
            If Not CanBeEmpty AndAlso lbListBox.Items.Count = 1 Then
                Me.ibtnDelete.Enabled = False
            End If

        End If

    End Sub

    Protected Sub ibtnMoveUp_Click(sender As System.Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnMoveUp.Click

        MoveItem(CommonEnums.MoveDirections.Up)

    End Sub

    Protected Sub ibtnMoveDown_Click(sender As System.Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnMoveDown.Click

        MoveItem(CommonEnums.MoveDirections.Down)

    End Sub
#End Region

End Class
