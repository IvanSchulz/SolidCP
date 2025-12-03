<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailLists.ascx.cs" Inherits="FuseCP.Portal.MailLists" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="FuseCP.Providers.Mail.MailList, FuseCP.Providers.Base"
    QuotaName="Mail.Lists" />
