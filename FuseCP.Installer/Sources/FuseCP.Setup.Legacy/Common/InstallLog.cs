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

namespace FuseCP.Setup
{
	public sealed class InstallLog
	{
		private InstallLog()
		{
		}

		private static StringBuilder sb;
		
		static InstallLog()
		{
			sb = new StringBuilder("Setup has:");
			sb.AppendLine();
		}

		public static void Append(string value)
		{
			sb.Append(value);
		}

		public static void AppendLine(string value)
		{
			sb.AppendLine(value);
		}

		public static void AppendLine(string format, params object[] args)
		{
			sb.AppendLine(String.Format(format, args));
		}

		public static void AppendLine()
		{
			sb.AppendLine();
		}

		public static void AppendFormat(string format, params object[] args)
		{
			sb.AppendFormat(format, args);
		}

		public static new string ToString()
		{
			return sb.ToString();
		}
	}
}
