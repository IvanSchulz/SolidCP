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
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
	public partial class AccountsList : FuseCPControlBase
	{
        private const string DirectionString = "DirectionString";
        private enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

        public bool Disabled
        {
            get { return ViewState["Disabled"] != null ? (bool)ViewState["Disabled"] : false; }
            set { ViewState["Disabled"] = value; }
        }

	    public bool EnableMailboxOnly
	    {
            get {return ViewState["EnableMailboxOnly"] != null ? (bool)ViewState["EnableMailboxOnly"]: false; }
            set { ViewState["EnableMailboxOnly"] = value; }
	    }
        
        public bool MailboxesEnabled
		{
			get { return ViewState["MailboxesEnabled"] != null ? (bool)ViewState["MailboxesEnabled"] : false; }
			set { ViewState["MailboxesEnabled"] = value; }
		}

		public bool ContactsEnabled
		{
			get { return ViewState["ContactsEnabled"] != null ? (bool)ViewState["ContactsEnabled"] : false; }
			set { ViewState["ContactsEnabled"] = value; }
		}

		public bool DistributionListsEnabled
		{
			get { return ViewState["DistributionListsEnabled"] != null ? (bool)ViewState["DistributionListsEnabled"] : false; }
			set { ViewState["DistributionListsEnabled"] = value; }
		}

        public bool SecurityGroupsEnabled
        {
            get { return ViewState["SecurityGroupsEnabled"] != null ? (bool)ViewState["SecurityGroupsEnabled"] : false; }
            set { ViewState["SecurityGroupsEnabled"] = value; }
        }

        public bool SharedMailboxEnabled
        {
            get { return ViewState["SharedMailboxEnabled"] != null ? (bool)ViewState["SharedMailboxEnabled"] : false; }
            set { ViewState["SharedMailboxEnabled"] = value; }
        }

		public int ExcludeAccountId
		{
			get { return PanelRequest.AccountID; }
		}

		public void SetAccounts(ExchangeAccount[] accounts)
		{
			BindAccounts(accounts, false);
		}

        public string[] GetAccounts()
        {
            // get selected accounts
            List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvAccounts, SelectedState.All);

            List<string> accountNames = new List<string>();
            foreach (ExchangeAccount account in selectedAccounts)
                accountNames.Add(account.AccountName);

            return accountNames.ToArray();
        }

        public IDictionary<string, ExchangeAccountType> GetFullAccounts()
		{
			// get selected accounts
			List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvAccounts, SelectedState.All);

            IDictionary<string, ExchangeAccountType> accounts = new Dictionary<string, ExchangeAccountType>();

            foreach (ExchangeAccount account in selectedAccounts)
            {
                accounts.Add(account.AccountName, account.AccountType);
            }

            return accounts;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// toggle controls
			if (!IsPostBack)
			{
                if (Disabled)
                {
                    btnAdd.Visible = btnDelete.Visible = gvAccounts.Columns[0].Visible = false;
                }

				chkIncludeMailboxes.Visible = chkIncludeRooms.Visible = chkIncludeEquipment.Visible = chkIncludeSharedMailbox.Visible = MailboxesEnabled;
                chkIncludeMailboxes.Checked = chkIncludeRooms.Checked = chkIncludeEquipment.Checked = chkIncludeSharedMailbox.Checked = MailboxesEnabled;
				
                if (EnableMailboxOnly)
				{
				    chkIncludeRooms.Checked = false;
				    chkIncludeRooms.Visible = false;
				    chkIncludeEquipment.Checked = false;
				    chkIncludeEquipment.Visible = false;
                    chkIncludeSharedMailbox.Checked = false;
                    chkIncludeSharedMailbox.Visible = false;
                }
                
                chkIncludeContacts.Visible = ContactsEnabled;
				chkIncludeContacts.Checked = ContactsEnabled;
				chkIncludeLists.Visible = DistributionListsEnabled;
				chkIncludeLists.Checked = DistributionListsEnabled;

                chkIncludeGroups.Visible = SecurityGroupsEnabled;
                chkIncludeGroups.Checked = SecurityGroupsEnabled;

                chkIncludeSharedMailbox.Visible = SharedMailboxEnabled;
                chkIncludeSharedMailbox.Checked = SharedMailboxEnabled;

                gvAccounts.Columns[3].Visible = gvPopupAccounts.Columns[3].Visible = SecurityGroupsEnabled;
			}

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
				Page.ClientScript.RegisterClientScriptBlock(typeof(AccountsList), "SelectAllCheckboxes",
					script, true);
			}
		}

		public string GetAccountImage(int accountTypeId)
		{
			ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
			string imgName = "mailbox_16.gif";
            if (accountType == ExchangeAccountType.Contact)
                imgName = "contact_16.gif";
            else if (accountType == ExchangeAccountType.DistributionList
                    || accountType == ExchangeAccountType.SecurityGroup
                    || accountType == ExchangeAccountType.DefaultSecurityGroup)
                imgName = "dlist_16.gif";
            else if (accountType == ExchangeAccountType.Room)
                imgName = "room_16.gif";
            else if (accountType == ExchangeAccountType.Equipment)
                imgName = "equipment_16.gif";
            else if (accountType == ExchangeAccountType.SharedMailbox)
                imgName = "shared_16.gif";

            return GetThemedImage("Exchange/" + imgName);
		}

        public string GetType(int accountTypeId)
        {
            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;

            switch(accountType)
            {
                case ExchangeAccountType.DistributionList:
                    return "Distribution";
                case ExchangeAccountType.SecurityGroup:
                    return "Security";
                case ExchangeAccountType.DefaultSecurityGroup:
                    return "Default";
                default:
                    return string.Empty;
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
			// get selected accounts
			List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvAccounts, SelectedState.Unselected);

			// add to the main list
			BindAccounts(selectedAccounts.ToArray(), false);
		}

		protected void btnAddSelected_Click(object sender, EventArgs e)
		{
			// get selected accounts
			List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvPopupAccounts, SelectedState.Selected);
			
			// add to the main list
			BindAccounts(selectedAccounts.ToArray(), true);
		}

        private SortDirection Direction
        {
            get { return ViewState[DirectionString] == null ? SortDirection.Descending : (SortDirection)ViewState[DirectionString]; }
            set { ViewState[DirectionString] = value; }
        }

        private static int CompareAccount(ExchangeAccount user1, ExchangeAccount user2)
        {
            return string.Compare(user1.DisplayName, user2.DisplayName);
        }

        private void BindPopupAccounts()
		{
            List<ExchangeAccountType> types = new List<ExchangeAccountType>();

            if (chkIncludeMailboxes.Checked) types.Add(ExchangeAccountType.Mailbox);
            if (chkIncludeContacts.Checked) types.Add(ExchangeAccountType.Contact);
            if (chkIncludeLists.Checked) types.Add(ExchangeAccountType.DistributionList);
            if (chkIncludeRooms.Checked) types.Add(ExchangeAccountType.Room);
            if (chkIncludeEquipment.Checked) types.Add(ExchangeAccountType.Equipment);
            if (chkIncludeGroups.Checked) types.Add(ExchangeAccountType.SecurityGroup);
            if (chkIncludeSharedMailbox.Checked) types.Add(ExchangeAccountType.SharedMailbox);

            ExchangeAccount[] accounts = ES.Services.ExchangeServer.SearchAccountsByTypes(PanelRequest.ItemID,
                types.ToArray(),
                ddlSearchColumn.SelectedValue, txtSearchValue.Text + "%", "");

            if (SecurityGroupsEnabled)
            {
                accounts = accounts.Where(x => !GetAccounts().Contains(x.AccountName)).ToArray();
            }

			if (ExcludeAccountId > 0)
			{
				List<ExchangeAccount> updatedAccounts = new List<ExchangeAccount>();
				foreach (ExchangeAccount account in accounts)
					if (account.AccountId != ExcludeAccountId)
						updatedAccounts.Add(account);

				accounts = updatedAccounts.ToArray();
			}

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

            if (gvPopupAccounts.Rows.Count > 0)
            {
                UpdateGridViewAccounts(gvPopupAccounts);
            }
		}

		private void BindAccounts(ExchangeAccount[] newAccounts, bool preserveExisting)
		{
			// get binded addresses
			List<ExchangeAccount> accounts = new List<ExchangeAccount>();
			if(preserveExisting)
				accounts.AddRange(GetGridViewAccounts(gvAccounts, SelectedState.All));

			// add new accounts
			if (newAccounts != null)
			{
				foreach (ExchangeAccount newAccount in newAccounts)
				{
					// check if exists
					bool exists = false;
					foreach (ExchangeAccount account in accounts)
					{
						if (String.Compare(newAccount.AccountName, account.AccountName, true) == 0)
						{
							exists = true;
							break;
						}
					}

					if (exists)
						continue;

					accounts.Add(newAccount);
				}
			}

			gvAccounts.DataSource = accounts;
			gvAccounts.DataBind();

            if (gvAccounts.Rows.Count > 0)
            {
                UpdateGridViewAccounts(gvAccounts);
            }

            btnDelete.Visible = gvAccounts.Rows.Count > 0;
		}

		private List<ExchangeAccount> GetGridViewAccounts(GridView gv, SelectedState state)
		{
			List<ExchangeAccount> accounts = new List<ExchangeAccount>();
			for (int i = 0; i < gv.Rows.Count; i++)
			{
				GridViewRow row = gv.Rows[i];
				CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
				if (chkSelect == null)
					continue;

				ExchangeAccount account = new ExchangeAccount();
				account.AccountType = (ExchangeAccountType)Enum.Parse(typeof(ExchangeAccountType), ((Literal)row.FindControl("litAccountType")).Text);
				account.AccountName = (string)gv.DataKeys[i][0];
				account.DisplayName = ((Literal)row.FindControl("litDisplayName")).Text;
				account.PrimaryEmailAddress = ((Literal)row.FindControl("litPrimaryEmailAddress")).Text;

				if(state == SelectedState.All || 
					(state == SelectedState.Selected && chkSelect.Checked) ||
					(state == SelectedState.Unselected && !chkSelect.Checked))
					accounts.Add(account);
			}
			return accounts;
		}

        private void UpdateGridViewAccounts(GridView gv)
        {
            CheckBox chkSelectAll = (CheckBox)gv.HeaderRow.FindControl("chkSelectAll");

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                {
                    continue;
                }

                ExchangeAccountType exAccountType = (ExchangeAccountType)Enum.Parse(typeof(ExchangeAccountType), ((Literal)row.FindControl("litAccountType")).Text);

                if (exAccountType != ExchangeAccountType.DefaultSecurityGroup)
                {
                    chkSelectAll = null;
                    chkSelect.Enabled = true;
                }
                else
                {
                    chkSelect.Enabled = false;
                }
            }

            if (chkSelectAll != null)
            {
                chkSelectAll.Enabled = false;
            }
        }

		protected void chkIncludeMailboxes_CheckedChanged(object sender, EventArgs e)
		{
			BindPopupAccounts();
		}

		protected void cmdSearch_Click(object sender, EventArgs e)
		{
			BindPopupAccounts();
		}

        protected void OnSorting(object sender, GridViewSortEventArgs e)
        {
            BindPopupAccounts();
        }
    }
}
