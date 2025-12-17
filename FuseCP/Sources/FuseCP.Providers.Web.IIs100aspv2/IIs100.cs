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

using Microsoft.Web.Administration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using FuseCP.Providers.Common;
using FuseCP.Providers.Web.Iis;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Providers.Web
{
    public class IIs100 : IIs70
    {

        public override string[] Install()
        {
            var messages = new List<string>();

            messages.AddRange(base.Install());

            // TODO: Setup ccs

            return messages.ToArray();
        }

        public override bool IsIISInstalled()
        {
            int value = 0;
            RegistryKey root = Registry.LocalMachine;
            RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\InetStp");
            if (rk != null)
            {
                value = (int)rk.GetValue("MajorVersion", null);
                rk.Close();
            }

            return value >= 10;
        }

        public override bool IsInstalled()
        {
            return IsIISInstalled();
        }

        public override WebSite GetSite(string siteId)
        {
            var site = base.GetSite(siteId);
            return site;
        }
    }
}
