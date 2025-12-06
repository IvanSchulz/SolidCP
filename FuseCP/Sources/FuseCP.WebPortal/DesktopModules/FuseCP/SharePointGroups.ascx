<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointGroups.ascx.cs" Inherits="FuseCP.Portal.SharePointGroups" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="SharePoint"
    TypeName="FuseCP.Providers.OS.SystemGroup, FuseCP.Providers.Base"
    QuotaName="SharePoint.Groups" />
