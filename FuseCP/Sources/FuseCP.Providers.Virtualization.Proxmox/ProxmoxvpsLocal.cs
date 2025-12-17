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
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using FuseCP.Providers.OS;
using FuseCP.Providers.HostedSolution.Proxmox;
using System.IO;

namespace FuseCP.Providers.Virtualization
{
	public class ProxmoxvpsLocal : Proxmoxvps
	{
		public override string ProxmoxClusterServerApiHost => "localhost";
		public override string ProxmoxClusterNode => Environment.MachineName;
		public override VirtualMachine CreateVirtualMachine(VirtualMachine vm)
		{
			string cmd = $"{DeploySSHScriptSettings} {DeploySSHScriptParamsSettings} {vm.OperatingSystemTemplateDeployParams}";

			cmd = cmd.Replace("[FQDN]", vm.Name);
			cmd = cmd.Replace("[CPUCORES]", vm.CpuCores.ToString());
			cmd = cmd.Replace("[RAMSIZE]", vm.RamSize.ToString());
			cmd = cmd.Replace("[HDDSIZE]", vm.HddSize[0].ToString());
			cmd = cmd.Replace("[OSTEMPLATENAME]", $"\"{vm.OperatingSystemTemplate}\"");
			cmd = cmd.Replace("[OSTEMPLATEFILE]", $"\"{vm.OperatingSystemTemplatePath}\"");
			cmd = cmd.Replace("[ADMINPASS]", $"\"{vm.AdministratorPassword}\"");
			cmd = cmd.Replace("[VLAN]", vm.defaultaccessvlan.ToString());
			cmd = cmd.Replace("[MAC]", vm.ExternalNicMacAddress);
			if (vm.ExternalNetworkEnabled)
			{
				cmd = cmd.Replace("[IP]", vm.PrimaryIP.IPAddress);
				cmd = cmd.Replace("[NETMASK]", vm.PrimaryIP.SubnetMask);
				cmd = cmd.Replace("[GATEWAY]", vm.PrimaryIP.DefaultGateway);
			}
			else
			{
				cmd = cmd.Replace("[IP]", "\"\"");
				cmd = cmd.Replace("[NETMASK]", "\"\"");
				cmd = cmd.Replace("[GATEWAY]", "\"\"");
			}

			string error = "Error creating wirtual machine.";
			try
			{
				var output = Shell.Default.Exec(cmd).Output().Result;
				error = $"Error creating wirtual machine. VM deploy script output:\n{output}";

				// Get created machine Id
				vm.Name = vm.Name.Split('.')[0];
				var createdMachine = GetVirtualMachines().FirstOrDefault(m => m.Name == vm.Name);
				if (createdMachine == null)
				{
					error = $"Can't find created machine. VM deploy script output:\n{output}";
					var ex = new Exception(error);

					HostedSolutionLog.LogError(error, ex);
					throw ex;
				}
				vm.VirtualMachineId = createdMachine.VirtualMachineId;

				// Update common settings
				UpdateVirtualMachine(vm);
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError(error, ex);
				throw;
			}

			return vm;
		}

        public override Stream GetFile(string vmId, string filename, bool delete = false)
        {
			var mem = new MemoryStream(); 
			using (var stream = File.OpenRead(filename)) stream.CopyTo(mem);
			if (delete) File.Delete(filename);
			mem.Seek(0, SeekOrigin.Begin);
			return mem;
        }


        public string GetInstalledVersion()
		{
			try
			{
				var output = Shell.Default.Exec($"pveversion -v").Output().Result;
				var match = Regex.Match(output, @"(?<=^proxmox-ve:\s*)(?<version>[0-9][0-9.]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				if (match.Success)
				{
					return match.Groups["version"].Value;
				}
			}
			catch { }

			return "";
		}
		public override bool IsInstalled() => !string.IsNullOrEmpty(GetInstalledVersion());
	}
}
