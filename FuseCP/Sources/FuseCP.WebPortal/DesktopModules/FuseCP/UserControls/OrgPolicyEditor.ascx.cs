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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer.Base.HostedSolution;

namespace FuseCP.Portal.UserControls
{
    public partial class OrgPolicyEditor : UserControl
    {
        #region Properties

        public string Value
        {
            get
            {
                return chkEnablePolicy.Checked.ToString();
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    chkEnablePolicy.Checked = false;
                }
                else
                {
                    try
                    {
                        chkEnablePolicy.Checked = Utils.ParseBool(value, true);
                    }
                    catch {}
                }

                ToggleControls();
            }
        }

        #endregion

        #region Methods

        public void SetAdditionalGroups(AdditionalGroup[] additionalGroups)
        {
            BindAdditionalGroups(additionalGroups);
        }

        public List<AdditionalGroup> GetGridViewGroups()
        {
            List<AdditionalGroup> additionalGroups = new List<AdditionalGroup>();
            for (int i = 0; i < gvAdditionalGroups.Rows.Count; i++)
            {
                GridViewRow row = gvAdditionalGroups.Rows[i];
                ImageButton cmdEdit = (ImageButton)row.FindControl("cmdEdit");
                if (cmdEdit == null)
                    continue;

                AdditionalGroup group = new AdditionalGroup();
                group.GroupId = (int)gvAdditionalGroups.DataKeys[i][0];
                group.GroupName = ((Literal)row.FindControl("litDisplayAdditionalGroup")).Text;

                additionalGroups.Add(group);
            }

            return additionalGroups;
        }

        protected void ToggleControls()
        {
            PolicyBlock.Visible = chkEnablePolicy.Checked;
        }

        protected void BindAdditionalGroups(AdditionalGroup[] additionalGroups)
        {
            gvAdditionalGroups.DataSource = additionalGroups;
            gvAdditionalGroups.DataBind();
        }

        protected int GetRowIndexByDataKey(int dataKey)
        {
            int index = 0;
            foreach (DataKey key in gvAdditionalGroups.DataKeys)
            {
                if (Utils.ParseInt(key.Value, 0) == dataKey)
                    break;

                index++;
            }

            return index >= gvAdditionalGroups.DataKeys.Count ? -1 : index;
        }

        #endregion

        #region Event Handlers

        protected void chkEnablePolicy_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void btnAddAdditionalGroup_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            List<AdditionalGroup> additionalGroups = GetGridViewGroups();

            AdditionalGroup additionalGroup = new AdditionalGroup();

            additionalGroup.GroupId = additionalGroups.Count != 0
                ? additionalGroups.Select(x => x.GroupId).Max() + 1
                : 1;

            additionalGroup.GroupName = txtAdditionalGroup.Text;

            additionalGroups.Add(additionalGroup);

            BindAdditionalGroups(additionalGroups.ToArray());

            txtAdditionalGroup.Text = string.Empty;
        }

        protected void btnUpdateAdditionalGroup_Click(object sender, EventArgs e)
        {
            if (ViewState["AdditionalGroupID"] == null || !Page.IsValid)
                return;

            List<AdditionalGroup> additionalGroups = GetGridViewGroups();

            additionalGroups
                .Where(x => x.GroupId == (int)ViewState["AdditionalGroupID"])
                .First().GroupName = txtAdditionalGroup.Text;

            BindAdditionalGroups(additionalGroups.ToArray());

            txtAdditionalGroup.Text = string.Empty;
        }

        protected void gvAdditionalGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int additionalGroupId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

            List<AdditionalGroup> additionalGroups = GetGridViewGroups();

            int rowIndex = GetRowIndexByDataKey(additionalGroupId);

            if (rowIndex != -1)
            {
                switch (e.CommandName)
                {
                    case "DeleteItem":
                        BindAdditionalGroups(
                            additionalGroups
                                .Where(x => x.GroupId != additionalGroupId).ToArray());
                        
                        break;
                    case "EditItem":
                        ViewState["AdditionalGroupID"] = additionalGroupId;

                        txtAdditionalGroup.Text = additionalGroups
                            .Where(x => x.GroupId == additionalGroupId)
                            .Select(y => y.GroupName).First();

                        break;
                }
            }
        }

        protected void DuplicateName_Validation(object source, ServerValidateEventArgs arguments)
        {
            List<AdditionalGroup> additionalGroups = GetGridViewGroups();

            arguments.IsValid = (additionalGroups.Where(x => x.GroupName.Trim() == arguments.Value).Count() == 0);
        }

        #endregion
    }
}
