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

namespace FuseCP.Providers.Web.Iis.Common
{
    using Microsoft.Web.Administration;
    using System;

    internal sealed class IsapiCgiRestrictionElement : ConfigurationElement
    {
        private static readonly string AllowedAttribute = "allowed";
        private static readonly string DescriptionAttribute = "description";
        private static readonly string GroupIdAttribute = "groupId";
        private static readonly string PathAttribute = "path";

        public bool Allowed
        {
            get
            {
                return (bool) base[AllowedAttribute];
            }
            set
            {
                base[AllowedAttribute] = value;
            }
        }

        public string Description
        {
            get
            {
                return (string) base[DescriptionAttribute];
            }
            set
            {
                base[DescriptionAttribute] = value;
            }
        }

        public string GroupId
        {
            get
            {
                return (string) base[GroupIdAttribute];
            }
        }

        public string Path
        {
            get
            {
                return (string) base[PathAttribute];
            }
            set
            {
                base[PathAttribute] = value;
            }
        }
    }
}

