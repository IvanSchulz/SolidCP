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
