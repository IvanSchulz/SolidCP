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
using System.ComponentModel;
using FuseCP.Web.Services;
using FuseCP.Providers;
using FuseCP.Providers.HostedSolution;
using FuseCP.Server.Utils;

namespace FuseCP.Server
{
	/// <summary>
	/// OCS Web Service
	/// </summary>
	[WebService(Namespace = "http://smbsaas/fusecp/server/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[Policy("ServerPolicy")]
	[ToolboxItem(false)]
	public class OCSEdgeServer : HostingServiceProviderWebService
	{
		private IOCSEdgeServer OCS
		{
			get { return (IOCSEdgeServer)Provider; }
		}

		#region Domains
		[WebMethod, SoapHeader("settings")]
		public void AddDomain(string domainName)
		{
			try
			{
				Log.WriteStart("{0}.AddDomain", ProviderSettings.ProviderName);
				OCS.AddDomain(domainName);
				Log.WriteEnd("{0}.AddDomain", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.AddDomain", ProviderSettings.ProviderName), ex);
				throw;
			}
		}
		[WebMethod, SoapHeader("settings")]
		public void DeleteDomain(string domainName)
		{
			try
			{
				Log.WriteStart("{0}.DeleteDomain", ProviderSettings.ProviderName);
				OCS.DeleteDomain(domainName);
				Log.WriteEnd("{0}.DeleteDomain", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.DeleteDomain", ProviderSettings.ProviderName), ex);
				throw;
			}
		}
	
		#endregion

	}
}
