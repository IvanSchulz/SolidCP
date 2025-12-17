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
using System.Web;
using System.Threading;

using FuseCP.Providers.Virtualization;
using FuseCP.EnterpriseServer.Code.Virtualization2012.Tasks;

namespace FuseCP.EnterpriseServer
{
    public class CreateServerAsyncWorker2012: ControllerAsyncBase
    {
        #region Properties
        public int ThreadUserId { get; set; }
        public string TaskId { get; set; }

        public VirtualMachine Item { get; set; }
        public LibraryItem OsTemplate { get; set; }

        public int ExternalAddressesNumber { get; set; }
        public bool RandomExternalAddresses { get; set; }
        public int[] ExternalAddresses { get; set; }

        public int PrivateAddressesNumber { get; set; }
        public bool RandomPrivateAddresses { get; set; }
        public string[] PrivateAddresses { get; set; }

        public int DmzAddressesNumber { get; set; }
        public bool RandomDmzAddresses { get; set; }
        public string[] DmzAddresses { get; set; }

        public string SummaryLetterEmail { get; set; }
        #endregion

        public CreateServerAsyncWorker2012()
        {
            ThreadUserId = -1; // admin
        }

        #region Create
        public void CreateAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(Create));
            t.Start();
        }

        private void Create()
        {
            // impersonate thread
            if (ThreadUserId != -1)
                SecurityContext.SetThreadPrincipal(ThreadUserId);

            // perform backup
            CreateVirtualMachineTask.CreateVirtualMachineNewTask(TaskId, Item, OsTemplate,
                ExternalAddressesNumber, RandomExternalAddresses, ExternalAddresses,
                PrivateAddressesNumber, RandomPrivateAddresses, PrivateAddresses,
                DmzAddressesNumber, RandomDmzAddresses, DmzAddresses,
                SummaryLetterEmail);
        }
        #endregion
    }
}
