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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FuseCP.EnterpriseServer;
using FuseCP.WebDavPortal.WebConfigSections;

namespace FuseCP.WebDav.Core.Config.Entities
{
    public class OfficeOnlineCollection : AbstractConfigCollection, IReadOnlyCollection<OfficeOnlineElement>
    {
        private readonly IList<OfficeOnlineElement> _officeExtensions;

        public OfficeOnlineCollection()
        {
            NewFilePath = ConfigSection.OfficeOnline.CobaltNewFilePath;
            CobaltFileTtl = ConfigSection.OfficeOnline.CobaltFileTtl;
            _officeExtensions = ConfigSection.OfficeOnline.Cast<OfficeOnlineElement>().ToList();
        }

        public bool IsEnabled {
            get
            {
                return GetWebdavSystemSettigns().GetValueOrDefault(EnterpriseServer.SystemSettings.WEBDAV_OWA_ENABLED_KEY, false);
            }
        }
        public string Url
        {
            get
            {
                return GetWebdavSystemSettigns().GetValueOrDefault(EnterpriseServer.SystemSettings.WEBDAV_OWA_URL, string.Empty);
            }
        }

        private SystemSettings GetWebdavSystemSettigns()
        {
            return ScpContext.Services.Organizations.GetWebDavSystemSettings() ?? new SystemSettings();
        }

        public string NewFilePath { get; private set; }
        public int CobaltFileTtl { get; private set; }

        public IEnumerator<OfficeOnlineElement> GetEnumerator()
        {
            return _officeExtensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _officeExtensions.Count; }
        }

        public bool Contains(string extension)
        {
            return _officeExtensions.Any(x=>x.Extension == extension);
        }
    }
}
