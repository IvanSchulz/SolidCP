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

using FuseCP.WebDavPortal.Models.Common;

namespace FuseCP.WebDavPortal.Models
{
    public class OfficeOnlineModel 
    {
        public string Url { get; set; }
        public string FileName { get; set; }
        public string Backurl { get; set; }

        public OfficeOnlineModel(string url, string fileName, string backUrl)
        {
            Url = url;
            FileName = fileName;
            Backurl = backUrl;
        }
    }
}
