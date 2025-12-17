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

using System.Collections.Generic;
using System.Web.UI;
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.Code.Helpers
{

    public static class VirtualMachineSettingExtensions
    {
        public static void BindSettingsControls(this Control page, VirtualMachine vm)
        {
            page.GetSettingsControls().ForEach(s => s.BindItem(vm));
        }

        public static void SaveSettingsControls(this Control page, ref VirtualMachine vm)
        {
            foreach (var s in page.GetSettingsControls()) s.SaveItem(ref vm);
        }

        public static List<IVirtualMachineSettingsControl> GetSettingsControls(this Control parent)
        {
            var result = new List<IVirtualMachineSettingsControl>();
            foreach (Control control in parent.Controls)
            {
                if (control is IVirtualMachineSettingsControl)
                    result.Add((IVirtualMachineSettingsControl)control);

                if (control.HasControls())
                    result.AddRange(control.GetSettingsControls());
            }
            return result;
        }
    }
}
