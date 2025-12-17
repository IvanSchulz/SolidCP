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

    internal class MessagesElement : ConfigurationElement
    {
        public bool AllowLocalDetailedErrors
        {
            get
            {
                return (bool) base["allowLocalDetailedErrors"];
            }
            set
            {
                base["allowLocalDetailedErrors"] = value;
            }
        }

        public string BannerMessage
        {
            get
            {
                return (string) base["bannerMessage"];
            }
            set
            {
                base["bannerMessage"] = value;
            }
        }

        public string ExitMessage
        {
            get
            {
                return (string) base["exitMessage"];
            }
            set
            {
                base["exitMessage"] = value;
            }
        }

        public bool ExpandVariables
        {
            get
            {
                return (bool) base["expandVariables"];
            }
            set
            {
                base["expandVariables"] = value;
            }
        }

        public string GreetingMessage
        {
            get
            {
                return (string) base["greetingMessage"];
            }
            set
            {
                base["greetingMessage"] = value;
            }
        }

        public string MaxClientsMessage
        {
            get
            {
                return (string) base["maxClientsMessage"];
            }
            set
            {
                base["maxClientsMessage"] = value;
            }
        }

        public bool SuppressDefaultBanner
        {
            get
            {
                return (bool) base["suppressDefaultBanner"];
            }
            set
            {
                base["suppressDefaultBanner"] = value;
            }
        }
    }
}

