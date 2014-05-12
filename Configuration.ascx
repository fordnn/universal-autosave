<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="Configuration.ascx.cs"
    Inherits="forDNN.Modules.UniversalAutosave.Configuration" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<div class="dnnLeft">
    <asp:HyperLink ID="lnkAddConfiguration" runat="server" CssClass="dnnPrimaryAction"
        resourcekey="AddConfiguration"></asp:HyperLink>
</div>
<div class="dnnRight">
    <dnn:label ID="lblConfigurationPageSize" runat="server" CssClass="SubHead" resourcekey="ItemsPerPage">
    </dnn:label>
    <div class="inlineBlock">
        <asp:DropDownList ID="ddlConfigurationPageSize" runat="server" AutoPostBack="True"
            OnSelectedIndexChanged="ddlConfigurationPageSize_SelectedIndexChanged">
            <asp:ListItem Value='1'>1</asp:ListItem>
            <asp:ListItem Value='10'>10</asp:ListItem>
            <asp:ListItem Value='25' Selected='True'>25</asp:ListItem>
            <asp:ListItem Value='50'>50</asp:ListItem>
            <asp:ListItem Value='100'>100</asp:ListItem>
            <asp:ListItem Value='250'>250</asp:ListItem>
            <asp:ListItem Value='1000'>1000</asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
<div class="dnnClear">
</div>
<div>
    <asp:DataGrid ID="grdConfig" AutoGenerateColumns="false" Width="100%" CellPadding="2"
        GridLines="None" CssClass="dnnGrid dnnSecurityRolesGrid" runat="server" OnItemDataBound="grdConfig_ItemDataBound"
        OnDeleteCommand="grdConfig_DeleteCommand" OnSortCommand="grdConfig_SortCommand"
        AllowPaging="true">
        <HeaderStyle CssClass="dnnGridHeader" VerticalAlign="Top" />
        <ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
        <AlternatingItemStyle CssClass="dnnGridAltItem" />
        <EditItemStyle CssClass="dnnFormInput" />
        <SelectedItemStyle CssClass="dnnFormError" />
        <FooterStyle CssClass="dnnGridFooter" />
        <PagerStyle Visible="false" CssClass="dnnGridPager" />
        <Columns>
            <dnn:imagecommandcolumn CommandName="Edit" IconKey="Edit" EditMode="URL" KeyField="ConfigurationID"
                HeaderStyle-Width="5%" />
            <dnn:imagecommandcolumn CommandName="Delete" IconKey="ActionDelete" KeyField="ConfigurationID"
                HeaderStyle-Width="5%" />
            <asp:TemplateColumn HeaderText="Title">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="litTitle"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Enabled" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                HeaderStyle-Width="5%">
                <ItemTemplate>
                    <asp:CheckBox ID="cbAutosaveEnabled" runat="server" Checked='<%# Bind("AutosaveEnabled") %>'
                        Enabled="false" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <dnn:pagingcontrol id="ctlPagingControl" runat="server">
    </dnn:pagingcontrol>
</div>
