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
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace FuseCP.Providers.Virtualization.Proxmox
{
	public class ProxmoxVHDHelper
	{

		public static VirtualHardDiskInfo[] Get(String Content)
		{
			List<VirtualHardDiskInfo> disks = new List<VirtualHardDiskInfo>();

			try
			{
				JToken jsonResponse = JToken.Parse(Content);
				JObject configvalue = (JObject)jsonResponse["data"];

				foreach (var property in configvalue)
				{
					string val = (string)property.Value;
					if ((property.Key.Contains("ide") || property.Key.Contains("sata") || property.Key.Contains("virtio") || property.Key.Contains("scsi")) && val.Contains(":"))
					{
						VirtualHardDiskInfo disk = new VirtualHardDiskInfo();
						disk.ControllerNumber = 1;
						disk.ControllerLocation = 1;
						if (property.Key.Contains("ide") || property.Key.Contains("virtio"))
						{
							disk.VHDControllerType = ControllerType.IDE;
						}
						else
						{
							disk.VHDControllerType = ControllerType.SCSI;
						}
						disk.Path = parsepath(val);
						disk.Name = property.Key;
						disks.Add(disk);
					}
				}

			}
			catch
			{
				disks = null;
			}

			return disks.ToArray();
		}


		static String parsepath(String io)
		{
			String Path = "";
			try
			{
				Array ioarray = io.Split(',');
				foreach (String ioval in ioarray)
				{
					if (ioval.Contains(':'))
						Path = ioval;
				}
			}
			catch
			{
				Path = io;
			}
			return Path;
		}
	}
}
