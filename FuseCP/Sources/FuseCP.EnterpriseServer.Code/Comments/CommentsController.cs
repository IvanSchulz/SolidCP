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
using System.Collections.Generic;
using System.Text;

namespace FuseCP.EnterpriseServer
{
    public class CommentsController : ControllerBase
    {
        public CommentsController(ControllerBase provider) : base(provider) { }
        public DataSet GetComments(int userId, string itemTypeId, int itemId)
        {
            return Database.GetComments(SecurityContext.User.UserId, userId, itemTypeId, itemId);
        }

        public int AddComment(string itemTypeId, int itemId,
            string commentText, int severityId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // add comment
            Database.AddComment(SecurityContext.User.UserId, itemTypeId,
                itemId, commentText, severityId);

            return 0;
        }

        public int DeleteComment(int commentId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // delete comment
            Database.DeleteComment(SecurityContext.User.UserId, commentId);

            return 0;
        }
    }
}
