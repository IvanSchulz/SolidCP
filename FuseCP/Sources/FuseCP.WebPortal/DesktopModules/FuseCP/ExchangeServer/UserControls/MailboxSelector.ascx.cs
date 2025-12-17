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
using System.Text.RegularExpressions;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class MailboxSelector : FuseCPControlBase
    {
        public const string DirectionString = "DirectionString";

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

        public bool ShowOnlyMailboxes
        {
            get { return ViewState["ShowOnlyMailboxes"] != null ? (bool)ViewState["ShowOnlyMailboxes"] : false; }
            set { ViewState["ShowOnlyMailboxes"] = value; }
        }

        public int ExcludeAccountId
        {
            get { return PanelRequest.AccountID; }
        }

        public void SetAccount(ExchangeAccount account)
        {
            BindSelectedAccount(account);
        }

        public string GetAccount()
        {
            return (string)ViewState["AccountName"];
        }
        public int GetAccountId()
        {
            return Utils.ParseInt(ViewState["AccountId"], 0);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // toggle controls
            if (!IsPostBack)
            {
                chkIncludeMailboxes.Visible = MailboxesEnabled;

                chkIncludeRooms.Visible = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeEquipment.Visible = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeSharedMailbox.Visible = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeMailboxes.Checked = MailboxesEnabled;

                chkIncludeRooms.Checked = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeEquipment.Checked = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeSharedMailbox.Checked = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeContacts.Visible = ContactsEnabled;
                chkIncludeContacts.Checked = ContactsEnabled;
                chkIncludeLists.Visible = DistributionListsEnabled;
                chkIncludeLists.Checked = DistributionListsEnabled;
            }

            // increase timeout
            ScriptManager scriptMngr = ScriptManager.GetCurrent(this.Page);
            scriptMngr.AsyncPostBackTimeout = 300;
        }

        private void BindSelectedAccount(ExchangeAccount account)
        {
            if (account != null)
            {
                txtDisplayName.Text = account.DisplayName;
                ViewState["AccountName"] = account.AccountName;
                ViewState["PrimaryEmailAddress"] = account.PrimaryEmailAddress;
                ViewState["AccountId"] = account.AccountId;
            }
            else
            {
                txtDisplayName.Text = "";
                ViewState["AccountName"] = null;
                ViewState["PrimaryEmailAddress"] = null;
                ViewState["AccountId"] = null;
            }
        }

        public string GetAccountImage(int accountTypeId)
        {
            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
            string imgName = "mailbox_16.gif";
            if (accountType == ExchangeAccountType.Contact)
                imgName = "contact_16.gif";
            else if (accountType == ExchangeAccountType.DistributionList)
                imgName = "dlist_16.gif";
            else if (accountType == ExchangeAccountType.Room)
                imgName = "room_16.gif";
            else if (accountType == ExchangeAccountType.Equipment)
                imgName = "equipment_16.gif";
            else if (accountType == ExchangeAccountType.SharedMailbox)
                imgName = "shared_16.gif";

            return GetThemedImage("Exchange/" + imgName);
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
            ExchangeAccount[] accounts = ES.Services.ExchangeServer.SearchAccounts(PanelRequest.ItemID,
                chkIncludeMailboxes.Checked, chkIncludeContacts.Checked, chkIncludeLists.Checked,
                chkIncludeRooms.Checked, chkIncludeEquipment.Checked, chkIncludeSharedMailbox.Checked, false,
                ddlSearchColumn.SelectedValue, txtSearchValue.Text + "%", "");

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


        }

        protected void chkIncludeMailboxes_CheckedChanged(object sender, EventArgs e)
        {
            BindPopupAccounts();
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            BindPopupAccounts();
        }

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            BindSelectedAccount(null);
        }

        protected void ImageButton1_Click(object sender, EventArgs e)
        {
            // bind all accounts
            BindPopupAccounts();

            // show modal
            SelectAccountsModal.Show();
        }

        protected void gvPopupAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectAccount")
            {

                string[] parts = e.CommandArgument.ToString().Split('^');
                ExchangeAccount account = new ExchangeAccount();
                account.AccountName = parts[0];
                account.DisplayName = parts[1];
                account.PrimaryEmailAddress = parts[2];
                account.AccountId = Utils.ParseInt(parts[3]);


                // set account
                BindSelectedAccount(account);

                // hide popup
                SelectAccountsModal.Hide();

                // update parent panel
                MainUpdatePanel.Update();
            }
        }

        protected void OnSorting(object sender, GridViewSortEventArgs e)
        {

            BindPopupAccounts();

        }
    }
}
