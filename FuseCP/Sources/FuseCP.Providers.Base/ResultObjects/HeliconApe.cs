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

namespace FuseCP.Providers.ResultObjects
{
    [Serializable]
    public class HeliconApeStatus
    {
		private string registrationInfo = String.Empty;
		private string version = String.Empty;
		private string installDir = String.Empty;

		public static HeliconApeStatus Empty = new HeliconApeStatus
		{
			IsEnabled = false,
			IsInstalled = false,
			IsRegistered = false,
			RegistrationInfo = String.Empty,
			Version = String.Empty,
		};

        public bool IsEnabled { get; set; }
        public bool IsInstalled { get; set; }
        public bool IsRegistered { get; set; }

		public string InstallDir
		{
			get { return installDir; }
			set { installDir = value; }
		}

		public string Version
		{
			get { return version; }
			set { version = value; }
		}

		public string RegistrationInfo
		{
			get { return registrationInfo; }
			set { registrationInfo = value; }
		}
    }
}
