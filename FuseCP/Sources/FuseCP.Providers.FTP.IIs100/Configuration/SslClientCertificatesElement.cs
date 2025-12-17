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
    using System;

    internal class SslClientCertificatesElement : ConfigurationElement
    {
        public bool Enabled
        {
            get
            {
                return (bool) base["enabled"];
            }
            set
            {
                base["enabled"] = value;
            }
        }

        public TimeSpan RevocationFreshnessTime
        {
            get
            {
                return (TimeSpan) base["revocationFreshnessTime"];
            }
            set
            {
                base["revocationFreshnessTime"] = value;
            }
        }

        public TimeSpan RevocationUrlRetrievalTimeout
        {
            get
            {
                return (TimeSpan) base["revocationUrlRetrievalTimeout"];
            }
            set
            {
                base["revocationUrlRetrievalTimeout"] = value;
            }
        }

        public bool UseAdMapper
        {
            get
            {
                return (bool) base["useAdMapper"];
            }
            set
            {
                base["useAdMapper"] = value;
            }
        }

        public ValidationFlags ValidationFlags
        {
            get
            {
                return (ValidationFlags) base["validationFlags"];
            }
            set
            {
                base["validationFlags"] = (int) value;
            }
        }
    }
}

