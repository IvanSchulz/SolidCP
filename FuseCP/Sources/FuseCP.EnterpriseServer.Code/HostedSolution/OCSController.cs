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

﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;
using FuseCP.Server.Client;
using FuseCP.EnterpriseServer.Data;

namespace FuseCP.EnterpriseServer.Code.HostedSolution
{
    public class OCSController: ControllerBase
    {
        public OCSController(ControllerBase provider) : base(provider) { }

        private OCSServer GetOCSProxy(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);

            OCSServer ocs = new OCSServer();
            ServiceProviderProxy.Init(ocs, serviceId);
            

            return ocs;
        }
        
        private bool CheckQuota(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

            IntResult userCount = GetOCSUsersCount(itemId, string.Empty, string.Empty);

            int allocatedBlackBerryUsers = cntx.Quotas[Quotas.OCS_USERS].QuotaAllocatedValuePerOrganization;

            return allocatedBlackBerryUsers == -1 || allocatedBlackBerryUsers > userCount.Value;
        }

        private void SetUserGeneralSettingsByDefault(int itemId, string instanceId, OCSServer ocs)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

            

            ocs.SetUserGeneralSettings(instanceId, !cntx.Quotas[Quotas.OCS_FederationByDefault].QuotaExhausted,
                !cntx.Quotas[Quotas.OCS_PublicIMConnectivityByDefault].QuotaExhausted, 
                !cntx.Quotas[Quotas.OCS_ArchiveIMConversationByDefault].QuotaExhausted,
                !cntx.Quotas[Quotas.OCS_ArchiveFederatedIMConversationByDefault].QuotaExhausted,
                !cntx.Quotas[Quotas.OCS_PresenceAllowedByDefault].QuotaExhausted);

    
        }


        private OCSEdgeServer[] GetEdgeServers(string edgeServices)
        {
            List<OCSEdgeServer> list = new List<OCSEdgeServer>();
            if (!string.IsNullOrEmpty(edgeServices))
            {
                string[] services = edgeServices.Split(';');
                foreach (string current in services)
                {
                    string[] data = current.Split(',');
                    try
                    {
                        int serviceId = int.Parse(data[1]);
                        OCSEdgeServer ocs = new OCSEdgeServer();
                        ServiceProviderProxy.Init(ocs, serviceId);
                        list.Add(ocs);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex);
                    }
                }
            }

            return list.ToArray();
        }

        public void DeleteDomain(int itemId, string domainName)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            if (org.IsOCSOrganization)
            {
                int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                string edgeServersData = settings[OCSConstants.EDGEServicesData];
                OCSEdgeServer[] edgeServers = GetEdgeServers(edgeServersData);
                DeleteDomain(domainName, edgeServers);
            }
        }

        public void DeleteDomain(string domainName, OCSEdgeServer[] edgeServers)
        {
            foreach (OCSEdgeServer currentEdgeServer in edgeServers)
            {
                try
                {
                    currentEdgeServer.DeleteDomain(domainName);
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
        }

        public void AddDomain(string domainName, OCSEdgeServer[] edgeServers)
        {
            foreach (OCSEdgeServer currentEdgeServer in edgeServers)
            {
                try
                {
                    currentEdgeServer.AddDomain(domainName);
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
        }

        public void  AddDomain(string domainName, int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            if (!org.IsOCSOrganization)
            {
                int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                string edgeServersData = settings[OCSConstants.EDGEServicesData];
                OCSEdgeServer[] edgeServers = GetEdgeServers(edgeServersData);
                AddDomain(domainName, edgeServers);
            }
        }

        private void CreateOCSDomains(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            if (!org.IsOCSOrganization)
            {
                List<OrganizationDomainName> domains = OrganizationController.GetOrganizationDomains(itemId);
                int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                string edgeServersData = settings[OCSConstants.EDGEServicesData];
                OCSEdgeServer[] edgeServers = GetEdgeServers(edgeServersData);
                foreach (OrganizationDomainName currentDomain in domains)
                {
                    AddDomain(currentDomain.DomainName, edgeServers);
                }

                org.IsOCSOrganization = true;

                PackageController.UpdatePackageItem(org);
            }
        }


        public OCSUserResult CreateOCSUser(int itemId, int accountId)
        {
            OCSUserResult res = TaskManager.StartResultTask<OCSUserResult>("OCS", "CREATE_OCS_USER");

            OCSUser retOCSUser = new OCSUser();
            bool isOCSUser;

            try
            {
                isOCSUser = Database.CheckOCSUserExists(accountId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_CHECK_IF_OCS_USER_EXISTS, ex);
                return res;
            }

            if (isOCSUser)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.USER_IS_ALREADY_OCS_USER);
                return res;
            }

            OrganizationUser user;
            try
            {
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user == null)
                    throw new ApplicationException(
                        string.Format("User is null. ItemId={0}, AccountId={1}", itemId,
                                      accountId));

            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }


            try
            {
                user = OrganizationController.GetUserGeneralSettings(itemId, accountId);
                if (string.IsNullOrEmpty(user.FirstName))
                {
                    TaskManager.CompleteResultTask(res, OCSErrorCodes.USER_FIRST_NAME_IS_NOT_SPECIFIED);
                    return res;
                }
                
                if (string.IsNullOrEmpty(user.LastName))
                {
                    TaskManager.CompleteResultTask(res, OCSErrorCodes.USER_LAST_NAME_IS_NOT_SPECIFIED);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_GET_USER_GENERAL_SETTINGS, ex);
                return res;
            }


            try
            {
                bool quota = CheckQuota(itemId);
                if (!quota)
                {
                    TaskManager.CompleteResultTask(res, OCSErrorCodes.USER_QUOTA_HAS_BEEN_REACHED);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_CHECK_QUOTA, ex);
                return res;
            }


           OCSServer ocs;

           try
            {
                ocs = GetOCSProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_GET_OCS_PROXY, ex);
                return res;
            }

            string instanceId;
            try
            {
                CreateOCSDomains(itemId);

                instanceId = ocs.CreateUser(user.PrimaryEmailAddress, user.DistinguishedName);
                retOCSUser.InstanceId = instanceId;
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_ADD_OCS_USER, ex);
                return res;
            }


            try
            {
                SetUserGeneralSettingsByDefault(itemId, instanceId, ocs);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_SET_DEFAULT_SETTINGS, ex);
                return res;
                
            }
            
            try
            {
                Database.AddOCSUser(accountId, instanceId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_ADD_OCS_USER_TO_DATABASE, ex);
                return res;
            }

            res.Value = retOCSUser;
            TaskManager.CompleteResultTask();
            return res;

        }

        
        public OCSUsersPagedResult GetOCSUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            OCSUsersPagedResult res = TaskManager.StartResultTask<OCSUsersPagedResult>("OCS", "GET_OCS_USERS");

            try
            {
                IDataReader reader =
                    Database.GetOCSUsers(itemId, sortColumn, sortDirection, name, email, startRow, count);
                List<OCSUser> accounts = new List<OCSUser>();
                ObjectUtils.FillCollectionFromDataReader(accounts, reader);
                res.Value = new OCSUsersPaged { PageUsers = accounts.ToArray() };
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.GET_OCS_USERS, ex);
                return res;
            }

            IntResult intRes = GetOCSUsersCount(itemId, name, email);
            res.ErrorCodes.AddRange(intRes.ErrorCodes);
            if (!intRes.IsSuccess)
            {
                TaskManager.CompleteResultTask(res);
                return res;
            }
            res.Value.RecordsCount = intRes.Value;

            TaskManager.CompleteResultTask();
            return res;
        }

        public IntResult GetOCSUsersCount(int itemId, string name, string email)
        {
            IntResult res = TaskManager.StartResultTask<IntResult>("OCS", "GET_OCS_USERS_COUNT");
            try
            {
                res.Value = Database.GetOCSUsersCount(itemId, name, email);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.GET_OCS_USER_COUNT, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject DeleteOCSUser(int itemId, string instanceId)
        {
            ResultObject res = TaskManager.StartResultTask<ResultObject>("OCS", "DELETE_OCS_USER");

            OCSServer ocsServer;

            try
            {
                ocsServer = GetOCSProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_GET_OCS_PROXY, ex);
                return res;
            }

            
            try
            {
                ocsServer.DeleteUser(instanceId);                                
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_DELETE_OCS_USER, ex);
                return res;
            }

            try
            {
                Database.DeleteOCSUser(instanceId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_DELETE_OCS_USER_FROM_METADATA, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public OCSUser GetUserGeneralSettings(int itemId, string instanceId)
        {
            TaskManager.StartTask("OCS", "GET_OCS_USER_GENERAL_SETTINGS");

            OCSUser user;

            try
            {
                OCSServer ocs = GetOCSProxy(itemId);
                user = ocs.GetUserGeneralSettings(instanceId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
                
            }
            TaskManager.CompleteTask();
            return user;

        }

        public void SetUserGeneralSettings(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            TaskManager.StartTask("OCS", "SET_OCS_USER_GENERAL_SETTINGS");
            try
            {
                OCSServer ocs = GetOCSProxy(itemId);
                ocs.SetUserGeneralSettings(instanceId, enabledForFederation, enabledForPublicIMConnectivity,
                                           archiveInternalCommunications, archiveFederatedCommunications,
                                           enabledForEnhancedPresence);
            }
            catch(Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            TaskManager.CompleteTask();

        }
    }
}
