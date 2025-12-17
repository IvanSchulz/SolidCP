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

namespace FuseCP.Providers.DNS
{
	/// <summary>
	/// Summary description for DnsSOARecord.
	/// </summary>
	public class DnsSOARecord : DnsRecord
	{
		private string primaryNsServer;
		private string primaryPerson;
		private string serialNumber;

		public DnsSOARecord()
		{
		}

		public string PrimaryNsServer
		{
			get { return this.primaryNsServer; }
			set { this.primaryNsServer = value; }
		}

		public string PrimaryPerson
		{
			get { return this.primaryPerson; }
			set { this.primaryPerson = value; }
		}

		public string SerialNumber
		{
			get { return this.serialNumber; }
			set { this.serialNumber = value; }
		}
	}
}
