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

using Microsoft.Web.Administration;

namespace FuseCP.Providers.Web.Iis.WebObjects
{
    using Common;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using System;
    using System.Text;
	using System.Collections.Generic;
	using System.Collections;
	using FuseCP.Providers.Web.Iis.Utility;
	using System.IO;
	using FuseCP.Providers.Utils;

    internal sealed class CustomHttpErrorsModuleService : ConfigurationModuleService
    {
		public const string StatusCodeAttribute = "statusCode";
		public const string SubStatusCodeAttribute = "subStatusCode";
		public const string PathAttribute = "path";
		public const string ResponseModeAttribute = "responseMode";
		public const string PrefixLanguageFilePath = "prefixLanguageFilePath";

		public void GetCustomErrors(ServerManager srvman, WebAppVirtualDirectory virtualDir)
		{
			var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
			//
			var httpErrorsSection = config.GetSection(Constants.HttpErrorsSection);

		    virtualDir.ErrorMode = (HttpErrorsMode)httpErrorsSection.GetAttributeValue("errorMode");
            virtualDir.ExistingResponse = (HttpErrorsExistingResponse)httpErrorsSection.GetAttributeValue("existingResponse");
			//
			var errorsCollection = httpErrorsSection.GetCollection();
			//
			var errors = new List<HttpError>();
			//
			foreach (var item in errorsCollection)
			{
				var item2Get = GetHttpError(item, virtualDir);
				//
				if (item2Get == null)
					continue;
				//
				errors.Add(item2Get);
			}
			//
			virtualDir.HttpErrors = errors.ToArray();
		}

		public void SetCustomErrors(WebAppVirtualDirectory virtualDir)
		{
			#region Revert to parent settings (inherited)
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				//
				var section = config.GetSection(Constants.HttpErrorsSection);
				//
				section.RevertToParent();
				//
				srvman.CommitChanges();
			} 
			#endregion

			#region Put the change in effect
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				//
				var section = config.GetSection(Constants.HttpErrorsSection);

                // set error mode
                section.SetAttributeValue("errorMode", virtualDir.ErrorMode);
                if (virtualDir.ErrorMode == HttpErrorsMode.Detailed)
                {
                    section.SetAttributeValue("existingResponse", HttpErrorsExistingResponse.PassThrough);
                }
                else
                {
                    section.SetAttributeValue("existingResponse", HttpErrorsExistingResponse.Auto);
                }

                // save custom errors
                if (virtualDir.HttpErrors != null && virtualDir.HttpErrors.Length > 0)
                {
                    var errorsCollection = section.GetCollection();
                    //
                    foreach (var item in virtualDir.HttpErrors)
                    {
                        int indexOf = FindHttpError(errorsCollection, item);
                        // Just update the element attributes - IIS 7 API will do the rest
                        if (indexOf > -1)
                        {
                            var item2Renew = errorsCollection[indexOf];
                            //
                            FillConfigurationElementWithData(item2Renew, item, virtualDir);
                            //
                            continue;
                        }
                        //
                        var item2Add = CreateHttpError(errorsCollection, item, virtualDir);
                        //
                        if (item2Add == null)
                            continue;
                        //
                        errorsCollection.Add(item2Add);
                    }
                }
			    
                //
				srvman.CommitChanges();
			} 
			#endregion
		}

		private HttpError GetHttpError(ConfigurationElement element, WebAppVirtualDirectory virtualDir)
		{
			if (element == null || virtualDir == null)
				return null;
			// skip inherited http errors
			if (!element.IsLocallyStored)
				return null;
			//
			var error = new HttpError
			{
				ErrorCode		= Convert.ToString(element.GetAttributeValue(StatusCodeAttribute)),
				ErrorSubcode	= Convert.ToString(element.GetAttributeValue(SubStatusCodeAttribute)),
				ErrorContent	= Convert.ToString(element.GetAttributeValue(PathAttribute)),
				HandlerType		= Enum.GetName(typeof(HttpErrorResponseMode), element.GetAttributeValue(ResponseModeAttribute))
			};

			// Make error path relative to the virtual directory's root folder
			if (error.HandlerType.Equals("File") // 0 is supposed to be File
				&& error.ErrorContent.Length > virtualDir.ContentPath.Length)
			{
				error.ErrorContent = error.ErrorContent.Substring(virtualDir.ContentPath.Length);
			}
			//
			return error;
		}

		private ConfigurationElement CreateHttpError(ConfigurationElementCollection collection, HttpError error, WebAppVirtualDirectory virtualDir)
		{
			// Skip elements either empty or with empty data
			if (error == null || String.IsNullOrEmpty(error.ErrorContent))
				return null;
			
			// Create new custom error
			ConfigurationElement error2Add = collection.CreateElement("error");
			
			if (!FillConfigurationElementWithData(error2Add, error, virtualDir))
				return null;
			//
			return error2Add;
		}

		private bool FillConfigurationElementWithData(ConfigurationElement item2Fill, HttpError error, WebAppVirtualDirectory virtualDir)
		{
			// code
			Int64 statusCode = 0;
			if (!Int64.TryParse(error.ErrorCode, out statusCode)
				|| statusCode < 400 || statusCode > 999)
				return false;

			// sub-code
			Int32 subStatusCode = -1;
			if (!Int32.TryParse(error.ErrorSubcode, out subStatusCode))
				return false;
			//
			if (subStatusCode == 0)
				subStatusCode = -1;

            // correct error content
            string errorContent = error.ErrorContent;
            if (error.HandlerType.Equals("File"))
            {
                if(error.ErrorContent.Length > virtualDir.ContentPath.Length)
                    errorContent = errorContent.Substring(virtualDir.ContentPath.Length);

                errorContent = FileUtils.CorrectRelativePath(errorContent);
            }

			item2Fill.SetAttributeValue(StatusCodeAttribute, statusCode);
			item2Fill.SetAttributeValue(SubStatusCodeAttribute, subStatusCode);
			item2Fill.SetAttributeValue(PathAttribute, errorContent);
			// Cleanup prefix language file path attribute.
			item2Fill.SetAttributeValue(PrefixLanguageFilePath, String.Empty);

			//
			item2Fill.SetAttributeValue(ResponseModeAttribute, error.HandlerType);
			// We are succeeded
			return true;
		}

		private int FindHttpError(ConfigurationElementCollection collection, HttpError error)
		{
			for (int i = 0; i < collection.Count; i++)
			{
				var item = collection[i];
				//
				if ((Int64)item.GetAttributeValue(StatusCodeAttribute) == Int64.Parse(error.ErrorCode)
					&& (Int32)item.GetAttributeValue(SubStatusCodeAttribute) == Int32.Parse(error.ErrorSubcode))
				{
					return i;
				}
			}
			//
			return -1;
		}
    }
}
