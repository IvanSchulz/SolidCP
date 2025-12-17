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
using FuseCP.Providers.Web;
namespace FuseCP.Providers.OS
{
    /// <summary>
    /// Summary description for FileSystemItem.
    /// </summary>
    [Serializable]
    public class SystemFile : ServiceProviderItem
    {
        private string fullName;
        private DateTime created;
        private DateTime changed;
        private bool isDirectory;
        private long size;
        private long quota;
        private bool isEmpty;
        private bool isPublished;
        private WebDavFolderRule[] rules;
        private string url;
        private int fsrmQuotaMB;
        private int frsmQuotaGB;
        private QuotaType fsrmQuotaType = QuotaType.Soft;
        private string driveLetter;

        public SystemFile()
        {
        }

        public SystemFile(string name, string fullName, bool isDirectory, long size,
            DateTime created, DateTime changed)
        {
            this.Name = name;
            this.fullName = fullName;
            this.isDirectory = isDirectory;
            this.size = size;
            this.created = created;
            this.changed = changed;
        }

        public int FRSMQuotaMB
        {
            get { return fsrmQuotaMB; }
            set { fsrmQuotaMB = value; }
        }

        public int FRSMQuotaGB
        {
            get { return frsmQuotaGB; }
            set { frsmQuotaGB = value; }
        }

        public QuotaType FsrmQuotaType
        {
            get { return fsrmQuotaType; }
            set { fsrmQuotaType = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public DateTime Changed
        {
            get { return changed; }
            set { changed = value; }
        }

        public bool IsDirectory
        {
            get { return isDirectory; }
            set { isDirectory = value; }
        }

        public long Size
        {
            get { return size; }
            set { size = value; }
        }

        public long Quota
        {
            get { return quota; }
            set { quota = value; }
        }

        public bool IsEmpty
        {
            get { return this.isEmpty; }
            set { this.isEmpty = value; }
        }

        public bool IsPublished
        {
            get { return this.isPublished; }
            set { this.isPublished = value; }
        }

        public WebDavFolderRule[] Rules
        {
            get { return this.rules; }
            set { this.rules = value; }
        }

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        public string RelativeUrl { get; set; }
        public string Summary { get; set; }
        public int? StorageSpaceFolderId { get; set; }
        public string UncPath { get; set; }

        public string DriveLetter
        {
            get { return this.driveLetter; }
            set { this.driveLetter = value; }
        }
    }
}
