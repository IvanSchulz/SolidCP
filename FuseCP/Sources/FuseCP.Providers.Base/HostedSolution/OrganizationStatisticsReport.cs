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
    public class OrganizationStatisticsReport : BaseReport<OrganizationStatisticsRepotItem>
    {
        public override string ToCSV()
        {
            StringBuilder mainBuilder = new StringBuilder();
            
            AddCSVHeader(mainBuilder);
            foreach (OrganizationStatisticsRepotItem item in Items)
            {
                StringBuilder sb = new StringBuilder();
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

                sb.AppendFormat("{0},", ToCsvString(item.TotalMailboxes));
                sb.AppendFormat("{0},", ToCsvString(item.TotalMailboxesSize / 1024.0 / 1024.0));
                sb.AppendFormat("{0},", ToCsvString(item.TotalPublicFoldersSize / 1024.0 / 1024.0));
                sb.AppendFormat("{0},", ToCsvString(item.TotalSharePointSiteCollections));
                sb.AppendFormat("{0},", ToCsvString(item.TotalSharePointSiteCollectionsSize / 1024.0 / 1024.0));
                sb.AppendFormat("{0},", ToCsvString(item.TotalCRMUsers));
                sb.AppendFormat("{0},", ToCsvString(item.TotalLyncUsers));
                sb.AppendFormat("{0}", ToCsvString(item.TotalLyncEVUsers));
                sb.AppendFormat("{0},", ToCsvString(item.TotalSfBUsers));
                sb.AppendFormat("{0}", ToCsvString(item.TotalSfBEVUsers));

                mainBuilder.Append(sb.ToString());
            }
            return mainBuilder.ToString();
        }

        private static void AddCSVHeader(StringBuilder sb)
        {
            sb.Append("Top Reseller,Reseller,Customer,Customer Created,Hosting Space,Hosting Space Created,Ogranization Name,Ogranization Created,Organization ID,Total mailboxes,Total mailboxes size(Mb),Total Public Folders size(Mb),Total SharePoint site collections,Total SharePoint site collections size(Mb),Total CRM users,Total Lync users,Total Lync EV users, Total SfB users,Total SfB EV users");
        }
    }
}
