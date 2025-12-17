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
using System.Linq;
using System.Text;

namespace FuseCP.Providers.Mail.SM9.Extensions
{
	public static class MailAccountExtensions
	{
		/// <summary>
		/// Prepares all the necessary parameters to call SetUserSettings web method.
		/// </summary>
		/// <param name="mailbox"></param>
		/// <returns></returns>
		public static string[] PrepareSetRequestedUserSettingsWebMethodParams(this MailAccount mailbox)
		{
			return new string[] {
                        "isenabled=" + mailbox.Enabled.ToString(),
						// Fix for incorrect mailbox size
                        "maxsize=" + (mailbox.UnlimitedSize ? "0" : mailbox.MaxMailboxSize.ToString()),
                        "lockpassword=" + mailbox.PasswordLocked.ToString(),
						"passwordlocked" + mailbox.PasswordLocked.ToString(),
                        "replytoaddress=" + (mailbox.ReplyTo != null ? mailbox.ReplyTo : ""),
                        "signature=" + (mailbox.Signature != null ? mailbox.Signature : ""),
						"spamforwardoption=none",
                        // Set UTF8 as default encoding for webmail
                        "textencoding=65001"
			};
		}
	}
}
