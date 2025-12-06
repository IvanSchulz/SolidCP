<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OdbcSources.ascx.cs" Inherits="FuseCP.Portal.OdbcSources" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="OS"
    TypeName="FuseCP.Providers.OS.SystemDSN, FuseCP.Providers.Base"
    QuotaName="OS.ODBC" />
