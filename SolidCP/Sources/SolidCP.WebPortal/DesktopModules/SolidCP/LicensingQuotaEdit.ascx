<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LicensingQuotaEdit.ascx.cs" Inherits="SolidCP.Portal.LicensingQuotaEdit" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-heading">
	<h3 class="panel-title">
		<asp:Image ID="Image1" SkinID="cabinet_48" runat="server" />
		<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Licensing Quota"></asp:Localize>
		-
		<asp:Literal ID="litQuotaName" runat="server" Text="License 1" />
	</h3>
</div>

<div class="panel-body form-horizontal">
	<scp:SimpleMessageBox id="messageBox" runat="server" />
	<table class="col-sm-12">
		<tr>
			<td class="col-sm-2">
				<asp:Localize ID="locDescription" runat="server" meta:resourcekey="locDescription" Text="License description: "></asp:Localize>
			</td>
			<td>
				<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" ValidationGroup="EditQuota"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td></td>
			<td>
				<asp:CheckBox ID="chkActive" runat="server" meta:resourcekey="chkActive" Text="Active" />
				<br />
				<br />
			</td>
		</tr>
		<tr>
			<td class="col-sm-2">
				<asp:Localize ID="locVmTemplates" runat="server" meta:resourcekey="locVmTemplates" Text="VM templates: "></asp:Localize>
			</td>
			<td>
				<div style="margin-top:15px; margin-bottom:25px; margin-left:10px;">
					<CPCC:StyleButton id="btnAddVmTemplateName" CssClass="btn btn-success" runat="server" OnClick="btnAddVmTemplateName_Click" CausesValidation="false"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddVmTemplateName"/> </CPCC:StyleButton>
				</div>
				<asp:Repeater ID="repVmTemplateNames" runat="server">
					<HeaderTemplate>
					</HeaderTemplate>
					<ItemTemplate>
						<div class="col-sm-11 form-inline">
							<asp:Label ID="lblVmTemplateName" meta:resourcekey="lblVmTemplateName" runat="server" Text="VM Template Name: *" AssociatedControlID="txtVmTemplateName"></asp:Label>
							<asp:TextBox ID="txtVmTemplateName" runat="server" CssClass="form-control" Width="270" Text='<%# Container.DataItem.ToString() %>'></asp:TextBox>
							<asp:RequiredFieldValidator ID="requireVmTemplateValidator" runat="server" Text="*" Display="Dynamic"
								ControlToValidate="txtVmTemplateName" meta:resourcekey="requireVmTemplateValidator" SetFocusOnError="true"
								ValidationGroup="EditQuota">*</asp:RequiredFieldValidator>
							<CPCC:StyleButton id="btnRemoveVmTemplateName" style="margin-left: 30px;" CssClass="btn btn-danger" runat="server" CausesValidation="false" CommandName="Remove" CommandArgument="<%# Container.ItemIndex %>" OnCommand="btnRemoveVmTemplateName_OnCommand">
								<i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="locRemoveVmTemplateName"/> </CPCC:StyleButton>
						</div>
					</ItemTemplate>
					<SeparatorTemplate>
						<br/>
                    </SeparatorTemplate>
				</asp:Repeater>
			</td>
		</tr>
	</table>
	<div class="text-right">
		<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
		<CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" ValidationGroup="EditQuota"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/> </CPCC:StyleButton>
	</div>
</div>