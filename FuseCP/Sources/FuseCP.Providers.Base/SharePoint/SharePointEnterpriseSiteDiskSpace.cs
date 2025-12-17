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

namespace FuseCP.Providers.SharePoint
{
    [Serializable]
    public class SharePointEnterpriseSiteDiskSpace
    {
        private string url;
        private long diskSpace;
       
        
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public long DiskSpace
        {
            get { return diskSpace; }
            set { diskSpace = value; }
        }
    }
}
