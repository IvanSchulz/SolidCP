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
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;
using System.Reflection;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeCheckDomainName : FuseCPModuleBase
    {
        private static string EXCHANGEACCOUNTEMAILADDRESSES = "ExchangeAccountEmailAddresses";
        private static string EXCHANGEACCOUNTS = "ExchangeAccounts";
        private static string LYNCUSERS = "LyncUsers";
        private static string SFBUSERS = "SfBUsers";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // save return URL
                if (Request.UrlReferrer!=null)
                    ViewState["ReturnUrl"] = Request.UrlReferrer.ToString();

                // domain name
                DomainInfo domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);
                litDomainName.Text = domain.DomainName;

                Bind();
            }
        }

        public string GetObjectType(string objectName, int objectType)
        {
            if (objectName == EXCHANGEACCOUNTS)
            {
                ExchangeAccountType accountType = (ExchangeAccountType)objectType;
                objectName = accountType.ToString();
            }

            string res = GetLocalizedString(objectName+".Text");

            if (string.IsNullOrEmpty(res))
                res = objectName;

            return res;
        }

        public bool AllowDelete(string objectName, int objectType)
        {
            if (objectName == EXCHANGEACCOUNTEMAILADDRESSES)
            {
                ExchangeAccountType accountType = (ExchangeAccountType)objectType;
                switch (accountType)
                {
                    case ExchangeAccountType.Room:
                    case ExchangeAccountType.Equipment:
                    case ExchangeAccountType.SharedMailbox:
                    case ExchangeAccountType.Mailbox:
                    case ExchangeAccountType.DistributionList:
                    case ExchangeAccountType.PublicFolder:
                        return true;
                }

            }
            return false;
        }


        public string GetObjectImage(string objectName, int objectType)
        {
            string imgName = "blank16.gif";

            if (objectName == EXCHANGEACCOUNTS)
            {
                ExchangeAccountType accountType = (ExchangeAccountType)objectType;

                imgName = "mailbox_16.gif";
                switch(accountType)
                {
                    case ExchangeAccountType.Contact:
                        imgName = "contact_16.gif";
                        break;
                    case ExchangeAccountType.DistributionList:
                        imgName = "dlist_16.gif";
                        break;
                    case ExchangeAccountType.Room:
                        imgName = "room_16.gif";
                        break;
                    case ExchangeAccountType.Equipment:
                        imgName = "equipment_16.gif";
                        break;
                    case ExchangeAccountType.SharedMailbox:
                        imgName = "shared_16.gif";
                        break;
                }

            }
            else if (objectName == EXCHANGEACCOUNTEMAILADDRESSES)
            {
                imgName = "mailbox_16.gif";
            }

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetEditUrl(string objectName, int objectType, string objectId, string ownerId)
        {
            if (objectName == EXCHANGEACCOUNTS)
            {
                string key = "";

                ExchangeAccountType accountType = (ExchangeAccountType)objectType;

                switch (accountType)
                {
                    case ExchangeAccountType.User:
                        key = "edit_user";
                        return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key,
                            "AccountID=" + objectId,
                            "ItemID=" + PanelRequest.ItemID, "context=user");

                    case ExchangeAccountType.Mailbox:
                    case ExchangeAccountType.Room:
                    case ExchangeAccountType.Equipment:
                    case ExchangeAccountType.SharedMailbox:
                        key = "mailbox_settings";
                        break;
                    case ExchangeAccountType.DistributionList:
                        key = "dlist_settings";
                        break;
                    case ExchangeAccountType.PublicFolder:
                        key = "public_folder_settings";
                        break;
                    case ExchangeAccountType.SecurityGroup:
                    case ExchangeAccountType.DefaultSecurityGroup:
                        key = "secur_group_settings";
                        break;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key,
                            "AccountID=" + objectId,
                            "ItemID=" + PanelRequest.ItemID);
                }
            }

            if (objectName == EXCHANGEACCOUNTEMAILADDRESSES)
            {
                string key = "";

                ExchangeAccountType accountType = (ExchangeAccountType)objectType;

                switch (accountType)
                {
                    case ExchangeAccountType.Mailbox:
                    case ExchangeAccountType.Room:
                    case ExchangeAccountType.Equipment:
                    case ExchangeAccountType.SharedMailbox:
                        key = "mailbox_addresses";
                        break;
                    case ExchangeAccountType.DistributionList:
                        key = "dlist_addresses";
                        break;
                    case ExchangeAccountType.PublicFolder:
                        key = "public_folder_addresses";
                        break;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key,
                                "AccountID=" + ownerId,
                                "ItemID=" + PanelRequest.ItemID);
                }
            }

            if (objectName == LYNCUSERS)
            {
                return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_lync_user",
                    "AccountID=" + objectId,
                    "ItemID=" + PanelRequest.ItemID);
            }
            if (objectName == SFBUSERS)
            {
                return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_sfb_user",
                    "AccountID=" + objectId,
                    "ItemID=" + PanelRequest.ItemID);
            }

            return "";
        }

        private void Bind()
        {
            DomainInfo domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);

            gvObjects.DataSource =
                ES.Services.Organizations.GetOrganizationObjectsByDomain(PanelRequest.ItemID, domain.DomainName);
            gvObjects.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (ViewState["ReturnUrl"] != null)
                Response.Redirect((string)ViewState["ReturnUrl"]);
        }

        protected void gvObjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                try
                {
                    string[] arg = e.CommandArgument.ToString().Split(',');
                    if (arg.Length != 3) return;

                    string[] emails = { arg[2] };

                    int accountID = 0;
                    if (!int.TryParse(arg[0], out accountID))
                        return;

                    int accountTypeID = 0;
                    if (!int.TryParse(arg[1], out accountTypeID))
                        return;

                    ExchangeAccountType accountType = (ExchangeAccountType)accountTypeID;

                    int result;

                    switch(accountType)
                    {
                        case ExchangeAccountType.Room:
                        case ExchangeAccountType.Equipment:
                        case ExchangeAccountType.SharedMailbox:
                        case ExchangeAccountType.Mailbox:
                            result = ES.Services.ExchangeServer.DeleteMailboxEmailAddresses(
                                PanelRequest.ItemID, accountID, emails);
                            break;
                        case ExchangeAccountType.DistributionList:
                            result = ES.Services.ExchangeServer.DeleteDistributionListEmailAddresses(
                                PanelRequest.ItemID, accountID, emails);
                            break;
                        case ExchangeAccountType.PublicFolder:
                            result = ES.Services.ExchangeServer.DeletePublicFolderEmailAddresses(
                                PanelRequest.ItemID, accountID, emails);
                            break;
                    }

                    Bind();
                }
                catch (AmbiguousMatchException)
                {
                }
            }
        }



    }
}
