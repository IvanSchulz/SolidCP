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
using FuseCP.Providers.OS;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for ServerInfo.
	/// </summary>
	[Serializable]
	public class ServerInfo
	{
		private int serverId;
		private string serverName;
		private string serverUrl;
		private String password;
		private string comments;
		private bool virtualServer;
		private string instantDomainAlias;
		private bool adEnabled;
		private string adRootDomain;
		private string adAuthenticationType;
		private string adUsername;
		private string adPassword;
		private int primaryGroupId;
		private string adParentDomain;
		private string adParentDomainController;
		private OSPlatform osPlatform = OSPlatform.Unknown;
		private bool? isCore = null;

		public ServerInfo()
		{
		}
		public OSPlatform OSPlatform
		{
			get { return osPlatform; }
			set { osPlatform = value; }
		}

		public bool? IsCore
		{
			get { return isCore; }
			set { isCore = value; }
		}
		public int PrimaryGroupId
		{
			get { return primaryGroupId; }
			set { primaryGroupId = value; }
		}

		public string ADRootDomain
		{
			get { return adRootDomain; }
			set { adRootDomain = value; }
		}

		public string ADAuthenticationType
		{
			get { return this.adAuthenticationType; }
			set { this.adAuthenticationType = value; }
		}

		public string ADUsername
		{
			get { return adUsername; }
			set { adUsername = value; }
		}

		public string ADPassword
		{
			get { return adPassword; }
			set { adPassword = value; }
		}

		public int ServerId
		{
			get { return serverId; }
			set { serverId = value; }
		}

		public string ServerName
		{
			get { return serverName; }
			set { serverName = value; }
		}

		public string ServerUrl
		{
			get { return serverUrl; }
			set { serverUrl = value; }
		}

		public string Comments
		{
			get { return comments; }
			set { comments = value; }
		}

		public String Password
		{
			get { return password; }
			set { password = value; }
		}

		public bool VirtualServer
		{
			get { return virtualServer; }
			set { virtualServer = value; }
		}

		public string InstantDomainAlias
		{
			get { return instantDomainAlias; }
			set { instantDomainAlias = value; }
		}

		public bool ADEnabled
		{
			get { return this.adEnabled; }
			set { this.adEnabled = value; }
		}

		public string ADParentDomain
		{
			get { return adParentDomain; }
			set { adParentDomain = value; }
		}

		public string ADParentDomainController
		{
			get { return adParentDomainController; }
			set { adParentDomainController = value; }
		}

		public bool PasswordIsSHA256 { get; set; } = false;
	}
}
