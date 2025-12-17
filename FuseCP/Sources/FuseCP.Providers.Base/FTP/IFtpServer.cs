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
using System.Collections;

namespace FuseCP.Providers.FTP
{
	/// <summary>
	/// Summary description for IFtpServer.
	/// </summary>
	public interface IFtpServer
	{
		// sites
		void ChangeSiteState(string siteId, ServerState state);
		ServerState GetSiteState(string siteId);
		bool SiteExists(string siteId);
		FtpSite[] GetSites();
		FtpSite GetSite(string siteId);
		string CreateSite(FtpSite site);
		void UpdateSite(FtpSite site);
		void DeleteSite(string siteId);

		// accounts
		bool AccountExists(string accountName);
		FtpAccount[] GetAccounts();
		FtpAccount GetAccount(string accountName);
		void CreateAccount(FtpAccount account);
		void UpdateAccount(FtpAccount account);
		void DeleteAccount(string accountName);
	}
}
