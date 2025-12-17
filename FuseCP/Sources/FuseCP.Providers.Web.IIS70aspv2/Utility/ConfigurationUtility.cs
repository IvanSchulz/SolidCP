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

namespace FuseCP.Providers.Web.Iis.Utility
{
    using System;
	using System.Collections.Generic;
	using Microsoft.Web.Administration;
	using System.Collections;

    internal static class ConfigurationUtility
    {
        public static bool ShouldPersist(string oldValue, string newValue)
        {
            if (String.IsNullOrEmpty(oldValue))
            {
                return !String.IsNullOrEmpty(newValue);
            }
            if (newValue == null)
            {
                return false;
            }
            return ((newValue.Length == 0) || !oldValue.Equals(newValue, StringComparison.Ordinal));
        }

        public static string GetQualifiedVirtualPath(string virtualPath)
        {
            if (!virtualPath.StartsWith("/"))
                return "/" + virtualPath;
            //
            return virtualPath;
        }

        public static string GetNonQualifiedVirtualPath(string virtualPath)
        {
            if (virtualPath == "/")
                return virtualPath;
            //
            if (virtualPath.StartsWith("/"))
                return virtualPath.Substring(1);
            //
            return virtualPath;
        }
    }
}

