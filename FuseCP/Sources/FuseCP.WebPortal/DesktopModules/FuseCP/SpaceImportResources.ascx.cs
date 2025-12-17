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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers;

namespace FuseCP.Portal
{
    public partial class SpaceImportResources : FuseCPModuleBase
    {
        private static TreeNode rootNode;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tree.Attributes.Add("onClick", "TreeViewCheckBoxClicked(event)");

                PrepareTree();
            }
        }

        private void PrepareTree()
        {
            // prepare tree
            tree.CollapseImageUrl = PortalUtils.GetThemedImage("min.gif");
            tree.ExpandImageUrl = PortalUtils.GetThemedImage("max.gif");
            tree.NoExpandImageUrl = PortalUtils.GetThemedImage("empty.gif");
            tree.Nodes.Clear();

            rootNode = new TreeNode();
            rootNode.ImageUrl = PortalUtils.GetThemedImage("folder.png");
            rootNode.Text = GetLocalizedString("Text.Resources");
            rootNode.SelectAction = TreeNodeSelectAction.None;
            rootNode.Value = "Root";
            rootNode.Expanded = true;
            tree.Nodes.Add(rootNode);

            // populate root node
            TreeNode node;
            ServiceProviderItemType[] types = ES.Services.Import.GetImportableItemTypes(PanelSecurity.PackageId);
            foreach (ServiceProviderItemType type in types)
            {
                node = new TreeNode();
                node.Value = "-" + type.ItemTypeId.ToString();
                node.Text = GetSharedLocalizedString("ServiceItemType." + type.DisplayName);
                node.PopulateOnDemand = true;
                node.SelectAction = TreeNodeSelectAction.None;
                node.ImageUrl = PortalUtils.GetThemedImage("folder.png");
                rootNode.ChildNodes.Add(node);
            }

            // Add Import HostHeaders
            node = new TreeNode();
            node.Value = "+100";
            node.Text = GetSharedLocalizedString("ServiceItemType.HostHeader");
            node.PopulateOnDemand = true;
            node.SelectAction = TreeNodeSelectAction.None;
            node.ImageUrl = PortalUtils.GetThemedImage("folder.png");
            rootNode.ChildNodes.Add(node);



        }

        protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("-"))
            {
                int itemTypeId = Utils.ParseInt(e.Node.Value.Substring(1), 0);
                string[] items = ES.Services.Import.GetImportableItems(PanelSecurity.PackageId, itemTypeId);

                foreach (string item in items)
                {
                    TreeNode node = new TreeNode();
                    node.Text = item;
                    node.Value = itemTypeId.ToString() + "|" + item;
                    node.ShowCheckBox = true;
                    node.SelectAction = TreeNodeSelectAction.None;
                    e.Node.ChildNodes.Add(node);
                }
            }

            if (e.Node.Value.StartsWith("+"))
            {
                int itemTypeId = Utils.ParseInt(e.Node.Value.Substring(1), 0);
                string[] items = ES.Services.Import.GetImportableItems(PanelSecurity.PackageId, itemTypeId * -1);

                switch (itemTypeId)
                {
                    case 100:

                        TreeNode headerNode = new TreeNode();
                        headerNode.Text = GetSharedLocalizedString("ServiceItemType.HostHeader");
                        headerNode.Value = "+" + itemTypeId.ToString();
                        headerNode.ShowCheckBox = true;
                        headerNode.SelectAction = TreeNodeSelectAction.None;
                        e.Node.ChildNodes.Add(headerNode);

                        foreach (string item in items)
                        {
                            string[] objectData = item.Split('|');

                            TreeNode userNode = null;
                            foreach (TreeNode n in headerNode.ChildNodes)
                            {
                                if (n.Value == "+" + itemTypeId.ToString() + "|" + objectData[1]) 
                                {
                                    userNode = n;
                                    break;
                                }
                            }

                            if (userNode == null)
                            {
                                userNode = new TreeNode();
                                userNode.Text = objectData[0];
                                userNode.Value = "+" + itemTypeId.ToString() + "|" + objectData[1];
                                userNode.ShowCheckBox = true;
                                userNode.SelectAction = TreeNodeSelectAction.None;
                                headerNode.ChildNodes.Add(userNode);
                            }

                            TreeNode siteNode = new TreeNode();
                            siteNode.Text = objectData[3];
                            siteNode.Value = "+" + itemTypeId.ToString() + "|" + item;
                            siteNode.ShowCheckBox = true;
                            userNode.SelectAction = TreeNodeSelectAction.None;
                            userNode.ChildNodes.Add(siteNode);
                        }

                        headerNode.Expand();
                        break;
                }

            }

        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            // collect data
            List<string> items = new List<string>();
            CollectNodesData(items, tree.Nodes);

            // import
            int result = ES.Services.Import.ImportItems(true, TaskID, PanelSecurity.PackageId, items.ToArray());

			if (result < 0)
			{
				ShowResultMessage(result);
				return;
			}

            // reset tree
            PrepareTree();

			// show progress dialog
			AsyncTaskID = TaskID;
			AsyncTaskTitle = GetLocalizedString("Text.ImportItems");
        }

        private void CollectNodesData(List<string> items, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                    items.Add(node.Value);

                // process children
                if(node.ChildNodes.Count > 0)
                    CollectNodesData(items, node.ChildNodes);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }


        void checkChildNodes(TreeNodeCollection ptnChildren, bool isChecked)
        {
            foreach (TreeNode childNode in ptnChildren)
            {
                childNode.Checked = isChecked;


                if (childNode.ChildNodes.Count > 0)
                {
                    this.checkChildNodes(childNode.ChildNodes, isChecked);
                }
            }
        }

        protected void tree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            foreach (TreeNode childNode in e.Node.ChildNodes)
            {
                childNode.Checked = e.Node.Checked;

                if (childNode.ChildNodes.Count > 0)
                {
                    this.checkChildNodes(childNode.ChildNodes, e.Node.Checked);
                }
            }
        }

        protected void tree_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
        }
    }
}
