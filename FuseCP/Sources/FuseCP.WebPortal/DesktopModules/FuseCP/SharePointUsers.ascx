<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointUsers.ascx.cs" Inherits="FuseCP.Portal.SharePointUsers" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="SharePoint"
    TypeName="FuseCP.Providers.OS.SystemUser, FuseCP.Providers.Base"
    QuotaName="SharePoint.Users" />
