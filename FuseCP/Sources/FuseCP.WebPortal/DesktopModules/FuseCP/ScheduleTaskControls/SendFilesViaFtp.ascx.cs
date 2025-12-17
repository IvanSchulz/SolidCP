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
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;
using FuseCP.Portal.UserControls.ScheduleTaskView;

namespace FuseCP.Portal.ScheduleTaskControls
{
	public partial class SendFilesViaFtp : EmptyView
	{
		private static readonly string FilePathParameter = "FILE_PATH";
		private static readonly string ServerParameter = "FTP_SERVER";
		private static readonly string UserNameParameter = "FTP_USERNAME";
		private static readonly string PasswordParameter = "FTP_PASSWORD";
		private static readonly string RemoteFolderParameter = "FTP_FOLDER";

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

			this.SetParameter(this.txtFilePath, FilePathParameter);
			this.SetParameter(this.txtServer, ServerParameter);
			this.SetParameter(this.txtUserName, UserNameParameter);
			this.SetParameter(this.txtPassword, PasswordParameter);
			this.SetParameter(this.txtRemoteFolder, RemoteFolderParameter);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public override ScheduleTaskParameterInfo[] GetParameters()
		{
			ScheduleTaskParameterInfo filePath = this.GetParameter(this.txtFilePath, FilePathParameter);
			ScheduleTaskParameterInfo server = this.GetParameter(this.txtServer, ServerParameter);
			ScheduleTaskParameterInfo userName = this.GetParameter(this.txtUserName, UserNameParameter);
			ScheduleTaskParameterInfo password = this.GetParameter(this.txtPassword, PasswordParameter);
			ScheduleTaskParameterInfo remoteFolder = this.GetParameter(this.txtRemoteFolder, RemoteFolderParameter);

			return new ScheduleTaskParameterInfo[5] { filePath, server, userName, password, remoteFolder };
		}
	}
}
