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
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Collections;
using FuseCP.Web.Services;
using System.ComponentModel;
using FuseCP.Providers;
using FuseCP.Providers.OS;
using FuseCP.Providers.RemoteDesktopServices;
using FuseCP.Server.Utils;
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer.Base.RDS;

namespace FuseCP.Server
{
    /// <summary>
    /// Summary description for RemoteDesktopServices
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class RemoteDesktopServices : HostingServiceProviderWebService, IRemoteDesktopServices
    {
        private IRemoteDesktopServices RDSProvider
        {
            get { return (IRemoteDesktopServices)Provider; }
        }

        [WebMethod, SoapHeader("settings")]
        public bool CreateCollection(string organizationId, RdsCollection collection)
        {
            try
            {
                Log.WriteStart("'{0}' CreateCollection", ProviderSettings.ProviderName);
                var result = RDSProvider.CreateCollection(organizationId, collection);
                Log.WriteEnd("'{0}' CreateCollection", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void EditRdsCollectionSettings(RdsCollection collection)
        {
            try
            {
                Log.WriteStart("'{0}' EditRdsCollectionSettings", ProviderSettings.ProviderName);
                RDSProvider.EditRdsCollectionSettings(collection);
                Log.WriteEnd("'{0}' EditRdsCollectionSettings", ProviderSettings.ProviderName);                
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' EditRdsCollectionSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<RdsUserSession> GetRdsUserSessions(string collectionName)
        {
            try
            {
                Log.WriteStart("'{0}' GetRdsUserSessions", ProviderSettings.ProviderName);
                var result = RDSProvider.GetRdsUserSessions(collectionName);
                Log.WriteEnd("'{0}' GetRdsUserSessions", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetRdsUserSessions", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool AddRdsServersToDeployment(RdsServer[] servers)
        {
            try
            {
                Log.WriteStart("'{0}' AddRdsServersToDeployment", ProviderSettings.ProviderName);
                var result = RDSProvider.AddRdsServersToDeployment(servers);
                Log.WriteEnd("'{0}' AddRdsServersToDeployment", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddRdsServersToDeployment", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public RdsCollection GetCollection(string collectionName)
        {
            try
            {
                Log.WriteStart("'{0}' GetCollection", ProviderSettings.ProviderName);
                var result = RDSProvider.GetCollection(collectionName);
                Log.WriteEnd("'{0}' GetCollection", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool RemoveCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            try
            {
                Log.WriteStart("'{0}' RemoveCollection", ProviderSettings.ProviderName);
                var result = RDSProvider.RemoveCollection(organizationId, collectionName, servers);
                Log.WriteEnd("'{0}' RemoveCollection", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RemoveCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SetUsersInCollection(string organizationId, string collectionName, List<string> users)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateUsersInCollection", ProviderSettings.ProviderName);
                var result = RDSProvider.SetUsersInCollection(organizationId, collectionName, users);
                Log.WriteEnd("'{0}' UpdateUsersInCollection", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateUsersInCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AddSessionHostServerToCollection(string organizationId, string collectionName, RdsServer server)
        {
            try
            {
                Log.WriteStart("'{0}' AddSessionHostServersToCollection", ProviderSettings.ProviderName);
                RDSProvider.AddSessionHostServerToCollection(organizationId, collectionName, server);
                Log.WriteEnd("'{0}' AddSessionHostServersToCollection", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddSessionHostServersToCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AddSessionHostServersToCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            try
            {
                Log.WriteStart("'{0}' AddSessionHostServersToCollection", ProviderSettings.ProviderName);
                RDSProvider.AddSessionHostServersToCollection(organizationId, collectionName, servers);
                Log.WriteEnd("'{0}' AddSessionHostServersToCollection", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddSessionHostServersToCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void RemoveSessionHostServerFromCollection(string organizationId, string collectionName, RdsServer server)
        {
            try
            {
                Log.WriteStart("'{0}' RemoveSessionHostServerFromCollection", ProviderSettings.ProviderName);
                RDSProvider.RemoveSessionHostServerFromCollection(organizationId, collectionName, server);
                Log.WriteEnd("'{0}' RemoveSessionHostServerFromCollection", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RemoveSessionHostServerFromCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void RemoveSessionHostServersFromCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            try
            {
                Log.WriteStart("'{0}' RemoveSessionHostServersFromCollection", ProviderSettings.ProviderName);
                RDSProvider.RemoveSessionHostServersFromCollection(organizationId, collectionName, servers);
                Log.WriteEnd("'{0}' RemoveSessionHostServersFromCollection", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RemoveSessionHostServersFromCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetRDServerNewConnectionAllowed(string newConnectionAllowed, RdsServer server)
        {
            try
            {
                Log.WriteStart("'{0}' SetRDServerNewConnectionAllowed", ProviderSettings.ProviderName);
                RDSProvider.SetRDServerNewConnectionAllowed(newConnectionAllowed, server);
                Log.WriteEnd("'{0}' SetRDServerNewConnectionAllowed", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetRDServerNewConnectionAllowed", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<StartMenuApp> GetAvailableRemoteApplications(string collectionName)
        {
            try
            {
                Log.WriteStart("'{0}' GetAvailableRemoteApplications", ProviderSettings.ProviderName);
                var result = RDSProvider.GetAvailableRemoteApplications(collectionName);
                Log.WriteEnd("'{0}' GetAvailableRemoteApplications", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateUsersInCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<RemoteApplication> GetCollectionRemoteApplications(string collectionName)
        {
            try
            {
                Log.WriteStart("'{0}' GetCollectionRemoteApplications", ProviderSettings.ProviderName);
                var result = RDSProvider.GetCollectionRemoteApplications(collectionName);
                Log.WriteEnd("'{0}' GetCollectionRemoteApplications", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetCollectionRemoteApplications", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool AddRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            try
            {
                Log.WriteStart("'{0}' AddRemoteApplication", ProviderSettings.ProviderName);
                var result = RDSProvider.AddRemoteApplication(collectionName, remoteApp);
                Log.WriteEnd("'{0}' AddRemoteApplication", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddRemoteApplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool AddRemoteApplications(string collectionName, List<RemoteApplication> remoteApps)
        {
            try
            {
                Log.WriteStart("'{0}' AddRemoteApplications", ProviderSettings.ProviderName);
                var result = RDSProvider.AddRemoteApplications(collectionName, remoteApps);
                Log.WriteEnd("'{0}' AddRemoteApplications", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddRemoteApplications", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool RemoveRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            try
            {
                Log.WriteStart("'{0}' RemoveRemoteApplication", ProviderSettings.ProviderName);
                var result = RDSProvider.RemoveRemoteApplication(collectionName, remoteApp);
                Log.WriteEnd("'{0}' RemoveRemoteApplication", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RemoveRemoteApplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool AddSessionHostFeatureToServer(string hostName)
        {
            try
            {
                Log.WriteStart("'{0}' AddSessionHostFeatureToServer", ProviderSettings.ProviderName);
                var result = RDSProvider.AddSessionHostFeatureToServer(hostName);
                Log.WriteEnd("'{0}' AddSessionHostFeatureToServer", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddSessionHostServersToCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool CheckSessionHostFeatureInstallation(string hostName)
        {
            try
            {
                Log.WriteStart("'{0}' CheckSessionHostFeatureInstallation", ProviderSettings.ProviderName);
                var result = RDSProvider.CheckSessionHostFeatureInstallation(hostName);
                Log.WriteEnd("'{0}' CheckSessionHostFeatureInstallation", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CheckSessionHostFeatureInstallation", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool CheckServerAvailability(string hostName)
        {
            try
            {
                Log.WriteStart("'{0}' CheckServerAvailability", ProviderSettings.ProviderName);
                var result = RDSProvider.CheckServerAvailability(hostName);
                Log.WriteEnd("'{0}' CheckServerAvailability", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CheckServerAvailability", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetApplicationUsers(string collectionName, string applicationName)
        {
            try
            {
                Log.WriteStart("'{0}' GetApplicationUsers", ProviderSettings.ProviderName);
                var result = RDSProvider.GetApplicationUsers(collectionName, applicationName);
                Log.WriteEnd("'{0}' GetApplicationUsers", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetApplicationUsers", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SetApplicationUsers(string collectionName, RemoteApplication remoteApp, string[] users)
        {
            try
            {
                Log.WriteStart("'{0}' SetApplicationUsers", ProviderSettings.ProviderName);
                var result = RDSProvider.SetApplicationUsers(collectionName, remoteApp, users);
                Log.WriteEnd("'{0}' SetApplicationUsers", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetApplicationUsers", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool CheckRDSServerAvaliable(string hostname)
        {
            try
            {
                Log.WriteStart("'{0}' CheckRDSServerAvaliable", ProviderSettings.ProviderName);
                var result = RDSProvider.CheckRDSServerAvaliable(hostname);
                Log.WriteEnd("'{0}' CheckRDSServerAvaliable", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CheckRDSServerAvaliable", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<string> GetServersExistingInCollections()
        {
            try
            {
                Log.WriteStart("'{0}' GetServersExistingInCollections", ProviderSettings.ProviderName);
                var result = RDSProvider.GetServersExistingInCollections();
                Log.WriteEnd("'{0}' GetServersExistingInCollections", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetServersExistingInCollections", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void LogOffRdsUser(string unifiedSessionId, string hostServer)
        {
            try
            {
                Log.WriteStart("'{0}' LogOffRdsUser", ProviderSettings.ProviderName);
                RDSProvider.LogOffRdsUser(unifiedSessionId, hostServer);
                Log.WriteEnd("'{0}' LogOffRdsUser", ProviderSettings.ProviderName);                
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' LogOffRdsUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<string> GetRdsCollectionSessionHosts(string collectionName)
        {
            try
            {
                Log.WriteStart("'{0}' GetRdsCollectionSessionHosts", ProviderSettings.ProviderName);
                var result = RDSProvider.GetRdsCollectionSessionHosts(collectionName);
                Log.WriteEnd("'{0}' GetRdsCollectionSessionHosts", ProviderSettings.ProviderName);

                return result;
            }
            catch(Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetRdsCollectionSessionHosts", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public RdsServerInfo GetRdsServerInfo(string serverName)
        {
            try
            {
                Log.WriteStart("'{0}' GetRdsServerInfo", ProviderSettings.ProviderName);
                var result = RDSProvider.GetRdsServerInfo(serverName);
                Log.WriteEnd("'{0}' GetRdsServerInfo", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetRdsServerInfo", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string GetRdsServerStatus(string serverName)
        {
            try
            {
                Log.WriteStart("'{0}' GetRdsServerStatus", ProviderSettings.ProviderName);
                var result = RDSProvider.GetRdsServerStatus(serverName);
                Log.WriteEnd("'{0}' GetRdsServerStatus", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetRdsServerStatus", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ShutDownRdsServer(string serverName)
        {
            try
            {
                Log.WriteStart("'{0}' ShutDownRdsServer", ProviderSettings.ProviderName);
                RDSProvider.ShutDownRdsServer(serverName);
                Log.WriteEnd("'{0}' ShutDownRdsServer", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShutDownRdsServer", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void RestartRdsServer(string serverName)
        {
            try
            {
                Log.WriteStart("'{0}' RestartRdsServer", ProviderSettings.ProviderName);
                RDSProvider.RestartRdsServer(serverName);
                Log.WriteEnd("'{0}' RestartRdsServer", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RestartRdsServer", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SaveRdsCollectionLocalAdmins(List<string> users, List<string> hosts, string organizationId, string collectionName)
        {
            try
            {
                Log.WriteStart("'{0}' SaveRdsCollectionLocalAdmins", ProviderSettings.ProviderName);
                RDSProvider.SaveRdsCollectionLocalAdmins(users, hosts, collectionName, organizationId);
                Log.WriteEnd("'{0}' SaveRdsCollectionLocalAdmins", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SaveRdsCollectionLocalAdmins", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<string> GetRdsCollectionLocalAdmins(string organizationId, string collectionName)
        {
            try
            {
                Log.WriteStart("'{0}' GetRdsCollectionLocalAdmins", ProviderSettings.ProviderName);
                var result = RDSProvider.GetRdsCollectionLocalAdmins(organizationId, collectionName);
                Log.WriteEnd("'{0}' GetRdsCollectionLocalAdmins", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetRdsCollectionLocalAdmins", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void MoveRdsServerToTenantOU(string hostName, string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' MoveRdsServerToTenantOU", ProviderSettings.ProviderName);
                RDSProvider.MoveRdsServerToTenantOU(hostName, organizationId);
                Log.WriteEnd("'{0}' MoveRdsServerToTenantOU", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MoveRdsServerToTenantOU", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void RemoveRdsServerFromTenantOU(string hostName, string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' RemoveRdsServerFromTenantOU", ProviderSettings.ProviderName);
                RDSProvider.RemoveRdsServerFromTenantOU(hostName, organizationId);
                Log.WriteEnd("'{0}' RemoveRdsServerFromTenantOU", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RemoveRdsServerFromTenantOU", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void InstallCertificate(byte[] certificate, string password, List<string> hostNames)
        {
            try
            {
                Log.WriteStart("'{0}' InstallCertificate", ProviderSettings.ProviderName);
                RDSProvider.InstallCertificate(certificate, password, hostNames);
                Log.WriteEnd("'{0}' InstallCertificate", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' InstallCertificate", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void MoveSessionHostToRdsOU(string hostName)
        {
            try
            {
                Log.WriteStart("'{0}' MoveSessionHostToRdsOU", ProviderSettings.ProviderName);
                RDSProvider.MoveSessionHostToRdsOU(hostName);
                Log.WriteEnd("'{0}' MoveSessionHostToRdsOU", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MoveSessionHostToRdsOU", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ApplyGPO(string organizationId, string collectionName, RdsServerSettings serverSettings)
        {
            try
            {
                Log.WriteStart("'{0}' ApplyGPO", ProviderSettings.ProviderName);
                RDSProvider.ApplyGPO(organizationId, collectionName, serverSettings);
                Log.WriteEnd("'{0}' ApplyGPO", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ApplyGPO", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ShadowSession(string sessionId, string fqdName, bool control)
        {
            try
            {
                Log.WriteStart("'{0}' ShadowSession", ProviderSettings.ProviderName);
                RDSProvider.ShadowSession(sessionId, fqdName, control);
                Log.WriteEnd("'{0}' ShadowSession", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShadowSession", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void MoveSessionHostsToCollectionOU(List<RdsServer> servers, string collectionName, string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' MoveSessionHostsToCollectionOU", ProviderSettings.ProviderName);
                RDSProvider.MoveSessionHostsToCollectionOU(servers, collectionName, organizationId);
                Log.WriteEnd("'{0}' MoveSessionHostsToCollectionOU", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MoveSessionHostsToCollectionOU", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ImportedRdsCollection GetExistingCollection(string collectionName)
        {
            try
            {
                Log.WriteStart("'{0}' GetExistingCollection", ProviderSettings.ProviderName);
                var result = RDSProvider.GetExistingCollection(collectionName);
                Log.WriteEnd("'{0}' GetExistingCollection", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetExistingCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ImportCollection(string organizationId, RdsCollection collection, List<string> users)
        {
            try
            {
                Log.WriteStart("'{0}' ImportCollection", ProviderSettings.ProviderName);
                RDSProvider.ImportCollection(organizationId, collection, users);
                Log.WriteEnd("'{0}' ImportCollection", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ImportCollection", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SendMessage(List<RdsMessageRecipient> recipients, string text)
        {
            try
            {
                Log.WriteStart("'{0}' SendMessage", ProviderSettings.ProviderName);
                RDSProvider.SendMessage(recipients, text);
                Log.WriteEnd("'{0}' SendMessage", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SendMessage", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string GetServerIp(string hostName)
        {
            try
            {
                Log.WriteStart("'{0}' GetServerIp for '{1}'", ProviderSettings.ProviderName, hostName);
                var result = RDSProvider.GetServerIp(hostName);
                Log.WriteEnd("'{0}' GetServerIp for '{1}'", ProviderSettings.ProviderName, hostName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetServerIp for '{1}'", ProviderSettings.ProviderName, hostName), ex);
                //throw;
                return "Unable to connect";
            }
        }
    }
}
