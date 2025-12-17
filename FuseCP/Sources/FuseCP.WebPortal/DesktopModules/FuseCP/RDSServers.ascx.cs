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
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;
using AjaxControlToolkit;
using FuseCP.Providers.RemoteDesktopServices;
using System.Text;

namespace FuseCP.Portal
{
    public partial class RDSServers : FuseCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{            
			if (!IsPostBack)
			{
                gvRDSServers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                gvRDSServers.Sort("S.Name", System.Web.UI.WebControls.SortDirection.Ascending);                
			}

            RegisterStatusScript();

            gvRDSServers.DataBound -= OnDataBound;
            gvRDSServers.DataBound += OnDataBound;
        }       

        private void RegisterStatusScript()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("RDSAjaxQuery"))
            {
                var builder = new StringBuilder();
                builder.AppendLine("function checkStatus() {");
                builder.AppendFormat("var hidden = document.getElementById('{0}').value;", hdnGridState.ClientID);
                builder.AppendFormat("var grid = document.getElementById('{0}');", gvRDSServers.ClientID);
                builder.AppendFormat("var itemId = document.getElementById('{0}').value;", hdnItemId.ClientID);
                builder.AppendLine("if (hidden === 'True'){");
                builder.AppendLine("for (i = 1; i < grid.rows.length; i++) {");
                builder.AppendLine("var fqdnName = grid.rows[i].cells[0].children[0].value;");
                builder.AppendLine("$.ajax({");
                builder.AppendLine("type: 'post',");
                builder.AppendLine("dataType: 'json',");
                builder.AppendLine("data: { fqdnName: fqdnName, itemIndex: i },");
                builder.AppendLine("url: 'RdsServerStatusHandler.ashx',");
                builder.AppendLine("success: function (data) {");
                builder.AppendFormat("$('#{0}').val('false');", hdnGridState.ClientID);
                builder.AppendLine("var array = data.split(':');");
                builder.AppendLine("var status = array[0];");
                builder.AppendLine("var index = array[1];");
                builder.AppendLine("var show = array[2];");
                builder.AppendLine("grid.rows[index].cells[6].childNodes[0].data = status;");
                builder.AppendLine("if (show === 'True'){");
                builder.AppendLine("var link = grid.rows[index].cells[7].children[0];");
                builder.AppendLine("link.style.display = 'inline'");
                builder.AppendLine("link = grid.rows[index].cells[8].children[0];");
                builder.AppendLine("link.style.display = 'inline'");
                builder.AppendLine("link = grid.rows[index].cells[9].children[0];");
                builder.AppendLine("link.style.display = 'inline'");
                builder.AppendLine("link = grid.rows[index].cells[10].children[0];");
                builder.AppendLine("link.style.display = 'inline'");
                builder.AppendLine("}");
                builder.AppendLine("}");
                builder.AppendLine("}");
                builder.AppendLine(")}");
                builder.AppendLine("}");
                builder.AppendLine("}");                
                
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));
                Page.ClientScript.RegisterClientScriptBlock(typeof(RDSServers), "RDSAjaxQuery", builder.ToString(), true);
            }
        }

        private void OnDataBound(object sender, EventArgs e)
        {
            if (gvRDSServers.Rows.Count > 0)
            {
                hdnGridState.Value = true.ToString();
            }
        }
        
        protected void odsRDSServersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.Exception != null)
			{
				ProcessException(e.Exception.InnerException);
				this.DisableControls = true;
				e.ExceptionHandled = true;
			}
		}

        protected void btnAddRDSServer_Click(object sender, EventArgs e)
		{
            Response.Redirect(EditUrl("add_rdsserver"));
		}

        protected void btnSearchClick(object sender, EventArgs e)
        {
            gvRDSServers.DataBind();
        }

        protected void gvRDSServers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete rds server
                int rdsServerId;
                bool hasValue = int.TryParse(e.CommandArgument.ToString(), out rdsServerId);

                ResultObject result = new ResultObject();
                result.IsSuccess = false;

                try
                {
                    if (hasValue)
                    {
                        result = ES.Services.RDS.RemoveRdsServer(rdsServerId);
                    }

                    ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();

                    if (!result.IsSuccess)
                    {
                        messageBox.ShowMessage(result, "REMOTE_DESKTOP_SERVICES_REMOVE_RDSSERVER", "RDS");
                        return;
                    }

                    gvRDSServers.DataBind();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("REMOTE_DESKTOP_SERVICES_REMOVE_RDSSERVER", ex);
                }
            }
            else if (e.CommandName == "ViewInfo")
            {
                try
                {
                    ShowInfo(e.CommandArgument.ToString());
                }
                catch (Exception)
                {
                }
            }
            else if (e.CommandName == "Restart")
            {
                Restart(e.CommandArgument.ToString());
            }
            else if (e.CommandName == "ShutDown")
            {
                ShutDown(e.CommandArgument.ToString());
            }
            else if (e.CommandName == "InstallCertificate")
            {
                InstallCertificate(e.CommandArgument.ToString());
            }
            else if (e.CommandName == "EditServer")
            {
                EditServer(e.CommandArgument.ToString());
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvRDSServers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            gvRDSServers.DataBind();
        }

        private void ShowInfo(string serverId)
        {
            ViewInfoModal.Show();
            var rdsServer = ES.Services.RDS.GetRdsServer(Convert.ToInt32(serverId));
            var serverInfo = ES.Services.RDS.GetRdsServerInfo(null, rdsServer.FqdName);
            litProcessor.Text = string.Format("{0}x{1} MHz", serverInfo.NumberOfCores, serverInfo.MaxClockSpeed);
            litLoadPercentage.Text = string.Format("{0}%", serverInfo.LoadPercentage);
            litMemoryAllocated.Text = string.Format("{0} MB", serverInfo.MemoryAllocatedMb);
            litFreeMemory.Text = string.Format("{0} MB", serverInfo.FreeMemoryMb);
            rpServerDrives.DataSource = serverInfo.Drives;
            rpServerDrives.DataBind();
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
        }

        private void Restart(string serverId)
        {
            var rdsServer = ES.Services.RDS.GetRdsServer(Convert.ToInt32(serverId));
            ES.Services.RDS.RestartRdsServer(null, rdsServer.FqdName);
            Response.Redirect(Request.Url.ToString(), true);
        }

        private void ShutDown(string serverId)
        {
            var rdsServer = ES.Services.RDS.GetRdsServer(Convert.ToInt32(serverId));
            ES.Services.RDS.ShutDownRdsServer(null, rdsServer.FqdName);
            Response.Redirect(Request.Url.ToString(), true);
        }

        private void RefreshServerInfo()
        {
            var servers = odsRDSServersPaged.Select();
            gvRDSServers.DataSource = servers;
            gvRDSServers.DataBind();
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
        }

        private void InstallCertificate(string serverId)
        {
            var rdsServer = ES.Services.RDS.GetRdsServer(Convert.ToInt32(serverId));            

            try
            {
                ES.Services.RDS.InstallSessionHostsCertificate(rdsServer);
                ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
                ShowSuccessMessage("RDSSESSIONHOST_CERTIFICATE_INSTALLED");
            }
            catch(Exception ex)
            {
                ShowErrorMessage("RDSSESSIONHOST_CERTIFICATE_NOT_INSTALLED", ex);
            }

            messageBoxPanel.Update();
        }

        private void EditServer(string serverId)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_rdsserver", "SpaceID=" + PanelSecurity.PackageId, "ServerId=" + serverId));
        }
	}
}
