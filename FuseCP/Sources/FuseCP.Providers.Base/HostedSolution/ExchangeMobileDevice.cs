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

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Providers.HostedSolution
{
	public class ExchangeMobileDevice
	{
		public MobileDeviceStatus Status { get; set; }
		public DateTime FirstSyncTime { get; set; }
		public DateTime LastPolicyUpdateTime { get; set; }
		public DateTime LastSyncAttemptTime { get; set; }
		public DateTime LastSuccessSync { get; set; }
		public string DeviceType { get; set; }
		public string DeviceID { get; set; }
		public string DeviceUserAgent { get; set; }
		public DateTime DeviceWipeSentTime { get; set; }
		public DateTime DeviceWipeRequestTime { get; set; }
		public DateTime DeviceWipeAckTime { get; set; }
		public int LastPingHeartbeat { get; set; }
		public string RecoveryPassword { get; set; }
		public string DeviceModel { get; set; }
		public string DeviceIMEI { get; set; }
		public string DeviceFriendlyName { get; set; }
		public string DeviceOS { get; set; }
		public string DeviceOSLanguage { get; set; }
		public string DevicePhoneNumber { get; set; }
		public string Id { get; set; }
	}
}
