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
using System.Text;

namespace FuseCP.Providers.Mail
{
	public class Tree
	{
		private TreeNodeCollection childNodes;

		public TreeNodeCollection ChildNodes
		{
			get { return childNodes; }
		}

		public Tree()
		{
			childNodes = new TreeNodeCollection();
		}

		public void Serialize(StringBuilder builder)
		{
			foreach(TreeNode node in childNodes.ToArray())
				node.Serialize(builder);
		}
	}

	public class TreeNodeCollection : ICollection<TreeNode>
	{
		private List<TreeNode> _collection;
		private Dictionary<string, TreeNode> _searchIndex;

		public TreeNode this[string keyName]
		{
			get
			{
				if (_searchIndex.ContainsKey(keyName))
					return _searchIndex[keyName];

				return null;
			}
		}

		public TreeNodeCollection()
		{
			_collection = new List<TreeNode>();
			_searchIndex = new Dictionary<string, TreeNode>();
		}

		public void Add(TreeNode node)
		{
			string keyName = node.NodeName;

			_collection.Add(node);

			if (keyName != TreeNode.Unnamed)
				_searchIndex.Add(keyName, node);
		}

		public TreeNode[] ToArray()
		{
			return _collection.ToArray();
		}

		#region ICollection<TreeNode> Members


		public void Clear()
		{
			_collection.Clear();
		}

		public bool Contains(TreeNode item)
		{
			return _collection.Contains(item);
		}

		public void CopyTo(TreeNode[] array, int arrayIndex)
		{
			_collection.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _collection.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(TreeNode item)
		{
			return _collection.Remove(item);
		}

		#endregion

		#region IEnumerable<TreeNode> Members

		public IEnumerator<TreeNode> GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		#endregion
	}

	public class TreeNode
	{
		private Tree root;
		private TreeNode parent;
		private TreeNodeCollection childNodes;

		private string nodeName;
		private string nodeValue;

		public const string Unnamed = "-";

		public TreeNodeCollection ChildNodes
		{
			get
			{
				EnsureChildControlsCreated();

				return childNodes;
			}
		}

		public bool IsUnnamed
		{
			get { return nodeName == Unnamed; }
		}

		public Tree Root
		{
			get { return root;  }
			set { root = value; }
		}

		public TreeNode Parent
		{
			get { return parent;  }
			set { parent = value; }
		}

		public string NodeName
		{
			get { return nodeName;	}
			set { nodeName = value; }
		}

		public string NodeValue
		{
			get { return nodeValue;  }
			set { nodeValue = value; }
		}

		public string this[string keyName]
		{
			get
			{
				EnsureChildControlsCreated();

				TreeNode keyNode = childNodes[keyName];
				if (keyNode != null)
					return keyNode.NodeValue;

				return null;
			}
			set
			{
				EnsureChildControlsCreated();

				TreeNode keyNode = childNodes[keyName];
				if (keyNode == null)
				{
					keyNode = new TreeNode(this);
					keyNode.NodeName = keyName;
					childNodes.Add(keyNode);
				}

				keyNode.NodeValue = value;
			}
		}

		public TreeNode(TreeNode parent) : this()
		{
			this.parent = parent;
		}

		public TreeNode()
		{
			this.nodeName = Unnamed;
		}

		private void EnsureChildControlsCreated()
		{
			if (childNodes == null)
				childNodes = new TreeNodeCollection();
		}

		public virtual void Serialize(StringBuilder builder)
		{
			if (childNodes != null)
			{
				builder.AppendLine(string.Concat("{ ", nodeName));

				foreach (TreeNode node in childNodes)
					node.Serialize(builder);

				builder.AppendLine("}");
			}
			else
			{
				builder.AppendLine(string.Concat(nodeName, "=", nodeValue));
			}
		}
	}
}
