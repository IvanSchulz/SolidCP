<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceNotes.ascx.cs" Inherits="FuseCP.Portal.SpaceNotes" %>
<%@ Register Src="UserControls/EditItemComments.ascx" TagName="EditItemComments" TagPrefix="scp" %>


<scp:EditItemComments ID="packageComments" runat="server"
    ItemTypeId="PACKAGE" RequestItemId="SpaceID" />
