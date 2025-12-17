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

﻿using System.Collections.Generic;
using System.Text;

namespace FuseCP.Providers.HostedSolution
{
    public class ExchangeStatisticsReport : BaseReport<ExchangeMailboxStatistics>
    {
    
        public override string ToCSV()
        {
            StringBuilder mainBuilder = new StringBuilder();
            StringBuilder sb = null;
            AddCSVHeader(mainBuilder);
            foreach(ExchangeMailboxStatistics item in Items)
            {
                sb = new StringBuilder();
				sb.Append("\n");
				sb.AppendFormat("{0},", ToCsvString(item.TopResellerName));
				sb.AppendFormat("{0},", ToCsvString(item.ResellerName));
				sb.AppendFormat("{0},", ToCsvString(item.CustomerName));
				sb.AppendFormat("{0},", ToCsvString(item.CustomerCreated));
				sb.AppendFormat("{0},", ToCsvString(item.HostingSpace));
                sb.AppendFormat("{0},", ToCsvString(item.HostingSpaceCreated));
				sb.AppendFormat("{0},", ToCsvString(item.OrganizationName));
				sb.AppendFormat("{0},", ToCsvString(item.OrganizationCreated));
				sb.AppendFormat("{0},", ToCsvString(item.OrganizationID));

				sb.AppendFormat("{0},", ToCsvString(item.DisplayName));
				sb.AppendFormat("{0},", ToCsvString(item.AccountCreated));
				sb.AppendFormat("{0},", ToCsvString(item.PrimaryEmailAddress));
				sb.AppendFormat("{0},", ToCsvString(item.MAPIEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.OWAEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.ActiveSyncEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.POPEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.IMAPEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.TotalSize / 1024.0 / 1024.0));
				sb.AppendFormat("{0},", UnlimitedToCsvString(item.MaxSize == -1 ? (double)item.MaxSize : item.MaxSize / 1024.0 / 1024.0));
				sb.AppendFormat("{0},", ToCsvString(item.LastLogon));
				sb.AppendFormat("{0},", ToCsvString(item.Enabled, "Enabled", "Disabled"));
				sb.AppendFormat("{0},", ToCsvString(item.MailboxType));
                sb.AppendFormat("{0},", ToCsvString(item.BlackberryEnabled));
                sb.AppendFormat("{0}", ToCsvString(item.MailboxPlan));
                mainBuilder.Append(sb.ToString());
            }
            return mainBuilder.ToString();
        }

		private void AddCSVHeader(StringBuilder sb)
		{
            sb.Append("Top Reseller,Reseller,Customer,Customer Created,Hosting Space,Hosting Space Created,Ogranization Name,Organization Created,Organization ID,Mailbox Display Name,Account Created,Primary E-mail Address,MAPI,OWA,ActiveSync,POP 3,IMAP,Mailbox Size (Mb),Max Mailbox Size (Mb),Last Logon,Enabled,Mailbox Type, BlackBerry, Mailbox Plan");
		}
    }
}
