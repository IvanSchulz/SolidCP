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

using System.Collections.Generic;

namespace FuseCP.Providers.HostedSolution
{
    public class ExchangeTransaction
    {
        List<TransactionAction> actions = null;

        public ExchangeTransaction()
        {
            actions = new List<TransactionAction>();
        }

        public List<TransactionAction> Actions
        {
            get { return actions; }
        }

        public void RegisterNewOrganizationUnit(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateOrganizationUnit;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewDistributionGroup(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateDistributionGroup;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterMailEnabledDistributionGroup(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.EnableDistributionGroup;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewGlobalAddressList(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateGlobalAddressList;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewAddressList(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateAddressList;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewAddressBookPolicy(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateAddressBookPolicy;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterNewRoomsAddressList(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateAddressList;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterNewOfflineAddressBook(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateOfflineAddressBook;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewActiveSyncPolicy(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateActiveSyncPolicy;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterNewAcceptedDomain(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateAcceptedDomain;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewUPNSuffix(string id, string suffix)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.AddUPNSuffix;
            action.Id = id;
            action.Suffix = suffix;
            Actions.Add(action);
        }

        public void RegisterNewMailbox(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateMailbox;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterEnableMailbox(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.EnableMailbox;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterNewContact(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateContact;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewPublicFolder(string mailbox, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreatePublicFolder;
            action.Id = id;
            action.Account = mailbox;
            Actions.Add(action);
        }


        public void RegisterNewPublicFolderMailbox(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreatePublicFolderMailbox;
            action.Id = id;
            Actions.Add(action);
        }

        public void ResetMailboxOnBehalfPermissions(string id, string[] accounts)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.ResetMailboxOnBehalfPermissions;
            action.Accounts = accounts;
            action.Id = id;
            Actions.Add(action);
        }

        public void AddMailBoxFullAccessPermission(string accountName, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.AddMailboxFullAccessPermission;
            action.Id = id;
            action.Account = accountName;
            Actions.Add(action);
        }

        public void AddSendAsPermission(string accountName, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.AddSendAsPermission;
            action.Id = id;
            action.Account = accountName;
            Actions.Add(action);
        }

        public void RemoveMailboxFullAccessPermission(string accountName, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.RemoveMailboxFullAccessPermission;
            action.Id = id;
            action.Account = accountName;
            Actions.Add(action);
        }

        public void RemoveSendAsPermission(string accountName, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.RemoveSendAsPermission;
            action.Id = id;
            action.Account = accountName;
            Actions.Add(action);
        }

        public void RemoveMailboxFolderPermissions(string folderPath, ExchangeAccount account)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.RemoveMailboxFolderPermissions;
            action.Id = folderPath;
            action.ExchangeAccount = account;
            Actions.Add(action);
        }

        public void AddMailboxFolderPermission(string folderPath, ExchangeAccount account)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.AddMailboxFolderPermission;
            action.Id = folderPath;
            action.ExchangeAccount = account;
            Actions.Add(action);
        }
    }
}
