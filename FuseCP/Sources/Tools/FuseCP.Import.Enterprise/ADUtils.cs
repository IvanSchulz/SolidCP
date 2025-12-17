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
using System.Globalization;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace FuseCP.Import.Enterprise
{
	public class ADUtils
	{
		public static string ADUsername
		{
			get
			{
				return ConfigurationManager.AppSettings["AD.Username"];
			}
		}

		public static string ADPassword
		{
			get
			{
				return ConfigurationManager.AppSettings["AD.Password"];
			}
		}

		public static string ADRootDomain
		{
			get { return Global.ADRootDomain; }
		}

		public static string PrimaryDomainController
		{
			get { return Global.PrimaryDomainController; }
		}

		public static string RootOU
		{
			get { return Global.RootOU; }
		}

		public static DirectoryEntry GetRootOU()
		{
			StringBuilder sb = new StringBuilder();
			// append provider
			AppendProtocol(sb);
			AppendDomainController(sb);
			AppendOUPath(sb, RootOU);
			AppendDomainPath(sb, ADRootDomain);

			DirectoryEntry de = GetADObject(sb.ToString());
			//ExchangeLog.LogEnd("GetRootOU");
			return de;
		}

		private static void AppendProtocol(StringBuilder sb)
		{
			sb.Append("LDAP://");
		}

		private static void AppendDomainController(StringBuilder sb)
		{
			string dc = PrimaryDomainController;
			if (dc.IndexOf(".") != -1)
				dc = dc.Substring(0, dc.IndexOf("."));
			sb.Append(dc + "/");
		}

		private static void AppendOUPath(StringBuilder sb, string ou)
		{
			if (string.IsNullOrEmpty(ou))
				return;

			string path = ou.Replace("/", "\\");
			string[] parts = path.Split('\\');
			for (int i = parts.Length - 1; i != -1; i--)
				sb.Append("OU=").Append(parts[i]).Append(",");
		}

		private static void AppendDomainPath(StringBuilder sb, string domain)
		{
			if (string.IsNullOrEmpty(domain))
				return;

			string[] parts = domain.Split('.');
			for (int i = 0; i < parts.Length; i++)
			{
				sb.Append("DC=").Append(parts[i]);

				if (i < (parts.Length - 1))
					sb.Append(",");
			}
		}

		private static DirectoryEntry GetADObject(string path)
		{
			DirectoryEntry de = null;
			de = new DirectoryEntry(path, ADUsername, ADPassword);
			de.RefreshCache();
			return de;
		}

		public static string RemoveADPrefix(string path)
		{
			string dn = path;
			if (dn.ToUpper().StartsWith("LDAP://"))
			{
				dn = dn.Substring(7);
			}
			int index = dn.IndexOf("/");

			if (index != -1)
			{
				dn = dn.Substring(index + 1);
			}
			return dn;
		}

		public static string GetAddressListsContainer()
		{
			StringBuilder sb = new StringBuilder();
			AppendProtocol(sb);
			AppendDomainController(sb);
			sb.Append("CN=Microsoft Exchange,CN=Services,CN=Configuration,");
			AppendDomainPath(sb, ADRootDomain);
			DirectoryEntry exchEntry = GetADObject(sb.ToString());
			DirectoryEntry orgEntry = null;
			foreach (DirectoryEntry child in exchEntry.Children)
			{
				orgEntry = child;
				break;
			}
			string ret = "CN=Address Lists Container," + RemoveADPrefix(orgEntry.Path);
			return ret;
		}
	}
}
