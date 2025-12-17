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
using System.Collections.Generic;
using System.Text;

namespace FuseCP.EnterpriseServer
{
    public class ZipFilesTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - FOLDER
            //  - ZIP_FILE

            BackgroundTask topTask = TaskManager.TopTask;

            // get input parameters
            string filesList = (string)topTask.GetParamValue("FOLDER");
            string zipFile = (string)topTask.GetParamValue("ZIP_FILE");

            // check input parameters
            if (String.IsNullOrEmpty(filesList))
            {
                TaskManager.WriteWarning("Specify 'Files List' task parameter");
                return;
            }

            if (String.IsNullOrEmpty(zipFile))
            {
                TaskManager.WriteWarning("Specify 'Zip File' task parameter");
                return;
            }

            // substitute parameters
            DateTime d = DateTime.Now;
            string date = d.ToString("yyyyMMdd");
            string time = d.ToString("HHmm");

            filesList = Utils.ReplaceStringVariable(filesList, "date", date);
            filesList = Utils.ReplaceStringVariable(filesList, "time", time);
            zipFile = Utils.ReplaceStringVariable(zipFile, "date", date);
            zipFile = Utils.ReplaceStringVariable(zipFile, "time", time);

            // zip files and folders
            FilesController.ZipFiles(topTask.PackageId, new string[] { filesList }, zipFile);
        }
    }
}
