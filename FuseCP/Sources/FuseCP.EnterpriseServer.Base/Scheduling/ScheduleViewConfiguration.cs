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

namespace FuseCP.EnterpriseServer.Base.Scheduling
{
	/// <summary>
	/// Represents view configuration for a certain hosting environment.
	/// </summary>
	[Serializable]
	public class ScheduleTaskViewConfiguration
	{
		private string environment;
		private string description;
		private string taskId;
		private string configurationId;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduleTaskViewConfiguration"/> class.
		/// </summary>
		public ScheduleTaskViewConfiguration()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduleTaskViewConfiguration"/> class.
		/// </summary>
		/// <param name="environment">Hosting environment for the view.</param>
		/// <param name="description">Configuration details for the view.</param>
		public ScheduleTaskViewConfiguration(string environment, string description)
		{
			this.environment = environment;
			this.description = description;
		}

		/// <summary>
		/// Gets or sets owner task id.
		/// </summary>
		public string TaskId
		{
			get
			{
				return this.taskId;
			}
			set
			{
				this.taskId = value;
			}
		}

		/// <summary>
		/// Gets or sets configuration's id.
		/// </summary>
		public string ConfigurationId
		{
			get
			{
				return this.configurationId;
			}
			set
			{
				this.configurationId = value;
			}
		}

		/// <summary>
		/// Gets or sets hosting environment for the view.
		/// </summary>
		public string Environment
		{
			get
			{
				return this.environment;
			}
			set
			{
				this.environment = value;
			}
		}

		/// <summary>
		/// Gets or sets configuration details for the view.
		/// </summary>
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}
	}
}
