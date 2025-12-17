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

namespace FuseCP.Providers.Web.Iis.Common
{
    using Microsoft.Web.Administration;

    internal sealed class FastCgiSection : ConfigurationSection
    {
        private FastCgiApplicationCollection _applications;

        public FastCgiApplicationCollection Applications
        {
            get
            {
                if (this._applications == null)
                {
                    this._applications = (FastCgiApplicationCollection) base.GetCollection(typeof(FastCgiApplicationCollection));
                }
                return this._applications;
            }
        }
    }
}

