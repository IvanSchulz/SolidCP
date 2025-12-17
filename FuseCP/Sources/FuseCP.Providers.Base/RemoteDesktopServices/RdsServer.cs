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

using System.Net;

namespace FuseCP.Providers.RemoteDesktopServices
{
    public class RdsServer
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public string Name
        {
            get
            {
                return string.IsNullOrEmpty(FqdName) ? string.Empty : FqdName.Split('.')[0];
            }
        }
        public string FqdName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ItemName { get; set; }
        public int? RdsCollectionId { get; set; }
        public string ConnectionEnabled { get; set; }
        public string Status { get; set; }
        public bool SslAvailable { get; set; }
        public string Controller { get; set; }
        public string ControllerName { get; set; }
        public string RDSCollectionName { get; set; }
    }
}
