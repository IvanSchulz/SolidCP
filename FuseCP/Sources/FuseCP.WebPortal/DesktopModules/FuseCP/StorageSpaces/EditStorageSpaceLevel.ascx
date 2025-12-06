<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditStorageSpaceLevel.ascx.cs" Inherits="FuseCP.Portal.StorageSpaces.EditStorageSpaceLevel" %>

<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="fcp" %>

<%@ Register TagPrefix="fcp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="fcp" %>
<%@ Register Src="UserControls/StorageSpaceLevelResourceGroups.ascx" TagName="ResourceGroups" TagPrefix="fcp" %>


<fcp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />


        <div class="panel-body form-horizontal">
            <fcp:SimpleMessageBox ID="messageBox" runat="server" />

            <fcp:CollapsiblePanel ID="colSsLevelGeneralSettings" runat="server"
                TargetControlID="panelSsLevelGeneralSettings" meta:ResourceKey="colSsLevelGeneralSettings"></fcp:CollapsiblePanel>

            <asp:Panel runat="server" ID="panelSsLevelGeneralSettings">
                <div style="padding: 10px;">
                    <table>
                        <tr>
                            <td class="Label" style="width: 260px;">
                                <asp:Localize ID="locName" runat="server" meta:resourcekey="locName"></asp:Localize>
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ID="valReqTxtName" ControlToValidate="txtName" meta:resourcekey="valReqTxtName" ErrorMessage="*" ValidationGroup="SaveSsLevel" />
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" style="width: 260px;">
                                <asp:Localize ID="locDescription" runat="server" meta:resourcekey="locDescription"></asp:Localize>
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                <asp:RequiredFieldValidator runat="server" ID="valReqTxtDescription" ControlToValidate="txtDescription" meta:resourcekey="valReqTxtDescription" ErrorMessage="*" ValidationGroup="SaveSsLevel" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </asp:Panel>

            <fcp:CollapsiblePanel ID="colSsLevelServices" runat="server"
                TargetControlID="panelSsLevelServices" meta:ResourceKey="colSsLevelServices"></fcp:CollapsiblePanel>

            <asp:Panel runat="server" ID="panelSsLevelServices">
                <table>
                    <tr>
                        <td colspan="2">
                            <fieldset id="Fieldset1" runat="server">
                                <legend>
                                    <asp:Localize ID="locResourceGroups" runat="server" meta:resourcekey="locResourceGroups" Text="Allowed Resource Groups"></asp:Localize></legend>
                                <fcp:ResourceGroups ID="resourceGroups" runat="server" />
                            </fieldset>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>

                </table>
            </asp:Panel>

        </div>
            <div class="panel-footer text-right">
                <fcp:ItemButtonPanel ID="buttonPanel" runat="server" ValidationGroup="SaveSsLevel"
                    OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
            </div>
