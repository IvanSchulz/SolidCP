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

namespace FuseCP.Portal.RDS
{
    public partial class RDSImportCollection : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                //TODO correct value for parameter rdsControllerServiceID
                throw new NotImplementedException("This feature has to be corrected in the code.");
                ES.Services.RDS.ImportCollection(PanelRequest.ItemID, txtCollectionName.Text, "");
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSCOLLECTION_NOT_IMPORTED", ex);
            }            
        }
    }
}
