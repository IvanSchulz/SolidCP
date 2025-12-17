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

using System.Collections.Generic;
using System;

namespace FuseCP.Providers.Common
{
    public class ResultObject
    {
        private bool isSuccess;
        private List<string> errorCodes;

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        public List<string> ErrorCodes
        {
            get { return errorCodes; }
            set { errorCodes = value; }
        }
        
        public ResultObject()
        {
            isSuccess = false;
            errorCodes = new List<string>();           
        }

        public void AddError(string errorCode, Exception ex)
        {
            if(ex != null)
                errorCode += ":" + ex.Message + "; " + ex.StackTrace;

            this.ErrorCodes.Add(errorCode);
            this.IsSuccess = false;
        }
    }
}
