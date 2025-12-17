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

namespace FuseCP.Portal
{
    public partial class Gauge : FuseCPControlBase
    {
        private int width = 100;
		private bool oneColour = false;

        public bool DisplayGauge
        {
            get { return (ViewState["DisplayGauge"] != null) ? (bool)ViewState["DisplayGauge"] : true; }
            set { ViewState["DisplayGauge"] = value; }
        }

        public bool DisplayText
        {
            get { return (ViewState["DisplayText"] != null) ? (bool)ViewState["DisplayText"] : true; }
            set { ViewState["DisplayText"] = value; }
        }

        public int Progress
        {
            get { return (ViewState["Progress"] != null) ? (int)ViewState["Progress"] : 0; }
            set { ViewState["Progress"] = value; }
        }

        public int Total
        {
            get { return (ViewState["Total"] != null) ? (int)ViewState["Total"] : 0; }
            set { ViewState["Total"] = value; }
        }

        public int Available
        {
            get { return (ViewState["Available"] != null) ? (int)ViewState["Available"] : -1; }
            set { ViewState["Available"] = value; }
        }
        
        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

		public bool OneColour
		{
			get { return this.oneColour; }
			set { this.oneColour = value; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!DisplayGauge)
                return;

            string leftSideSrc = Page.ResolveUrl(PortalUtils.GetThemedImage("gauge_left.gif"));
            string rightSideSrc = Page.ResolveUrl(PortalUtils.GetThemedImage("gauge_right.gif"));
            string bkgSrc = Page.ResolveUrl(PortalUtils.GetThemedImage("gauge_bkg.gif"));

            // calculate the width of the gauge
            int fTotal = Total;
            int percent = (fTotal > 0) ? Convert.ToInt32(Math.Round((double)Progress / (double)fTotal * 100)) : 0;

            double fFilledWidth = (fTotal > 0) ? ((double)Progress / (double)fTotal * Width) : 0;
            int filledWidth = Convert.ToInt32(fFilledWidth);

			if (filledWidth > Width)
				filledWidth = Width;

            string fillSrc = "gauge_green.gif";
            if(percent > 60 && percent < 90 && !oneColour)
                fillSrc = "gauge_yellow.gif";
            else if (percent >= 90 && !oneColour)
                fillSrc = "gauge_red.gif";

            fillSrc = Page.ResolveUrl(PortalUtils.GetThemedImage(fillSrc));

            GaugeContent.Text = DrawImage(leftSideSrc, 1);
            GaugeContent.Text += DrawImage(fillSrc, filledWidth);
            GaugeContent.Text += DrawImage(bkgSrc, width - filledWidth);
            GaugeContent.Text += DrawImage(rightSideSrc, 1);
        }

        private string DrawImage(string src, int width)
        {
            return String.Format("<img src=\"{0}\" width=\"{1}\" height=\"11\" align=\"absmiddle\"/>",
                src, width);
        }
    }
}
