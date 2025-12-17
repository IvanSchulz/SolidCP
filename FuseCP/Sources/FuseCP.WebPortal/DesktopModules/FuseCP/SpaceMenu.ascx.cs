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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class SpaceMenu : FuseCPModuleBase
    {
        DataSet myPackages;
        int currentPackage;

        private PackageContext cntx = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //PanelSecurity.SelectedUser.Role == UserRole.ResellerCSR ||
            //    PanelSecurity.SelectedUser.Role == UserRole.Reseller ||
            //    PanelSecurity.SelectedUser.Role == UserRole.ResellerHelpdesk ||

            if ( PanelSecurity.SelectedUser.Role == UserRole.Administrator)
            {
                Visible = false;
            }

            if (PanelSecurity.PackageId == 0)
            {
                myPackages = new PackagesHelper().GetMyPackages();
                //For selectedUser have Packages or not then HIDE Menu
                if (myPackages.Tables[0].Rows.Count == 0)
                {
                    Visible = false;
                }

                if (Session["currentPackage"] == null || ((int)Session["currentUser"]) != PanelSecurity.SelectedUserId)
                {
                    if (myPackages.Tables[0].Rows.Count > 0)
                    {
                        Session["currentPackage"] = myPackages.Tables[0].Rows[0][0].ToString();
                        Session["currentUser"] = PanelSecurity.SelectedUserId;
                    }
                }
                currentPackage = Convert.ToInt32(Session["currentPackage"]);
            }
            else
            { 
                currentPackage = PanelSecurity.PackageId; 
            }
            // load package context
            cntx = PackagesHelper.GetCachedPackageContext(currentPackage);

            // bind root node
            MenuItem rootItem = new MenuItem(locMenuTitle.Text);
            rootItem.Value = "Hosting Space Menu";
            rootItem.Selectable = false;

            menu.Items.Add(rootItem);

            BindMenu(rootItem.ChildItems, PortalUtils.GetModuleMenuItems(this));
        }

        private void BindMenu(MenuItemCollection items, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                string pageId = null;
                if (node.Attributes["pageID"] != null)
                    pageId = node.Attributes["pageID"].Value;

                if (!PortalUtils.PageExists(pageId))
                    continue;

                string url = null;
                if (node.Attributes["url"] != null)
                    url = node.Attributes["url"].Value;

                string title = null;
                if (node.Attributes["title"] != null)
                    title = node.Attributes["title"].Value;

                string target = null;
                if (node.Attributes["target"] != null)
                    target = node.Attributes["target"].Value;

                string resourceGroup = null;
                if (node.Attributes["resourceGroup"] != null)
                    resourceGroup = node.Attributes["resourceGroup"].Value;

                string quota = null;
                if (node.Attributes["quota"] != null)
                    quota = node.Attributes["quota"].Value;

                bool disabled = false;
                if (node.Attributes["disabled"] != null)
                    disabled = Utils.ParseBool(node.Attributes["disabled"].Value, false);

                // get custom page parameters
                XmlNodeList xmlParameters = node.SelectNodes("Parameters/Add");
                List<string> parameters = new List<string>();
                foreach (XmlNode xmlParameter in xmlParameters)
                {
                    parameters.Add(xmlParameter.Attributes["name"].Value
                        + "=" + xmlParameter.Attributes["value"].Value);
                }

                // add menu item
                string pageUrl = !String.IsNullOrEmpty(url) ? url : PortalUtils.NavigatePageURL(
                    pageId, PortalUtils.SPACE_ID_PARAM, currentPackage.ToString(), parameters.ToArray());
                string pageName = !String.IsNullOrEmpty(title) ? title : PortalUtils.GetLocalizedPageName(pageId);
                MenuItem item = new MenuItem(pageName, pageId, "", disabled ? null : pageUrl);
                
                if (!String.IsNullOrEmpty(target))
                    item.Target = target;
                item.Selectable = !disabled;

                // check groups/quotas
                bool display = true;
                String displayValue = String.Empty;
                if (cntx != null)
                {
                    display = (String.IsNullOrEmpty(resourceGroup)
                        || cntx.Groups.ContainsKey(resourceGroup)) &&
                        (String.IsNullOrEmpty(quota)
                        || (cntx.Quotas.ContainsKey(quota) &&
                            cntx.Quotas[quota].QuotaAllocatedValue != 0));

                    if (!String.IsNullOrEmpty(resourceGroup))
                    {
                        displayValue = "String.IsNullOrEmpty(resourceGroup): " + String.IsNullOrEmpty(resourceGroup).ToString();
                        displayValue += "| cntx.Groups.ContainsKey(resourceGroup): " + cntx.Groups.ContainsKey(resourceGroup).ToString();
                        displayValue += "| resourceGroup: " + resourceGroup;
                    }
                    if (!String.IsNullOrEmpty(quota))
                    {
                        displayValue += "| String.IsNullOrEmpty(quota): " + String.IsNullOrEmpty(quota).ToString();
                        displayValue += "| cntx.Quotas.ContainsKey(quota): " + cntx.Quotas.ContainsKey(quota).ToString();

                        if (cntx.Quotas.ContainsKey(quota))
                            displayValue += "| cntx.Quotas[quota].QuotaAllocatedValue: " + cntx.Quotas[quota].QuotaAllocatedValue.ToString();

                        displayValue += "| quota: " + quota;
                    }
                }

                if (display)
                {
                    // process nested menu items
                    XmlNodeList xmlNestedNodes = node.SelectNodes("MenuItems/MenuItem");
                    BindMenu(item.ChildItems, xmlNestedNodes);
                }
                //item.Text += displayValue;
                //Response.Write("DisplayValue :[" + displayValue + "] ");

                //for Selected == added kuldeep 
                if (Request.QueryString.Get("pid") != null)
                {
                    string pid = Request.QueryString.Get("pid").ToString();
                  

                    if (item.NavigateUrl.IndexOf(pid) >= 0)
                    {
                        item.Selected = true;
                    }
                }

                if (display && !(disabled && item.ChildItems.Count == 0))
                    items.Add(item);
            }
        }
    }
}
