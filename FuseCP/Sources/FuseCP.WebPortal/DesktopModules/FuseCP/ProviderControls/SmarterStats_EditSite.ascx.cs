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
using CPCC;

using FuseCP.Providers.Statistics;

namespace FuseCP.Portal.ProviderControls
{
    public partial class SmarterStats_EditSite : FuseCPControlBase, IStatsEditInstallationControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && ViewState["binded"] == null)
            {
                // users
                List<StatsUser> users = new List<StatsUser>();
                AddNewUser(users, true, true);

                users[0].Username = "admin";

                // bind users
                gvUsers.DataSource = users;
                gvUsers.DataBind();

                // set site ID
                txtSiteId.Text = GetLocalizedString("Text.Pending");
                litSiteStatus.Text = GetLocalizedString("Text.Unknown");
            }
        }

        public void BindItem(StatsSite item)
        {
            LocalizeGridView(gvUsers);

            if (item == null)
                return;

            txtSiteId.Text = item.SiteId;
            litSiteStatus.Text = item.Status;

            // users
            List<StatsUser> users = new List<StatsUser>();
            users.AddRange(item.Users);

            if (users.Count == 0)
                AddNewUser(users, true, true);

            // bind users
            gvUsers.DataSource = users;
            gvUsers.DataBind();

            ViewState["binded"] = true;
        }

        public void SaveItem(StatsSite item)
        {
            // users
            item.Users = CollectFormData(false).ToArray();
        }

        public List<StatsUser> CollectFormData(bool includeEmpty)
        {
            List<StatsUser> users = new List<StatsUser>();
            foreach (GridViewRow row in gvUsers.Rows)
            {
                CheckBox chkOwner = (CheckBox)row.FindControl("chkOwner");
                CheckBox chkAdmin = (CheckBox)row.FindControl("chkAdmin");
                TextBox txtUsername = (TextBox)row.FindControl("txtUsername");
                TextBox txtPassword = (TextBox)row.FindControl("txtPassword");
                TextBox txtFirstName = (TextBox)row.FindControl("txtFirstName");
                TextBox txtLastName = (TextBox)row.FindControl("txtLastName");

                // create a new HttpError object and add it to the collection
                StatsUser user = new StatsUser();
                user.IsOwner = chkOwner.Checked;
                user.IsAdmin = chkAdmin.Checked;
                user.Username = txtUsername.Text.Trim();
                user.Password = txtPassword.Text.Trim();
                user.FirstName = txtFirstName.Text.Trim();
                user.LastName = txtLastName.Text.Trim();

                if (includeEmpty || user.Username != "")
                    users.Add(user);
            }

            return users;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // collect form data
            List<StatsUser> users = CollectFormData(true);

            // add new user
            AddNewUser(users, false, false);

            // bind users
            gvUsers.DataSource = users;
            gvUsers.DataBind();
        }

        public void AddNewUser(List<StatsUser> users, bool isAdmin, bool isOwner)
        {
            StatsUser user = new StatsUser();
            user.IsAdmin = isAdmin;
            user.IsOwner = isOwner;
            user.Username = "";
            user.Password = "";
            user.FirstName = "";
            user.LastName = "";
            users.Add(user);
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_item")
            {
                List<StatsUser> users = CollectFormData(true);

                // remove error
                users.RemoveAt(Utils.ParseInt((string)e.CommandArgument, 0));

                // bind users
                gvUsers.DataSource = users;
                gvUsers.DataBind();
            }
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            StyleButton cmdDelete = (StyleButton)e.Row.FindControl("cmdDelete");
            CheckBox chkAdmin = (CheckBox)e.Row.FindControl("chkAdmin");

            if (cmdDelete != null)
                cmdDelete.CommandArgument = e.Row.RowIndex.ToString();

            StatsUser user = (StatsUser)e.Row.DataItem;
            if (user != null && user.IsOwner)
            {
                cmdDelete.Visible = false;
                chkAdmin.Enabled = false;
            }
        }
    }
}
