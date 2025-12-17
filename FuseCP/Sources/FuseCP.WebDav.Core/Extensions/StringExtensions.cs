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

namespace FuseCP.WebDav.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceLast(this string source, string target, string newValue)
        {
            int index = source.LastIndexOf(target);
            string result = source.Remove(index, target.Length).Insert(index, newValue);
            return result;
        }

        public static string Tail(this string source, int tailLength)
        {
            if (source == null || tailLength >= source.Length)
            {
                return source;
            }

            return source.Substring(source.Length - tailLength);
        }

        public static string RemoveLeadingFromPath(this string source, string toRemove)
        {
            return source.StartsWith('/' + toRemove) ? source.Substring(toRemove.Length + 1) : source;
        }
    }
}
