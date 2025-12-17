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
using System.Xml;

namespace FuseCP.Portal
{
    public partial class TextHTML : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // get module data
            XmlNode content = PortalUtils.GetModuleContentNode(this);
            string resourcekey = content.Attributes["resourcekey"] != null
                ? content.Attributes["resourcekey"].Value : null;

            bool template = content.Attributes["template"] != null
                ? Utils.ParseBool(content.Attributes["template"].Value, false) : false;

            string text = !String.IsNullOrEmpty(resourcekey)
                ? GetSharedLocalizedString("ModuleContent." + resourcekey) : content.InnerText;

            if(template)
            {
                text = ES.Services.Packages.EvaluateUserPackageTempate(
                    PanelSecurity.SelectedUserId, PanelSecurity.PackageId, text);
            }

            litContent.Text = text;
        }
    }
}
