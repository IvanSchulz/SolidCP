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

﻿using System;
﻿using System.Globalization;
﻿using FuseCP.Providers.WebAppGallery;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal
{
    public partial class WebApplicationGalleryHeader :  FuseCPModuleBase
    {
        public void BindApplicationDetails(GalleryApplication application)
        {
            lblVersion.Text = application.Version;
            lblDescription.Text = application.Description;
            lblTitle.Text = application.Title;
            lblSize.Text = application.InstallerFileSize.ToString(CultureInfo.InvariantCulture);
            imgLogo.ImageUrl = "~/DesktopModules/FuseCP/resizeimage.ashx?url=" + Server.UrlEncode(application.IconUrl) +
                               "&width=200&height=200";

            hlAuthor.Text = application.AuthorName;
            hlAuthor.NavigateUrl = application.AuthorUrl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
