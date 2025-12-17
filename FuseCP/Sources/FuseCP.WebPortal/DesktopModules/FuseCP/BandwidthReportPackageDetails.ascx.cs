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
    public partial class BandwidthReportPackageDetails : FuseCPModuleBase
    {
        private int bandwidthTotal;
        public int BandwidthTotal
        {
            get { return this.bandwidthTotal; }
            set { this.bandwidthTotal = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSummary();
            }
        }

        private void BindSummary()
        {
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package == null)
                RedirectToBrowsePage();

            DateTime startDate = new DateTime(Int64.Parse(Request["StartDate"]));
            DateTime endDate = new DateTime(Int64.Parse(Request["EndDate"]));

            litPeriod.Text = startDate.ToString("MMM dd, yyyy") +
                " - " + endDate.ToString("MMM dd, yyyy");

            // get summary
            DataSet ds = ES.Services.Packages.GetPackageBandwidth(PanelSecurity.PackageId, startDate, endDate);

            // calculate total
            foreach (DataRow dr in ds.Tables[0].Rows)
                BandwidthTotal += Convert.ToInt32(dr["MegaBytesTotal"]);

            litTotal.Text = BandwidthTotal.ToString();

            // bind summary
            gvSummary.DataSource = ds;
            gvSummary.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
    }
}
