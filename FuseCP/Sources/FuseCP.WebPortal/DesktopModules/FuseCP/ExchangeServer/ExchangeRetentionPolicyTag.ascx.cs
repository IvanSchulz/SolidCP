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

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeRetentionPolicyTag : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                BindRetentionPolicy();

                string[] types = Enum.GetNames(typeof(ExchangeRetentionPolicyTagType));

                ddTagType.Items.Clear();
                for (int i = 0; i < types.Length; i++)
                {
                    string name = GetSharedLocalizedString("Text."+ types[i]);
                    ddTagType.Items.Add(new ListItem(name, i.ToString()));
                }

                string[] action = Enum.GetNames(typeof(ExchangeRetentionPolicyTagAction));

                ddRetentionAction.Items.Clear();
                for (int i = 0; i < action.Length; i++)
                {
                    string name = GetSharedLocalizedString("Text."+action[i]);
                    ddRetentionAction.Items.Add(new ListItem(name, i.ToString()));
                }

                ClearEditValues();
            }

            txtStatus.Visible = false;

        }


        private void BindRetentionPolicy()
        {
            FuseCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[] list = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTags(PanelRequest.ItemID);

            gvPolicy.DataSource = list;
            gvPolicy.DataBind();
        }


        public void btnAddPolicy_Click(object sender, EventArgs e)
        {
            Page.Validate("CreatePolicy");

            if (!Page.IsValid)
                return;

            FuseCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag = new Providers.HostedSolution.ExchangeRetentionPolicyTag();
            tag.TagName = txtPolicy.Text;
            tag.TagType = Convert.ToInt32(ddTagType.SelectedValue);
            tag.AgeLimitForRetention = ageLimitForRetention.QuotaValue;
            tag.RetentionAction = Convert.ToInt32(ddRetentionAction.SelectedValue);

            IntResult result = ES.Services.ExchangeServer.AddExchangeRetentionPolicyTag(PanelRequest.ItemID, tag);

            if (!result.IsSuccess)
            {
                messageBox.ShowMessage(result, "EXCHANGE_UPDATERETENTIONPOLICY", null);
                return;
            }
            else
            {
                messageBox.ShowSuccessMessage("EXCHANGE_UPDATERETENTIONPOLICY");
            }

            BindRetentionPolicy();
            ClearEditValues();
        }

        protected void gvPolicy_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int mailboxPlanId = Utils.ParseInt(e.CommandArgument.ToString(), 0);
            Providers.HostedSolution.ExchangeRetentionPolicyTag tag;

            switch (e.CommandName)
            {
                case "DeleteItem":
                    try
                    {
                        tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(PanelRequest.ItemID, mailboxPlanId);

                        if (tag.ItemID != PanelRequest.ItemID)
                        {
                            ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                            BindRetentionPolicy();
                            return;
                        }


                        ResultObject result = ES.Services.ExchangeServer.DeleteExchangeRetentionPolicyTag(PanelRequest.ItemID, mailboxPlanId);
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

                        ClearEditValues();

                    }
                    catch (Exception)
                    {
                        ShowErrorMessage("EXCHANGE_DELETE_RETENTIONPOLICY");
                    }

                    BindRetentionPolicy();
                break;

                case "EditItem":
                        ViewState["PolicyID"] = mailboxPlanId;

                        tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(PanelRequest.ItemID, mailboxPlanId);

                        txtPolicy.Text = tag.TagName;
                        Utils.SelectListItem(ddTagType, tag.TagType);
                        ageLimitForRetention.QuotaValue = tag.AgeLimitForRetention;
                        Utils.SelectListItem(ddRetentionAction, tag.RetentionAction);

                        btnUpdatePolicy.Enabled = true;
                        btnCancelPolicy.Enabled = true;

                        btnAddPolicy.Enabled = false;
                        ddTagType.Enabled = false;
                        

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
            Providers.HostedSolution.ExchangeRetentionPolicyTag tag;

            tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(PanelRequest.ItemID, mailboxPlanId);

            if (tag.ItemID != PanelRequest.ItemID)
            {
                ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                BindRetentionPolicy();
                return;
            }


            tag.TagName = txtPolicy.Text;
            tag.TagType = Convert.ToInt32(ddTagType.SelectedValue);
            tag.AgeLimitForRetention = ageLimitForRetention.QuotaValue;
            tag.RetentionAction = Convert.ToInt32(ddRetentionAction.SelectedValue);

            ResultObject result = ES.Services.ExchangeServer.UpdateExchangeRetentionPolicyTag(PanelRequest.ItemID, tag);

            if (!result.IsSuccess)
            {
                messageBox.ShowMessage(result,"EXCHANGE_UPDATERETENTIONPOLICY", null);
            }
            else
            {
               messageBox.ShowSuccessMessage("EXCHANGE_UPDATERETENTIONPOLICY");
            }

            BindRetentionPolicy();
        }


        public string GetTagType(int ItemID)
        {
            string imgName = string.Empty;

            if (ItemID == PanelRequest.ItemID)
                imgName = "admin_16.png";
            else
                imgName = "company24.png";

            return GetThemedImage("Exchange/" + imgName);
        }

        protected void ClearEditValues()
        {
            txtPolicy.Text = string.Empty;
            ageLimitForRetention.QuotaValue = 0;

            ddTagType.Enabled = true;

            btnAddPolicy.Enabled = true;
            btnUpdatePolicy.Enabled = false;
            btnCancelPolicy.Enabled = false;
        }

        protected void btnCancelPolicy_Click(object sender, EventArgs e)
        {
            ClearEditValues();
        }

    }
}
