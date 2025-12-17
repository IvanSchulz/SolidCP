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

using System.DirectoryServices;

namespace FuseCP.Import.Enterprise
{
	public partial class OUForm : BaseForm
	{
		public const string OU = "organizationalUnit";
		public const string CONTAINER = "container";
		public const string COMPUTER = "computer";
		public const string USER = "user";
		public const string CONTACT = "contact";
		public const string GROUP = "group";
		public const string TMP = "Tmp";

		public OUForm()
		{
			InitializeComponent();
			
		}

		public void InitializeForm()
		{
			PopulateRootNode();
		}

		private void PopulateRootNode()
		{
			ouTree.Nodes.Clear();
			DirectoryEntry root = null;
			try
			{
				root = ADUtils.GetRootOU();
			}
			catch (Exception ex)
			{
				ShowError("Unable to load root OU.", ex);
			}
			if (root != null)
			{
				DataNode rootNode = AddTreeNode(null, root);
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
			ouTree.Update();
			try
			{
				DirectoryEntry parent = node.Tag as DirectoryEntry;

				foreach (DirectoryEntry child in parent.Children)
				{
					AddTreeNode(node, child);
				}
			}
			catch (Exception ex)
			{
				ShowError("Unable to load Active Directory data.", ex);
			}

			node.Populated = true;
			node.Expand();
		}

		private DataNode AddTreeNode(DataNode parentNode, DirectoryEntry entry)
		{
			bool hasChildren = true;
			DataNode node = new DataNode();
			node.Text = (string)entry.Properties["name"].Value;
			
			node.NodeType = entry.SchemaClassName;
			int imageIndex = 2;
			switch (entry.SchemaClassName)
			{
				case OU:
					imageIndex = 1;
					break;
				case CONTAINER:
					imageIndex = 2;
					break;
				case COMPUTER:
					imageIndex = 3;
					break;
				case USER:
					imageIndex = 4;
					hasChildren = false;
					break;
				case CONTACT:
					imageIndex = 8;
					hasChildren = false;
					break;
				case GROUP:
					imageIndex = 5;
					hasChildren = false;
					break;
				default:
					imageIndex = 6;
					break;
			}
			
			node.SelectedImageIndex = imageIndex;
			node.ImageIndex = imageIndex;
			node.Tag = entry;
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
				ouTree.Nodes.Add(node);
			return node;
		}

		private void OnSelectClick(object sender, EventArgs e)
		{
			DataNode node = ouTree.SelectedNode as DataNode;
			if (node == null || node.NodeType != OU)
			{
				ShowWarning("Please select Organizational Unit.");
				return;
			}

			this.directoryEntry = (DirectoryEntry)node.Tag;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private DirectoryEntry directoryEntry;

		public DirectoryEntry DirectoryEntry
		{
			get { return directoryEntry; }
		}



	}
}
