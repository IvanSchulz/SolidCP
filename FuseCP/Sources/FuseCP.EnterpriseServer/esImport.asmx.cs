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

using FuseCP.Providers;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esImport: WebService
    {
        [WebMethod]
        public List<ServiceProviderItemType> GetImportableItemTypes(int packageId)
        {
            return ImportController.GetImportableItemTypes(packageId);
        }

        [WebMethod]
        public List<string> GetImportableItems(int packageId, int itemTypeId)
        {
            return ImportController.GetImportableItems(packageId, itemTypeId);
        }

        [WebMethod]
        public int ImportItems(bool async, string taskId, int packageId, string[] items)
        {
            return ImportController.ImportItems(async, taskId, packageId, items);
        }
    }
}
