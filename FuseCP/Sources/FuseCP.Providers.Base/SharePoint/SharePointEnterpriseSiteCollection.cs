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
	/// <summary>
	/// Represents SharePoint site collection information.
	/// </summary>
	[Serializable]
	public class SharePointEnterpriseSiteCollection : ServiceProviderItem
	{
		private int organizationId;
		private string url;
		private string physicalAddress;
		private string ownerLogin;
		private string ownerName;
		private string ownerEmail;
		private int localeId;
		private string title;
		private string description;
		private long bandwidth;
		private long diskspace;
	    private long maxSiteStorage;
	    private long warningStorage;
        private string rootWebApplicationInteralIpAddress;
        private string rootWebApplicationFQDN;



	    [Persistent]
        public long MaxSiteStorage
	    {
	        get { return maxSiteStorage; }
	        set { maxSiteStorage = value; }
	    }

        [Persistent]
        public long WarningStorage
	    {
	        get { return warningStorage; }
	        set { warningStorage = value; }
	    }

	    /// <summary>
		/// Gets or sets service item name.
		/// </summary>
		public override string Name
		{
			get
			{
				return this.Url;
			}
			set
			{
				this.Url = value;
			}
		}

		/// <summary>
		/// Gets or sets id of organization which owns this site collection.
		/// </summary>
		[Persistent]
		public int OrganizationId
		{
			get
			{
				return this.organizationId;
			}
			set
			{
				this.organizationId = value;
			}
		}

		/// <summary>
		/// Gets or sets url of the host named site collection to be created. It must not contain port number.
		/// </summary>
		[Persistent]
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}

		/// <summary>
		/// Gets or sets physical address of the host named site collection. It contains scheme and port number.
		/// </summary>
		[Persistent]
		public string PhysicalAddress
		{
			get
			{
				return this.physicalAddress;
			}
			set
			{
				this.physicalAddress = value;
			}
		}

		/// <summary>
		/// Gets or sets login name of the site collection's owner/primary site administrator.
		/// </summary>
		[Persistent]
		public string OwnerLogin
		{
			get
			{
				return this.ownerLogin;
			}
			set
			{
				this.ownerLogin = value;
			}
		}

		/// <summary>
		/// Gets or sets display name of the site collection's owner/primary site administrator.
		/// </summary>
		[Persistent]
		public string OwnerName
		{
			get
			{
				return this.ownerName;
			}
			set
			{
				this.ownerName = value;
			}
		}

		/// <summary>
		/// Gets or sets display email of the site collection's owner/primary site administrator.
		/// </summary>
		[Persistent]
		public string OwnerEmail
		{
			get
			{
				return this.ownerEmail;
			}
			set
			{
				this.ownerEmail = value;
			}
		}

        /// <summary>
        /// Gets or sets the internal ip address
        /// </summary>
        [Persistent]
        public string RootWebApplicationInteralIpAddress
        {
            get
            {
                return this.rootWebApplicationInteralIpAddress;
            }
            set
            {
                this.rootWebApplicationInteralIpAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the internal ip address
        /// </summary>
        [Persistent]
        public string RootWebApplicationFQDN
        {
            get
            {
                return this.rootWebApplicationFQDN;
            }
            set
            {
                this.rootWebApplicationFQDN = value;
            }
        }


		/// <summary>
		/// Gets or sets locale id of the site collection to be created.
		/// </summary>
		[Persistent]
		public int LocaleId
		{
			get
			{
				return this.localeId;
			}
			set
			{
				this.localeId = value;
			}
		}

		/// <summary>
		/// Gets or sets title of the the site collection to be created.
		/// </summary>
		[Persistent]
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
			}
		}

		/// <summary>
		/// Gets or sets description of the the site collection to be created.
		/// </summary>
		[Persistent]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		/// <summary>
		/// Gets or sets bandwidth of the the site collection.
		/// </summary>
		[Persistent]
		public long Bandwidth
		{
			get
			{
				return this.bandwidth;
			}
			set
			{
				this.bandwidth = value;
			}
		}

		/// <summary>
		/// Gets or sets diskspace of the the site collection.
		/// </summary>
		[Persistent]
		public long Diskspace
		{
			get
			{
				return this.diskspace;
			}
			set
			{
				this.diskspace = value;
			}
		}
	
	}
}
