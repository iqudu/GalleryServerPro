<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="taskfooter.ascx.cs" Inherits="GalleryServerPro.Web.Controls.Task.taskfooter" %>
	<div class="gsp_rightBottom">
		<p class="gsp_minimargin">
			<asp:Button ID="btnOkBottom" runat="server" Text="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>" />
			<asp:Button ID="btnCancelBottom" runat="server" OnClick="btnCancel_Click" CausesValidation="false"
				Text="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Text %>" ToolTip="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Tooltip %>" />&nbsp;</p>
	</div>
