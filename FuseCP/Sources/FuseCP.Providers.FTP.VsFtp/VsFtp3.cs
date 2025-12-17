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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using System.Diagnostics;
using Mono.Posix;
using Mono.Unix;
using CryptSharp;

using FuseCP.Providers;
using FuseCP.Providers.OS;
using FuseCP.Providers.FTP;
using FuseCP.Server.Utils;
using Mono.Unix.Native;
using FuseCP.Providers.Database;

namespace FuseCP.Providers.FTP
{
	public class VsFtp3 : HostingServiceProviderBase, IFtpServer
	{
		readonly string NewLine = Environment.NewLine;
		const string PasswordFile = "/etc/vsftpd/ftpd.passwd";
		const string UsersConfigFolder = "/etc/vsftpd/users";
		const string VsftpdUser = "fusecp-vsftpd";
		const string FuseCPUser = "fusecp";
		const string VsftpdGroup = "fusecp";
		const string LocalRoot = "/var/www";
		const string LocalUmask = "022";
		const string VsftpServiceId = "vsftpd";

		IUnixOperatingSystem Unix => OSInfo.Unix;
		ServiceManager Service => Unix.ServiceController[VsftpServiceId];
		public string ConfigFile => ProviderSettings[nameof(ConfigFile)];

		VsFtpConfig config = null;
		public VsFtpConfig Config => config ??= new VsFtpConfig(ConfigFile);

		public Shell Shell => Shell.Standard;
		public void Reload() => Service.Reload();

		#region Sites

		public virtual void ChangeSiteState(string siteId, ServerState state)
		{
			// not implemented
		}

		public virtual string CreateSite(FtpSite site)
		{
			// not implemented
			return null;
		}

		public virtual void DeleteSite(string siteId)
		{
			// not implemented
		}

		public virtual FtpSite GetSite(string siteId)
		{
			// not implemented
			return null;
		}

		public virtual FtpSite[] GetSites()
		{
			// not implemented
			return null;
		}

		public virtual bool SiteExists(string siteId)
		{
			// not implemented
			return false;
		}

		public virtual ServerState GetSiteState(string siteId)
		{
			// not implemented
			return ServerState.Started;
		}

		public virtual void UpdateSite(FtpSite site)
		{
			// not implemented
		}

		#endregion

		#region Accounts

		IEnumerable<string> EnabledUsers
		{
			get
			{
				var passwdFile = File.ReadAllText(PasswordFile);
				return Regex.Matches(passwdFile, "^.*?(?=:)", RegexOptions.Multiline)
					.OfType<Match>()
					.Select(m => m.Value);
			}
		}
		IEnumerable<string> Users => Directory.EnumerateFiles(UsersConfigFolder)
			.Select(Path.GetFileName);

		public virtual bool AccountExists(string accountName)
		{
			return Users.Any(user => user == accountName);
		}
		public virtual FtpAccount GetAccount(string accountName)
		{
			int n = 0;
			if (Users.Any(user =>
			{
				n++;
				return user == accountName;
			}))
			{
				var configFile = $"{UsersConfigFolder}/{accountName}";
				var userConfig = new VsFtpConfig(configFile);

				var ftp = new FtpAccount()
				{
					CanRead = userConfig.DownloadEnable,
					CanWrite = userConfig.WriteEnable,
					CreatedDate = File.GetCreationTime(configFile),
					Enabled = EnabledUsers.Any(user => user == accountName),
					Folder = userConfig.LocalRoot,
					Name = accountName,
					GroupName = VsftpdGroup,
					Id = n,
					Password = "****"
				};
				return ftp;
			}

			return null;
		}

		public virtual FtpAccount[] GetAccounts()
		{
			return Users.Select(user => GetAccount(user)).ToArray();
		}
		public string PasswordToken(string password)
		{
			var token = Crypter.Sha512.Crypt(password, new CrypterOptions() { { CrypterOption.Rounds, 100000 } });
			return token;
		}

		public virtual void CreateAccount(FtpAccount account)
		{
			if (AccountExists(account.Name)) throw new InvalidOperationException($"User {account.Name} already exists.");

			if (!Directory.Exists(account.Folder) && !string.IsNullOrEmpty(account.Folder))
			{
				Directory.CreateDirectory(account.Folder);
				Unix.ChangeUnixFileOwner(account.Folder, FuseCPUser, VsftpdGroup);
			}

			// Create user's config file
			var configFile = $"{UsersConfigFolder}/{account.Name}";
			var config = new VsFtpConfig(configFile);
			config.LocalRoot = account.Folder;
			config.WriteEnable = account.CanWrite;
			config.DownloadEnable = account.CanRead;
			config.Save();
			account.CreatedDate = File.GetCreationTime(configFile);

			SetPassword(account);

			Reload();
		}

		void SetPassword(FtpAccount account)
		{
			// Update user's password
			var passwd = File.ReadAllText(PasswordFile).Trim();
			// remove old password
			passwd = Regex.Replace(passwd, @$"(?<=^|\n){Regex.Escape(account.Name)}:.*?(?:\r?\n|$)", "", RegexOptions.Singleline).Trim();
			// add new password
			if (account.Enabled)
			{
				if (!string.IsNullOrWhiteSpace(passwd)) passwd += NewLine;
				passwd += $"{account.Name}:{PasswordToken(account.Password)}";
			}
			File.WriteAllText(PasswordFile, passwd);
		}

		public virtual void UpdateAccount(FtpAccount account)
		{
			if (!AccountExists(account.Name)) throw new InvalidOperationException($"Ftp user account {account.Name} does not exist.");

			if (!Directory.Exists(account.Folder) && !string.IsNullOrEmpty(account.Folder))
			{
				Directory.CreateDirectory(account.Folder);
				Unix.ChangeUnixFileOwner(account.Folder, VsftpdUser, VsftpdGroup);
			}

			// Update user's config file
			var config = new VsFtpConfig($"{UsersConfigFolder}/{account.Name}");
			config.LocalRoot = account.Folder;
			config.WriteEnable = account.CanWrite;
			config.DownloadEnable = account.CanRead;
			config.Save();

			SetPassword(account);

			Reload();
		}

		public virtual void DeleteAccount(string accountName)
		{
			if (AccountExists(accountName))
			{
				// Remove user from password file
				var passwd = File.ReadAllText(PasswordFile);
				passwd = Regex.Replace(passwd, @$"^{Regex.Escape(accountName)}:.*?(?:\r?\n|$)", "", RegexOptions.Multiline);
				File.WriteAllText(PasswordFile, passwd);

				// Remove user's config file
				File.Delete($"{UsersConfigFolder}/{accountName}");

				Reload();
			}
			else throw new ArgumentException($"User {accountName} does not exist.");
		}
		#endregion

		#region HostingServiceProviderBase methods
		public void AddUnixUser(string user, string group)
		{
			Shell.Exec($"useradd --home /home/{user} --gid {group} -m --shell /bin/false {user}");
		}
		public override string[] Install()
		{
			if (!Regex.IsMatch(Config.Text, @"^# This file has been modified by FuseCP\.", RegexOptions.Multiline))
			{
				// Create wsp-vsftpd user
				AddUnixUser(VsftpdUser, VsftpdGroup);

				var dir = Path.GetDirectoryName(PasswordFile);
				if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

				// Create Password File
				File.WriteAllText(PasswordFile, "");

				// Configure PAM
				File.WriteAllText($"/etc/pam.d/{VsftpdUser}", @$"auth required pam_pwdfile.so pwdfile {PasswordFile}{NewLine}account required pam_permit.so");

				// Configure vsftpd
				Config.Text = $"# This file has been modified by FuseCP.{NewLine}{Config.Text}{NewLine}# FuseCP settings";
				Config.AnonymousEnable = false;
				Config.LocalEnable = true;
				Config.WriteEnable = true;
				Config.LocalUmask = LocalUmask;
				Config.LocalRoot = LocalRoot;
				Config.ChrootLocalUser = true;
				Config.AllowWriteableChroot = true;
				//Config.HideIds = true;
				if (!Directory.Exists(UsersConfigFolder)) Directory.CreateDirectory(UsersConfigFolder);
				Config.GuestEnable = true;
				Config.VirtualUseLocalPrivs = true;
				Config.PamServiceName = VsftpdUser;
				Config.NoprivUser = VsftpdUser;
				Config.GuestUsername = VsftpdUser;
				Config.Save();

				Reload();
			}

			return null;
		}

		public override void DeleteServiceItems(ServiceProviderItem[] items)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is FtpAccount)
				{
					try
					{
						DeleteAccount(item.Name);
					}
					catch (Exception ex)
					{
						Log.WriteError($"Error deleting '{item.Name}' vsftpd user", ex);
					}
				}
			}
		}

		public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is FtpAccount)
				{
					try
					{
						// change FTP account state
						FtpAccount account = GetAccount(item.Name);
						account.Enabled = enabled;
						UpdateAccount(account);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error switching '{0}' {1}", item.Name, item.GetType().Name), ex);
					}
				}
			}
		}


		public override bool IsInstalled()
		{
			if (!OSInfo.IsWindows)
			{
				var processes = Process.GetProcessesByName("vsftpd")
					.Select(p => p.ExecutableFile())
					.Concat(new string[] { Shell.Default.Find("vsftpd") })
					.Where(exe => exe != null)
					.Distinct();
				foreach (var exe in processes)
				{
					if (File.Exists(exe))
					{
						try
						{
							var output = Shell.Default.ExecScript($"\"{exe}\" -v 0>&1").Output().Result;
							var match = Regex.Match(output, @"[0-9][0-9.]+");
							if (match.Success)
							{
								var version = new Version(match.Value);
								if (version.Major == 3) return true;
							}
						}
						catch { }
					}
				}
			}
			return false;
		}
		#endregion

	}
}
