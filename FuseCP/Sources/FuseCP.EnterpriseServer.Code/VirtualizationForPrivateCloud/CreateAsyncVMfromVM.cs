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

//﻿using System;
//using System.Collections.Generic;
//using System.Web;
//using System.Threading;
//using System.ComponentModel;
//using FuseCP.Providers;
//using FuseCP.Providers.Common;
//using FuseCP.Providers.ResultObjects;
//using FuseCP.Providers.Virtualization;
//using FuseCP.Providers.VirtualizationForPC;

//namespace FuseCP.EnterpriseServer
//{
//    public class CreateAsyncVMfromVM
//    {
//        public static void Run(string sourceVM, VMInfo vmInfo)
//        {
//            BackgroundWorker createVMBackground = new BackgroundWorker();
//            ResultObject taskInfo = null;

//            object[] parameters = { sourceVM, vmInfo };

//            createVMBackground.DoWork += (sender, e) => {
//                string sourceVMName = (string)((object[])e.Argument)[0];
//                VMInfo vm = (VMInfo)((object[])e.Argument)[1];

//                e.Result = vm;
//                Guid taskGuid = Guid.NewGuid();
//                // Add audit log
//                 taskInfo = TaskManager.StartResultTask<ResultObject>("VPSForPC", "CREATE", taskGuid);
//                 TaskManager.ItemId = vm.Id;
//                 TaskManager.ItemName = vm.Name;
//                 TaskManager.PackageId = vm.PackageId;
//                 e.Result = CreateVM(sourceVMName, vm, taskGuid);
//            };
            
//            createVMBackground.RunWorkerCompleted += (sender, e) => {
//                if (e.Error != null || !String.IsNullOrEmpty(((VMInfo)e.Result).exMessage) )
//                {
//                    if (taskInfo != null)
//                    {
//                        TaskManager.CompleteResultTask(taskInfo
//                            , VirtualizationErrorCodes.CREATE_ERROR
//                            , new Exception(((VMInfo)e.Result).exMessage)
//                            , ((VMInfo)e.Result).logMessage);
//                    }
//                }
//                else
//                {
//                    if (taskInfo != null)
//                    {
//                        TaskManager.CompleteResultTask(taskInfo, null, null, ((VMInfo)e.Result).logMessage);
//                    }

//                    PackageController.UpdatePackageItem((VMInfo)e.Result);
//                }
//            };

//            createVMBackground.RunWorkerAsync(parameters);
//        }

//        private static VMInfo CreateVM(string sourceVMName, VMInfo vmInfo, Guid taskGuid)
//        {
//            VirtualizationServerForPC ws = new VirtualizationServerForPC();
//            ServiceProviderProxy.Init(ws, vmInfo.ServiceId);

//            vmInfo = ws.CreateVMFromVM(sourceVMName, vmInfo, taskGuid);

//            return vmInfo;
//        }
//    }
//}


﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using FuseCP.Providers.Virtualization;
using FuseCP.Providers.VirtualizationForPC;

namespace FuseCP.EnterpriseServer
{
    public class CreateVMFromVMAsyncWorker: ControllerAsyncBase
    {
        public int ThreadUserId { get; set; }
        public VMInfo vmTemplate {get; set; }
        public string vmName { get; set; }
        public int packageId { get; set; }

        public CreateVMFromVMAsyncWorker()
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
            VirtualizationServerControllerForPrivateCloud.CreateVMFromVMAsunc(packageId, vmTemplate, vmName);
        }
    }
}
