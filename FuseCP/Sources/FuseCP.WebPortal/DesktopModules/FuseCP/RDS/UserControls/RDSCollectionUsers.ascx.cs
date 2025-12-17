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
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.RDS.UserControls
{
    public partial class RDSCollectionUsers : FuseCPControlBase
	{
        public const string DirectionString = "DirectionString";
        public event EventHandler OnRefreshClicked;
        private static OrganizationUser[] LocalAdmins = null;

        public bool ButtonAddEnabled
        {
            get
            {
                return btnAdd.Enabled;
            }
            set
            {
                btnAdd.Enabled = value;
            }
        }

		protected enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

        public void SetUsers(OrganizationUser[] users)
		{
            BindAccounts(users, false);
		}

        public OrganizationUser[] GetUsers()
		{
			return GetGridViewUsers(SelectedState.All).ToArray();
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
                Page.ClientScript.RegisterClientScriptBlock(typeof(RDSCollectionUsers), "SelectAllCheckboxes",
					script, true);
			}            
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
            // bind all accounts
            BindPopupAccounts(null, null);

			// show modal
			AddAccountsModal.Show();
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
            List<string> lockedUsers = new List<string>();

            if (PanelRequest.Ctl == "rds_collection_edit_users")
            {
                lockedUsers = CheckDeletedUsers();

                if (!lockedUsers.Any())
                {
                    List<OrganizationUser> selectedAccounts = GetGridViewUsers(SelectedState.Unselected);
                    BindAccounts(selectedAccounts.ToArray(), false);
                }                
            }
            else
            {
                List<OrganizationUser> selectedAccounts = GetGridViewUsers(SelectedState.Unselected);
                BindAccounts(selectedAccounts.ToArray(), false);
            }

            if (OnRefreshClicked != null)
            {
                OnRefreshClicked(lockedUsers, new EventArgs());
            }
		}

		protected void btnAddSelected_Click(object sender, EventArgs e)
		{
            List<OrganizationUser> selectedAccounts = GetGridViewAccounts();

            BindAccounts(selectedAccounts.ToArray(), true);

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
                default:
                    imgName = "admin_16.png";
                    break;
            }

            return GetThemedImage("Exchange/" + imgName);
        }

        public List<string> CheckDeletedUsers()
        {            
            var rdsUsers = GetGridViewUsers(SelectedState.Selected);
            var collectionUsers = ES.Services.RDS.GetRdsCollectionUsers(PanelRequest.CollectionID);

            if (rdsUsers.All(r => !collectionUsers.Select(c => c.AccountName.ToLower()).Contains(r.AccountName.ToLower())))
            {
                return new List<string>();
            }

            if (LocalAdmins == null) LocalAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);
            var organizationUsers = ES.Services.Organizations.GetOrganizationUsersPaged(PanelRequest.ItemID, null, null, null, 0, Int32.MaxValue).PageUsers;
            var applicationUsers = ES.Services.RDS.GetApplicationUsers(PanelRequest.ItemID, PanelRequest.CollectionID, null);
            var remoteAppUsers = organizationUsers.Where(x => applicationUsers.Select(a => a.Split('\\').Last().ToLower()).Contains(x.SamAccountName.Split('\\').Last().ToLower()));

            var deletedUsers = new List<OrganizationUser>();

            deletedUsers.AddRange(rdsUsers.Where(r => LocalAdmins.Select(l => l.AccountName.ToLower()).Contains(r.AccountName.ToLower())));
            remoteAppUsers = remoteAppUsers.Where(r => !LocalAdmins.Select(l => l.AccountName.ToLower()).Contains(r.AccountName.ToLower()));
            deletedUsers.AddRange(rdsUsers.Where(r => remoteAppUsers.Select(l => l.AccountName.ToLower()).Contains(r.AccountName.ToLower())));
            deletedUsers = deletedUsers.Distinct().ToList();            

            return deletedUsers.Select(d => d.DisplayName).ToList();
        }

        public void BindUsers()
        {
            var collectionUsers = ES.Services.RDS.GetRdsCollectionUsers(PanelRequest.CollectionID);
            LocalAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);

            foreach (var user in collectionUsers)
            {
                if (LocalAdmins.Select(l => l.AccountName).Contains(user.AccountName))
                {
                    user.IsVIP = true;
                }
                else
                {
                    user.IsVIP = false;
                }
            }
            
            SetUsers(collectionUsers);
        }

		protected void BindPopupAccounts(string filterColumn, string filterValue)
		{
            OrganizationUser[] accounts;

            if (PanelRequest.Ctl == "rds_collection_edit_users")
            {
                accounts = ES.Services.Organizations.GetOrganizationUsersPaged(PanelRequest.ItemID, filterColumn, filterValue, null, 0, Int32.MaxValue).PageUsers;
            }
            else
            {
                accounts = ES.Services.RDS.GetRdsCollectionUsers(PanelRequest.CollectionID);
            }

            if (LocalAdmins == null) LocalAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);

            foreach (var user in accounts)
            {
                if (LocalAdmins.Select(l => l.AccountName).Contains(user.AccountName))
                {
                    user.IsVIP = true;
                }
                else
                {
                    user.IsVIP = false;
                }
            }

            accounts = accounts.Where(x => !GetUsers().Select(p => p.AccountName).Contains(x.AccountName)).ToArray();
            Array.Sort(accounts, CompareAccount);            

            gvPopupAccounts.DataSource = accounts;
            gvPopupAccounts.DataBind();
		}

        protected void BindAccounts(OrganizationUser[] newUsers, bool preserveExisting)
		{
			// get binded addresses
            List<OrganizationUser> users = new List<OrganizationUser>();
			if(preserveExisting)
                users.AddRange(GetGridViewUsers(SelectedState.All));

			// add new accounts
            if (newUsers != null)
			{
                foreach (OrganizationUser newUser in newUsers)
				{
					// check if exists
					bool exists = false;
                    foreach (OrganizationUser user in users)
					{
                        if (String.Compare(user.AccountName, newUser.AccountName, true) == 0)
						{
							exists = true;
							break;
						}
					}

					if (exists)
						continue;

                    users.Add(newUser);
				}
			}

            gvUsers.DataSource = users.OrderBy(u => u.DisplayName);
            gvUsers.DataBind();
		}

        protected List<OrganizationUser> GetGridViewUsers(SelectedState state)
        {
            List<OrganizationUser> users = new List<OrganizationUser>();
            for (int i = 0; i < gvUsers.Rows.Count; i++)
            {
                GridViewRow row = gvUsers.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                OrganizationUser user = new OrganizationUser();
                user.AccountName = (string)gvUsers.DataKeys[i][0];
                user.DisplayName = ((Literal)row.FindControl("litAccount")).Text;
                user.PrimaryEmailAddress = ((Literal)row.FindControl("litEmail")).Text;
                user.SamAccountName = ((HiddenField)row.FindControl("hdnSamAccountName")).Value;
                user.IsVIP = Convert.ToBoolean(((HiddenField)row.FindControl("hdnIsVip")).Value);

                if (state == SelectedState.All ||
                    (state == SelectedState.Selected && chkSelect.Checked) ||
                    (state == SelectedState.Unselected && !chkSelect.Checked))
                    users.Add(user);
            }

            return users;
        }

        protected List<OrganizationUser> GetGridViewAccounts()
        {
            List<OrganizationUser> accounts = new List<OrganizationUser>();
            for (int i = 0; i < gvPopupAccounts.Rows.Count; i++)
            {
                GridViewRow row = gvPopupAccounts.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                if (chkSelect.Checked)
                {
                    accounts.Add(new OrganizationUser
                    {
                        AccountName = (string)gvPopupAccounts.DataKeys[i][0],
                        DisplayName = ((Literal)row.FindControl("litDisplayName")).Text,
                        PrimaryEmailAddress = ((Literal)row.FindControl("litPrimaryEmailAddress")).Text,
                        SamAccountName = ((HiddenField)row.FindControl("hdnSamName")).Value,
                        IsVIP = Convert.ToBoolean(((HiddenField)row.FindControl("hdnLocalAdmin")).Value)
                    });
                }
            }

            return accounts;

        }

		protected void cmdSearch_Click(object sender, EventArgs e)
		{
			BindPopupAccounts(ddlSearchColumn.Text, txtSearchValue.Text);
		}

        protected SortDirection Direction
        {
            get { return ViewState[DirectionString] == null ? SortDirection.Descending : (SortDirection)ViewState[DirectionString]; }
            set { ViewState[DirectionString] = value; }
        }

        protected static int CompareAccount(OrganizationUser user1, OrganizationUser user2)
        {
            return string.Compare(user1.DisplayName, user2.DisplayName);
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetupInstructions")
            {
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_setup_letter", "CollectionID=" + PanelRequest.CollectionID, "ItemID=" + PanelRequest.ItemID, "AccountID=" + e.CommandArgument.ToString()));
            }
        }
	}
}
