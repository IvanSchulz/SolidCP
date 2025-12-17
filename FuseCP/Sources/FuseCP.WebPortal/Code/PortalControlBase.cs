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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace FuseCP.WebPortal
{
    public class PortalControlBase : UserControl
    {
        private Control containerControl;
        private PageModule module;
        public PageModule Module
        {
            get { return module; }
            set { module = value; }
        }

        public Control ContainerControl
        {
            get { return containerControl; }
            set { containerControl = value; }
        }

        protected int ModuleID
        {
            get { return module.ModuleId; }
        }

        protected Hashtable ModuleSettings
        {
            get { return module.Settings; }
        }

		public string EditUrl(string controlKey)
		{
			return EditUrl(null, null, controlKey);
		}

		public string EditUrl(string keyName, string keyValue, string controlKey)
		{
			return EditUrl(keyName, keyValue, controlKey, null);
		}

		public string EditUrl(string keyName, string keyValue, string controlKey, params string[] additionalParams)
		{
            return EditUrlStat(Request[DefaultPage.PAGE_ID_PARAM], ModuleID, keyName, keyValue, controlKey, additionalParams);
		}

        public static string EditUrlStat(string pageId, int ModuleID, string keyName, string keyValue, string controlKey, string[] additionalParams)
        {
            List<string> url = new List<string>();

            if (!String.IsNullOrEmpty(pageId))
                url.Add(String.Concat(DefaultPage.PAGE_ID_PARAM, "=", pageId));

            url.Add(String.Concat(DefaultPage.MODULE_ID_PARAM, "=", ModuleID));
            url.Add(String.Concat(DefaultPage.CONTROL_ID_PARAM, "=", controlKey));

            if (!String.IsNullOrEmpty(keyName) && !String.IsNullOrEmpty(keyValue))
            {
                url.Add(String.Concat(keyName, "=", keyValue));
            }

            if (additionalParams != null)
            {
                foreach (string additionalParam in additionalParams)
                    url.Add(additionalParam);
            }

            return "~/Default.aspx?" + String.Join("&", url.ToArray());
        }
    }
}
