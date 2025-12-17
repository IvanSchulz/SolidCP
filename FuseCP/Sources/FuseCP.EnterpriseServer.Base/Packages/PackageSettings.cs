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
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for PackageSettings.
    /// </summary>
    public class PackageSettings
    {
        public const string INSTANT_ALIAS = "PreviewDomain";
        public const string SPACES_FOLDER = "ChildSpacesFolder";
        public const string NAME_SERVERS = "NameServers";
        public const string SHARED_SSL_SITES = "SharedSslSites";
		public const string EXCHANGE_SERVER = "ExchangeServer";
        public const string HOSTED_SOLLUTION = "HostedSollution";
        public const string VIRTUAL_PRIVATE_SERVERS = "VirtualPrivateServers";
        public const string VIRTUAL_PRIVATE_SERVERS_PROXMOX = "VirtualPrivateServersProxmox";
        public const string VIRTUAL_PRIVATE_SERVERS_2012 = "VirtualPrivateServers2012";

        public const string VIRTUAL_PRIVATE_SERVERS_FOR_PRIVATE_CLOUD = "VirtualPrivateServersForPrivateCloud";
        public int PackageId;
        public string SettingsName;

        private NameValueCollection settingsHash = null;
        public string[][] SettingsArray;

        [XmlIgnore, IgnoreDataMember]
        NameValueCollection Settings
        {
            get
            {
                if (settingsHash == null)
                {
                    // create new dictionary
                    settingsHash = new NameValueCollection();

                    // fill dictionary
                    if (SettingsArray != null)
                    {
                        foreach (string[] pair in SettingsArray)
                            settingsHash.Add(pair[0], pair[1]);
                    }
                }
                return settingsHash;
            }
        }

        [XmlIgnore, IgnoreDataMember]
        public string this[string settingName]
        {
            get
            {
                return Settings[settingName];
            }
            set
            {
                // set setting
                Settings[settingName] = value;

                // rebuild array
                SettingsArray = new string[Settings.Count][];
                for (int i = 0; i < Settings.Count; i++)
                {
                    SettingsArray[i] = new string[] { Settings.Keys[i], Settings[Settings.Keys[i]] };
                }
            }
        }

        public int GetInt(string settingName)
        {
            return Int32.Parse(Settings[settingName]);
        }

        public long GetLong(string settingName)
        {
            return Int64.Parse(Settings[settingName]);
        }

        public bool GetBool(string settingName)
        {
            return Boolean.Parse(Settings[settingName]);
        }
    }
}
