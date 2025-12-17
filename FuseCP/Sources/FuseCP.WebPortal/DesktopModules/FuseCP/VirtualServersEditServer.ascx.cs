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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class VirtualServersEditServer : FuseCPModuleBase
    {
        ServerInfo server = null;
        DataSet dsServices = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindServer();
                    BindServices();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("VSERVER_GET_SERVER", ex);
                    return;
                }
            }

            // toggle controls
            ToggleGroupControls();
        }

        private void BindServer()
        {
            server = ES.Services.Servers.GetServerById(PanelRequest.ServerId);

            if (server == null)
                RedirectToBrowsePage();

            // header
            txtName.Text = PortalAntiXSS.DecodeOld(server.ServerName);
            txtComments.Text = PortalAntiXSS.DecodeOld(server.Comments);

            Utils.SelectListItem(ddlPrimaryGroup, server.PrimaryGroupId);

            // Preview Domain
            txtPreviewDomain.Text = server.InstantDomainAlias;
        }

        private void BindServices()
        {
            // load services
            dsServices = ES.Services.Servers.GetVirtualServices(PanelRequest.ServerId);

            // bind primary groups
            ddlPrimaryGroup.Items.Clear();
            ddlPrimaryGroup.Items.Add(new ListItem("<Select Group>", ""));
            DataView dvGroups = dsServices.Tables[0].DefaultView;
            foreach (DataRowView dr in dvGroups)
            {
                int groupId = (int)dr["GroupID"];
                DataView dvServices = GetGroupServices(groupId);

                if (dvServices.Count > 1)
                {
                    ddlPrimaryGroup.Items.Add(new ListItem(dr["GroupName"].ToString(), groupId.ToString()));
                }
            }

            // select primary group
            if (server == null)
                server = ES.Services.Servers.GetServerById(PanelRequest.ServerId);

            bool showBindToPrimary = (ddlPrimaryGroup.Items.Count > 2);
            ddlPrimaryGroup.SelectedIndex = -1;

            if (showBindToPrimary)
                Utils.SelectListItem(ddlPrimaryGroup, server.PrimaryGroupId);
            rowPrimaryGroup.Visible = showBindToPrimary;

            // bind services
            try
            {
                dlServiceGroups.DataSource = dsServices.Tables[0];
                dlServiceGroups.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("INIT_SERVICE_ITEM_FORM", ex);
            }
        }

        private void ToggleGroupControls()
        {
            int primaryGroupId = Utils.ParseInt(ddlPrimaryGroup.SelectedValue, 0);
            for (int i = 0; i < dlServiceGroups.Items.Count; i++)
            {
                int groupId = (int)dlServiceGroups.DataKeys[i];
                DataListItem item = dlServiceGroups.Items[i];

                Control rowBound = item.FindControl("rowBound");
                rowBound.Visible = (groupId != primaryGroupId
                    && ddlPrimaryGroup.Items.Count > 2
                    && ddlPrimaryGroup.SelectedIndex > 0);

                CheckBox chkBind = (CheckBox)item.FindControl("chkBind");

                Control rowDistType = item.FindControl("rowDistType");
                rowDistType.Visible = (!rowBound.Visible || !chkBind.Checked);
            }
        }

        public DataView GetGroupServices(int groupId)
        {
            return new DataView(dsServices.Tables[1], "GroupID=" + groupId.ToString(), "", DataViewRowState.CurrentRows);
        }

        protected void dlServiceGroups_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item == null)
                return;

            int groupId = (int)dlServiceGroups.DataKeys[e.Item.ItemIndex];

            DataView dvServices = GetGroupServices(groupId);

            // find distribution table
            Control tblDistr = e.Item.FindControl("tblGroupDistribution");
            tblDistr.Visible = (dvServices.Count > 1);
        }

        private void UpdateServer()
        {
            if (!Page.IsValid)
                return;

            ServerInfo server = new ServerInfo();

            // header
            server.ServerId = PanelRequest.ServerId;
            server.ServerName = txtName.Text;
            server.Comments = txtComments.Text;
            server.PrimaryGroupId = Utils.ParseInt(ddlPrimaryGroup.SelectedValue, 0);

            // Preview Domain
            server.InstantDomainAlias = txtPreviewDomain.Text;

            // gather groups info
            List<VirtualGroupInfo> groups = new List<VirtualGroupInfo>();
            for (int i = 0; i < dlServiceGroups.Items.Count; i++)
            {
                int groupId = (int)dlServiceGroups.DataKeys[i];
                DataListItem item = dlServiceGroups.Items[i];

                CheckBox chkBind = (CheckBox)item.FindControl("chkBind");
                DropDownList ddlDistType = (DropDownList)item.FindControl("ddlDistType");
				Control rowBound = item.FindControl("rowBound");

                VirtualGroupInfo group = new VirtualGroupInfo();
                group.GroupId = groupId;
                group.DistributionType = Utils.ParseInt(ddlDistType.SelectedValue, 0);
				group.BindDistributionToPrimary = chkBind.Checked && rowBound.Visible;
                groups.Add(group);
            }

            try
            {
                // update server
                int result = ES.Services.Servers.UpdateServer(server);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // update groups
                result = ES.Services.Servers.UpdateVirtualGroups(PanelRequest.ServerId, groups.ToArray());
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VSERVER_UPDATE_SERVER", ex);
                return;
            }

            // return to browse page
            RedirectToBrowsePage();
        }

        private void DeleteServer()
        {
            try
            {
                int result = ES.Services.Servers.DeleteServer(PanelRequest.ServerId);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VSERVER_DELETE_SERVER", ex);
                return;
            }

            RedirectToBrowsePage();
        }

        private void RemoveServices()
        {
            // iterate through all services
            List<int> ids = new List<int>();

            foreach (DataListItem itemGroup in dlServiceGroups.Items)
            {
                DataList dlServices = (DataList)itemGroup.FindControl("dlServices");
                if (dlServices != null)
                {
                    for (int i = 0; i < dlServices.Items.Count; i++)
                    {
                        DataListItem itemService = dlServices.Items[i];
                        CheckBox chkSelected = (CheckBox)itemService.FindControl("chkSelected");
                        if (chkSelected != null && chkSelected.Checked)
                        {
                            int serviceId = (int)dlServices.DataKeys[i];
                            ids.Add(serviceId);
                        }
                    }
                }
            }

            // remove virtual services
            try
            {
                int result = ES.Services.Servers.DeleteVirtualServices(PanelRequest.ServerId, ids.ToArray());
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VSERVER_REMOVE_SERVICES", ex);
                return;
            }

            // rebind
            BindServices();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateServer();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectToBrowsePage();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteServer();
        }
        protected void btnAddServices_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "add_services"));
        }

        protected void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            RemoveServices();
        }
    }
}
