<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlUsers.ascx.cs" Inherits="FuseCP.Portal.SqlUsers" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddUser"
    CreateControlID="edit_item"
    GroupName="MsSQL2000"
    TypeName="FuseCP.Providers.Database.SqlUser, FuseCP.Providers.Base"
    QuotaName="MsSQL2000.Users" />
