<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addobjects.ascx.cs"
	Inherits="GalleryServerPro.Web.Pages.Task.addobjects" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Src="../../controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<%@ Import Namespace="GalleryServerPro.Web" %>
<div class="gsp_content">
	<asp:PlaceHolder ID="phTaskHeader" runat="server" />
	<div runat="server">
		<script type="text/javascript">

			$(function () {
				$('#pnlOptions').hide();
				$('#pnlOptionsHdr').click(function () {
					$('#pnlOptions').slideToggle('fast');
				});
			});
			
			function add_file(ctrl, el) {
				if (ctrl.FileCount < ctrl.MaximumFileCount) ctrl.AddFile();
			}

			function remove_file(ctrl, item) {
				if (ctrl.FileCount > 1) ctrl.RemoveFileAt(item);
			}

			function generate_file_list(ctrl, cur) {
				var files = ctrl.GetFiles();
				var out = "";
				var cls = "done";

				for (var i = 0; i < files.length; i++) {
					var file = files[i].substring(files[i].lastIndexOf("\\") + 1, files[i].length);
					var li = "<li class=\"" + cls + "\">";
					if (file == cur) {
						li = (ctrl.Uploading) ? "<li class=\"cur\">" : "<li class=\"done\">";
						cls = "";
					}
					out += li + file + "</li>";
				}

				return "<ul>" + out + "</ul>";
			}

			//	File size functions
			function format_file_size(n, fmt) {
				if (!fmt) {		//	no formatting specified; automatically select the best format
					if (n < 1000) fmt = "b";
					else if (n < 1000000) fmt = "kb";
					else if (n < 1000000000) fmt = "mb";
					else fmt = "gb";
				}

				switch (fmt.toLowerCase()) {
					case "kb": return String((n * 0.001).toFixed(2)) + " <asp:Literal ID='l11' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Kilobyte_Abbreviation %>' />"; break;
					case "mb": return String((n * 0.000001).toFixed(2)) + " <asp:Literal ID='l12' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Megabyte_Abbreviation %>' />"; break;
					case "gb": return String((n * 0.000000001).toFixed(2)) + " <asp:Literal ID='l13' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Gigabyte_Abbreviation %>' />"; break;
					default: return String(n.toFixed(2)) + " <asp:Literal ID='l14' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Byte_Abbreviation %>' />";
				}
			}

			function get_percentage(n) { return String(Math.round(n * 100)); }

			//	Time functions
			function format_time(t, txt) {
				var s = Math.floor(t);
				var m = Math.floor(s / 60);
				var h = Math.floor(m / 60);

				if (!txt) {
					//	Output will always have be least mm:ss
					s = pad_time(s % 60);
					m = pad_time(m % 60) + ":";
					h = (h == 0) ? "" : pad_time(h % 60) + ":";

					return (h + m + s);
				} else {
					var secs = (s > 1) ? "<asp:Literal ID='l15' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Seconds_Plural %>' />" : "<asp:Literal ID='l16' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Seconds_Singular %>' />"; //	plural & singular second units
					var mins = (m > 1) ? "<asp:Literal ID='l17' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Minutes_Plural %>' />" : "<asp:Literal ID='l18' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Minutes_Singular %>' />"; //	plural & singular minute units
					var hours = (h > 1) ? "<asp:Literal ID='l19' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Hours_Plural %>' />" : "<asp:Literal ID='l20' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Hours_Singular %>' />"; //	plural & singular hours units

					s = (s > 0) ? String(s) + " " + secs : "";
					m = (m > 0) ? String(m) + " " + mins : "";
					h = (h > 0) ? String(h) + " " + hours : "";

					var out = "";
					if (h !== "") { //	longer than an hour
						out = h;
						if (m != "") out += ", " + m; //	at least one minute
						if (s != "") out += ", " + s; //	at least one second
					}

					if (m !== "" && out == "") { //	shorter than an hour, greater than 60 seconds
						out += m;
						if (s != "") out += ", " + s; //	at least one second
					}

					if (s !== "" && out == "") out = s; //	at least one second

					if (out == "") out = "<asp:Literal ID='l21' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Time_Remaining_Less_Than_One_Second %>' />"; //	less than a second

					return out;
				}
			}

			function pad_time(t) { return String(((t > 9) ? "" : "0") + t); }

			function get_file_position(ctrl, cur) {
				var files = ctrl.GetFiles();
				for (var i = 0; i < files.length; i++) {
					var file = files[i].substring(files[i].lastIndexOf("\\") + 1, files[i].length);
					if (file == cur) return String(i + 1);
				}

				return "1";
			}

			function init_upload(ctrl) {
				if (ctrl.GetFiles().length > 0 && !ctrl.Uploading) {
					ctrl.Upload();
					UploadDialog.Show();
				}
			}

			(new Image()).src = "<%= Utils.GalleryRoot %>/images/componentart/upload/vertical.png";
			(new Image()).src = "<%= Utils.GalleryRoot %>/images/spinner.gif";

			function ValidateExternalHtml() {
				// Enable the external HTML validator
				ValidatorEnable($get("<%= rfv1.ClientID %>"), true);
			}

			function tsAddObjects_onTabSelect(sender, eventArgs) {
				// Disable the external HTML validator if the local media objects tab is selected
				if (eventArgs == null) return;
				if (eventArgs.get_tab() == null) return;

				var tabId = eventArgs.get_tab().get_id();

				if (tabId == "tabLocalMedia") {
					ValidatorEnable($get("<%= rfv1.ClientID %>"), false);

					if (typeof (Upload1) !== 'undefined') {
						if (Upload1.FileCount == 0) Upload1.AddFile();
					}
				}
			}

			function AddExternalMedia_Validate() {
				ValidateExternalHtml();
				if (Page_IsValid) {
					ClearFiles();
					return true;
				}
				else
					return false;
			}

			function ClearFiles() {
				if (typeof (Upload1) !== 'undefined') {
					Upload1.clearFiles();
				}
			}

		</script>
	</div>
	<tis:wwErrorDisplay ID="wwMessage" runat="server" CellPadding="2" UseFixedHeightWhenHiding="False"
		Center="False" Width="500px">
	</tis:wwErrorDisplay>
	<div class="gsp_addleftpadding1">
		<ComponentArt:TabStrip ID="tsAddObjects" MultiPageId="mpAddObjects" TopGroupCssClass="ts0TopGroup"
			DefaultSelectedItemLookId="SelectedTab" DefaultItemLookId="DefaultTab" DefaultChildSelectedItemLookId="SelectedTab"
			TopGroupShowSeparators="true" DefaultGroupSeparatorWidth="22" DefaultGroupSeparatorHeight="22"
			DefaultGroupFirstSeparatorWidth="15" DefaultGroupFirstSeparatorHeight="22" DefaultGroupLastSeparatorWidth="22"
			DefaultGroupLastSeparatorHeight="22" Width="100%" runat="server">
			<ClientEvents>
				<TabSelect EventHandler="tsAddObjects_onTabSelect" />
			</ClientEvents>
			<ItemLooks>
				<ComponentArt:ItemLook LookId="DefaultTab" CssClass="ts0DefaultTab" HoverCssClass="ts0DefaultTabHover"
					LabelPaddingLeft="10" LabelPaddingRight="17" LabelPaddingTop="2" LabelPaddingBottom="6" />
				<ComponentArt:ItemLook LookId="SelectedTab" CssClass="ts0SelectedTab" LabelPaddingLeft="10"
					LabelPaddingRight="17" LabelPaddingTop="2" LabelPaddingBottom="6" />
				<ComponentArt:ItemLook LookId="DisabledTab" CssClass="ts0DefaultTab" LabelPaddingLeft="10"
					LabelPaddingRight="17" LabelPaddingTop="2" LabelPaddingBottom="6" />
			</ItemLooks>
			<Tabs>
				<ComponentArt:TabStripTab ID="tabLocalMedia" runat="server" Visible="false" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_Local_Media_Tab_Title %>" />
				<ComponentArt:TabStripTab ID="tabExternal" runat="server" Visible="false" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Title %>" />
			</Tabs>
		</ComponentArt:TabStrip>
		<ComponentArt:MultiPage ID="mpAddObjects" CssClass="mp2MultiPage" runat="server">
			<ComponentArt:PageView ID="pvAddLocal" CssClass="mp2MultiPageContent" runat="server">
				<p class="gsp_h3">
					<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_Local_Media_Tab_Hdr %>" /></p>
				<p class="gsp_addtopmargin5">
					<asp:Label ID="lblLocalMediaOverview" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_Local_Media_Tab_Dtl %>" /></p>
				<div class="gsp_addleftpadding1 gsp_addtopmargin5">
					<asp:PlaceHolder ID="phUpload" runat="server" />
					<div id="pnlOptionsHdr">
						<div style="padding: 10px 5px 5px 0; cursor: pointer; vertical-align: middle;" title='<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Error_Options_Tooltip %>" />'>
							<asp:Image ID="imgExpCol" ImageUrl="~/gs/images/expand.jpg" runat="server" AlternateText="" />
							<span class="gsp_bold">
								<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Error_Options_Hdr %>" /></span>
							<asp:Label ID="lblExpCol" runat="server" CssClass="gsp_fs" />
						</div>
					</div>
					<div id="pnlOptions">
						<p class="gsp_fs">
							<asp:CheckBox ID="chkDiscardOriginalImage" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_Discard_Original_Image_Option_Text %>" /></p>
						<p class="gsp_fs">
							<asp:CheckBox ID="chkDoNotExtractZipFile" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_Do_Not_Extract_Zip_File_Option_Text %>" /></p>
					</div>
				</div>
				<div class="gsp_rightBottom" style="margin-top: 1em;">
					<asp:Button ID="btnUploadMediaObjects" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_Upload_Upload_Button_Text %>"
						OnClick="btnUploadMediaObjects_Click" ToolTip="<%$ Resources:GalleryServerPro, Task_Add_Objects_Upload_Upload_Button_Tooltip %>"
						EnableViewState="false" />&nbsp;
					<asp:Button ID="btnCancelLocal" runat="server" OnClientClick="ClearFiles()" OnClick="btnCancel_Click"
						CausesValidation="false" Text="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Text %>"
						ToolTip="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Tooltip %>" /></div>
				<ComponentArt:Dialog ID="UploadDialog" runat="server" Visible="false" AllowDrag="true"
					AllowResize="false" Modal="true" Alignment="MiddleCentre" Width="658" Height="347"
					CssClass="gsp_ns" ContentCssClass="dlg-up" ContentClientTemplateId="UploadContent">
				</ComponentArt:Dialog>
			</ComponentArt:PageView>
			<ComponentArt:PageView ID="pvAddExternal" CssClass="mp2MultiPageContent" runat="server"
				Height="300">
				<p class="gsp_h3">
					<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Hdr %>" />
				</p>
				<p>
					<asp:Label ID="lblExternalOverview" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Dtl %>" /></p>
				<table class="gsp_standardTable gsp_addtopmargin5" style="width: 100%;">
					<tr>
						<td style="width: 125px;">
							<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Type_Label %>" />
						</td>
						<td>
							<asp:DropDownList ID="ddlMediaTypes" runat="server" Width="200">
								<asp:ListItem Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Type_Audio %>"
									Value="Audio" />
								<asp:ListItem Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Type_Image %>"
									Value="Image" />
								<asp:ListItem Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Type_Video %>"
									Value="Video" Selected="True" />
								<asp:ListItem Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Type_Other %>"
									Value="Other" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td style="width: 125px;">
							<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Title_Label %>" />
						</td>
						<td>
							<asp:TextBox ID="txtTitle" runat="server" Style="width: 92%;" />
						</td>
					</tr>
					<tr>
						<td class="gsp_aligntop">
							<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Tab_Html_Label %>" />
						</td>
						<td>
							<asp:RequiredFieldValidator ID="rfv1" runat="server" Enabled="false" ControlToValidate="txtExternalHtmlSource"
								Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Embed_Code_Missing_Text %>"
								ForeColor="" CssClass="gsp_msgwarning" />
							<asp:TextBox ID="txtExternalHtmlSource" runat="server" Rows="5" TextMode="MultiLine"
								Style="width: 92%;" />
						</td>
					</tr>
				</table>
				<div class="gsp_rightBottom">
					<asp:Button ID="btnAddExternalHtmlSource" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Add_Button_Text %>"
						ToolTip="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Add_Button_Tooltip %>"
						OnClientClick="return AddExternalMedia_Validate();" OnClick="btnAddExternalHtmlSource_Click" />&nbsp;<asp:Button
							ID="btnCancelExternal" runat="server" OnClientClick="ClearFiles()" OnClick="btnCancel_Click"
							CausesValidation="false" Text="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Text %>"
							ToolTip="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Tooltip %>" /></div>
				<tis:PopupInfo ID="PopupInfo1" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
					DefaultDialogBodyCss="dg5ContentBodyCss">
					<PopupInfoItems>
						<tis:PopupInfoItem ID="poi1" runat="server" ControlId="chkDiscardOriginalImage" DialogTitle="<%$ Resources:GalleryServerPro, Task_Add_Objects_Discard_Original_Image_Hdr %>"
							DialogBody="<%$ Resources:GalleryServerPro, Task_Add_Objects_Discard_Original_Image_Bdy %>" />
						<tis:PopupInfoItem ID="poi2" runat="server" ControlId="chkDoNotExtractZipFile" DialogTitle="<%$ Resources:GalleryServerPro, Task_Add_Objects_ZipOption_Hdr %>"
							DialogBody="<%$ Resources:GalleryServerPro, Task_Add_Objects_ZipOption_Bdy %>" />
						<tis:PopupInfoItem ID="poi3" runat="server" ControlId="lblExternalOverview" DialogTitle="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Object_Overview_Hdr %>"
							DialogBody="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Object_Overview_Bdy %>" />
						<tis:PopupInfoItem ID="poi4" runat="server" ControlId="ddlMediaTypes" DialogTitle="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Object_Type_Hdr %>"
							DialogBody="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Object_Type_Bdy %>" />
						<tis:PopupInfoItem ID="poi5" runat="server" ControlId="txtTitle" DialogTitle="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Object_Title_Hdr %>"
							DialogBody="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Object_Title_Bdy %>" />
						<tis:PopupInfoItem ID="poi6" runat="server" ControlId="txtExternalHtmlSource" DialogTitle="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Object_Html_Hdr %>"
							DialogBody="<%$ Resources:GalleryServerPro, Task_Add_Objects_External_Object_Html_Bdy %>" />
					</PopupInfoItems>
				</tis:PopupInfo>
				<uc1:popup ID="ucPopupContainer" runat="server" />
			</ComponentArt:PageView>
		</ComponentArt:MultiPage>
	</div>
	<asp:PlaceHolder ID="phTaskFooter" runat="server" />
</div>
