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
    public partial class SpaceQuotasControl : FuseCPControlBase
    {
        DataSet dsQuotas = null;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindQuotas(int packageId)
        {
            try
            {
                dsQuotas = ES.Services.Packages.GetPackageQuotas(packageId);
                dsQuotas.Tables[1].Columns.Add("QuotaAvailable", typeof(int));
                foreach (DataRow r in dsQuotas.Tables[1].Rows) r["QuotaAvailable"] = -1;

                dlGroups.DataSource = dsQuotas.Tables[0];
                dlGroups.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public bool IsGroupVisible(int groupId)
        {
            return new DataView(dsQuotas.Tables[1], "GroupID=" + groupId.ToString(), "", DataViewRowState.CurrentRows).Count > 0;
        }

        public DataView GetGroupQuotas(int groupId)
        {
            return new DataView(dsQuotas.Tables[1], "GroupID=" + groupId.ToString(), "", DataViewRowState.CurrentRows);
        }

        public string GetQuotaTitle(string quotaName, object quotaDescription)
        {
            string description = (quotaDescription.GetType() == typeof(System.DBNull)) ? string.Empty : (string)quotaDescription;

            return quotaName.Contains("ServiceLevel") ? description
                                                      : GetSharedLocalizedString("Quota." + quotaName);
        }
    }
}
