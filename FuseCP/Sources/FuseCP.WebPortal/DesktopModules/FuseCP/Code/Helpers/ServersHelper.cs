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
using System.Data;

using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
    /// <summary>
    /// Summary description for ServersHelper
    /// </summary>
    public class ServersHelper
    {
        public DataSet GetRawServers()
        {
            return ES.Services.Servers.GetRawServers();
        }

        public DataSet GetRawVirtualServers()
        {
            return ES.Services.Servers.GetVirtualServers();
        }

        #region Domains Paged ODS Methods
        DataSet dsDomainsPaged;

        public int GetDomainsPagedCount(int packageId, int serverId, bool recursive, string filterColumn, string filterValue)
        {
            return (int)dsDomainsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetDomainsPaged(int maximumRows, int startRowIndex, string sortColumn,
            int packageId, int serverId, bool recursive, string filterColumn, string filterValue)
        {

            dsDomainsPaged = ES.Services.Servers.GetDomainsPaged(packageId, serverId, recursive,
                filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return dsDomainsPaged.Tables[1];
        }
        #endregion

        #region Event Log Entries Paged
        SystemLogEntriesPaged logEntries;

        public int GetEventLogEntriesPagedCount(string logName)
        {
            return logEntries.Count;
        }

        public SystemLogEntry[] GetEventLogEntriesPaged(string logName, int maximumRows, int startRowIndex)
        {
            logEntries = ES.Services.Servers.GetLogEntriesPaged(PanelRequest.ServerId, logName, startRowIndex, maximumRows);
            return logEntries.Entries;
        }
        #endregion

        #region DNS Zone Records
        public DataSet GetRawDnsZoneRecords(int domainId)
        {
            return ES.Services.Servers.GetRawDnsZoneRecords(domainId);
        }
        #endregion
    }
}
