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
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.Virtualization;
using System.Collections.Generic;

namespace FuseCP.Portal.VPSForPC
{
	public static class MyVMInfoExtensions
	{
		public static string GetDomainName(this VMInfo vm)
		{
			var computerName = vm.ComputerName;
			//
			var result = String.Empty;
			//
			if (String.IsNullOrEmpty(computerName) == false)
			{
				//
				var indexOf = computerName.IndexOf(".");
				//
				if (indexOf > -1)
				{
					result = computerName.Substring(indexOf + 1);
				}
			}
			//
			return result;
		}

		public static string GetComputerName(this VMInfo vm)
		{
			var computerName = vm.ComputerName;
			//
			var result = String.Empty;
			//
			if (String.IsNullOrEmpty(computerName) == false)
			{
				//
				var indexOf = computerName.IndexOf(".");
				//
				if (indexOf == -1)
				{
					result = computerName;
				}
				else
				{
					result = computerName.Substring(0, indexOf);
				}
			}
			//
			return result;
		}
	}

    public partial class VpsDetailsGeneral : FuseCPModuleBase
    {
        private class ActionButton
        {
            public string Text { get; set; }
            public string Command { get; set; }
            public string Style { get; set; }
            public string OnClientClick { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGeneralDetails();
        }

        private void BindGeneralDetails()
        {
            VMInfo item = VirtualMachinesForPCHelper.GetCachedVirtualMachineForPC(PanelRequest.ItemID);

            if (!String.IsNullOrEmpty(item.CurrentTaskId))
            {
                DetailsTable.Visible = false;
                return;
            }

            VMInfo vm = null;
            try
            {
                vm = ES.Services.VPSPC.GetVirtualMachineGeneralDetails(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_VM_DETAILS", ex);
            }

            if (vm != null)
            {
                bool displayRDP = (Request.Browser.Browser == "IE"
                    && Request.Browser.ActiveXControls
                    && Request.Browser.VBScript
                    && vm.State != VMComputerSystemStateInfo.PowerOff
                    && vm.State != VMComputerSystemStateInfo.Paused
                    && vm.State != VMComputerSystemStateInfo.Saved);
				// Retrieve processed VM properties
				var computerName = vm.GetComputerName().ToUpper();
				var domainName = vm.GetDomainName();
				//
				lnkHostname.Text = computerName;
                lnkHostname.Visible = displayRDP;

				litHostname.Text = computerName;
                litHostname.Visible = !displayRDP;

                litDomain.Text = domainName;

                if (!IsPostBack)
                {
                    // set host name change form
                    UpdatePanel1.Attributes.Add("style", "Width:160px; Height:120px;");
                }

                litRdpPageUrl.Text = Page.ResolveUrl("~/DesktopModules/FuseCP/VPSForPC/RemoteDesktop/Connect.aspx?ItemID=" + PanelRequest.ItemID + "&Resolution=");

                litStatus.Text = GetLocalizedString("State." + vm.State.ToString());
                litCreated.Text = vm.CreatedDate.ToString();

                // CPU
                vmInfoPerfomence.Visible = (vm.State != VMComputerSystemStateInfo.CreationFailed);
                imgThumbnail.Visible = (vm.State != VMComputerSystemStateInfo.CreationFailed);

                if (vm.State != VMComputerSystemStateInfo.CreationFailed)
                {
                    cpuGauge.Progress = vm.PerfCPUUtilization;
                    litCpuPercentage.Text = String.Format(GetLocalizedString("CpuPercentage.Text"), vm.PerfCPUUtilization);

                    // RAM
                    if (vm.Memory > 0)
                    {
                        int ramPercent = Convert.ToInt32((float)vm.ProcessMemory / (float)vm.Memory * 100);
                        ramGauge.Total = vm.Memory;
                        ramGauge.Progress = vm.ProcessMemory;
                        litRamPercentage.Text = String.Format(GetLocalizedString("MemoryPercentage.Text"), ramPercent);
                        litRamUsage.Text = String.Format(GetLocalizedString("MemoryUsage.Text"), vm.ProcessMemory, vm.Memory);
                    }
                    else
                    {
                        ramGauge.Visible = false;
                        litRamPercentage.Visible = false;
                        litRamUsage.Visible = false;
                        locRam.Visible = false;
                    }

                    // HDD
                    if (vm.HddLogicalDisks != null && vm.HddLogicalDisks.Length > 0)
                    {
                        HddRow.Visible = true;

                        int freeHdd = 0;
                        int sizeHdd = 0;

                        foreach (LogicalDisk disk in vm.HddLogicalDisks)
                        {
                            freeHdd += disk.FreeSpace;
                            sizeHdd += disk.Size;
                        }

                        int usedHdd = sizeHdd - freeHdd;

                        int hddPercent = Convert.ToInt32((float)usedHdd / (float)sizeHdd * 100);
                        hddGauge.Total = sizeHdd;
                        hddGauge.Progress = usedHdd;
                        litHddPercentage.Text = String.Format(GetLocalizedString("HddPercentage.Text"), hddPercent);
                        litHddUsage.Text = String.Format(GetLocalizedString("HddUsage.Text"), freeHdd, sizeHdd, vm.HddLogicalDisks.Length);
                    }

                    // update image
                    imgThumbnail.ImageUrl =
                        String.Format("~/DesktopModules/FuseCP/VPSForPC/VirtualMachineImage.ashx?ItemID={0}&rnd={1}",
                        PanelRequest.ItemID, DateTime.Now.Ticks);
                }
                // load virtual machine meta item
                VMInfo vmi = VirtualMachinesForPCHelper.GetCachedVirtualMachineForPC(PanelRequest.ItemID);

                // draw buttons
                List<ActionButton> buttons = new List<ActionButton>();

                vmi.StartTurnOffAllowed = true;
                vmi.RebootAllowed = true;
                vmi.StartTurnOffAllowed = true;
                vmi.PauseResumeAllowed = true;
                vmi.ResetAllowed = true;

                if (vmi.StartTurnOffAllowed
                    && (vm.State == VMComputerSystemStateInfo.PowerOff
                    || vm.State == VMComputerSystemStateInfo.Saved
                    || vm.State == VMComputerSystemStateInfo.Stored))
                    buttons.Add(CreateActionButton("Start", "start.png"));

                if (vm.State == VMComputerSystemStateInfo.Running)
                {
                    if (vmi.RebootAllowed)
                        buttons.Add(CreateActionButton("Reboot", "reboot.png"));

                    if (vmi.StartTurnOffAllowed)
                        buttons.Add(CreateActionButton("ShutDown", "shutdown.png"));
                }

                if (vmi.StartTurnOffAllowed
                    && (vm.State == VMComputerSystemStateInfo.Running
                    || vm.State == VMComputerSystemStateInfo.Paused))
                    buttons.Add(CreateActionButton("TurnOff", "turnoff.png"));

                if (vmi.PauseResumeAllowed
                    && vm.State == VMComputerSystemStateInfo.Running)
                    buttons.Add(CreateActionButton("Pause", "pause.png"));

                if (vmi.PauseResumeAllowed
                    && vm.State == VMComputerSystemStateInfo.Paused)
                    buttons.Add(CreateActionButton("Resume", "start2.png"));

                repButtons.DataSource = buttons;
                repButtons.DataBind();

                // other actions
				//bool manageAllowed = VirtualMachinesForPCHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
				//btnChangeHostnamePopup.Visible = manageAllowed;
            }
            else
            {
                DetailsTable.Visible = false;
                messageBox.ShowErrorMessage("VPS_LOAD_VM_ITEM");
            }
        }

        private ActionButton CreateActionButton(string command, string icon)
        {
            ActionButton btn = new ActionButton();
            btn.Command = command;
            btn.Style = String.Format(
                "background: transparent url({0}) left center no-repeat;",
                PortalUtils.GetThemedImage(String.Format("VPS/{0}", icon)));

            string localizedText = GetLocalizedString("Command." + command);
            btn.Text = localizedText != null ? localizedText : command;

            btn.OnClientClick = GetLocalizedString("OnClientClick." + command);

            return btn;
        }

        protected void repButtons_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            try
            {
                ResultObject res = null;

                string command = e.CommandName;
                if (command == "Snapshot")
                {
                    res = ES.Services.VPSPC.CreateSnapshot(PanelRequest.ItemID);
                }
                else
                {
                    // parse command
                    VirtualMachineRequestedState state = (VirtualMachineRequestedState)Enum.Parse(
                        typeof(VirtualMachineRequestedState), command, true);

                    // call services
                    res = ES.Services.VPSPC.ChangeVirtualMachineState(PanelRequest.ItemID, state);
                }

                // check results
                if (res.IsSuccess)
                {
                    if (command == "Snapshot")
                    {
                        // go to snapshots screen
                        Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_snapshots",
                            "SpaceID=" + PanelSecurity.PackageId.ToString()));
                    }
                    else
                    {
                        // return
                        BindGeneralDetails();
                        return;
                    }
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_CHANGE_VM_STATE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_CHANGE_VM_STATE", ex);
            }
        }
    }
}
