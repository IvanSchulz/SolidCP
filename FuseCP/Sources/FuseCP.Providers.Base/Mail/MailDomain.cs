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

namespace FuseCP.Providers.Mail
{ 
	[Serializable] 
	public class MailDomain : ServiceProviderItem
	{

		#region Smarter Mail 5.x String Constants
		//Domain Features
		public const string SMARTERMAIL5_SHOW_DOMAIN_REPORTS = "ShowDomainReports";
		public const string SMARTERMAIL5_SHOW_CALENDAR = "ShowCalendar";
		public const string SMARTERMAIL5_SHOW_CONTACTS = "ShowContacts";
		public const string SMARTERMAIL5_SHOW_TASKS = "ShowTasks";
		public const string SMARTERMAIL5_SHOW_NOTES = "ShowNotes";
		public const string SMARTERMAIL5_POP_RETRIEVAL = "ShowCalendar";
		public const string SMARTERMAIL5_POP_RETREIVAL_ENABLED = "EnablePopRetreival";
	    public const string SMARTERMAIL5_CATCHALLS_ENABLED = "EnableCatchAlls";
		//Domain Throttling
		public const string SMARTERMAIL5_MESSAGES_PER_HOUR = "MessagesPerHour";
		public const string SMARTERMAIL5_MESSAGES_PER_HOUR_ENABLED = "MessagesPerHourEnabled";
		public const string SMARTERMAIL5_BANDWIDTH_PER_HOUR = "BandwidthPerHour";
		public const string SMARTERMAIL5_BANDWIDTH_PER_HOUR_ENABLED = "BandwidthPerHourEnabled";
		public const string SMARTERMAIL5_BOUNCES_PER_HOUR = "BouncesReceivedPerHour";
		public const string SMARTERMAIL5_BOUNCES_PER_HOUR_ENABLED = "BouncesPerHourEnabled";
		//Domain Limits
		public const string SMARTERMAIL5_POP_RETREIVAL_ACCOUNTS = "PopRetreivalAccounts";
				
		#endregion

        #region Smarter Mail 6.x String Constants
        //Domain Features
        public const string SMARTERMAIL6_IMAP_RETREIVAL_ENABLED = "EnableImapRetreival";
        public const string SMARTERMAIL6_MAIL_SIGNING_ENABLED = "EnableMailSigning";
	    public const string SMARTERMAIL6_EMAIL_REPORTS_ENABLED = "EnableEmailReports";
        public const string SMARTERMAIL6_SYNCML_ENABLED = "EnableSyncML";
        #endregion 

        #region Smarter Mail 100.x String Constants
        public const string SMARTERMAIL100_MESSAGES_PER_HOUR_ACTION = "messagesAction";
        public const string SMARTERMAIL100_BANDWIDTH_PER_HOUR_ACTION = "bandwidthAction";
        public const string SMARTERMAIL100_BOUNCES_PER_HOUR_Action = "bouncesAction";
        public const string SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_TYPE = "blockedCountriesAtAuthType";
		public const string SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_COUNTRIES = "blockedCountriesAtAuthCountries";
        public const string SMARTERMAIL100_SET_BLOCKED_COUNTRIES = "blockedCountriesAtAuthSet";
        #endregion

        //license type
		public const string SMARTERMAIL_LICENSE_TYPE = "LicenseType";

		private string[] blackList = new string[0]; 
		private string redirectionHosts; 
		private bool redirectionActive; 
		private bool enabled; 
		private string postmasterAccount; 
		private string catchAllAccount; 
		private string abuseAccount;
	    private int maxPopRetrievalAccounts;

		public string RedirectionHosts
		{
			get { return this.redirectionHosts; }
			set { this.redirectionHosts = value; }
		}

		public string[] BlackList
		{
			get { return this.blackList; }
			set { this.blackList = value; }
		}

		public string CatchAllAccount
		{
			get { return this.catchAllAccount; }
			set { this.catchAllAccount = value; }
		}

		public string AbuseAccount
		{
			get { return this.abuseAccount; }
			set { this.abuseAccount = value; }
		}

		public bool RedirectionActive
		{
			get { return this.redirectionActive; }
			set { this.redirectionActive = value; }
		}

		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		public string PostmasterAccount
		{
			get { return this.postmasterAccount; }
			set { this.postmasterAccount = value; }
		}

		#region SmarterMail

		private string primaryDomainAdminUserName;
		private string primaryDomainAdminPassword;
		private string primaryDomainAdminFirstName;
		private string primaryDomainAdminLastName;

		private string serverIP;
		private string path;
		private int imapPort = 143;
		private int popPort = 110;
		private int smtpPort = 25;
	    private int smtpPortAlt;
	    private int ldapPort;
		private int maxAliases;
		private int maxDomainAliases;
		private int maxLists;
		private int maxDomainSizeInMB;
		private int maxDomainUsers;
		private int maxMailboxSizeInMB;
		private int maxMessageSize;
		private int maxRecipients;
		private bool requireSmtpAuthentication;
		private string listCommandAddress = "";
	    private bool isGlobalAddressList;
	    private bool sharedContacts;
	    private bool sharedNotes;
	    private bool sharedCalendars;
	    private bool sharedFolders;
	    private bool sharedTasks;
	    private bool bypassForwardBlackList;
	    private bool showstatsmenu;
	    private bool showspammenu;
	    private bool showlistmenu;
	    private bool showdomainaliasmenu;
	    private bool showcontentfilteringmenu;
		

	    public bool ShowsStatsMenu
	    {
	        get { return showstatsmenu; }
	        set { showstatsmenu = value; }
	    }

	    public bool ShowSpamMenu
	    {
	        get { return showspammenu; }
	        set { showspammenu = value; }
	    }

	    public bool ShowListMenu
	    {
	        get { return showlistmenu; }
	        set { showlistmenu = value; }
	    }

	    public bool ShowDomainAliasMenu
	    {
	        get { return showdomainaliasmenu; }
	        set { showdomainaliasmenu = value; }
	    }

	    public bool ShowContentFilteringMenu
	    {
	        get { return showcontentfilteringmenu; }
	        set { showcontentfilteringmenu = value; }
	    }

	    public bool BypassForwardBlackList
	    {
	        get { return bypassForwardBlackList; }
	        set { bypassForwardBlackList = value; }
	    }

	    public bool IsGlobalAddressList
	    {
	        get { return isGlobalAddressList; }
	        set { isGlobalAddressList = value; }
	    }

	    public bool SharedContacts
	    {
	        get { return sharedContacts; }
	        set { sharedContacts = value; }
	    }

	    public bool SharedNotes
	    {
	        get { return sharedNotes; }
	        set { sharedNotes = value; }
	    }

	    public bool SharedCalendars
	    {
	        get { return sharedCalendars; }
	        set { sharedCalendars = value; }
	    }

	    public bool SharedFolders
	    {
	        get { return sharedFolders; }
	        set { sharedFolders = value; }
	    }

	    public bool SharedTasks
	    {
	        get { return sharedTasks; }
	        set { sharedTasks = value; }
	    }

	    public string PrimaryDomainAdminUserName
		{
			get { return primaryDomainAdminUserName; }
			set { primaryDomainAdminUserName = value; }
		}

		public string PrimaryDomainAdminPassword
		{
			get { return primaryDomainAdminPassword; }
			set { primaryDomainAdminPassword = value; }
		}

		public string PrimaryDomainAdminFirstName
		{
			get { return primaryDomainAdminFirstName; }
			set { primaryDomainAdminFirstName = value; }
		}

		public string PrimaryDomainAdminLastName
		{
			get { return primaryDomainAdminLastName; }
			set { primaryDomainAdminLastName = value; }
		}

		public string ServerIP
		{
			get { return serverIP; }
			set { serverIP = value; }
		}

		public string Path
		{
			get { return path; }
			set { path = value; }
		}

	    public int SmtpPortAlt
	    {
	        get { return smtpPortAlt; }
	        set { smtpPortAlt = value; }
	    }

	    public int LdapPort
	    {
	        get { return ldapPort; }
	        set { ldapPort = value; }
	    }

	    public int ImapPort
		{
			get { return imapPort; }
			set { imapPort = value; }
		}

		public int PopPort
		{
			get { return popPort; }
			set { popPort = value; }
		}

		public int SmtpPort
		{
			get { return smtpPort; }
			set { smtpPort = value; }
		}

		public int MaxAliases
		{
			get { return maxAliases; }
			set { maxAliases = value; }
		}

		public int MaxDomainAliases
		{
			get { return maxDomainAliases; }
			set { maxDomainAliases = value; }
		}

		public int MaxLists
		{
			get { return maxLists; }
			set { maxLists = value; }
		}

	
        public int MaxDomainSizeInMB
		{
			get { return maxDomainSizeInMB; }
			set { maxDomainSizeInMB = value; }
		}
		        
        public int MaxDomainUsers
		{
			get { return maxDomainUsers; }
			set { maxDomainUsers = value; }
		}

		public int MaxMailboxSizeInMB
		{
			get { return maxMailboxSizeInMB; }
			set { maxMailboxSizeInMB = value; }
		}

		public int MaxMessageSize
		{
			get { return maxMessageSize; }
			set { maxMessageSize = value; }
		}

		public int MaxRecipients
		{
			get { return maxRecipients; }
			set { maxRecipients = value; }
		}

	    public int MaxPopRetrievalAccounts
	    {
            get { return maxPopRetrievalAccounts; }
            set { maxPopRetrievalAccounts = value; }
	    }


		public bool RequireSmtpAuthentication
		{
			get { return requireSmtpAuthentication; }
			set { requireSmtpAuthentication = value; }
		}

		public string ListCommandAddress
		{
			get { return listCommandAddress; }
			set { listCommandAddress = value; }
		}



		#endregion

        #region IceWarp

        public bool UseDomainDiskQuota { get; set; }
        public bool UseDomainLimits { get; set; }
        public bool UseUserLimits { get; set; }

	    public int MegaByteSendLimit { get; set; }
	    public int NumberSendLimit { get; set; }

        public int DefaultUserQuotaInMB { get; set; }
        public int DefaultUserMaxMessageSizeMegaByte { get; set; }
        public int DefaultUserMegaByteSendLimit { get; set; }
        public int DefaultUserNumberSendLimit { get; set; }

        #endregion
    } 
}
