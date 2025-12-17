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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Web.Services
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class WCFServiceTypesAttribute: Attribute 
	{
		public Type[] Types { get; set; }
		public WCFServiceTypesAttribute(Type[] types)
		{
			Types = types;
		}
	}

    [AttributeUsage(AttributeTargets.Assembly)]
    public class HttpHandlerTypesAttribute : Attribute
    {
        public Type[] Types { get; set; }
        public HttpHandlerTypesAttribute(Type[] types)
        {
            Types = types;
        }
    }
}
