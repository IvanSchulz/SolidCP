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
using System.Diagnostics;
using System.Text;
using System.Threading;
using Microsoft.Web.PlatformInstaller;
using FuseCP.Server.Code;

namespace FuseCP.Server.WPIService
{
    // Define a service contract.
    //[ServiceContract(Namespace = "http://Helicon.Zoo.WPIService")]
    //public interface IWPIService
    //{
    //  //  [OperationContract]
    //    void Initialize(string[] feeds);
        
    //    //[OperationContract]
    //    void BeginInstallation(string[] productsToInstall);

    //    //[OperationContract]
    //    string GetStatus();

    //    string GetLogs();
    //}
    
    enum EWPIServiceStatus
    {
        Initialised,
        Installation,
        InstallationComplete,
        InstallationError
    }

    // Service class which implements the service contract.
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class WPIService : WPIServiceContract
    {
        private WpiHelper  _wpiHelper;
        private string[] _productsToInstall;

        private EWPIServiceStatus _installationStatus;
        private string _statusMessage = "preparing...";

        private Thread _installerThread;
        private object _lock = new object();

        public bool IsFinished { get; private set; }

        #region IWPIService contract

        public override string Ping() 
        {
            return "OK";
        }
        
        public override void Initialize(string[] feeds)
        {
            lock (_lock)
            {
                if (_installationStatus == EWPIServiceStatus.Installation)
                {
                    throw new Exception("Invalid state, already in Installation process");
                }

                _installationStatus = EWPIServiceStatus.Initialised;

                if (_wpiHelper == null)
                {
                    _wpiHelper = new WpiHelper(feeds);
                    Console.WriteLine("_wpiHelper initialized");
                }
            }
        }

        public override void BeginInstallation(string[] productsToInstall)
        {
            lock (_lock)
            {

                if (_installationStatus != EWPIServiceStatus.Initialised)
                {
                    throw new Exception("Invalid state, expected EWPIServiceStatus.Initialised. now: " + _installationStatus);
                }

                _installationStatus = EWPIServiceStatus.Installation;
                _statusMessage = "Preparing for install";

                _productsToInstall = new string[productsToInstall.Length];
                productsToInstall.CopyTo(_productsToInstall,0);

                _installerThread = new Thread(new ThreadStart(InternalBeginInstallation));
                _installerThread.Start();
            }
        }


        public override string GetStatus()
        {

            lock(_lock)
            {

                string result = this._statusMessage;

                //Allow exit from app, if finished
                IsInstallationProceed();
                
                return result;
            }
        }

        public override string GetLogFileDirectory()
        {

            lock (_lock)
            {
                return null != _wpiHelper ? _wpiHelper.GetLogFileDirectory() : null;
            }
        }

        #endregion

        #region private implementaion
        private bool IsInstallationProceed()
        {
            if (_installationStatus == EWPIServiceStatus.InstallationComplete)
            {
                IsFinished = true;
                return false;
            }
            else if (_installationStatus == EWPIServiceStatus.InstallationError)
            {
                IsFinished = true;
                return false;
            }

            return true;
        }


        private void InternalBeginInstallation()
        {
            _wpiHelper.InstallProducts(
                _productsToInstall,
                true, //search and install dependencies
                WpiHelper.DeafultLanguage,
                InstallStatusUpdatedHandler, 
                InstallCompleteHandler
            );

            lock (_lock)
            {
                _installationStatus = EWPIServiceStatus.InstallationComplete;
            }
        }

        private void InstallCompleteHandler(object sender, EventArgs eventArgs)
        {
            lock(_lock)
            {
                _installationStatus = EWPIServiceStatus.InstallationComplete;
            }

            CheckIISAlive();
        }


        private void InstallStatusUpdatedHandler(object sender, InstallStatusEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}: ", e.InstallerContext.ProductName);

            switch (e.InstallerContext.InstallationState)
            {
                case InstallationState.Waiting:
                    sb.Append("please wait...").AppendLine();
                    break;
                case InstallationState.Downloading:
                    sb.Append("downloading").AppendLine();
                    if (e.ProgressValue > 0)
                    {
                        sb.AppendFormat("{0} of {1} Kb downloaded", e.ProgressValue,
                                        e.InstallerContext.Installer.InstallerFile.FileSize);
                        sb.AppendLine();
                    }
                    break;
                case InstallationState.Downloaded:
                    sb.Append("downloaded").AppendLine();
                    break;
                case InstallationState.DownloadFailed:
                    sb.AppendFormat("download failed").AppendLine();
                    sb.AppendLine(e.InstallerContext.InstallStateDetails);
                    break;
                case InstallationState.DependencyFailed:
                    sb.AppendFormat("dependency failed").AppendLine();
                    sb.AppendLine(e.InstallerContext.InstallStateDetails);
                    sb.AppendFormat("{0}: {1}", e.InstallerContext.ReturnCode.Status, e.InstallerContext.ReturnCode.DetailedInformation).AppendLine();
                    break;
                case InstallationState.Installing:
                    sb.Append("installing").AppendLine();
                    break;
                case InstallationState.InstallCompleted:
                    sb.Append("install completed").AppendLine();
                    break;
                case InstallationState.Canceled:
                    sb.AppendFormat("canceled").AppendLine();
                    sb.AppendLine(e.InstallerContext.InstallStateDetails);
                    sb.AppendFormat("{0}: {1}", e.InstallerContext.ReturnCode.Status, e.InstallerContext.ReturnCode.DetailedInformation).AppendLine();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            lock (_lock)
            {
                _statusMessage = sb.ToString();
            }
        }

        private void CheckIISAlive()
        {
            // run iisreset /start
            ProcessStartInfo processStartInfo = new ProcessStartInfo(@"C:\Windows\System32\iisreset.exe", "/start");
            processStartInfo.UseShellExecute = false;

            try
            {
                using (Process exeProcess = Process.Start(processStartInfo))
                {
                    exeProcess.WaitForExit();
                    Debug.Write("WPIService: iisreset /start returns "+exeProcess.ExitCode);
                }
            }
            catch (Exception ex)
            {
                Debug.Write("WPIService: iisreset /start exception: "+ex.ToString());
            }
        }

        #endregion
    }
}
