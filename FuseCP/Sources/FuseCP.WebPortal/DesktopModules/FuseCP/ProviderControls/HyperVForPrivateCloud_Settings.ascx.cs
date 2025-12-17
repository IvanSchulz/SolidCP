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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Specialized;
using FuseCP.Providers.Virtualization;
using FuseCP.EnterpriseServer;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using System.Linq;

namespace FuseCP.Portal.ProviderControls
{
    public partial class HyperVForPrivateCloud_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private StringDictionary localsettings;
        private static LibraryItem[] hosts;
        private LibraryItem[] Hosts
        {
            get
            {
                try
                {
                    if (radioServer.SelectedValue.Equals("host"))
                    {
                        hosts = ES.Services.VPSPC.GetHosts(PanelRequest.ServiceId);
                    }
                    else
                    {
                        hosts = ES.Services.VPSPC.GetClusters(PanelRequest.ServiceId);
                    }
                }
                catch
                {
                    hosts = null;
                }

                return hosts;
            }
        }

        void BindHosts()
        {
            string selectedItem = (localsettings != null ? localsettings["ServerName"] : String.Empty);

            if (!String.IsNullOrEmpty(listHosts.SelectedValue))
            {
                selectedItem = listHosts.SelectedItem.Text;
            }

            listHosts.Items.Clear();
            listHosts.DataSource = Hosts;
            listHosts.DataBind();
            listHosts.Items.Insert(0, new ListItem(GetLocalizedString("listHosts.Text"), ""));

            if (!String.IsNullOrEmpty(selectedItem))
            {
                ListItem selItem = listHosts.Items.FindByText(selectedItem);

                if (selItem != null)
                {
                    selItem.Selected = true;
                }
            }
        }

        void IHostingServiceProviderSettings.BindSettings(StringDictionary settings)
        {
            localsettings = settings;

            radioServer.SelectedValue = localsettings["ServerType"];

            if (String.IsNullOrEmpty(radioServer.SelectedValue))
            {
                radioServer.SelectedValue = "cluster";
            }

            BindHosts();
            // bind networks
            BindNetworksList();

            // CPU
            txtCpuLimit.Text = settings["CpuLimit"];
            txtCpuReserve.Text = settings["CpuReserve"];
            txtCpuWeight.Text = settings["CpuWeight"];
            txtLibraryPath.Text = settings["LibraryPath"];
            txtMonitoringServerName.Text = settings["MonitoringServerName"];

            chkUseSPNSCVMM.Checked = (String.IsNullOrEmpty(settings["UseSPNSCVMM"]) ? false : Convert.ToBoolean(settings["UseSPNSCVMM"]));
            chkUseSPNSCOM.Checked = (String.IsNullOrEmpty(settings["UseSPNSCOM"]) ? false : Convert.ToBoolean(settings["UseSPNSCOM"]));
            //// DVD library
            //txtDvdLibraryPath.Text = settings["DvdLibraryPath"];

            // VHD type
            radioVirtualDiskType.SelectedValue = settings["VirtualDiskType"];

            // External network
            ddlExternalNetworks.SelectedValue = settings["ExternalNetworkName"];

            // Private network
            ddlPrivateNetworks.SelectedValue = settings["PrivateNetworkName"];

            // host name
            txtHostnamePattern.Text = settings["HostnamePattern"];

            // start action
            radioStartAction.SelectedValue = settings["StartAction"];
            txtStartupDelay.Text = settings["StartupDelay"];

            // stop
            radioStopAction.SelectedValue = settings["StopAction"];

            //HyperVCloud
            txtSCVMMServer.Text = settings[VMForPCSettingsName.SCVMMServer.ToString()];
            txtSCVMMPrincipalName.Text = settings[VMForPCSettingsName.SCVMMPrincipalName.ToString()];

            txtSCOMServer.Text = settings[VMForPCSettingsName.SCOMServer.ToString()];
            txtSCOMPrincipalName.Text = settings[VMForPCSettingsName.SCOMPrincipalName.ToString()];

            //Check State 
            CheckServerAndSetState(btnSCVMMServer, VMForPCSettingsName.SCVMMServer, txtSCVMMServer.Text, txtSCVMMPrincipalName.Text);
            CheckServerAndSetState(btnSCOMServer, VMForPCSettingsName.SCOMServer, txtSCOMServer.Text, txtSCOMPrincipalName.Text);
        }

        void IHostingServiceProviderSettings.SaveSettings(StringDictionary settings)
        {

            settings["ServerType"] = radioServer.SelectedValue;
            settings["ServerName"] = listHosts.SelectedItem.Text.Trim();

            // CPU
            settings["CpuLimit"] = txtCpuLimit.Text.Trim();
            settings["CpuReserve"] = txtCpuReserve.Text.Trim();
            settings["CpuWeight"] = txtCpuWeight.Text.Trim();

            settings["MonitoringServerName"] = txtMonitoringServerName.Text.Trim();
            settings["LibraryPath"] = txtLibraryPath.Text.Trim();

            settings["UseSPNSCVMM"] = chkUseSPNSCVMM.Checked.ToString();
            settings["UseSPNSCOM"] = chkUseSPNSCOM.Checked.ToString();

            // VHD type
            settings["VirtualDiskType"] = radioVirtualDiskType.SelectedValue;

            // External network
            settings["ExternalNetworkName"] = ddlExternalNetworks.SelectedValue;

            // Private network
            settings["PrivateNetworkName"] = ddlPrivateNetworks.SelectedValue;

            // host name
            settings["HostnamePattern"] = txtHostnamePattern.Text.Trim();

            // start action
            settings["StartAction"] = radioStartAction.SelectedValue;
            settings["StartupDelay"] = Utils.ParseInt(txtStartupDelay.Text.Trim(), 0).ToString();

            // stop
            settings["StopAction"] = radioStopAction.SelectedValue;

            //HyperVCloud
            settings[VMForPCSettingsName.SCVMMServer.ToString()] = txtSCVMMServer.Text.Trim();
            settings[VMForPCSettingsName.SCVMMPrincipalName.ToString()] = txtSCVMMPrincipalName.Text.Trim();

            settings[VMForPCSettingsName.SCOMServer.ToString()] = txtSCOMServer.Text.Trim();
            settings[VMForPCSettingsName.SCOMPrincipalName.ToString()] = txtSCOMPrincipalName.Text.Trim();
        }

        protected void radioServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindHosts();
        }

        protected void listHosts_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindNetworksList();
        }

        private void BindNetworksList()
        {
            try
            {
                if (ddlExternalNetworks.Items != null)
                {
                    ddlExternalNetworks.Items.Clear();
                }

                if (ddlPrivateNetworks.Items != null)
                {
                    ddlPrivateNetworks.Items.Clear();
                }

                LibraryItem[] localHosts = hosts ?? Hosts;

                VirtualNetworkInfo[] networks = localHosts.Where(item => item.Path == listHosts.SelectedValue).Select(item => item.Networks).FirstOrDefault();

                ddlExternalNetworks.DataSource = networks ?? new VirtualNetworkInfo[] {};
                ddlExternalNetworks.DataBind();

                ddlPrivateNetworks.DataSource = networks ?? new VirtualNetworkInfo[] { };
                ddlPrivateNetworks.DataBind();
            }
            catch
            {
                ddlExternalNetworks.Items.Add(new ListItem(GetLocalizedString("ErrorReadingNetworksList.Text"), ""));
                ddlPrivateNetworks.Items.Add(new ListItem(GetLocalizedString("ErrorReadingNetworksList.Text"), ""));
            }
        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            BindNetworksList();
        }

        protected void btnSCVMMServer_Click(object sender, EventArgs e)
        {
            if (CheckServerAndSetState(sender, VMForPCSettingsName.SCVMMServer, txtSCVMMServer.Text, txtSCVMMPrincipalName.Text.Trim()))
            {
                BindHosts();
            }
        }

        protected void btnSCOMServer_Click(object sender, EventArgs e)
        {
            CheckServerAndSetState(sender, VMForPCSettingsName.SCOMServer, txtSCOMServer.Text, txtSCOMPrincipalName.Text.Trim());
        }

        private bool CheckServerAndSetState(object obj, VMForPCSettingsName control, string conn, string name)
        {
            bool temp = false;
            try
            {
                temp = ES.Services.VPSPC.CheckServerState(control, conn, name, PanelRequest.ServiceId);
            }
            catch (Exception ex)
            {
                messageBoxError.ShowErrorMessage("Server Error", ex);
            }
            finally
            {
                ((WebControl)obj).CssClass = temp ? "enabled" : "disabled";
            }
            return temp;
        }
    }
}
