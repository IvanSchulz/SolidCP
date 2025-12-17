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

using FuseCP.EnterpriseServer.Code.Virtualization2012.Tasks;
using FuseCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuseCP.EnterpriseServer.Code.Virtualization2012
{
    class VirtualizationAsyncWorker2012: ControllerAsyncBase
    {
        #region Main Properties
        public int ThreadUserId { get; set; }
        public int ItemId { get; set; }
        public VirtualMachine Vm { get; set; }
        #endregion

        #region VPS Delete Properties
        public bool SaveFiles { get; set; }
        public bool ExportVps { get; set; }
        public string ExportPath { get; set; }
        public bool KeepPackageItem { get; set; } //Need for reinstall keep SQL data.
        #endregion

        #region VPS Reinstall Properties
        public string AdminPassword { get; set; }
        public string[] PrivIps { get; set; }
        public string[] DmzIps { get; set; }
        public LibraryItem OsTemplate { get; set; }
        #endregion


        public VirtualizationAsyncWorker2012()
        {
            ThreadUserId = -1; // admin
        }

        #region Delete VPS
        public void DeleteVPSAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(DeleteVPS));
            t.Start();
        }
        private void DeleteVPS()
        {
            // impersonate thread
            if (ThreadUserId != -1)
                SecurityContext.SetThreadPrincipal(ThreadUserId);

            DeleteVirtualMachineTask.DeleteVirtualMachineNewTask(ItemId, Vm, SaveFiles, ExportVps, ExportPath, KeepPackageItem);
        }
        #endregion

        #region Reinstall VPS
        public void ReinstallVPSAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(ReinstallVPS));
            t.Start();
        }
        private void ReinstallVPS()
        {
            // impersonate thread
            if (ThreadUserId != -1)
                SecurityContext.SetThreadPrincipal(ThreadUserId);

            ReinstallVirtualMachineTask.ReinstallVirtualMachineNewTask(ItemId, Vm, OsTemplate, AdminPassword, PrivIps, DmzIps, SaveFiles, ExportVps, ExportPath);
        }
        #endregion
    }
}
