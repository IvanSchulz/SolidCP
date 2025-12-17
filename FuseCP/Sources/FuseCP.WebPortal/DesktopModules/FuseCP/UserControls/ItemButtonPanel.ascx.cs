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

namespace FuseCP.Portal
{
    public partial class ItemButtonPanel : FuseCPControlBase
    {
        public bool ButtonSaveVisible
        {
            set { btnSave.Visible = value; }
            get { return btnSave.Visible; }
        }

        public bool ButtonSaveExitVisible
        {
            set { btnSaveExit.Visible = value; }
            get { return btnSaveExit.Visible; }
        }

        public string ValidationGroup
        {
            set { 
                btnSave.ValidationGroup = value;
                btnSaveExit.ValidationGroup = value;
            }
            get { return btnSave.ValidationGroup; }
        }

        public string OnSaveClientClick
        {
            set
            {
                btnSave.OnClientClick = value;
                btnSaveExit.OnClientClick = value;
            }
        }


        public event EventHandler SaveClick = null;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveClick!=null)
            {
                SaveClick(this, e);
            }
        }

        public event EventHandler SaveExitClick = null;
        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (SaveExitClick!=null)
            {
                SaveExitClick(this, e);
            }
        }

    }
}
