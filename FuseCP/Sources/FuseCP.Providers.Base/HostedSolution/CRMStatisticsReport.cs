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
    public class CRMStatisticsReport : BaseReport<CRMOrganizationStatistics>
    {
        public override string ToCSV()
        {
            StringBuilder mainBuilder = new StringBuilder();            
            AddCSVHeader(mainBuilder);
            foreach (CRMOrganizationStatistics item in Items)
            {
                StringBuilder  sb = new StringBuilder();
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

                sb.AppendFormat("{0},", ToCsvString(item.CRMOrganizationId.ToString()));
                sb.AppendFormat("{0},", ToCsvString(item.CRMUserName));
                sb.AppendFormat("{0},", ToCsvString(item.ClientAccessMode.ToString()));
				sb.AppendFormat("{0}", ToCsvString(!item.CRMDisabled));
                mainBuilder.Append(sb.ToString());
            }
            return mainBuilder.ToString();
        }

        private static void AddCSVHeader(StringBuilder sb)
        {
            sb.Append("Top Reseller,Reseller,Customer,Customer Created,Hosting Space,Hosting Space Created,Ogranization Name,Ogranization Created,Organization ID, CRM Organization ID, User Name, CAL, User State" );
        }
    }
}
