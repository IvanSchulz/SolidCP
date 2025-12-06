<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBUserSettings.ascx.cs" Inherits="FuseCP.Portal.SfB.UserControls.SfBUserSettings" %>
<%@ Register Src="../../ExchangeServer/UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="fcp" %>
<asp:DropDownList ID="ddlSipAddresses" runat="server" CssClass="form-control"></asp:DropDownList>
<fcp:EmailAddress id="email" runat="server" ValidationGroup="CreateMailbox"></fcp:EmailAddress>

