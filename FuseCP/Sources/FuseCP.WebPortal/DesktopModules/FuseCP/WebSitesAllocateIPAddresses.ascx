<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesAllocateIPAddresses.ascx.cs" Inherits="FuseCP.Portal.WebSitesAllocateIPAddresses" %>
<%@ Register Src="UserControls/AllocatePackageIPAddresses.ascx" TagName="AllocatePackageIPAddresses" TagPrefix="fcp" %>

<div class="panel-body form-horizontal">

    <fcp:AllocatePackageIPAddresses id="allocateAddresses" runat="server"
            Pool="WebSites"
            ResourceGroup="Web"
            ListAddressesControl="" />
            
</div>
