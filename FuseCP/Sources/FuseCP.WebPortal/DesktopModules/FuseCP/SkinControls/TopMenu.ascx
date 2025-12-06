<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenu.ascx.cs" Inherits="FuseCP.Portal.SkinControls.TopMenu" %>
<asp:SiteMapDataSource ID="siteMapSource" runat="server" ShowStartingNode="False" />

<asp:Menu ID="topMenu" runat="server" DataSourceID="siteMapSource"
    CssSelectorClass="TopMenu"
    EnableViewState="False" onmenuitemdatabound="topMenu_MenuItemDataBound">
</asp:Menu>