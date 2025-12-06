<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSites.ascx.cs" Inherits="FuseCP.Portal.WebSites" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="fcp" %>

<fcp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddWebSite"
    CreateControlID="add_site"
    GroupName="Web"
    TypeName="FuseCP.Providers.Web.WebSite, FuseCP.Providers.Base"
    QuotaName="Web.Sites" />
