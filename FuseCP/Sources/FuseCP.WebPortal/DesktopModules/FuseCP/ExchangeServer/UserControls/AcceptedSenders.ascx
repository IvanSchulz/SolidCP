<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AcceptedSenders.ascx.cs" Inherits="FuseCP.Portal.ExchangeServer.UserControls.AcceptedSenders" %>
<%@ Register Src="AccountsList.ascx" TagName="AccountsList" TagPrefix="fcp" %>
<asp:UpdatePanel ID="MainUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
    
	<table>
		<tr>
			<td>
				<asp:RadioButtonList ID="rblAcceptMessages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblAcceptMessages_SelectedIndexChanged">
					<asp:ListItem Text="All senders" meta:resourcekey="rblAcceptMessagesAll"></asp:ListItem>
					<asp:ListItem Text="Only senders in the following list" meta:resourcekey="rblAcceptMessagesOnlyList"></asp:ListItem>
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr>
			<td>
				<fcp:AccountsList id="acceptAccounts" runat="server"
					MailboxesEnabled="true"
					ContactsEnabled="true"
					DistributionListsEnabled="false">
				</fcp:AccountsList>
			</td>
		</tr>
	</table>

	</ContentTemplate>
</asp:UpdatePanel>
