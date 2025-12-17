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
using System.Web;
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeStorageUsage : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind quotas
                BindQuotas();


            }

        }

        private void BindQuotas()
        {
            OrganizationStatistics stats = ES.Services.ExchangeServer.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

            btnUsedSize.Text = (stats.UsedDiskSpace < 0) ? GetLocalizedString("Unlimited.Text") : stats.UsedDiskSpace.ToString();
        }

        protected void btnRecalculate_Click(object sender, EventArgs e)
        {
            ES.Services.ExchangeServer.CalculateOrganizationDiskspace(PanelRequest.ItemID);
        }

        protected void btnUsedSize_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "storage_usage_details",
                    "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
