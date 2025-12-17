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
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.VPS.UserControls
{
    public partial class DynamicMemory : FuseCPControlBase, IVirtualMachineSettingsControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public bool IsEditMode { get; set; }

        public VirtualMachineSettingsMode Mode { get; set; }

        public void BindItem(VirtualMachine item)
        {
        }

        public void SaveItem(ref VirtualMachine item)
        {
        }
    }
}
