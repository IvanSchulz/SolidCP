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
using System.Text;

namespace FuseCP.Providers.Web
{
    public class WebDavSetting
    {
        public string LocationDrive { get; set; }
        public string HomeFolder { get; set; }
        public string Domain { get; set; }

        public WebDavSetting() { }

        public WebDavSetting(string locationDrive, string homeFolder, string domain)
        {
            LocationDrive = locationDrive;
            HomeFolder = homeFolder;
            Domain = domain;
        }

        public bool IsEmpty()
        {
            // 06.09.2015 roland.breitschaft@x-company.de
            // Problem: Object returns the wrong Empty-State
            // Fix: Make an Validation with 'or'
            //return string.IsNullOrEmpty(LocationDrive) && string.IsNullOrEmpty(HomeFolder) && string.IsNullOrEmpty(Domain);
            return string.IsNullOrEmpty(LocationDrive) || string.IsNullOrEmpty(HomeFolder) || string.IsNullOrEmpty(Domain);
        }
    }
}
