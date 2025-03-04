﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomizationProfileUserList.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.Administration.Modules.CustomizationProfileUserList" %>
<%@ Reference Control="~/Apps/MetaUI/Toolbar/MetaToolbar.ascx" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Grid/EntityGrid.ascx" %>
<%@ Reference Control="~/Apps/MetaUI/Grid/MetaGridServerEventAction.ascx" %>
<%@ Register TagPrefix="mc" TagName="MetaToolbar" Src="~/Apps/MetaUI/Toolbar/MetaToolbar.ascx" %>
<%@ Register TagPrefix="mc" TagName="EntityGrid" Src="~/Apps/MetaUIEntity/Grid/EntityGrid.ascx" %>
<%@ Register TagPrefix="mc" TagName="MCGridAction" Src="~/Apps/MetaUI/Grid/MetaGridServerEventAction.ascx" %>

<div id="mainDiv" runat="server"> 
<table class="ibn-propertysheet" cellspacing="0" cellpadding="0" border="0" width="100%" style="table-layout: fixed">
	<tr runat="server" id="ToolbarRow">
		<td class="ibn-stylebox2noBottom">
			<mc:MetaToolbar runat="server" ID="MainMetaToolbar" ToolbarMode="ListViewUI" GridId="grdMain" />
		</td>
	</tr>
	<tr>
		<td >
			<mc:EntityGrid ID="grdMain" runat="server" ShowPaging="false" />	
			<mc:MCGridAction runat="server" ID="ctrlGridEventUpdater" />
			<asp:LinkButton ID="lbAddItems" runat="server" Visible="false" OnClick="lbAddItems_Click"></asp:LinkButton>
		</td>
	</tr>
</table>
</div>