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

using FuseCP.Server.Utils;
using System;
using System.DirectoryServices;
using System.Security.Principal;
using System.Threading;

namespace FuseCP.Providers.HostedSolution
{
    public class ADPermission
    {
        public static SecurityIdentifier EveryoneIdentity => new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        public static SecurityIdentifier AuthenticatedUsersIdentity => new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
        public static SecurityIdentifier PreWindows2000Identity => new SecurityIdentifier(WellKnownSidType.BuiltinPreWindows2000CompatibleAccessSid, null);

        public static void SetOUAclPermissions(OrganizationProvider orgProvider, string organizationId, string rootDomain, string rootDomainPath, string parentdomain)
        {
            string OUPath = orgProvider.GetOrganizationPath(organizationId);

            string dSHeuristicsDomain = rootDomain;

            if (!string.IsNullOrEmpty(parentdomain))
            {
                dSHeuristicsDomain = parentdomain;
                Log.WriteInfo("SetOUAclPermissions Parentdomain found: {0}", parentdomain);
            }
            else
            {
                Log.WriteInfo("SetOUAclPermissions Parentdomain not found {0}", parentdomain);
            }


            string dSHeuristicsOU = orgProvider.GetdSHeuristicsOU(dSHeuristicsDomain);
            Log.WriteInfo("dSHeuristicsOU: {0}", dSHeuristicsOU);

            DirectoryEntry GetdSHeuristicspath = new DirectoryEntry(dSHeuristicsOU);
            object DSObject = ActiveDirectoryUtils.GetADObjectProperty(GetdSHeuristicspath, "dSHeuristics") ?? "notset";
            string dSHeuristics = DSObject.ToString();
            Log.WriteInfo("dSHeuristics is : {0}", dSHeuristics);

            if (dSHeuristics is "001")
            {
                ActiveDirectoryUtils.DisableInheritance(OUPath);

                Log.WriteInfo("Removing PreWindows2000Identity from OU");
                ActiveDirectoryUtils.RemoveIdentityAllows(OUPath, PreWindows2000Identity);



                Log.WriteInfo("RemoveIdentityAllows for Everyone\n SID: {0}", EveryoneIdentity.ToString());
                ActiveDirectoryUtils.RemoveIdentityAllows(OUPath, EveryoneIdentity);
                Log.WriteInfo("RemoveIdentityAllows for AuthenticatedUsers\n SID: {0}", AuthenticatedUsersIdentity.ToString());
                ActiveDirectoryUtils.RemoveIdentityAllows(OUPath, AuthenticatedUsersIdentity);

                Log.WriteInfo("Changes for Exchange Servers: Recipient Management");
                var exchServers = ActiveDirectoryUtils.GetObjectTargetAccountName("Recipient Management", rootDomain);
                Log.WriteInfo("Recipient Management Exchange Servers: {0} ", exchServers);
                if (ActiveDirectoryUtils.AccountExists(exchServers))
                    ActiveDirectoryUtils.AddPermission(OUPath, new NTAccount(exchServers), ActiveDirectoryRights.GenericAll);

                Log.WriteInfo("Changes for Exchange Servers: Public Folder Management");
                exchServers = ActiveDirectoryUtils.GetObjectTargetAccountName("Public Folder Management", rootDomain);
                Log.WriteInfo("Public Folder Management Exchange Servers: {0} ", exchServers);
                if (ActiveDirectoryUtils.AccountExists(exchServers))
                    ActiveDirectoryUtils.AddPermission(OUPath, new NTAccount(exchServers), ActiveDirectoryRights.GenericAll);

                Log.WriteInfo("Completed Changes for Exchange Servers");

                var groupAccount = ActiveDirectoryUtils.GetObjectTargetAccountName(organizationId, orgProvider.RootDomain);
                Log.WriteInfo("Changes for GroupAccount: {0}", groupAccount.ToString());
                for (int i = 0; i <= 25; i++)
                {
                    if (ActiveDirectoryUtils.AccountExists(groupAccount))
                    {
                        ActiveDirectoryUtils.AddOrgPermisionsToIdentity(OUPath, new NTAccount(groupAccount));
                        break;
                    }

                    if (i == 25)
                        throw new Exception($"Can not find {groupAccount} group to set ACL permissions after {i * 2} seconds. Set Acl permissions manually");

                    Thread.Sleep(2000);
                }

                var privilegedGroup = ActiveDirectoryUtils.GetObjectTargetAccountName("Privileged Services", rootDomain);
                if (!ActiveDirectoryUtils.AccountExists(privilegedGroup))
                    ActiveDirectoryUtils.CreateGroup(rootDomainPath, "Privileged Services");

                ActiveDirectoryUtils.AddPermission(OUPath, new NTAccount(privilegedGroup), ActiveDirectoryRights.GenericRead);
            }
        }
    }
}
