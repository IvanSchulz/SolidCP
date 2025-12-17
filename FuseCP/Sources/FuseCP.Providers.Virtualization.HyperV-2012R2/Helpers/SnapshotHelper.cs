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

using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.Virtualization
{
    public class SnapshotHelper
    {
        private PowerShellManager _powerShell;

        public SnapshotHelper(PowerShellManager powerShellManager)
        {
            _powerShell = powerShellManager;
        }

        public VirtualMachineSnapshot GetFromPS(PSObject psObject, string runningSnapshotId = null)
        {
            var snapshot = new VirtualMachineSnapshot
            {
                Id = psObject.GetString("Id"),
                Name = psObject.GetString("Name"),
                VMName = psObject.GetString("VMName"),
                ParentId = psObject.GetString("ParentSnapshotId"),
                Created = psObject.GetProperty<DateTime>("CreationTime"),
                SnapshotType = psObject.GetString("SnapshotType")
            };

            if (string.IsNullOrEmpty(snapshot.ParentId))
                snapshot.ParentId = null; // for capability

            if (!String.IsNullOrEmpty(runningSnapshotId))
                snapshot.IsCurrent = snapshot.Id == runningSnapshotId;

            return snapshot;
        }

        public VirtualMachineSnapshot GetFromCim(CimInstance objSnapshot)
        {
            if (objSnapshot == null || objSnapshot.CimInstanceProperties.Count == 0)
                return null;

            VirtualMachineSnapshot snapshot = new VirtualMachineSnapshot();
            snapshot.Id = (string)objSnapshot.CimInstanceProperties["InstanceID"].Value;
            snapshot.Name = (string)objSnapshot.CimInstanceProperties["ElementName"].Value;

            string parentId = (string)objSnapshot.CimInstanceProperties["Parent"].Value;
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
            snapshot.Created = (DateTime)objSnapshot.CimInstanceProperties["CreationTime"].Value;

            if (string.IsNullOrEmpty(snapshot.ParentId))
                snapshot.ParentId = null; // for capability

            return snapshot;
        }

        public void Delete(VirtualMachineSnapshot snapshot, bool includeChilds) //TODO: better to use VMObject instead of VMName ???
        {
            Command cmd = new Command("Remove-VMSnapshot");
            cmd.Parameters.Add("VMName", snapshot.VMName);
            cmd.Parameters.Add("Name", snapshot.Name);
            if (includeChilds) cmd.Parameters.Add("IncludeAllChildSnapshots", true);

            _powerShell.Execute(cmd, true);
        }

        public void Delete(VirtualMachineData vmData)
        {
            Command cmd = new Command("Remove-VMSnapshot");

            _powerShell.ExecuteOnVm(cmd, vmData);
        }
    }
}
