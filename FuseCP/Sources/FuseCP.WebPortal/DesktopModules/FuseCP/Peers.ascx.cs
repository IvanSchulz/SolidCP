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

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class Peers : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // set display preferences
            usersList.PageSize = UsersHelper.GetDisplayItemsPerPage();
        }

        protected void odsUserPeers_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "edit_peer"));
        }

        protected string GetRoleName(int roleID)
        {
            switch (roleID)
            {
                case 1:
                    return "Administrator";
                case 2:
                    return "Reseller";
                case 3:
                    return "User";
                case 4:
                    return "CSR";
                case 5:
                    return "CSR";
                case 6:
                    return "Helpdesk";
                case 7:
                    return "Helpdesk";
                default:
                    return "Unknown";
            }
        }


        protected string GetStateImage(object status)
        {
            string imgName = "enabled.png";

            if (status != null)
            {
                try
                {
                    switch ((int)status)
                    {
                        case (int)UserLoginStatus.Disabled:
                            imgName = "disabled.png";
                            break;
                        case (int)UserLoginStatus.LockedOut:
                            imgName = "locked.png";
                            break;
                        default:
                            imgName = "enabled.png";
                            break;
                    }
                }
                catch (Exception) { }
            }

            return GetThemedImage("Exchange/" + imgName);
        }

    }
}
