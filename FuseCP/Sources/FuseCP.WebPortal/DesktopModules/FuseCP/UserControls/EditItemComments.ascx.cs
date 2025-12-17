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
    public partial class EditItemComments : FuseCPControlBase
    {
        public int ItemId
        {
            get
            {
                // get item id from view state
                int itemId = (ViewState["ItemId"] != null) ? (int)ViewState["ItemId"] : -1;
                if (itemId == -1)
                {
                    // lookup in the request
                    if (RequestItemId != null)
                        itemId = Utils.ParseInt(Request[RequestItemId], -1);
                }

                return itemId;
            }
            set { ViewState["ItemId"] = value; }
        }

        public string ItemTypeId
        {
            get { return (ViewState["ItemTypeId"] != null) ? (string)ViewState["ItemTypeId"] : ""; }
            set { ViewState["ItemTypeId"] = value; }
        }

        public string RequestItemId
        {
            get { return (ViewState["RequestItemId"] != null) ? (string)ViewState["RequestItemId"] : null; }
            set { ViewState["RequestItemId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComments();

                btnAdd.Enabled = (ItemId > 0);
                AddCommentPanel.Visible = (ItemId > 0);
            }
        }

        private void BindComments()
        {
            try
            {
                gvComments.DataSource = ES.Services.Comments.GetComments(PanelSecurity.EffectiveUserId, ItemTypeId, ItemId);
                gvComments.DataBind();
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("COMMENT_GET", ex);
                return;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtComments.Text.Trim() == "")
                return;

            try
            {
                int result = ES.Services.Comments.AddComment(ItemTypeId, ItemId, txtComments.Text, 2);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("COMMENT_ADD", ex);
                return;
            }

            // clear fields
            txtComments.Text = "";

            // rebind list
            BindComments();
        }

        public string WrapComment(string text)
        {
            return (text != null) ? Server.HtmlEncode(text).Replace("\n", "<br/>") : text;
        }

        protected void gvComments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int commentId = (int)gvComments.DataKeys[e.RowIndex][0];

            // delete comment
            try
            {
                int result = ES.Services.Comments.DeleteComment(commentId);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("COMMENT_DELETE", ex);
                return;
            }

            // rebind list
            BindComments();
        }
    }
}
