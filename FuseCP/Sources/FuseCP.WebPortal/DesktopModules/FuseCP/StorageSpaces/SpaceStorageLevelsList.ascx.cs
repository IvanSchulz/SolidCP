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
using FuseCP.Providers.Common;
using FuseCP.Providers.StorageSpaces;

namespace FuseCP.Portal.StorageSpaces
{
    public partial class SpaceStorageLevelsList : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvSsLevels.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                gvSsLevels.Sort("Name", System.Web.UI.WebControls.SortDirection.Ascending);
            }
        }

        protected void odsRDSServersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        

        protected void gvSsLevels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int id;
                bool hasValue = int.TryParse(e.CommandArgument.ToString(), out id);

                ResultObject result = new ResultObject();
                result.IsSuccess = false;

                if (hasValue)
                {
                    result = ES.Services.StorageSpaces.RemoveStorageSpaceLevel(id);
                }

                messageBox.ShowMessage(result, "STORAGE_SPACES_LEVEL_REMOVE", null);

                if (!result.IsSuccess)
                {
                    return;
                }

                gvSsLevels.DataBind();
            }
            else if (e.CommandName == "EditSsLevel")
            {
                EditSsLevel(e.CommandArgument.ToString());
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSsLevels.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            gvSsLevels.DataBind();
        }

        protected void btnAddSsLevel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SsLevelId", (-1).ToString(), "add_storage_space_level"));
        }

        private void EditSsLevel(string levelId)
        {
            Response.Redirect(EditUrl("SsLevelId", levelId, "edit_storage_space_level"));
        }

        protected bool CheckLevelIsInUse(int levelId)
        {
            return ES.Services.StorageSpaces.GetStorageSpacesByLevelId(levelId).Any();
        }
    }
}
