<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="videoaudioother.ascx.cs" Inherits="GalleryServerPro.Web.Pages.Admin.videoaudioother" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"	TagPrefix="tis" %>
<div class="gsp_indentedContent">
	<asp:PlaceHolder ID="phAdminHeader" runat="server" />
		<div class="gsp_addpadding1">
		<p class="gsp_msgdark">
			<asp:Label ID="lbl1" runat="server" CssClass="gsp_bold" Text="<%$ Resources:GalleryServerPro, Admin_Settings_Apply_To_Label %>"
				EnableViewState="false" />&nbsp;<asp:Label ID="lblGalleryDescription" runat="server"
					EnableViewState="false" /></p>
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<p class="admin_h3" style="margin-top: 0;">
			<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_General_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<p>
				<asp:CheckBox ID="chkAutoStart" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AutoStart_Label %>" />
			</p>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoSettings_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<table class="gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblVideoPlayerWidth" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerWidth_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtVideoPlayerWidth" runat="server" />&nbsp;<asp:RangeValidator ID="rvVideoPlayerWidth"
							runat="server" Display="Dynamic" ControlToValidate="txtVideoPlayerWidth" Type="Integer"
							MinimumValue="0" MaximumValue="10000" Text="<%$ Resources:GalleryServerPro, Validation_Int_0_To_10000_Text %>" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblVideoPlayerHeight" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerHeight_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtVideoPlayerHeight" runat="server" />&nbsp;<asp:RangeValidator ID="rvVideoPlayerHeight"
							runat="server" Display="Dynamic" ControlToValidate="txtVideoPlayerHeight" Type="Integer"
							MinimumValue="0" MaximumValue="10000" Text="<%$ Resources:GalleryServerPro, Validation_Int_0_To_10000_Text %>" /></td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblVideoThumbnailPosition" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoThumbnailPosition_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtVideoThumbnailPosition" runat="server" />&nbsp;<asp:RangeValidator ID="rvVideoThumbnailPosition"
							runat="server" Display="Dynamic" ControlToValidate="txtVideoThumbnailPosition" Type="Integer"
							MinimumValue="0" MaximumValue="86400" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_Validation_VideoThumbnailPosition_Text %>" /></td>
				</tr>
			</table>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioSettings_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<table class="gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblAudioPlayerWidth" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerWidth_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtAudioPlayerWidth" runat="server" />&nbsp;<asp:RangeValidator ID="rvAudioPlayerWidth"
							runat="server" Display="Dynamic" ControlToValidate="txtAudioPlayerWidth" Type="Integer"
							MinimumValue="0" MaximumValue="10000" Text="<%$ Resources:GalleryServerPro, Validation_Int_0_To_10000_Text %>" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblAudioPlayerHeight" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerHeight_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtAudioPlayerHeight" runat="server" />&nbsp;<asp:RangeValidator ID="rvAudioPlayerHeight"
							runat="server" Display="Dynamic" ControlToValidate="txtAudioPlayerHeight" Type="Integer"
							MinimumValue="0" MaximumValue="10000" Text="<%$ Resources:GalleryServerPro, Validation_Int_0_To_10000_Text %>" /></td>
				</tr>
			</table>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_OtherSettings_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<table class="gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblGenericWidth" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericWidth_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtGenericWidth" runat="server" />&nbsp;<asp:RangeValidator ID="rvGenericWidth"
							runat="server" Display="Dynamic" ControlToValidate="txtGenericWidth" Type="Integer"
							MinimumValue="0" MaximumValue="10000" Text="<%$ Resources:GalleryServerPro, Validation_Int_0_To_10000_Text %>" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblGenericHeight" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericHeight_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtGenericHeight" runat="server" />&nbsp;<asp:RangeValidator ID="rvGenericHeight"
							runat="server" Display="Dynamic" ControlToValidate="txtGenericHeight" Type="Integer"
							MinimumValue="0" MaximumValue="10000" Text="<%$ Resources:GalleryServerPro, Validation_Int_0_To_10000_Text %>" /></td>
				</tr>
			</table>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="wbi1" runat="server" BindingSource="GallerySettingsUpdateable"
				BindingSourceMember="AutoStartMediaObject" ControlId="chkAutoStart" BindingProperty="Checked"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AutoStart_Label %>" />
			<tis:wwDataBindingItem ID="wbi2" runat="server" BindingSource="GallerySettingsUpdateable"
				BindingSourceMember="DefaultVideoPlayerWidth" ControlId="txtVideoPlayerWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerWidth_Label %>" />
			<tis:wwDataBindingItem ID="wbi3" runat="server" BindingSource="GallerySettingsUpdateable"
				BindingSourceMember="DefaultVideoPlayerHeight" ControlId="txtVideoPlayerHeight"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerHeight_Label %>" />
			<tis:wwDataBindingItem ID="wbi4" runat="server" BindingSource="GallerySettingsUpdateable"
				BindingSourceMember="VideoThumbnailPosition" ControlId="txtVideoThumbnailPosition"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoThumbnailPosition_Label %>" />
			<tis:wwDataBindingItem ID="wbi5" runat="server" BindingSource="GallerySettingsUpdateable"
				BindingSourceMember="DefaultAudioPlayerWidth" ControlId="txtAudioPlayerWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerWidth_Label %>" />
			<tis:wwDataBindingItem ID="wbi6" runat="server" BindingSource="GallerySettingsUpdateable"
				BindingSourceMember="DefaultAudioPlayerHeight" ControlId="txtAudioPlayerHeight"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerHeight_Label %>" />
			<tis:wwDataBindingItem ID="wbi7" runat="server" BindingSource="GallerySettingsUpdateable"
				BindingSourceMember="DefaultGenericObjectWidth" ControlId="txtGenericWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericWidth_Label %>" />
			<tis:wwDataBindingItem ID="wbi8" runat="server" BindingSource="GallerySettingsUpdateable"
				BindingSourceMember="DefaultGenericObjectHeight" ControlId="txtGenericHeight" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericHeight_Label %>" />
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="poi1" runat="server" ControlId="chkAutoStart" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_autoStartMediaObject_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_autoStartMediaObject_Bdy %>" />
			<tis:PopupInfoItem ID="poi2" runat="server" ControlId="lblVideoPlayerWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerWidth_Bdy %>" />
			<tis:PopupInfoItem ID="poi3" runat="server" ControlId="lblVideoPlayerHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerHeight_Bdy %>" />
			<tis:PopupInfoItem ID="poi4" runat="server" ControlId="lblVideoThumbnailPosition"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_VideoThumbnailPosition_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_VideoThumbnailPosition_Bdy %>" />
			<tis:PopupInfoItem ID="poi5" runat="server" ControlId="lblAudioPlayerWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerWidth_Bdy %>" />
			<tis:PopupInfoItem ID="poi6" runat="server" ControlId="lblAudioPlayerHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerHeight_Bdy %>" />
			<tis:PopupInfoItem ID="poi7" runat="server" ControlId="lblGenericWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectWidth_Bdy %>" />
			<tis:PopupInfoItem ID="poi8" runat="server" ControlId="lblGenericHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectHeight_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<asp:PlaceHolder ID="phAdminFooter" runat="server" />
</div>