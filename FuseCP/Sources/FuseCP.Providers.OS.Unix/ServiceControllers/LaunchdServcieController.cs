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
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using Claunia.PropertyList;

namespace FuseCP.Providers.OS;


public class LaunchdServiceController : ServiceController
{
	public const string ServicesDirectory = "/Library/LaunchDaemons";
	public override bool IsInstalled => OSInfo.IsMac;
	public Shell Shell => Shell.Standard;
	public override void SystemReboot() => Shell.Exec("launchctrl reboot");
	string ServiceFile(string serviceId) => Path.Combine(ServicesDirectory, $"{serviceId}.plist");
	public override IEnumerable<OSService> All()
	{
		var servicesText = Shell.Exec("launchctl list").Output().Result;
		var matches = Regex.Matches(servicesText, @"^\s*(?<name>.+?)\s+(?<pid>\d+)\s*$", RegexOptions.Multiline);
		foreach (Match match in matches)
		{
			var name = match.Groups["name"].Value;
			var pid = match.Groups["pid"].Value;
			var script = Path.Combine(ServicesDirectory, name);
			bool running = !string.IsNullOrEmpty(pid);
			string description = "";
			if (File.Exists(script))
			{
				var scriptText = File.ReadAllText(script);
				description = Regex.Match(scriptText, @"^\s*description\s*=\s*""(?<description>.*?)""\s*$", RegexOptions.Multiline)?.Value ?? "";
			}
			var srvc = new OSService()
			{
				Name = name,
				Id = name,
				Description = description,
				CanStop = running,
				CanPauseAndContinue = false,
				Status = running ? OSServiceStatus.Running : OSServiceStatus.Stopped
			};
			yield return srvc;
		}
	}

	public override OSService Info(string serviceId)
	{
		var output = Shell.Exec($"launchctl print system/{serviceId}").Output().Result;
		if (output == null) return null;
		var exists = Regex.IsMatch(output, @"^\s*(?<id>[^\n]*?)[ \t]*=[ \t]*{[ \t]*\r?\n", RegexOptions.Singleline);
		if (!exists)
		{
			if (File.Exists(ServiceFile(serviceId))) return new OSService()
			{
				Id = serviceId,
				Name = serviceId,
				Description = "",
				Status = OSServiceStatus.Stopped
			};
			else return null;
		}
		var match = Regex.Match(output, @"^\s*state\s*=\s*(?<state>.+?)\s*?$", RegexOptions.Multiline);
		string status = null;
		if (match.Success)
		{
			status = match.Groups["state"].Value;
		}

		return new OSService()
		{
			Id = serviceId,
			Name = serviceId,
			Description = "",
			Status = status == "running" ? OSServiceStatus.Running : OSServiceStatus.Stopped
		};
	}
	public override void ChangeStatus(string serviceId, OSServiceStatus status)
	{
		var service = Info(serviceId);
		if (service == null) throw new ArgumentException($"Service {serviceId} not found");
		if (service.Status == OSServiceStatus.Running)
		{
			if (status == OSServiceStatus.PausePending || status == OSServiceStatus.Paused ||
				status == OSServiceStatus.Stopped || status == OSServiceStatus.StopPending)
			{
				Shell.Exec($"launchctl bootout system {ServiceFile(serviceId)}");
			}
		}
		else
		{
			if (status == OSServiceStatus.StartPending || status == OSServiceStatus.Running ||
				status == OSServiceStatus.ContinuePending)
			{
				Shell.Exec($"launchctl bootstrap system {ServiceFile(serviceId)}");
			}
		}
	}
	public override void Remove(string serviceId)
	{
		var serviceFile = Path.Combine(ServicesDirectory, $"{serviceId}.plist");

		Shell.Exec($"launchctl disable system/{serviceId}");
		Shell.Exec($"launchctl bootout  system {ServiceFile(serviceId)}");
	}

	public override ServiceManager Install(ServiceDescription serviceDescription)
	{
		var srvc = serviceDescription as LaunchdServiceDescription;
		if (srvc == null) throw new ArgumentException("Service description is not of type LaunchdServiceDescription");

		var serviceId = srvc.ServiceId;
		var serviceFile = ServiceFile(serviceId);
		var dict = new NSDictionary();
		dict.Add("Label", serviceId);
		if (srvc.Program != null)
		{
			if (string.IsNullOrEmpty(srvc.Arguments))
			{
				dict.Add("Program", srvc.Program);
			}
			else
			{
				var args = new[] { srvc.Program }
					.Concat(
						Regex.Matches(srvc.Arguments, @"[^""\s]*(""[^""]*"")?[^""\s]*", RegexOptions.ExplicitCapture)
						.OfType<Match>()
						.Select(m => m.Value.Length > 2 && m.Value[0] == '"' && m.Value[m.Value.Length - 1] == '"' ?
							m.Value.Substring(1, m.Value.Length - 2) : m.Value))
						.ToArray();
				dict.Add("ProgramArguments", new NSArray(args
					.Select(arg => new NSString(arg))
					.ToArray()));
			}
		}

		if (srvc.UserName != null) dict.Add("UserName", srvc.UserName);
		if (srvc.OnDemand != null) dict.Add("OnDemand", srvc.OnDemand);
		if (srvc.StartOnMount != null) dict.Add("StartOnMount", srvc.StartOnMount);
		if (srvc.QueueDirectories != null && srvc.QueueDirectories.Count > 0)
		{
			dict.Add("QueueDirectories", new NSArray(srvc.QueueDirectories
				.Select(dir => new NSString(dir))
				.ToArray()));
		}
		if (srvc.WatchPaths != null && srvc.WatchPaths.Count > 0)
		{
			dict.Add("WatchPaths", new NSArray(srvc.WatchPaths
				.Select(dir => new NSString(dir))
				.ToArray()));
		}
		if (srvc.StartInterval != null) dict.Add("StartInterval", srvc.StartInterval);
		if (srvc.RunAtLoad != null) dict.Add("RunAtLoad", srvc.RunAtLoad);
		if (srvc.RootDirectory != null) dict.Add("RootDirectory", srvc.RootDirectory);
		if (srvc.WorkingDirectory != null) dict.Add("WorkingDirectory", srvc.WorkingDirectory);
		if (srvc.StandardOutPath != null) dict.Add("StandardOutPath", srvc.StandardOutPath);
		if (srvc.StandardErrorPath != null) dict.Add("StandardErrorPath", srvc.StandardErrorPath);
		if (srvc.StandardInPath != null) dict.Add("StandardInPath", srvc.StandardInPath);
		if (srvc.LowPriorityIO != null) dict.Add("LowPriorityIO", srvc.LowPriorityIO);
		if (srvc.AbandonProcessGroup != null) dict.Add("AbandonProcessGroup", srvc.AbandonProcessGroup);
		if (srvc.SessionCreate != null) dict.Add("SessionCreate", srvc.SessionCreate);
		if (srvc.KeepAlive != null) dict.Add("KeepAlive", srvc.KeepAlive);
		if (srvc.ExitTimeout != null) dict.Add("ExitTimeout", srvc.ExitTimeout);
		if (srvc.Environment != null && srvc.Environment.Count > 0)
		{
			var env = new NSDictionary();
			foreach (var kvp in srvc.Environment) env.Add(kvp.Key, kvp.Value);
			dict.Add("EnvironmentVariables", env);
		}

		using (var file = File.Create(serviceFile))
		{
			PropertyListParser.SaveAsXml(dict, file);
		}
		Shell.Exec($"launchctl bootstrap system {serviceFile}");
		Shell.Exec($"launchctl enable system/{serviceId}");

		return new ServiceManager(this, serviceId);
	}

	public override void Enable(string serviceId)
	{
		Shell.Exec($"launchctl enable system/{serviceId}");
	}
	public override void Disable(string serviceId)
	{
		Shell.Exec($"launchctl disable system/{serviceId}");
	}
}
