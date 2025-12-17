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
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace FuseCP.Providers.OS
{
	public class CertificateInfo
	{
		public StoreLocation Location { get; set; }
		public StoreName Name { get; set; }
		public X509FindType FindType { get; set; }
		public string FindValue { get; set; }

		public byte[] File { get; set; }
		public string Password { get; set; }
		
		public string LetsEncryptHosts { get; set; }
		public string LetsEncryptEmail { get; set; }
	}
}
