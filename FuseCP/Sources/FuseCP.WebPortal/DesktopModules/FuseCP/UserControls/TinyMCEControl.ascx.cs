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

using FuseCP.EnterpriseServer;
using FuseCP.Portal;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FuseCP.Portal
{
	public class TinyMCEControl : FuseCPControlBase
    {
		protected TextBox txtHtml;

		public int Rows
		{
			get
			{
				return this.txtHtml.Rows;
			}
			set
			{
				this.txtHtml.Rows = value;
			}
		}

		public string Text
		{
			get
			{
				return Regex.Replace(Regex.Replace(this.txtHtml.Text, "(<\\/ad:else>)|(<\\/ad:elseif>)", "", RegexOptions.IgnoreCase), "<.*?style=\"(.*?)\".*?>", (Match m) => m.Value.Replace("#", "##"), RegexOptions.IgnoreCase);
			}
			set
			{
				if (value == null)
				{
					return;
				}
				string str = Regex.Replace(value, "<.*?style=\"(.*?)\".*?>", (Match m) => m.Value.Replace("##", "#"), RegexOptions.IgnoreCase);
				this.txtHtml.Text = str;
			}
		}

		public bool UserGlobalMailTemplateCSS { get; set; }

		public Unit Width
		{
			get
			{
				return this.txtHtml.Width;
			}
			set
			{
				this.txtHtml.Width = value;
			}
		}

		public TinyMCEControl()
		{
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			string item = "";
			if (this.UserGlobalMailTemplateCSS)
			{
				item = ES.Services.Users.GetUserSettings(PanelSecurity.SelectedUserId, "GlobalMailTemplateCSS")["CSS"];
			}
			ClientScriptManager clientScript = this.Page.ClientScript;
			clientScript.RegisterClientScriptInclude("tinymce", base.ResolveUrl("~/tinymce/tinymce.min.js"));
			clientScript.RegisterClientScriptInclude("jquery.tinymce", base.ResolveUrl("~/tinymce/jquery.tinymce.min.js"));
			clientScript.RegisterClientScriptBlock(base.GetType(), "tinymce.textarea", string.Concat("<script language='javascript' type='text/javascript'>\n     $().ready(function () {\n            tinymce.init({\n              selector: '.htmlarea',\n              plugins: [\n                'advlist autolink lists link image charmap print preview anchor',\n                'searchreplace visualblocks code fullscreen',\n                'insertdatetime media table contextmenu paste code'\n              ],\n              toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | code',\n              removed_menuitems: 'newdocument',\n              custom_elements : 'ad:if,ad:elseif,ad:else,ad:foreach,ad:for,ad:set,ad:template',\n              extended_valid_elements : 'ad:if[test],ad:elseif[test],ad:else[test],ad:foreach[collection|var|index],ad:for[from|to|index],ad:set[name|value],ad:template[name]',\n              protect : [\n                /\\<\\/?ad\\:[^>]+\\>/g\n              ],\n              ", (string.IsNullOrEmpty(item) ? "" : string.Concat("content_style: ", javaScriptSerializer.Serialize(item), ",")), "\n              statusbar: false,\n              force_br_newlines : false,\n              force_p_newlines : false,\n              forced_root_block : '',\n            });\n     });\n </script>"));
		}
	}
}
