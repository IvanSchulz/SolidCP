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
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using FuseCP.Providers.OS;
using FuseCP.UniversalInstaller.Core;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class CertificatePage : BannerWizardPage
	{
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CommonSettings Settings { get; private set; }

		public CertificatePage(CommonSettings settings)
		{
			InitializeComponent();
			Settings = settings;
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();

			Text = "Certificate Settings";

			string component = Settings.ComponentName;
			Description = $"Configure a Server Certificate for {component}.";

			AllowMoveBack = true;
			AllowMoveNext = true;
			AllowCancel = true;

			// init fields
			txtLetsEncryptEmail.Text = Settings.LetsEncryptCertificateEmail ?? "";
			txtCertFileFile.Text = Settings.CertificateFile ?? "";
			txtCertFilePassword.Text = Settings.CertificatePassword ?? "";
			txtStoreLocation.Text = Settings.CertificateStoreLocation ?? "";
			txtStoreName.Text = Settings.CertificateStoreName ?? "";
			txtStoreFindType.Text = Settings.CertificateFindType ?? "";
			txtStoreFindValue.Text = Settings.CertificateFindValue ?? "";
			manualCert.Checked = false;
			tabControl.Selected += SetAllowedMoveNext;
			manualCert.CheckedChanged += SetAllowedMoveNext;
			if (!Installer.UseLettuceEncrypt || OSInfo.IsWindows && !Settings.RunOnNetCore) // TODO support Let's Encrypt also on Windows
			{
				// remove Let's Encrypt tab pages
				tabControl.TabPages.RemoveAt(2);
				//tabControl.TabPages.RemoveAt(1);
			}

			if (!string.IsNullOrEmpty(Settings.CertificateFindValue)) tabControl.SelectedTab = tabPageCertStore;
			else if (!string.IsNullOrEmpty(Settings.CertificateFile)) tabControl.SelectedTab = tabPageCertFile;
			else if (!string.IsNullOrEmpty(Settings.LetsEncryptCertificateEmail)) tabControl.SelectedTab = tabPageLetsEncrypt;
			else tabControl.SelectedTab = tabPageManual;

			string[] names, locations;
			CertificateStoreInfo.GetStoreNames(out names, out locations);

			txtStoreLocation.Items.Clear();
			txtStoreLocation.Items.AddRange(locations.OfType<object>().ToArray());

			txtStoreName.Items.Clear();
			txtStoreName.Items.AddRange(names.OfType<object>().ToArray());

			txtStoreFindType.Items.Clear();
			txtStoreFindType.Items.Add(X509FindType.FindBySubjectName.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindByThumbprint.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindBySubjectDistinguishedName.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindBySubjectKeyIdentifier.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindBySerialNumber.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindByIssuerName.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindByIssuerDistinguishedName.ToString());

			Update();
		}

		private void SetAllowedMoveNext(object sender, EventArgs args)
		{
			AllowMoveNext = Hidden || (tabControl.SelectedTab != tabPageManual) || manualCert.Checked;
		}
		bool IsIis7 => OSInfo.IsWindows && OSInfo.Windows?.WebServer.Version.Major >= 7;
		bool IsSecure(Uri uri) => uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) || (IsIis7 || !OSInfo.IsWindows) && Utils.IsHostLocal(uri.Host);
		bool IsSecure(string urls) => (urls ?? "").Split(',', ';')
			.Any(url => !string.IsNullOrEmpty(url) && IsSecure(new Uri(url)));

		public override bool Hidden => false; // always needs certificate, at least for WSHttpBinding. !IsSecure(Settings.Urls);
		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if ((Installer.Current.Settings.Installer.IsUnattended || Hidden) && AllowMoveNext)
				Wizard.GoNext();
		}

		private bool CheckEmail()
		{
			string email = txtLetsEncryptEmail.Text?.Trim() ?? "";

			if (!email.Contains("@"))
			{
				ShowWarning(String.Format("'{0}' is not a valid email address.", email));
				return false;
			}
			return true;
		}

		private bool CheckCertStore()
		{
			X509FindType findType;
			StoreLocation location;
			StoreName name;
			if (!Enum.TryParse<StoreLocation>(txtStoreLocation.Text, out location)) {
				ShowWarning("The entered Store Location is invalid.");
				return false;
			}
			if (!Enum.TryParse<StoreName>(txtStoreName.Text, out name)) {
				ShowWarning("The entered Store Name is invalid.");
				return false;
			}
			if (!Enum.TryParse<X509FindType>(txtStoreFindType.Text, out findType))
			{
				ShowWarning("The entered Find Type is invalid.");
				return false;
			}
			if (string.IsNullOrEmpty(txtStoreFindValue.Text))
			{
				ShowWarning("You must specify a Find Value.");
				return false;
			}

			if (!CertificateStoreInfo.Exists(location, name, findType, txtStoreFindValue.Text))
			{
				ShowWarning($"No valid certificates found for {txtStoreFindValue.Text}.");
				return false;
			}
			
			return true;

		}

		private bool CheckCertFile()
		{
			var file = txtCertFileFile.Text;
			if (!File.Exists(file))
			{
				ShowWarning("The entered Certificate File could not be found.");
				return false;
			}
			try
			{
				var cert2 = new X509Certificate2(file, txtCertFilePassword.Text);
			} catch
			{
				ShowWarning("The entered password is invalid.");
				return false;
			}
			return true;
		}
		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			Settings.LetsEncryptCertificateEmail = null;
			Settings.CertificateFile = null;
			Settings.CertificatePassword = null;
			Settings.CertificateStoreLocation = null;
			Settings.CertificateStoreName = null;
			Settings.CertificateFindType = null;
			Settings.CertificateFindValue = null;

			if (!Hidden)
			{

				if (tabControl.SelectedTab == tabPageCertStore)
				{
					if (!CheckCertStore())
					{
						e.Cancel = true;
						return;
					}
					Settings.CertificateStoreLocation = txtStoreLocation.Text;
					Settings.CertificateStoreName = txtStoreName.Text;
					Settings.CertificateFindType = txtStoreFindType.Text;
					Settings.CertificateFindValue = txtStoreFindValue.Text;
				} else if (tabControl.SelectedTab == tabPageCertFile)
				{
					if (!CheckCertFile())
					{
						e.Cancel = true;
						return;
					}
					Settings.CertificateFile = txtCertFileFile.Text;
					Settings.CertificatePassword = txtCertFilePassword.Text;
				} else if (tabControl.SelectedTab == tabPageLetsEncrypt)
				{
					if (!CheckEmail())
					{
						e.Cancel = true;
						return;
					}
					Settings.LetsEncryptCertificateEmail = txtLetsEncryptEmail.Text;
					System.Net.IPAddress ip;
					var urls = string.IsNullOrEmpty(Settings.Urls) ? Settings.WebSiteDomain : Settings.Urls;
					Settings.LetsEncryptCertificateDomains = string.Join(",", urls.Split(',', ';')
						.Where(url => url.StartsWith("https", StringComparison.OrdinalIgnoreCase) ||
							url.StartsWith("net.tcp", StringComparison.OrdinalIgnoreCase))
						.Select(url => new Uri(url).Host)
						.Where(host => !System.Net.IPAddress.TryParse(host, out ip))
						.ToArray());
				} else if (tabControl.SelectedTab == tabPageManual)
				{
					Settings.ConfigureCertificateManually = manualCert.Checked;

					if (!manualCert.Checked)
					{
						e.Cancel = true;
						return;
					}
				}
			}

			base.OnBeforeMoveNext(e);
		}

		private void btnOpenCertFile_Click(object sender, EventArgs e)
		{
			if (openCertFileDialog.ShowDialog() == DialogResult.OK)
			{
				txtCertFileFile.Text = openCertFileDialog.FileName;
			}
		}
	}
}
