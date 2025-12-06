<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxPermissions.ascx.cs" Inherits="FuseCP.Portal.ExchangeServer.ExchangeMailboxPermissions" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="uc2" %>
<%@ Register Src="UserControls/AccountsListWithPermissions.ascx" TagName="AccountsListWithPermissions" TagPrefix="uc2" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="fcp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="fcp" %>
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
                    <fcp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_permissions" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <fcp:SimpleMessageBox id="messageBox" runat="server" />                    					
					
                    <fcp:CollapsiblePanel id="secFullAccessPermission" runat="server"
                        TargetControlID="panelFullAccessPermission" meta:resourcekey="secFullAccessPermission" Text="Full Access Permission">
                    </fcp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelFullAccessPermission">
                        <asp:Label runat="server" ID="Label1" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsList id="fullAccessPermission" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true" DistributionListsEnabled="true" SecurityGroupsEnabled="false">
                        </uc2:AccountsList>                                            
                    </asp:Panel>
                    
                    <fcp:CollapsiblePanel id="secSendAsPermission" runat="server"
                        TargetControlID="panelSendAsPermission" meta:resourcekey="secSendAsPermission" Text="Send As Permission">
                    </fcp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelSendAsPermission">
                        <asp:Label runat="server" ID="lblSendAsPermission" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsList id="sendAsPermission" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true" DistributionListsEnabled="true" SecurityGroupsEnabled="false">
                        </uc2:AccountsList>                                           
                    </asp:Panel>

                    <fcp:CollapsiblePanel id="secOnBehalfOf" runat="server"
                        TargetControlID="panelOnBehalfOf" meta:resourcekey="secOnBehalfOf" Text="Send on Behalf">
                    </fcp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelOnBehalfOf">
                        <asp:Label runat="server" ID="Label2" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsList id="onBehalfOfPermissions" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true" DistributionListsEnabled="true" SecurityGroupsEnabled="false">
                        </uc2:AccountsList>                                            
                    </asp:Panel>
                    
                    <fcp:CollapsiblePanel id="secCalendarPermissions" runat="server"
                        TargetControlID="panelCalendarPermissions" meta:resourcekey="secCalendarPermissions" Text="Calendar access">
                    </fcp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelCalendarPermissions">
                        <asp:Label runat="server" ID="Label3" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsListWithPermissions id="calendarPermissions" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true" SecurityGroupsEnabled="false">
                        </uc2:AccountsListWithPermissions>                                            
                    </asp:Panel>
                    
                    <fcp:CollapsiblePanel id="secContactsPermissions" runat="server"
                        TargetControlID="panelContactsPermissions" meta:resourcekey="secContactsPermissions" Text="Contacts access">
                    </fcp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelContactsPermissions">
                        <asp:Label runat="server" ID="Label4" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsListWithPermissions id="contactsPermissions" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true" SecurityGroupsEnabled="false">
                        </uc2:AccountsListWithPermissions>                                            
                    </asp:Panel>


                        </div>
				</div>
<div class="panel-footer text-right">
                        <fcp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="EditMailbox" 
                            OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
			        </div>		
