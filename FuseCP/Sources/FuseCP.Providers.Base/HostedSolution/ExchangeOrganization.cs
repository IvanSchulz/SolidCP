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
    public class ExchangeOrganization
    {
        int itemId;
        int exchangeMailboxPlanID;
        int lyncUserPlanID;
        int sfbUserPlanID;


        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }


        public int ExchangeMailboxPlanID
        {
            get { return this.exchangeMailboxPlanID; }
            set { this.exchangeMailboxPlanID = value; }
        }

        public int LyncUserPlanID
        {
            get { return this.lyncUserPlanID; }
            set { this.lyncUserPlanID = value; }
        }
        public int SfBUserPlanID
        {
            get { return this.sfbUserPlanID; }
            set { this.sfbUserPlanID = value; }
        }

    }
}
