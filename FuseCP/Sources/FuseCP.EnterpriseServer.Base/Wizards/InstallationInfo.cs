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
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for InstallationInfo.
	/// </summary>
	[Serializable]
	public class InstallationInfo
	{
        private NameValueCollection propertiesHash = null;
        public string[][] PropertiesArray;

		private int packageId;
		private string applicationId;
		private int webSiteId;
		private string virtualDir;
        private int databaseId;
		private string databaseName;
        private string databaseGroup;
        private int userId;
		private string username;
		private string password;

		public InstallationInfo()
		{
		}

        [XmlIgnore, IgnoreDataMember]
        NameValueCollection Properties
        {
            get
            {
                if (propertiesHash == null)
                {
                    // create new dictionary
                    propertiesHash = new NameValueCollection();

                    // fill dictionary
                    if (PropertiesArray != null)
                    {
                        foreach (string[] pair in PropertiesArray)
                            propertiesHash.Add(pair[0], pair[1]);
                    }
                }
                return propertiesHash;
            }
        }

        [XmlIgnore, IgnoreDataMember]
        public string this[string propertyName]
        {
            get
            {
                return Properties[propertyName];
            }
            set
            {
                // set setting
                Properties[propertyName] = value;

                // rebuild array
                PropertiesArray = new string[Properties.Count][];
                for (int i = 0; i < Properties.Count; i++)
                {
                    PropertiesArray[i] = new string[] { Properties.Keys[i], Properties[Properties.Keys[i]] };
                }
            }
        }

        public int PackageId
        {
            get { return this.packageId; }
            set { this.packageId = value; }
        }

        public string ApplicationId
        {
            get { return this.applicationId; }
            set { this.applicationId = value; }
        }

        public int WebSiteId
        {
            get { return this.webSiteId; }
            set { this.webSiteId = value; }
        }

        public string VirtualDir
        {
            get { return this.virtualDir; }
            set { this.virtualDir = value; }
        }

        public int DatabaseId
        {
            get { return this.databaseId; }
            set { this.databaseId = value; }
        }

        public string DatabaseName
        {
            get { return this.databaseName; }
            set { this.databaseName = value; }
        }

        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public string DatabaseGroup
        {
            get { return this.databaseGroup; }
            set { this.databaseGroup = value; }
        }
	}
}
