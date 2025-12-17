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
using System.Text;

namespace FuseCP.EnterpriseServer
{
    public class ApplicationInfo
    {
        private string categoryName;

        private string id;
        private string folder;
        private string codebase;
        private string settingsControl;

        private string name;
        private string shortDescription;
        private string fullDescription;
        private string version;
        private string logo;
        private int size;
        private string homeSite;
        private string supportSite;
        private string docsSite;
        private string manufacturer;
        private string license;
        private ApplicationRequirement[] requirements;
        private ApplicationWebSetting[] webSettings;

        private int installationsNumber;

        public ApplicationInfo()
        {
        }

        public string HomeSite
        {
            get { return this.homeSite; }
            set { this.homeSite = value; }
        }

        public string SupportSite
        {
            get { return this.supportSite; }
            set { this.supportSite = value; }
        }

        public string ShortDescription
        {
            get { return this.shortDescription; }
            set { this.shortDescription = value; }
        }

        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        public string CategoryName
        {
            get { return this.categoryName; }
            set { this.categoryName = value; }
        }

        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public string DocsSite
        {
            get { return this.docsSite; }
            set { this.docsSite = value; }
        }

        public string FullDescription
        {
            get { return this.fullDescription; }
            set { this.fullDescription = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Codebase
        {
            get { return this.codebase; }
            set { this.codebase = value; }
        }

        public string Logo
        {
            get { return this.logo; }
            set { this.logo = value; }
        }

        public int InstallationsNumber
        {
            get { return this.installationsNumber; }
            set { this.installationsNumber = value; }
        }

        public string SettingsControl
        {
            get { return this.settingsControl; }
            set { this.settingsControl = value; }
        }

        public string Folder
        {
            get { return this.folder; }
            set { this.folder = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Manufacturer
        {
            get { return this.manufacturer; }
            set { this.manufacturer = value; }
        }

        public string License
        {
            get { return this.license; }
            set { this.license = value; }
        }

        public ApplicationWebSetting[] WebSettings
        {
            get { return this.webSettings; }
            set { this.webSettings = value; }
        }

        public ApplicationRequirement[] Requirements
        {
            get { return this.requirements; }
            set { this.requirements = value; }
        }
    }

    public class ApplicationRequirement
    {
        private string[] groups;
        private string[] quotas;
        private bool display;

        public string[] Groups
        {
            get { return this.groups; }
            set { this.groups = value; }
        }

        public string[] Quotas
        {
            get { return this.quotas; }
            set { this.quotas = value; }
        }

        public bool Display
        {
            get { return this.display; }
            set { this.display = value; }
        }
    }

    public class ApplicationWebSetting
    {
        private string name;
        private string value;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }

    public class ApplicationCategory
    {
        private string id;
        private string name;
        private string[] applications;

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string[] Applications
        {
            get { return this.applications; }
            set { this.applications = value; }
        }
    }
}
