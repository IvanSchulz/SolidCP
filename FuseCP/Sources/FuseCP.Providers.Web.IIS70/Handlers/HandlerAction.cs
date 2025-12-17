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
using System.Security.AccessControl;
using System.Text;
using Microsoft.Web.Administration;

namespace FuseCP.Providers.Web.Handlers
{
	internal sealed class HandlerAction : ConfigurationElement
	{
		// Fields
		private static readonly string ModulesAttribute = "modules";
		private static readonly string NameAttribute = "name";
		private static readonly string PathAttribute = "path";
		private static readonly string PreConditionAttribute = "preCondition";
		private static readonly string RequireAccessAttribute = "requireAccess";
		private static readonly string ResourceTypeAttribute = "resourceType";
		private static readonly string ScriptProcessorAttribute = "scriptProcessor";
		private static readonly string TypeAttribute = "type";
		private static readonly string VerbAttribute = "verb";

		// Properties
		public string Modules
		{
			get
			{
				return (string)base[ModulesAttribute];
			}
			set
			{
				base[ModulesAttribute] = value;
			}
		}

		public string Name
		{
			get
			{
				return (string)base[NameAttribute];
			}
			set
			{
				base[NameAttribute] = value;
			}
		}

		public string Path
		{
			get
			{
				return (string)base[PathAttribute];
			}
			set
			{
				base[PathAttribute] = value;
			}
		}

		public string PreCondition
		{
			get
			{
				return (string)base[PreConditionAttribute];
			}
			set
			{
				base[PreConditionAttribute] = value;
			}
		}

		public HandlerRequiredAccess RequireAccess
		{
			get
			{
				return (HandlerRequiredAccess)base[RequireAccessAttribute];
			}
			set
			{
				base[RequireAccessAttribute] = (int)value;
			}
		}

		public ResourceType ResourceType
		{
			get
			{
				return (ResourceType)base[ResourceTypeAttribute];
			}
			set
			{
				base[ResourceTypeAttribute] = (int)value;
			}
		}

		public string ScriptProcessor
		{
			get
			{
				return (string)base[ScriptProcessorAttribute];
			}
			set
			{
				base[ScriptProcessorAttribute] = value;
			}
		}

		public string Type
		{
			get
			{
				return (string)base[TypeAttribute];
			}
			set
			{
				base[TypeAttribute] = value;
			}
		}

		public string Verb
		{
			get
			{
				return (string)base[VerbAttribute];
			}
			set
			{
				base[VerbAttribute] = value;
			}
		}
	}

 

}
