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
using FuseCP.Providers.Common;

namespace FuseCP.Providers
{
    public abstract class HostingServiceProviderBase : IHostingServiceProvider
    {
        RemoteServerSettings serverSettings = new RemoteServerSettings();
        ServiceProviderSettings providerSettings = new ServiceProviderSettings();

        public RemoteServerSettings ServerSettings
        {
            get { return serverSettings; }
            set { serverSettings = value; }
        }

        public ServiceProviderSettings ProviderSettings
        {
            get { return providerSettings; }
            set { providerSettings = value; }
        }

        #region IHostingServiceProvider methods
		public virtual string[] GetProviderDefaults()
		{
			return new string[] { };
		}

        public virtual string[] Install()
        {
            // install in silence
            return new string[] { };
        }

        public virtual SettingPair[] GetProviderDefaultSettings()
        {
            return new SettingPair[] { };
        }

        public virtual void Uninstall()
        {
            // nothing to do
        }

        public abstract bool IsInstalled();

        public virtual void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
        {
            
            // do nothing
        }

        public virtual void DeleteServiceItems(ServiceProviderItem[] items)
        {
            // do nothing
        }

        public virtual ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            // don't calculate disk space
            return null;
        }

        public virtual ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(ServiceProviderItem[] items, DateTime since)
        {
            // don't calculate bandwidth
            return null;
        }
        #endregion

        #region Helper Methods
        protected void CheckTempPath(string path)
        {
            // check path
            string tempPath = Path.GetTempPath();

			//bug when calling from local machine
            //if (!path.ToLower().StartsWith(tempPath.ToLower()))
            //   throw new Exception("The path is not allowed");
        }
        #endregion
    }
}
