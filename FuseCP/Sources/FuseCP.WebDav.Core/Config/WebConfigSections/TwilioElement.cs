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

using System.Configuration;

namespace FuseCP.WebDav.Core.Config.WebConfigSections
{
    public class TwilioElement : ConfigurationElement
    {
        private const string AccountSidPropName = "accountSid";
        private const string AuthorizationTokenPropName = "authorizationToken";
        private const string PhoneFromPropName = "phoneFrom";

        [ConfigurationProperty(AccountSidPropName, IsKey = true, IsRequired = true)]
        public string AccountSid
        {
            get { return this[AccountSidPropName].ToString(); }
            set { this[AccountSidPropName] = value; }
        }

        [ConfigurationProperty(AuthorizationTokenPropName, IsKey = true, IsRequired = true)]
        public string AuthorizationToken
        {
            get { return this[AuthorizationTokenPropName].ToString(); }
            set { this[AuthorizationTokenPropName] = value; }
        }

        [ConfigurationProperty(PhoneFromPropName, IsKey = true, IsRequired = true)]
        public string PhoneFrom
        {
            get { return this[PhoneFromPropName].ToString(); }
            set { this[PhoneFromPropName] = value; }
        }
    }
}
