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
using System.Reflection;
using System.Text;

namespace FuseCP.Providers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LogPropertyAttribute : Attribute
    {
        public LogPropertyAttribute()
        {
        }

        public LogPropertyAttribute(string nameInLog)
        {
            NameInLog = nameInLog;
        }

        public string NameInLog { get; set; }

        public string GetLogString(object obj, PropertyInfo propertyInfo)
        {
            if (obj != null && propertyInfo != null)
            {
                var value = LogExtensionHelper.GetString(propertyInfo.GetValue(obj, null));
                return GetLogString(propertyInfo.Name, value);
            }

            return "";
        }

        public string GetLogString(string name, string value)
        {
            var logName = string.IsNullOrEmpty(NameInLog) ? LogExtensionHelper.DecorateName(name) : NameInLog;
            return LogExtensionHelper.CombineString(logName, value);
        }
    }
}
