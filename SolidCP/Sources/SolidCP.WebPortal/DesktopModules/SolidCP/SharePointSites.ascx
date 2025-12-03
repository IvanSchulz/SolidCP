<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointSites.ascx.cs" Inherits="FuseCP.Portal.SharePointSites" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="SharePoint"
    TypeName="FuseCP.Providers.SharePoint.SharePointSite, FuseCP.Providers.Base"
    QuotaName="SharePoint.Sites" />
