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
using System.Xml;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.EnterpriseServer
{
	public class SystemController: ControllerBase
	{
		public SystemController() : this(null) { }
		public SystemController(ControllerBase provider): base(provider) { }

		public SystemSettings GetSystemSettings(string settingsName)
		{
			// check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.IsActive);
			if (accountCheck < 0)
				return null;

			bool isDemoAccount = (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);

			return GetSystemSettingsInternal(settingsName, !isDemoAccount);
		}

        public SystemSettings GetSystemSettingsActive(string settingsName, bool decrypt)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsActive);
            if (accountCheck < 0)
                return null;

            bool isDemoAccount = (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);

            return GetSystemSettingsInternal(settingsName, decrypt && isDemoAccount);
        }

		internal SystemSettings GetSystemSettingsInternal(string settingsName, bool decryptPassword)
		{
			// create settings object
			SystemSettings settings = new SystemSettings();
			
			// get service settings
			IDataReader reader = null;

			try
			{
				// get service settings
				reader = Database.GetSystemSettings(settingsName);

				while (reader.Read())
				{
					string name = (string)reader["PropertyName"];
					string val = (string)reader["PropertyValue"];

					if (name.ToLower().IndexOf("password") != -1 && decryptPassword)
						val = CryptoUtils.Decrypt(val);

					settings[name] = val;
				}
			}
			finally
			{
				if (reader != null)
				{
					if (!reader.IsClosed) reader.Close();
					reader.Dispose();
				}
			}

			return settings;
		}

		public int SetSystemSettings(string settingsName, SystemSettings settings)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
				| DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			XmlDocument xmldoc = new XmlDocument();
			XmlElement root = xmldoc.CreateElement("properties");

			foreach (string[] pair in settings.SettingsArray)
			{
				string name = pair[0];
				string val = pair[1];

				if (name.ToLower().IndexOf("password") != -1)
					val = CryptoUtils.Encrypt(val);

				XmlElement property = xmldoc.CreateElement("property");

				property.SetAttribute("name", name);
				property.SetAttribute("value", val);

				root.AppendChild(property);
			}

			Database.SetSystemSettings(settingsName, root.OuterXml);

			return 0;
		}

		public bool GetSystemSetupMode()
		{
			var fcpaSystemSettings = GetSystemSettings(SystemSettings.SETUP_SETTINGS);
			// Flag either not found or empty
			if (String.IsNullOrEmpty(fcpaSystemSettings["EnabledFCPA"]))
			{
				return false;
			}
			//
			return true;
		}

		public int SetupControlPanelAccounts(string passwordA, string passwordB, string ip)
		{
			try
			{
				TaskManager.StartTask("SYSTEM", "COMPLETE_FCPA");
				//
				TaskManager.WriteParameter("Password A", passwordA);
				TaskManager.WriteParameter("Password B", passwordB);
				TaskManager.WriteParameter("IP Address", ip);
				//
				var enabledScpaMode = GetSystemSetupMode();
				//
				if (enabledScpaMode == false)
				{
					//
					TaskManager.WriteWarning("Attempt to execute FCPA procedure for an uknown reason");
					//
					return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
				}

				// Entering the security context into Supervisor mode
				SecurityContext.SetThreadSupervisorPrincipal();
				//
				var accountA = UserController.GetUserInternally("serveradmin");
				var accountB = UserController.GetUserInternally("admin");
				//
				var resultCodeA = UserController.ChangeUserPassword(accountA.UserId, passwordA);
				//
				if (resultCodeA < 0)
				{
					TaskManager.WriteParameter("Result Code A", resultCodeA);
					//
					return resultCodeA;
				}
				//
				var resultCodeB = UserController.ChangeUserPassword(accountB.UserId, passwordB);
				//
				if (resultCodeB < 0)
				{
					TaskManager.WriteParameter("Result Code B", resultCodeB);
					//
					return resultCodeB;
				}
				// Disable FCPA mode
				SetSystemSettings(SystemSettings.SETUP_SETTINGS, SystemSettings.Empty);
				// Operation has succeeded
				return 0;
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

        public bool CheckIsTwilioEnabled()
        {
            var settings = SystemController.GetSystemSettingsActive(SystemSettings.TWILIO_SETTINGS, false);

            return settings != null
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SystemSettings.TWILIO_ACCOUNTSID_KEY, string.Empty))
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SystemSettings.TWILIO_AUTHTOKEN_KEY, string.Empty))
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SystemSettings.TWILIO_PHONEFROM_KEY, string.Empty));
        }

		//Theme options
		public DataSet GetThemes()
		{
			return Database.GetThemes();
		}

		public DataSet GetThemeSettings(int ThemeID)
		{
			return Database.GetThemeSettings(ThemeID);
		}

		public DataSet GetThemeSetting(int ThemeID, string SettingsName)
		{
			return Database.GetThemeSetting(ThemeID, SettingsName);
		}

		public Data.DbType GetDatabaseType() => Database.DbType;

		public bool GetUseEntityFramework()
		{
			return Database.UseEntityFramework;
		}

		public int SetUseEntityFramework(bool useEntityFramework)
		{
			if (Database.IsSqlServer)
			{
				// check account
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
					| DemandAccount.IsActive);
				if (accountCheck < 0) return accountCheck;

				SystemSettings settings = new SystemSettings();

				// authentication settings
				settings = new SystemSettings();
				settings[SystemSettings.ALWAYS_USE_ENTITYFRAMEWORK] = useEntityFramework ? "True" : "False";

				int result = SetSystemSettings(SystemSettings.DEBUG_SETTINGS, settings);

				Database.AlwaysUseEntityFramework = useEntityFramework;

				return result;
			}

			return 0;
		}
	}
}
