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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace FuseCP.Portal
{
    public partial class UserLookup : FuseCPControlBase
    {
        public bool AllowEmptySelection
        {
            get { return (ViewState["AllowEmptySelection"] != null) ? (bool)ViewState["AllowEmptySelection"] : true; }
            set { ViewState["AllowEmptySelection"] = value; }
        }

        public int SelectedUserId
        {
            get { return (ViewState["SelectedUserId"] != null) ? (int)ViewState["SelectedUserId"] : 0; }
            set { ViewState["SelectedUserId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.Name"));
                searchBox.AddCriteria("Email", GetLocalizedString("SearchField.Email"));

                // reset user
                user.UserId = SelectedUserId;

                // toggle controls
                ToggleSearchPanel(false);

                // remove menu items
                if (!AllowEmptySelection)
                    RemoveMenuItem("switch_empty");
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        private void RemoveMenuItem(string name)
        {
            MenuItem item = null;
            foreach (MenuItem mnuItem in mnuActions.Items[0].ChildItems)
            {
                if (mnuItem.Value == name)
                {
                    item = mnuItem;
                    break;
                }
            }

            if (item != null)
                mnuActions.Items[0].ChildItems.Remove(item);
        }

        private void ToggleSearchPanel(bool selectMode)
        {
            SelectPanel.Visible = selectMode;
            gvUsers.DataSourceID = selectMode ? "odsUsersPaged" : "";
        }

        protected void mnuPackages_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "switch_logged")
            {
                ToggleSearchPanel(false);
                SelectedUserId = PanelSecurity.EffectiveUserId;
                user.UserId = SelectedUserId;
            }
            else if (e.Item.Value == "switch_workfor")
            {
                ToggleSearchPanel(false);
                SelectedUserId = PanelSecurity.SelectedUserId;
                user.UserId = SelectedUserId;
            }
            else if (e.Item.Value == "switch_empty")
            {
                ToggleSearchPanel(false);
                SelectedUserId = 0;
                user.UserId = SelectedUserId;
            }
            else if (e.Item.Value == "select")
            {
                ToggleSearchPanel(true);
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "select")
            {
                SelectedUserId = Utils.ParseInt(e.CommandArgument.ToString(), PanelSecurity.EffectiveUserId);
                user.UserId = SelectedUserId;
                ToggleSearchPanel(false);
            }
        }

        protected void odsUsersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ToggleSearchPanel(false);
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'Users'");
            res.Append(", UserID: " + PanelSecurity.EffectiveUserId.ToString());
	        res.Append(", StatusID: 0");
            res.Append(", RoleID: 0");
            res.Append(", Recursive: true");
            return res.ToString();
        }
    }
}
