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
	public class ProxmoxNetworkHelper
	{
		private const int defaultvlan = 0;

		public static VirtualMachineNetworkAdapter[] Get(String Content)
		{
			List<VirtualMachineNetworkAdapter> adapters = new List<VirtualMachineNetworkAdapter>();
			try
			{
				JToken jsonResponse = JToken.Parse(Content);
				JObject configvalue = (JObject)jsonResponse["data"];
				foreach (var property in configvalue)
				{
					string val = (string)property.Value;
					if (property.Key.Contains("net"))
					{
						VirtualMachineNetworkAdapter adapter = CreateAdapter(val);
						adapter.Name = String.Format("{0} {1} VLAN {2}", property.Key, adapter.Name, adapter.vlan);
						adapters.Add(adapter);
					}
				}
			}
			catch
			{
				adapters = null;
			}
			return adapters.ToArray();
		}

		private static VirtualMachineNetworkAdapter CreateAdapter(String adapterinfo)
		{
			VirtualMachineNetworkAdapter adapter = new VirtualMachineNetworkAdapter();
			try
			{
				adapter.vlan = defaultvlan;
				Array adapterarray = adapterinfo.Split(',');
				foreach (String adapterval in adapterarray)
				{
					if (adapterval.Contains(":"))
					{
						adapter.MacAddress = adapterval.Split('=')[1];
						adapter.MacAddress = adapter.MacAddress.Replace(":", "");
						adapter.Name = adapterinfo.Split('=')[0];
					}
					else if (adapterval.Contains("bridge"))
					{
						adapter.SwitchName = adapterval.Split('=')[1];
					}
					else if (adapterval.Contains("tag"))
					{
						adapter.vlan = Convert.ToInt32(adapterval.Split('=')[1]);
					}

				}
			}
			catch
			{
				adapter = null;
			}
			return adapter;
		}
	}
}
