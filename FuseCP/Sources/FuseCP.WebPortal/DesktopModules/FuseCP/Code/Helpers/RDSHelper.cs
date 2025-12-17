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
using System.Collections.Generic;
using System.Text;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.RemoteDesktopServices;

namespace FuseCP.Portal
{
	public class RDSHelper
	{
        #region RDS Servers

        RdsServersPaged rdsServers;

        public int GetRDSServersPagedCount(string filterColumn, string filterValue)
        {
            //return 4;
            return rdsServers.RecordsCount;
        }

        public RdsServer[] GetRDSServersPaged(int maximumRows, int startRowIndex, string sortColumn, string filterColumn, string filterValue)
        {
            rdsServers = ES.Services.RDS.GetRdsServersPaged(filterColumn, filterValue, sortColumn, startRowIndex, maximumRows, "0");

            foreach (var rdsServer in rdsServers.Servers)
            {
                rdsServer.Status = "...";                
                rdsServer.SslAvailable = ES.Services.RDS.GetRdsCertificateByItemId(rdsServer.ItemId) != null;
            }

            return rdsServers.Servers;            
        }

        public int GetOrganizationRdsServersPagedCount(int itemId, string filterValue)
        {
            return rdsServers.RecordsCount;
        }

        public RdsServer[] GetOrganizationRdsServersPaged(int itemId, int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            //TODO supply correct value for parameter rdsControllerServiceID.
            throw new NotImplementedException("This feture has to be corrected.");
            rdsServers = ES.Services.RDS.GetOrganizationRdsServersPaged(itemId, null, "S.Name", filterValue, sortColumn, startRowIndex, maximumRows, "");

            return rdsServers.Servers;
        }

        public RdsServer[] GetFreeRDSServers(int packageId, string ServiceId)
        {
            return ES.Services.RDS.GetFreeRdsServersPaged(packageId, "", "", "", 0, 1000, ServiceId).Servers;
        }

        #endregion

        #region RDS Collectons

        RdsCollectionPaged rdsCollections;

        public int GetRDSCollectonsPagedCount(int itemId, string filterValue)
        {
            //return 3;
            return rdsCollections.RecordsCount;
        }

        public RdsCollection[] GetRDSCollectonsPaged(int itemId, int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            rdsCollections = ES.Services.RDS.GetRdsCollectionsPaged(itemId, "DisplayName", filterValue, sortColumn, startRowIndex, maximumRows);

            return rdsCollections.Collections;

            //return new RdsCollection[] { new RdsCollection { Name = "Collection 1", Description = "" },
            //                             new RdsCollection { Name = "Collection 2", Description = "" },
            //                             new RdsCollection { Name = "Collection 3", Description = "" }};
        }

        #endregion
    }
}
