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

namespace FuseCP.Providers.Statistics
{
	/// <summary>
	/// Summary description for StatisticsItem.
	/// </summary>
	[Serializable]
	public class StatsSite : ServiceProviderItem
	{
        private string siteId;
        private string logDirectory;
        private string exportPath;
        private string exportPathUrl;
        private int timeZoneId;
        private string[] domainAliases;
        private StatsUser[] users;
        private string statisticsUrl;
        private string status;

        public StatsSite()
		{
		}

        public string StatisticsUrl
        {
            get { return this.statisticsUrl; }
            set { this.statisticsUrl = value; }
        }

        [Persistent]
        public string SiteId
        {
            get { return this.siteId; }
            set { this.siteId = value; }
        }

        public string LogDirectory
        {
            get { return this.logDirectory; }
            set { this.logDirectory = value; }
        }

        public string ExportPath
        {
            get { return this.exportPath; }
            set { this.exportPath = value; }
        }

        public string ExportPathUrl
        {
            get { return this.exportPathUrl; }
            set { this.exportPathUrl = value; }
        }

        public int TimeZoneId
        {
            get { return this.timeZoneId; }
            set { this.timeZoneId = value; }
        }

        public string[] DomainAliases
        {
            get { return this.domainAliases; }
            set { this.domainAliases = value; }
        }

        public StatsUser[] Users
        {
            get { return this.users; }
            set { this.users = value; }
        }

        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
	}
}
