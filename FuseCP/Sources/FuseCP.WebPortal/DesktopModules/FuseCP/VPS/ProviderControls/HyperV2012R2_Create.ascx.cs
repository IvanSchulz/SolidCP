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
using System.Web.UI;
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.ProviderControls
{
    public partial class HyperV2012R2_Create : FuseCPControlBase, IVirtualMachineSettingsControl
    {
        private bool _isEditMode;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                _isEditMode = value;
                if (_isEditMode)
                {
                    SettingContols.ForEach(s => s.Mode |= VirtualMachineSettingsMode.Edit);
                } else
                {
                    SettingContols.ForEach(s => s.Mode &= ~VirtualMachineSettingsMode.Edit);
                }
            }
        }

        VirtualMachineSettingsMode mode;
        public VirtualMachineSettingsMode Mode {
            get => mode;
            set
            {
                mode = value;
                SettingContols.ForEach(s => s.Mode = mode);
            }
        }


        public void BindItem(VirtualMachine item)
        {
            SettingContols.ForEach(s => s.BindItem(item));
        }

        public void SaveItem(ref VirtualMachine item)
        {
            foreach (var s in SettingContols) s.SaveItem(ref item);
        }

        private List<IVirtualMachineSettingsControl> SettingContols
        {
            get
            {
                return Controls
                    .Cast<Control>()
                    .Where(c => c is IVirtualMachineSettingsControl)
                    .Cast<IVirtualMachineSettingsControl>()
                    .ToList();
            }
        }
    }
}
