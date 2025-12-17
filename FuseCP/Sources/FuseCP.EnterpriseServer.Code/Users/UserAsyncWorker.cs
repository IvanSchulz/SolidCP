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
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.EnterpriseServer
{
    public class UserAsyncWorker: ControllerAsyncBase
    {
        private int threadUserId = -1;
        private int userId;
        private string taskId;
        private UserInfo user;

        #region Public properties
        public int ThreadUserId
        {
            get { return this.threadUserId; }
            set { this.threadUserId = value; }
        }

        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public string TaskId
        {
            get { return this.taskId; }
            set { this.taskId = value; }
        }

        public UserInfo User
        {
            get { return this.user; }
            set { this.user = value; }
        }
        #endregion

        #region Update User
        public void UpdateUserAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(UpdateUser));
            t.Start();
        }

        public void UpdateUser()
        {
            // impersonate thread
            if (threadUserId != -1)
                SecurityContext.SetThreadPrincipal(threadUserId);

            // update
            UserController.UpdateUser(taskId, user);
        }
        #endregion

        #region Delete User
        public void DeleteUserAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(DeleteUser));
            t.Start();
        }

        public void DeleteUser()
        {
            // impersonate thread
            if (threadUserId != -1)
                SecurityContext.SetThreadPrincipal(threadUserId);

            // get user details
            UserInfo user = UserController.GetUserInternally(userId);

            // place log record
            TaskManager.StartTask(taskId, "USER", "DELETE", user.Username, userId);

            try
            {
                // delete user packages
                List<PackageInfo> packages = PackageController.GetMyPackages(userId);

                // delete user packages synchronously
                if (packages.Count > 0)
                {
                    PackageAsyncWorker packageWorker = new PackageAsyncWorker();
                    packageWorker.UserId = SecurityContext.User.UserId;
                    packageWorker.Packages = packages;

                    // invoke worker
                    packageWorker.DeletePackagesServiceItems();
                }

                // delete user from database
                Database.DeleteUser(SecurityContext.User.UserId, userId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion
    }
}
