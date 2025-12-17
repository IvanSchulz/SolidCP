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
	public class OCSServer : HostingServiceProviderWebService
	{
		private IOCSServer OCS
		{
			get { return (IOCSServer)Provider; }
		}



		#region Users

		[WebMethod, SoapHeader("settings")]
		public string CreateUser(string userUpn, string userDistinguishedName)
		{
			try
			{
				Log.WriteStart("{0}.CreateUser", ProviderSettings.ProviderName);
				string ret = OCS.CreateUser(userUpn, userDistinguishedName);
				Log.WriteEnd("{0}.CreateUser", ProviderSettings.ProviderName);
				return ret;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.CreateUser", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public OCSUser GetUserGeneralSettings(string instanceId)
		{
			try
			{
				Log.WriteStart("{0}.GetUserGeneralSettings", ProviderSettings.ProviderName);
				OCSUser ret = OCS.GetUserGeneralSettings(instanceId);
				Log.WriteEnd("{0}.GetUserGeneralSettings", ProviderSettings.ProviderName);
				return ret;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.GetUserGeneralSettings", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetUserGeneralSettings(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
		{
			try
			{
				Log.WriteStart("{0}.SetUserGeneralSettings", ProviderSettings.ProviderName);
				OCS.SetUserGeneralSettings(instanceId, enabledForFederation, enabledForPublicIMConectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
				Log.WriteEnd("{0}.SetUserGeneralSettings", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.SetUserGeneralSettings", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void DeleteUser(string instanceId)
		{
			try
			{
				Log.WriteStart("{0}.DeleteUser", ProviderSettings.ProviderName);
				OCS.DeleteUser(instanceId);
				Log.WriteEnd("{0}.DeleteUser", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.DeleteUser", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetUserPrimaryUri(string instanceId, string userUpn)
		{
			try
			{
				Log.WriteStart("{0}.SetUserPrimaryUri", ProviderSettings.ProviderName);
				OCS.SetUserPrimaryUri(instanceId, userUpn);
				Log.WriteEnd("{0}.SetUserPrimaryUri", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.SetUserPrimaryUri", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		#endregion

	}
}
