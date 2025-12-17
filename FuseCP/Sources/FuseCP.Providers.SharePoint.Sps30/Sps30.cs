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
using System.IO;
using Microsoft.Win32;

namespace FuseCP.Providers.SharePoint
{
	public class Sps30 : Sps20
	{
        private static readonly string Wss3RegistryKey = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\12.0";
        
        #region Constants
		private const string SHAREPOINT_REGLOC = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\12.0";
		#endregion

		#region Sites
		public override void ExtendVirtualServer(SharePointSite site)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.ExtendVirtualServer(site, ExclusiveNTLM);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}

		public override void UnextendVirtualServer(string url, bool deleteContent)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.UnextendVirtualServer(url, deleteContent);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}
		#endregion

		#region Backup/Restore
		public override string BackupVirtualServer(string url, string fileName, bool zipBackup)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				return wss.BackupVirtualServer(url, fileName, zipBackup);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}

		public override void RestoreVirtualServer(string url, string fileName)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.RestoreVirtualServer(url, fileName);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}
		#endregion

		#region Web Parts
		public override string[] GetInstalledWebParts(string url)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				return wss.GetInstalledWebParts(url);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}

		public override void InstallWebPartsPackage(string url, string fileName)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.InstallWebPartsPackage(url, fileName);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}

		public override void DeleteWebPartsPackage(string url, string packageName)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.DeleteWebPartsPackage(url, packageName);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}
		#endregion

		#region Private Helpers
		protected override string GetAdminToolPath()
		{
			RegistryKey spKey = Registry.LocalMachine.OpenSubKey(SHAREPOINT_REGLOC);
			if (spKey == null)
				throw new Exception("SharePoint Services is not installed on the system");

			return ((string)spKey.GetValue("Location")) + @"\bin\stsadm.exe";
		}

		protected override bool IsSharePointInstalled()
		{
			RegistryKey spKey = Registry.LocalMachine.OpenSubKey(SHAREPOINT_REGLOC);
			if (spKey == null)
				return false;

			string spVal = (string)spKey.GetValue("SharePoint");
			return (String.Compare(spVal, "installed", true) == 0);
		}

		private AppDomain CreateAppDomain()
		{
			string binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

			AppDomainSetup info = new AppDomainSetup();
			info.ApplicationBase = binPath;

			return AppDomain.CreateDomain("WSS30", null, info);
		}

		private Sps30Remote CreateSps30Remote(AppDomain domain)
		{
			return (Sps30Remote)domain.CreateInstanceAndUnwrap(
				typeof(Sps30Remote).Assembly.FullName,
				typeof(Sps30Remote).FullName);
		}
		#endregion

        public override bool IsInstalled()
        {
            RegistryKey spKey = Registry.LocalMachine.OpenSubKey(Wss3RegistryKey);
            if (spKey == null)
            {
                return false;
            }

            string spVal = (string)spKey.GetValue("SharePoint");
            return (String.Compare(spVal, "installed", true) == 0);
        }
	}
}
