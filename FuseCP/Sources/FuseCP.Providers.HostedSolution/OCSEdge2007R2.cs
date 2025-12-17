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
	public class OCSEdge2007R2 : HostingServiceProviderBase, IOCSEdgeServer
	{
		#region Properties
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

		#region IOCSEdgeServer implementation

		public void AddDomain(string domainName)
		{
			AddDomainInternal(domainName);
		}

		public void DeleteDomain(string domainName)
		{
			DeleteDomainInternal(domainName);
		}

		#endregion

		#region Domains

		private ManagementObject GetDomain(string domainName)
		{
			HostedSolutionLog.LogStart("GetDomain");
			ManagementObject objDomain = Wmi.GetWmiObject("MSFT_SIPFederationInternalDomainData", "SupportedInternalDomain='{0}'", domainName);
			HostedSolutionLog.LogEnd("GetDomain");	
			return objDomain;
				
		}

		private void AddDomainInternal(string domainName)
		{
			HostedSolutionLog.LogStart("AddDomainInternal");
			HostedSolutionLog.DebugInfo("Domain Name: {0}", domainName);
			try
			{
				if (string.IsNullOrEmpty(domainName))
					throw new ArgumentException("domainName");

				if ( GetDomain(domainName) != null )
				{
					HostedSolutionLog.LogWarning("OCS internal domain '{0}' already exists", domainName);
				}
				else
				{
					using (ManagementObject newDomain = Wmi.CreateInstance("MSFT_SIPFederationInternalDomainData"))
					{
						newDomain["SupportedInternalDomain"] = domainName;
						newDomain.Put();
					}
				}
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("AddDomainInternal", ex);
				throw;
			}
			HostedSolutionLog.LogEnd("AddDomainInternal");	
		}

		private void DeleteDomainInternal(string domainName)
		{
			HostedSolutionLog.LogStart("DeleteDomainInternal");
			HostedSolutionLog.DebugInfo("Domain Name: {0}", domainName);
			try
			{
				if (string.IsNullOrEmpty(domainName))
					throw new ArgumentException("domainName");

				using (ManagementObject domainObj = GetDomain(domainName))
				{
					if (domainObj == null)
						HostedSolutionLog.LogWarning("OCS internal domain '{0}' not found", domainName);
					else
						domainObj.Delete();
				}
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("DeleteDomainInternal", ex);
				throw;
			}
			HostedSolutionLog.LogEnd("DeleteDomainInternal");
		}

		#endregion

		public override bool IsInstalled()
		{
			if (!FuseCP.Providers.OS.OSInfo.IsWindows) return false;

			try
			{
				Wmi.GetWmiObjects("MSFT_SIPFederationInternalDomainData", null);
				return true;
			}
			catch
			{
				return false;
			}
		}

        
	}
}

