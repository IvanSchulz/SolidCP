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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.Virtualization;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.Proxmox.UserControls
{
    public partial class ServerTabs : FuseCPControlBase
    {
        class Tab
        {
            string id;
            string name;
            string url;

            public Tab(string id, string name, string url)
            {
                this.id = id;
                this.name = name;
                this.url = url;
            }

            public string Id
            {
                get { return this.id; }
                set { this.id = value; }
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }

            public string Url
            {
                get { return this.url; }
                set { this.url = value; }
            }
        }

        private string selectedTab;
        public string SelectedTab
        {
            get { return selectedTab; }
            set { selectedTab = value; }
        }

        private BackgroundTask task = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            // load item
            VirtualMachine vm = VirtualMachinesProxmoxHelper.GetCachedVirtualMachine(PanelRequest.ItemID);

            if (!String.IsNullOrEmpty(vm.CurrentTaskId))
            {
                // show panel
                TaskTable.Visible = true;

                // bind task details
                BindTask(vm);

                return;
            }

            if (TaskTable.Visible)
                Response.Redirect(Request.Url.ToString()); // refresh screen

            // show tabs
            TabsTable.Visible = true;

            // disable timer
            refreshTimer.Enabled = false;

            // check if Proxmox created with error
            bool createError = (vm.ProvisioningStatus == VirtualMachineProvisioningStatus.Error);

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            // build tabs list
            List<Tab> tabsList = new List<Tab>();

            tabsList.Add(CreateTab("vps_general", "Tab.General"));

            if (!createError)
                tabsList.Add(CreateTab("vps_config", "Tab.Configuration"));

            if (vm.DvdDriveInstalled && !createError)
                tabsList.Add(CreateTab("vps_dvd", "Tab.DVD"));

            if (vm.SnapshotsNumber > 0 && !createError)
                tabsList.Add(CreateTab("vps_snapshots", "Tab.Snapshots"));

            
            if ((vm.ExternalNetworkEnabled || vm.PrivateNetworkEnabled) && !createError)
                tabsList.Add(CreateTab("vps_network", "Tab.Network"));
            

            /*
            if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.PROXMOX_REPLICATION_ENABLED))
            {
                tabsList.Add(CreateTab("vps_replication", "Tab.Replication"));
            }
            */

            //tabsList.Add(CreateTab("vps_permissions", "Tab.Permissions"));
            //tabsList.Add(CreateTab("vps_tools", "Tab.Tools"));
            tabsList.Add(CreateTab("vps_audit_log", "Tab.AuditLog"));

            //if (!createError)
            //    tabsList.Add(CreateTab("vps_help", "Tab.Help"));


            // find selected menu item
            int idx = 0;
            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                    break;
                idx++;
            }
            dlTabs.SelectedIndex = idx;

            dlTabs.DataSource = tabsList;
            dlTabs.DataBind();

            // show provision error message
            if(createError && idx == 0)
                messageBox.ShowErrorMessage("VPS_PROVISION_ERROR");
        }
        
        private void BindTask(VirtualMachine vm)
        {
            task = ES.Services.Tasks.GetTaskWithLogRecords(vm.CurrentTaskId, DateTime.MinValue);
            if (task == null)
                return;

            // bind task details
            litTaskName.Text = String.Format("{0} &quot;{1}&quot;",
                GetAuditLogTaskName(task.Source, task.TaskName),
                task.ItemName);

            // time
            litStarted.Text = task.StartDate.ToString("T");
            TimeSpan d = (TimeSpan)(DateTime.Now - task.StartDate);
            litElapsed.Text = new TimeSpan(d.Hours, d.Minutes, d.Seconds).ToString();

            // bind records
            repRecords.DataSource = task.GetLogs();
            repRecords.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id, GetLocalizedString(text),
                HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void repRecords_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            BackgroundTaskLogRecord record = (BackgroundTaskLogRecord)e.Item.DataItem;

            Literal litRecord = (Literal)e.Item.FindControl("litRecord");
            Gauge gauge = (Gauge)e.Item.FindControl("gauge");

            if (litRecord != null)
            {
                string text = record.Text;

                // localize text
                string locText = GetSharedLocalizedString("TaskActivity." + text);
                if (locText != null)
                    text = locText;

                // format parameters
                if (record.TextParameters != null
                    && record.TextParameters.Length > 0
                    && record.Severity == 0)
                    text = String.Format(text, record.TextParameters);

                litRecord.Text = text;

                // gauge
                gauge.Visible = false;
                if (e.Item.ItemIndex == task.GetLogs().Count - 1)
                {
                    if (task.IndicatorCurrent == -1)
                        litRecord.Text += "...";
                    else
                    {
                        gauge.Visible = true;
                        gauge.Total = task.IndicatorMaximum;
                        gauge.Progress = task.IndicatorCurrent;
                    }
                }
            }
        }
    }
}
