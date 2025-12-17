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

namespace FuseCP.Providers.FTP.IIs100.Config
{
    using Microsoft.Web.Administration;

    internal class AuthenticationElement : ConfigurationElement
    {
        private AnonymousAuthenticationElement _anonymousAuthentication;
        private BasicAuthenticationElement _basicAuthentication;
        private CustomAuthenticationElement _customAuthentication;

        public AnonymousAuthenticationElement AnonymousAuthentication
        {
            get
            {
                if (this._anonymousAuthentication == null)
                {
                    this._anonymousAuthentication = (AnonymousAuthenticationElement) base.GetChildElement("anonymousAuthentication", typeof(AnonymousAuthenticationElement));
                }
                return this._anonymousAuthentication;
            }
        }

        public BasicAuthenticationElement BasicAuthentication
        {
            get
            {
                if (this._basicAuthentication == null)
                {
                    this._basicAuthentication = (BasicAuthenticationElement) base.GetChildElement("basicAuthentication", typeof(BasicAuthenticationElement));
                }
                return this._basicAuthentication;
            }
        }

        public CustomAuthenticationElement CustomAuthentication
        {
            get
            {
                if (this._customAuthentication == null)
                {
                    this._customAuthentication = (CustomAuthenticationElement) base.GetChildElement("customAuthentication", typeof(CustomAuthenticationElement));
                }
                return this._customAuthentication;
            }
        }
    }
}

