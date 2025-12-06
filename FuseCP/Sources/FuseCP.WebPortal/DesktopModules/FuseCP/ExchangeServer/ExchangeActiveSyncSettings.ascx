<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeActiveSyncSettings.ascx.cs" Inherits="FuseCP.Portal.ExchangeServer.ExchangeActiveSyncSettings" %>
<%@ Register Src="UserControls/HoursBox.ascx" TagName="HoursBox" TagPrefix="fcp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="fcp" %>
<%@ Register Src="UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="fcp" %>
<%@ Register Src="UserControls/DaysBox.ascx" TagName="DaysBox" TagPrefix="fcp" %>
<%@ Register TagPrefix="fcp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="fcp" %>

<fcp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeActiveSyncConfig48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="ActiveSync Mailbox Policy"></asp:Localize>
				    </h3>
                </div>
				<div class="panel-body form-horizontal">
				    <fcp:SimpleMessageBox id="messageBox" runat="server" />
				    
				    <table>
					    <tr>
						    <td class="Normal" colspan="2">
						        <asp:CheckBox ID="chkAllowNonProvisionable" runat="server"
						            meta:resourcekey="chkAllowNonProvisionable" Text="Allow Non-provisionable devices" />
						    </td>
						</tr>
						<tr>
						    <td nowrap><asp:Label meta:resourcekey="lblRefreshInterval" runat="server" ID="lblRefreshInterval" /></td>
						    <td><fcp:HoursBox id="hoursRefreshInterval" runat="server"  ValidationGroup="EditMailbox">
                                </fcp:HoursBox>
				            </td>
						</tr>
					</table>
                    
					<fcp:CollapsiblePanel id="secApplication" runat="server"
                        TargetControlID="ApplicationPanel" meta:resourcekey="secApplication" Text="Application">
                    </fcp:CollapsiblePanel>
                    <asp:Panel ID="ApplicationPanel" runat="server" Height="0" style="overflow:hidden;">
                        <table>
					        <tr>
						        <td class="Normal" colspan="2">
						            <asp:CheckBox ID="chkAllowAttachments" runat="server"
						                meta:resourcekey="chkAllowAttachments" Text="Allow attachments to be downloaded to device" />
						        </td>
						    </tr>
						    <tr>
							    <td class="FormLabel200" align="right">
							        <asp:Localize ID="locMaxAttachmentSize" runat="server"
							            meta:resourcekey="locMaxAttachmentSize" Text="Maximum attachment size:"></asp:Localize></td>
							    <td>
									<fcp:SizeBox id="sizeMaxAttachmentSize" runat="server" ValidationGroup="EditMailbox" DisplayUnitsKB="true" DisplayUnitsMB="false" DisplayUnitsPct="false"/>
								</td>
						    </tr>
						</table>
						<br />
				    </asp:Panel>
				    
				    
					<fcp:CollapsiblePanel id="secWSS" runat="server"
                        TargetControlID="WSSPanel" meta:resourcekey="secWSS" Text="WSS/UNC">
                    </fcp:CollapsiblePanel>
                    <asp:Panel ID="WSSPanel" runat="server" Height="0" style="overflow:hidden;">
				        <table>
					        <tr>
						        <td class="Normal" colspan="2">
						            <asp:CheckBox ID="chkWindowsFileShares" runat="server"
						                meta:resourcekey="chkWindowsFileShares" Text="Windows File Shares" />
						        </td>
						    </tr>
					        <tr>
						        <td class="Normal" colspan="2">
						            <asp:CheckBox ID="chkWindowsSharePoint" runat="server"
						                meta:resourcekey="chkWindowsSharePoint" Text="Windows SharePoint Services" />
						        </td>
						    </tr>
					    </table>
					    <br />
				    </asp:Panel>
				    
				    
					<fcp:CollapsiblePanel id="secPassword" runat="server"
                        TargetControlID="PasswordPanel" meta:resourcekey="secPassword" Text="Password">
                    </fcp:CollapsiblePanel>
                    <asp:Panel ID="PasswordPanel" runat="server" Height="0" style="overflow:hidden;">
                    
						<asp:UpdatePanel ID="PasswordUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
							<ContentTemplate>
							
				        <table>
					        <tr>
						        <td class="Normal" colspan="2">
						            <asp:CheckBox ID="chkRequirePasword" runat="server"
						                meta:resourcekey="chkRequirePasword" Text="Require password" AutoPostBack="true" OnCheckedChanged="chkRequirePasword_CheckedChanged" />
						        </td>
						    </tr>
					        <tr id="PasswordSettingsRow" runat="server">
						        <td class="Normal" colspan="2" style="padding-left: 20px;">

				                    <table>
					                    <tr>
						                    <td class="Normal" colspan="2">
						                        <asp:CheckBox ID="chkRequireAlphaNumeric" runat="server"
						                            meta:resourcekey="chkRequireAlphaNumeric" Text="Require alphnumeric password" />
						                    </td>
						                </tr>
					                    <tr>
						                    <td class="Normal" colspan="2">
						                        <asp:CheckBox ID="chkEnablePasswordRecovery" runat="server"
						                            meta:resourcekey="chkEnablePasswordRecovery" Text="Enable Password Recovery" />
						                    </td>
						                </tr>
					                    <tr>
						                    <td class="Normal" colspan="2">
						                        <asp:CheckBox ID="chkRequireEncryption" runat="server"
						                            meta:resourcekey="chkRequireEncryption" Text="Require encryption on device" />
						                    </td>
						                </tr>
					                    <tr>
						                    <td class="Normal" colspan="2">
						                        <asp:CheckBox ID="chkAllowSimplePassword" runat="server"
						                            meta:resourcekey="chkAllowSimplePassword" Text="Allow simple password" />
						                    </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locNumberAttempts" runat="server"
							                        meta:resourcekey="locNumberAttempts" Text="Number of failed attempts allowed:"></asp:Localize></td>
							                <td>
									            <fcp:SizeBox id="sizeNumberAttempts" runat="server" ValidationGroup="EditMailbox" DisplayUnits="false" DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="false"/>
								            </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locMinimumPasswordLength" runat="server"
							                        meta:resourcekey="locMinimumPasswordLength" Text="Minimum password length:"></asp:Localize></td>
							                <td>
									            <fcp:SizeBox id="sizeMinimumPasswordLength" runat="server" ValidationGroup="EditMailbox"
									                DisplayUnits="false" EmptyValue="0"  DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="false"/>
								            </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locTimeReenter" runat="server"
							                        meta:resourcekey="locTimeReenter" Text="Time without user input before password must be re-entered:"></asp:Localize></td>
							                <td>
									            <fcp:SizeBox id="sizeTimeReenter" runat="server" ValidationGroup="EditMailbox" DisplayUnits="false"  DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="false"/>
									            <asp:Localize ID="locMinutes" runat="server"
							                        meta:resourcekey="locMinutes" Text="minutes"></asp:Localize>
								            </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locPasswordExpiration" runat="server"
							                        meta:resourcekey="locPasswordExpiration" Text="Password expiration:"></asp:Localize></td>
							                <td>
									            <fcp:SizeBox id="sizePasswordExpiration" runat="server" ValidationGroup="EditMailbox" DisplayUnits="false"  DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="false"/>
									            <asp:Localize ID="locDays" runat="server"
							                        meta:resourcekey="locDays" Text="days"></asp:Localize>
								            </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locPasswordHistory" runat="server"
							                        meta:resourcekey="locPasswordHistory" Text="Enforce password history:"></asp:Localize></td>
							                <td>
									            <fcp:SizeBox id="sizePasswordHistory" runat="server" ValidationGroup="EditMailbox" DisplayUnits="false" RequireValidatorEnabled="true"  DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="false"/>
								            </td>
						                </tr>
					                </table>

						        </td>
						    </tr>
					    </table>
					    
					        </ContentTemplate>
					    </asp:UpdatePanel>
					    <br />
				    </asp:Panel>
				    
					

				    
				</div>
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditMailbox" OnClientClick="ShowProgressDialog('Updating settings...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>&nbsp;
						<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
				    </div>
