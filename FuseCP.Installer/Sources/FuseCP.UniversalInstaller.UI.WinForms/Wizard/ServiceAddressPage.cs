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
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using FuseCP.UniversalInstaller;
using FuseCP.UniversalInstaller.Web;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class ServiceAddressPage : BannerWizardPage
	{
		public ServiceAddressPage()
		{
			InitializeComponent();
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CommonSettings Settings { get; set; }
		WebUtils WebUtils => Installer.Current.WebUtils;
		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Service Settings";
			
			string component = Settings.ComponentName;
			this.Description = string.Format("Specify {0} address settings.", component);
			
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;

			// init fields
			var url = Settings.Urls.Split(',', ';').FirstOrDefault();
			var uri = new Uri(url);
			PopulateIPs();
			this.txtTcpPort.Text = uri.Port.ToString();
			UpdateApplicationAddress();
		}

		private void PopulateIPs()
		{
			try
			{
				Log.WriteStart("Loading IPs");
				cbIP.Items.Clear();
				string[] ips = WebUtils.GetIPs();
				foreach (string ip in ips)
				{
					cbIP.Items.Add(ip);
				}
				Log.WriteEnd("Loaded IPs");

				/*
				if (string.IsNullOrEmpty(Wizard.SetupVariables.ServiceIP))
				{
					//select first available
					if (cbIP.Items.Count > 0)
					{
						cbIP.Text = cbIP.Items[0].ToString();
					}
				}
				else
				{
					//add 127.0.0.1 
					if (!cbIP.Items.Contains(Wizard.SetupVariables.ServiceIP))
					{
						cbIP.Items.Insert(0, Wizard.SetupVariables.ServiceIP);
					}
					cbIP.Text = Wizard.SetupVariables.ServiceIP;
				}*/

			}
			catch (Exception ex)
			{
				Log.WriteError("WMI error", ex);
			}
		}

		private void OnAddressChanged(object sender, System.EventArgs e)
		{
			UpdateApplicationAddress();
		}

		private void UpdateApplicationAddress()
		{
			string address = "soap.tcp://";
			string ip = string.Empty;
			string port = string.Empty;

				//ip
				if (cbIP.Text.Trim().Length > 0)
				{
					ip = cbIP.Text.Trim();
				}
			
			//port
			if (ip.Length > 0 && txtTcpPort.Text.Trim().Length > 0 )
			{
				port = ":" + txtTcpPort.Text.Trim();
			}
			
			//address string
			address += ip + port;
			txtAddress.Text = address;
		}

		private bool CheckAddress()
		{
			string ip = cbIP.Text;
			string port = txtTcpPort.Text;

			if (ip.Trim().Length == 0)
			{
				ShowWarning("Please enter IP address");
				return false;
			}

			if (!Regex.IsMatch(ip, @"^(?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(?=\.?\d)\.)){4}$"))
			{
				ShowWarning("Please enter valid IP address (for example, 192.168.1.42)");
				return false;
			}

			if (port.Trim().Length == 0)
			{
				ShowWarning("Please enter TCP port");
				return false;
			}

			for (int i = 0; i < port.Length; i++)
			{
				if (!Char.IsNumber(port, i))
				{
					ShowWarning("Please enter valid TCP port (for example, 80).");
					return false;
				}
			}
			return true;
		}


		private bool IsEqualString(string s1, string s2)
		{
			bool ret = false;
			if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
			{
				ret = true;
			}
			else
			{
				ret = (s1 == s2);
			}
			return ret;
		}

		private bool ProcessAddressSettings()
		{
			if (!CheckAddress())
			{
				return false;
			}
			return true;
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			if (!ProcessAddressSettings())
			{
				e.Cancel = true;
				return;
			}
			/*Wizard.SetupVariables.ServiceIP = this.cbIP.Text;
			Wizard.SetupVariables.ServicePort = this.txtTcpPort.Text;*/

			base.OnBeforeMoveNext(e);
		}
	}
}
