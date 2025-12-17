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
using System.ComponentModel;
using FuseCP.Web.Services;
using FuseCP.Providers;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.SharePoint;
using FuseCP.Server.Utils;

namespace FuseCP.Server
{
    /// <summary>
    /// Summary description for HostedSharePointServerEnt
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class HostedSharePointServerEnt : HostingServiceProviderWebService
    {
        private delegate TReturn Action<TReturn>();

        /// <summary>
        /// Gets hosted SharePoint provider instance.
        /// </summary>
        private IHostedSharePointServerEnt HostedSharePointServerEntProvider
        {
            get { return (IHostedSharePointServerEnt)Provider; }
        }

        /// <summary>
        /// Gets list of supported languages by this installation of SharePoint.
        /// </summary>
        /// <returns>List of supported languages</returns>
        [WebMethod, SoapHeader("settings")]
        public int[] Enterprise_GetSupportedLanguages()
        {
            return ExecuteAction<int[]>(delegate
            {
                return HostedSharePointServerEntProvider.Enterprise_GetSupportedLanguages();
            }, "GetSupportedLanguages");
        }


        /// <summary>
        /// Gets list of SharePoint collections within root web application.
        /// </summary>
        /// <returns>List of SharePoint collections within root web application.</returns>
        [WebMethod, SoapHeader("settings")]
        public SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections()
        {
            return ExecuteAction<SharePointEnterpriseSiteCollection[]>(delegate
            {
                return HostedSharePointServerEntProvider.Enterprise_GetSiteCollections();
            }, "GetSiteCollections");
        }

        /// <summary>
        /// Gets SharePoint collection within root web application with given name.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be loaded.</param>
        /// <returns>SharePoint collection within root web application with given name.</returns>
        [WebMethod, SoapHeader("settings")]
        public SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url)
        {
            return ExecuteAction<SharePointEnterpriseSiteCollection>(delegate
            {
                return HostedSharePointServerEntProvider.Enterprise_GetSiteCollection(url);
            }, "GetSiteCollection");
        }

        /// <summary>
        /// Creates site collection within predefined root web application.
        /// </summary>
        /// <param name="siteCollection">Information about site coolection to be created.</param>
        [WebMethod, SoapHeader("settings")]
        public void Enterprise_CreateSiteCollection(SharePointEnterpriseSiteCollection siteCollection)
        {
            siteCollection.OwnerLogin = AttachNetbiosDomainName(siteCollection.OwnerLogin);
            ExecuteAction<object>(delegate
            {
                HostedSharePointServerEntProvider.Enterprise_CreateSiteCollection(siteCollection);
                return new object();
            }, "CreateSiteCollection");
        }


        [WebMethod, SoapHeader("settings")]
        public void Enterprise_UpdateQuotas(string url, long maxSize, long warningSize)
        {
            ExecuteAction<object>(delegate
            {
                HostedSharePointServerEntProvider.Enterprise_UpdateQuotas(url, maxSize, warningSize);
                return new object();
            }, "UpdateQuotas");



        }

        [WebMethod, SoapHeader("settings")]
        public SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            SharePointSiteDiskSpace[] ret = null;
            ret = ExecuteAction<SharePointSiteDiskSpace[]>(delegate
            {
                return HostedSharePointServerEntProvider.Enterprise_CalculateSiteCollectionsDiskSpace(urls);
            }, "CalculateSiteCollectionDiskSpace");
            return ret;

        }
        /// <summary>
        /// Deletes site collection under given url.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        [WebMethod, SoapHeader("settings")]
        public void Enterprise_DeleteSiteCollection(SharePointEnterpriseSiteCollection siteCollection)
        {
            ExecuteAction<object>(delegate
            {
                HostedSharePointServerEntProvider.Enterprise_DeleteSiteCollection(siteCollection);
                return new object();
            }, "DeleteSiteCollection");
        }
        /// <summary>
        /// Backups site collection under give url.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        /// <param name="filename">Resulting backup file name.</param>
        /// <param name="zip">A value which shows whether created backup must be archived.</param>
        /// <returns>Created backup full path.</returns>
        [WebMethod, SoapHeader("settings")]
        public string Enterprise_BackupSiteCollection(string url, string filename, bool zip)
        {
            return ExecuteAction<string>(delegate
            {
                return
                    HostedSharePointServerEntProvider.Enterprise_BackupSiteCollection(url, filename, zip);
            }, "BackupSiteCollection");
        }

        /// <summary>
        /// Restores site collection under given url from backup.
        /// </summary>
        /// <param name="siteCollection">Site collection to be restored.</param>
        /// <param name="filename">Backup file name to restore from.</param>
        [WebMethod, SoapHeader("settings")]
        public void Enterprise_RestoreSiteCollection(SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            siteCollection.OwnerLogin = AttachNetbiosDomainName(siteCollection.OwnerLogin);
            ExecuteAction<object>(delegate
            {
                HostedSharePointServerEntProvider.Enterprise_RestoreSiteCollection(siteCollection, filename);
                return new object();
            }, "RestoreSiteCollection");
        }

        /// <summary>
        /// Gets binary data chunk of specified size from specified offset.
        /// </summary>
        /// <param name="path">Path to file to get bunary data chunk from.</param>
        /// <param name="offset">Offset from which to start data reading.</param>
        /// <param name="length">Binary data chunk length.</param>
        /// <returns>Binary data chunk read from file.</returns>
        [WebMethod, SoapHeader("settings")]
        public byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return ExecuteAction<byte[]>(delegate
            {
                return
                    HostedSharePointServerEntProvider.Enterprise_GetTempFileBinaryChunk(path, offset, length);
            }, "GetTempFileBinaryChunk");
        }

        /// <summary>
        /// Appends supplied binary data chunk to file.
        /// </summary>
        /// <param name="fileName">Non existent file name to append to.</param>
        /// <param name="path">Full path to existent file to append to.</param>
        /// <param name="chunk">Binary data chunk to append to.</param>
        /// <returns>Path to file that was appended with chunk.</returns>
        [WebMethod, SoapHeader("settings")]
        public virtual string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return ExecuteAction<string>(delegate
            {
                return
                    HostedSharePointServerEntProvider.Enterprise_AppendTempFileBinaryChunk(fileName, path, chunk);
            }, "AppendTempFileBinaryChunk");
        }


        [WebMethod, SoapHeader("settings")]
        public long Enterprise_GetSiteCollectionSize(string url)
        {
            return ExecuteAction<long>(delegate
            {
                return
                    HostedSharePointServerEntProvider.Enterprise_GetSiteCollectionSize(url);
            }, "GetSiteCollectionSize");
        }


        [WebMethod, SoapHeader("settings")]
        public void Enterprise_SetPeoplePickerOu(string site, string ou)
        {
            HostedSharePointServerEntProvider.Enterprise_SetPeoplePickerOu(site, ou);
        }


        /// <summary>
        /// Executes supplied action and performs logging.
        /// </summary>
        /// <typeparam name="TReturn">Type of action's return value.</typeparam>
        /// <param name="action">Action to be executed.</param>
        /// <param name="actionName">Action name for logging purposes.</param>
        /// <returns>Action execution result.</returns>
        private TReturn ExecuteAction<TReturn>(Action<TReturn> action, string actionName)
        {
            try
            {
                Log.WriteStart("'{0}' {1}", ProviderSettings.ProviderName, actionName);
                TReturn result = action();
                Log.WriteEnd("'{0}' {1}", ProviderSettings.ProviderName, actionName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't {1} '{0}' provider", ProviderSettings.ProviderName, actionName), ex);
                throw;
            }
        }

        /// <summary>
        /// Returns fully qualified netbios account name.
        /// </summary>
        /// <param name="accountName">Account name.</param>
        /// <returns>Fully qualified netbios account name.</returns>
        private string AttachNetbiosDomainName(string accountName)
        {
            string domainNetbiosName = String.Format("{0}\\", ActiveDirectoryUtils.GetNETBIOSDomainName(ServerSettings.ADRootDomain));
            return String.Format("{0}{1}", domainNetbiosName, accountName.Replace(domainNetbiosName, String.Empty));
        }
    }
}
