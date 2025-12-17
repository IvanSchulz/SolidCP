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

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class SpaceSummaryLetter : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindLetter();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SPACE_LETTER_GET", ex);
                    return;
                }
            }
        }

        private void BindLetter()
        {
            string body = null;

            try
            {
                body = ES.Services.Packages.GetEvaluatedPackageTemplateBody(PanelSecurity.PackageId);
            }
            catch (Exception ex)
            {
                body = ex.ToString();
            }
            litContent.Text = body != null ? body : "Your reseller has not setup Hosting Space Summary Letter";

            // bind user details
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package == null)
                RedirectSpaceHomePage();

            // load user details
            UserInfo user = ES.Services.Users.GetUserById(package.UserId);
            txtTo.Text = user.Email;
        }

        private void SendLetter()
        {
            try
            {
                int result = ES.Services.Packages.SendPackageSummaryLetter(PanelSecurity.PackageId,
                    txtTo.Text.Trim(), txtCC.Text.Trim());

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("SPACE_LETTER_SEND");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SPACE_LETTER_SEND", ex);
                return;
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            SendLetter();
        }
    }
}
