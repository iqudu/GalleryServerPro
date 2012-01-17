<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="taskheader.ascx.cs" Inherits="GalleryServerPro.Web.Controls.Task.taskheader" %>
<asp:Panel runat="server" CssClass="gsp_addleftpadding1">
	<div style="float: right">
		<p class="gsp_minimargin">
			<asp:Button ID="btnOkTop" runat="server" Text="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>" />
			<asp:Button ID="btnCancelTop" runat="server" OnClick="btnCancel_Click" CausesValidation="false"
				Text="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Text %>" ToolTip="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Tooltip %>" />&nbsp;</p>
	</div>
	<p class="gsp_h1">
		<asp:Label ID="lblTaskHeader" runat="server" /></p>
	<p class="gsp_taskBody">
		<asp:Label ID="lblTaskBody" runat="server" /></p>
</asp:Panel>
