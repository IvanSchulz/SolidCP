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
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using FuseCP.UniversalInstaller;
using FuseCP.Providers.OS;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class EnterpriseServerUrlPage : BannerWizardPage
	{
		public EnterpriseServerUrlPage()
		{
			InitializeComponent();
		}
		public WebPortalSettings Settings => Installer.Current.Settings.WebPortal;
		public bool CanEmbed
		{
			get
			{
				var installerPath = Settings.InstallPath;
				var webClientsPath = Path.GetFullPath(Path.Combine(installerPath, "..", Installer.Current.EnterpriseServerFolder, "bin", "Code", "FuseCP.Web.Clients.dll"));
				var webClientsAltPath = Path.GetFullPath(Path.Combine(installerPath, "..", Installer.Current.EnterpriseServerAltFolder, "bin", "Code", "FuseCP.Web.Clients.dll"));
				return File.Exists(webClientsPath) || File.Exists(webClientsAltPath);
			}
		}
		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			Text = "Enterprise Server URL";
			Description = "Enter the Enterprise Server URL";

			if (Installer.Current.Settings.Installer.Action == SetupActions.Setup)
			{
				LoadUrl();
			}
			txtURL.Text = Settings.EnterpriseServerUrl;
			txtPath.Text = Settings.EnterpriseServerPath;
			chkBoxEmbed.Checked = Settings.EmbedEnterpriseServer;
			chkExpose.Checked = Settings.ExposeEnterpriseServerWebServices;

			chkBoxEmbed_CheckedChanged(this, EventArgs.Empty);

			AllowMoveBack = true;
			AllowMoveNext = true;
			AllowCancel = true;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended && AllowMoveNext)
				Wizard.GoNext();
		}

		private void LoadUrl()
		{
			/*try
			{
				Settings.EnterpriseServerUrl = string.Empty;

				string installFolder = Settings.InstallFolder;
				string path = Path.Combine(installFolder, @"App_Data\SiteSettings.config");

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlElement urlNode = doc.SelectSingleNode("SiteSettings/EnterpriseServer") as XmlElement;
				if (urlNode == null)
				{
					Log.WriteInfo("EnterpriseServer setting not found");
					return;
				}

				Settings.EnterpriseServerUrl = urlNode.InnerText;
				Settings.EmbedEnterpriseServer = urlNode.InnerText.StartsWith("assembly://");
				if (Settings.EmbedEnterpriseServer && string.IsNullOrEmpty(Settings.EnterpriseServerPath))
				{
					Settings.EnterpriseServerPath = $"..\\{Global.EntServer.ComponentName}";
				}
			}
			catch(Exception ex)
			{
				Log.WriteError("Site settings error", ex);
			}*/
			
			var path = DefaultEntServerPath;
			Settings.EmbedEnterpriseServer = path != null;
			Settings.EnterpriseServerPath = path;
			if (Settings.EmbedEnterpriseServer)
			{
				Settings.EnterpriseServerUrl = "assembly://FuseCP.EnterpriseServer";
			}
			else
			{
				if (string.IsNullOrEmpty(Settings.EnterpriseServerUrl))
				{

					Settings.EnterpriseServerUrl = "http://localhost:9002";
				}
			}
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				if (!CheckFields())
				{
					e.Cancel = true;
					return;
				}
				Settings.EnterpriseServerUrl = txtURL.Text;
				Settings.EnterpriseServerPath = txtPath.Text;
				Settings.EmbedEnterpriseServer = chkBoxEmbed.Checked;
				Settings.ExposeEnterpriseServerWebServices = chkExpose.Checked;
			}
			catch
			{
				this.AllowMoveNext = false;
				ShowError("Unable to set enterprise server URL.");
				return;
			}
			base.OnBeforeMoveNext(e);
		}

		private bool CheckFields()
		{
			string url = txtURL.Text;

			if (url.Trim().Length == 0)
			{
				ShowWarning("Please enter valid URL");
				return false;
			}

			Regex r = new Regex(@"(http|https|assembly)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
			Match m = r.Match(url);
			if (!m.Success)
			{
				ShowWarning("Please enter valid URL");
				return false;
			}

			if (chkBoxEmbed.Checked)
			{
				if (string.IsNullOrEmpty(txtPath.Text))
				{
					ShowWarning("Please enter valid Path");
					return false;
				}
				else
				{
					var entservpath = AbsolutePath(txtPath.Text);

					if (!Directory.Exists(entservpath))
					{
						ShowWarning("The specified path does not exist");
						return false;
					}
					else if (!File.Exists(Path.Combine(entservpath, "web.config")) ||
						!File.Exists(Path.Combine(entservpath, "bin", "FuseCP.EnterpriseServer.dll")))
					{
						ShowWarning("There is no Enterprise Server installation in the specified path");
						return false;
					}
				}
            }

			return true;
		}
		private string DefaultEntServerPath
		{
			get
			{
				var folder1 = Path.Combine("..", Installer.Current.EnterpriseServerFolder);
				var folder2 = Path.Combine("..", Installer.Current.PathWithSpaces(Installer.Current.EnterpriseServerFolder));
				if (Directory.Exists(folder1))
				{
					return folder1;
				}
				else if (Directory.Exists(folder2))
				{
					return folder2;
				}
				else
				{
					return null;
				}
			}
		}
		private string AbsolutePath(string relativePath) => Path.IsPathRooted(relativePath) ? relativePath :
			Path.GetFullPath(Path.Combine(Settings.InstallPath, relativePath));
		private string RelativePath(string absolutePath) => GetRelativePath(Settings.InstallPath, absolutePath);

		/// <summary>
		/// Creates a relative path from one file or folder to another.
		/// </summary>
		/// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
		/// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
		/// <returns>The relative path from the start directory to the end path.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="fromPath"/> or <paramref name="toPath"/> is <c>null</c>.</exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static string GetRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath))
            {
                throw new ArgumentNullException("fromPath");
            }

            if (string.IsNullOrEmpty(toPath))
            {
                throw new ArgumentNullException("toPath");
            }

            Uri fromUri = new Uri(AppendDirectorySeparatorChar(fromPath));
            Uri toUri = new Uri(AppendDirectorySeparatorChar(toPath));

            if (fromUri.Scheme != toUri.Scheme)
            {
                return toPath;
            }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (string.Equals(toUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private static string AppendDirectorySeparatorChar(string path)
        {
            // Append a slash only if the path is a directory and does not have a slash.
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

		string oldUrl = null;
		private void chkBoxEmbed_CheckedChanged(object sender, EventArgs e)
		{
			lblEnterpriseServerPath.Enabled = chkExpose.Enabled = txtPath.Enabled = chkBoxEmbed.Checked;
			lblURL.Enabled = txtURL.Enabled = !chkBoxEmbed.Checked;
			
			if (chkBoxEmbed.Checked) {
				oldUrl = txtURL.Text;
				if (oldUrl.StartsWith("assembly://")) oldUrl = null;
				txtURL.Text = "assembly://FuseCP.EnterpriseServer";
				if (string.IsNullOrEmpty(txtPath.Text) && DefaultEntServerPath != null)
				{
					txtPath.Text = DefaultEntServerPath;
				}
			}
			else
			{
				txtURL.Text = oldUrl ?? "http://localhost:9002";
			}
		}

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {
			folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			folderBrowserDialog.SelectedPath = AbsolutePath(string.IsNullOrEmpty(txtPath.Text) ? DefaultEntServerPath : txtPath.Text);
			folderBrowserDialog.ShowDialog();

			txtPath.Text = RelativePath(folderBrowserDialog.SelectedPath);
        }
    }
}
