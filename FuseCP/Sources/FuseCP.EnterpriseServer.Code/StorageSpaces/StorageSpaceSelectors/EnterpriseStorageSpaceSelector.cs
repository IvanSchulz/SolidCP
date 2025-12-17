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
using System.Linq;
using FuseCP.Providers.StorageSpaces;
using FuseCP.EnterpriseServer.Data;

namespace FuseCP.EnterpriseServer
{
    public class EnterpriseStorageSpaceSelector : ControllerBase, IStorageSpaceSelector
    {
        private readonly int _esId;

        public EnterpriseStorageSpaceSelector(ControllerBase provider, int esId): base(provider)
        {
            _esId = esId;
        }


        public StorageSpace FindBest(string groupName, long quotaSizeInBytes)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException("groupName");
            }


            var storages = ObjectUtils.CreateListFromDataReader<StorageSpace>(Database.GetStorageSpacesByResourceGroupName(groupName)).Where(x => !x.IsDisabled).ToList();

            if (!storages.Any())
            {
                throw new Exception(string.Format("Storage spaces not found for '{0}' resource group", groupName));
            }

            var service = ServerController.GetServiceInfo(_esId);

            storages = storages.Any(x => x.ServerId == service.ServerId) ? storages.Where(x => x.ServerId == service.ServerId).ToList() : storages;

            var orderedStorages = storages.OrderByDescending(x => x.FsrmQuotaSizeBytes - x.UsedSizeBytes);

            var bestStorage = orderedStorages.First();

            if (bestStorage.FsrmQuotaSizeBytes - bestStorage.UsedSizeBytes < quotaSizeInBytes)
            {
                throw new Exception("Space storages was found, but available space not enough");
            }
            
            if (bestStorage.FsrmQuotaSizeBytes - bestStorage.UsedSizeBytes < quotaSizeInBytes)
            {
                throw new Exception("Space storages was found, but available space not enough");
            }

            return bestStorage;
        }
    }
}
