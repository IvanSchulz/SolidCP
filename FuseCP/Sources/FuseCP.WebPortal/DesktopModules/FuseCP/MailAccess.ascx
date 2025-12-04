<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailAccess.ascx.cs" Inherits="FuseCP.Portal.MailAccess" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    ShowCreateButton="False"
    ShowQuota="False"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="FuseCP.Providers.Mail.MailDomain, FuseCP.Providers.Base"
    QuotaName="Mail.AllowAccessControls" />
