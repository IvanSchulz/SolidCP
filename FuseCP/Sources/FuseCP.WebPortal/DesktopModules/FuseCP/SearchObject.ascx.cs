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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using FuseCP.WebPortal;
using System.Xml;

namespace FuseCP.Portal
{
    public partial class SearchObject : FuseCPModuleBase
    {
        const string TYPE_WEBSITE = "WebSite";
        const string TYPE_DOMAIN = "Domain";
        const string TYPE_ORGANIZATION = "Organization";
        const string TYPE_EXCHANGEACCOUNT = "ExchangeAccount";
        const string TYPE_RDSCOLLECTION = "RDSCollection";
        const string TYPE_LYNC = "LyncAccount";
        const string TYPE_SFB = "SfBAccount";
        const string TYPE_FOLDER = "WebDAVFolder";
        const string TYPE_SHAREPOINT = "SharePointFoundationSiteCollection";
        const string TYPE_SHAREPOINTENTERPRISE = "SharePointEnterpriseSiteCollection";

        const string PID_SPACE_WEBSITES = "SpaceWebSites";
        const string PID_SPACE_DIMAINS = "SpaceDomains";
        const string PID_SPACE_EXCHANGESERVER = "SpaceExchangeServer";

        String m_strColTypes = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var jsonSerialiser = new JavaScriptSerializer();
                String[] aTypes = jsonSerialiser.Deserialize<String[]>(tbFilters.Text);
                if ((aTypes != null) && (aTypes.Length > 0))
                    m_strColTypes = "'" + String.Join("','", aTypes) + "'";
                else
                    m_strColTypes = "";
            }
        }

        protected void odsObjectPaged_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["colType"] = m_strColTypes;
        }

        protected void odsObjectPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                e.ExceptionHandled = true;
            }
        }

        protected string GetTypeDisplayName(string type)
        {
            return GetSharedLocalizedString("ServiceItemType." + type)
                ?? GetSharedLocalizedString("UserItemType." + type)
                ?? type;
        }

        public string GetItemPageUrl(string fullType, string itemType, int itemId, int spaceId, int accountId, string textSearch = "")
        {
            string res = "";
            if (fullType.Equals("AccountHome"))
            {
                res = PortalUtils.GetUserHomePageUrl(itemId);
            }
            else
            {
                switch (itemType)
                {
                    case TYPE_WEBSITE:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_WEBSITES, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "edit_item",
                            "moduleDefId=websites");
                        break;
                    case TYPE_DOMAIN:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_DIMAINS, "DomainID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "edit_item",
                            "moduleDefId=domains");
                        break;
                    case TYPE_ORGANIZATION:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "organization_home",
                            "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_EXCHANGEACCOUNT:
                        if (fullType.Equals("Mailbox"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=Mailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("Room"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=Mailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("SharedMailbox"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=Mailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("Equipment"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=Mailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("User"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=User", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("Contact"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=contact_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("PublicFolder"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=public_folder_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("DistributionList"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=dlist_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("DefaultSecurityGroup"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=secur_group_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("SecurityGroup"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=secur_group_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=User", "moduleDefId=ExchangeServer");
                        }
                        break;
                    case TYPE_RDSCOLLECTION:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=rds_edit_collection",
                            "CollectionId=" + accountId, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_LYNC:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId.ToString(), "ctl=edit_lync_user",
                            "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_SFB:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId.ToString(), "ctl=edit_sfb_user",
                            "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_FOLDER:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId.ToString(), "ctl=enterprisestorage_folder_settings",
                            "FolderID=" + textSearch, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_SHAREPOINT:
                    case TYPE_SHAREPOINTENTERPRISE:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=" + (itemType == TYPE_SHAREPOINT ? "sharepoint_edit_sitecollection" : "sharepoint_enterprise_edit_sitecollection"),
                            "SiteCollectionID=" + accountId, "moduleDefId=ExchangeServer");
                        break;
                    default:
                        res = PortalUtils.GetSpaceHomePageUrl(spaceId);
                        break;
                }
            }

            return res;
        }

    }
}
