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
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Portal.Code.Framework;

namespace FuseCP.Portal.UserControls.ScheduleTaskView
{
	/// <summary>
	/// Represents empty view which states that no configuration is availalbe for this schedule task.
	/// </summary>
	public partial class EmptyView : UserControl, ISchedulerTaskView
	{
		private List<ScheduleTaskParameterInfo> input;

		/// <summary>
		/// Sets scheduler task parameters on view.
		/// </summary>
		/// <param name="parameters">Parameters list to be set on view.</param>
		public virtual void SetParameters(ScheduleTaskParameterInfo[] parameters)
		{
			input = new List<ScheduleTaskParameterInfo>(parameters);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public virtual ScheduleTaskParameterInfo[] GetParameters()
		{
			return new ScheduleTaskParameterInfo[0];
		}

		/// <summary>
		/// Searches for parameter by its id.
		/// </summary>
		/// <param name="id">Parameter id.</param>
		/// <returns>Found parameter.</returns>
		protected ScheduleTaskParameterInfo FindParameterById(string id)
		{
			return input.Find(delegate(ScheduleTaskParameterInfo param)
						{
							return param.ParameterId == id;
						}
				);
		}

		/// <summary>
		/// Sets parameter's value to textbox control's text property.
		/// </summary>
		/// <param name="control">Control to set value to.</param>
		/// <param name="parameterName">Parameter name.</param>
		protected void SetParameter(TextBox control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = this.FindParameterById(parameterName);
			control.Text = String.IsNullOrEmpty(parameter.ParameterValue)
											? parameter.DefaultValue
											: parameter.ParameterValue;

		}

		/// <summary>
		/// Sets parameter's value to drop down list control's list and selected value.
		/// </summary>
		/// <param name="control">Control to set value to.</param>
		/// <param name="parameterName">Parameter name.</param>
		protected void SetParameter(DropDownList control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = this.FindParameterById(parameterName);
			control.Items.Clear();
			Utils.ParseGroup(parameter.DefaultValue).ForEach(delegate(KeyValuePair<string, string> i) { control.Items.Add(new ListItem(i.Key, i.Value)); });
			Utils.SelectListItem(control, parameter.ParameterValue);
		}

		/// <summary>
		/// Sets parameter's value to checkbox control's checked value.
		/// </summary>
		/// <param name="control">Control to set value to.</param>
		/// <param name="parameterName">Parameter name.</param>
		protected void SetParameter(CheckBox control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = this.FindParameterById(parameterName);
			control.Checked = String.IsNullOrEmpty(parameter.ParameterValue)
											? Convert.ToBoolean(parameter.DefaultValue)
											: Convert.ToBoolean(parameter.ParameterValue);
		}

		/// <summary>
		/// Gets text parameter value from textbox control.
		/// </summary>
		/// <param name="control">Control to get value from.</param>
		/// <param name="parameterName">Paramter name.</param>
		/// <returns>Parameter.</returns>
		protected ScheduleTaskParameterInfo GetParameter(TextBox control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = new ScheduleTaskParameterInfo();
			parameter.ParameterId = parameterName;
			parameter.ParameterValue = (control.Text.Length > 1000) ? control.Text.Substring(0, 1000) : control.Text;
			return parameter;
		}

		/// <summary>
		/// Gets text parameter value from drop down list control.
		/// </summary>
		/// <param name="control">Control to get value from.</param>
		/// <param name="parameterName">Paramter name.</param>
		/// <returns>Parameter.</returns>
		protected ScheduleTaskParameterInfo GetParameter(DropDownList control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = new ScheduleTaskParameterInfo();
			parameter.ParameterId = parameterName;
			parameter.ParameterValue = control.SelectedValue;
			return parameter;
		}

		/// <summary>
		/// Gets checked parameter value from textbox control.
		/// </summary>
		/// <param name="control">Control to get value from.</param>
		/// <param name="parameterName">Paramter name.</param>
		/// <returns>Parameter.</returns>
		protected ScheduleTaskParameterInfo GetParameter(CheckBox control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = new ScheduleTaskParameterInfo();
			parameter.ParameterId = parameterName;
			parameter.ParameterValue = control.Checked.ToString();
			return parameter;
		}

	}
}
