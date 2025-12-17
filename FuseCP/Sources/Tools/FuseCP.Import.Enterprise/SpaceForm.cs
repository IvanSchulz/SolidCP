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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FuseCP.EnterpriseServer;


namespace FuseCP.Import.Enterprise
{
	public partial class SpaceForm : BaseForm
	{
		public const string USER = "User";
		public const string CUSTOMERS = "Customers";
		public const string SPACES = "Spaces";
		public const string SPACE = "Space";
		public const string TMP = "Tmp";

		private string username;
		private string password;

		public SpaceForm()
		{
			InitializeComponent();
		}

		Controller EnterpriseServer => new Controller();

		public void InitializeForm(string username, string password)
		{
			this.username = username;
			this.password = password;
			PopulateRootNode(username, password);
		}

		private void PopulateRootNode(string username, string password)
		{
			spaceTree.Nodes.Clear();
			UserInfo info = null;
			try
			{
				using (var ES = EnterpriseServer)
				{
					info = ES.UserController.GetUser(username);
					ES.SecurityContext.SetThreadPrincipal(info);
				}
			}
			catch (Exception ex)
			{
				ShowError("Unable to load user.", ex);
			}
			if (info != null)
			{
				DataNode rootNode = AddTreeNode(null, info.Username, 0, USER, info, true);
				rootNode.Expand();
			}
		}

		private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			DataNode node = e.Node as DataNode;
			ExpandNode(node);
		}

		private void ExpandNode(DataNode node)
		{
			if (node == null || node.Populated)
				return;

			node.Nodes.Clear();
			spaceTree.Update();

			switch (node.NodeType)
			{
				case USER:
					PopulateUser(node);
					break;
				case CUSTOMERS:
					PopulateCustomers(node);
					break;
				case SPACES:
					PopulateSpaces(node);
					break;
			}

			node.Populated = true;
			node.Expand();
		}

		private void PopulateSpaces(DataNode parentNode)
		{
			UserInfo info = parentNode.Tag as UserInfo;
			DataSet ds = null;
			try
			{
				using (var ES = EnterpriseServer)
				{
					ds = ES.PackageController.GetRawMyPackages(info.UserId);
				}
			}
			catch (Exception ex)
			{
				ShowError("Unable to load spaces.", ex);
			}
			if (ds != null)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					PackageInfo data = new PackageInfo();
					data.PackageId = Utils.GetDbInt32(dr["PackageId"]);
					data.PackageName = Utils.GetDbString(dr["PackageName"]);
					data.UserId = Utils.GetDbInt32(dr["UserId"]);
					AddTreeNode(parentNode, data.PackageName, 3, SPACE, data, false);
				}
			}

		}

		private void PopulateCustomers(DataNode parentNode)
		{
			UserInfo info = parentNode.Tag as UserInfo;
			DataSet ds = null;
			try
			{
				using (var ES = EnterpriseServer)
				{
					ds = ES.UserController.GetRawUsers(info.UserId, false);
				}
			}
			catch (Exception ex)
			{
				ShowError("Unable to load users.", ex);
			}
			if (ds != null)
			{

				foreach (DataRow dr in ds.Tables[0].Rows)
				{

					UserInfo user = new UserInfo();
					user.UserId = Utils.GetDbInt32(dr["UserId"]);
					user.Username = Utils.GetDbString(dr["Username"]);
					user.RoleId = Utils.GetDbInt32(dr["RoleId"]);
					AddTreeNode(parentNode, user.Username, 0, USER, user, true);
				}
			}

		}

		private void PopulateUser(DataNode parentNode)
		{
			UserInfo info = parentNode.Tag as UserInfo;
			if ((UserRole)info.RoleId != UserRole.User)
			{
				AddTreeNode(parentNode, CUSTOMERS, 1, CUSTOMERS, info, true);
			}
			AddTreeNode(parentNode, SPACES, 2, SPACES, info, true); 
		}

		private DataNode AddTreeNode(DataNode parentNode, string text, int imageIndex, string nodeType, object data, bool hasChildren)
		{
			DataNode node = new DataNode();
			node.Text = text;
			node.SelectedImageIndex = imageIndex;
			node.ImageIndex = imageIndex;
			node.NodeType = nodeType;
			node.Tag = data;
			if (hasChildren)
			{
				node.Populated = false;
				DataNode tmpNode = new DataNode();
				tmpNode.Text = "Expanding...";
				tmpNode.SelectedImageIndex = 2;
				tmpNode.ImageIndex = 2;
				tmpNode.NodeType = TMP;
				node.Nodes.Add(tmpNode);
			}
			else
			{
				node.Populated = true;
			}
			if (parentNode != null)
				parentNode.Nodes.Add(node);
			else
				spaceTree.Nodes.Add(node);
			return node;
		}

		private void OnSelectClick(object sender, EventArgs e)
		{
			DataNode node = spaceTree.SelectedNode as DataNode;
			if (node == null || node.NodeType != SPACE)
			{
				ShowWarning("Please select hosting space.");
				return;
			}
			PackageInfo data = node.Tag as PackageInfo;
			if (data == null || data.PackageId == 0 || data.PackageId == 1)
			{
				//ShowWarning("Invalid hosting space. Please select hosting space with allowed Exchange organizations.");
				return;
			}

			PackageContext cntx = null;
			try
			{
				using (var ES = EnterpriseServer)
				{
					cntx = ES.PackageController.GetPackageContext(data.PackageId);
				}
			}
			catch (Exception ex)
			{
				ShowError("Unable to load space data", ex);
				return;
			}

			if (cntx == null)
			{
				//ShowWarning("Invalid hosting space. Please select hosting space with allowed Exchange organizations.");
				return;
			}

			bool exchangeEnabled = false;
			bool orgEnabled = false;
			
			foreach (HostingPlanGroupInfo group in cntx.GroupsArray)
			{
				if (!group.Enabled)
					continue;
				if (group.GroupName == ResourceGroups.Exchange)
				{
					exchangeEnabled = true;
					continue;
				}
				else if (group.GroupName == ResourceGroups.HostedOrganizations)
				{
					orgEnabled = true;
					continue;
				}
			}
			if (!orgEnabled)
			{
				ShowWarning("Invalid hosting space. Please select hosting space with allowed organizations.");
				return;
			}
            Global.IsExchange = exchangeEnabled;
			this.selectedSpace = data;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private PackageInfo selectedSpace;

		public PackageInfo SelectedSpace
		{
			get { return selectedSpace; }
		}
	
	}
}
