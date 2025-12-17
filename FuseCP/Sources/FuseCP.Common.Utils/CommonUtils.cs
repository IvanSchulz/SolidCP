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
using System.IO;
using System.Data;
using System.Collections;
using System.Xml;
using System.Web;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Common.Utils
{
	public class Utils
	{
		public static bool IsEmpty(string str)
		{
			return (str == null || str.Trim() == "");
		}

		public static bool IsNotEmpty(string str)
		{
			return !IsEmpty(str);
		}

		public static int ParseInt(string val, int defaultValue)
		{
			int result = defaultValue;
			try { result = Int32.Parse(val); }
			catch { /* do nothing */ }
			return result;
		}

		public static decimal ParseDecimal(string val, decimal defaultValue)
		{
			decimal result = defaultValue;
			try { result = Decimal.Parse(val); }
			catch { /* do nothing */ }
			return result;
		}

		public static string[] ParseDelimitedString(string str, params char[] delimiter)
		{
			string[] parts = str.Split(delimiter);
			ArrayList list = new ArrayList();
			foreach (string part in parts)
				if (part.Trim() != "" && !list.Contains(part.Trim()))
					list.Add(part);
			return (string[])list.ToArray(typeof(string));
		}

		public static string ReplaceStringVariable(string str, string variable, string value)
		{
			if (IsEmpty(str) || IsEmpty(value))
				return str;

			Regex re = new Regex("\\[" + variable + "\\]+", RegexOptions.IgnoreCase);
			return re.Replace(str, value);
		}

		public static string BuildIdentityXmlFromArray(int[] ids, string rootName, string childName)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement nodeRoot = doc.CreateElement(rootName);
			foreach (int id in ids)
			{
				XmlElement nodeChild = doc.CreateElement(childName);
				nodeChild.SetAttribute("id", id.ToString());
				nodeRoot.AppendChild(nodeChild);
			}

			return nodeRoot.OuterXml;
		}
	}
}
