<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Statistics.ascx.cs" Inherits="FuseCP.Portal.Statistics" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="fcp" %>

<fcp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    ViewLinkText="ViewStatistics"
    GroupName="Statistics"
    TypeName="FuseCP.Providers.Statistics.StatsSite, FuseCP.Providers.Base"
    QuotaName="Stats.Sites" />
