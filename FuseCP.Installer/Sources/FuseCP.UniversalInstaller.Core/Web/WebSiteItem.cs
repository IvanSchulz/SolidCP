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

namespace FuseCP.UniversalInstaller.Web
{
	/// <summary>
	/// Web site item.
	/// </summary>
	[Serializable]
	public sealed class WebSiteItem : WebVirtualDirectoryItem
	{
		private string siteId;
		private string siteIPAddress;
		private string logFileDirectory;
		private ServerBinding[] bindings;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public WebSiteItem()
		{
		}

		/// <summary>
		/// Site id.
		/// </summary>
		public string SiteId
		{
			get { return siteId; }
			set { siteId = value; }
		}

		/// <summary>
		/// Site IP address.
		/// </summary>
		public string SiteIPAddress
		{
			get { return siteIPAddress; }
			set { siteIPAddress = value; }
		}

		/// <summary>
		/// Site log file directory.
		/// </summary>
		public string LogFileDirectory
		{
			get { return logFileDirectory; }
			set { logFileDirectory = value; }
		}

		/// <summary>
		/// Site bindings.
		/// </summary>
		public ServerBinding[] Bindings
		{
			get { return bindings; }
			set { bindings = value; }
		}
	}
}
