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

using FuseCP.Providers.Statistics;

namespace FuseCP.Portal
{
    public partial class StatisticsEditStatistics : FuseCPModuleBase
    {
        StatsSite item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);
            ddlWebSites.Visible = (PanelRequest.ItemID == 0);
            valRequireWebSite.Enabled = (PanelRequest.ItemID == 0);
            btnUpdate.Text = (PanelRequest.ItemID > 0) ? GetLocalizedString("Text.Update") : GetLocalizedString("Text.Add");

            // bind item
            BindItem();
        }

        private void BindItem()
        {
            try
            {
                if (!IsPostBack)
                {
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        item = ES.Services.StatisticsServers.GetSite(PanelRequest.ItemID);
                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        BindWebSites(PanelSecurity.PackageId);
                    }
                }

                // load provider control
                LoadProviderControl((int)ViewState["PackageId"], "Statistics", providerControl, "EditSite.ascx");

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        lblDomainName.Text = item.Name;

						if (String.Compare(Request["Mode"], "view", true) == 0
							&& !String.IsNullOrEmpty(item.StatisticsUrl))
						{
							// view mode
							Response.Redirect(item.StatisticsUrl);
						}

                        // other controls
                        IStatsEditInstallationControl ctrl = (IStatsEditInstallationControl)providerControl.Controls[0];
                        ctrl.BindItem(item);
                    }
                }

            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
                return;
            }
        }

        private void BindWebSites(int packageId)
        {
            ddlWebSites.DataSource = ES.Services.WebServers.GetWebSites(packageId, false);
            ddlWebSites.DataBind();
            ddlWebSites.Items.Insert(0, new ListItem("<Select Web Site>", ""));
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            StatsSite item = new StatsSite();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = ddlWebSites.SelectedValue;

            // get other props
            IStatsEditInstallationControl ctrl = (IStatsEditInstallationControl)providerControl.Controls[0];
            ctrl.SaveItem(item);

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.StatisticsServers.AddSite(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("STATS_ADD_STAT", ex);
                    return;
                }
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.StatisticsServers.UpdateSite(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("STATS_UPDATE_STAT", ex);
                    return;
                }
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.StatisticsServers.DeleteSite(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("STATS_DELETE_STAT", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveItem();
        }
    }
}
