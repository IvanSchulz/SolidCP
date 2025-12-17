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
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Runtime.Serialization;

namespace FuseCP.Providers.WebAppGallery
{
    [Flags]
    public enum GalleryApplicationWellKnownDependency
    {
        None = 0,
        PHP = 1,
        AspNet20 = 2,
        AspNet40 = 4,
        SQL = 8,
        MySQL = 16
    }

	public class Author
	{
		[XmlElement(ElementName="name", Namespace="http://www.w3.org/2005/Atom")]
		public string Name { get; set; }
		[XmlElement(ElementName = "uri", Namespace = "http://www.w3.org/2005/Atom")]
		public string Uri { get; set; }
	}

	public class MsDeploy
	{
		[XmlElement(ElementName = "startPage", Namespace = "http://www.w3.org/2005/Atom")]
		public string StartPage { get; set; }
	}

	public class InstallerFile
	{
		[XmlElement(ElementName = "fileSize", Namespace = "http://www.w3.org/2005/Atom")]
		public string FileSize { get; set; }
		[XmlElement(ElementName = "installerURL", Namespace = "http://www.w3.org/2005/Atom")]
		public string InstallerUrl { get; set; }
		[XmlElement(ElementName = "displayURL", Namespace = "http://www.w3.org/2005/Atom")]
		public string DisplayUrl { get; set; }
		[XmlElement(ElementName = "md5", Namespace = "http://www.w3.org/2005/Atom")]
		public string MD5 { get; set; }
		[XmlElement(ElementName = "sha1", Namespace = "http://www.w3.org/2005/Atom")]
		public string SHA1 { get; set; }
	}

	public class Installer
	{
		[XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
		public string Id { get; set; }
		[XmlElement(ElementName = "languageId", Namespace = "http://www.w3.org/2005/Atom")]
		public string LanguageId { get; set; }
		[XmlElement(ElementName = "installerFile", Namespace = "http://www.w3.org/2005/Atom")]
		public InstallerFile InstallerFile { get; set; }
		[XmlElement(ElementName = "msDeploy", Namespace = "http://www.w3.org/2005/Atom")]
		public MsDeploy MsDeploy { get; set; }
	}

	public class InstallerItem
	{
		[XmlElement(ElementName = "installerFile", Namespace = "http://www.w3.org/2005/Atom")]
		public InstallerFile InstallerFile { get; set; }
		[XmlElement(ElementName = "installerItemId", Namespace = "http://www.w3.org/2005/Atom")]
		public string InstallerItemId { get; set; }
		[XmlElement(ElementName = "languageId", Namespace = "http://www.w3.org/2005/Atom")]
		public string LanguageId { get; set; }
		[XmlElement(ElementName = "helpLink", Namespace = "http://www.w3.org/2005/Atom")]
		public string HelpLink { get; set; }
		[XmlElement(ElementName = "msDeploy", Namespace = "http://www.w3.org/2005/Atom")]
		public MsDeploy MsDeploy { get; set; }
	}

	public class Dependency
	{
		[XmlElement(ElementName = "productId", Namespace = "http://www.w3.org/2005/Atom")]
		public string ProductId { get; set; }

        [XmlAttribute(AttributeName = "idref", Namespace = "http://www.w3.org/2005/Atom")]
        public string IdRef { get; set; }

		#region Version 0.2
		[XmlArray(ElementName = "logicalAnd", Namespace = "http://www.w3.org/2005/Atom"),
		XmlArrayItem(ElementName = "dependency")]
		public List<Dependency> LogicalAnd { get; set; }

		[XmlArray(ElementName = "logicalOr", Namespace = "http://www.w3.org/2005/Atom"),
		XmlArrayItem(ElementName = "dependency")]
		public List<Dependency> LogicalOr { get; set; }
		#endregion

		#region Version 2.0.1.0
		[XmlArray(ElementName = "and", Namespace = "http://www.w3.org/2005/Atom"),
		XmlArrayItem(ElementName = "dependency")]
		public List<Dependency> And { get; set; }

		[XmlArray(ElementName = "or", Namespace = "http://www.w3.org/2005/Atom"),
		XmlArrayItem(ElementName = "dependency")]
		public List<Dependency> Or { get; set; }

		#endregion
	}

	[XmlRoot("entry", Namespace = "http://www.w3.org/2005/Atom")]
	[DataContract(Namespace = "http://www.w3.org/2005/Atom", Name ="entry")]
    public class GalleryApplication
    {
		[XmlElement(ElementName = "productId", Namespace = "http://www.w3.org/2005/Atom")]
		[DataMember(Name ="productId")]
		public string Id { get; set; }

		[XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
		[DataMember(Name ="title")]
        public string Title { get; set; }

		[XmlElement(ElementName = "version", Namespace = "http://www.w3.org/2005/Atom")]
		[DataMember(Name ="version")]
        public string Version { get; set; }

		[XmlElement(ElementName = "summary", Namespace = "http://www.w3.org/2005/Atom")]
        [DataMember(Name = "summary")]
		public string Summary { get; set; }

		[XmlElement(ElementName = "longSummary", Namespace = "http://www.w3.org/2005/Atom")]
        [DataMember(Name = "longSummary")]
		public string Description { get; set; }

		public string Link { get; set; }

		[XmlElement(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
		[DataMember(Name ="author")]
		public Author Author { get; set; }

		[XmlElement(ElementName = "productFamily", Namespace = "http://www.w3.org/2005/Atom")]
		[DataMember(Name = "productFamily")]
		public string ProductFamily { get; set; }

		[XmlArray(ElementName="keywords"), XmlArrayItem(ElementName="item")]
		[DataMember(Name = "keywords")]
		public List<string> Keywords { get; set; }

		[XmlArray(ElementName = "installerItems"), XmlArrayItem(ElementName = "installerItem")]
		[DataMember(Name = "installerItems")]
		public List<InstallerItem> InstallerItems { get; set; }

		[XmlArray(ElementName = "installers"), XmlArrayItem(ElementName = "installer")]
		[DataMember(Name = "installers")]
		public List<Installer> Installers { get; set; }

		[XmlElement(ElementName = "dependency", Namespace = "http://www.w3.org/2005/Atom")]
		[DataMember(Name = "dependency")]
		public Dependency Dependency { get; set; }

        public GalleryApplicationWellKnownDependency WellKnownDependencies { get; set; }

		public DateTime LastUpdated { get; set; }

		public DateTime Published { get; set; }

		[XmlIgnore, IgnoreDataMember]
        public string DownloadUrl
		{
			get
			{
				if (InstallerItems.Count > 0)
					return InstallerItems[0].InstallerFile.InstallerUrl;
				else if (Installers.Count > 0)
					return Installers[0].InstallerFile.InstallerUrl;
				else
					return "N/A";
			}
		}

        public string IconUrl { get; set; }

		[XmlIgnore, IgnoreDataMember]
		public string StartPage
		{
			get
			{
				if (InstallerItems.Count > 0)
					return InstallerItems[0].MsDeploy.StartPage;
				else if (Installers.Count > 0)
					return Installers[0].MsDeploy.StartPage;
				else
					return String.Empty;
			}
		}

        [XmlIgnore, IgnoreDataMember]
        public string Size
		{
			get
			{
				if (InstallerItems.Count > 0)
					return InstallerItems[0].InstallerFile.FileSize;
				else if (Installers.Count > 0)
					return Installers[0].InstallerFile.FileSize;
				else
					return "0";
			}
		}

        [XmlElement(ElementName = "installerFileSize", Namespace = "http://www.w3.org/2005/Atom")]
        [DataMember(Name = "installerFileSize")]
		public int InstallerFileSize { get; set; }


		public string AuthorName {
			get { return Author.Name; }
		}

		public string AuthorUrl {
			get { return Author.Uri; }
		}

		[XmlElement("pageName", Namespace = "http://www.w3.org/2005/Atom")]
		[DataMember(Name = "pageName")]
		public string PageName { get; set; }
    }

	public enum GalleryWebAppStatus
	{
		NotDownloaded,
		Downloaded,
		Downloading,
        UnauthorizedAccessException,
		Failed
	}

	public enum UseDatabase
	{
		None,
		Sql,
		MySql
	}
}
