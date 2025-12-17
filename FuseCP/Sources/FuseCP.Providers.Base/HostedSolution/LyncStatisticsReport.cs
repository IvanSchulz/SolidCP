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

﻿using System.Text;

namespace FuseCP.Providers.HostedSolution
{
    public class LyncStatisticsReport : BaseReport<LyncUserStatistics>
    {
        public override string ToCSV()
        {
            StringBuilder mainBuilder = new StringBuilder();
            StringBuilder sb = null;
            AddCSVHeader(mainBuilder);
            foreach (LyncUserStatistics item in Items)
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

                sb.AppendFormat("{0},", ToCsvString(item.SipAddress));
                sb.AppendFormat("{0},", ToCsvString(item.PhoneNumber));
                sb.AppendFormat("{0},", ToCsvString(item.Conferencing));
                sb.AppendFormat("{0},", ToCsvString(item.EnterpriseVoice));
                sb.AppendFormat("{0},", ToCsvString(item.Federation));
                sb.AppendFormat("{0},", ToCsvString(item.InstantMessaing));
                sb.AppendFormat("{0},", ToCsvString(item.MobileAccess));
                sb.AppendFormat("{0},", ToCsvString(item.LyncUserPlan));

                mainBuilder.Append(sb.ToString());
            }

            return mainBuilder.ToString();

        }

        private static void AddCSVHeader(StringBuilder sb)
        {
            sb.Append("Top Reseller,Reseller,Customer,Customer Created,Hosting Space,Hosting Space Created,Ogranization Name,Organization Created,Organization ID,Display Name,SipAddress,PhoneNumber,Conferencing,EnterpriseVoice,Federation,InstantMessaging,MobileAccess");
        }
    }
}
