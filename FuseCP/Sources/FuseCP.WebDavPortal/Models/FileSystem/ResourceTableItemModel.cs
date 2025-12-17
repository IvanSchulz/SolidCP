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
using FuseCP.WebDav.Core.Client;
using FuseCP.WebDavPortal.Models.Common.DataTable;

namespace FuseCP.WebDavPortal.Models.FileSystem
{
    public class ResourceTableItemModel : JqueryDataTableBaseEntity
    {
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public Uri Href { get; set; }
        public bool IsTargetBlank { get; set; }
        public bool IsFolder { get; set; }
        public long Size { get; set; }
        public bool IsRoot { get; set; }
        public long Quota { get; set; }
        public string Type { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedFormated { get; set; }
        public string IconHref { get; set; }
        public string FolderUrlAbsoluteString { get; set; }
        public string FolderUrlLocalString { get; set; }
        public string FolderName { get; set; }
        public string Summary { get; set; }

        public override dynamic this[int index]
        {
            get
            {
                switch (index)
                {
                    case 1 :
                    {
                        return Size;
                    }
                    case 2:
                    {
                        return LastModified;
                    }
                    case 3:
                    {
                        return Type;
                    }
                    default:
                    {
                        return DisplayName;
                    }
                }
            }
        }
    }
}
