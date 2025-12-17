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

namespace FuseCP.Providers.Virtualization
{
    public class ReplicationDetailInfo
    {
        public VmReplicationMode Mode { get; set; }
        public ReplicationState State { get; set; }
        public ReplicationHealth Health { get; set; }
        public string HealthDetails { get; set; }
        public string PrimaryServerName { get; set; }
        public string ReplicaServerName { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string AverageSize { get; set; }
        public string MaximumSize { get; set; }
        public TimeSpan AverageLatency { get; set; }
        public int Errors { get; set; }
        public int SuccessfulReplications { get; set; }
        public int MissedReplicationCount { get; set; }
        public string PendingSize { get; set; }
        public DateTime LastSynhronizedAt { get; set; }
    }
}
