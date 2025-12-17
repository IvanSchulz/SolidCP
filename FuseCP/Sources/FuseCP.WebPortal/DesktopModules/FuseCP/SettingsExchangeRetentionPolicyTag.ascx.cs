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
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal
{
    public partial class SettingsExchangeRetentionPolicyTag : FuseCPControlBase, IUserSettingsEditorControl
    {


        public void BindSettings(UserSettings settings)
        {
            BindRetentionPolicy();


            string[] types = Enum.GetNames(typeof(ExchangeRetentionPolicyTagType));

            ddTagType.Items.Clear();
            for (int i = 0; i < types.Length; i++)
            {
                string name = GetSharedLocalizedString("Text." +types[i]);
                ddTagType.Items.Add(new ListItem(name, i.ToString()));
            }

            string[] action = Enum.GetNames(typeof(ExchangeRetentionPolicyTagAction));

            ddRetentionAction.Items.Clear();
            for (int i = 0; i < action.Length; i++)
            {
                string name = GetSharedLocalizedString("Text."+action[i]);
                ddRetentionAction.Items.Add(new ListItem(name, i.ToString()));
            }

            txtStatus.Visible = false;
        }


        private void BindRetentionPolicy()
        {
            Providers.HostedSolution.Organization[] orgs = null;

            if (PanelSecurity.SelectedUserId != 1)
            {
                PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                if ((Packages != null) & (Packages.GetLength(0) > 0))
                {
                    orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                }
            }
            else
            {
                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
            }

            if ((orgs != null) & (orgs.GetLength(0) > 0))
            {
                ExchangeRetentionPolicyTag[] list = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTags(orgs[0].Id);

                gvPolicy.DataSource = list;
                gvPolicy.DataBind();
            }

            btnUpdatePolicy.Enabled = (string.IsNullOrEmpty(txtPolicy.Text)) ? false : true;
        }


        public void btnAddPolicy_Click(object sender, EventArgs e)
        {
            Page.Validate("CreatePolicy");

            if (!Page.IsValid)
                return;

            ExchangeRetentionPolicyTag tag = new ExchangeRetentionPolicyTag();
            tag.TagName = txtPolicy.Text;
            tag.TagType = Convert.ToInt32(ddTagType.SelectedValue);
            tag.AgeLimitForRetention = ageLimitForRetention.QuotaValue;
            tag.RetentionAction = Convert.ToInt32(ddRetentionAction.SelectedValue);

            Providers.HostedSolution.Organization[] orgs = null;
            if (PanelSecurity.SelectedUserId != 1)
            {
                PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                if ((Packages != null) & (Packages.GetLength(0) > 0))
                {
                    orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                }
            }
            else
            {
                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
            }


            if ((orgs != null) & (orgs.GetLength(0) > 0))
            {
                IntResult result = ES.Services.ExchangeServer.AddExchangeRetentionPolicyTag(orgs[0].Id, tag);

                if (!result.IsSuccess)
                {
                    messageBox.ShowMessage(result, "EXCHANGE_UPDATEPLANS", null);
                    return;
                }
                else
                {
                    messageBox.ShowSuccessMessage("EXCHANGE_UPDATEPLANS");
                }
            }

            BindRetentionPolicy();

        }

        protected void gvPolicy_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int mailboxPlanId = Utils.ParseInt(e.CommandArgument.ToString(), 0);
            Providers.HostedSolution.Organization[] orgs = null;
            Providers.HostedSolution.ExchangeRetentionPolicyTag tag;

            switch (e.CommandName)
            {
                case "DeleteItem":
                    try
                    {

                        if (PanelSecurity.SelectedUserId != 1)
                        {
                            PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                            if ((Packages != null) & (Packages.GetLength(0) > 0))
                            {
                                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                            }
                        }
                        else
                        {
                            orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
                        }

                        tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(orgs[0].Id, mailboxPlanId);

                        if (tag.ItemID != orgs[0].Id)
                        {
                            messageBox.ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                            BindRetentionPolicy();
                            return;
                        }


                        ResultObject result = ES.Services.ExchangeServer.DeleteExchangeRetentionPolicyTag(orgs[0].Id, mailboxPlanId);
                        if (!result.IsSuccess)
                        {
                            messageBox.ShowMessage(result, "EXCHANGE_DELETE_RETENTIONPOLICY", null);
                            return;
                        }
                        else
                        {
                            messageBox.ShowSuccessMessage("EXCHANGE_DELETE_RETENTIONPOLICY");
                        }

                        ViewState["PolicyID"] = null;

                        txtPolicy.Text = string.Empty;
                        ageLimitForRetention.QuotaValue = 0;

                        btnUpdatePolicy.Enabled = (string.IsNullOrEmpty(txtPolicy.Text)) ? false : true;

                    }
                    catch (Exception)
                    {
                        messageBox.ShowErrorMessage("EXCHANGE_DELETE_RETENTIONPOLICY");
                    }

                    BindRetentionPolicy();
                break;

                case "EditItem":
                        ViewState["PolicyID"] = mailboxPlanId;

                        if (PanelSecurity.SelectedUserId != 1)
                        {
                            PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                            if ((Packages != null) & (Packages.GetLength(0) > 0))
                            {
                                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                            }
                        }
                        else
                        {
                            orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
                        }


                        tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(orgs[0].Id, mailboxPlanId);

                        txtPolicy.Text = tag.TagName;
                        Utils.SelectListItem(ddTagType, tag.TagType);
                        ageLimitForRetention.QuotaValue = tag.AgeLimitForRetention;
                        Utils.SelectListItem(ddRetentionAction, tag.RetentionAction);

                        btnUpdatePolicy.Enabled = (string.IsNullOrEmpty(txtPolicy.Text)) ? false : true;

                    break;
            }
        }


        public void SaveSettings(UserSettings settings)
        {
            settings["PolicyID"] = "";
        }


        protected void btnUpdatePolicy_Click(object sender, EventArgs e)
        {
            Page.Validate("CreatePolicy");

            if (!Page.IsValid)
                return;

            if (ViewState["PolicyID"] == null)
                return;

            int mailboxPlanId = (int)ViewState["PolicyID"];
            Providers.HostedSolution.Organization[] orgs = null;
            Providers.HostedSolution.ExchangeRetentionPolicyTag tag;


            if (PanelSecurity.SelectedUserId != 1)
            {
                PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                if ((Packages != null) & (Packages.GetLength(0) > 0))
                {
                    orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                }
            }
            else
            {
                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
            }

            tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(orgs[0].Id, mailboxPlanId);

            if (tag.ItemID != orgs[0].Id)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                BindRetentionPolicy();
                return;
            }


            tag.TagName = txtPolicy.Text;
            tag.TagType = Convert.ToInt32(ddTagType.SelectedValue);
            tag.AgeLimitForRetention = ageLimitForRetention.QuotaValue;
            tag.RetentionAction = Convert.ToInt32(ddRetentionAction.SelectedValue);

            if ((orgs != null) & (orgs.GetLength(0) > 0))
            {
                ResultObject result = ES.Services.ExchangeServer.UpdateExchangeRetentionPolicyTag(orgs[0].Id, tag);

                if (!result.IsSuccess)
                {
                    messageBox.ShowMessage(result, "EXCHANGE_UPDATEPLANS", null);
                }
                else
                {
                    messageBox.ShowSuccessMessage("EXCHANGE_UPDATEPLANS");
                }
            }

            BindRetentionPolicy();
        }

    }
}
