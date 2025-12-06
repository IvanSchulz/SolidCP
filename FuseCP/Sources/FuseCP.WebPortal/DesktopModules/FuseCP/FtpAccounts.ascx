<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FtpAccounts.ascx.cs" Inherits="FuseCP.Portal.FtpAccounts" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="FTP"
    TypeName="FuseCP.Providers.FTP.FtpAccount, FuseCP.Providers.Base"
    QuotaName="FTP.Accounts" />
