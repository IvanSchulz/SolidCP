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

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;

using FuseCP.Providers.Virtualization;

namespace FuseCP.EnterpriseServer
{
    public class CreateServerAsyncWorker: ControllerAsyncBase
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

        public string SummaryLetterEmail { get; set; }
        #endregion

        public CreateServerAsyncWorker()
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
            VirtualizationServerController.CreateVirtualMachineInternal(TaskId, Item, OsTemplate,
                ExternalAddressesNumber, RandomExternalAddresses, ExternalAddresses,
                PrivateAddressesNumber, RandomPrivateAddresses, PrivateAddresses,
                SummaryLetterEmail);
        }
        #endregion
    }
}
