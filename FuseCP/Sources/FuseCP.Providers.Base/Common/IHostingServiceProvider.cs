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
using FuseCP.Providers.Common;

namespace FuseCP.Providers
{
	/// <summary>
	/// Summary description for IPanelServiceProvider.
	/// </summary>
	public interface IHostingServiceProvider
	{
		/// <summary>
		/// Executes each time when the service is being added to some server.
		/// Prepare environment for service usage (like setting folder permissions,
		/// creating system users, etc.)
		/// </summary>
		string[] Install();

        /// <summary>
        /// Returns the list of additional provider properties.
        /// </summary>
        /// <returns>The array of additional properties.</returns>
        SettingPair[] GetProviderDefaultSettings();

		/// <summary>
		/// Executes when service is being removed from server.
		/// Performs any clean up operations.
		/// </summary>
		void Uninstall();

		/// <summary>
		/// Checks whether service is installed within the system. This method will be
		/// used by server creation wizard for automatic services detection and configuring.
		/// </summary>
		/// <returns>True if service is installed; otherwise - false.</returns>
		bool IsInstalled();

		void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled);
        void DeleteServiceItems(ServiceProviderItem[] items);

        ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items);
        ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(ServiceProviderItem[] items, DateTime since);
	}
}
