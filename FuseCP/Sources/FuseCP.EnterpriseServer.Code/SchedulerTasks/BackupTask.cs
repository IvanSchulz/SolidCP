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
using System.Threading;


namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Represents scheduler task that performs hosting space backup.
	/// </summary>
	public class BackupTask : SchedulerTask
	{
		/// <summary>
		/// Performs actual backup.
		/// </summary>
		public override void DoWork()
		{

			string backupFileName;
			int storePackageId;
			string storePackageFolder;
			string storeServerFolder;
			bool deleteTempBackup;

            BackgroundTask topTask = TaskManager.TopTask;

			try
			{
				backupFileName = (string)topTask.GetParamValue("BACKUP_FILE_NAME");
				storePackageId = Convert.ToInt32(topTask.GetParamValue("STORE_PACKAGE_ID"));
				storePackageFolder = (string)topTask.GetParamValue("STORE_PACKAGE_FOLDER");
				storeServerFolder = (string)topTask.GetParamValue("STORE_SERVER_FOLDER");
				deleteTempBackup = Convert.ToBoolean(topTask.GetParamValue("DELETE_TEMP_BACKUP"));
			}
			catch(Exception ex)
			{
				TaskManager.WriteError(ex, "Some parameters are absent or have incorrect value.");
				return;
			}

			try
			{
				PackageInfo package = PackageController.GetPackage(topTask.PackageId);
				// We do not take into account service id as long as scheduled tasks run against packages.
				BackupController.Backup(false, "BackupTask", package.UserId, package.PackageId, 0, 0,
                    backupFileName, storePackageId, storePackageFolder, storeServerFolder, deleteTempBackup);
			}
			catch(Exception ex)
			{
				TaskManager.WriteError(ex, "Failed to do backup.");
			}
		}
	}
}
