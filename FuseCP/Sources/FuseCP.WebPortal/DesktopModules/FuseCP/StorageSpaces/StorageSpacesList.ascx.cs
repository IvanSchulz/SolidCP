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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;

namespace FuseCP.Portal.StorageSpaces
{
    public partial class StorageSpacesList : FuseCPModuleBase
    {
        /// private List<ServerInfo> _servers; -- compiler indicates this is never referenced

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvStorageSpaces.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                gvStorageSpaces.Sort("Name", System.Web.UI.WebControls.SortDirection.Ascending);
            }
        }

        protected string GetServiceName(int serviceId)
        {

            var service = ES.Services.Servers.GetServiceInfo(serviceId);

            if (service == null)
            {
                return string.Empty;
            }
            else
            {
                return service.ServiceName;
            }
        }

        public decimal ConvertBytesToGB(object size)
        {
            return Math.Round(Convert.ToDecimal(size) / (1024*1024*1024), 2);
        }

        protected void odsStorageSpacesPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void gvStorageSpaces_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int id;
                bool hasValue = int.TryParse(e.CommandArgument.ToString(), out id);

                ResultObject result = new ResultObject();
                result.IsSuccess = false;

                if (hasValue)
                {
                    result = ES.Services.StorageSpaces.RemoveStorageSpace(id);
                }

                messageBox.ShowMessage(result, "STORAGE_SPACES_LEVEL_REMOVE", null);

                if (!result.IsSuccess)
                {
                    return;
                }

                gvStorageSpaces.DataBind();
            }
            else if (e.CommandName == "EditStorageSpace")
            {
                EditStorageSpace(e.CommandArgument.ToString());
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStorageSpaces.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            gvStorageSpaces.DataBind();
        }

        protected void btnAddStoragSpace_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("StorageSpaceId", "-1", "add_storage_space"));
        }

        private void EditStorageSpace(string ssid)
        {
            Response.Redirect(EditUrl("StorageSpaceId", ssid, "edit_storage_space"));
        }

        protected bool CheckStorageIsInUse(int storageId)
        {
            return ES.Services.StorageSpaces.GetStorageSpaceFoldersByStorageSpaceId(storageId).Any();
        }
    }
}
