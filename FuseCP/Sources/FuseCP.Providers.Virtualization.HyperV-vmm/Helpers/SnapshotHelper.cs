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
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.Virtualization
{
    public static class SnapshotHelper
    {
        public static VirtualMachineSnapshot GetFromPS(PSObject psObject, string runningSnapshotId = null)
        {
            var snapshot = new VirtualMachineSnapshot
            {
                Id = psObject.GetString("Id"),
                Name = psObject.GetString("Name"),
                VMName = psObject.GetString("VMName"),
                ParentId = psObject.GetString("ParentSnapshotId"),
                Created = psObject.GetProperty<DateTime>("CreationTime")
            };

            if (string.IsNullOrEmpty(snapshot.ParentId))
                snapshot.ParentId = null; // for capability

            if (!String.IsNullOrEmpty(runningSnapshotId))
                snapshot.IsCurrent = snapshot.Id == runningSnapshotId;

            return snapshot;
        }

        public static VirtualMachineSnapshot GetFromWmi(ManagementBaseObject objSnapshot)
        {
            if (objSnapshot == null || objSnapshot.Properties.Count == 0)
                return null;

            VirtualMachineSnapshot snapshot = new VirtualMachineSnapshot();
            snapshot.Id = (string)objSnapshot["InstanceID"];
            snapshot.Name = (string)objSnapshot["ElementName"];

            string parentId = (string)objSnapshot["Parent"];
            if (!String.IsNullOrEmpty(parentId))
            {
                int idx = parentId.IndexOf("Microsoft:");
                snapshot.ParentId = parentId.Substring(idx, parentId.Length - idx - 1);
                snapshot.ParentId = snapshot.ParentId.ToLower().Replace("microsoft:", "");
            }
            if (!String.IsNullOrEmpty(snapshot.Id))
            {
                snapshot.Id = snapshot.Id.ToLower().Replace("microsoft:", "");
            }
            snapshot.Created = Wmi.ToDateTime((string)objSnapshot["CreationTime"]);

            if (string.IsNullOrEmpty(snapshot.ParentId))
                snapshot.ParentId = null; // for capability

            return snapshot;
        }

        public static void Delete(PowerShellManager powerShell, VirtualMachineSnapshot snapshot, bool includeChilds)
        {
            Command cmd = new Command("Remove-VMSnapshot");
            cmd.Parameters.Add("VMName", snapshot.VMName);
            cmd.Parameters.Add("Name", snapshot.Name);
            if (includeChilds) cmd.Parameters.Add("IncludeAllChildSnapshots", true);

            powerShell.Execute(cmd, true);
        }

        public static void Delete(PowerShellManager powerShell, string vmName)
        {
            Command cmd = new Command("Remove-VMSnapshot");
            cmd.Parameters.Add("VMName", vmName);

            powerShell.Execute(cmd, true);
        }
    }
}
