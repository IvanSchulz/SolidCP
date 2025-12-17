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
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class DiskspaceReportPackageDetails : FuseCPModuleBase
    {
        private long diskspaceTotal;
        public long DiskspaceTotal
        {
            get { return this.diskspaceTotal; }
            set { this.diskspaceTotal = value; }
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
            DataSet ds = ES.Services.Packages.GetPackageDiskspace(PanelSecurity.PackageId);

            long negativeDiskspace = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                long diskspace = Convert.ToInt64(dr["Diskspace"]);
                DiskspaceTotal += diskspace;

                if (diskspace < 0)
                    negativeDiskspace += diskspace;
            }

            // subtract negative from "Files" and remove negative groups
            DataRowCollection rows = ds.Tables[0].Rows;
            int i = 0;
            while (i < rows.Count)
            {
                int groupId = (int)rows[i]["GroupID"];
                long diskspace = Convert.ToInt64(rows[i]["Diskspace"]);
                long diskspaceBytes = Convert.ToInt64(rows[i]["DiskspaceBytes"]);
                if (groupId == 1)
                {
                    rows[i]["Diskspace"] = diskspace + negativeDiskspace;
                }
                else if (diskspaceBytes < 0)
                {
                    rows.RemoveAt(i);
                    continue;
                }
                i++;
            }

            litTotal.Text = PortalAntiXSS.Encode(DiskspaceTotal.ToString());

            // get summary
            gvSummary.DataSource = ds;
            gvSummary.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectToBrowsePage();
        }
    }
}
