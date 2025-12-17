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
    public partial class Quota : FuseCPControlBase
    {
        public string QuotaName
        {
            get { return (ViewState["QuotaName"] != null) ? (string)ViewState["QuotaName"] : ""; }
            set { ViewState["QuotaName"] = value; }
        }

        public bool DisplayGauge
        {
            get { return quotaViewer.DisplayGauge; }
            set { quotaViewer.DisplayGauge = value; }
        }

        public int QuotaAllocatedValue
        {
            set { quotaViewer.QuotaValue = value; }
        }

        public QuotaViewer Viewer
        {
            get { return quotaViewer; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindQuota();
            }
        }

        public void BindQuota()
        {
            try
            {
                // load package context
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                // get quota
                if (cntx.Quotas.ContainsKey(QuotaName))
                {
                    QuotaValueInfo quota = cntx.Quotas[QuotaName];
                    quotaViewer.QuotaTypeId = quota.QuotaTypeId;
                    quotaViewer.QuotaUsedValue = quota.QuotaUsedValue;
                    quotaViewer.QuotaValue = quota.QuotaAllocatedValue;
                    quotaViewer.QuotaAvailable = -1;
                	//this.Visible = quota.QuotaAllocatedValue != 0;
                }
                else
                {
                	this.Visible = false;
                    quotaViewer.QuotaTypeId = 1; // bool
                    quotaViewer.QuotaUsedValue = 0;
                    quotaViewer.QuotaValue = 0;
                    quotaViewer.QuotaAvailable = -1;
                }
            }
            catch
            {
                /* do nothing */
            }
        }
    }
}
