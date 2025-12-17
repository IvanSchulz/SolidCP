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
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;

namespace FuseCP.Portal
{
    public partial class DiskspaceReport : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            // set display preferences
            gvReport.PageSize = UsersHelper.GetDisplayItemsPerPage();        
        }

        protected void odsReport_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            if (dr != null)
            {
                if ((int)dr["UsagePercentage"] > 100)
                    e.Row.CssClass = "NormalBold";
            }
        }

        public string GetAllocatedValue(int val)
        {
            return (val == -1) ? GetLocalizedString("Unlimited.Text") : val.ToString();
        }

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
            ExportReport();
        }

        private void ExportReport()
        {
            // first try to load user specific transform (ie, reseller) if not found, then load the default admin user set            
            UserSettings DiskTransform;
            try {
                DiskTransform = ES.Services.Users.GetUserSettings(PanelSecurity.SelectedUserId, SystemSettings.DISKSPACE_TRANSFORM);
            } catch (AmbiguousMatchException) {
                DiskTransform = ES.Services.Users.GetUserSettings(1, SystemSettings.DISKSPACE_TRANSFORM);   //admin case
            }
            RenderReport(DiskTransform);
        }

        private string ReportTransform(string xslInput, string xmlInput)
        {


            string output = String.Empty;
            using (StringReader xmltransform = new StringReader(xslInput)) // xslInput is a string that contains xsl
            using (StringReader xmlinput = new StringReader(xmlInput)) // xmlInput is a string that contains xml
            {
                using (XmlReader xmlReaderTransform = XmlReader.Create(xmltransform))
                using (XmlReader xmlReaderInput = XmlReader.Create(xmlinput))
                {
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(xmlReaderTransform);
                    using (StringWriter outstream = new StringWriter())
                    using (XmlWriter xmlOutput = XmlWriter.Create(outstream, xslt.OutputSettings)) // use OutputSettings of xsl, so it can be output as HTML
                    {
                        xslt.Transform(xmlReaderInput, xmlOutput);
                        output = outstream.ToString();
                    }
                }
            }
            return output;
        }

        private void RenderReport(UserSettings Transform)
        {
            // build Report data into XML
            DataTable dtRecords = new ReportsHelper().GetPackagesDiskspacePaged(-1, Int32.MaxValue, 0, "");

            XmlSerializer xmlser = new XmlSerializer(dtRecords.GetType());
            string xmlString;
            using (StringWriter strwriter = new StringWriter())
            {
                xmlser.Serialize(strwriter, dtRecords);
                xmlString = strwriter.ToString();
            }

            // where the magic happens
            string TransformedXML = ReportTransform(Transform["Transform"], xmlString);

            string fileName = "DiskSpaceReport" + Transform["TransformSuffix"];

            Response.Clear();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            Response.ContentType = Transform["TransformContentType"];

            Response.Write(TransformedXML);

            Response.End(); ;
        }
    }
}
