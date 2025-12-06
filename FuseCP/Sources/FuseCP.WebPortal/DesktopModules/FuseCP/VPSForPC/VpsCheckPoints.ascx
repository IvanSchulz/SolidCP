<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsCheckPoints.ascx.cs" Inherits="FuseCP.Portal.VPSForPC.VpsCheckPoints" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="fcp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="fcp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="fcp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="fcp" %>

	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Monitoring48" runat="server" />
                    <fcp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Snapshots" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <fcp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <fcp:ServerTabs id="tabs" runat="server" SelectedTab="vps_checkpoints" />	
                    <fcp:SimpleMessageBox id="messageBox" runat="server" />

                    <asp:TreeView runat="server" ID="treeCheckPoints"></asp:TreeView>
                <div class="FormButtonsBar" >
                    <CPCC:StyleButton id="btnRestoreCheckPoint" CssClass="btn btn-warning" runat="server" OnClick="btnRestoreCheckPoint_Click" CausesValidation="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRestoreText"/> </CPCC:StyleButton>&nbsp;        
                    <CPCC:StyleButton id="btnCreateCheckPoint" CssClass="btn btn-success" runat="server" OnClick="btnCreateCheckPoint_Click" CausesValidation="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
                </div> 
            </div>
                    </div>
                    </div>
            </div>
	        <div class="Right">
		        <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
	        </div>
