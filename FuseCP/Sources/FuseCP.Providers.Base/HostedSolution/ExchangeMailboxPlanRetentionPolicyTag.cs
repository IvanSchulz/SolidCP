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
    [Serializable]
    public class ExchangeMailboxPlanRetentionPolicyTag
    {
        int planTagID;
        [LogProperty]
        public int PlanTagID
        {
            get { return planTagID; }
            set { planTagID = value; }
        }

        int tagID;
        [LogProperty]
        public int TagID
        {
            get { return tagID; }
            set { tagID = value;  }
        }

        int mailboxPlanId;
        public int MailboxPlanId
        {
            get { return mailboxPlanId; }
            set { mailboxPlanId = value; }
        }

        string mailboxPlan;
        [LogProperty]
        public string MailboxPlan
        {
            get { return mailboxPlan; }
            set { mailboxPlan = value; }
        }

        string tagName;
        [LogProperty]
        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }


    }
}
