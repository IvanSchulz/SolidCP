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
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.Proxmox
{
    public partial class VpsDetailsSnapshots : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSnapshotsTree();
            }
        }

        private void BindSnapshotsTree()
        {
            VirtualMachineSnapshot[] snapshots = ES.Services.Proxmox.GetVirtualMachineSnapshots(PanelRequest.ItemID);

            // clear tree
            SnapshotsTree.Nodes.Clear();

            // fill tree by root nodes
            AddChildNodes(SnapshotsTree.Nodes, null, snapshots);

            // select first node
            if (SnapshotsTree.Nodes.Count > 0)
            {
                SnapshotsTree.Nodes[0].Selected = true;
            }

            // refresh
            BindSelectedNode();

            // quotas
            VirtualMachine vm = ES.Services.Proxmox.GetVirtualMachineItem(PanelRequest.ItemID);
            snapshotsQuota.QuotaUsedValue = snapshots.Length;
            snapshotsQuota.QuotaValue = vm.SnapshotsNumber;
            btnTakeSnapshot.Enabled = snapshots.Length < vm.SnapshotsNumber;
        }

        private void BindSelectedNode()
        {
            TreeNode node = SnapshotsTree.SelectedNode;

            btnApply.Enabled =
                btnRename.Enabled =
                btnDelete.Enabled =
                //btnDeleteSubtree.Enabled =
                SnapshotDetailsPanel.Visible = (node != null);

            NoSnapshotsPanel.Visible = (SnapshotsTree.Nodes.Count == 0);

            if (node != null)
            {
                // set name
                txtSnapshotName.Text = node.Text;

                // load snapshot details
                VirtualMachineSnapshot snapshot = ES.Services.Proxmox.GetSnapshot(PanelRequest.ItemID, node.Value);
                if (snapshot != null)
                    litCreated.Text = snapshot.Created.ToString();

                // set image
                /*
                imgThumbnail.ImageUrl =
                    string.Format("~/DesktopModules/FuseCP/Proxmox/VirtualMachineSnapshotImage.ashx?ItemID={0}&SnapshotID={1}&rnd={2}",
                    PanelRequest.ItemID, HttpUtility.UrlEncode(node.Value), DateTime.Now.Ticks);
                */
            }
        }

        private void AddChildNodes(TreeNodeCollection parent, string parentId, VirtualMachineSnapshot[] snapshots)
        {
            foreach (VirtualMachineSnapshot snapshot in snapshots)
            {
                if (snapshot.ParentId == parentId)
                {
                    // add node
                    TreeNode node = new TreeNode(snapshot.Name, snapshot.Id);
                    node.Expanded = true;
                    node.ImageUrl = PortalUtils.GetThemedImage("Proxmox/snapshot.png");
                    parent.Add(node);

                    // check if the current
                    if (snapshot.IsCurrent)
                    {
                        TreeNode nowNode = new TreeNode(GetLocalizedString("Now.Text"), "");
                        nowNode.ImageUrl = PortalUtils.GetThemedImage("Proxmox/start2.png");
                        nowNode.SelectAction = TreeNodeSelectAction.None;
                        node.ChildNodes.Add(nowNode);
                    }

                    // fill children
                    AddChildNodes(node.ChildNodes, snapshot.Id, snapshots);
                }
            }
        }

        protected void btnTakeSnapshot_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.Proxmox.CreateSnapshot(PanelRequest.ItemID);

                if (res.IsSuccess)
                {
                    // bind tree
                    BindSnapshotsTree();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_TAKE_SNAPSHOT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_TAKE_SNAPSHOT", ex);
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.Proxmox.ApplySnapshot(PanelRequest.ItemID, GetSelectedSnapshot());

                if (res.IsSuccess)
                {
                    // bind tree
                    BindSnapshotsTree();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_APPLY_SNAPSHOT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_APPLY_SNAPSHOT", ex);
            }
        }

        protected void btnRenameSnapshot_Click(object sender, EventArgs e)
        {
            try
            {
                string newName = txtSnapshotName.Text.Trim();
                ResultObject res = ES.Services.Proxmox.RenameSnapshot(PanelRequest.ItemID, GetSelectedSnapshot(), newName);

                if (res.IsSuccess)
                {
                    // bind tree
                    SnapshotsTree.SelectedNode.Text = newName;
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_RENAME_SNAPSHOT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_RENAME_SNAPSHOT", ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.Proxmox.DeleteSnapshot(PanelRequest.ItemID, GetSelectedSnapshot());

                if (res.IsSuccess)
                {
                    // bind tree
                    BindSnapshotsTree();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETE_SNAPSHOT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETE_SNAPSHOT", ex);
            }
        }

        /*
        protected void btnDeleteSubtree_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.Proxmox.DeleteSnapshotSubtree(PanelRequest.ItemID, GetSelectedSnapshot());

                if (res.IsSuccess)
                {
                    // bind tree
                    BindSnapshotsTree();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETE_SNAPSHOT_SUBTREE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETE_SNAPSHOT_SUBTREE", ex);
            }
        }
        */

        private string GetSelectedSnapshot()
        {
            return SnapshotsTree.SelectedNode.Value;
        }

        protected void SnapshotsTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindSelectedNode();
        }
    }
}
