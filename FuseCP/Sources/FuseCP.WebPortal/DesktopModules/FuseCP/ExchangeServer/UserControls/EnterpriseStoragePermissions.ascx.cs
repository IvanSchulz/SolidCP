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
using FuseCP.Providers.HostedSolution;
using System.Linq;
using FuseCP.Providers.Web;
using FuseCP.EnterpriseServer.Base.HostedSolution;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class EnterpriseStoragePermissions : FuseCPControlBase
	{
        public const string DirectionString = "DirectionString";

		protected enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

        public void SetPermissions(ESPermission[] permissions)
		{
			BindAccounts(permissions, false);
		}

		public ESPermission[] GetPemissions()
		{
			return GetGridViewPermissions(SelectedState.All).ToArray();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// register javascript
			if (!Page.ClientScript.IsClientScriptBlockRegistered("SelectAllCheckboxes"))
			{
				string script = @"    function SelectAllCheckboxes(box)
                {
		            var state = box.checked;
                    var elm = box.parentElement.parentElement.parentElement.parentElement.getElementsByTagName(""INPUT"");
                    for(i = 0; i < elm.length; i++)
                        if(elm[i].type == ""checkbox"" && elm[i].id != box.id && elm[i].checked != state && !elm[i].disabled)
		                    elm[i].checked = state;
                }";
                Page.ClientScript.RegisterClientScriptBlock(typeof(EnterpriseStoragePermissions), "SelectAllCheckboxes",
					script, true);
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			// bind all accounts
			BindPopupAccounts();

			// show modal
			AddAccountsModal.Show();
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
            List<ESPermission> selectedAccounts = GetGridViewPermissions(SelectedState.Unselected);

			BindAccounts(selectedAccounts.ToArray(), false);
		}

		protected void btnAddSelected_Click(object sender, EventArgs e)
		{
            List<ExchangeAccount> selectedAccounts = GetGridViewAccounts();

            List<ESPermission> permissions = new List<ESPermission>();
            foreach (ExchangeAccount account in selectedAccounts)
            {
                permissions.Add(new ESPermission
                {
                    Account = account.AccountName,
                    DisplayName = account.DisplayName,
                    Access = "Read-Only",
                });
            }

            BindAccounts(permissions.ToArray(), true);

		}

        public string GetAccountImage(int accountTypeId)
        {
            string imgName = string.Empty;

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
            switch (accountType)
            {
                case ExchangeAccountType.Room:
                    imgName = "room_16.gif";
                    break;
                case ExchangeAccountType.Equipment:
                    imgName = "equipment_16.gif";
                    break;
                case ExchangeAccountType.SecurityGroup:
                    imgName = "dlist_16.gif";
                    break;
                case ExchangeAccountType.DefaultSecurityGroup:
                    imgName = "dlist_16.gif";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }

            return GetThemedImage("Exchange/" + imgName);
        }

		protected void BindPopupAccounts()
		{
			ExchangeAccount[] accounts = ES.Services.EnterpriseStorage.SearchESAccounts(PanelRequest.ItemID,
				ddlSearchColumn.SelectedValue, "%" + txtSearchValue.Text + "%", "");

            accounts = accounts.Where(x => !GetPemissions().Select(p => p.Account).Contains(x.AccountName)).ToArray();
            Array.Sort(accounts, CompareAccount);
            if (Direction == SortDirection.Ascending)
            {
                Array.Reverse(accounts);
                Direction = SortDirection.Descending;
            }
            else
                Direction = SortDirection.Ascending;

            gvPopupAccounts.DataSource = accounts;
            gvPopupAccounts.DataBind();
		}

        protected void BindAccounts(ESPermission[] newPermissions, bool preserveExisting)
		{
			// get binded addresses
            List<ESPermission> permissions = new List<ESPermission>();
			if(preserveExisting)
                permissions.AddRange(GetGridViewPermissions(SelectedState.All));

			// add new accounts
            if (newPermissions != null)
			{
                foreach (ESPermission newPermission in newPermissions)
				{
					// check if exists
					bool exists = false;
                    foreach (ESPermission permission in permissions)
					{
						if (String.Compare(newPermission.Account, permission.Account, true) == 0)
						{
							exists = true;
							break;
						}
					}

					if (exists)
						continue;

                    permissions.Add(newPermission);
				}
			}

            gvPermissions.DataSource = permissions;
            gvPermissions.DataBind();
		}

        protected List<ESPermission> GetGridViewPermissions(SelectedState state)
        {
            List<ESPermission> permissions = new List<ESPermission>();
            for (int i = 0; i < gvPermissions.Rows.Count; i++)
            {
                GridViewRow row = gvPermissions.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                ESPermission permission = new ESPermission();
                permission.Account = (string)gvPermissions.DataKeys[i][0];
                permission.Access = ((Literal)row.FindControl("litAccess")).Text;
                permission.DisplayName = ((Literal)row.FindControl("litAccount")).Text;

                if (state == SelectedState.All ||
                    (state == SelectedState.Selected && chkSelect.Checked) ||
                    (state == SelectedState.Unselected && !chkSelect.Checked))
                    permissions.Add(permission);
            }
            
            return permissions;
        }

        protected List<ExchangeAccount> GetGridViewAccounts()
        {
            List<ExchangeAccount> accounts = new List<ExchangeAccount>();
            for (int i = 0; i < gvPopupAccounts.Rows.Count; i++)
            {
                GridViewRow row = gvPopupAccounts.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                if (chkSelect.Checked)
                {
                    accounts.Add(new ExchangeAccount
                    {
                        AccountName = (string)gvPopupAccounts.DataKeys[i][0],
                        DisplayName = ((Literal)row.FindControl("litDisplayName")).Text
                    });
                }
            }

            return accounts;

        }

		protected void cmdSearch_Click(object sender, EventArgs e)
		{
			BindPopupAccounts();
		}

        protected SortDirection Direction
        {
            get { return ViewState[DirectionString] == null ? SortDirection.Descending : (SortDirection)ViewState[DirectionString]; }
            set { ViewState[DirectionString] = value; }
        }

        protected static int CompareAccount(ExchangeAccount user1, ExchangeAccount user2)
        {
            return string.Compare(user1.DisplayName, user2.DisplayName);
        }

        protected void btn_UpdateAccess(object sender, EventArgs e)
        {
            if (gvPermissions.HeaderRow != null)
            {
                CheckBox chkAllSelect = (CheckBox)gvPermissions.HeaderRow.FindControl("chkSelectAll");
                if (chkAllSelect != null)
                {
                    chkAllSelect.Checked = false;
                }
            }

            for (int i = 0; i < gvPermissions.Rows.Count; i++)
            {
                GridViewRow row = gvPermissions.Rows[i];

                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                Literal litAccess = (Literal)row.FindControl("litAccess");

                if (chkSelect == null || litAccess == null)
                    continue;

                if (chkSelect.Checked)
                {
                    chkSelect.Checked = false;
                    litAccess.Text = ((Button)sender).CommandArgument;
                }
            }
        }
	}
}
