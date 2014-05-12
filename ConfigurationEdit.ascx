<%@ Control Language="C#" Inherits="forDNN.Modules.UniversalAutosave.UniversalAutosave"
	AutoEventWireup="true" CodeBehind="ConfigurationEdit.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="forDNN" Namespace="forDNN.Modules.UniversalAutosave.Controls"
	Assembly="forDNN.Modules.UniversalAutosave" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<div class="dnnForm" id="tabs-demo">
	<ul class="dnnAdminTabNav">
		<li><a href="#basic-settings">
			<asp:Label runat="server" ID="lblBasicSettings" resourcekey="BasicSettings"></asp:Label></a></li>
		<li><a href="#pages">
			<asp:Label runat="server" ID="litPages" resourcekey="Pages"></asp:Label></a></li>
		<li><a href="#permissions">
			<asp:Label runat="server" ID="litPermissions" resourcekey="Permissions"></asp:Label></a></li>
		<li><a href="#controls">
			<asp:Label runat="server" ID="litControls" resourcekey="Controls"></asp:Label></a>
		</li>
		<li><a href="#events">
			<asp:Label runat="server" ID="lblEvents" resourcekey="Events"></asp:Label></a></li>
		<li><a href="#saved-values">
			<asp:Label runat="server" ID="lblSavedValues" resourcekey="SavedValues"></asp:Label></a></li>
	</ul>
	<div class="dnnClear">
		&nbsp;
	</div>
	<div id="basic-settings" class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblTitle" CssClass="SubHead" resourcekey="Title">
			</dnn:Label>
			<asp:TextBox runat="server" ID="tbTitle"></asp:TextBox>
			<asp:RequiredFieldValidator runat="server" ID="reqTitle" CssClass="dnnFormMessage dnnFormError"
				Display="Dynamic" ControlToValidate="tbTitle" resourcekey="RequiredTitle"></asp:RequiredFieldValidator>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblDescription" CssClass="SubHead" resourcekey="Description">
			</dnn:Label>
			<asp:TextBox runat="server" ID="tbDescription" TextMode="MultiLine"></asp:TextBox>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblEnableAutosave" CssClass="SubHead" resourcekey="EnableAutosave">
			</dnn:Label>
			<asp:CheckBox runat="server" ID="cbEnableAutosave" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblAutosaveIcon" CssClass="SubHead" resourcekey="AutosaveIcon">
			</dnn:Label>
			<asp:CheckBox runat="server" ID="cbAutosaveIcon" Checked="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblBlurAutosave" CssClass="SubHead" resourcekey="BlurAutosave">
			</dnn:Label>
			<asp:CheckBox runat="server" ID="cbBlurAutosave" Checked="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblAutosavePeriod" CssClass="SubHead" resourcekey="AutosavePeriod">
			</dnn:Label>
			<asp:TextBox runat="server" ID="tbAutosavePeriod" Text="0" MaxLength="5"></asp:TextBox>
			<asp:RangeValidator ID="vldAutosavePeriod" runat="server" ControlToValidate="tbAutosavePeriod"
				CssClass="dnnFormMessage dnnFormError" Display="Dynamic" MaximumValue="86400"
				MinimumValue="0" Type="Integer" resourcekey="InvalidAutosavePeriod"></asp:RangeValidator>
			<asp:RequiredFieldValidator runat="server" ID="reqAutosavePeriod" CssClass="dnnFormMessage dnnFormError"
				Display="Dynamic" ControlToValidate="tbAutosavePeriod" resourcekey="RequiredAutosavePeriod"></asp:RequiredFieldValidator>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblHistoryLength" CssClass="SubHead" resourcekey="HistoryLength">
			</dnn:Label>
			<asp:TextBox runat="server" ID="tbHistoryLength" Text="0" MaxLength="4"></asp:TextBox>
			<asp:RangeValidator ID="vldHistoryLength" runat="server" ControlToValidate="tbHistoryLength"
				CssClass="dnnFormMessage dnnFormError" Display="Dynamic" MaximumValue="9999"
				MinimumValue="0" Type="Integer" resourcekey="InvalidHistoryLength"></asp:RangeValidator>
			<asp:RequiredFieldValidator runat="server" ID="reqHistoryLength" CssClass="dnnFormMessage dnnFormError"
				Display="Dynamic" ControlToValidate="tbHistoryLength" resourcekey="RequiredHistoryLength"></asp:RequiredFieldValidator>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblHistoryExpiry" CssClass="SubHead" resourcekey="HistoryExpiry">
			</dnn:Label>
			<asp:TextBox runat="server" ID="tbHistoryExpiry" Text="0" MaxLength="3"></asp:TextBox>
			<asp:RangeValidator ID="vldHistoryExpiry" runat="server" ControlToValidate="tbHistoryExpiry"
				CssClass="dnnFormMessage dnnFormError" Display="Dynamic" MaximumValue="365" MinimumValue="0"
				Type="Integer" resourcekey="InvalidHistoryExpiry"></asp:RangeValidator>
			<asp:RequiredFieldValidator runat="server" ID="reqHistoryExpiry" CssClass="dnnFormMessage dnnFormError"
				Display="Dynamic" ControlToValidate="tbHistoryExpiry" resourcekey="RequiredHistoryExpiry"></asp:RequiredFieldValidator>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblAutosaveLocation" CssClass="SubHead" resourcekey="AutosaveLocation">
			</dnn:Label>
			<asp:DropDownList runat="server" ID="ddlAutosaveLocation">
				<asp:ListItem Value="0" resourcekey="AutosaveLocation-0"></asp:ListItem>
				<asp:ListItem Value="1" resourcekey="AutosaveLocation-1" Selected></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblUrlIndependent" CssClass="SubHead" resourcekey="UrlIndependent">
			</dnn:Label>
			<asp:CheckBox runat="server" ID="cbUrlIndependent" />
		</div>
	</div>
	<div id="pages" class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label ID="lblQuickSearchPages" runat="server" CssClass="SubHead" resourcekey="QuickSearch">
			</dnn:Label>
			<div class="inlineBlock">
				<input type="text" id="tbQuickSearchPages" onkeyup="javascript:uaSearchTree('#tbQuickSearchPages', '<%=lstPages.ClientID%>');" />
			</div>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblPagesTitle" CssClass="SubHead" resourcekey="PagesTitle">
			</dnn:Label>
			<div class="inlineBlock">
				<telerik:RadTreeView ID="lstPages" runat="server" EnableDragAndDrop="False" CssClass="normalCheckBox">
				</telerik:RadTreeView>
			</div>
		</div>
	</div>
	<div id="permissions" class="dnnClear">
		<fordnn:uapermissionsgrid id="grdPermissions" runat="server" />
	</div>
	<div id="controls" class="dnnClear">
		<h2 id="PanelAddControlsWizard" class="dnnFormSectionHead">
			<asp:HyperLink runat="server" ID="lnkAddControlsWizard" resourcekey="AddControlsWizard"></asp:HyperLink>
		</h2>
		<fieldset>
			<asp:LinkButton runat="server" ID="lnkStartWizard" CssClass="dnnPrimaryAction" resourcekey="StartWizard"
				Target="_blank" OnClick="btnStartWizard_Click"></asp:LinkButton>
		</fieldset>
		<h2 id="PanelControlsGrid" class="dnnFormSectionHead">
			<asp:HyperLink runat="server" ID="lnkControlsGrid" resourcekey="ControlsGrid"></asp:HyperLink>
		</h2>
		<fieldset>
			<asp:DataGrid ID="grdControls" AutoGenerateColumns="false" Width="100%" CellPadding="2"
				GridLines="None" CssClass="dnnGrid dnnSecurityRolesGrid" runat="server" OnItemDataBound="grdControls_ItemDataBound"
				OnDeleteCommand="grdControls_DeleteCommand">
				<HeaderStyle CssClass="dnnGridHeader" VerticalAlign="Top" />
				<ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
				<AlternatingItemStyle CssClass="dnnGridAltItem" />
				<EditItemStyle CssClass="dnnFormInput" />
				<SelectedItemStyle CssClass="dnnFormError" />
				<FooterStyle CssClass="dnnGridFooter" />
				<PagerStyle Visible="false" CssClass="dnnGridPager" />
				<Columns>
					<dnn:imagecommandcolumn CommandName="Edit" IconKey="Edit" EditMode="URL" KeyField="ControlID"
						HeaderStyle-Width="5%" />
					<dnn:imagecommandcolumn CommandName="Delete" IconKey="ActionDelete" KeyField="ControlID"
						HeaderStyle-Width="5%" />
					<asp:TemplateColumn HeaderText="Selector">
						<HeaderTemplate>
							<asp:Label ID="lblQuickSearchControls" runat="server" resourcekey="Selector">
							</asp:Label>
							<input type="text" id="tbQuickSearchControls" onkeyup="javascript:uaSearchTable('#tbQuickSearchControls', '#<%=grdControls.ClientID%>');" />
						</HeaderTemplate>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblSelectorValue" Text='<%# Bind("Selector") %>'></asp:Label>
							<div class="editItem dnnFormItem" style="display: none">
								<asp:TextBox runat="server" ID="tbSelectorValue" Text='<%# Bind("Selector") %>'></asp:TextBox>&nbsp;
								<asp:ImageButton runat="server" ID="btnSelectorSave" />&nbsp;
								<asp:ImageButton runat="server" ID="btnSelectorCancel" />
							</div>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Enabled" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
						HeaderStyle-Width="5%">
						<ItemTemplate>
							<asp:CheckBox ID="cbEnabled" runat="server" Checked='<%# Bind("Enabled") %>' />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="RestoreOnLoad" HeaderStyle-HorizontalAlign="Center"
						ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
						<ItemTemplate>
							<asp:CheckBox ID="cbRestoreOnLoad" runat="server" Checked='<%# Bind("RestoreOnLoad") %>' />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="RestoreIfEmpty" HeaderStyle-HorizontalAlign="Center"
						ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
						<ItemTemplate>
							<asp:CheckBox ID="cbRestoreIfEmpty" runat="server" Checked='<%# Bind("RestoreIfEmpty") %>' />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="ShowCannedOnly" HeaderStyle-HorizontalAlign="Center"
						ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
						<ItemTemplate>
							<asp:CheckBox ID="cbShowCannedOnly" runat="server" Checked='<%# Bind("ShowCannedOnly") %>' />
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</fieldset>
		<h2 id="PanelQuickAddSelector" class="dnnFormSectionHead">
			<asp:HyperLink runat="server" ID="lnkPanelQuickAddSelector" resourcekey="QuickAddSelector"></asp:HyperLink>
		</h2>
		<fieldset>
			<div class="dnnFormItem">
				<dnn:Label runat="server" ID="lblQuickAddSelector" CssClass="SubHead" resourcekey="QuickAddSelector">
				</dnn:Label>
				<asp:TextBox runat="server" ID="tbQuickAddSelector"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="reqQuickAddSelector" CssClass="dnnFormMessage dnnFormError"
					Display="Dynamic" ControlToValidate="tbQuickAddSelector" resourcekey="RequiredQuickAddSelector"
					ValidationGroup="QuickAddSelector"></asp:RequiredFieldValidator>&nbsp;
				<asp:LinkButton ID="btnQuickAddSelector" CssClass="dnnPrimaryAction" runat="server"
					BorderStyle="None" resourcekey="QuickAddSelectorButton" OnClick="btnQuickAddSelector_Click"
					ValidationGroup="QuickAddSelector"></asp:LinkButton>
			</div>
		</fieldset>
	</div>
	<div id="events" class="dnnClear">
		<h2 id="H1" class="dnnFormSectionHead">
			<asp:HyperLink runat="server" ID="lnkEventsWizard" resourcekey="EventsWizard"></asp:HyperLink>
		</h2>
		<fieldset>
			<asp:LinkButton runat="server" ID="lnkStartEventWizard" CssClass="dnnPrimaryAction"
				resourcekey="StartEventWizard" Target="_blank" OnClick="btnStartWizard_Click"></asp:LinkButton>
		</fieldset>
		<h2 id="H2" class="dnnFormSectionHead">
			<asp:HyperLink runat="server" ID="lnkEventsGrid" resourcekey="EventsGrid"></asp:HyperLink>
		</h2>
		<fieldset>
			<asp:DataGrid ID="grdEvents" AutoGenerateColumns="false" Width="100%" CellPadding="2"
				GridLines="None" CssClass="dnnGrid dnnSecurityRolesGrid" runat="server" OnItemDataBound="grdEvents_ItemDataBound"
				OnDeleteCommand="grdEvents_DeleteCommand">
				<HeaderStyle CssClass="dnnGridHeader" VerticalAlign="Top" />
				<ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
				<AlternatingItemStyle CssClass="dnnGridAltItem" />
				<EditItemStyle CssClass="dnnFormInput" />
				<SelectedItemStyle CssClass="dnnFormError" />
				<FooterStyle CssClass="dnnGridFooter" />
				<PagerStyle Visible="false" CssClass="dnnGridPager" />
				<Columns>
					<dnn:imagecommandcolumn CommandName="Edit" IconKey="Edit" EditMode="URL" KeyField="EventID"
						HeaderStyle-Width="5%" />
					<dnn:imagecommandcolumn CommandName="Delete" IconKey="ActionDelete" KeyField="EventID"
						HeaderStyle-Width="5%" />
					<asp:TemplateColumn HeaderText="Selector">
						<HeaderTemplate>
							<asp:Label ID="lblQuickSearchEvents" runat="server" resourcekey="Events">
							</asp:Label>
							<input type="text" id="tbQuickSearchEvents" onkeyup="javascript:uaSearchTable('#tbQuickSearchEvents', '#<%=grdEvents.ClientID%>');" />
						</HeaderTemplate>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblSelectorValue" Text='<%# Bind("Selector") %>'></asp:Label>
							<div class="editItem dnnFormItem" style="display: none">
								<asp:TextBox runat="server" ID="tbSelectorValue" Text='<%# Bind("Selector") %>'></asp:TextBox>&nbsp;
								<asp:ImageButton runat="server" ID="btnSelectorSave" />&nbsp;
								<asp:ImageButton runat="server" ID="btnSelectorCancel" />
							</div>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn HeaderText="EventName" DataField="EventName"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Enabled" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
						HeaderStyle-Width="5%">
						<ItemTemplate>
							<asp:CheckBox ID="cbEnabled" runat="server" Checked='<%# Bind("Enabled") %>' />
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</fieldset>
		<h2 id="H3" class="dnnFormSectionHead">
			<asp:HyperLink runat="server" ID="lnkQuickAddEventSelector" resourcekey="QuickAddEventSelector"></asp:HyperLink>
		</h2>
		<fieldset>
			<div class="dnnFormItem">
				<dnn:Label runat="server" ID="lblQuickAddEventSelector" CssClass="SubHead" resourcekey="QuickAddEventSelector">
				</dnn:Label>
				<asp:TextBox runat="server" ID="tbQuickAddEventSelector"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="reqQuickAddEventSelector" CssClass="dnnFormMessage dnnFormError"
					Display="Dynamic" ControlToValidate="tbQuickAddEventSelector" resourcekey="RequiredQuickAddEventSelector"
					ValidationGroup="QuickAddEventSelector"></asp:RequiredFieldValidator>&nbsp;
				<asp:LinkButton ID="btnQuickAddEventSelector" CssClass="dnnPrimaryAction" runat="server"
					BorderStyle="None" resourcekey="QuickAddEventSelectorButton" OnClick="btnQuickAddEventSelector_Click"
					ValidationGroup="QuickAddEventSelector"></asp:LinkButton>
			</div>
		</fieldset>
	</div>
	<div id="saved-values" class="dnnClear">
		<asp:Label runat="server" ID="lblFilterBy" CssClass="Head" resourcekey="FilterBy">
		</asp:Label>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblFilterByControl" CssClass="SubHead" resourcekey="FilterByControl">
			</dnn:Label>
			<asp:DropDownList runat="server" ID="ddlFilterByControl" CssClass="filterBox">
			</asp:DropDownList>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblFilterByUser" CssClass="SubHead" resourcekey="FilterByUser">
			</dnn:Label>
			<asp:DropDownList runat="server" ID="ddlFilterByUser" CssClass="filterBox">
			</asp:DropDownList>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblPeriodFrom" CssClass="SubHead" resourcekey="PeriodFrom">
			</dnn:Label>&nbsp;
			<asp:TextBox runat="server" ID="tbPeriodFrom" CssClass="calendarTextBox"></asp:TextBox>&nbsp;
			<asp:HyperLink ImageUrl="~/images/calendar.png" runat="server" ID="lnkCalendarFrom"
				resourcekey="Calendar"></asp:HyperLink>
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblPeriodTo" CssClass="SubHead" resourcekey="PeriodTo">
			</dnn:Label>&nbsp;
			<asp:TextBox runat="server" ID="tbPeriodTo" CssClass="calendarTextBox"></asp:TextBox>&nbsp;
			<asp:HyperLink ImageUrl="~/images/calendar.png" runat="server" ID="lnkCalendarTo"
				resourcekey="Calendar"></asp:HyperLink>&nbsp;
			<asp:LinkButton runat="server" ID="btnRefreshValues" class="dnnPrimaryAction" resourcekey="RefreshValues"
				OnClick="btnRefreshValues_Click"></asp:LinkButton>
		</div>
		<fieldset>
			<asp:Label runat="server" ID="lblNoValuesFound" resourcekey="NoValuesFound" CssClass="NormalRed"
				Visible="false"></asp:Label>
			<asp:DataGrid ID="grdValues" AutoGenerateColumns="false" Width="100%" CellPadding="2"
				GridLines="None" CssClass="dnnGrid dnnSecurityRolesGrid" runat="server" OnItemDataBound="grdValues_ItemDataBound"
				OnDeleteCommand="grdValues_DeleteCommand">
				<HeaderStyle CssClass="dnnGridHeader" VerticalAlign="Top" />
				<ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
				<AlternatingItemStyle CssClass="dnnGridAltItem" />
				<EditItemStyle CssClass="dnnFormInput" />
				<SelectedItemStyle CssClass="dnnFormError" />
				<FooterStyle CssClass="dnnGridFooter" />
				<PagerStyle Visible="false" CssClass="dnnGridPager" />
				<Columns>
					<dnn:imagecommandcolumn CommandName="Edit" IconKey="Edit" EditMode="URL" KeyField="ControlID"
						HeaderStyle-Width="5%" />
					<dnn:imagecommandcolumn CommandName="Delete" IconKey="ActionDelete" KeyField="ControlID"
						HeaderStyle-Width="5%" />
					<asp:TemplateColumn HeaderText="Value">
						<HeaderTemplate>
							<asp:Label ID="lblQuickSearchControls" runat="server" resourcekey="QuickSearchValue">
							</asp:Label>
							<input type="text" id="tbQuickSearchValues" onkeyup="javascript:doSearchTable('#tbQuickSearchValues', '#<%=grdValues.ClientID%>');" />
						</HeaderTemplate>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblValue" Text='<%# Bind("Value") %>'></asp:Label>
							<div class="editItem dnnFormItem" style="display: none">
								<asp:TextBox runat="server" ID="tbValue" Text='<%# Bind("Value") %>'></asp:TextBox>&nbsp;
								<asp:ImageButton runat="server" ID="btnValueSave" />&nbsp;
								<asp:ImageButton runat="server" ID="btnValueCancel" />
							</div>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Canned" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
						HeaderStyle-Width="5%">
						<ItemTemplate>
							<asp:CheckBox ID="cbCanned" runat="server" Checked='<%# Bind("Canned") %>' />
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</fieldset>
	</div>
	<div class="dnnClear">
		&nbsp;</div>
	<div class="dnnClear">
		<div>
			<asp:LinkButton ID="cmdUpdateClose" CssClass="CommandButton" runat="server" BorderStyle="None"
				resourcekey="cmdUpdateClose" OnClick="cmdUpdateClose_Click">Update</asp:LinkButton>&nbsp;
			<asp:LinkButton ID="cmdUpdate" CssClass="dnnPrimaryAction" runat="server" BorderStyle="None"
				resourcekey="cmdUpdate" OnClick="cmdUpdate_Click">Update</asp:LinkButton>&nbsp;
			<asp:LinkButton ID="cmdCancel" CssClass="CommandButton" runat="server" CausesValidation="False"
				BorderStyle="None" resourcekey="cmdCancel" OnClick="cmdCancel_Click" Style="height: 19px">Cancel</asp:LinkButton>&nbsp;
			<asp:LinkButton ID="cmdDelete" CssClass="CommandButton" runat="server" CausesValidation="False"
				BorderStyle="None" Visible="False" resourcekey="cmdDelete" OnClick="cmdDelete_Click">Delete</asp:LinkButton>
		</div>
	</div>
</div>
