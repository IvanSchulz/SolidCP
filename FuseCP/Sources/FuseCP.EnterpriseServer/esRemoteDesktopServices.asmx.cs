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

using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.RemoteDesktopServices;
using FuseCP.EnterpriseServer.Base.RDS;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esRemoteDesktopServices: WebService
    {

        [WebMethod]
        public RdsCollection GetRdsCollection(int collectionId, bool quick)
        {
            return RemoteDesktopServicesController.GetRdsCollection(collectionId, quick);
        }

        [WebMethod]
        public RdsCollectionSettings GetRdsCollectionSettings(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollectionSettings(collectionId);
        }

        [WebMethod]
        public List<RdsCollection> GetOrganizationRdsCollections(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsCollections(itemId);
        }

        [WebMethod]
        public int AddRdsCollection(int itemId, RdsCollection collection)
        {
            return RemoteDesktopServicesController.AddRdsCollection(itemId, collection);
        }

        [WebMethod]
        public ResultObject EditRdsCollection(int itemId, RdsCollection collection)
        {
            return RemoteDesktopServicesController.EditRdsCollection(itemId, collection);
        }

        [WebMethod]
        public ResultObject EditRdsCollectionSettings(int itemId, RdsCollection collection)
        {
            return RemoteDesktopServicesController.EditRdsCollectionSettings(itemId, collection);
        }

        [WebMethod]
        public RdsCollectionPaged GetRdsCollectionsPaged(int itemId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return RemoteDesktopServicesController.GetRdsCollectionsPaged(itemId, filterColumn, filterValue, sortColumn,
                startRow, maximumRows);
        }

        [WebMethod]
        public ResultObject RemoveRdsCollection(int itemId, RdsCollection collection)
        {
            return RemoteDesktopServicesController.RemoveRdsCollection(itemId, collection);
        }

        [WebMethod]
        public RdsServersPaged GetRdsServersPaged(string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return RemoteDesktopServicesController.GetRdsServersPaged(filterColumn, filterValue, sortColumn, startRow,
                maximumRows, rdsControllerServiceID);
        }

        [WebMethod]
        public RdsServersPaged GetFreeRdsServersPaged(int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, string ServiceId)
        {
            return RemoteDesktopServicesController.GetFreeRdsServersPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows, ServiceId);
        }

        [WebMethod]
        public RdsServersPaged GetOrganizationRdsServersPaged(int itemId, int? collectionId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsServersPaged(itemId, collectionId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        [WebMethod]
        public RdsServersPaged GetOrganizationFreeRdsServersPaged(int itemId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return RemoteDesktopServicesController.GetOrganizationFreeRdsServersPaged(itemId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        [WebMethod]
        public RdsServer GetRdsServer(int rdsSeverId)
        {
            return RemoteDesktopServicesController.GetRdsServer(rdsSeverId);
        }

        [WebMethod]
        public ResultObject SetRDServerNewConnectionAllowed(int itemId, string newConnectionAllowed, int rdsSeverId)
        {
            return RemoteDesktopServicesController.SetRDServerNewConnectionAllowed(itemId, newConnectionAllowed, rdsSeverId);
        }

        [WebMethod]
        public List<RdsServer> GetCollectionRdsServers(int collectionId)
        {
            return RemoteDesktopServicesController.GetCollectionRdsServers(collectionId);
        }

        [WebMethod]
        public List<RdsServer> GetOrganizationRdsServers(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsServers(itemId);
        }

        [WebMethod]
        public ResultObject AddRdsServer(RdsServer rdsServer, string rdsControllerServiceID)
        {
            return RemoteDesktopServicesController.AddRdsServer(rdsServer, rdsControllerServiceID);
        }

        [WebMethod]
        public ResultObject AddRdsServerToCollection(int itemId, RdsServer rdsServer, RdsCollection rdsCollection)
        {
            return RemoteDesktopServicesController.AddRdsServerToCollection(itemId, rdsServer, rdsCollection);
        }

        [WebMethod]
        public ResultObject AddRdsServerToOrganization(int itemId, int serverId)
        {
            return RemoteDesktopServicesController.AddRdsServerToOrganization(itemId, serverId);
        }

        [WebMethod]
        public ResultObject RemoveRdsServer(int rdsServerId)
        {
            return RemoteDesktopServicesController.RemoveRdsServer(rdsServerId);
        }

        [WebMethod]
        public ResultObject RemoveRdsServerFromCollection(int itemId, RdsServer rdsServer, RdsCollection rdsCollection)
        {
            return RemoteDesktopServicesController.RemoveRdsServerFromCollection(itemId, rdsServer, rdsCollection);
        }

        [WebMethod]
        public ResultObject RemoveRdsServerFromOrganization(int itemId, int rdsServerId)
        {
            return RemoteDesktopServicesController.RemoveRdsServerFromOrganization(itemId, rdsServerId);
        }

        [WebMethod]
        public ResultObject UpdateRdsServer(RdsServer rdsServer)
        {
            return RemoteDesktopServicesController.UpdateRdsServer(rdsServer);
        }

        [WebMethod]
        public List<OrganizationUser> GetRdsCollectionUsers(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollectionUsers(collectionId);
        }

        [WebMethod]
        public ResultObject SetUsersToRdsCollection(int itemId, int collectionId, List<OrganizationUser> users)
        {
            return RemoteDesktopServicesController.SetUsersToRdsCollection(itemId, collectionId, users);
        }

        [WebMethod]
        public List<RemoteApplication> GetCollectionRemoteApplications(int itemId, string collectionName)
        {
            return RemoteDesktopServicesController.GetCollectionRemoteApplications(itemId, collectionName);
        }

        [WebMethod]
        public List<StartMenuApp> GetAvailableRemoteApplications(int itemId, string collectionName)
        {
            return RemoteDesktopServicesController.GetAvailableRemoteApplications(itemId, collectionName);
        }

        [WebMethod]
        public ResultObject AddRemoteApplicationToCollection(int itemId, RdsCollection collection, RemoteApplication application)
        {
            return RemoteDesktopServicesController.AddRemoteApplicationToCollection(itemId, collection, application);
        }

        [WebMethod]
        public ResultObject RemoveRemoteApplicationFromCollection(int itemId, RdsCollection collection, RemoteApplication application)
        {
            return RemoteDesktopServicesController.RemoveRemoteApplicationFromCollection(itemId, collection, application);
        }

        [WebMethod]
        public ResultObject SetRemoteApplicationsToRdsCollection(int itemId, int collectionId, List<RemoteApplication> remoteApps)
        {
            return RemoteDesktopServicesController.SetRemoteApplicationsToRdsCollection(itemId, collectionId, remoteApps);
        }

        [WebMethod]
        public int GetOrganizationRdsUsersCount(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsUsersCount(itemId);
        }

        [WebMethod]
        public int GetOrganizationRdsServersCount(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsServersCount(itemId);
        }

        [WebMethod]
        public int GetOrganizationRdsCollectionsCount(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsCollectionsCount(itemId);
        }        

        [WebMethod]
        public List<string> GetApplicationUsers(int itemId, int collectionId, RemoteApplication remoteApp)
        {
            return RemoteDesktopServicesController.GetApplicationUsers(itemId, collectionId, remoteApp);
        }

        [WebMethod]
        public ResultObject SetApplicationUsers(int itemId, int collectionId, RemoteApplication remoteApp, List<string> users)
        {
            return RemoteDesktopServicesController.SetApplicationUsers(itemId, collectionId, remoteApp, users);
        }

        [WebMethod]
        public List<RdsUserSession> GetRdsUserSessions(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsUserSessions(collectionId);
        }

        [WebMethod]
        public ResultObject LogOffRdsUser(int itemId, string unifiedSessionId, string hostServer)
        {
            return RemoteDesktopServicesController.LogOffRdsUser(itemId, unifiedSessionId, hostServer);
        }

        [WebMethod]
        public List<string> GetRdsCollectionSessionHosts(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollectionSessionHosts(collectionId);
        }

        [WebMethod]
        public RdsServerInfo GetRdsServerInfo(int? itemId, string fqdnName)
        {
            return RemoteDesktopServicesController.GetRdsServerInfo(itemId, fqdnName);
        }

        [WebMethod]
        public string GetRdsServerStatus(int? itemId, string fqdnName)
        {
            return RemoteDesktopServicesController.GetRdsServerStatus(itemId, fqdnName);
        }

        [WebMethod]
        public ResultObject ShutDownRdsServer(int? itemId, string fqdnName)
        {
            return RemoteDesktopServicesController.ShutDownRdsServer(itemId, fqdnName);
        }

        [WebMethod]
        public ResultObject RestartRdsServer(int? itemId, string fqdnName)
        {
            return RemoteDesktopServicesController.RestartRdsServer(itemId, fqdnName);
        }

        [WebMethod]
        public List<OrganizationUser> GetRdsCollectionLocalAdmins(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollectionLocalAdmins(collectionId);
        }

        [WebMethod]
        public ResultObject SaveRdsCollectionLocalAdmins(OrganizationUser[] users, int collectionId)
        {
            return RemoteDesktopServicesController.SaveRdsCollectionLocalAdmins(users, collectionId);
        }

        [WebMethod]
        public ResultObject InstallSessionHostsCertificate(RdsServer rdsServer)
        {
            return RemoteDesktopServicesController.InstallSessionHostsCertificate(rdsServer);
        }

        [WebMethod]
        public RdsCertificate GetRdsCertificateByServiceId(int serviceId)
        {
            return RemoteDesktopServicesController.GetRdsCertificateByServiceId(serviceId);
        }

        [WebMethod]
        public RdsCertificate GetRdsCertificateByItemId(int? itemId)
        {
            return RemoteDesktopServicesController.GetRdsCertificateByItemId(itemId);
        }

        [WebMethod]
        public ResultObject AddRdsCertificate(RdsCertificate certificate)
        {
            return RemoteDesktopServicesController.AddRdsCertificate(certificate);
        }

        [WebMethod]
        public List<ServiceInfo> GetRdsServices()
        {
            return RemoteDesktopServicesController.GetRdsServices();
        }

        [WebMethod]
        public string GetRdsSetupLetter(int itemId, int? accountId)
        {
            return RemoteDesktopServicesController.GetRdsSetupLetter(itemId, accountId);
        }

        [WebMethod]
        public int SendRdsSetupLetter(int itemId, int? accountId, string to, string cc)
        {
            return RemoteDesktopServicesController.SendRdsSetupLetter(itemId, accountId, to, cc);
        }

        [WebMethod]
        public RdsServerSettings GetRdsServerSettings(int serverId, string settingsName)
        {
            return RemoteDesktopServicesController.GetRdsServerSettings(serverId, settingsName);
        }

        [WebMethod]
        public int UpdateRdsServerSettings(int serverId, string settingsName, RdsServerSettings settings)
        {
            return RemoteDesktopServicesController.UpdateRdsServerSettings(serverId, settingsName, settings);
        }

        [WebMethod]
        public ResultObject ShadowSession(int itemId, string sessionId, bool control, string fqdName)
        {
            return RemoteDesktopServicesController.ShadowSession(itemId, sessionId, control, fqdName);
        }

        [WebMethod]
        public ResultObject ImportCollection(int itemId, string collectionName, string rdsControllerServiceID)
        {
            return RemoteDesktopServicesController.ImportCollection(itemId, collectionName, rdsControllerServiceID);
        }

        [WebMethod]
        public int GetRemoteDesktopServiceId(int itemId)
        {
            return RemoteDesktopServicesController.GetRemoteDesktopServiceId(itemId);
        }

        [WebMethod]
        public ResultObject SendMessage(RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName)
        {
            return RemoteDesktopServicesController.SendMessage(recipients, text, itemId, rdsCollectionId, userName);
        }

        [WebMethod]
        public List<RdsMessage> GetRdsMessagesByCollectionId(int rdsCollectionId)
        {
            return RemoteDesktopServicesController.GetRdsMessagesByCollectionId(rdsCollectionId);
        }
    }
}
