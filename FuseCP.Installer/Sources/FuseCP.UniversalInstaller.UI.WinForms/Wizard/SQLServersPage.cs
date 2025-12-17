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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FuseCP.UniversalInstaller;
using FuseCP.UniversalInstaller.Web;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class SQLServersPage : BannerWizardPage
	{
		public SQLServersPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "SQL Servers";
			this.Description = "Specify SQL servers to manage with myLittleAdmin.";
			
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;

			PopulateServers();
		}

		private void PopulateServers()
		{
			try
			{
				Log.WriteStart("Populating SQL servers");
				ServerItem[] servers = null;

				if (Wizard.SetupVariables.SetupAction == SetupActions.Setup)
				{
					servers = LoadServersFromConfigFile();
				}
				else
				{
					if (Wizard.SetupVariables.SQLServers != null)
						servers = Wizard.SetupVariables.SQLServers;
				}
				if ( servers == null )
					servers = new ServerItem[] { };

				DataSet ds = new DataSet();
				DataTable dt = new DataTable("Servers");
				ds.Tables.Add(dt);
				DataColumn colServer = new DataColumn("Server", typeof(string));
				DataColumn colName = new DataColumn("Name", typeof(string));
				dt.Columns.AddRange(new DataColumn[]{colServer, colName});

				foreach (ServerItem item in servers)
				{
					dt.Rows.Add(item.Server, item.Name);
				}
				grdServers.DataSource = ds;
				grdServers.DataMember = "Servers";
				Log.WriteEnd("Populated SQL servers");
			}
			catch (Exception ex)
			{
				Log.WriteError("Configuration error", ex);
			}
		}

		private ServerItem[] LoadServersFromConfigFile()
		{
			string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, "config.xml");

			if (!File.Exists(path))
			{
				Log.WriteInfo(string.Format("File {0} not found", path));
				return null;
			}

			List<ServerItem> list = new List<ServerItem>();
			XmlDocument doc = new XmlDocument();
			doc.Load(path);

			XmlNodeList servers = doc.SelectNodes("//myLittleAdmin/sqlservers/sqlserver");
			foreach (XmlElement serverNode in servers)
			{
				list.Add(
					new ServerItem(
						serverNode.GetAttribute("address"),
						serverNode.GetAttribute("name")));
			}
			return list.ToArray();
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			List<ServerItem> list = new List<ServerItem>();
			DataSet ds = grdServers.DataSource as DataSet;
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				list.Add(new ServerItem(row["Server"].ToString(), row["Name"].ToString()));
			}
			Wizard.SetupVariables.SQLServers = list.ToArray();
			base.OnBeforeMoveNext(e);
		}
	}
}
