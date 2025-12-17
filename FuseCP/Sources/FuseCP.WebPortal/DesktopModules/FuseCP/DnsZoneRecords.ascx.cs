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
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.DNS;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal
{
    public partial class DnsZoneRecords : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //int recorddefaultTTL = 0;
            if (!IsPostBack)
            {
                // save return URL
                ViewState["ReturnUrl"] = Request.UrlReferrer.ToString();

                // toggle panels
                ShowPanels(false);

                // domain name
                DomainInfo domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);
                litDomainName.Text = domain.DomainName;
                if (Utils.IsIdnDomain(domain.DomainName))
                {
                    litDomainName.Text = string.Format("{0} ({1})", Utils.UnicodeToAscii(domain.DomainName), domain.DomainName);
                }

                if (PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Dns, Quotas.DNS_EDITTTL))
                {
                    gvRecords.Columns[3].Visible = true;
                }


                //recorddefaultTTL = domain.RecordDefaultTTL;
            }
        }

        public string GetRecordFullData(string recordType, string recordData, int mxPriority, int port)
        {

            switch (recordType)
            {
                case "MX":
                    return String.Format("[{0}], {1}", mxPriority, recordData);
                case "SRV":
                    return String.Format("[{0}], {1}", port, recordData);
                default:
                    return recordData;
            }
        }

        private void GetRecordsDetails(int recordIndex)
        {
            GridViewRow row = gvRecords.Rows[recordIndex];
            ViewState["SrvPort"] = ((Literal)row.Cells[0].FindControl("litSrvPort")).Text;
            ViewState["SrvWeight"] = ((Literal)row.Cells[0].FindControl("litSrvWeight")).Text;
            ViewState["SrvPriority"] = ((Literal)row.Cells[0].FindControl("litSrvPriority")).Text;
            ViewState["MxPriority"] = ((Literal)row.Cells[0].FindControl("litMxPriority")).Text;
            ViewState["RecordName"] = ((Literal)row.Cells[0].FindControl("litRecordName")).Text; ;
            ViewState["RecordType"] = (DnsRecordType)Enum.Parse(typeof(DnsRecordType), ((Literal)row.Cells[0].FindControl("litRecordType")).Text, true);
            ViewState["RecordData"] = ((Literal)row.Cells[0].FindControl("litRecordData")).Text;
            ViewState["RecordTTL"] = ((Literal)row.Cells[0].FindControl("litRecordTTL")).Text;
        }

        private void BindDnsRecord(int recordIndex)
        {
            try
            {
                ViewState["NewRecord"] = false;
                GetRecordsDetails(recordIndex);

                ddlRecordType.SelectedValue = ViewState["RecordType"].ToString();
                litRecordType.Text = ViewState["RecordType"].ToString();
                txtRecordName.Text = ViewState["RecordName"].ToString();
                txtRecordData.Text = ViewState["RecordData"].ToString();
                txtRecordTTL.Text = ViewState["RecordTTL"].ToString();
                txtMXPriority.Text = ViewState["MxPriority"].ToString();
                txtSRVPriority.Text = ViewState["SrvPriority"].ToString();
                txtSRVWeight.Text = ViewState["SrvWeight"].ToString();
                txtSRVPort.Text = ViewState["SrvPort"].ToString();

                if (txtRecordTTL.Text == "0")
                {
                    rowRecordTTL.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("GDNS_GET_RECORD", ex);
                return;
            }
        }

        protected void ddlRecordType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleRecordControls();
        }

        private void ToggleRecordControls()
        {
            rowMXPriority.Visible = false;
            rowSRVPriority.Visible = false;
            rowSRVWeight.Visible = false;
            rowSRVPort.Visible = false;
            lblRecordData.Text = "Record Data:";
            IPValidator.Enabled = false;
            rowRecordTTL.Visible = false;

            if (PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Dns, Quotas.DNS_EDITTTL))
            {
                rowRecordTTL.Visible = true;
            }


            switch (ddlRecordType.SelectedValue)
            {
                case "A":
                    lblRecordData.Text = "IP:";
                    IPValidator.Enabled = true;
                    break;
                case "AAAA":
                    lblRecordData.Text = "IP (v6):";
                    IPValidator.Enabled = true;
                    break;                    
                case "MX":
                    rowMXPriority.Visible = true;
                    break;
                case "SRV":
                    rowSRVPriority.Visible = true;
                    rowSRVWeight.Visible = true;
                    rowSRVPort.Visible = true;
                    lblRecordData.Text = "Host offering this service:";
                    break;
                default:
                    break;
            }


        }

		protected void Validate(object source, ServerValidateEventArgs args) {
			var ip = args.Value;
			System.Net.IPAddress ipaddr;
			args.IsValid = System.Net.IPAddress.TryParse(ip, out ipaddr) && (ip.Contains(":") || ip.Contains(".")) && 
                ((ddlRecordType.SelectedValue == "A" && ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) ||
                (ddlRecordType.SelectedValue == "AAAA" && ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6));
		}

        private void SaveRecord()
        {
            if (Page.IsValid)
            {
                bool newRecord = (bool)ViewState["NewRecord"];

                if (newRecord)
                {
                    // add record
                    try
                    {
                        int result = ES.Services.Servers.AddDnsZoneRecord(PanelRequest.DomainID,
                                                                          txtRecordName.Text.Trim(),
                                                                          (DnsRecordType)
                                                                          Enum.Parse(typeof(DnsRecordType),
                                                                                     ddlRecordType.SelectedValue, true),
                                                                          txtRecordData.Text.Trim(),
                                                                          Int32.Parse(txtMXPriority.Text.Trim()),
                                                                          Int32.Parse(txtSRVPriority.Text.Trim()),
                                                                          Int32.Parse(txtSRVWeight.Text.Trim()),
                                                                          Int32.Parse(txtSRVPort.Text.Trim()),
                                                                          Int32.Parse(txtRecordTTL.Text.Trim()));

                        if (result < 0)
                        {
                            ShowResultMessage(result);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("GDNS_ADD_RECORD", ex);
                        return;
                    }
                }
                else
                {
                    // update record
                    try
                    {
                        int result = ES.Services.Servers.UpdateDnsZoneRecord(PanelRequest.DomainID,
                                                                             ViewState["RecordName"].ToString(),
                                                                             ViewState["RecordData"].ToString(),
                                                                             txtRecordName.Text.Trim(),
                                                                             (DnsRecordType)ViewState["RecordType"],
                                                                             txtRecordData.Text.Trim(),
                                                                             Int32.Parse(txtMXPriority.Text.Trim()),
                                                                             Int32.Parse(txtSRVPriority.Text.Trim()),
                                                                             Int32.Parse(txtSRVWeight.Text.Trim()),
                                                                             Int32.Parse(txtSRVPort.Text.Trim()),
                                                                             Int32.Parse(txtRecordTTL.Text.Trim()));

                        if (result < 0)
                        {
                            ShowResultMessage(result);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("GDNS_UPDATE_RECORD", ex);
                        return;
                    }
                }

                // rebind and switch
                gvRecords.DataBind();
                ShowPanels(false);
            }
            else
                return;
        }

        private void DeleteRecord(int recordIndex)
        {
            try
            {
                GetRecordsDetails(recordIndex);

                int result = ES.Services.Servers.DeleteDnsZoneRecord(PanelRequest.DomainID,
                    ViewState["RecordName"].ToString(),
                    (DnsRecordType)ViewState["RecordType"],
                    ViewState["RecordData"].ToString());

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("GDNS_DELETE_RECORD", ex);
                return;
            }

            // rebind grid
            gvRecords.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ViewState["NewRecord"] = true;

            DomainInfo domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);
            lblDomainName.Text = "." + domain.DomainName;


            // erase fields
            ddlRecordType.SelectedIndex = 0;
            txtRecordName.Text = "";
            txtRecordData.Text = "";
            txtMXPriority.Text = "1";
            txtSRVPriority.Text = "0";
            txtSRVWeight.Text = "0";
            txtSRVPort.Text = "0";

            txtRecordTTL.Text = domain.RecordDefaultTTL.ToString();


            ShowPanels(true);
        }
        private void ShowPanels(bool editMode)
        {
            bool newRecord = (ViewState["NewRecord"] != null) ? (bool)ViewState["NewRecord"] : true;
            pnlEdit.Visible = editMode;
            litRecordType.Visible = !newRecord;
            ddlRecordType.Visible = newRecord;
            pnlRecords.Visible = !editMode;
            ToggleRecordControls();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ShowPanels(false);
        }
        protected void gvRecords_RowCreated(object sender, GridViewEditEventArgs e)
        {
            //txtRecordTTL = defaultTTL;
        }
        protected void gvRecords_RowEditing(object sender, GridViewEditEventArgs e)
        {
            BindDnsRecord(e.NewEditIndex);
            ShowPanels(true);
            e.Cancel = true;
        }
        protected void gvRecords_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteRecord(e.RowIndex);
            e.Cancel = true;
        }

        protected void odsDnsRecords_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ShowErrorMessage("GDNS_GET_RECORD", e.Exception);
                //this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (ViewState["ReturnUrl"] != null)
                Response.Redirect(ViewState["ReturnUrl"].ToString());
            else
                RedirectToBrowsePage();
        }
    }
}
