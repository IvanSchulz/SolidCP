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
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FuseCP.Providers.FTP
{
	/// <summary>
	/// Summary description for FtpDirectoryItem.
	/// </summary>
	[Serializable]
	public class FtpAccount : ServiceProviderItem
	{
		private bool canRead;
		private bool canWrite;
		private string folder;
		private string password;
		private bool enabled;

		public FtpAccount()
		{
		}

		[Persistent]
		public bool CanRead
		{
			get { return canRead; }
			set { canRead = value; }
		}

		[Persistent]
		public bool CanWrite
		{
			get { return canWrite; }
			set { canWrite = value; }
		}

		[Persistent]
		public string Folder
		{
			get { return folder; }
			set { folder = value; }
		}

		[Persistent]
		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		[XmlIgnore, IgnoreDataMember]
		public string VirtualPath
		{
			get
			{
				return String.Format("/{0}", Name);
			}
		}
	}
}
