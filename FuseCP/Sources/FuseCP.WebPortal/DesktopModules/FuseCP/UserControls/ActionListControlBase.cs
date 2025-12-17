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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;


namespace FuseCP.Portal.UserControls
{
    public abstract class ActionListControlBase<TEnum> : FuseCPControlBase 
    {
        public event CancelEventHandler ExecutingAction;

        public event EventHandler ExecutedAction;

        #region Properties

        public string GridViewID { get; set; }

        public string CheckboxesName { get; set; }

        private string _message = "ACTIONS_MESSAGE";
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        #endregion

        protected abstract DropDownList ActionsList { get; }

        protected abstract int DoAction(List<int> ids);

        public TEnum SelectedAction
        {
            get
            {
                return (TEnum)(object)Convert.ToInt32(ActionsList.SelectedValue);
            }
        }

        protected GridView GridView
        {
            get { return Parent.FindControl(GridViewID) as GridView; }
        }

        public void ResetSelection()
        {
            ActionsList.ClearSelection();
        }
        public void RemoveActionItem<TNum>(TNum value)
        {
            ActionsList.Items.Remove(ActionsList.Items.FindByValue(((int)(object)value).ToString()));
        }

        protected void FireExecuteAction()
        {
            if (ExecutingAction != null)
            {
                var e = new CancelEventArgs();
                ExecutingAction(this, e);

                if (e.Cancel) 
                    return;
            }

            DoAction();

            if (ExecutedAction != null)
                ExecutedAction(this, new EventArgs());
        }

        protected void DoAction()
        {
            if (GridView == null || String.IsNullOrWhiteSpace(CheckboxesName))
                return;

            // Get checked users
            var ids = Utils.GetCheckboxValuesFromGrid<int>(GridView, CheckboxesName);

            if ((int)(object)SelectedAction != 0)
            {
                if (ids.Count > 0)
                {
                    try
                    {
                        var result = DoAction(ids);

                        if (result < 0)
                        {
                            HostModule.ShowResultMessage(result);
                            return;
                        }

                        HostModule.ShowSuccessMessage(Message);
                    }
                    catch (Exception ex)
                    {
                        HostModule.ShowErrorMessage(Message, ex);
                    }

                    // Refresh users grid
                    GridView.DataBind();
                }
                else
                {
                    HostModule.ShowWarningMessage(Message);
                }
            }
        }

    }
}
