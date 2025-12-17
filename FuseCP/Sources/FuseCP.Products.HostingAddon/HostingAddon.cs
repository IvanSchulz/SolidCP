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
using System.Data;

using FuseCP.EnterpriseServer;

namespace FuseCP.Ecommerce.EnterpriseServer
{
	public class HostingAddon : ProvisioningControllerBase, IProvisioningController
	{
		public const string ROOT_SERVICE_ID = "RootServiceID";
		public const string PACKAGE_ID = "PackageID";
		public const string HOSTING_ADDON = "HostingAddon";
		public const string PACKAGE_ADDON_ID = "PackageAddonID";

		public string PackageAddonID
		{
			get { return ServiceSettings[PACKAGE_ADDON_ID]; }
			set { ServiceSettings[PACKAGE_ADDON_ID] = value; }
		}

		public HostingAddon(Service serviceInfo)
			: base(serviceInfo)
		{
		}

		#region IProvisioningController Members

		public void Activate()
		{
			int packageAddonId = 0;

			if (!Int32.TryParse(PackageAddonID, out packageAddonId))
				throw new Exception("Unable to parse Package Add-On ID: " + PackageAddonID);
			// try to get package addon
			PackageAddonInfo addon = PackageController.GetPackageAddon(packageAddonId);
			// check returned package
			if (addon != null)
			{
				// workaround for bug in GetPackageAddon routine
				addon.PackageAddonId = packageAddonId;
				// change package add-on status
				addon.StatusId = (int)PackageStatus.Active;
				// save package add-on changes
				PackageResult result = PackageController.UpdatePackageAddon(addon);
				// check returned result
				if (result.Result < 0)
					throw new Exception("Unable to activate Package Add-On: " + addon.PackageAddonId);
			}
		}

		public void Suspend()
		{
			int packageAddonId = 0;

			if (!Int32.TryParse(PackageAddonID, out packageAddonId))
				throw new Exception("Unable to parse Package Add-On ID: " + PackageAddonID);
			// try to get package addon
			PackageAddonInfo addon = PackageController.GetPackageAddon(packageAddonId);
			// check returned package
			if (addon != null)
			{
				// workaround for bug in GetPackageAddon routine
				addon.PackageAddonId = packageAddonId;
				// change package add-on status
				addon.StatusId = (int)PackageStatus.Suspended;
				// save package add-on changes
				PackageResult result = PackageController.UpdatePackageAddon(addon);
				// check returned result
				if (result.Result < 0)
					throw new Exception("Unable to suspend Package Add-On: " + addon.PackageAddonId);
			}
		}

		public void Cancel()
		{
			int packageAddonId = 0;

			if (!Int32.TryParse(PackageAddonID, out packageAddonId))
				throw new Exception("Unable to parse Package Add-On ID: " + PackageAddonID);
			// try to get package addon
			PackageAddonInfo addon = PackageController.GetPackageAddon(packageAddonId);
			// check returned package
			if (addon != null)
			{
				// workaround for bug in GetPackageAddon routine
				addon.PackageAddonId = packageAddonId;
				// change package add-on status
				addon.StatusId = (int)PackageStatus.Cancelled;
				// save package add-on changes
				PackageResult result = PackageController.UpdatePackageAddon(addon);
				// check returned result
				if (result.Result < 0)
					throw new Exception("Unable to cancel Package Add-On: " + addon.PackageAddonId);
			}
		}

		public void Order()
		{
			int rootServiceId = Utils.ParseInt(ServiceSettings[ROOT_SERVICE_ID], 0);

			// each add-on should have root service id assigned
			if (rootServiceId < 0)
			{
				throw new Exception(
					"Incorrect add-on settings. Root Service ID couldn't be found please review logs and correct this issue."
				);
			}

			// get root service settings
			KeyValueBunch rootSettings = ServiceController.GetServiceSettings(
				ServiceInfo.SpaceId,
				rootServiceId
			);

			// failed to load root service settings
			if (rootSettings == null)
				throw new Exception("Unable to load root service settings.");

			// add package add-on
			PackageAddonInfo addon = new PackageAddonInfo();

			// load Package ID
			int packageId = 0;
			if (!Int32.TryParse(rootSettings[PACKAGE_ID], out packageId))
				throw new Exception("Couldn't parse parent service settings: PackageID property. Parent Service ID: " + rootServiceId);

			// load Plan ID
			int hostingAddon = 0;
			if (!Int32.TryParse(ServiceSettings[HOSTING_ADDON], out hostingAddon))
				throw new Exception("Couldn't parse service settings: HostingAddon property. Service ID: " + ServiceInfo.ServiceId);

			addon.PackageId = packageId;
			addon.PlanId = hostingAddon;
			addon.Quantity = 1;
			addon.StatusId = (int)PackageStatus.Active;
			addon.PurchaseDate = DateTime.UtcNow;

			PackageResult result = PackageController.AddPackageAddon(addon);

			// failed to create package add-on
			if (result.Result < 0)
				throw new Exception("Unable to add package add-on. Status code: " + result.Result);
			
			// save service settings
			PackageAddonID = result.Result.ToString();
		}

		public void Rollback() {}

		#endregion
	}
}
