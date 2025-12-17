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

#if Reporting
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.VPSForPC
{
    public partial class MonitoringPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));
            cs.RegisterClientScriptInclude("jqueryui", ResolveUrl("~/JavaScript/jquery-ui-1.8.9.min.js"));

			if (Page.IsPostBack == false)
			{
				SetDefaultMonitoringTimeFrame();

				BindPerformanceValues();
			}
        }

		private void SetDefaultMonitoringTimeFrame()
		{
			if (String.IsNullOrEmpty(txtStartPeriod.Text))
			{
				txtStartPeriod.Text = DateTime.Now.AddDays(-1).ToShortDateString();
			}
			//
			if (String.IsNullOrEmpty(txtEndPeriod.Text))
			{
				txtEndPeriod.Text = DateTime.Now.ToShortDateString();
			}
		}

		private void BindPerformanceValues()
		{
			DateTime StartP = Convert.ToDateTime(txtStartPeriod.Text);
			DateTime EndP = Convert.ToDateTime(txtEndPeriod.Text);

			EndP = (EndP.CompareTo(DateTime.Now.Date) == 0 ? DateTime.Now : EndP);

			PerformanceType pt = PerformanceType.Processor;

			string charType = Page.Request.QueryString["chartType"];

			InitControls(charType, StartP, EndP);

			switch (charType)
			{
				case "Processor":
					pt = PerformanceType.Processor;
					break;
				case "Network":
					pt = PerformanceType.Network;
					break;
				case "Memory":
					pt = PerformanceType.Memory;
					break;
			}

			LoadChartData(ChartCounter, pt, StartP, EndP);
		}

		protected void operationTimer_Tick(object sender, EventArgs e)
		{
			BindPerformanceValues();
		}

        private void LoadChartData(Chart control, PerformanceType perfType, DateTime startPeriod, DateTime endPeriod)
        {
            PerformanceDataValue[] perfValues = ES.Services.VPSPC.GetPerfomanceValue(PanelRequest.ItemID, perfType, startPeriod, endPeriod);

            if (perfValues != null)
            {
                foreach (PerformanceDataValue item in perfValues)
                {
					//
					control.Series["series"].Points.AddXY(item.TimeSampled.ToString(), item.SampleValue);
                }
            }
        }

        private void InitControls(string charType, DateTime startPeriod, DateTime endPeriod)
        {
            ChartCounter.Titles.Add(charType);
            ChartCounter.Series["series"].ChartType = (charType.Equals("Processor") ? SeriesChartType.Line : SeriesChartType.SplineArea);
            //ChartCounter.Series["series"].IsValueShownAsLabel = true;
            ChartCounter.Series["series"].Color = (!charType.Equals("Memory") ? System.Drawing.Color.FromArgb(220, 65, 140, 240) : ChartCounter.Series["series"].Color);
            ChartCounter.Series["series"]["ShowMarkerLines"] = "True";

            ChartCounter.ChartAreas["chartArea"].AxisX.IsMarginVisible = true;
        }
    }
}
#endif
