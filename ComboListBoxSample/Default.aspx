<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ComboListBoxSample._Default" %>

<%@ Register Src="ComboListBox.ascx" TagName="ComboListBox" TagPrefix="Arcnous" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/_Common.css" type="text/css" rel="stylesheet" />
    <link href="CSS/ComboListBox.css" type="text/css" rel="stylesheet" />
</head>
<script src="js/ComboListBox.js"></script>
<body>
    <form id="form1" runat="server">
        <div style="display: inline-block; width: 45%; margin-right: 20px">
            <span>This combo list box control is set to text box entry mode. For this mode, each item's data is represented by a single text value. Things you can do:</span><br />
            <ul>
                <li>Enter new items and add them to the list (either with the ENTER key or with the PLUS button)</li>
                <li>Select an item in the list box and modify it</li>
                <li>Select an item in the list box and reorder it</li>
                <li>Select an item in the list box and delete it</li>
            </ul>
            <Arcnous:ComboListBox ID="clb01" runat="server" Width="100%"></Arcnous:ComboListBox>
        </div>
        <div style="display: inline-block; width: 45%">
            <span>This combo list box control is set to drop down list mode. For this mode, each item's data is typically represented by a text-value pair. Things you can do:</span><br />
            <ul>
                <li>Select an item from the source list to add it to the list box</li>
                <li>Add all items from the source list at the same time using the double PLUS button</li>
                <li>Select an item in the list box and modify it</li>
                <li>Select an item in the list box and reorder it</li>
                <li>Select an item in the list box and delete it</li>
            </ul>
            <Arcnous:ComboListBox ID="clb02" runat="server" Width="100%"></Arcnous:ComboListBox>
        </div>
        <div style="margin-top: 10px">
            <span>The control is designed specifically for our application and uses internal datatables to manage the data. It exists in various screens where users can enter new items or select multiple ones. The control can start empy (as these), or it can be pre-populated with items that have already been entered/selected by users for additional editing. The typical use of the control is to intialize it's properties and then pass configured datatables that will be populated as users add/edit values through the front-end. The data are then read by the parent screen and sent to a business object for storage into database tables. The control also fires various events that are available to the parent page so that it can react to user interaction if it needs to. In addition to the two main modes, the control has various other options set via its properties, such as:</span><br />
            <ul>
                <li>Not allow the user to manual reorder the list box items and instead set the control to auto-sort either alphabetically, by sequence number, or not at all</li>
                <li>Set the maximum number of items allowed in the list box</li>
                <li>Set the maximum number characters allowed for each entry</li>
                <li>Set the control to autosize itself based on the parent control height (not supported in this test page) or based on the number of items</li>
            </ul>
        </div>
    </form>
</body>
</html>
