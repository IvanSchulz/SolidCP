<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailDomains.ascx.cs" Inherits="FuseCP.Portal.MailDomains" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    ShowCreateButton="False"
    ShowQuota="False"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="FuseCP.Providers.Mail.MailDomain, FuseCP.Providers.Base"
    QuotaName="Mail.Domains" />
