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
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using FuseCP.Providers.OS;

namespace FuseCP.UniversalInstaller.Controls
{
    /// <summary>
    /// Components control
    /// </summary>
    internal partial class ComponentsControl : ResultViewControl
    {
        delegate void SetGridDataSourceCallback(List<ComponentInfo> dataSource);

        private string componentCode = null;
        private string componentVersion = null;
        private string componentSettingsXml = null;

        public ComponentsControl()
        {
            InitializeComponent();
            grdComponents.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Action on click Install link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInstallLinkClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grdComponents.Columns.IndexOf(colLink))
            {
                var component = grdComponents.Rows[e.RowIndex].DataBoundItem as ComponentInfo;
                if (component != null)
                {
                    StartInstaller(component);
                    StartLoadingComponents();
                }
            }
            else
            {

            }
        }

        public static string GetShellVersion()
        {
            return Installer.Current.GetEntryAssembly().GetName().Version.ToString();
        }

		private void StartInstaller(ComponentInfo component)
        {
            var res = Installer.Current.Install(component);
			Update();
			if (res)
			{
				AppContext.AppForm.ReloadApplication();
			}

            /*
            string applicationName = info.ApplicationName;
            string componentName = info.ComponentName;
            string componentCode = info.ComponentCode;
            string componentDescription = info.ComponentDescription;
            string component = info.Component;
            string version = info.Version.ToString();
            string fileName = info.FullFilePath.Replace('\\', Path.DirectorySeparatorChar);
            string installerPath = info.InstallerPath.Replace('\\', Path.DirectorySeparatorChar);
            string installerType = info.InstallerType;

            if (info.IsInstalled)
            {
                AppContext.AppForm.ShowWarning(Global.Messages.ComponentIsAlreadyInstalled);
                return;
            }
            try
            {
                // download installer
                Loader form = new Loader(fileName, (e) => AppContext.AppForm.ShowError(e));
                DialogResult result = form.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    string tmpFolder = FileUtils.GetTempDirectory();
                    string path = Path.Combine(tmpFolder, installerPath);
                    Update();
                    string method = "Install";
                    Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", installerType, method, path));

                    //prepare installer args
                    Hashtable args = new Hashtable();

                    args[Global.Parameters.ComponentName] = componentName;
                    args[Global.Parameters.ApplicationName] = applicationName;
                    args[Global.Parameters.ComponentCode] = componentCode;
                    args[Global.Parameters.ComponentDescription] = componentDescription;
                    args[Global.Parameters.Version] = version;
                    args[Global.Parameters.InstallerFolder] = tmpFolder;
                    args[Global.Parameters.InstallerPath] = installerPath;
                    args[Global.Parameters.InstallerType] = installerType;
                    args[Global.Parameters.Installer] = Path.GetFileName(fileName);
                    args[Global.Parameters.ShellVersion] = Installer.Current.LoadContext.GetShellVersion();
                    args[Global.Parameters.BaseDirectory] = FileUtils.GetCurrentDirectory();
                    args[Global.Parameters.ShellMode] = Global.VisualInstallerShell;
                    args[Global.Parameters.IISVersion] = Global.IISVersion;
                    args[Global.Parameters.SetupXml] = this.componentSettingsXml;
                    args[Global.Parameters.ParentForm] = FindForm();
                    args[Global.Parameters.UIType] = UI.Current.GetType().Name;

                    //run installer
                    var res = (Result)Installer.Current.LoadContext.Execute(path, installerType, method, new object[] { args }) == Result.OK; 
                    Log.WriteInfo(string.Format("Installer returned {0}", res));
                    Log.WriteEnd("Installer finished");
                    Update();
                    if (res)
                    {
                        AppContext.AppForm.ReloadApplication();
                    }
                    FileUtils.DeleteTempDirectory();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Installer error", ex);
                AppContext.AppForm.ShowError(ex);
            }
            finally
            {
                this.componentSettingsXml = null;
                this.componentCode = null;
            }
            */
        }

        /// <summary>
        /// Displays component description when entering grid row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var component = grdComponents.Rows[e.RowIndex].DataBoundItem as ComponentInfo;
            if (component != null)
            {
                lblDescription.Text = component.ComponentDescription;
            }
        }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
            StartLoadingComponents();
		}
		/// <summary>
		/// Start new thread to load components
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnLoadComponentsClick(object sender, EventArgs e)
        {
            StartLoadingComponents();
        }

        public void StartLoadingComponents()
        {
            //load list of available components in the separate thread
            AppContext.AppForm.StartAsyncProgress("Connecting...", true);
            Task.Run(LoadComponents);
        }

		/// <summary>
		/// Loads list of available components via web service
		/// </summary>
		private async Task LoadComponents()
        {
            try
            {
                Log.WriteStart("Loading list of available components");
                lblDescription.Text = string.Empty;
                //load components via web service
                var releases = Installer.Current.Releases;
                var dsComponents = await releases.GetAvailableComponentsAsync();

                //remove already installed components or components not available on this platform
                foreach (var component in dsComponents.ToArray())
                {
                    if (component.IsInstalled || !component.IsAvailableOnPlatform) dsComponents.Remove(component);
				}

				this.grdComponents.ClearSelection();
				this.grdComponents.SelectionChanged += (sender, args) => this.grdComponents.ClearSelection();

                Log.WriteEnd("Available components loaded");
                using (var alock = await Releases.Lock.LockAsync())
                {
                    SetGridDataSource(dsComponents);
                }
                AppContext.AppForm.FinishProgress();
            }
            catch (Exception ex)
            {
                Log.WriteError("Web service error", ex);
                AppContext.AppForm.FinishProgress();
                AppContext.AppForm.ShowServerError();
            }
        }

        /// <summary>
        /// Thread safe grid binding.
        /// </summary>
        /// <param name="dataSource">Data source</param>
        /// <param name="dataMember">Data member</param>
        private void SetGridDataSource(List<ComponentInfo> dataSource)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.grdComponents.InvokeRequired)
            {
                SetGridDataSourceCallback callBack = new SetGridDataSourceCallback(SetGridDataSource);
                this.grdComponents.Invoke(callBack, new object[] { dataSource });
            }
            else
            {
                this.grdComponents.DataSource = dataSource;
                //this.grdComponents.DataMember = dataMember;
            }
        }

        /// <summary>
        /// Installs component during unattended setup
        /// </summary>
        /// <param name="componentCode"></param>
        internal void InstallComponent(string componentCode, string componentVersion, string settingsXml)
        {
            //load list of available components in the separate thread
            this.componentCode = componentCode;
            this.componentVersion = componentVersion;
            this.componentSettingsXml = settingsXml;
            AppContext.AppForm.StartAsyncProgress("Connecting...", true);
            ThreadPool.QueueUserWorkItem(o => Install());
        }

        /// <summary>
        /// Loads list of available components via web service and install specified component
        /// during unattended setup
        /// </summary>
        private void Install()
        {
            LoadComponents();
            foreach (DataGridViewRow gridRow in grdComponents.Rows)
            {
                var component = gridRow.DataBoundItem as ComponentInfo;
                if (component != null)
                {
                    string code = component.ComponentCode;
                    string version = component.Version.ToString();
                    if (code == componentCode)
                    {
                        //check component version if specified
                        if (!string.IsNullOrEmpty(componentVersion))
                        {
                            if (version != componentVersion)
                                continue;
                        }
                        StartInstaller(component);
                        //AppContext.AppForm.ProceedUnattendedSetup();
                        break;
                    }
                }
            }
        }
    }
}
