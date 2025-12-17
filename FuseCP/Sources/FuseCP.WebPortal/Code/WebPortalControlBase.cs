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
using System.Resources;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.Text;

namespace FuseCP.Portal
{
	public class WebPortalControlBase : FuseCP.WebPortal.PortalControlBase
	{
		protected Hashtable Settings
		{
			get { return base.ModuleSettings; }
		}

        public string GetThemedImage(string imageUrl)
        {
            return PortalUtils.GetThemedImage(imageUrl);
        }

		public string GetSharedLocalizedString(string moduleName, string resourceKey)
		{
			return PortalUtils.GetSharedLocalizedString(moduleName, resourceKey);
		}

		public string GetLocalizedString(string resourceKey)
		{
			return (string)GetLocalResourceObject(resourceKey);
		}

        public string NavigateURL()
        {
            return PortalUtils.NavigateURL();
        }

        public string NavigateURL(string keyName, string keyValue, params string[] additionalParams)
        {
            return PortalUtils.NavigateURL(keyName, keyValue, additionalParams);
        }

        public string NavigatePageURL(string pageId, string keyName, string keyValue, params string[] additionalParams)
        {
            return PortalUtils.NavigatePageURL(pageId, keyName, keyValue, additionalParams);
        }
	}
}
