<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ComboListBox.ascx.vb" Inherits="ComboListBoxSample.ComboListBox" %>
<asp:Panel ID="pnlComboListBoxWrapper" runat="server" CssClass="CLBWrapper">
	<table>
		<tr>
			<td class="CLBAddButtonCell">
				<asp:ImageButton ID="ibtnAddConfirm" runat="server" ImageUrl="~/Images/Plus16.png" CssClass="CLBAddButton"></asp:ImageButton>
			</td>
			<td class="CLBEntryCell" id="tdEntryCell" runat="server">
				<asp:TextBox ID="txtSourceEntry" runat="server" CssClass="CLBTextBox"></asp:TextBox><asp:DropDownList
						ID="ddlSourceList" runat="server" CssClass="CLBDropDownList" AutoPostBack="True">
					</asp:DropDownList>
			</td>
		</tr>
	</table>
	<table>
		<tr>
			<td class="CLBArrowsCell" id="tdArrowsCell" runat="server">
				<asp:ImageButton ID="ibtnMoveUp" runat="server" ImageUrl="~/Images/Up16.png" CssClass="CLBUpArrow"></asp:ImageButton><br />
				<asp:ImageButton ID="ibtnMoveDown" runat="server" ImageUrl="~/Images/Down16.png" CssClass="CLBDownArrow"></asp:ImageButton>
			</td>
			<td class="CLBListBoxCell">
				<asp:ListBox ID="lbListBox" runat="server" CssClass="CLBListBox" AutoPostBack="True" width="100%"></asp:ListBox>
			</td>
			<td class="CLBDeleteButtonCell">
				<asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/Delete16.png" CssClass="CLBDeleteButton"></asp:ImageButton>
			</td>
		</tr>
	</table>
</asp:Panel>