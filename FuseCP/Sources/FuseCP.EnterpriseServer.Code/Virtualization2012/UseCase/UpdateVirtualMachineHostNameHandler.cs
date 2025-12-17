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

using FuseCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using FuseCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using FuseCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using FuseCP.Providers.Common;
using FuseCP.Providers.Virtualization;
//using FuseCP.Providers.Virtualization2012;
using FuseCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.EnterpriseServer.Code.Virtualization2012.UseCase
{
    public class UpdateVirtualMachineHostNameHandler: ControllerBase
    {
        public UpdateVirtualMachineHostNameHandler(ControllerBase provider) : base(provider) { }
        
        public ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            if (String.IsNullOrEmpty(hostname))
                throw new ArgumentNullException("hostname");

            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            PowerShellScript.CheckCustomPsScript(PsScriptPoint.before_renaming, vm);

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "UPDATE_HOSTNAME", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
                vm.ClusterName = (Utils.ParseBool(settings["UseFailoverCluster"], false)) ? settings["ClusterName"] : null;

                // update virtual machine name
                JobResult result = vs.RenameVirtualMachine(vm.VirtualMachineId, hostname, vm.ClusterName);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                // update meta item
                vm.Name = hostname;
                PackageController.UpdatePackageItem(vm);

                // update NetBIOS name if required
                if (updateNetBIOS)
                {
                    result = KvpExchangeHelper.SendComputerNameKVP(itemId, hostname);
                    if (result.ReturnValue != ReturnCode.JobStarted
                        && result.Job.JobState == ConcreteJobState.Completed)
                    {
                        LogHelper.LogReturnValueResult(res, result);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CHANGE_ADMIN_PASSWORD_ERROR, ex);
                return res;
            }

            PowerShellScript.CheckCustomPsScript(PsScriptPoint.after_renaming, vm);

            TaskManager.CompleteResultTask();
            return res;
        }
    }
}
