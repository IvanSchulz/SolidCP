<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailForwardings.ascx.cs" Inherits="FuseCP.Portal.MailForwardings" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="FuseCP.Providers.Mail.MailAlias, FuseCP.Providers.Base"
    QuotaName="Mail.Forwardings" />
