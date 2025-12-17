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

namespace FuseCP.Providers.SharePoint
{
    /// <summary>
    /// Exposes functionality for share point server provider hosted in conjunction with organization management provider and 
    /// exchange server.
    /// </summary>
    public interface IHostedSharePointServerEnt
    {
        /// <summary>
        /// When implemented gets root web application uri.
        /// </summary>
        Uri Enterprise_RootWebApplicationUri
        {
            get;
        }

        /// <summary>
        /// When implemented gets list of supported languages by this installation of SharePoint.
        /// </summary>
        /// <returns>List of supported languages</returns>
        int[] Enterprise_GetSupportedLanguages();

        /// <summary>
        /// When implemented gets list of SharePoint collections within root web application.
        /// </summary>
        /// <returns>List of SharePoint collections within root web application.</returns>
        SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections();

        /// <summary>
        /// When implemented gets SharePoint collection within root web application with given name.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be loaded.</param>
        /// <returns>SharePoint collection within root web application with given name.</returns>
        SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url);

        /// <summary>
        /// When implemented creates site collection within predefined root web application.
        /// </summary>
        /// <param name="siteCollection">Information about site coolection to be created.</param>
        void Enterprise_CreateSiteCollection(SharePointEnterpriseSiteCollection siteCollection);

        /// <summary>
        /// When implemented deletes site collection under given url.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        void Enterprise_DeleteSiteCollection(SharePointEnterpriseSiteCollection siteCollection);

        /// <summary>
        /// When implemeneted backups site collection under give url.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        /// <param name="filename">Resulting backup file name.</param>
        /// <param name="zip">A value which shows whether created backup must be archived.</param>
        /// <returns>Created backup full path.</returns>
        string Enterprise_BackupSiteCollection(string url, string filename, bool zip);

        /// <summary>
        /// When implemented restores site collection under given url from backup.
        /// </summary>
        /// <param name="siteCollection">Site collection to be restored.</param>
        /// <param name="filename">Backup file name to restore from.</param>
        void Enterprise_RestoreSiteCollection(SharePointEnterpriseSiteCollection siteCollection, string filename);

        /// <summary>
        /// When implemented gets binary data chunk of specified size from specified offset.
        /// </summary>
        /// <param name="path">Path to file to get bunary data chunk from.</param>
        /// <param name="offset">Offset from which to start data reading.</param>
        /// <param name="length">Binary data chunk length.</param>
        /// <returns>Binary data chunk read from file.</returns>
        byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length);

        /// <summary>
        /// When implemented appends supplied binary data chunk to file.
        /// </summary>
        /// <param name="fileName">Non existent file name to append to.</param>
        /// <param name="path">Full path to existent file to append to.</param>
        /// <param name="chunk">Binary data chunk to append to.</param>
        /// <returns>Path to file that was appended with chunk.</returns>
        string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);

        void Enterprise_UpdateQuotas(string url, long maxStorage, long warningStorage);

        SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls);

        long Enterprise_GetSiteCollectionSize(string url);

        void Enterprise_SetPeoplePickerOu(string site, string ou);
    }
}
