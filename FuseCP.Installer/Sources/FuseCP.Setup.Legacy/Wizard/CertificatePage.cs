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

using FuseCP.Setup.Web;
using FuseCP.Providers.OS;
using FuseCP.UniversalInstaller.Core;

namespace FuseCP.Setup
{
	public partial class CertificatePage : BannerWizardPage
	{
		public CertificatePage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();

			Text = "Certificate Settings";

			string component = SetupVariables.ComponentFullName;
			Description = $"Configure a Server Certificate for {component}.";

			AllowMoveBack = true;
			AllowMoveNext = true;
			AllowCancel = true;

			// init fields
			txtLetsEncryptEmail.Text = SetupVariables.LetsEncryptEmail ?? "";
			txtCertFileFile.Text = SetupVariables.CertificateFile ?? "";
			txtCertFilePassword.Text = SetupVariables.CertificatePassword ?? "";
			txtStoreLocation.Text = SetupVariables.CertificateStoreLocation ?? "";
			txtStoreName.Text = SetupVariables.CertificateStore ?? "";
			txtStoreFindType.Text = SetupVariables.CertificateFindType ?? "";
			txtStoreFindValue.Text = SetupVariables.CertificateFindValue ?? "";
			manualCert.Checked = false;
			tabControl.Selected += SetAllowedMoveNext;
			manualCert.CheckedChanged += SetAllowedMoveNext;

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
			AllowMoveNext = !IsHttps || (tabControl.SelectedTab != tabPageManual) || manualCert.Checked;
		}

		bool iis7 => SetupVariables.IISVersion.Major >= 7;

		bool IsHttps => (iis7 || !OSInfo.IsWindows) && Utils.IsHttps(SetupVariables.WebSiteIP, SetupVariables.WebSiteDomain);

		public override bool Hidden => !IsHttps;
		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if ((!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) || !IsHttps) && AllowMoveNext)
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
			SetupVariables.LetsEncryptEmail = null;
			SetupVariables.CertificateFile = null;
			SetupVariables.CertificatePassword = null;
			SetupVariables.CertificateStoreLocation = null;
			SetupVariables.CertificateStore = null;
			SetupVariables.CertificateFindType = null;
			SetupVariables.CertificateFindValue = null;

			if (IsHttps)
			{

				if (tabControl.SelectedTab == tabPageCertStore)
				{
					if (!CheckCertStore())
					{
						e.Cancel = true;
						return;
					}
					SetupVariables.CertificateStoreLocation = txtStoreLocation.Text;
					SetupVariables.CertificateStore = txtStoreName.Text;
					SetupVariables.CertificateFindType = txtStoreFindType.Text;
					SetupVariables.CertificateFindValue = txtStoreFindValue.Text;
				} else if (tabControl.SelectedTab == tabPageCertFile)
				{
					if (!CheckCertFile())
					{
						e.Cancel = true;
						return;
					}
					SetupVariables.CertificateFile = txtCertFileFile.Text;
					SetupVariables.CertificatePassword = txtCertFilePassword.Text;
				} else if (tabControl.SelectedTab == tabPageLetsEncrypt)
				{
					if (!CheckEmail())
					{
						e.Cancel = true;
						return;
					}
					SetupVariables.LetsEncryptEmail = txtLetsEncryptEmail.Text;
				} else if (tabControl.SelectedTab == tabPageManual)
				{
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
