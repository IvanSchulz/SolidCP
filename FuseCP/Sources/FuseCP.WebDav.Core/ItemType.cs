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

using FuseCP.WebDav.Core.Attributes.Resources;
using FuseCP.WebDav.Core.Resources;

namespace FuseCP.WebDav.Core
{
    namespace Client
    {
        public enum ItemType
        {
            [LocalizedDescription(typeof(WebDavResources), "ItemTypeResource")]
            Resource,
            [LocalizedDescription(typeof(WebDavResources), "ItemTypeFolder")]
            Folder,
            Version,
            VersionHistory
        }
    }
}
