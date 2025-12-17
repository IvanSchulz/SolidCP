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

namespace FuseCP.Providers.SharePoint
{
	[Serializable]
    public class SharePointSite : ServiceProviderItem
    {
        private string ownerLogin;
        private string ownerEmail;
        private string databaseServer;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string siteTemplate;
        private string databaseGroupName;
		private string applicationPool;
		private string rootFolder;
        private int localeID;

        [Persistent]
        public string OwnerLogin
        {
            get { return this.ownerLogin; }
            set { this.ownerLogin = value; }
        }

        [Persistent]
        public string OwnerEmail
        {
            get { return this.ownerEmail; }
            set { this.ownerEmail = value; }
        }

        [Persistent]
        public string DatabaseName
        {
            get { return this.databaseName; }
            set { this.databaseName = value; }
        }

        [Persistent]
        public string DatabaseUser
        {
            get { return this.databaseUser; }
            set { this.databaseUser = value; }
        }

        public string DatabaseServer
        {
            get { return this.databaseServer; }
            set { this.databaseServer = value; }
        }

        public string DatabasePassword
        {
            get { return this.databasePassword; }
            set { this.databasePassword = value; }
        }

        public string SiteTemplate
        {
            get { return this.siteTemplate; }
            set { this.siteTemplate = value; }
        }

        [Persistent]
        public string DatabaseGroupName
        {
            get { return this.databaseGroupName; }
            set { this.databaseGroupName = value; }
        }

		public string ApplicationPool
		{
			get { return this.applicationPool; }
			set { this.applicationPool = value; }
		}

		public string RootFolder
		{
			get { return this.rootFolder; }
			set { this.rootFolder = value; }
		}

        [Persistent]
        public int LocaleID
        {
            get { return this.localeID; }
            set { this.localeID = value; }
        }
    }
}
