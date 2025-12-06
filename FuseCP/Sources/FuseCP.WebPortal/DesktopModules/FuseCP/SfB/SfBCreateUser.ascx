<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBCreateUser.ascx.cs" Inherits="FuseCP.Portal.SfB.CreateSfBUser" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="fcp" %>
<%@ Register Src="UserControls/SfBUserPlanSelector.ascx" TagName="SfBUserPlanSelector" TagPrefix="fcp" %>

<fcp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div id="ExchangeContainer">
    <div class="Module">
        <div class="Left">
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="SfBLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </div>
                <div class="panel-body form-horizontal">
                    <fcp:SimpleMessageBox id="messageBox" runat="server" />
                    <table id="ExistingUserTable"   runat="server" width="100%"> 					    
					    <tr>
					        <td class="FormLabel150"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
					        <td>                                
                                <fcp:UserSelector ID="userSelector" runat="server" IncludeMailboxesOnly="false" IncludeMailboxes="true" ExcludeOCSUsers="true" ExcludeSfBUsers="true"/>
                            </td>
					    </tr>
					    	    					    					    
					</table>

					    <table>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize ID="locPlanName" runat="server" meta:resourcekey="locPlanName" Text="Plan Name: *"></asp:Localize>
                                </td>
                                <td>                                
                                    <fcp:SfBUserPlanSelector ID="planSelector" runat="server" />
                                </td>
					        </tr>
                        </table>

                        <asp:Panel runat="server" ID="pnEnterpriseVoice">
                        <table>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize runat="server" ID="locPhoneNumber" meta:resourcekey="locPhoneNumber" Text="Phone Number:" />
                                </td>
                                <td>
                                    <!-- <asp:TextBox runat="server" ID="tb_PhoneNumber" /> -->
                                    <asp:dropdownlist id="ddlPhoneNumber" Runat="server" CssClass="form-control"></asp:dropdownlist>
                                    <asp:RegularExpressionValidator ID="PhoneFormatValidator" runat="server"
		                            ControlToValidate="ddlPhoneNumber" Display="Dynamic" ValidationGroup="Validation1" SetFocusOnError="true"
		                            ValidationExpression="^([0-9])*$"
                                    ErrorMessage="Must contain only numbers.">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize runat="server" ID="locSfBPin" meta:resourcekey="locSfBPin" Text="SfB Pin:" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbPin" />
                                    <asp:RegularExpressionValidator ID="PinRegularExpressionValidator" runat="server"
		                            ControlToValidate="tbPin" Display="Dynamic" ValidationGroup="Validation1" SetFocusOnError="true"
		                            ValidationExpression="^([0-9])*$"
                                    ErrorMessage="Must contain only numbers.">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
					    </table>
                        </asp:Panel>
					
					<div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click" ValidationGroup="Validation1"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>					    
				    </div>			
                </div>
            </div>
        </div>
    </div>
</div>

