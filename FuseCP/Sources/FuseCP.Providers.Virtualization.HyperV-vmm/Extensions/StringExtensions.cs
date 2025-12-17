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
using System.Text.RegularExpressions;

namespace FuseCP.Providers.Virtualization.Extensions
{
    public static class StringExtensions
    {
        public static string[] ParseExact(
            this string data,
            string format)
        {
            return ParseExact(data, format, false);
        }

        public static string[] ParseExact(
            this string data,
            string format,
            bool ignoreCase)
        {
            string[] values;

            if (TryParseExact(data, format, out values, ignoreCase))
                return values;
            else
                throw new ArgumentException("Format not compatible with value.");
        }

        public static bool TryExtract(
            this string data,
            string format,
            out string[] values)
        {
            return TryParseExact(data, format, out values, false);
        }

        public static bool TryParseExact(
            this string data,
            string format,
            out string[] values,
            bool ignoreCase)
        {
            int tokenCount = 0;
            format = Regex.Escape(format).Replace("\\{", "{");

            for (tokenCount = 0; ; tokenCount++)
            {
                string token = string.Format("{{{0}}}", tokenCount);
                if (!format.Contains(token)) break;
                format = format.Replace(token,
                    string.Format("(?'group{0}'.*)", tokenCount));
            }

            RegexOptions options =
                ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

            Match match = new Regex(format, options).Match(data);

            if (tokenCount != (match.Groups.Count - 1))
            {
                values = new string[] { };
                return false;
            }
            else
            {
                values = new string[tokenCount];
                for (int index = 0; index < tokenCount; index++)
                    values[index] =
                        match.Groups[string.Format("group{0}", index)].Value;
                return true;
            }
        }
    }
}
