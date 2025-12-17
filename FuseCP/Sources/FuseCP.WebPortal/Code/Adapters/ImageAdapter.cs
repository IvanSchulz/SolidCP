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
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
	public class ImageAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
	{
		protected override void Render(HtmlTextWriter writer)
		{
			Image img = Control as Image;
			//
			if (img != null && Page != null)
			{
				//
				HttpBrowserCapabilities browser = Page.Request.Browser;
				//
				if (browser.Browser == "IE" &&
					(browser.Version == "5.0" || browser.Version == "5.5" || browser.Version == "6.0")
					&& !String.IsNullOrEmpty(img.ImageUrl) && img.ImageUrl.ToLower().EndsWith(".png"))
				{
					// add other attributes
					string imageUrl = img.ImageUrl; // save original URL

					// change original URL to empty
					img.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "FuseCP.WebPortal.Code.Adapters.empty.gif");

					imageUrl = img.ResolveClientUrl(imageUrl);
					img.Attributes["style"] =
						String.Format("filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}', sizingMethod='scale');",
						imageUrl);
				}
			}
			//
			base.Render(writer);
		}
	}
}
