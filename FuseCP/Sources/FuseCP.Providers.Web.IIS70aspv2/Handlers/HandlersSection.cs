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
	internal sealed class HandlersSection : ConfigurationSection
	{
		// Fields
		private HandlerActionCollection _handlers;
		private static readonly string AccessPolicyAttribute = "accessPolicy";

		// Properties
		public HandlerAccessPolicy AccessPolicy
		{
			get
			{
				return (HandlerAccessPolicy)base[AccessPolicyAttribute];
			}
			set
			{
				base[AccessPolicyAttribute] = (int)value;
			}
		}

		public HandlerActionCollection Handlers
		{
			get
			{
				if (this._handlers == null)
				{
					this._handlers = (HandlerActionCollection)base.GetCollection(typeof(HandlerActionCollection));
				}
				return this._handlers;
			}
		}
	}


}
