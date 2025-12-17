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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuseCP.UniversalInstaller.Controls
{
    public delegate void OperationProgressDelegate(int percentage);

    /// <summary>
    /// Loader form.
    /// </summary>
    internal partial class Loader : Form
    {
        private Core.SetupLoader appLoader;
        bool NoUnzip = false;

        public Loader()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private RemoteFile RemoteFile;
        private Action<Exception> Callback;
        private bool SetupOnly = false;
		public Loader(RemoteFile remoteFile, Action<Exception> callback, bool setupOnly)
            : this()
        {
            //Start(remoteFile, callback, setupOnly);
            RemoteFile = remoteFile;
            Callback = callback;
            SetupOnly = setupOnly;
        }

		/*public Loader(string localFile, string componentCode, string version, Action<Exception> callback)
            : this()
        {
            Start(componentCode, version, callback);
        }*/


		/*
        /// <summary>
        /// Resolves URL of the component's distributive and initiates download process.
        /// </summary>
        /// <param name="componentCode">Component code to resolve</param>
        /// <param name="version">Component version to resolve</param>
        private void Start(string componentCode, string version, Action<Exception> callback)
        {
            string remoteFile = Utils.GetDistributiveLocationInfo(componentCode, version);

            Start(remoteFile, callback);
        }*/

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

            Start(RemoteFile, Callback, SetupOnly);
		}

		/// <summary>
		/// Initializes and starts the app distributive download process.
		/// </summary>
		/// <param name="remoteFile">URL of the file to be downloaded</param>
		public void Start(RemoteFile remoteFile, Action<Exception> callback, bool setupOnly)
        {
            appLoader = new Core.SetupLoader(remoteFile);
            appLoader.SetupOnly = setupOnly;

            appLoader.OperationFailed += new EventHandler<Core.LoaderEventArgs<Exception>>(appLoader_OperationFailed);
            appLoader.OperationFailed += (object sender, Core.LoaderEventArgs<Exception> e) => {
                if (callback != null)
                {
                    try
                    {
                        callback(e.EventData);
                    }
                    catch
                    {
                        // Just swallow the exception as we have no interest in it.
                    }
                }
            };
            appLoader.ProgressChanged += new EventHandler<Core.LoaderEventArgs<Int32>>(appLoader_ProgressChanged);
            appLoader.StatusChanged += new EventHandler<Core.LoaderEventArgs<String>>(appLoader_StatusChanged);
            appLoader.OperationCompleted += new EventHandler<EventArgs>(appLoader_OperationCompleted);
            appLoader.NoUnzipStatus += (sender, args) => NoUnzip = true;

            Task.Run(appLoader.LoadAppDistributive);
        }

        void appLoader_OperationCompleted(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        void appLoader_StatusChanged(object sender, Core.LoaderEventArgs<String> e)
        {
            lblProcess.Text = e.StatusMessage;
            lblValue.Text = e.EventData;
            // Adjust Cancel button availability for an operation being performed
            if (btnCancel.Enabled != e.Cancellable)
            {
                btnCancel.Enabled = e.Cancellable;
            }
            // This check allows to avoid extra form redrawing operations
            if (ControlBox != e.Cancellable)
            {
                ControlBox = e.Cancellable;
            }
        }

        void appLoader_ProgressChanged(object sender, Core.LoaderEventArgs<Int32> e)
        {
            bool updateControl = (progressBar.Value != e.EventData);
            progressBar.Value = e.EventData;
            // Adjust Cancel button availability for an operation being performed
            if (btnCancel.Enabled != e.Cancellable)
            {
                btnCancel.Enabled = e.Cancellable;
            }
            // This check allows to avoid extra form redrawing operations
            if (ControlBox != e.Cancellable)
            {
                ControlBox = e.Cancellable;
            }
            //
            if (updateControl)
            {
                progressBar.Update();
            }
        }

        void appLoader_OperationFailed(object sender, Core.LoaderEventArgs<Exception> e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DetachEventHandlers();

            Installer.Current.Cancel.Cancel();

            Log.WriteInfo("Execution was canceled by user");
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void DetachEventHandlers()
        {
            // Detach event handlers
            if (appLoader != null)
            {
                appLoader.OperationFailed -= new EventHandler<Core.LoaderEventArgs<Exception>>(appLoader_OperationFailed);
                appLoader.ProgressChanged -= new EventHandler<Core.LoaderEventArgs<Int32>>(appLoader_ProgressChanged);
                appLoader.StatusChanged -= new EventHandler<Core.LoaderEventArgs<String>>(appLoader_StatusChanged);
                appLoader.OperationCompleted -= new EventHandler<EventArgs>(appLoader_OperationCompleted);
            }
        }

        private void OnLoaderFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
            {
                if (appLoader != null)
                {
                    appLoader.AbortOperation();
                    appLoader = null;
                }
            }
        }
    }
}
