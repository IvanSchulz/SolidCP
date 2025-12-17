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
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
	public partial class ExchangePublicFolders : FuseCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                // quota values
				BindStats();

                // build tree
                BuildFoldersTree();
            }
		}

		private void BindStats()
		{
			OrganizationStatistics stats = ES.Services.ExchangeServer.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
			foldersQuota.QuotaUsedValue = stats.CreatedPublicFolders;
			foldersQuota.QuotaValue = stats.AllocatedPublicFolders;
            if (stats.AllocatedPublicFolders != -1) foldersQuota.QuotaAvailable = stats.AllocatedPublicFolders - stats.CreatedPublicFolders;
		}

        private void BuildFoldersTree()
        {
			// clear all tree
			FoldersTree.Nodes.Clear();

			// get organization info
			Organization org = ES.Services.ExchangeServer.GetOrganization(PanelRequest.ItemID);

			string rootFolder = org.OrganizationId;
			TreeNode rootNode = new TreeNode(rootFolder, "");
			rootNode.Expanded = true;
			rootNode.ImageUrl = GetThemedImage("FileManager/OpenFolder.gif");
			FoldersTree.Nodes.Add(rootNode);

			// get public folder accounts
			ExchangeAccount[] folders = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.PublicFolder);

			// add folders to the tree
			foreach(ExchangeAccount folder in folders)
			{
				string[] path = folder.DisplayName.Substring(1).Split('\\');
				AddNodeToTree(folder.AccountId, path);
			}
        }

		private void AddNodeToTree(int accountId, string[] path)
		{
			TreeNodeCollection nodes = FoldersTree.Nodes;
			TreeNode node = null;
			int i = 0;
			while (i < path.Length)
			{
				bool found = false;
				foreach (TreeNode childNode in nodes)
				{
					if (String.Compare(childNode.Text, path[i], true) == 0)
					{
						node = childNode;
						nodes = childNode.ChildNodes;
						found = true;
						break;
					}
				}

				// create node if required
				if (!found)
				{
					node = new TreeNode(path[i], "");
					node.Expanded = true;
					node.ShowCheckBox = true;
					node.ImageUrl = GetThemedImage("FileManager/OpenFolder.gif");
					nodes.Add(node);
					nodes = node.ChildNodes;
				}

				// set accountId
				if (i == path.Length - 1)
					node.Value = accountId.ToString();

				// set navigation url
                if(!String.IsNullOrEmpty(node.Value))
				    node.NavigateUrl = EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "public_folder_settings",
					    "AccountID=" + node.Value,
					    "ItemID=" + PanelRequest.ItemID.ToString());

				i++;
			}
		}

        protected void btnCreatePublicFolder_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_public_folder",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnDeleteFolders_Click(object sender, EventArgs e)
        {
            
   
			List<int> accountIds = new List<int>();

            // check if any folder is checked to delete 
            if (FoldersTree.CheckedNodes.Count == 0 && FoldersTree.Nodes[0].ChildNodes.Count > 0)
            {
                messageBox.ShowWarningMessage("EXCHANGE_SELECT_PUBLIC_FOLDER_TO_DELETE");
            }
            else
            {
                if (FoldersTree.Nodes[0].ChildNodes.Count == 0)
                {
                    messageBox.ShowWarningMessage("EXCHANGE_NONE_PUBLIC_FOLDER_TO_DELETE");
                }
            }
            

            // delete folders
            GetSelectedFoldersRecursively(accountIds, FoldersTree.Nodes[0]);

			// delete selected folders
			try
			{
				int result = ES.Services.ExchangeServer.DeletePublicFolders(
					PanelRequest.ItemID, accountIds.ToArray());
				if (result < 0)
				{
					messageBox.ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("EXCHANGE_DELETE_PUBLIC_FOLDER", ex);
			}

            // re-build tree
            BuildFoldersTree();
             
			// rebind stats
			BindStats();
            
        }

		private void GetSelectedFoldersRecursively(List<int> accountIds, TreeNode parentNode)
        {
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                // recurse children
				GetSelectedFoldersRecursively(accountIds, node);
            }

            // delete node if required
            if (parentNode.Checked)
            {
                accountIds.Add(Utils.ParseInt(parentNode.Value, 0));
            }
        }
	}
}
