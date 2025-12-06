<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBPhoneNumbers.ascx.cs" Inherits="FuseCP.Portal.SfB.SfBPhoneNumbers" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="fcp" %>
<%@ Register Src="UserControls/SfBUserPlanSelector.ascx" TagName="SfBUserPlanSelector" TagPrefix="fcp" %>

<%@ Register Src="../UserControls/PackagePhoneNumbers.ascx" TagName="PackagePhoneNumbers" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="fcp" %>

<fcp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

                <div class="panel-heading">
                    <asp:Image ID="Image1" SkinID="SfBLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </div>
                <div class="panel-body form-horizontal">
                    <fcp:PackagePhoneNumbers id="phoneNumbers" runat="server"
                            Pool="PhoneNumbers"
                            EditItemControl=""
                            SpaceHomeControl=""
                            AllocateAddressesControl="sfb_allocate_phonenumbers"
                            ManageAllowed="true" />
    
                    <br />
                    <fcp:CollapsiblePanel id="secQuotas" runat="server"
                        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
                    </fcp:CollapsiblePanel>
                    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
    
                    <table cellspacing="6">
                        <tr>
                            <td><asp:Localize ID="locIPQuota" runat="server" meta:resourcekey="locIPQuota" Text="Number of Phone Numbes:"></asp:Localize></td>
                            <td><fcp:Quota ID="phoneQuota" runat="server" QuotaName="SfB.PhoneNumbers" /></td>
                        </tr>
                    </table>
    
    
                    </asp:Panel>
                </div>


