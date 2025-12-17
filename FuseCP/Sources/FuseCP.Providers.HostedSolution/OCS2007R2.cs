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
using System.Management;
using FuseCP.Providers.Utils;

namespace FuseCP.Providers.HostedSolution
{
	public class OCS2007R2 : HostingServiceProviderBase, IOCSServer
	{

		#region Properties

		/// <summary>
		/// Pool FQDN
		/// </summary>
		private string PoolFQDN
		{
			get { return ProviderSettings[OCSConstants.PoolFQDN]; }			
		}

		private WmiHelper wmi = null;
		
		/// <summary>
		/// Wmi helper instance
		/// </summary>
		private WmiHelper Wmi
		{
			get
			{
				if (wmi == null)
					wmi = new WmiHelper("root\\cimv2");
				return wmi;
			}

		}
		#endregion

		#region IOCSServer implementation

		public string CreateUser(string userUpn, string userDistinguishedName)
		{
			return CreateUserInternal(userUpn, userDistinguishedName);
		}

		public OCSUser GetUserGeneralSettings(string instanceId)
		{
			return GetUserGeneralSettingsInternal(instanceId);
		}

		public void SetUserGeneralSettings(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
		{
			SetUserGeneralSettingsInternal(instanceId, enabledForFederation, enabledForPublicIMConectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
		}

		public void DeleteUser(string instanceId)
		{
			DeleteUserInternal(instanceId);
		}

		public void SetUserPrimaryUri(string instanceId, string userUpn)
		{
			SetUserPrimaryUriInternal(instanceId, userUpn);
		}

		#endregion

		#region Users

		private void DeleteUserInternal(string instanceId)
		{
			HostedSolutionLog.LogStart("DeleteUserInternal");
			try
			{
				if (string.IsNullOrEmpty(instanceId))
					throw new ArgumentException("instanceId");

				using (ManagementObject userObject = GetUserByInstanceId(instanceId))
				{
					if (userObject == null)
					{
						HostedSolutionLog.LogWarning("OCS user {0} not found", instanceId);
					}
					else
					{
						userObject.Delete();
					}
				}
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("DeleteUserInternal", ex);
				throw;
			}
			HostedSolutionLog.LogEnd("DeleteUserInternal");
		}

		private void SetUserGeneralSettingsInternal(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
		{
			HostedSolutionLog.LogStart("SetUserGeneralSettingsInternal");
			try
			{
				if (string.IsNullOrEmpty(instanceId))
					throw new ArgumentException("instanceId");

				using (ManagementObject userObject = GetUserByInstanceId(instanceId))
				{
					if (userObject == null)
					{
						throw new Exception(string.Format("OCS user {0} not found", instanceId));
					}

					userObject["EnabledForFederation"] = enabledForFederation;
					userObject["PublicNetworkEnabled"] = enabledForPublicIMConectivity;
					userObject["ArchiveInternalCommunications"] = archiveInternalCommunications;
					userObject["ArchiveFederatedCommunications"] = archiveFederatedCommunications;
					if (enabledForEnhancedPresence)
						userObject["EnabledForEnhancedPresence"] = true;
					userObject.Put();

				}
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("SetUserGeneralSettingsInternal", ex);
				throw;
			}
			HostedSolutionLog.LogEnd("SetUserGeneralSettingsInternal");
		}

		private void SetUserPrimaryUriInternal(string instanceId, string userUpn)
		{
			HostedSolutionLog.LogStart("SetUserPrimaryUriInternal");
			try
			{
				if (string.IsNullOrEmpty(instanceId))
					throw new ArgumentException("instanceId");

				if (string.IsNullOrEmpty(userUpn))
					throw new ArgumentException("userUpn");

				using (ManagementObject userObject = GetUserByInstanceId(instanceId))
				{
					if (userObject == null)
					{
						throw new Exception(string.Format("OCS user {0} not found", instanceId));
					}
					string primaryUri = "sip:" + userUpn;
					userObject["PrimaryURI"] = primaryUri;
					userObject.Put();
				}
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("SetUserPrimaryUriInternal", ex);
				throw;
			}
			HostedSolutionLog.LogEnd("SetUserPrimaryUriInternal");
		}

		private OCSUser GetUserGeneralSettingsInternal(string instanceId)
		{
			HostedSolutionLog.LogStart("GetUserGeneralSettingsInternal");
			try
			{
				if (string.IsNullOrEmpty(instanceId))
					throw new ArgumentException("instanceId");

				using (ManagementObject userObject = GetUserByInstanceId(instanceId))
				{
					if (userObject == null)
					{
						throw new Exception(string.Format("OCS user {0} not found", instanceId));
					}

					OCSUser user = new OCSUser();
					user.InstanceId = instanceId;
					user.PrimaryUri = (string)userObject["PrimaryURI"];
					user.DisplayName = (string)userObject["DisplayName"];
					user.EnabledForFederation = (bool)userObject["EnabledForFederation"];
					user.EnabledForPublicIMConectivity = (bool)userObject["PublicNetworkEnabled"];
					user.ArchiveInternalCommunications = (bool)userObject["ArchiveInternalCommunications"];
					user.ArchiveFederatedCommunications = (bool)userObject["ArchiveFederatedCommunications"];
					user.EnabledForEnhancedPresence = (bool)userObject["EnabledForEnhancedPresence"];

					HostedSolutionLog.LogEnd("GetUserGeneralSettingsInternal");	

					return user;

				}
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("GetUserGeneralSettingsInternal", ex);
				throw;
			}
			
		}

		private string CreateUserInternal(string userUpn, string userDistinguishedName)
		{
			HostedSolutionLog.LogStart("CreateUserInternal");
			HostedSolutionLog.DebugInfo("UPN: {0}", userUpn);
			HostedSolutionLog.DebugInfo("User Distinguished Name: {0}", userDistinguishedName);
            
            try
			{
				if (string.IsNullOrEmpty(userUpn))
					throw new ArgumentException("userUpn");

				if (string.IsNullOrEmpty(userDistinguishedName))
					throw new ArgumentException("userDistinguishedName");

				if ( string.IsNullOrEmpty(PoolFQDN))
					throw new Exception("Pool FQDN is not specified");

				
                
                string poolDN = GetPoolDistinguishedName(PoolFQDN);
				if ( string.IsNullOrEmpty(poolDN))
					throw new Exception(string.Format("Pool {0} not found", PoolFQDN));

				if ( !string.IsNullOrEmpty(FindUserByDistinguishedName(userDistinguishedName)))
					throw new Exception(string.Format("User with distinguished name '{0}' already exists", userDistinguishedName));

				string primaryUri = "sip:" + userUpn;

				if (!string.IsNullOrEmpty(FindUserByPrimaryUri(primaryUri)))
					throw new Exception(string.Format("User with primary URI '{0}' already exists", primaryUri));

				using (ManagementObject newUser = Wmi.CreateInstance("MSFT_SIPESUserSetting"))
				{
					newUser["PrimaryURI"] = primaryUri;
					newUser["UserDN"] = userDistinguishedName;
					newUser["HomeServerDN"] = poolDN;
					newUser["Enabled"] = true;
					newUser["EnabledForInternetAccess"] = true;
					newUser.Put();
				}
				string instanceId = null;
				int attempts = 0;
				while (true)
				{
					instanceId = FindUserByPrimaryUri(primaryUri);
					if (!string.IsNullOrEmpty(instanceId))
						break;

					if (attempts > 9)
						throw new Exception(
							string.Format("Could not find OCS user '{0}'", primaryUri));

					attempts++;
					ExchangeLog.LogWarning("Attempt #{0} to create OCS user failed!", attempts);
					// wait 30 sec
					System.Threading.Thread.Sleep(1000);
				}

				HostedSolutionLog.LogEnd("CreateUserInternal");

				return instanceId;
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("CreateUserInternal", ex);
				throw;
			}
		}

		private string GetPoolDistinguishedName(string poolFQDN)
		{
			HostedSolutionLog.LogStart("GetPoolDistinguishedName");
			string ret = null;
			using (ManagementObject objPool = Wmi.GetWmiObject("MSFT_SIPPoolSetting", "PoolFQDN = '{0}'", poolFQDN))
			{
				if (objPool != null)
				{
					ret = (string)objPool["PoolDN"];
				}
			}
			HostedSolutionLog.LogEnd("GetPoolDistinguishedName");
			return ret;
		}

		private string FindUserByDistinguishedName(string userDistinguishedName)
		{
			string ret = null;
			HostedSolutionLog.LogStart("FindUserByDistinguishedName");
			using (ManagementObject objUser = Wmi.GetWmiObject("MSFT_SIPESUserSetting", "UserDN = '{0}'", userDistinguishedName))
			{
				if (objUser != null)
					ret = (string)objUser["InstanceID"];
			}
			HostedSolutionLog.LogEnd("FindUserByDistinguishedName");
			return ret;
		}

		private string FindUserByPrimaryUri(string uri)
		{
			string ret = null;
			HostedSolutionLog.LogStart("FindUserByPrimaryUri");
			using (ManagementObject objUser = Wmi.GetWmiObject("MSFT_SIPESUserSetting", "PrimaryURI = '{0}'", uri))
			{
				if (objUser != null)
					ret = (string)objUser["InstanceID"];
			}
			HostedSolutionLog.LogEnd("FindUserByPrimaryUri");
			return ret;
		}

		private ManagementObject GetUserByInstanceId(string instanceId)
		{
			HostedSolutionLog.LogStart("GetUserByInstanceId");
			ManagementObject objUser = Wmi.GetWmiObject("MSFT_SIPESUserSetting", "InstanceID = '{0}'", instanceId);
			HostedSolutionLog.LogEnd("GetUserByInstanceId");
			return objUser;
		}

		#endregion

		public override bool IsInstalled()
        {
			if (!FuseCP.Providers.OS.OSInfo.IsWindows) return false;

			try
			{
				Wmi.GetWmiObjects("MSFT_SIPESUserSetting", null);
				return true;
			}
			catch
			{
				return false;
			}
        }
	}
}
