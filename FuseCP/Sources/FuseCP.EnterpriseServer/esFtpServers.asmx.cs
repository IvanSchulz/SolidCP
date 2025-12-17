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

using FuseCP.Providers.FTP;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esFtpServers: WebService
    {
        [WebMethod]
        public FtpSite[] GetFtpSites(int serviceId)
        {
            return FtpServerController.GetFtpSites(serviceId);
        }

        [WebMethod]
        public DataSet GetRawFtpAccountsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return FtpServerController.GetRawFtpAccountsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<FtpAccount> GetFtpAccounts(int packageId, bool recursive)
        {
            return FtpServerController.GetFtpAccounts(packageId, recursive);
        }

        [WebMethod]
        public FtpAccount GetFtpAccount(int itemId)
        {
            return FtpServerController.GetFtpAccount(itemId);
        }

        [WebMethod]
        public int AddFtpAccount(FtpAccount item)
        {
            return FtpServerController.AddFtpAccount(item);
        }

        [WebMethod]
        public int UpdateFtpAccount(FtpAccount item)
        {
            return FtpServerController.UpdateFtpAccount(item);
        }

        [WebMethod]
        public int DeleteFtpAccount(int itemId)
        {
            return FtpServerController.DeleteFtpAccount(itemId);
        }
    }
}
