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
    using Utility;
    using System;
    using System.Reflection;

    internal sealed class FastCgiApplicationCollection : ConfigurationElementCollectionBase<FastCgiApplication>
    {
        public FastCgiApplication Add(string fullPath, string arguments)
        {
            FastCgiApplication element = base.CreateElement();
            element.FullPath = fullPath;
            if (ConfigurationUtility.ShouldPersist(element.Arguments, arguments))
            {
                element.Arguments = arguments;
            }
            return base.Add(element);
        }

        protected override FastCgiApplication CreateNewElement(string elementTagName)
        {
            return new FastCgiApplication();
        }

        public FastCgiApplication this[string fullPath, string arguments]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    FastCgiApplication application = base[i];
                    if (string.Equals(application.FullPath, fullPath, StringComparison.OrdinalIgnoreCase) && string.Equals(application.Arguments, arguments, StringComparison.OrdinalIgnoreCase))
                    {
                        return application;
                    }
                }
                return null;
            }
        }
    }
}

