<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlDatabases.ascx.cs" Inherits="FuseCP.Portal.SqlDatabases" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddDatabase"
    CreateControlID="edit_item"
    GroupName="MsSQL2000"
    TypeName="FuseCP.Providers.Database.SqlDatabase, FuseCP.Providers.Base"
    QuotaName="MsSQL2000.Databases" />
