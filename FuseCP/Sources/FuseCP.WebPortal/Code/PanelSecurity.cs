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
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

using FuseCP.EnterpriseServer;
using FuseCP.WebPortal;

namespace FuseCP.Portal
{
	/// <summary>
	/// Summary description for PanelSecurity.
	/// </summary>
	public class PanelSecurity
	{

		public static int ParseInt(object val, int defaultValue)
		{
			int result = defaultValue;
			if (val != null && !String.IsNullOrEmpty(val.ToString()))
			{
				try
				{
					result = Int32.Parse(val.ToString());
				}
				catch
				{
					/* do nothing */
				}
			}
			return result;
		}

		public static int PackageId
		{
			get
			{
				HttpRequest request = HttpContext.Current.Request;
				string sSpaceId = request[PortalUtils.SPACE_ID_PARAM];
				if (!String.IsNullOrEmpty(sSpaceId))
				{
					return ParseInt(sSpaceId, 0);
				}
				return 0;
			}
		}

		#region Recently Switched Users
		public static UserInfo[] GetRecentlySwitchedUsers()
		{
			return GetRecentlySwitchedUsersInternal().ToArray();
		}

		private static List<UserInfo> GetRecentlySwitchedUsersInternal()
		{
			List<UserInfo> users = new List<UserInfo>();

			// get existing list
			string[] pairs = GetRecentlySwitchedUsersArray();
			foreach (string pair in pairs)
			{
				string[] parts = pair.Split('=');
				UserInfo user = new UserInfo();
				user.UserId = ParseInt(parts[0], 0);
				user.Username = parts[1];
				users.Add(user);
			}

			return users;
		}

		public static void AddRecentlySwitchedUser(int userId)
		{
			// get existing list
			List<UserInfo> users = GetRecentlySwitchedUsersInternal();

			// check if the user exists
			UserInfo existUser = null;
			foreach (UserInfo user in users)
			{
				if (user.UserId == userId)
				{
					existUser = user;
					break;
				}
			}

			if (existUser != null)
			{
				// move user to the top of the list
				users.Remove(existUser);
				users.Insert(0, existUser);
			}
			else
			{
				// read new user
				UserInfo newUser = ES.Services.Users.GetUserById(userId);
				if (newUser == null)
					return;

				if (users.Count == 10)
				{
					// remove last user
					users.RemoveAt(9);
				}

				// insert new
				users.Insert(0, newUser);
			}

			// save results
			List<string> pairs = new List<string>();
			foreach (UserInfo user in users)
			{
				pairs.Add(user.UserId.ToString() + "=" + user.Username);
			}
			string s = String.Join("*", pairs.ToArray());

			string key = "RecentlySwitchedUsers" + LoggedUserId;
			HttpContext.Current.Items[key] = s;

			HttpCookie cookie = new HttpCookie(key, s);
			cookie.HttpOnly = true;
			HttpContext.Current.Response.Cookies.Remove(key);
			HttpContext.Current.Response.Cookies.Add(cookie);
		}

		private static string[] GetRecentlySwitchedUsersArray()
		{
			string[] users = new string[] { };

			string key = "RecentlySwitchedUsers" + LoggedUserId;
			HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

			if (HttpContext.Current.Items[key] != null)
				users = ((string)HttpContext.Current.Items[key]).Split('*');
			else if (cookie != null)
				users = cookie.Value.Split('*');

			return users;
		}
		#endregion

		#region Selected user
		public static int SelectedUserId
		{
			get
			{
				HttpRequest request = HttpContext.Current.Request;
				string sUserId = request[PortalUtils.USER_ID_PARAM];
				if (!String.IsNullOrEmpty(sUserId))
				{
					return ParseInt(sUserId, 0);
				}
				else
				{
					// try to get from current space
					int spaceId = PackageId;
					if (spaceId > 0)
					{
						// load space
						// check context
						PackageInfo space = (PackageInfo)HttpContext.Current.Items["FuseCPSelectedSpace"];
						if (space != null)
						{
							return space.UserId;
						}
						else
						{
							space = ES.Services.Packages.GetPackage(spaceId);
							if (space != null)
							{
								// place to cache
								HttpContext.Current.Items["FuseCPSelectedSpace"] = space;

								// return
								return space.UserId;
							}
						}
					}
					else
					{
						return EffectiveUserId;
					}
				}
				return 0;
			}
		}

		public static UserInfo SelectedUser
		{
			get
			{
				UserInfo user = (UserInfo)HttpContext.Current.Items["FuseCPSelectedUser"];
				if (user == null)
				{
					try
					{
						user = ES.Services.Users.GetUserById(SelectedUserId);
					}
					catch { }

					// create <empty> user
					if (user == null)
					{
						user = new UserInfo();
						user.UserId = -1;
						user.FirstName = "Unknown";
						user.LastName = "User";
						user.Role = UserRole.User;
						user.IsDemo = true;
						user.Email = "";
						user.Username = "Unknown";
					}

					// add to context
					HttpContext.Current.Items["FuseCPSelectedUser"] = user;
				}
				return user;
			}
		}
		#endregion

		#region Logged user
		public static int LoggedUserId
		{
			get
			{
				return (LoggedUser != null) ? LoggedUser.UserId : 0;
			}
		}

		public static UserInfo LoggedUser
		{
			get
			{
				UserInfo user = null;
				try
				{
					user = (UserInfo)HttpContext.Current.Items["FuseCPLoggedUser"];
				}
				catch { }

				if (user == null)
				{
					// load ES settings
					try
					{
						user = PortalUtils.GetCurrentUser();
					}
					catch { }

					if (user != null)
					{
						// add to context
						HttpContext.Current.Items["FuseCPLoggedUser"] = user;
					}
				}
				return user;
			}
		}
		#endregion

		#region Effective user
		public static int EffectiveUserId
		{
			get
			{
				return (LoggedUser != null && LoggedUser.IsPeer) ? LoggedUser.OwnerId : LoggedUserId;
			}
		}

		public static UserInfo EffectiveUser
		{
			get
			{
				UserInfo user = (UserInfo)HttpContext.Current.Items["FuseCPEffectiveUser"];
				if (user == null)
				{
					user = ES.Services.Users.GetUserById(EffectiveUserId);

					// add to context
					HttpContext.Current.Items["FuseCPEffectiveUser"] = user;
				}
				return user;
			}
		}
		#endregion
	}
}
