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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal
{
    public class OrganizationsHelper
    {
        #region Organizations
        DataSet orgs;

        public int GetOrganizationsPagedCount(int packageId,
            bool recursive, string filterColumn, string filterValue)
        {
            return (int)orgs.Tables[0].Rows[0][0];
        }

        public DataTable GetOrganizationsPaged(int packageId,
            bool recursive, string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";

            orgs = ES.Services.Organizations.GetRawOrganizationsPaged(packageId,
                recursive, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);
            
            return orgs.Tables[1];
        }

        //public Organization[] GetOrganizations(int packageId, bool recursive)
        //{
        //    return ES.Services.Organizations.GetOrganizations(packageId, recursive);
        //}

        public DataTable GetOrganizations(int packageId, bool recursive)
        {
            orgs = ES.Services.Organizations.GetRawOrganizationsPaged(packageId,
                recursive, "ItemName", "%", "ItemName", 0, int.MaxValue);

            return orgs.Tables[1];
        }
        #endregion

        #region Accounts
        OrganizationUsersPaged users;

        public int GetOrganizationUsersPagedCount(int itemId, 
            string filterColumn, string filterValue)
        {
            return users.RecordsCount;            
        }

        public OrganizationUser[] GetOrganizationUsersPaged(int itemId, 
            string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";
			if (maximumRows == 0)
			{
				maximumRows = Int32.MaxValue;
			}

            users = ES.Services.Organizations.GetOrganizationUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return users.PageUsers;            
        }

        OrganizationDeletedUsersPaged deletedUsers;

        public int GetOrganizationDeletedUsersPagedCount(int itemId,
            string filterColumn, string filterValue)
        {
            return deletedUsers.RecordsCount;
        }

        public OrganizationDeletedUser[] GetOrganizationDeletedUsersPaged(int itemId,
            string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";
            if (maximumRows == 0)
            {
                maximumRows = Int32.MaxValue;
            }

            deletedUsers = ES.Services.Organizations.GetOrganizationDeletedUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return deletedUsers.PageDeletedUsers;
        }

        #endregion

        #region Security Groups

        ExchangeAccountsPaged accounts;

        public int GetOrganizationSecurityGroupsPagedCount(int itemId, string accountTypes,
            string filterColumn, string filterValue)
        {
            return accounts.RecordsCount;
        }

        public ExchangeAccount[] GetOrganizationSecurityGroupsPaged(int itemId, string accountTypes,
            string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";

            accounts = ES.Services.Organizations.GetOrganizationSecurityGroupsPaged(itemId,
                filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return accounts.PageItems;
        }

        #endregion
    }
}
