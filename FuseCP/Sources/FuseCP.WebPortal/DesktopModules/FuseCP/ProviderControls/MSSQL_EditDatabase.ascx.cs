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
using FuseCP.Providers.Database;

namespace FuseCP.Portal.ProviderControls
{
    public partial class MSSQL_EditDatabase : FuseCPControlBase, IDatabaseEditDatabaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["ControlsToggled"] == null)
            {
                bool editMode = (PanelRequest.ItemID > 0);
                btnBackup.Enabled = editMode;
                btnRestore.Enabled = editMode;
                btnTruncate.Enabled = editMode;
            }
        }

        public void BindItem(SqlDatabase item)
        {
            litDataName.Text = item.DataName;
            litDataSize.Text = item.DataSize.ToString();

            litLogName.Text = item.LogName;
            litLogSize.Text = item.LogSize.ToString();

            // enable/disable controls according to hosting context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(item.PackageId);
            string backupQuotaName = item.GroupName + ".Backup";
            string restoreQuotaName = item.GroupName + ".Restore";
            string truncateQuotaName = item.GroupName + ".Truncate";
            btnBackup.Enabled = cntx.Quotas.ContainsKey(backupQuotaName) && !cntx.Quotas[backupQuotaName].QuotaExhausted;
            btnRestore.Enabled = cntx.Quotas.ContainsKey(restoreQuotaName) && !cntx.Quotas[restoreQuotaName].QuotaExhausted;
            btnTruncate.Enabled = cntx.Quotas.ContainsKey(truncateQuotaName) && !cntx.Quotas[truncateQuotaName].QuotaExhausted;

            ViewState["ControlsToggled"] = true;
        }

        public void SaveItem(SqlDatabase item)
        {
        }

        protected void btnTruncate_Click(object sender, EventArgs e)
        {
            // truncate
            try
            {
                int result = ES.Services.DatabaseServers.TruncateSqlDatabase(PanelRequest.ItemID);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("SQL_TRUNCATE_DATABASE", ex);
                return;
            }

            HostModule.ShowSuccessMessage("SQL_TRUNCATE_DATABASE");
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), "backup",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), "restore",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
        }
    }
}
