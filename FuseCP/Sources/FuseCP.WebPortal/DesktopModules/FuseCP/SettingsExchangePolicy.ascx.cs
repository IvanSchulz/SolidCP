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

using System.Linq;
using System.Collections.Generic;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;

namespace FuseCP.Portal
{
    public partial class SettingsExchangePolicy : FuseCPControlBase, IUserSettingsEditorControl
    {
        internal static AdditionalGroup[] additionalGroups;

        #region IUserSettingsEditorControl Members

        public void BindSettings(UserSettings settings)
        {
            // mailbox
            mailboxPasswordPolicy.Value = settings["MailboxPasswordPolicy"];
            orgIdPolicy.Value = settings["OrgIdPolicy"];

            additionalGroups = ES.Services.Organizations.GetAdditionalGroups(settings.UserId);
            
            orgPolicy.SetAdditionalGroups(additionalGroups);
            orgPolicy.Value = settings["OrgPolicy"];
        }

        public void SaveSettings(UserSettings settings)
        {
            settings["MailboxPasswordPolicy"] = mailboxPasswordPolicy.Value;
            settings["OrgIdPolicy"] = orgIdPolicy.Value;
            settings["OrgPolicy"] = orgPolicy.Value;

            if (Utils.ParseBool(orgPolicy.Value, false))
            {
                List<AdditionalGroup> newAdditionalGroups = orgPolicy.GetGridViewGroups();
 
                foreach (AdditionalGroup oldGroup in additionalGroups)
                {
                    AdditionalGroup upGroup = newAdditionalGroups.Where(x => x.GroupId == oldGroup.GroupId).FirstOrDefault();

                    if(upGroup != null && upGroup.GroupName != oldGroup.GroupName)
                    {
                        ES.Services.Organizations.UpdateAdditionalGroup(oldGroup.GroupId, upGroup.GroupName);

                        newAdditionalGroups.Remove(upGroup);
                    }
                    else
                    {
                        ES.Services.Organizations.DeleteAdditionalGroup(oldGroup.GroupId);
                    }
                }

                foreach (AdditionalGroup newGroup in newAdditionalGroups)
                {
                    ES.Services.Organizations.AddAdditionalGroup(settings.UserId, newGroup.GroupName);
                }
            }
        }

        #endregion
    }
}
