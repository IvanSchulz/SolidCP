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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;
using FuseCP.Portal.Code.Framework;

namespace FuseCP.Portal.UserControls.ScheduleTaskView
{
	public partial class BackupDatabase : EmptyView
	{
		private static readonly string BackupFolderParameter = "BACKUP_FOLDER";
		private static readonly string DatabaseNameParameter = "DATABASE_NAME";
		private static readonly string DatabaseGroupParameter = "DATABASE_GROUP";
		private static readonly string BackupNameParameter = "BACKUP_NAME";
		private static readonly string ZipBackupParameter = "ZIP_BACKUP";

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Sets scheduler task parameters on view.
		/// </summary>
		/// <param name="parameters">Parameters list to be set on view.</param>
		public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
		{
			base.SetParameters(parameters);

			this.SetParameter(this.ddlDatabaseType, DatabaseGroupParameter);
			this.SetParameter(this.txtDatabaseName, DatabaseNameParameter);
			this.SetParameter(this.txtBackupFolder, BackupFolderParameter);
			this.SetParameter(this.txtBackupName, BackupNameParameter);
			this.SetParameter(this.ddlZipBackup, ZipBackupParameter);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public override ScheduleTaskParameterInfo[] GetParameters()
		{
			ScheduleTaskParameterInfo databaseGroup = this.GetParameter(this.ddlDatabaseType, DatabaseGroupParameter);
			ScheduleTaskParameterInfo databaseName = this.GetParameter(this.txtDatabaseName, DatabaseNameParameter);
			ScheduleTaskParameterInfo backupFolder = this.GetParameter(this.txtBackupFolder, BackupFolderParameter);
			ScheduleTaskParameterInfo backupName = this.GetParameter(this.txtBackupName, BackupNameParameter);
			ScheduleTaskParameterInfo zipBackup = this.GetParameter(this.ddlZipBackup, ZipBackupParameter);

			return new ScheduleTaskParameterInfo[5] { databaseGroup, databaseName, backupFolder, backupName, zipBackup};
		}
	}
}
