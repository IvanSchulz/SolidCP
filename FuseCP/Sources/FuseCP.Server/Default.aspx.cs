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

#if NETFRAMEWORK

using System;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using FuseCP.Providers.OS;
using FuseCP.Server.Code;

namespace FuseCP.Server
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // set url
            string url = Request.Url.ToString();
            litUrl.Text = url.Substring(0, url.LastIndexOf("/"));

            // set version
            litVersion.Text = AutoDiscoveryHelper.GetServerVersion();

            // asp.net mode
            litAspNetMode.Text = (IntPtr.Size == 8) ? "64-bit" : "32-bit";
            litOS.Text = AutoDiscoveryHelper.OS;
        }
    }
}
#endif
