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
using System.Collections.Specialized;
//using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace FuseCP.Providers
{
    /// <summary>
    /// Summary description for ServiceProviderSettings.
    /// </summary>
    [DataContract]
    public class ServiceProviderSettingsSoapHeader
	{
        [DataMember]
        public string[] Settings;

        /// <summary>
        /// This property is just a flag telling us that this SOAP header should be encrypted.
        /// </summary>
        [XmlAttribute("SecureHeader", Namespace = "http://smbsaas/fusecp/server/")]
        [DataMember]
        public bool SecureHeader;
	}
}
