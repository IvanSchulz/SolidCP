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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using FuseCP.Web.Services;
using System.ComponentModel;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esComments: WebService
    {
        [WebMethod]
        public DataSet GetComments(int userId, string itemTypeId, int itemId)
        {
            return CommentsController.GetComments(userId, itemTypeId, itemId);
        }

        [WebMethod]
        public int AddComment(string itemTypeId, int itemId,
            string commentText, int severityId)
        {
            return CommentsController.AddComment(itemTypeId, itemId, commentText, severityId);
        }

        [WebMethod]
        public int DeleteComment(int commentId)
        {
            return CommentsController.DeleteComment(commentId);
        }
    }
}
