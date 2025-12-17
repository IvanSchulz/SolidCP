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

namespace FuseCP.Portal
{
    /// <summary>
    /// Summary description for ServerItemsHelper
    /// </summary>
    public class ServiceItemsHelper
    {
        #region Web Sites
        DataSet dsItemsPaged;

        public int GetServiceItemsPagedCount(int packageId, string groupName, string typeName,
            int serverId, bool recursive, string filterColumn, string filterValue)
        {
            return (int)dsItemsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetServiceItemsPaged(int packageId, string groupName, string typeName,
            int serverId, bool recursive, string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            dsItemsPaged = ES.Services.Packages.GetRawPackageItemsPaged(packageId, groupName, typeName, serverId,
                recursive, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return dsItemsPaged.Tables[1];
        }
        #endregion

        #region Web Sites
        DataSet dsWebSitesPaged;

        public int GetWebSitesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsWebSitesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetWebSitesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsWebSitesPaged = ES.Services.WebServers.GetRawWebSitesPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsWebSitesPaged.Tables[1];
        }
        #endregion

        #region Ftp Accounts
        DataSet dsFtpAccountsPaged;

        public int GetFtpAccountsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsFtpAccountsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetFtpAccountsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsFtpAccountsPaged = ES.Services.FtpServers.GetRawFtpAccountsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsFtpAccountsPaged.Tables[1];
        }
        #endregion

        #region Mail Accounts
        DataSet dsMailAccountsPaged;

        public int GetMailAccountsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailAccountsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailAccountsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailAccountsPaged = ES.Services.MailServers.GetRawMailAccountsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailAccountsPaged.Tables[1];
        }
        #endregion

        #region Mail Forwardings
        DataSet dsMailForwardingsPaged;

        public int GetMailForwardingsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailForwardingsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailForwardingsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailForwardingsPaged = ES.Services.MailServers.GetRawMailForwardingsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailForwardingsPaged.Tables[1];
        }
        #endregion

        #region Mail Groups
        DataSet dsMailGroupsPaged;

        public int GetMailGroupsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailGroupsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailGroupsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailGroupsPaged = ES.Services.MailServers.GetRawMailGroupsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailGroupsPaged.Tables[1];
        }
        #endregion

        #region Mail Lists
        DataSet dsMailListsPaged;

        public int GetMailListsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailListsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailListsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailListsPaged = ES.Services.MailServers.GetRawMailListsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailListsPaged.Tables[1];
        }
        #endregion

        #region Mail Domains
        DataSet dsMailDomainsPaged;

        public int GetMailDomainsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailDomainsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailDomainsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailDomainsPaged = ES.Services.MailServers.GetRawMailDomainsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailDomainsPaged.Tables[1];
        }
        #endregion

        #region Databases
        DataSet dsSqlDatabasesPaged;

        public int GetSqlDatabasesPagedCount(string groupName, string filterColumn, string filterValue)
        {
            return (int)dsSqlDatabasesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSqlDatabasesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string groupName, string filterColumn, string filterValue)
        {
            dsSqlDatabasesPaged = ES.Services.DatabaseServers.GetRawSqlDatabasesPaged(PanelSecurity.PackageId,
                groupName, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return dsSqlDatabasesPaged.Tables[1];
        }
        #endregion

        #region Database Users
        DataSet dsSqlUsersPaged;

        public int GetSqlUsersPagedCount(string groupName, string filterColumn, string filterValue)
        {
            return (int)dsSqlUsersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSqlUsersPaged(int maximumRows, int startRowIndex, string sortColumn,
            string groupName, string filterColumn, string filterValue)
        {
            dsSqlUsersPaged = ES.Services.DatabaseServers.GetRawSqlUsersPaged(PanelSecurity.PackageId,
                groupName, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return dsSqlUsersPaged.Tables[1];
        }
        #endregion

        #region SharePoint Users
        DataSet dsSharePointUsersPaged;

        public int GetSharePointUsersPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsSharePointUsersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSharePointUsersPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsSharePointUsersPaged = ES.Services.SharePointServers.GetRawSharePointUsersPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSharePointUsersPaged.Tables[1];
        }
        #endregion

        #region SharePoint Groups
        DataSet dsSharePointGroupsPaged;

        public int GetSharePointGroupsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsSharePointGroupsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSharePointGroupsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsSharePointGroupsPaged = ES.Services.SharePointServers.GetRawSharePointGroupsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSharePointGroupsPaged.Tables[1];
        }
        #endregion

        #region Statistics Items
        DataSet dsStatisticsItemsPaged;

        public int GetStatisticsSitesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsStatisticsItemsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetStatisticsSitesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsStatisticsItemsPaged = ES.Services.StatisticsServers.GetRawStatisticsSitesPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsStatisticsItemsPaged.Tables[1];
        }
        #endregion

        #region SharePoint Sites
        DataSet dsSharePointSitesPaged;

        public int GetSharePointSitesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsSharePointSitesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSharePointSitesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsSharePointSitesPaged = ES.Services.SharePointServers.GetRawSharePointSitesPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSharePointSitesPaged.Tables[1];
        }
        #endregion

        #region ODBC DSNs
        DataSet dsOdbcSourcesPaged;

        public int GetOdbcSourcesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsOdbcSourcesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetOdbcSourcesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsOdbcSourcesPaged = ES.Services.OperatingSystems.GetRawOdbcSourcesPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsOdbcSourcesPaged.Tables[1];
        }
        #endregion

        #region Shared SSL Folders
        DataSet dsSharedSSLFoldersPaged;

        public int GetSharedSSLFoldersPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsSharedSSLFoldersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSharedSSLFoldersPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsSharedSSLFoldersPaged = ES.Services.WebServers.GetRawSSLFoldersPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSharedSSLFoldersPaged.Tables[1];
        }
        #endregion
    }
}
