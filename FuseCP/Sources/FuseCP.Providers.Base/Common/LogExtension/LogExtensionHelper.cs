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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FuseCP.Providers
{
    public class LogExtensionHelper
    {
        public const string LOG_STRING_TEMPLATE = "{0}: {1}";
        public const string LOG_ARRAY_SEPARATOR = ", ";

        public static string CombineString(string name, string value)
        {
            if (name == null)
                name = "";

            if (value == null)
                value = "";

            return String.Format(LOG_STRING_TEMPLATE, name, value);
        }

        public static string DecorateName(string name)
        {
            if (name == null)
                return "";

            name = Regex.Replace(name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0"); // "DriveIsSCSICompatible" becomes "Drive Is SCSI Compatible"
            name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name); // Capitalize
            name = Regex.Replace(name, @"\bId\b", "ID", RegexOptions.IgnoreCase); // "Id" becomes "ID"

            return name;
        }

        public static string GetString(object value)
        {
            if (value == null)
                return "";

            // if array
            if (value.GetType().IsArray)
            {
                var elementType = value.GetType().GetElementType();

                if (elementType != null && !elementType.IsValueType)
                {
                    string[] strs = ((IEnumerable) value).Cast<object>().Select(x => x.ToString()).ToArray();
                    return string.Join(LOG_ARRAY_SEPARATOR, strs);
                }
            }

            return value.ToString();
        }
    }
}
