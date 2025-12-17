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

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPCC
{
    [ParseChildren(false)]
    [PersistChildren(true)]
    public class StyleButton : Button
    {
        protected override string TagName
        {
            get { return "button"; }
        }
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Button; }
        }
        public new string Text
        {
            get { return ViewState["NewText"] as string; }
            set { ViewState["NewText"] = HttpUtility.HtmlDecode(value); }
        }
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
           
            LiteralControl lc = new LiteralControl(this.Text);
            Controls.Add(lc);
            base.Text = UniqueID;
        }
        protected override void RenderContents(HtmlTextWriter writer)
        {
            RenderChildren(writer);
        }
    }
}
