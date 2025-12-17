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

using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeStorageUsageBreakdown : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStatistics();
            }

        }

        private void BindStatistics()
        {
            // total counters
            int totalMailboxItems = 0;
            int totalMailboxesSizeMB = 0;

            // mailboxes
            ExchangeItemStatistics[] mailboxes = ES.Services.ExchangeServer.GetMailboxesStatistics(PanelRequest.ItemID);
            gvMailboxes.DataSource = mailboxes;
            gvMailboxes.DataBind();

            foreach (ExchangeItemStatistics item in mailboxes)
            {
                totalMailboxItems += item.TotalItems;
                totalMailboxesSizeMB += item.TotalSizeMB;
            }

            OrganizationStatistics stats = ES.Services.ExchangeServer.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

            lblTotalMailboxItems.Text = totalMailboxItems.ToString();
            lblTotalMailboxSize.Text = totalMailboxesSizeMB.ToString();
            lblTotalMailboxes.Text = stats.CreatedMailboxes.ToString();
            int avgSize = totalMailboxesSizeMB / stats.CreatedMailboxes;
            lblAverageMailboxSize.Text = avgSize.ToString("N2");
        }
    }
}
