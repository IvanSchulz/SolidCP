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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class OrganizationSettingsGeneralSettings : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
                litOrganizationName.Text = org.OrganizationId;

                BindSettings();
            }

        }

        private void BindSettings()
        {
            var settings = ES.Services.Organizations.GetOrganizationGeneralSettings(PanelRequest.ItemID);

            if (settings != null)
            {
                txtOrganizationLogoUrl.Text = settings.OrganizationLogoUrl;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            SaveGeneralSettings(GetSettings());
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveGeneralSettings(GetSettings()))
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "organization_home",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
        }

        private OrganizationGeneralSettings GetSettings()
        {
            var settings = new OrganizationGeneralSettings
            {
                OrganizationLogoUrl = txtOrganizationLogoUrl.Text
            };

            return settings;
        }


        private bool SaveGeneralSettings(OrganizationGeneralSettings settings)
        {
            try
            {
                ES.Services.Organizations.UpdateOrganizationGeneralSettings(PanelRequest.ItemID, GetSettings());
            }
            catch (Exception ex)
            {
                ShowErrorMessage("ORANIZATIONSETTINGS_NOT_UPDATED", ex);
                return false;
            }

            return true;
        }
    }
}
