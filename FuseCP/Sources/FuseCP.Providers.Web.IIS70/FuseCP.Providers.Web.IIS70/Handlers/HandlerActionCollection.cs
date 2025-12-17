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
using Microsoft.Web.Administration;

namespace FuseCP.Providers.Web.Handlers
{
	internal sealed class HandlerActionCollection : ConfigurationElementCollectionBase<HandlerAction>
	{
		// Methods
		public HandlerAction AddAt(int index, string name, string path, string verb)
		{
			HandlerAction element = base.CreateElement();
			element.Name = name;
			element.Path = path;
			element.Verb = verb;
			return base.AddAt(index, element);
		}

		public HandlerAction AddCopy(HandlerAction action)
		{
			HandlerAction destination = base.CreateElement();
			CopyInfo(action, destination);
			return base.Add(destination);
		}

		public HandlerAction AddCopyAt(int index, HandlerAction action)
		{
			HandlerAction destination = base.CreateElement();
			CopyInfo(action, destination);
			return base.AddAt(index, destination);
		}

		private static void CopyInfo(HandlerAction source, HandlerAction destination)
		{
			destination.Name = source.Name;
			destination.Modules = source.Modules;
			destination.Path = source.Path;
			destination.PreCondition = source.PreCondition;
			destination.RequireAccess = source.RequireAccess;
			destination.ResourceType = source.ResourceType;
			destination.ScriptProcessor = source.ScriptProcessor;
			destination.Type = source.Type;
			destination.Verb = source.Verb;
		}

		protected override HandlerAction CreateNewElement(string elementTagName)
		{
			return new HandlerAction();
		}

		// Properties
		public new HandlerAction this[string name]
		{
			get
			{
				for (int i = 0; i < base.Count; i++)
				{
					HandlerAction action = base[i];
					if (string.Equals(action.Name, name, StringComparison.OrdinalIgnoreCase))
					{
						return action;
					}
				}
				return null;
			}
		}
	}


}
