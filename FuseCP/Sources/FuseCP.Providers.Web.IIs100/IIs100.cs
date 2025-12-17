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

using Microsoft.Web.Administration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using FuseCP.Providers.Common;
using FuseCP.Providers.Web.Iis;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Providers.Web
{
	public class IIs100 : IIs70
	{
		private SslFlags SSLFlags
		{
			get
			{
				return (UseSni ? SslFlags.Sni : SslFlags.None) | (UseCcs ? SslFlags.CentralCertStore : SslFlags.None);
			}
		}

		public string CCSUncPath
		{
			get { return ProviderSettings["SSLCCSUNCPath"]; }
		}

		public string CCSCommonPassword
		{
			get { return ProviderSettings["SSLCCSCommonPassword"]; }
		}

		public bool UseSni
		{
			get
			{
				try
				{
					return Convert.ToBoolean(ProviderSettings["SSLUseSNI"]);
				}
				catch
				{
					return false;
				}
			}
		}

		public bool UseCcs
		{
			get
			{
				try
				{
					return Convert.ToBoolean(ProviderSettings["SSLUseCCS"]);
				}
				catch
				{
					return false;
				}
			}
		}

		public override SettingPair[] GetProviderDefaultSettings()
		{
			var allSettings = new List<SettingPair>();
			allSettings.AddRange(base.GetProviderDefaultSettings());

			// Add these to get some default values in. These are also used a marker in the IIS70_Settings.ascx.cs to know that it is the IIS80 provider that is used
			allSettings.Add(new SettingPair("SSLUseCCS", false.ToString()));
			allSettings.Add(new SettingPair("SSLUseSNI", false.ToString()));
			allSettings.Add(new SettingPair("SSLCCSUNCPath", ""));
			allSettings.Add(new SettingPair("SSLCCSCommonPassword", ""));

			return allSettings.ToArray();
		}

		public override string[] Install()
		{
			var messages = new List<string>();

			messages.AddRange(base.Install());

			// TODO: Setup ccs

			return messages.ToArray();
		}

		public override bool IsIISInstalled() => GetIISVersion() >= 10;

		public override bool CheckCertificate(WebSite webSite)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.CheckCertificate(webSite);
		}

		public override ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.DeleteCertificate(certificate, website);
		}

		public override SSLCertificate InstallPFX(byte[] certificate, string password, WebSite website)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.InstallPfx(certificate, password, website);
		}

		public override SSLCertificate ImportCertificate(WebSite website)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.ImportCertificate(website);
		}

		public override byte[] ExportCertificate(string serialNumber, string password)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.ExportPfx(serialNumber, password);
		}

		public override SSLCertificate GenerateCSR(SSLCertificate certificate)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			sslObjectService.GenerateCsr(certificate);

			return certificate;
		}

		public override List<SSLCertificate> GetServerCertificates()
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.GetServerCertificates();
		}

		public override SSLCertificate InstallCertificate(SSLCertificate certificate, WebSite website)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.InstallCertificate(certificate, website);
		}

		public override String LEInstallCertificate(WebSite website, string email)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.LEInstallCertificate(website, email);
		}

		public override WebSite GetSite(string siteId)
		{
			var site = base.GetSite(siteId);
			site.SniEnabled = UseSni;
			return site;
		}

		public override Version DefaultVersion => new Version(10, 0);
	}
}
