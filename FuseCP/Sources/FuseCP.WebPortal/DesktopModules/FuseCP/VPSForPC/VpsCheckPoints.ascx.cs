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
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.Virtualization;
using FuseCP.EnterpriseServer;


namespace FuseCP.Portal.VPSForPC
{
    public partial class VpsCheckPoints : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCheckPoints();
            }
        }

        protected void LoadCheckPoints() 
        {
            try
            {
                VirtualMachineSnapshot[] checkPoints = ES.Services.VPSPC.GetVirtualMachineSnapshots(PanelRequest.ItemID);

                treeCheckPoints.Nodes.Clear();
                foreach (VirtualMachineSnapshot curr in checkPoints)
                {
                    treeCheckPoints.Nodes.Add(new TreeNode(curr.Name, curr.Id));
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("Load Snapshot error.", ex);
                btnCreateCheckPoint.Enabled = false;
                btnRestoreCheckPoint.Enabled = false;
            }
        }

        protected void btnCreateCheckPoint_Click(object sender, EventArgs e)
        {
            ResultObject ret = ES.Services.VPSPC.CreateSnapshot(PanelRequest.ItemID);

            if (ret.IsSuccess)
            {
                LoadCheckPoints();
                messageBox.ShowSuccessMessage("CreateCheckPointSuccess");
            }
            else
            {
                messageBox.ShowErrorMessage("CreateCheckPointError");
            }
        }

        protected void btnRestoreCheckPoint_Click(object sender, EventArgs e)
        {
            if (treeCheckPoints.SelectedNode != null)
            {
                ResultObject ret = ES.Services.VPSPC.ApplySnapshot(PanelRequest.ItemID, treeCheckPoints.SelectedNode.Value);


                if (ret.IsSuccess)
                {
                    LoadCheckPoints();
                    messageBox.ShowSuccessMessage("RestoreSnapshotSuccess");
                }
                else
                {
                    messageBox.ShowErrorMessage("RestoreSheckPointError");
                }
            }
            else
            {
                messageBox.ShowErrorMessage("RestoreSnapshotNotSelNode");
            }
        }
    }
}
