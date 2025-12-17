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
    public class ExchangeDisclaimer
    {
        int exchangeDisclaimerId;
        int itemId;
        string disclaimerName;
        string disclaimerText;

        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }

        [LogProperty("Disclaimer ID")]
        public int ExchangeDisclaimerId
        {
            get { return this.exchangeDisclaimerId; }
            set { this.exchangeDisclaimerId = value; }
        }

        [LogProperty]
        public string DisclaimerName
        {
            get { return this.disclaimerName; }
            set { this.disclaimerName = value; }
        }

        [LogProperty]
        public string DisclaimerText
        {
            get { return this.disclaimerText; }
            set { this.disclaimerText = value; }
        }

        [LogProperty("Disclaimer Unique Name")]
        public string FCPUniqueName
        {
            get
            {
                Regex r = new Regex(@"[^A-Za-z0-9]");
                return "FCPDisclaimer" + ExchangeDisclaimerId.ToString() + "_" + r.Replace(DisclaimerName, "");
            }
        }

    }
}
