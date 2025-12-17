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

namespace FuseCP.Providers.Virtualization
{
    [Persistent]
    public class VmReplication
    {
        [Persistent]
        public string Thumbprint { get; set; }

        [Persistent]
        public string[] VhdToReplicate { get; set; }

        [Persistent]
        public ReplicaFrequency ReplicaFrequency { get; set; }

        [Persistent]
        public int AdditionalRecoveryPoints { get; set; }

        [Persistent]
        public int VSSSnapshotFrequencyHour { get; set; }
    }
}
