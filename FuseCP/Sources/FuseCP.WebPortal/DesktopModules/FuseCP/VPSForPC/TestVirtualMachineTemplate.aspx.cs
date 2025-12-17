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
using System.Web;

namespace FuseCP.Portal.VPSForPC
{
    public partial class TestVirtualMachineTemplate : System.Web.UI.Page
    {
        public const string EVALUATE_VIRTUAL_MACHINE_TEMPLATE = "EVALUATE_VIRTUAL_MACHINE_TEMPLATE";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEvaluate_Click(object sender, EventArgs e)
        {
            try
            {
                litResults.Text = HttpUtility.HtmlEncode(ES.Services.VPS.EvaluateVirtualMachineTemplate(Utils.ParseInt(txtItemId.Text.Trim(), 0), txtTemplate.Text));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage(EVALUATE_VIRTUAL_MACHINE_TEMPLATE, ex);                
            }
        }
    }
}
