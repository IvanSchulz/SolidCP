<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountNotes.ascx.cs" Inherits="FuseCP.Portal.UserAccountNotes" %>
<%@ Register Src="UserControls/EditItemComments.ascx" TagName="EditItemComments" TagPrefix="fcp" %>
   
<fcp:EditItemComments ID="userComments" runat="server"
    ItemTypeId="USER" />
            
