<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeStorageUsage.ascx.cs" Inherits="FuseCP.Portal.ExchangeServer.ExchangeStorageUsage" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="fcp" %>

<fcp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeStorage48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Storage Usage"></asp:Localize>
				</h3>
                        </div>
				<div class="panel-body form-horizontal">
				    <fcp:SimpleMessageBox id="messageBox" runat="server" />
				    
				    <div style="margin-left: 30px;margin-top: 30px;">
				       
			            <table cellpadding="2">					        
					        <tr>
					            <td class="FormLabel150" style="white-space:nowrap;"><asp:Localize ID="locUsedSize" runat="server" meta:resourcekey="locUsedSize" Text="Allocated Disk Space:"></asp:Localize></td>
					            <td style="white-space:nowrap;">
						            <asp:LinkButton runat="server" CssClass="NormalBold" Text="100"  meta:resourcekey="btnUsedSize"  ID="btnUsedSize" onclick="btnUsedSize_Click"  />						            
					            </td>
					        </tr>
				        </table>			    						
				        <br />
				        <br />
				        <CPCC:StyleButton id="btnRecalculate" CausesValidation="false" CssClass="btn btn-success" runat="server" onclick="btnRecalculate_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRecalculateText"/> </CPCC:StyleButton>&nbsp;						
				    </div>
				</div>
