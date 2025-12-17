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

namespace FuseCP.Providers.HostedSolution
{
	public enum CalendarProcessingFlags
    {
		None,
		AutoUpdate,
		AutoAccept
	}

	public class ExchangeResourceMailboxSettings
    {
        string displayName;
        string accountName;
		int resourceCapacity;
		CalendarProcessingFlags automateProcessing;
		int bookingWindowInDays;
		int maximumDurationInMinutes;
		bool allowRecurringMeetings;
		bool enforceSchedulingHorizon;
		bool scheduleOnlyDuringWorkHours;
		ExchangeAccount[] resourceDelegates;
		bool allBookInPolicy;
		bool allRequestInPolicy;
		bool addAdditionalResponse;
		string additionalResponse;

		public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; }
		}

		public string AccountName
		{
			get { return this.accountName; }
			set { this.accountName = value; }
		}

		public int ResourceCapacity
		{
			get { return this.resourceCapacity; }
			set { this.resourceCapacity = value; }
		}

		public CalendarProcessingFlags AutomateProcessing
		{
			get { return this.automateProcessing; }
			set { this.automateProcessing = value; }
		}

		public int BookingWindowInDays
		{
			get { return this.bookingWindowInDays; }
			set { this.bookingWindowInDays = value; }
		}

		public int MaximumDurationInMinutes
		{
			get { return this.maximumDurationInMinutes; }
			set { this.maximumDurationInMinutes = value; }
		}

		public bool AllowRecurringMeetings
		{
			get { return this.allowRecurringMeetings; }
			set { this.allowRecurringMeetings = value; }
		}

		public bool EnforceSchedulingHorizon
		{
			get { return this.enforceSchedulingHorizon; }
			set { this.enforceSchedulingHorizon = value; }
		}

		public bool ScheduleOnlyDuringWorkHours
		{
			get { return this.scheduleOnlyDuringWorkHours; }
			set { this.scheduleOnlyDuringWorkHours = value; }
		}

		public ExchangeAccount[] ResourceDelegates
		{
			get { return this.resourceDelegates; }
			set { this.resourceDelegates = value; }
		}

		public bool AllBookInPolicy
		{
			get { return this.allBookInPolicy; }
			set { this.allBookInPolicy = value; }
		}

		public bool AllRequestInPolicy
		{
			get { return this.allRequestInPolicy; }
			set { this.allRequestInPolicy = value; }
		}

		public bool AddAdditionalResponse
		{
			get { return this.addAdditionalResponse; }
			set { this.addAdditionalResponse = value; }
		}

		public string AdditionalResponse
		{
			get { return this.additionalResponse; }
			set { this.additionalResponse = value; }
		}
	}
}
