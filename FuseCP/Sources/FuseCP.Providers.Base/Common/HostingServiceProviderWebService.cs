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

namespace FuseCP.Providers
{
    public abstract class HostingServiceProviderWebService: IDisposable
    {
        public ServiceProviderSettingsSoapHeader settings = new ServiceProviderSettingsSoapHeader();

        private RemoteServerSettings serverSettings;
        private ServiceProviderSettings providerSettings;

        private IHostingServiceProvider provider;
        protected virtual IHostingServiceProvider Provider
        {
            get
            {
                if (provider == null)
                {
                    // try to create provider class
                    Type providerType = Type.GetType(ProviderSettings.ProviderType);
                    try
                    {
                        provider = (IHostingServiceProvider)Activator.CreateInstance(providerType);

                        ((HostingServiceProviderBase)provider).ServerSettings = ServerSettings;
                        ((HostingServiceProviderBase)provider).ProviderSettings = ProviderSettings;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(String.Format("Can not create '{0}' provider instance with '{1}' type",
                            ProviderSettings.ProviderName, ProviderSettings.ProviderType), ex);
                    }
                }
                return provider;
            }
        }

        public RemoteServerSettings ServerSettings
        {
            get
            {
                if (serverSettings == null)
                {
                    // parse server settings
                    serverSettings = new RemoteServerSettings(settings.Settings);
                }
                return serverSettings;
            }
            set => serverSettings = value;
        }

        public ServiceProviderSettings ProviderSettings
        {
            get
            {
                if (providerSettings == null)
                {
                    // parse provider settings
                    providerSettings = new ServiceProviderSettings(settings.Settings);
                }
                return providerSettings;
            }
            set { providerSettings = value; }
        }

        public void Dispose()
        {
            if (Provider is IDisposable disposableProvider) disposableProvider.Dispose();    
        }
    }
}
