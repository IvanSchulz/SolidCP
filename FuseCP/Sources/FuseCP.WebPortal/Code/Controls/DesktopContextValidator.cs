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

﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace FuseCP.WebPortal.Code.Controls
{
	public class DesktopValidationEventArgs : EventArgs
	{
		private bool contextIsValid;

		public bool ContextIsValid
		{
			get { return contextIsValid; }
			set { contextIsValid = value; }
		}
	}

	public class DesktopContextValidator : CustomValidator
	{
		public event EventHandler<DesktopValidationEventArgs> EvaluatingContext;

		protected override bool ControlPropertiesValid()
		{
			return true;
		}

		protected override bool EvaluateIsValid()
		{
			//
			DesktopValidationEventArgs args = new DesktopValidationEventArgs();
			//
			if (EvaluatingContext != null)
			{
				// trying evaluate manual context
				EvaluatingContext(this, args);
				//
				return args.ContextIsValid;
			}

			return true;
		}
	}
}
