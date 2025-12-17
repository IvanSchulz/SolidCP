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

namespace FuseCP.Providers
{
    public class ServiceProviderItemType
    {
        private int itemTypeId;
        private int groupId;
        private string displayName;
        private string typeName;
        private int typeOrder;
        private bool calculateDiskspace;
        private bool calculateBandwidth;
        private bool suspendable;
        private bool disposable;
        private bool searchable;
        private bool importable;
        private bool backupable;

        public int ItemTypeId
        {
            get { return itemTypeId; }
            set { itemTypeId = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public int TypeOrder
        {
            get { return typeOrder; }
            set { typeOrder = value; }
        }

        public bool CalculateDiskspace
        {
            get { return calculateDiskspace; }
            set { calculateDiskspace = value; }
        }

        public bool CalculateBandwidth
        {
            get { return calculateBandwidth; }
            set { calculateBandwidth = value; }
        }

        public bool Suspendable
        {
            get { return suspendable; }
            set { suspendable = value; }
        }

        public bool Disposable
        {
            get { return disposable; }
            set { disposable = value; }
        }

        public bool Searchable
        {
            get { return searchable; }
            set { searchable = value; }
        }

        public bool Importable
        {
            get { return importable; }
            set { importable = value; }
        }

        public bool Backupable
        {
            get { return backupable; }
            set { backupable = value; }
        }
    }
}
