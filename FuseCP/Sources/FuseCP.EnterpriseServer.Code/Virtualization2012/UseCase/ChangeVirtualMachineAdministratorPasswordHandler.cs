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
using FuseCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using FuseCP.Providers.Common;
using FuseCP.Providers.Virtualization;
// FuseCP.Providers.Virtualization2012;
using FuseCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.EnterpriseServer.Code.Virtualization2012.UseCase
{
    public class ChangeVirtualMachineAdministratorPasswordHandler: ControllerBase
    {
        public ChangeVirtualMachineAdministratorPasswordHandler(ControllerBase provider) : base(provider) { }
        public ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return ChangeAdministratorPasswordInternal(itemId, password, false);
        }

        public ResultObject ChangeAdministratorPasswordAndCleanResult(int itemId, string password)
        {
            return ChangeAdministratorPasswordInternal(itemId, password, true);
        }

        private ResultObject ChangeAdministratorPasswordInternal(int itemId, string password, bool cleanResult)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "CHANGE_ADMIN_PASSWORD", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // change administrator password
                JobResult result = KvpExchangeHelper.SendAdministratorPasswordKVP(itemId, password, cleanResult);
                if (result.ReturnValue != ReturnCode.JobStarted
                    && result.Job.JobState == ConcreteJobState.Completed)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                // update meta item
                vm.AdministratorPassword = CryptoUtils.Encrypt(password);
                PackageController.UpdatePackageItem(vm);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CHANGE_ADMIN_PASSWORD_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }
    }
}
