<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBPhoneNumbers.ascx.cs" Inherits="FuseCP.Portal.SfBPhoneNumbers" %>
<%@ Register Src="UserControls/PackagePhoneNumbers.ascx" TagName="PackagePhoneNumbers" TagPrefix="fcp" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="fcp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="fcp" %>

<div class="panel-body form-horizontal">
    <fcp:PackagePhoneNumbers id="webAddresses" runat="server"
            Pool="PhoneNumbers"
            EditItemControl=""
            SpaceHomeControl=""
            ManageAllowed="true" />
    
    <br />
    <fcp:CollapsiblePanel id="secQuotas" runat="server"
        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
    </fcp:CollapsiblePanel>
    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
    
    <table cellspacing="6">
        <tr>
            <td><asp:Localize ID="locIPQuota" runat="server" meta:resourcekey="locIPQuota" Text="Number of Phone Numbes:"></asp:Localize></td>
            <td><fcp:Quota ID="addressesQuota" runat="server" QuotaName="SfB.PhoneNumbers" /></td>
        </tr>
    </table>
    
    
    </asp:Panel>
</div>

