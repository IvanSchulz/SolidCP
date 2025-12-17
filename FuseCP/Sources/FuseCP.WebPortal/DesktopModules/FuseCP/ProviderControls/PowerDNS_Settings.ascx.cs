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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ProviderControls
{
    public partial class PowerDNS_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        #region Constants

        //pdns mysql db settings
        const string PDNSDbServer = "PDNSDbServer";
        const string PDNSDbPort = "PDNSDbPort";
        const string PDNSDbName = "PDNSDbName";
        const string PDNSDbUser = "PDNSDbUser";
        const string PDNSDbPassword = "PDNSDbPassword";

        //soa record settings
        const string ResponsiblePerson = "ResponsiblePerson";
        const string RefreshInterval = "RefreshInterval";
        const string RetryDelay = "RetryDelay";
        const string ExpireLimit = "ExpireLimit";
        const string MinimumTTL = "MinimumTTL";

        //DNS RecordTTL
        const string RecordDefaultTTL = "RecordDefaultTTL";
        const string RecordMinimumTTL = "RecordMinimumTTL";

        //name servers
        const string cNameServers = "NameServers";

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            RenderValidationJavaScrip();
        }

        private void RenderValidationJavaScrip()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("pdnsValidationFunctions"))
            {
                StringBuilder jsFunctions = new StringBuilder();

                jsFunctions.Append("<script type=\"text/javascript\"> ");
                jsFunctions.Append("function pdnsComparePasswordFields(source, args) {");
                jsFunctions.Append(" var txtPwd = document.getElementById('" + txtPassword.ClientID + "');");
                jsFunctions.Append(" var txtCPwd = document.getElementById('" + txtConfirmPassword.ClientID + "');");
                jsFunctions.Append(" var result = true;");
                jsFunctions.Append(" if (txtPwd.value != '' && txtCPwd.value == '') {");
                jsFunctions.Append("  result = false;");
                jsFunctions.Append(" }");
                jsFunctions.Append(" args.IsValid = result;");
                jsFunctions.Append("} ");
                jsFunctions.Append("</");
                jsFunctions.Append("script>");

                Page.ClientScript.RegisterClientScriptBlock(
                      typeof(PowerDNS_Settings)
                    , "pdnsValidationFunctions"
                    , jsFunctions.ToString()
                    , false
                );
            }
        }

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				RenderFtuNote();
			}
		}

		private void RenderFtuNote()
		{
			string ftuNote = GetLocalizedString("FirsttimeUserNote");
			//
			ServerInfo serverInfo = ES.Services.Servers.GetServerById(PanelRequest.ServerId);
			//
			lblFirsttimeUserNote.InnerHtml = String.Format(ftuNote, serverInfo.ServerName);
		}

        public void BindSettings(StringDictionary settings)
        {
            //server settings
            txtServerAddress.Text = settings[PDNSDbServer];

            txtServerPort.Text = settings[PDNSDbPort];

            txtDatabase.Text = settings[PDNSDbName];
            txtUsername.Text = settings[PDNSDbUser];

            ViewState[PDNSDbPassword] = settings[PDNSDbPassword];

            if (!string.IsNullOrEmpty((string)ViewState[PDNSDbPassword]))
            {
                trCurrentPassword.Visible = true;
                varRequirePassword.Enabled = false;
            }
            else
            {
                varRequirePassword.Enabled = true;
                trCurrentPassword.Visible = false;
            }

            //soa record settings
            txtResponsiblePerson.Text = settings[ResponsiblePerson];
            intRefresh.Interval = Utils.ParseInt(settings[RefreshInterval], 0);
            intRetry.Interval = Utils.ParseInt(settings[RetryDelay], 0);
            intExpire.Interval = Utils.ParseInt(settings[ExpireLimit], 0);
            intTtl.Interval = Utils.ParseInt(settings[MinimumTTL], 0);

            //DNS RecordTTL
            txtRecordDefaultTTL.Text = settings[RecordDefaultTTL];
            txtRecordMinimumTTL.Text = settings[RecordMinimumTTL];

            //name servers
            nameServers.Value = settings[cNameServers];


            //ip address settings
            secondaryDNSServers.BindSettings(settings);
            iPAddressesList.BindSettings(settings);
        }

        public void SaveSettings(StringDictionary settings)
        {
            //server settings
            settings[PDNSDbServer] = txtServerAddress.Text;

            int port = 3306;
            if (!Int32.TryParse(txtServerPort.Text, out port))
            {
                port = 3306;
            }
            settings[PDNSDbPort] = port.ToString();


            settings[PDNSDbName] = txtDatabase.Text;
            settings[PDNSDbUser] = txtUsername.Text;
            settings[PDNSDbPassword] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState[PDNSDbPassword];


            
            //soa record settings
            settings[ResponsiblePerson] = txtResponsiblePerson.Text;
            settings[RefreshInterval] = intRefresh.Interval.ToString();
            settings[RetryDelay] = intRetry.Interval.ToString();
            settings[ExpireLimit] = intExpire.Interval.ToString();
            settings[MinimumTTL] = intTtl.Interval.ToString();

            //DNS RecordTTL
            settings[RecordDefaultTTL] = txtRecordDefaultTTL.Text;
            settings[RecordMinimumTTL] = txtRecordMinimumTTL.Text;

            //ip address settings
            secondaryDNSServers.SaveSettings(settings);
            iPAddressesList.SaveSettings(settings);  


            //name servers
            settings[cNameServers] = nameServers.Value;
        }
    }
}
