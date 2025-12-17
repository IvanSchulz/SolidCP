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

namespace FuseCP.Providers.HostedSolution
{
	public class ExchangeActiveSyncPolicy
	{
		//general 
		private bool allowNonProvisionableDevices;
		private bool attachmentsEnabled;
		private int maxAttachmentSizeKB;
		private bool uncAccessEnabled;
		private bool wssAccessEnabled;
		//password
		private bool devicePasswordEnabled;
		private bool alphanumericPasswordRequired;
		private bool passwordRecoveryEnabled;
		private bool deviceEncryptionEnabled;
		private bool allowSimplePassword;
		private int maxPasswordFailedAttempts;
		private int minPasswordLength;
		private int inactivityLockMin;
		private int passwordExpirationDays;
		private int passwordHistory;
		private int refreshInterval;


		public int RefreshInterval
		{
			get
			{
				return refreshInterval;
			}
			set
			{
				refreshInterval = value;
			}
		}
		
		public bool AllowNonProvisionableDevices
		{
			get { return allowNonProvisionableDevices; }
			set { allowNonProvisionableDevices = value; }
		}
	
		public bool AttachmentsEnabled
		{
			get { return attachmentsEnabled; }
			set { attachmentsEnabled = value; }
		}

		
		public int MaxAttachmentSizeKB
		{
			get { return maxAttachmentSizeKB; }
			set { maxAttachmentSizeKB = value; }
		}

		
		public bool UNCAccessEnabled
		{
			get { return uncAccessEnabled; }
			set { uncAccessEnabled = value; }
		}

		
		public bool WSSAccessEnabled
		{
			get { return wssAccessEnabled; }
			set { wssAccessEnabled = value; }
		}

		public bool DevicePasswordEnabled
		{
			get { return devicePasswordEnabled; }
			set { devicePasswordEnabled = value; }
		}

		public bool AlphanumericPasswordRequired
		{
			get { return alphanumericPasswordRequired; }
			set { alphanumericPasswordRequired = value; }
		}

		public bool PasswordRecoveryEnabled
		{
			get { return passwordRecoveryEnabled; }
			set { passwordRecoveryEnabled = value; }
		}

		public bool DeviceEncryptionEnabled
		{
			get { return deviceEncryptionEnabled; }
			set { deviceEncryptionEnabled = value; }
		}

		public bool AllowSimplePassword
		{
			get { return allowSimplePassword; }
			set { allowSimplePassword = value; }
		}

		public int MaxPasswordFailedAttempts
		{
			get { return maxPasswordFailedAttempts; }
			set { maxPasswordFailedAttempts = value; }
		}

		public int MinPasswordLength
		{
			get { return minPasswordLength; }
			set { minPasswordLength = value; }
		}

		public int InactivityLockMin
		{
			get { return inactivityLockMin; }
			set { inactivityLockMin = value; }
		}

		public int PasswordExpirationDays
		{
			get { return passwordExpirationDays; }
			set { passwordExpirationDays = value; }
		}

		public int PasswordHistory
		{
			get { return passwordHistory; }
			set { passwordHistory = value; }
		}
	}
}
