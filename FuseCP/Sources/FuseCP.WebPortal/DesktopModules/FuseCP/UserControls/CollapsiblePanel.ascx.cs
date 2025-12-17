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
    public partial class CollapsiblePanel : FuseCPControlBase
    {
        public const string DEFAULT_EXPAND_IMAGE = "shevron_expand.gif";
        public const string DEFAULT_COLLAPSE_IMAGE = "shevron_collapse.gif";

        public string CssClass
        {
            get { return HeaderPanel.CssClass; }
            set { HeaderPanel.CssClass = value; }
        }

        private string _resourceKey;
        public string ResourceKey
        {
            get { return _resourceKey; }
            set { _resourceKey = value; }
        }

        bool _isCollapsed = false;
        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set { _isCollapsed = value; }
        }

        string _expandImage;
        public string ExpandImage
        {
            get { return _expandImage; }
            set { _expandImage = value; }
        }

        string _collapseImage;
        public string CollapseImage
        {
            get { return _collapseImage; }
            set { _collapseImage = value; }
        }

        public string TargetControlID
        {
            get { return cpe.TargetControlID; }
            set { cpe.TargetControlID = value; }
        }

        public string Text
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        protected void cpe_ResolveControlID(object sender, AjaxControlToolkit.ResolveControlEventArgs e)
        {
            e.Control = this.Parent.FindControl(e.ControlID);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_collapseImage))
                _collapseImage = DEFAULT_COLLAPSE_IMAGE;

            if (String.IsNullOrEmpty(_expandImage))
                _expandImage = DEFAULT_EXPAND_IMAGE;

            // Initialize the ContentPanel to be either expanded or collapsed depending on the flag
			// Due to the fact that this control can loaded dynamically we need to setup images every time.
            cpe.Collapsed = _isCollapsed;
            cpe.CollapsedImage = GetThemedImage(_expandImage);
            cpe.ExpandedImage = GetThemedImage(_collapseImage);

            // get localized title
            if (!String.IsNullOrEmpty(ResourceKey))
            {
                FuseCPControlBase parentControl = this.Parent as FuseCPControlBase;
                if(parentControl != null)
                    lblTitle.Text = parentControl.GetLocalizedString(ResourceKey + ".Text");
            }

            ToggleImage.ImageUrl = GetThemedImage(_isCollapsed ? _expandImage : _collapseImage);
        }
    }
}
