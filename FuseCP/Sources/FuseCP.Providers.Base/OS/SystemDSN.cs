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

namespace FuseCP.Providers.OS
{
    public class SystemDSN : ServiceProviderItem
    {
        private string driver;
        private string databaseServer;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;

        [Persistent]
        public string Driver
        {
            get { return this.driver; }
            set { this.driver = value; }
        }

        public string DatabaseServer
        {
            get { return this.databaseServer; }
            set { this.databaseServer = value; }
        }

        public string DatabaseUser
        {
            get { return this.databaseUser; }
            set { this.databaseUser = value; }
        }

        [Persistent]
        public string DatabasePassword
        {
            get { return this.databasePassword; }
            set { this.databasePassword = value; }
        }

        public string DatabaseName
        {
            get { return this.databaseName; }
            set { this.databaseName = value; }
        }
    }
}
