<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsPermissions.ascx.cs" Inherits="FuseCP.Portal.VPSForPC.VpsDetailsPermissions" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="fcp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="fcp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="fcp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="fcp" %>


	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Server48" runat="server" />
				    <fcp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Permissions" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <fcp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
			        <fcp:ServerTabs id="tabs" runat="server" SelectedTab="vps_permissions" />	
                    <fcp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <div class="FormButtonsBarClean">
                        <asp:CheckBox ID="chkOverride" runat="server" meta:resourcekey="chkOverride"
                                Text="Override space permissions" AutoPostBack="true" />
                    </div>
                    
		            <asp:GridView ID="gvVpsPermissions" runat="server" AutoGenerateColumns="False"
			            EmptyDataText="gvVpsPermissions" CssSelectorClass="NormalGridView">
			            <Columns>
				            <asp:BoundField HeaderText="columnUsername" DataField="Username" />
				            <asp:TemplateField HeaderText="columnChangeState" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkChangeState" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
				            <asp:TemplateField HeaderText="columnChangeConfig" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkChangeConfig" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
				            <asp:TemplateField HeaderText="columnManageSnapshots" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkManageSnapshots" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
				            <asp:TemplateField HeaderText="columnDeleteVps" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkDeleteVps" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
				            <asp:TemplateField HeaderText="columnReinstallVps" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkReinstallVps" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
			            </Columns>
		            </asp:GridView>
		            <br />
						<CPCC:StyleButton id="btnUpdateVpsPermissions" CssClass="btn btn-success" runat="server" CausesValidation="false" OnClick="btnUpdateVpsPermissions_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateVpsPermissionsText"/> </CPCC:StyleButton>
                  <br />
				   
			    </div>
		    </div>
            </div>
            </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="HSFormComments"></asp:Localize>
		    </div>
