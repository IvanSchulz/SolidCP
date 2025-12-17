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
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.Virtualization
{
    public static class HardDriveHelper
    {
        public static VirtualHardDiskInfo[] Get(PowerShellManager powerShell, string vmname)
        {
            List<VirtualHardDiskInfo> disks = new List<VirtualHardDiskInfo>();

            Collection<PSObject> result = GetPS(powerShell, vmname);

            if (result != null && result.Count > 0)
            {
                foreach (PSObject d in result)
                {
                    VirtualHardDiskInfo disk = new VirtualHardDiskInfo();

                    disk.SupportPersistentReservations = Convert.ToBoolean(d.GetProperty("SupportPersistentReservations"));
                    disk.MaximumIOPS = Convert.ToUInt64(d.GetProperty("MaximumIOPS"));
                    disk.MinimumIOPS = Convert.ToUInt64(d.GetProperty("MinimumIOPS"));
                    disk.VHDControllerType = d.GetEnum<ControllerType>("ControllerType");
                    disk.ControllerNumber = Convert.ToInt32(d.GetProperty("ControllerNumber"));
                    disk.ControllerLocation = Convert.ToInt32(d.GetProperty("ControllerLocation"));
                    disk.Path = d.GetProperty("Path").ToString();
                    disk.Name = d.GetProperty("Name").ToString();

                    GetVirtualHardDiskDetail(powerShell, disk.Path, ref disk);

                    disks.Add(disk);
                }
            }
            return disks.ToArray();
        }

        //public static VirtualHardDiskInfo GetByPath(PowerShellManager powerShell, string vhdPath)
        //{
        //    VirtualHardDiskInfo info = null;
        //    var vmNames = new List<string>();

        //    Command cmd = new Command("Get-VM");

        //    Collection<PSObject> result = powerShell.Execute(cmd, true);

        //    if (result == null || result.Count == 0)
        //        return null;

        //    vmNames = result.Select(r => r.GetString("Name")).ToList();
        //    var drives = vmNames.SelectMany(n => Get(powerShell, n));

        //    return drives.FirstOrDefault(d=>d.Path == vhdPath);
        //}

        public static Collection<PSObject> GetPS(PowerShellManager powerShell, string vmname)
        {
            Command cmd = new Command("Get-VMHardDiskDrive");
            cmd.Parameters.Add("VMName", vmname);

            return powerShell.Execute(cmd, true);
        }

        public static void GetVirtualHardDiskDetail(PowerShellManager powerShell, string path, ref VirtualHardDiskInfo disk)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Command cmd = new Command("Get-VHD");
                cmd.Parameters.Add("Path", path);
                Collection<PSObject> result = powerShell.Execute(cmd, true);
                if (result != null && result.Count > 0)
                {
                    disk.DiskFormat = result[0].GetEnum<VirtualHardDiskFormat>("VhdFormat");
                    disk.DiskType = result[0].GetEnum<VirtualHardDiskType>("VhdType");
                    disk.ParentPath = result[0].GetProperty<string>("ParentPath");
                    disk.MaxInternalSize = Convert.ToInt64(result[0].GetProperty("Size"));
                    disk.FileSize = Convert.ToInt64(result[0].GetProperty("FileSize"));
                    disk.Attached = disk.InUse = Convert.ToBoolean(result[0].GetProperty("Attached"));
                }
            }
        }

        public static void Delete(PowerShellManager powerShell, VirtualHardDiskInfo[] disks)
        {
            if (disks != null && disks.GetLength(0) > 0)
            {
                foreach (VirtualHardDiskInfo disk in disks)
                {
                    Command cmd = new Command("Remove-item");
                    cmd.Parameters.Add("path", disk.Path);
                    powerShell.Execute(cmd, true);
                }
            }
        }
    }
}
