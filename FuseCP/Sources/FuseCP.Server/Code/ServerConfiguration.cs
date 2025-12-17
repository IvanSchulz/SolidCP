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
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace FuseCP.Server
{
    /// <summary>
    /// Summary description for ServerConfiguration
    /// </summary>
    public class ServerConfiguration : IConfigurationSectionHandler
    {
        #region Public Properties
        private static SecuritySettings security = new SecuritySettings();

        public static SecuritySettings Security
        {
            get
            {
                return security;
            }
        }
        #endregion

        private ServerConfiguration()
        {
        }

        static ServerConfiguration()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            System.Configuration.ConfigurationManager.GetSection("FuseCP.server");
        }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // parse "security" section
            XmlNode nodeSecurity = section.SelectSingleNode("security");
            if (nodeSecurity == null)
                throw new Exception("'FuseCP/security' section is missing");

            security = new SecuritySettings();
            security.ParseSection(nodeSecurity);

            return null;
        }

        #region Inner Classes
        public class SecuritySettings
        {
            private bool securityEnabled;
            private string password;

            public bool SecurityEnabled
            {
                get { return this.securityEnabled; }
                set { this.securityEnabled = value; }
            }

            public string Password
            {
                get { return this.password; }
                set { this.password = value; }
            }

            public void ParseSection(XmlNode section)
            {
                // enabled
                XmlNode nodeEnabled = section.SelectSingleNode("enabled");
                if (nodeEnabled == null)
                    throw new Exception("'FuseCP/security/enabled' node is missing");

                if (nodeEnabled.Attributes["value"] == null)
                    throw new Exception("'FuseCP/security/enabled/@value' attribute is missing");

                securityEnabled = true;
                Boolean.TryParse(nodeEnabled.Attributes["value"].Value, out securityEnabled);

                // password
                XmlNode nodePassword = section.SelectSingleNode("password");
                if (nodePassword == null)
                    throw new Exception("'FuseCP/security/password' node is missing");

                if (nodePassword.Attributes["value"] == null)
                    throw new Exception("'FuseCP/security/password/@value' attribute is missing");

                password = nodePassword.Attributes["value"].Value;
            }
        }
        #endregion
    }
}
