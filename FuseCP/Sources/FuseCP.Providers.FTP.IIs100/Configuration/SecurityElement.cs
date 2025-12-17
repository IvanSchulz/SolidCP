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

    internal class SecurityElement : ConfigurationElement
    {
        private AuthenticationElement _authentication;
        private SslElement _ssl;
        private SslClientCertificatesElement _sslClientCertificates;

        public AuthenticationElement Authentication
        {
            get
            {
                if (this._authentication == null)
                {
                    this._authentication = (AuthenticationElement) base.GetChildElement("authentication", typeof(AuthenticationElement));
                }
                return this._authentication;
            }
        }

        public SslElement Ssl
        {
            get
            {
                if (this._ssl == null)
                {
                    this._ssl = (SslElement) base.GetChildElement("ssl", typeof(SslElement));
                }
                return this._ssl;
            }
        }

        public SslClientCertificatesElement SslClientCertificates
        {
            get
            {
                if (this._sslClientCertificates == null)
                {
                    this._sslClientCertificates = (SslClientCertificatesElement) base.GetChildElement("sslClientCertificates", typeof(SslClientCertificatesElement));
                }
                return this._sslClientCertificates;
            }
        }
    }
}

