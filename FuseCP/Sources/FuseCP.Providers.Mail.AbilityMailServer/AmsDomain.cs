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
using System.IO;
using System.Text;

namespace FuseCP.Providers.Mail
{
	public class AmsDomain
	{
		private string domainName;
		private TreeNode domainConfig;

		public FuseCP.Providers.Mail.TreeNode DomainConfig
		{
			get { return this.domainConfig; }
		}

		public AmsDomain(string domainName)
		{
			this.domainName = domainName;
			this.domainConfig = new TreeNode();
		}

		public bool Load(Tree config)
		{
			foreach (TreeNode node in config.ChildNodes)
			{
				string amsDomain = node["domain"];
				if (String.Compare(amsDomain, domainName, true) == 0)
				{
					this.domainConfig = node;
					return true;
				}
			}
			return false;
		}

		public bool Save(Tree config)
		{
			if (!config.ChildNodes.Contains(domainConfig))
			{
                domainConfig["dir"] = Path.Combine(AMSHelper.AMSLocation, domainName);
				config.ChildNodes.Add(domainConfig);
			}

			return AMSHelper.SetDomainsConfig(config);
		}

		public bool Delete(Tree config)
		{
			if (config.ChildNodes.Contains(domainConfig))
				config.ChildNodes.Remove(domainConfig);

			Tree usersConfig = AMSHelper.GetUsersConfig();
			List<TreeNode> nodesToDelete = new List<TreeNode>();
			foreach (TreeNode node in usersConfig.ChildNodes)
			{
				if (string.Compare(node["domain"], domainName, true) == 0)
					nodesToDelete.Add(node);
			}

			while (nodesToDelete.Count > 0)
			{
				usersConfig.ChildNodes.Remove(nodesToDelete[0]);
				nodesToDelete.RemoveAt(0);
			}

			Tree listsConfig = AMSHelper.GetMailListsConfig();
			foreach (TreeNode node in listsConfig.ChildNodes)
			{
				if (string.Compare(node["domain"], domainName, true) == 0)
					nodesToDelete.Add(node);
			}

			while (nodesToDelete.Count > 0)
			{
				listsConfig.ChildNodes.Remove(nodesToDelete[0]);
				nodesToDelete.RemoveAt(0);
			}

			return AMSHelper.RemoveDomain(domainName) && 
				AMSHelper.SetUsersConfig(usersConfig) && 
				AMSHelper.SetMailListsConfig(listsConfig) && 
				AMSHelper.SetDomainsConfig(config);
		}

		public bool DeleteAlias(Tree config)
		{
			if (config.ChildNodes.Contains(domainConfig))
				config.ChildNodes.Remove(domainConfig);

			Tree usersConfig = AMSHelper.GetUsersConfig();
			List<TreeNode> nodesToDelete = new List<TreeNode>();
			foreach (TreeNode node in usersConfig.ChildNodes)
			{
				if (string.Compare(node["domain"], domainName, true) == 0)
					nodesToDelete.Add(node);
			}

			while (nodesToDelete.Count > 0)
			{
				usersConfig.ChildNodes.Remove(nodesToDelete[0]);
				nodesToDelete.RemoveAt(0);
			}

			Tree listsConfig = AMSHelper.GetMailListsConfig();
			foreach (TreeNode node in listsConfig.ChildNodes)
			{
				if (string.Compare(node["domain"], domainName, true) == 0)
					nodesToDelete.Add(node);
			}

			while (nodesToDelete.Count > 0)
			{
				listsConfig.ChildNodes.Remove(nodesToDelete[0]);
				nodesToDelete.RemoveAt(0);
			}

			return AMSHelper.SetUsersConfig(usersConfig) &&
				AMSHelper.SetMailListsConfig(listsConfig) &&
				AMSHelper.SetDomainsConfig(config);
		}

		public void Read(MailDomain domain)
		{
			domainConfig["enabled"] = domain.Enabled ? "1" : "0";
			domainConfig["domain"] = domain.Name;
			domainConfig["mode"] = "0";

			domainConfig["usemaxusers"] = (domain.MaxDomainUsers == 0) ? "0" : "1";
			domainConfig["maxusers"] = (domain.MaxDomainUsers == 0) ? "0" : domain.MaxDomainUsers.ToString();

			domainConfig["usemaxmailinglists"] = (domain.MaxLists == 0) ? "0" : "1";
			domainConfig["maxmailinglists"] = (domain.MaxLists == 0) ? "0" : domain.MaxLists.ToString();
			
			if (!string.IsNullOrEmpty(domain.CatchAllAccount))
			{
				domainConfig["usecatchcalluser"] =  "1";
				domainConfig["catchalluser"] = domain.CatchAllAccount;
			}
			else
			{
				domainConfig["usecatchcalluser"] = "0";
				domainConfig["catchalluser"] = string.Empty;
			}
		}

		public MailDomain ToMailDomain()
		{
			if (domainConfig["mode"] == "0")
			{
				MailDomain domain = new MailDomain();

				domain.Enabled = domainConfig["enabled"] == "1" ? true : false;
				domain.Name = domainConfig["domain"];

				if (domainConfig["usemaxusers"] == "1")
					domain.MaxDomainUsers = Convert.ToInt32(domainConfig["maxusers"]);

				if (domainConfig["usemaxmailinglists"] == "1")
					domain.MaxLists = Convert.ToInt32(domainConfig["maxmailinglists"]);

				if (domainConfig["usecatchcalluser"] == "1")
					domain.CatchAllAccount = domainConfig["catchalluser"];

				return domain;
			}

			return null;
		}

		public static string[] GetDomainAliases(Tree config, string domainName)
		{
			List<string> list = new List<string>();

			foreach (TreeNode node in config.ChildNodes)
			{
				string mode = node["mode"];
				string convert = node["convertdomain"];

				if (String.Compare(convert, domainName, true) == 0 && mode == "1")
					list.Add(node["domain"]);
			}

			return list.ToArray();
		}

		public static string[] GetDomains(Tree config)
		{
			List<string> domains = new List<string>();

			foreach (TreeNode node in config.ChildNodes)
				domains.Add(node["domain"]);

			return domains.ToArray();
		}
	}
}
