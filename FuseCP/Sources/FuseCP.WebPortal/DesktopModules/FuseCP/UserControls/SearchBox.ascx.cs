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

namespace FuseCP.Portal
{
    public partial class SearchBox : FuseCPControlBase
    {
        public string AjaxData { get; set; }

        public string FilterColumn
        {
            get
            {
                return ddlFilterColumn.SelectedValue;
            }
            set
            {
                ddlFilterColumn.SelectedIndex = -1;
                ListItem li = ddlFilterColumn.Items.FindByValue(value);
                if (li != null)
                    li.Selected = true;
            }
        }

        public string FilterValue
        {
            get
            {
                string val = tbSearchText.Text.Trim();
                string valText = tbSearch.Text.Trim();
                if (valText.Length == 0)
                    val = valText;
                if (val.Length == 0)
                    val = tbSearch.Text.Trim();
                val = val.Replace("%", "");
                return "%" + val + "%";
            }
            set
            {
                if (value != null)
                {
                    value = value.Replace("%", "");
                    tbSearch.Text = value;
                    tbSearchText.Text = value;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void AddCriteria(string columnName, string columnTitle)
        {
            ddlFilterColumn.Items.Add(new ListItem(columnTitle, columnName));
        }

        public string GetCriterias()
        {
            string res = null;
            foreach (ListItem itm in ddlFilterColumn.Items)
            {
                if (res != null)
                    res += ", ";
                res = res + "'" + itm.Value + "'";
            }
            return res;
        }

        public override void Focus()
        {
            base.Focus();
            tbSearch.Focus();
        }

        protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (tbObjectId.Text.Length > 0)
            {
                Response.Redirect(PortalUtils.GetUserHomePageUrl(Int32.Parse(tbObjectId.Text)));
            }
            else
            {
                String strText = tbSearchText.Text;
                if (strText.Length > 0)
                {
                    Response.Redirect(NavigatePageURL(PortalUtils.GetUserCustomersPageId(),
                        PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                        "FilterColumn=" + ddlFilterColumn.SelectedValue,
                        "FilterValue=" + Server.UrlEncode(FilterValue)));
                }
                else
                {
                    Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetObjectSearchPageId(),
                        PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                        "Query=" + Server.UrlEncode(tbSearch.Text),"FullType=Users"));
                }
            }
        }
    }
}
