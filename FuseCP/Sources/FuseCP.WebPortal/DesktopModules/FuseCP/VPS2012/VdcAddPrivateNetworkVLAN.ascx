<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAddPrivateNetworkVLAN.ascx.cs" Inherits="FuseCP.Portal.VPS2012.VdcAddPrivateNetworkVLAN" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="fcp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/AllocatePackageVLANs.ascx" TagName="AllocatePackageVLANs" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="fcp" %>

<fcp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">

                <fcp:AllocatePackageVLANs id="allocateVLANs" runat="server"
                        ResourceGroup="VPS2012"
                        ListAddressesControl="vdc_private_network" />
				    
			    </div>
		    </div>
	    </div>
