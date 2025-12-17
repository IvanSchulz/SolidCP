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
	public class MailAccount : ServiceProviderItem
	{
		private bool enabled = true;
		private string password;
		private string replyTo;
		private bool responderEnabled;
		private string responderSubject;
		private string responderMessage;
		private string firstName; // SM
		private string lastName; // SM
		private bool deleteOnForward;
		private string[] forwardingAddresses;
		private string signature;
		private bool passwordLocked;
		private int maxMailboxSize;
		private bool changePassword;
		private bool isDomainAdmin;
		private bool isDomainAdminEnabled;
		private bool retainLocalCopy;
        private string signatureName;
        private string signatureGuid;
		private int signatureiD;

        public bool UnlimitedSize
		{
			get
			{
				return (maxMailboxSize < 0);
			}
            set
            {
                if (value)
                {
                    maxMailboxSize = 0;
                }
            }
		}

		public string ReplyTo
		{
			get { return this.replyTo; }
			set { this.replyTo = value; }
		}

		public string ResponderSubject
		{
			get { return this.responderSubject; }
			set { this.responderSubject = value; }
		}

		public string ResponderMessage
		{
			get { return this.responderMessage; }
			set { this.responderMessage = value; }
		}

		public bool ResponderEnabled
		{
			get { return this.responderEnabled; }
			set { this.responderEnabled = value; }
		}

		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		[Persistent]
		public string Password
		{
			get { return this.password; }
			set { this.password = value; }
		}

		#region SmarterMail

		/// <summary>
		/// First Name
		/// </summary>
		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		/// <summary>
		/// Last name
		/// </summary>
		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public bool DeleteOnForward
		{
			get { return deleteOnForward; }
			set { deleteOnForward = value; }
		}

		public string[] ForwardingAddresses
		{
			get { return forwardingAddresses; }
			set { forwardingAddresses = value; }
		}

		public string Signature
		{
			get { return signature; }
			set { signature = value; }
		}

		public string SignatureGuid
		{
			get { return signatureGuid; }
			set { signatureGuid = value; }
		}

		public string SignatureName
		{
			get { return signatureName; }
			set { signatureName = value; }
		}

		public int SignatureiD
		{
			get { return signatureiD; }
			set { signatureiD = value; }
		}

		public bool IsDomainAdminEnabled
		{
			get { return isDomainAdminEnabled; }
			set { isDomainAdminEnabled = value; }
		}

		public bool IsDomainAdmin
		{
			get { return isDomainAdmin; }
			set { isDomainAdmin = value; }
		}

		public bool PasswordLocked
		{
			get { return passwordLocked; }
			set { passwordLocked = value; }
		}

		public int MaxMailboxSize
		{
			get { return maxMailboxSize; }
			set { maxMailboxSize = value; }
		}

		public bool ChangePassword
		{
			get { return changePassword; }
			set { changePassword = value; }
		}

		#endregion

		#region MDaemon

		public bool RetainLocalCopy
		{
			get { return retainLocalCopy; }
			set { retainLocalCopy = value; }
		}

		#endregion

		#region hMailServer

		public bool SignatureEnabled { get; set; }
        public string SignatureHTML { get; set; }		
        public bool ForwardingEnabled { get; set; }
        public long Size { get; set; }
        public string LastLogonTime { get; set; }
        public long QuotaUsed { get; set; }
        public bool ResponderExpires {get;set;}
        public string ResponderExpirationDate { get; set; }
        
		#endregion

        #region IceWarp

        public int IceWarpAccountType { get; set; }
        public int IceWarpAccountState { get; set; }
	    public int IceWarpRespondType { get; set; }

        public bool RespondOnlyBetweenDates { get; set; }   // Added this because Calendar Control used did not allow null values
        public DateTime RespondFrom { get; set; }
        public DateTime RespondTo { get; set; }
        public string RespondWithReplyFrom { get; set; }
        public int RespondPeriodInDays { get; set; }

        public bool DeleteOlder { get; set; }
        public int DeleteOlderDays { get; set; }
        public bool ForwardOlder { get; set; }
        public int ForwardOlderDays { get; set; }
        public string ForwardOlderTo { get; set; }

        public int MaxMessageSizeMegaByte { get; set; }
        public int MegaByteSendLimit { get; set; }
        public int NumberSendLimit { get; set; }

        public string FullName { get; set; }
        #endregion
    }
}
