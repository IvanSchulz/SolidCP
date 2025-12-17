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
using System.Net;

using System.Collections;
using System.ComponentModel;

namespace FuseCP.Providers.Web
{
	public enum HttpErrorsMode
	{
        DetailedLocalOnly = 0,
        Custom = 1,
        Detailed = 2
	}

    public enum HttpErrorsExistingResponse
    {
        Auto = 0,
        Replace = 1,
        PassThrough = 2
    }

    public class HttpError
	{
        public const HttpErrorsMode DefaultHttpErrorsMode = HttpErrorsMode.DetailedLocalOnly;
        public const HttpErrorsExistingResponse DefaultHttpErrorsExistingResponse = HttpErrorsExistingResponse.Auto;

		private string errorCode;
		private string errorSubcode;
		private string handlerType;
		private string errorContent;

		public HttpError()
		{
		}

		public string ErrorCode
		{
			get { return errorCode; }
			set {errorCode = value; }
		}

        public string ErrorSubcode
		{
			get { return errorSubcode; }
			set {errorSubcode = value; }
		}

		public string HandlerType
		{
			get { return handlerType; }
			set{ handlerType = value; }
		}

		public string ErrorContent
		{
			get { return errorContent; }
			set { errorContent = value; }

		}
	}
}
