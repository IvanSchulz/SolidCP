<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailGroups.ascx.cs" Inherits="FuseCP.Portal.MailGroups" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="FuseCP.Providers.Mail.MailGroup, FuseCP.Providers.Base"
    QuotaName="Mail.Groups" />
