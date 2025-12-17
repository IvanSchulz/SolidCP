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
    public class SfBTransaction
    {
        #region Fields

        private readonly List<TransactionAction> actions;

        #endregion

        #region Properties

        public List<TransactionAction> Actions
        {
            get { return actions; }
        }

        #endregion

        #region Constructor

        public SfBTransaction()
        {
            actions = new List<TransactionAction>();
        }

        #endregion

        #region Methods

        public void RegisterNewSipDomain(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewSipDomain, Id = id });
        }

        public void RegisterNewSimpleUrl(string sipDomain, string tenantID)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewSimpleUrl, Id = sipDomain, Account = tenantID });
        }

        public void RegisterNewConferencingPolicy(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewConferencingPolicy, Id = id });
        }

        public void RegisterNewCsExternalAccessPolicy(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewExternalAccessPolicy, Id = id });
        }

        public void RegisterNewCsMobilityPolicy(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewMobilityPolicy, Id = id });
        }

        public void RegisterNewCsUser(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewUser, Id = id });
        }

        #endregion
    }
}
