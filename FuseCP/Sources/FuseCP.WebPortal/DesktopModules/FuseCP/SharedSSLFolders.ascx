<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedSSLFolders.ascx.cs" Inherits="FuseCP.Portal.SharedSSLFolders" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="add"
    GroupName="Web"
    TypeName="FuseCP.Providers.Web.SharedSSLFolder, FuseCP.Providers.Base"
    QuotaName="Web.SharedSSL" />
