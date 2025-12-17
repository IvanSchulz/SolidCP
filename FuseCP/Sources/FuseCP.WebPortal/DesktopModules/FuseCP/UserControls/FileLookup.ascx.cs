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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;

//using DNNTV = DotNetNuke.UI.WebControls;
using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
    public partial class FileLookup : FuseCPControlBase
    {

        public bool Enabled
        {
            get { return txtFile.Enabled; }
            set { txtFile.Enabled = value; }
        }

        public int PackageId
        {
            get { return (ViewState["PackageId"] != null) ? (int)ViewState["PackageId"] : PanelSecurity.PackageId; }
            set { ViewState["PackageId"] = value; InitTree(); }
        }

        public bool IncludeFiles
        {
            get { return (ViewState["IncludeFiles"] != null) ? (bool)ViewState["IncludeFiles"] : false; }
            set { ViewState["IncludeFiles"] = value; }
        }

        public string RootFolder
        {
            get { return (ViewState["RootFolder"] != null) ? (string)ViewState["RootFolder"] : ""; }
            set { ViewState["RootFolder"] = value; }
        }

        public string SelectedFile
        {
            get { return txtFile.Text; }
            set { txtFile.Text = value; }
        }

        public Unit Width
        {
            get { return txtFile.Width; }
            set { txtFile.Width = value; pnlLookup.Width = value; }
        }

        public string ValidationGroup
        {
            get { return valRequireFile.ValidationGroup; }
            set { valRequireFile.ValidationGroup = value; }
        }

        public bool ValidationEnabled
        {
            get { return valRequireFile.Enabled; }
            set { valRequireFile.Enabled = value; }
        }

        public bool DropShadow
        {
            get { return DropShadowExtender1.Opacity > 0.0F; }
            set { DropShadowExtender1.Opacity = value ? 0.6F : 0.0F; }
        }

        private bool treeInitialized = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(!treeInitialized)
                    InitTree();
            }
        }

        private void InitTree()
        {
            pnlLookup.Width = Width;

            // prepare tree
            DNNTree.CollapseImageUrl = ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/Images/min.gif"));
            DNNTree.ExpandImageUrl = ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/Images/max.gif"));
            DNNTree.NoExpandImageUrl = ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/Images/empty.gif"));

            DNNTree.Nodes.Clear();

            TreeNode node = new TreeNode();
            node.ImageUrl = ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/Images/folder.png"));
            node.Value = PackageId.ToString() + "," + RootFolder + "\\";
            node.Text = GetLocalizedString("Text.Root");
            node.PopulateOnDemand = true;
            DNNTree.Nodes.Add(node);

            // set flag
            treeInitialized = true;
        }

		protected void DNNTree_SelectedNodeChanged(object sender, EventArgs e)
		{
			if (DNNTree.SelectedNode != null)
			{
				string[] key = DNNTree.SelectedNode.Value.Split(',');
				string path = key[1];

				if (path.Length > 1 && path.EndsWith("\\"))
					path = path.Substring(0, path.Length - 1);

				path = path.Substring(RootFolder.Length);

                if (!String.IsNullOrEmpty(RootFolder) &&
                    !path.StartsWith("\\"))
                    path = "\\" + path;

                PopupControlExtender1.Commit(path);
			}
		}

		protected void DNNTree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
		{
			if (e.Node.ChildNodes.Count > 0)
				return;

			string[] key = e.Node.Value.Split(',');

			int packageId = Utils.ParseInt(key[0], 0);
			string path = key[1];

			// read child folders
            SystemFile[] files = null;

            try
            {
                files = ES.Services.Files.GetFiles(packageId, path, IncludeFiles);
            }
            catch (Exception ex)
            {
                // add error node
                TreeNode node = new TreeNode();
                node.Text = "Error: " + ex.Message;
                e.Node.ChildNodes.Add(node);
                return;
            }

			foreach (SystemFile file in files)
			{
				string fullPath = path + file.Name;
				if (file.IsDirectory)
					fullPath += "\\";

				TreeNode node = new TreeNode();
				node.Value = packageId.ToString() + "," + fullPath;
				node.Text = file.Name;
				node.PopulateOnDemand = (file.IsDirectory && !file.IsEmpty);
				
				node.ImageUrl = file.IsDirectory? ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/Images/folder.png")) : 
					ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/Images/file.png"));

				e.Node.ChildNodes.Add(node);
			}
		}
    }
}
