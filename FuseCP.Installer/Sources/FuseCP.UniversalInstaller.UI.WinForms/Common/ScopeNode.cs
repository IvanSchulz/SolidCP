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
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using FuseCP.UniversalInstaller.Controls;

namespace FuseCP.UniversalInstaller
{
	internal enum NodeType
	{
		Servers,
		Server,
		Components,
		Component,
		Module,
		Settings,
		Service
	};
	
	internal class ScopeNode : TreeNode
	{
		private bool populated;
		private Icon largeIcon;
		private Icon smallIcon;
		private ResultViewControl resultView;
		private NodeType nodeType;
		private object dataObject;

		public ScopeNode()
		{
		}

		public bool Populated
		{
			get { return populated; }
			set { populated = value; }
		}

		public Icon LargeIcon
		{
			get { return largeIcon; }
			set { largeIcon = value; }
		}

		public Icon SmallIcon
		{
			get { return smallIcon; }
			set { smallIcon = value; }
		}

		public ResultViewControl ResultView
		{
			get { return resultView; }
			set { resultView = value; }
		}

		public NodeType NodeType
		{
			get { return nodeType; }
			set { nodeType = value; }
		}
		
		public object DataObject
		{
			get { return dataObject; }
			set { dataObject = value; }
		}
	}
}
