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

using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;
using System;
using System.Collections;

namespace FuseCP.EnterpriseServer.MailTemplates
{
	public class TemplateHashtable : Hashtable, IDisposable
	{
		ControllerBase Provider;
		UserController userController;
		protected UserController UserController => userController ??= new UserController(Provider);
		public TemplateHashtable(ControllerBase provider) { Provider = provider; }

		public string LogoUrl
		{
			get
			{
				string str;
				object item = this["logoUrl"];
				if (item != null)
				{
					str = item.ToString();
				}
				else
				{
					str = null;
				}
				return str;
			}
			set
			{
				this["logoUrl"] = value;
			}
		}

		public UserInfo User
		{
			get
			{
				return (UserInfo)this["user"];
			}
			set
			{
				this["user"] = value;
			}
		}

		public TemplateHashtable(UserInfo user, ControllerBase provider = null) : this(user, null, provider)
		{
		}

		public TemplateHashtable(UserInfo user, int orgId, ControllerBase provider = null) : this(user, new int?(orgId), provider)
		{
		}

		private TemplateHashtable(UserInfo user, int? orgId, ControllerBase provider = null): this(provider)
		{
			string organizationLogoUrl;
			string str;
			if (user != null)
			{
				this.User = user;
				UserSettings userSettings = UserController.GetUserSettings(user.UserId, "FuseCPPolicy");
				if (!string.IsNullOrEmpty(userSettings["LogoImageURL"]))
				{
					this.LogoUrl = userSettings["LogoImageURL"];
				}
			}
			if (orgId.HasValue)
			{
				OrganizationGeneralSettings organizationGeneralSettings;

				using (var organizationController = new OrganizationController())
				{
					organizationGeneralSettings = organizationController.GetOrganizationGeneralSettings(orgId.Value);
				}
				if (organizationGeneralSettings != null)
				{
					organizationLogoUrl = organizationGeneralSettings.OrganizationLogoUrl;
				}
				else
				{
					organizationLogoUrl = null;
				}
				if (!string.IsNullOrEmpty(organizationLogoUrl))
				{
					if (organizationGeneralSettings != null)
					{
						str = organizationGeneralSettings.OrganizationLogoUrl;
					}
					else
					{
						str = null;
					}
					this.LogoUrl = str;
				}
			}
		}

		bool isDisposed = false;
		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
				userController?.Dispose();
			}
		}
	}
}
