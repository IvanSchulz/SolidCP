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
using System.Linq;
using System.Text;

namespace FuseCP.Providers.HostedSolution
{
    public class TransactionAction
    {
        private TransactionActionTypes actionType;

        public TransactionActionTypes ActionType
        {
            get { return actionType; }
            set { actionType = value; }
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string suffix;

        public string Suffix
        {
            get { return suffix; }
            set { suffix = value; }
        }

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; }

        }
        
        public string[] Accounts { get; set; }
        public ExchangeAccount ExchangeAccount { get; set; }

        public enum TransactionActionTypes
        {
            CreateOrganizationUnit,
            CreateGlobalAddressList,
            CreateAddressList,
            CreateAddressBookPolicy,
            CreateOfflineAddressBook,
            CreateDistributionGroup,
            EnableDistributionGroup,
            CreateAcceptedDomain,
            AddUPNSuffix,
            CreateMailbox,
            CreateContact,
            CreatePublicFolder,
            CreatePublicFolderMailbox,
            CreateActiveSyncPolicy,
            AddMailboxFullAccessPermission,
            AddSendAsPermission,
            RemoveMailboxFullAccessPermission,
            RemoveSendAsPermission,
            EnableMailbox,
            LyncNewSipDomain,
            LyncNewSimpleUrl,
            LyncNewUser,
            LyncNewConferencingPolicy,
            LyncNewExternalAccessPolicy,
            LyncNewMobilityPolicy,
            SfBNewSipDomain,
            SfBNewSimpleUrl,
            SfBNewUser,
            SfBNewConferencingPolicy,
            SfBNewExternalAccessPolicy,
            SfBNewMobilityPolicy,
            ResetMailboxOnBehalfPermissions,
            RemoveMailboxFolderPermissions,
            AddMailboxFolderPermission
        };
    }
}
