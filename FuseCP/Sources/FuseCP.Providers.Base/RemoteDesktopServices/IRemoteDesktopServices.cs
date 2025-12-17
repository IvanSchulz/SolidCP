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
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using FuseCP.EnterpriseServer.Base.RDS;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Providers.RemoteDesktopServices
{
    /// <summary>
    /// Summary description for IRemoteDesktopServices.
    /// </summary>
    public interface IRemoteDesktopServices
    {
        bool CreateCollection(string organizationId, RdsCollection collection);
        bool AddRdsServersToDeployment(RdsServer[] servers);
        RdsCollection GetCollection(string collectionName);
        bool RemoveCollection(string organizationId, string collectionName, List<RdsServer> servers);
        bool SetUsersInCollection(string organizationId, string collectionName, List<string> users);
        void AddSessionHostServerToCollection(string organizationId, string collectionName, RdsServer server);
        void AddSessionHostServersToCollection(string organizationId, string collectionName, List<RdsServer> servers);
        void RemoveSessionHostServerFromCollection(string organizationId, string collectionName, RdsServer server);
        void RemoveSessionHostServersFromCollection(string organizationId, string collectionName, List<RdsServer> servers);

        void SetRDServerNewConnectionAllowed(string newConnectionAllowed, RdsServer server);

        List<StartMenuApp> GetAvailableRemoteApplications(string collectionName);
        List<RemoteApplication> GetCollectionRemoteApplications(string collectionName);
        bool AddRemoteApplication(string collectionName, RemoteApplication remoteApp);
        bool AddRemoteApplications(string collectionName, List<RemoteApplication> remoteApps);
        bool RemoveRemoteApplication(string collectionName, RemoteApplication remoteApp);

        bool AddSessionHostFeatureToServer(string hostName);
        bool CheckSessionHostFeatureInstallation(string hostName);

        bool CheckServerAvailability(string hostName);
        string GetServerIp(string hostName);
        string[] GetApplicationUsers(string collectionName, string applicationName);
        bool SetApplicationUsers(string collectionName, RemoteApplication remoteApp, string[] users);
        bool CheckRDSServerAvaliable(string hostname);
        List<string> GetServersExistingInCollections();
        void EditRdsCollectionSettings(RdsCollection collection);
        List<RdsUserSession> GetRdsUserSessions(string collectionName);
        void LogOffRdsUser(string unifiedSessionId, string hostServer);
        List<string> GetRdsCollectionSessionHosts(string collectionName);
        RdsServerInfo GetRdsServerInfo(string serverName);
        string GetRdsServerStatus(string serverName);
        void ShutDownRdsServer(string serverName);
        void RestartRdsServer(string serverName);
        void SaveRdsCollectionLocalAdmins(List<string> users, List<string> hosts, string collectionName, string organizationId);
        List<string> GetRdsCollectionLocalAdmins(string organizationId, string collectionName);
        void MoveRdsServerToTenantOU(string hostName, string organizationId);
        void RemoveRdsServerFromTenantOU(string hostName, string organizationId);
        void InstallCertificate(byte[] certificate, string password, List<string> hostNames);
        void MoveSessionHostToRdsOU(string hostName);
        void ApplyGPO(string organizationId, string collectionName, RdsServerSettings serverSettings);
        void ShadowSession(string sessionId, string fqdName, bool control);
        void MoveSessionHostsToCollectionOU(List<RdsServer> servers, string collectionName, string organizationId);
        ImportedRdsCollection GetExistingCollection(string collectionName);
        void ImportCollection(string organizationId, RdsCollection collection, List<string> users);
        void SendMessage(List<RdsMessageRecipient> recipients, string text);
    }
}
