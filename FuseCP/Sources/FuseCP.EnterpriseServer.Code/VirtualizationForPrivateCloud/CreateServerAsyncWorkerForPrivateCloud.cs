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
using FuseCP.Providers.VirtualizationForPC;

namespace FuseCP.EnterpriseServer
{
    public class CreateVMAsyncWorker: ControllerAsyncBase
    {
        public int ThreadUserId { get; set; }
        public VMInfo vmTemplate { get; set; }

        public CreateVMAsyncWorker()
        {
            ThreadUserId = -1; // admin
        }

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
            VirtualizationServerControllerForPrivateCloud.CreateVirtualMachineAsunc(vmTemplate);
        }
    }
}
