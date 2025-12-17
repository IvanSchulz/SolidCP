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

﻿using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal
{
    public partial class WebApplicationGallery : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

			// Maintains appearance settings corresponding to user's display preferences
			gvApplications.PageSize = UsersHelper.GetDisplayItemsPerPage();

            try
            {
				GalleryCategoriesResult result = ES.Services.WebApplicationGallery.GetGalleryCategories(PanelSecurity.PackageId);
				//
				if (!result.IsSuccess)
				{
					rbsCategory.Visible = false;
					messageBox.ShowMessage(result, "WAG_NOT_AVAILABLE", "ModuleWAG");
					return;
				}

                if (!IsPostBack)
                {
                    //
                    SetLanguage();
                    BindLanguages();
                    BindCategories();
					BindApplications();
                    ViewState["IsSearchResults"] = false;
                }
            }
            catch(Exception ex)
            {
                ShowErrorMessage("GET_WEB_GALLERY_CATEGORIES", ex);             
            }
        }

		protected void gvApplications_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvApplications.PageIndex = e.NewPageIndex;
            if ((bool)ViewState["IsSearchResults"] == false)
            {
                // categorized app list
                BindApplications();
                if (null != rbsCategory.SelectedItem)
                {
                    rbsCategory.SelectedItem.Attributes["class"] = "selected";
                }
            }
            else
            {
                // search result app list
                SearchButton_Click(sender, null);
            }
		}

        private void BindLanguages()
        {
            GalleryLanguagesResult result = ES.Services.WebApplicationGallery.GetGalleryLanguages(PanelSecurity.PackageId);
            dropDownLanguages.DataSource = result.Value;
            //dropDownLanguages.SelectedIndex = 0;
            dropDownLanguages.SelectedValue = (string)Session["WebApplicationGaleryLanguage"];
            dropDownLanguages.DataTextField = "Value";
            dropDownLanguages.DataValueField = "Name";
            dropDownLanguages.DataBind();

        }

        private void BindCategories()
        {
			GalleryCategoriesResult result = ES.Services.WebApplicationGallery.GetGalleryCategories(PanelSecurity.PackageId);
			//
			rbsCategory.DataSource = result.Value;
            rbsCategory.DataTextField = "Name";
            rbsCategory.DataValueField = "Id";
            rbsCategory.DataBind();

            // add empty
            ListItem listItem = new ListItem("All", "");
            listItem.Attributes["class"] = "selected";
            rbsCategory.Items.Insert(0, listItem);
        }

		private void BindApplications()
		{
            ViewState["IsSearchResults"] = false;
            WebAppGalleryHelpers helper = new WebAppGalleryHelpers();
			//
			GalleryApplicationsResult result = helper.GetGalleryApplications(rbsCategory.SelectedValue, PanelSecurity.PackageId);
			//
			gvApplications.DataSource = result.Value;
			gvApplications.DataBind();
		}

		protected void CategorySelectedIndexChanged(object sender, EventArgs e)
		{
		    ViewState["IsSearchResults"] = false;
		    searchBox.Text = "";
            gvApplications.PageIndex = 0;
            rbsCategory.SelectedItem.Attributes["class"] = "selected";

            BindApplications();
		}

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Install")
                Response.Redirect(EditUrl("ApplicationID", e.CommandArgument.ToString(), "edit",
                    "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if ((bool)ViewState["IsSearchResults"] == false)
            {
                gvApplications.PageIndex = 0;
            }
            ViewState["IsSearchResults"] = true;

            WebAppGalleryHelpers helper = new WebAppGalleryHelpers();
            GalleryApplicationsResult result = helper.GetGalleryApplicationsFiltered(searchBox.Text, PanelSecurity.PackageId);

            gvApplications.DataSource = result.Value;
            gvApplications.DataBind();
        }

        protected void dropDownLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["WebApplicationGaleryLanguage"] = dropDownLanguages.SelectedValue;

            SetLanguage();

            BindLanguages();
            BindCategories();
            BindApplications();

        }

        private void SetLanguage()
        {
            string lang = (string)Session["WebApplicationGaleryLanguage"];
            if (string.IsNullOrEmpty(lang))
            {
                lang = "en";
            }
            ES.Services.WebApplicationGallery.SetResourceLanguage(PanelSecurity.PackageId, lang);
        }

        protected string GetIconUrlOrDefault(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "/App_Themes/Default/Icons/sphere_128.png";
            }
            
            return "~/DesktopModules/FuseCP/ResizeImage.ashx?width=120&height=120&url=" + Server.UrlEncode(url);
        }
    }
}
