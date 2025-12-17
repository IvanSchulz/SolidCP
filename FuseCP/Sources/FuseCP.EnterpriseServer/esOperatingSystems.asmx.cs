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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using FuseCP.Web.Services;
using System.ComponentModel;

using FuseCP.Providers.OS;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esOperatingSystems: WebService
    {
        [WebMethod]
        public DataSet GetRawOdbcSourcesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return OperatingSystemController.GetRawOdbcSourcesPaged(packageId, filterColumn,
                filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public string[] GetInstalledOdbcDrivers(int packageId)
        {
            return OperatingSystemController.GetInstalledOdbcDrivers(packageId);
        }

        [WebMethod]
        public List<SystemDSN> GetOdbcSources(int packageId, bool recursive)
        {
            return OperatingSystemController.GetOdbcSources(packageId, recursive);
        }

        [WebMethod]
        public SystemDSN GetOdbcSource(int itemId)
        {
            return OperatingSystemController.GetOdbcSource(itemId);
        }

        [WebMethod]
        public int AddOdbcSource(SystemDSN item)
        {
            return OperatingSystemController.AddOdbcSource(item);
        }

        [WebMethod]
        public int UpdateOdbcSource(SystemDSN item)
        {
            return OperatingSystemController.UpdateOdbcSource(item);
        }

        [WebMethod]
        public int DeleteOdbcSource(int itemId)
        {
            return OperatingSystemController.DeleteOdbcSource(itemId);
        }

        [WebMethod]
        public bool CheckFileServicesInstallation(int serviceId)
        {
            return OperatingSystemController.CheckFileServicesInstallation(serviceId);
        }
    }
}
