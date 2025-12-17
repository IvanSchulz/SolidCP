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
using System.Configuration;

namespace FuseCP.WebPortal
{
	public abstract class PageTitleProvider
	{
		private static PageTitleProvider _instance;
		public static PageTitleProvider Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				try
				{
					_instance = (PageTitleProvider)Activator.CreateInstance(Type.GetType(
						ConfigurationManager.AppSettings["WebPortal.PageTitleProvider"]));
				}
				catch (Exception ex)
				{
					throw new Exception("Could not create '{0}' page title provider", ex);
				}

				return _instance;
			}
		}

		public abstract string ProcessPageTitle(string title);
	}
}
