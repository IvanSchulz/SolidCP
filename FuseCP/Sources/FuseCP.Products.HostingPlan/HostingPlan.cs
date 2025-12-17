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
	public class HostingPlan : ProvisioningControllerBase, IProvisioningController
	{
		public const string HOSTING_PLAN = "HostingPlan";
		public const string INITIAL_STATUS = "InitialStatus";
		public const string PACKAGE_ID = "PackageID";
		public const string USER_ROLE = "UserRole";
		public const string USER_STATUS = "UserStatus";

		private PackageStatus previousStatus;

		public HostingPlan(Service serviceInfo)
			: base(serviceInfo)
		{
		}

		#region IProvisioningController Members

		public void Activate()
		{
			int packageId = Utils.ParseInt(ServiceSettings[PACKAGE_ID], -1);

			if (packageId > -1)
			{
				RememberCurrentStatus(packageId);

				int status = PackageController.ChangePackageStatus(packageId, PackageStatus.Active, false);

				if (status < 0)
					throw new Exception("Unable to change package status. Status code: " + status);
			}
		}

		public void Suspend()
		{
			int packageId = Utils.ParseInt(ServiceSettings[PACKAGE_ID], -1);

			if (packageId > -1)
			{
				RememberCurrentStatus(packageId);

				int status = PackageController.ChangePackageStatus(packageId, PackageStatus.Suspended, false);

				if (status < 0)
					throw new Exception("Unable to change package status. Status code: " + status);
			}
		}

		public void Cancel()
		{
			int packageId = Utils.ParseInt(ServiceSettings[PACKAGE_ID], -1);

			if (packageId > -1)
			{
				RememberCurrentStatus(packageId);

				int status = PackageController.ChangePackageStatus(packageId, PackageStatus.Cancelled, false);

				if (status < 0)
					throw new Exception("Unable to change package status. Status code: " + status);
			}
		}

		public void Order()
		{
			RememberCurrentStatus(PackageStatus.New);

			PackageResult result = PackageController.AddPackage(
				UserInfo.UserId,
				Convert.ToInt32(ServiceSettings[HOSTING_PLAN]),
				String.Format("My Space ({0})", UserInfo.Username),
				String.Empty,
				Convert.ToInt32(ServiceSettings[INITIAL_STATUS]),
				DateTime.Now,
				true
			);

			// throws an exception
			if (result.Result <= 0)
				throw new Exception("Couldn't add package to the user: " + UserInfo.Username);

			ServiceSettings[PACKAGE_ID] = result.Result.ToString();

			// update user details
			if (!String.IsNullOrEmpty(ServiceSettings[USER_ROLE]))
				UserInfo.Role = (UserRole)Enum.Parse(typeof(UserRole), ServiceSettings[USER_ROLE]);

			if (!String.IsNullOrEmpty(ServiceSettings[USER_STATUS]))
				UserInfo.Status = (UserStatus)Enum.Parse(typeof(UserStatus), ServiceSettings[USER_STATUS]);

			UserController.UpdateUser(UserInfo);
		}

		public void Rollback()
		{
			int packageId = Utils.ParseInt(ServiceSettings[PACKAGE_ID], -1);

			if (packageId > -1)
			{
				switch (previousStatus)
				{
					case PackageStatus.New: // delete package
						int removeResult = PackageController.DeletePackage(packageId);
						if (removeResult != 0)
							throw new Exception("Unable to rollback(remove) package. Status code: " + removeResult);
						break;
					case PackageStatus.Active: // return to active state
					case PackageStatus.Cancelled:
					case PackageStatus.Suspended:

						PackageInfo package = PackageController.GetPackage(packageId);
						package.StatusId = (int)previousStatus;

						PackageResult result = PackageController.UpdatePackage(package);

						if (result.Result != 0)
							throw new Exception("Unable to rollback(update) package. Status code: " + result.Result);

						break;
				}
			}
		}

		#endregion

		#region Helper routines

		private void RememberCurrentStatus(PackageStatus current)
		{
			previousStatus = current;
		}

		private void RememberCurrentStatus(int packageId)
		{
			PackageInfo package = PackageController.GetPackage(packageId);

			previousStatus = (PackageStatus)package.StatusId;
		}

		#endregion
	}
}
