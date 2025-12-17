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
using FuseCP.WebPortal;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.SkinControls
{
    public partial class TopMenu : System.Web.UI.UserControl
    {
        public string Align
        {
            get
            {
                if (ViewState["Align"] == null)
                {
                    return "top"; 
                }
                return ViewState["Align"].ToString(); 
            }
            set { ViewState["Align"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void topMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            var node = ((SiteMapNode)e.Item.DataItem);
            e.Item.Value = node.Key; 
            if (node["align"] == Align)
            {
                topMenu.Items.Remove(e.Item);
                return;
            }

            // Remove API Documentation link if EnterpriseServer is not embedded
            if (node.Key == "APIDocumentation" && !PortalConfiguration.SiteSettings["EnterpriseServer"].StartsWith("assembly://"))
            {
                e.Item.Parent.ChildItems.Remove(e.Item);
            }

            //if (Align.Equals("left") && node.Title.ToLower().Equals("account home"))
            //{
            //    e.Item.Text = string.Empty;

            //    string imagePath = String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images", "/");

            //    e.Item.ImageUrl = imagePath + "home_24.png"; 
            //}

            string target = node["target"];

            if(!String.IsNullOrEmpty(target))
                e.Item.Target = target;

            //for Selected == added kuldeep 
            if (Request.QueryString.Get("pid") != null)
            {
                string pid = Request.QueryString.Get("pid").ToString();
                if(e.Item.DataPath == pid)
                {
                    e.Item.Selected = true;
                }
            }
        
        }
    }
}
