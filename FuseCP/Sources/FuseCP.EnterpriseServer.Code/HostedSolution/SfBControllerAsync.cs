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
using System.Threading;
using System.Collections.Generic;
using System.Text;
using FuseCP.Providers;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;
using FuseCP.Server.Client;
//using FuseCP.Providers.SfB;
using FuseCP.EnterpriseServer.Code.HostedSolution;


namespace FuseCP.EnterpriseServer.Code.HostedSolution
{
    public class SfBControllerAsync: ControllerAsyncBase
    {
        private int sfbServiceId;
        private int organizationServiceId;

        public int SfBServiceId
        {
            get { return this.sfbServiceId; }
            set { this.sfbServiceId = value; }
        }

        public int OrganizationServiceId
        {
            get { return this.organizationServiceId; }
            set { this.organizationServiceId = value; }
        }


        public void Enable_CsComputerAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(Enable_CsComputer));
            t.Start();
        }

        private void Enable_CsComputer()
        {
            int[] sfbServiceIds;

            SfBController.GetSfBServices(sfbServiceId, out sfbServiceIds);

            foreach (int id in sfbServiceIds)
            {
                SfBServer sfb = null;
                try
                {
                    sfb = SfBController.GetSfBServer(id, organizationServiceId);
                    if (sfb != null)
                    {
                        sfb.ReloadConfiguration();
                    }
                }
                catch (Exception exe)
                {
                    TaskManager.WriteError(exe);
                    continue;
                }
            }
        }
    }
}
