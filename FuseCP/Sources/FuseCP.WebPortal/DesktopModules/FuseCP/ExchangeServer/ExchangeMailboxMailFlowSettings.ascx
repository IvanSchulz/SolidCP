<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxMailFlowSettings.ascx.cs" Inherits="FuseCP.Portal.ExchangeServer.ExchangeMailboxMailFlowSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="fcp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="fcp" %>
<%@ Register Src="UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="fcp" %>
<%@ Register Src="UserControls/AcceptedSenders.ascx" TagName="AcceptedSenders" TagPrefix="fcp" %>
<%@ Register Src="UserControls/RejectedSenders.ascx" TagName="RejectedSenders" TagPrefix="fcp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="fcp" %>
<%@ Register TagPrefix="fcp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="fcp" %>

<fcp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeMailbox48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                    </h3>
                </div>
				<div class="panel-body form-horizontal">
                     <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <fcp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_mailflow" />	
                    </div>
                    <div class="panel panel-default tab-content">
                    <fcp:SimpleMessageBox id="messageBox" runat="server" />
					<fcp:CollapsiblePanel id="secForwarding" runat="server"
                        TargetControlID="Forwarding" meta:resourcekey="secForwarding" Text="Forwarding Address">
                    </fcp:CollapsiblePanel>
                    <asp:Panel ID="Forwarding" runat="server" Height="0" style="overflow:hidden;">
						<asp:UpdatePanel ID="ForwardingUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
							<ContentTemplate>
							
					    <table>
							<tr>
								<td>
									<asp:CheckBox ID="chkEnabledForwarding" runat="server" meta:resourcekey="chkEnabledForwarding" Text="Enable Forwarding" AutoPostBack="true" OnCheckedChanged="chkEnabledForwarding_CheckedChanged" />
								</td>
							</tr>
						</table>
						<table id="ForwardSettingsPanel" runat="server">
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locForwardTo" runat="server" meta:resourcekey="locForwardTo" Text="Forward To:"></asp:Localize></td>
							    <td>
									<fcp:MailboxSelector id="forwardingAddress" runat="server"
											MailboxesEnabled="true"
											ContactsEnabled="true"
											DistributionListsEnabled="true" />
								</td>
						    </tr>
						    <tr>
								<td></td>
								<td>
									<asp:CheckBox ID="chkDoNotDeleteOnForward" runat="server" meta:resourcekey="chkDoNotDeleteOnForward" Text="Deliver messages to both forwarding address and mailbox" />
								</td>
						    </tr>
						</table>
						
							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>


					<fcp:CollapsiblePanel id="secSendOnBehalf" runat="server"
                        TargetControlID="SendOnBehalf" meta:resourcekey="secSendOnBehalf" Text="Send On Behalf">
                    </fcp:CollapsiblePanel>
                    <asp:Panel ID="SendOnBehalf" runat="server" Height="0" style="overflow:hidden;">
					    <table>
							<tr>
								<td>
									<asp:Localize ID="locGrantAccess" runat="server" meta:resourcekey="locGrantAccess" Text="Grant this permission to:"></asp:Localize>
								</td>
							</tr>
							<tr>
								<td>
									<fcp:AccountsList id="accessAccounts" runat="server"
											MailboxesEnabled="true" />
								</td>
							</tr>
					    </table>
                      <table id="tablesavesentitems" runat="server" style="width:100%;margin-top:10px;">
    					    <tr>
	   				            <td align="left">
                                   <asp:CheckBox ID="chkSaveSentItems" runat="server" meta:resourcekey="chkSaveSentItems" Text="Copy items sent as and on behalf of this mailbox" />
			    		        </td>
		    			    </tr>
					</table>
					</asp:Panel>
					
					
					<fcp:CollapsiblePanel id="secAcceptMessagesFrom" runat="server"
                        TargetControlID="AcceptMessagesFrom" meta:resourcekey="secAcceptMessagesFrom" Text="Accept Messages From">
                    </fcp:CollapsiblePanel>
                    <asp:Panel ID="AcceptMessagesFrom" runat="server" Height="0" style="overflow:hidden;">
					    <fcp:AcceptedSenders id="acceptAccounts" runat="server" />
					    <asp:CheckBox ID="chkSendersAuthenticated" runat="server" meta:resourcekey="chkSendersAuthenticated" Text="Require that all senders are authenticated" />
					</asp:Panel>
					
					
					<fcp:CollapsiblePanel id="secRejectMessagesFrom" runat="server"
                        TargetControlID="RejectMessagesFrom" meta:resourcekey="secRejectMessagesFrom" Text="Reject Messages From">
                    </fcp:CollapsiblePanel>
                    <asp:Panel ID="RejectMessagesFrom" runat="server" Height="0" style="overflow:hidden;">
					    <fcp:RejectedSenders id="rejectAccounts" runat="server" />
					</asp:Panel>
					
					<table style="width:100%;margin-top:10px;">
					    <tr>
					        <td align="center">
					            <asp:CheckBox ID="chkPmmAllowed" runat="server" meta:resourcekey="chkPmmAllowed" AutoPostBack="true" Visible="false"
					                Text="Allow these settings to be managed from Personal Mailbox Manager" OnCheckedChanged="chkPmmAllowed_CheckedChanged" />
					        </td>
					    </tr>
					</table>
					

				</div>
                    </div>
				    <div class="panel-footer text-right">
                        <fcp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="EditMailbox" 
                            OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
				    </div>
