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
using System.IO;
using System.Collections.Generic;
using System.Text;

using FuseCP.Providers.Database;

namespace FuseCP.EnterpriseServer
{
    public class BackupDatabaseTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - DATABASE_GROUP
            //  - DATABASE_NAME
            //  - BACKUP_FOLDER
            //  - BACKUP_NAME
            //  - ZIP_BACKUP

            BackgroundTask topTask = TaskManager.TopTask;

            string databaseGroup = (string)topTask.GetParamValue("DATABASE_GROUP");
            string databaseName = (string)topTask.GetParamValue("DATABASE_NAME");
            string backupFolder = (string)topTask.GetParamValue("BACKUP_FOLDER");
            string backupName = (string)topTask.GetParamValue("BACKUP_NAME");
            string strZipBackup = (string)topTask.GetParamValue("ZIP_BACKUP");

            // check input parameters
            if (String.IsNullOrEmpty(databaseName))
            {
                TaskManager.WriteWarning("Specify 'Database Name' task parameter.");
                return;
            }

            bool zipBackup = (strZipBackup.ToLower() == "true");

            if (String.IsNullOrEmpty(backupName))
            {
                backupName = databaseName + (zipBackup ? ".zip" : ".bak");
            }
            else
            {
                // check extension
                string ext = Path.GetExtension(backupName);
                if (zipBackup && String.Compare(ext, ".zip", true) != 0)
                {
                    // change extension to .zip
                    backupName = Path.GetFileNameWithoutExtension(backupName) + ".zip";
                }
            }

            // try to find database
            SqlDatabase item = (SqlDatabase)PackageController.GetPackageItemByName(topTask.PackageId, databaseGroup,
                databaseName, typeof(SqlDatabase));

            if (item == null)
            {
                TaskManager.WriteError("Database with the specified name was not found in the current hosting space.");
                return;
            }

            if (String.IsNullOrEmpty(backupFolder))
                backupFolder = "\\";

            // substitute parameters
            DateTime d = DateTime.Now;
            string date = d.ToString("yyyyMMdd");
            string time = d.ToString("HHmm");

            backupFolder = Utils.ReplaceStringVariable(backupFolder, "date", date);
            backupFolder = Utils.ReplaceStringVariable(backupFolder, "time", time);
            backupName = Utils.ReplaceStringVariable(backupName, "date", date);
            backupName = Utils.ReplaceStringVariable(backupName, "time", time);

            // backup database
            DatabaseServerController.BackupSqlDatabase(item.Id, backupName, zipBackup, false, backupFolder);
        }
    }
}
