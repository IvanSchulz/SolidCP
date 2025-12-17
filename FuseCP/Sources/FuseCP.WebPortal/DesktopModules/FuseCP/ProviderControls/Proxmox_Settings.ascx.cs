// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.ProviderControls
{
	public partial class Proxmox_Settings : FuseCPControlBase, IHostingServiceProviderSettings
	{
		// Distinguish between Proxmox and Proxmox (localhost)
		protected bool IsLocal {
			get {
				var isLocal = (bool?)ViewState[nameof(IsLocal)];
				if (isLocal == null)
				{
					var service = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);
					var provider = ES.Services.Servers.GetProvider(service.ProviderId);
					isLocal = IsLocal = provider.ProviderType == "FuseCP.Providers.Virtualization.ProxmoxvpsLocal, FuseCP.Providers.Virtualization.Proxmoxvps";
				}
				return isLocal.Value;
			}
			set => ViewState[nameof(IsLocal)] = value;
		}
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		void IHostingServiceProviderSettings.BindSettings(StringDictionary settings)
		{
			// Proxmox Cluster Settings
			txtProxmoxClusterServerHost.Text = IsLocal ? "localhost" : settings["ProxmoxClusterServerHost"];
			txtProxmoxClusterServerPort.Text = settings["ProxmoxClusterServerPort"] ?? "8006";
			txtProxmoxClusterAdminUser.Text = settings["ProxmoxClusterAdminUser"];
			var realm = settings["ProxmoxClusterRealm"] ?? "pam";
			if (string.Equals(realm, "pam", StringComparison.OrdinalIgnoreCase)) lstProxmoxClusterRealm.SelectedIndex = 0;
			else lstProxmoxClusterRealm.SelectedIndex = 1;
			bool trustCert = IsLocal;
			bool.TryParse(settings["ProxmoxTrustClusterServerCertificate"] ?? $"{IsLocal}", out trustCert);
			chkProxmoxTrustServerCertificate.Checked = trustCert;
			chkProxmoxTrustServerCertificate.Enabled = !IsLocal;
			ViewState["PWD"] = settings["ProxmoxClusterAdminPass"];
			rowPassword.Visible = ((string)ViewState["PWD"]) != "";

			// Proxmox SSH Settings
			txtDeploySSHServerHost.Text = settings["DeploySSHServerHost"];
			txtDeploySSHServerPort.Text = settings["DeploySSHServerPort"] ?? "22";
			txtDeploySSHUser.Text = settings["DeploySSHUser"];
			ViewState["SSHPWD"] = settings["DeploySSHPass"];
			rowSSHPassword.Visible = ((string)ViewState["SSHPWD"]) != "";
			txtDeploySSHKey.Text = settings["DeploySSHKey"];
			ViewState["SSHKEYPWD"] = settings["DeploySSHKeyPass"];
			rowSSHKEYPassword.Visible = ((string)ViewState["SSHKEYPWD"]) != "";
			txtDeploySSHScript.Text = settings["DeploySSHScript"];
			txtDeploySSHScriptParams.Text = settings["DeploySSHScriptParams"];
			DeploySSHServerHostValidator.Visible = DeploySSHServerPortValidator.Visible =
				DeploySSHUserValidator.Visible = pnlSshSettings.Visible =
				ProxmoxClusterServerHostValidator.Visible = 
				rowServerHost.Visible = !IsLocal;
			
			// OS Templates
			//txtOSTemplatesPath.Text = settings["OsTemplatesPath"];
			repOsTemplates.DataSource = new ConfigFile(settings["OsTemplates"])?.LibraryItems; //ES.Services.VPS2012.GetOperatingSystemTemplatesByServiceId(PanelRequest.ServiceId).ToList();
			repOsTemplates.DataBind();

			// DVD Path
			txtProxmoxIsosonStorage.Text = settings["ProxmoxIsosonStorage"];

			// host name
			txtHostnamePattern.Text = settings["HostnamePattern"];

		}

		void IHostingServiceProviderSettings.SaveSettings(StringDictionary settings)
		{
			// Proxmox Cluster Settings
			settings["ProxmoxClusterServerHost"] = txtProxmoxClusterServerHost.Text.Trim();
			settings["ProxmoxClusterServerPort"] = txtProxmoxClusterServerPort.Text.Trim();
			settings["ProxmoxClusterAdminUser"] = txtProxmoxClusterAdminUser.Text.Trim();
			settings["ProxmoxClusterRealm"] = lstProxmoxClusterRealm.SelectedValue; // txtProxmoxClusterRealm.Text.Trim();
			settings["ProxmoxClusterAdminPass"] = (txtProxmoxClusterAdminPass.Text.Length > 0) ? txtProxmoxClusterAdminPass.Text : (string)ViewState["PWD"];
			settings["ProxmoxTrustClusterServerCertificate"] = chkProxmoxTrustServerCertificate.Checked.ToString();

			// Proxmox SSH Settings
			settings["DeploySSHServerHost"] = txtDeploySSHServerHost.Text.Trim();
			settings["DeploySSHServerPort"] = txtDeploySSHServerPort.Text.Trim();
			settings["DeploySSHUser"] = txtDeploySSHUser.Text.Trim();
			settings["DeploySSHPass"] = (txtDeploySSHPass.Text.Length > 0) ? txtDeploySSHPass.Text : (string)ViewState["SSHPWD"];
			if (chkdelsshpass.Checked == true)
				settings["DeploySSHPass"] = "";
			settings["DeploySSHKey"] = txtDeploySSHKey.Text.Trim();
			settings["DeploySSHKeyPass"] = (txtDeploySSHKeyPass.Text.Length > 0) ? txtDeploySSHKeyPass.Text : (string)ViewState["SSHKEYPWD"];
			if (chkdelsshkeypass.Checked == true)
				settings["DeploySSHKeyPass"] = "";
			settings["DeploySSHScript"] = txtDeploySSHScript.Text.Trim();
			settings["DeploySSHScriptParams"] = txtDeploySSHScriptParams.Text.Trim();

			// OS Templates
			settings["OsTemplates"] = GetConfigXml(GetOsTemplates());
			//settings["OsTemplatesPath"] = txtOSTemplatesPath.Text.Trim();

			// DVD Path
			settings["ProxmoxIsosonStorage"] = txtProxmoxIsosonStorage.Text.Trim();

			// host name
			settings["HostnamePattern"] = txtHostnamePattern.Text.Trim();

		}

		private List<ServiceInfo> GetServices(string data)
		{
			if (string.IsNullOrEmpty(data))
				return null;
			List<ServiceInfo> list = new List<ServiceInfo>();
			string[] servicesIds = data.Split(',');
			foreach (string current in servicesIds)
			{
				ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(Utils.ParseInt(current));
				list.Add(serviceInfo);
			}

			return list;
		}

		private StringDictionary ConvertArrayToDictionary(string[] settings)
		{
			StringDictionary r = new StringDictionary();
			foreach (string setting in settings)
			{
				int idx = setting.IndexOf('=');
				r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
			}
			return r;
		}





		// OS Templates
		protected void btnAddOsTemplate_Click(object sender, EventArgs e)
		{
			var templates = GetOsTemplates();

			templates.Add(new LibraryItem());

			RebindOsTemplate(templates);
		}

		protected void btnRemoveOsTemplate_OnCommand(object sender, CommandEventArgs e)
		{
			var templates = GetOsTemplates();

			templates.RemoveAt(Convert.ToInt32(e.CommandArgument));

			RebindOsTemplate(templates);
		}

		private List<LibraryItem> GetOsTemplates()
		{
			var result = new List<LibraryItem>();

			foreach (RepeaterItem item in repOsTemplates.Items)
			{
				var template = new LibraryItem();
				int processVolume;

				template.Name = GetTextBoxText(item, "txtTemplateName");
				template.Path = GetTextBoxText(item, "txtTemplateFileName");

				template.DeployScriptParams = GetTextBoxText(item, "txtDeployScriptParams");

				int.TryParse(GetTextBoxText(item, "txtProcessVolume"), out processVolume);
				template.ProcessVolume = processVolume;

				template.LegacyNetworkAdapter = GetCheckBoxValue(item, "chkLegacyNetworkAdapter");
				template.RemoteDesktop = true; // obsolete
				template.ProvisionComputerName = GetCheckBoxValue(item, "chkCanSetComputerName");
				template.ProvisionAdministratorPassword = GetCheckBoxValue(item, "chkCanSetAdminPass");
				template.ProvisionNetworkAdapters = GetCheckBoxValue(item, "chkCanSetNetwork");


				//var syspreps = GetTextBoxText(item, "txtSysprep").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
				//template.SysprepFiles = syspreps.Select(s => s.Trim()).ToArray();
				template.SysprepFiles = new string[0];

				result.Add(template);
			}

			return result;
		}
		private void RebindOsTemplate(List<LibraryItem> templates)
		{
			repOsTemplates.DataSource = templates;
			repOsTemplates.DataBind();
		}

		private string GetConfigXml(List<LibraryItem> items)
		{
			var templates = items.ToArray();
			return new ConfigFile(templates).Xml;
		}

		private string GetTextBoxText(RepeaterItem item, string name)
		{
			return (item.FindControl(name) as TextBox).Text;
		}

		private bool GetCheckBoxValue(RepeaterItem item, string name)
		{
			return (item.FindControl(name) as CheckBox).Checked;
		}


	}
}
