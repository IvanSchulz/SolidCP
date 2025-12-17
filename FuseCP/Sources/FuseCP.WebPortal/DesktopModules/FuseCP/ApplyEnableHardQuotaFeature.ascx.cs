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
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.Scheduling;
using FuseCP.Portal.Code.Framework;

namespace FuseCP.Portal
{
    public partial class ApplyEnableHardQuotaFeature : FuseCPModuleBase
    {

        public int PackageId
        {
            get { return PanelSecurity.PackageId; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            messageBox.ShowWarningMessage("ApplyEnableHardQuotaFeature");
        }
        
        private void Update()
        {
            try
            {
                ES.Services.Files.ApplyEnableHardQuotaFeature(PackageId);
                // redirect               
                Response.Redirect(NavigatePageURL("SpaceHome", "SpaceID", PackageId.ToString()));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("APPLY_QUOTA", ex);
            }
        }
              

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
			Update();
        }
       
    }
}
