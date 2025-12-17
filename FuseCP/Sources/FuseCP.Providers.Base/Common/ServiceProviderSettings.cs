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
using System.Collections.Specialized;
using System.Text;

namespace FuseCP.Providers
{
    public class ServiceProviderSettings
    {
        // settings hash
        StringDictionary hash = new StringDictionary();

        private int providerGroupID = 0;
        private string providerCode = "unknown";
        private string providerName = "Unknown";
        private string providerType = "";

        public ServiceProviderSettings()
        {
            // just do nothing
        }

        public ServiceProviderSettings(string[] settings)
        {
            if (settings != null)
            {
                // parse settings array
                foreach (string setting in settings)
                {
                    int idx = setting.IndexOf('=');
                    string key = setting.Substring(0, idx);
                    string val = setting.Substring(idx + 1);

                    if (key.StartsWith("Server:") ||
                        key.StartsWith("AD:"))
                        continue;

                    if (key == "Provider:ProviderGroupID")
                        ProviderGroupID = Int32.Parse(val);
                    else if (key == "Provider:ProviderCode")
                        ProviderCode = val;
                    else if (key == "Provider:ProviderName")
                        ProviderName = val;
                    else if (key == "Provider:ProviderType")
                        ProviderType = val;
                    else
                        hash[key] = val;
                }
            }
        }
        
        public string this[string settingName]
        {
            get
            {
                return hash[settingName];
            }
            set
            {
                hash[settingName] = value;
            }
        }

        public int GetInt(string settingName)
        {
            int result;
            Int32.TryParse(hash[settingName], out result);
            return result;
        }

        public long GetLong(string settingName)
        {
            long result;
            Int64.TryParse(hash[settingName], out result);
            return result;
        }

        public bool GetBool(string settingName)
        {
            bool result;
            Boolean.TryParse(hash[settingName], out result);
            return result;
        }

        public TimeSpan GetTimeSpan(string settingName)
        {
            double seconds;
            if (!Double.TryParse(hash[settingName], out seconds))
                seconds = 0;
            return TimeSpan.FromSeconds(seconds);
        }


        #region Public properties
        public int ProviderGroupID
        {
            get { return this.providerGroupID; }
            set { this.providerGroupID = value; }
        }

        public string ProviderCode
        {
            get { return this.providerCode; }
            set { this.providerCode = value; }
        }

        public string ProviderName
        {
            get { return this.providerName; }
            set { this.providerName = value; }
        }

        public string ProviderType
        {
            get { return this.providerType; }
            set { this.providerType = value; }
        }

        public StringDictionary Settings
        {
            get { return hash; }
            set { hash = value; }
        }
        #endregion
    }
}
