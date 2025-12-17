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
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.EnterpriseServer
{
	public class BackupAsyncWorker: ControllerAsyncBase
	{
		public int threadUserId = -1;
		public string taskId;
		public int userId;
		public int packageId;
		public int serviceId;
		public int serverId;
		public string backupFileName;
		public int storePackageId;
		public string storePackageFolder;
		public string storeServerFolder;
		public string storePackageBackupPath;
		public string storeServerBackupPath;
		public bool deleteTempBackup;

		#region Backup
		public void BackupAsync()
		{
			// start asynchronously
			Thread t = new Thread(Backup);
            t.Start();
		}

		public void Backup()
		{
			// impersonate thread
			if (threadUserId != -1)
				SecurityContext.SetThreadPrincipal(threadUserId);

			// perform backup
			BackupController.BackupInternal(taskId, userId, packageId, serviceId, serverId, backupFileName, storePackageId,
				storePackageFolder, storeServerFolder, deleteTempBackup);
		}
		#endregion

		#region Restore
		public void RestoreAsync()
		{
			// start asynchronously
			Thread t = new Thread(new ThreadStart(Restore));
			t.Start();
		}

		public void Restore()
		{
			// impersonate thread
			if (threadUserId != -1)
				SecurityContext.SetThreadPrincipal(threadUserId);

			// perform restore
			BackupController.RestoreInternal(taskId, userId, packageId, serviceId, serverId,
				storePackageId, storePackageBackupPath, storeServerBackupPath);
		}
		#endregion
	}
}
