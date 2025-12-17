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

namespace FuseCP.Providers
{
	/// <summary>
	/// Summary description for DailyStatistics.
	/// </summary>
	[Serializable]
	public class DailyStatistics
	{
		private int year;
		private int month;
		private int day;
		private long bytesSent;
		private long bytesReceived;

		public DailyStatistics()
		{
		}

		public int Year
		{
			get { return year; }
			set { year = value; }
		}

		public int Month
		{
			get { return month; }
			set { month = value; }
		}

		public int Day
		{
			get { return day; }
			set { day = value; }
		}

		public long BytesSent
		{
			get { return bytesSent; }
			set { bytesSent = value; }
		}

		public long BytesReceived
		{
			get { return bytesReceived; }
			set { bytesReceived = value; }
		}
	}
}
