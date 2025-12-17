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

namespace FuseCP.Providers.Database
{
	public class SqlDatabase : ServiceProviderItem
	{
		private string dataName;
		private string dataPath;
		private int dataSize;
		private string logName;
		private string logPath;
		private int logSize;
		private string[] users;
        private string location;
        private string internalServerName;
        private string externalServerName;

		public SqlDatabase()
		{
		}

		public string DataName 
		{
			get { return dataName; }
			set { dataName = value; } 
		}

		public string DataPath 
		{
			get { return dataPath; }
			set { dataPath = value; } 
		}

		public int DataSize
		{
			get { return dataSize; }
			set { dataSize = value; }
		}

		public string LogName 
		{
			get { return logName; }
			set { logName = value; } 
		}

		public string LogPath 
		{
			get { return logPath; }
			set { logPath = value; } 
		}

		public int LogSize
		{
			get { return logSize; }
			set { logSize = value; }
		}

		public string[] Users
		{
			get { return users; }
			set { users = value; } 
		}

        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public string InternalServerName
        {
            get { return this.internalServerName; }
            set { this.internalServerName = value; }
        }

        public string ExternalServerName
        {
            get { return this.externalServerName; }
            set { this.externalServerName = value; }
        }


	}
}
