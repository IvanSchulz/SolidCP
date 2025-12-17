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

using System.Collections.Generic;
using FuseCP.Providers.OS;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Providers.HostedSolution
{
    public interface IOrganization
    {
        Organization CreateOrganization(string organizationId);

        void DeleteOrganization(string organizationId);

        int CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled);

        void DisableUser(string loginName, string organizationId);

        void DeleteUser(string loginName, string organizationId);

        OrganizationUser GetUserGeneralSettings(string loginName, string organizationId);

        int CreateSecurityGroup(string organizationId, string groupName);

        OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId);

        string[] GetSecurityGroupsNotes(string[] groupNames, string organizationId);

        void DeleteSecurityGroup(string groupName, string organizationId);

        void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes);

        void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName);

        void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName);

        void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password,
                                    bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials,
                                    string lastName,
                                    string address, string city, string state, string zip, string country,
                                    string jobTitle,
                                    string company, string department, string office, string managerAccountName,
                                    string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
                                    string webPage, string notes, string externalEmail,
                                    bool userMustChangePassword);

        void SetUserPassword(string organizationId, string accountName, string password);

        void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName);

        bool OrganizationExists(string organizationId);

        void DeleteOrganizationDomain(string organizationDistinguishedName, string domain);

        void CreateOrganizationDomain(string organizationDistinguishedName, string domain);

        PasswordPolicyResult GetPasswordPolicy();

        string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName);

        bool DoesSamAccountNameExist(string accountName);

        MappedDrive[] GetDriveMaps(string organizationId);

        int CreateMappedDrive(string organizationId, string drive, string labelAs, string path);

        void DeleteMappedDrive(string organizationId, string drive);

        void DeleteMappedDriveByPath(string organizationId, string path);

        void DeleteMappedDrivesGPO(string organizationId);

        void SetDriveMapsTargetingFilter(string organizationId, ExchangeAccount[] accounts, string folderName);

        void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder);

        List<OrganizationUser> GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration);
        void ApplyPasswordSettings(string organizationId, OrganizationPasswordSettings passwordSettings);

        bool CheckPhoneNumberIsInUse(string phoneNumber, string userSamAccountName = null);

        OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId);

        void SetOUAclPermissions(string organizationId);

        ExchangeAccount[] GetUserGroups(string userName, int ouPath);
    }
}
