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

namespace FuseCP.Providers.HostedSolution
{
    public class ExchangeAccount
    {
        int accountId;
        int itemId;
        int packageId;
        string subscriberNumber;
        ExchangeAccountType accountType;
        string accountName;
        string displayName;
        string primaryEmailAddress;
        bool mailEnabledPublicFolder;
        MailboxManagerActions mailboxManagerActions;
        //string accountPassword;
        string samAccountName;
        int mailboxPlanId;
        string mailboxPlan;
        string publicFolderPermission;
        string userPrincipalName;
        string notes;
        int levelId;
        bool isVip;

        [LogProperty]
        public int AccountId
        {
            get { return this.accountId; }
            set { this.accountId = value; }
        }

        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }

        public int PackageId
        {
            get { return this.packageId; }
            set { this.packageId = value; }
        }

        public ExchangeAccountType AccountType
        {
            get { return this.accountType; }
            set { this.accountType = value; }
        }

        [LogProperty]
        public string AccountName
        {
            get { return this.accountName; }
            set { this.accountName = value; }
        }

        [LogProperty]
        public string SamAccountName
        {
            get { return this.samAccountName; }
            set { this.samAccountName = value; }
        }

        [LogProperty]
        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; }
        }

        [LogProperty("Email Address")]
        public string PrimaryEmailAddress
        {
            get { return this.primaryEmailAddress; }
            set { this.primaryEmailAddress = value; }
        }

        public bool MailEnabledPublicFolder
        {
            get { return this.mailEnabledPublicFolder; }
            set { this.mailEnabledPublicFolder = value; }
        }

        //public string AccountPassword
        //{
        //    get { return this.accountPassword; }
        //    set { this.accountPassword = value; }
        //}

        public MailboxManagerActions MailboxManagerActions
        {
            get { return this.mailboxManagerActions; }
            set { this.mailboxManagerActions = value; }
        }

        public int MailboxPlanId
        {
            get { return this.mailboxPlanId; }
            set { this.mailboxPlanId = value; }
        }

        public string MailboxPlan
        {
            get { return this.mailboxPlan; }
            set { this.mailboxPlan = value; }
        }


        public string SubscriberNumber
        {
            get { return this.subscriberNumber; }
            set { this.subscriberNumber = value; }
        }

        public string PublicFolderPermission
        {
            get { return this.publicFolderPermission; }
            set { this.publicFolderPermission = value; }
        }


        public string UserPrincipalName
        {
            get { return this.userPrincipalName; }
            set { this.userPrincipalName = value; }
        }

        public string Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }

        int archivingMailboxPlanId;
        public int ArchivingMailboxPlanId
        {
            get { return this.archivingMailboxPlanId; }
            set { this.archivingMailboxPlanId = value; }
        }

        string archivingMailboxPlan;
        public string ArchivingMailboxPlan
        {
            get { return this.archivingMailboxPlan; }
            set { this.archivingMailboxPlan = value; }
        }

        bool enableArchiving;
        public bool EnableArchiving
        {
            get { return this.enableArchiving; }
            set { this.enableArchiving = value; }
        }

        public bool IsVIP
        {
            get { return this.isVip; }
            set { this.isVip = value; }
        }

        public int LevelId
        {
            get { return this.levelId; }
            set { this.levelId = value; }
        }

        public bool Disabled { get; set; }

        public bool Locked { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(accountName) ? accountName : base.ToString();
        }
    }
}
