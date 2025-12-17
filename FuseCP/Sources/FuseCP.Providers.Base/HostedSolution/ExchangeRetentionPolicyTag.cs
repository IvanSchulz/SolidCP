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
using System.Text.RegularExpressions;

namespace FuseCP.Providers.HostedSolution
{
    public class ExchangeRetentionPolicyTag
    {
        int tagID;

        [LogProperty]
        public int TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }

        int itemID;
        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        string tagName;

        [LogProperty]
        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }

        int tagType;

        [LogProperty]
        public int TagType
        {
            get { return tagType; }
            set { tagType = value; }
        }

        int ageLimitForRetention;

        [LogProperty("Tag Age Limit For Retention")]
        public int AgeLimitForRetention
        {
            get { return ageLimitForRetention; }
            set { ageLimitForRetention = value;}
        }

        int retentionAction;

        [LogProperty("Tag Retention Action")]
        public int RetentionAction
        {
            get { return retentionAction; }
            set { retentionAction = value; }
        }

        [LogProperty("Tag Unique Name")]
        public string FCPUniqueName
        {
            get
            {
                Regex r = new Regex(@"[^A-Za-z0-9]");
                return "FCPPolicyTag" + TagID.ToString() + "_" + r.Replace(TagName, "");
            }
        }
    }
}
