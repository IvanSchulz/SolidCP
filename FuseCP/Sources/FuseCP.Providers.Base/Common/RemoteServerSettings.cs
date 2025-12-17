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
using System.DirectoryServices;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Providers
{
    public class RemoteServerSettings
    {
        // Active Directory settings
        private bool adEnabled;
        private AuthenticationTypes adAuthenticationType;
        private string adRootDomain;
        private string adUsername;
        private string adPassword;
        private string adParentDomain;
        private string adParentDomainController;

        // Server settings
        private int serverId;
        private string serverName;

        public RemoteServerSettings()
        {
            // just do nothing
        }

        public RemoteServerSettings(string[] settings)
        {
            // parse settings array
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                string key = setting.Substring(0, idx);
                string val = setting.Substring(idx + 1);

                if (key == "AD:Enabled")
                    ADEnabled = Boolean.Parse(val);
                else if (key == "AD:AuthenticationType")
                    ADAuthenticationType = (AuthenticationTypes)Enum.Parse(typeof(AuthenticationTypes), val, true);
                else if (key == "AD:RootDomain")
                    ADRootDomain = val;
                else if (key == "AD:Username")
                    ADUsername = val;
                else if (key == "AD:Password")
                    ADPassword = val;
                else if (key == "Server:ServerId")
                    ServerId = Int32.Parse(val);
                else if (key == "Server:ServerName")
                    ServerName = val;
                else if (key == "AD:ParentDomain")
                    ADParentDomain = val;
                else if (key == "AD:ParentDomainController")
                    ADParentDomainController = val;
            }
        }

        #region Public Properties
        public bool ADEnabled
        {
            get { return this.adEnabled; }
            set { this.adEnabled = value; }
        }

        public AuthenticationTypes ADAuthenticationType
        {
            get { return this.adAuthenticationType; }
            set { this.adAuthenticationType = value; }
        }

        public string ADRootDomain
        {
            get { return this.adRootDomain; }
            set { this.adRootDomain = value; }
        }

        public string ADUsername
        {
            get { return this.adUsername; }
            set { this.adUsername = value; }
        }

        public string ADPassword
        {
            get { return this.adPassword; }
            set { this.adPassword = value; }
        }

        public int ServerId
        {
            get { return this.serverId; }
            set { this.serverId = value; }
        }

        public string ServerName
        {
            get { return this.serverName; }
            set { this.serverName = value; }
        }

        public string ADParentDomain
        {
            get { return this.adParentDomain; }
            set { this.adParentDomain = value; }
        }

        public string ADParentDomainController
        {
            get { return this.adParentDomainController; }
            set { this.adParentDomainController = value; }
        }
        #endregion
    }
}
