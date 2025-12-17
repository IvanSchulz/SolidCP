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

using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CustomControls
{
    public class SwitchLabel : Label
    {
        protected void RadioButton_FixAutoPostBack_OnPreRender(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            HtmlGenericControl label = (HtmlGenericControl)radioButton.Parent;

            // Set onclick handler of the parent label to be the same as the radio button.
            //   This fixes issues caused by bootstrap javascript which adds labels.
            label.Attributes.Add("onclick",
                "javascript:setTimeout('__doPostBack(\\'" +
                radioButton.UniqueID + "\\',\\'\\')', 0)");
        }
    }
}
