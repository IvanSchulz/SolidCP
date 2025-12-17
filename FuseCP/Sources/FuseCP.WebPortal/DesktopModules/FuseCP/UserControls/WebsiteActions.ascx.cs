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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Portal.UserControls;
using FuseCP.Providers;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal
{
    public enum WebsiteActionTypes
    {
        None = 0,
        Stop = 1,
        Start = 2,
        RestartAppPool = 3,
    }

    public partial class WebsiteActions : ActionListControlBase<WebsiteActionTypes>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override DropDownList ActionsList
        {
            get { return ddlWebsiteActions; }
        }

        protected override int DoAction(List<int> ids)
        {
            switch (SelectedAction)
            {
                case WebsiteActionTypes.Stop:
                    return ChangeWebsiteState(false, ids);
                case WebsiteActionTypes.Start:
                    return ChangeWebsiteState(true, ids);
                case WebsiteActionTypes.RestartAppPool:
                    return RestartAppPool(ids);
            }

            return 0;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            switch (SelectedAction)
            {
                case WebsiteActionTypes.Stop:
                case WebsiteActionTypes.Start:
                case WebsiteActionTypes.RestartAppPool:
                    FireExecuteAction();
                    break;
            }
        }

        private int ChangeWebsiteState(bool enable, List<int> ids)
        {
            foreach (var id in ids)
            {
                var state = enable ? ServerState.Started : ServerState.Paused;
                int result = ES.Services.WebServers.ChangeSiteState(id, state);

                if (result < 0)
                    return result;
            }

            return 0;
        }

        private int RestartAppPool(List<int> ids)
        {
            foreach (var id in ids)
            {
                int result = ES.Services.WebServers.ChangeAppPoolState(id, AppPoolState.Recycle);

                if (result < 0)
                    return result;
            }

            return 0;
        }
    }
}
