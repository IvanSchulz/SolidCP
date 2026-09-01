<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LicensingQuotas.ascx.cs" Inherits="SolidCP.Portal.LicensingQuotas" %>
<%@ Import Namespace="SolidCP.Portal" %>

<div class="panel-body row">
	<div class="col-md-6">
        <div class="form-group">
		<asp:Label ID="lblGroup" runat="server" meta:resourcekey="lblGroup" Text="Resource group:" CssClass="col-sm-2"></asp:Label>
            <div class="col-sm-10">
				<asp:DropDownList ID="ddlGroups" runat="server" CssClass="form-control" AutoPostBack="true">
				</asp:DropDownList>
             </div>
        </div>
	</div>
</div>

<asp:GridView id="gvLicensingQuotas" runat="server" AutoGenerateColumns="False"
	DataSourceID="odsLicensingQuotas" AllowPaging="False" AllowSorting="True" EmptyDataText="gvLicensingQuotas"
	CssSelectorClass="NormalGridView">
	<Columns>
		<asp:TemplateField SortExpression="QuotaDescription" HeaderText="gvQuotaName">
			<ItemTemplate>
				<b><asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("QuotaID", Eval("QuotaID").ToString(), "edit_licensing_quota") %>'>
					<%# PortalAntiXSS.EncodeOld((string)Eval("QuotaDescription")) %>
				</asp:hyperlink></b>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="LicenseDescription" SortExpression="LicenseDescription" HeaderText="gvLicenseDescription"></asp:BoundField>
		<asp:TemplateField SortExpression="IsHidden" HeaderText="gvIsActive">
			<ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
				<%# PanelFormatter.GetYesNo(!(bool)Eval("IsHidden"))%>
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsLicensingQuotas" runat="server"
	SelectMethod="GetLicensingQuotas"
	TypeName="SolidCP.Portal.LicensingQuotas"
	SelectCountMethod="GetLicensingQuotasCount"
	SortParameterName="sortColumn">
	 <SelectParameters>
	    <asp:ControlParameter Name="groupId" ControlID="ddlGroups" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:ObjectDataSource>