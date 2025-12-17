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
using System.ComponentModel;
using System.IO;
using System.Xml;
using FuseCP.Providers.OS;
using FuseCP.UniversalInstaller;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class EmbedEnterpriseServerPage : BannerWizardPage
	{
		public WebPortalSettings Settings => Installer.Current.Settings.WebPortal;
		public EmbedEnterpriseServerPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			Text = "Embed Enterprise Server";
			Description = "Embed Enterprise Server";

			LoadUrl();
			
			chkBoxEmbed.Checked = Settings.EmbedEnterpriseServer;
			chkExpose.Checked = Settings.ExposeEnterpriseServerWebServices;
			chkExpose.Enabled = chkBoxEmbed.Checked;

			AllowMoveBack = true;
			AllowMoveNext = true;
			AllowCancel = true;
		}

		public bool CanEmbed
		{
			get
			{
				var installerPath = Settings.InstallPath;
				var webClientsPath = Path.GetFullPath(Path.Combine(installerPath, "..", Installer.Current.Settings.EnterpriseServer.InstallFolder, "bin", "Code", "FuseCP.Web.Clients.dll"));
				return File.Exists(webClientsPath);
			}
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
			try
			{
				Settings.EnterpriseServerUrl = string.Empty;

				string installFolder = Settings.InstallPath;
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
					if (Directory.Exists(Path.Combine(Settings.InstallPath, "..", Installer.Current.EnterpriseServerFolder)))
					{
						Settings.EnterpriseServerPath = Path.Combine("..", Installer.Current.EnterpriseServerFolder);
					} else if (Directory.Exists(Path.Combine(Settings.InstallPath, "..", Installer.Current.EnterpriseServerAltFolder)))
					{
						Settings.EnterpriseServerPath = Path.Combine("..", Installer.Current.EnterpriseServerAltFolder);
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Site settings error", ex);
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
				var embed = chkBoxEmbed.Checked;
				Settings.EmbedEnterpriseServer = embed;
				Settings.ExposeEnterpriseServerWebServices = chkExpose.Checked;
				if (embed) Settings.EnterpriseServerUrl = "assembly://FuseCP.EnterpriseServer";
			}
			catch
			{
				this.AllowMoveNext = false;
				ShowError("Unable to set enterprise server URL.");
				return;
			}
			base.OnBeforeMoveNext(e);
		}

		private bool CheckFields() => true;

		private string DefaultEntServerPath => Path.Combine("..", Global.EntServer.ComponentName);
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

		private void chkBoxEmbed_CheckedChanged(object sender, EventArgs e)
		{
			chkExpose.Enabled = chkBoxEmbed.Checked;
		}
	}
}
