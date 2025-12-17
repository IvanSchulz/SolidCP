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

namespace FuseCP.Providers.Mail
{
	/// <summary>
	/// Summary description for MailboxRights.
	/// </summary>
	public enum ReplyTo 
	{ 
		RepliesToList = 0, 
		RepliesToSender = 1, 
		RepliesToModerator = 2
    }

    #region MailEnable
    public enum PostingMode
	{ 
		MembersCanPost = 0,
		AnyoneCanPost = 1,
		PasswordProtectedPosting = 2,
        ModeratorCanPost = 3
	}

    public enum PrefixOption
    {
        Default = 0,
        Altered = 1,
        CustomPrefix = 2
    }
    #endregion

    #region Merak
    public enum PasswordProtection
    {
        NoProtection = 0,
        ClientModerated = 1,
        ServerModerated = 2
    }

    #endregion

    #region IceWarp

    public enum IceWarpListMembersSource
    {
        MembersInFile = 0,
        AllDomainUsers = 1,
        AllDomainAdmins = 3
    }

    public enum IceWarpListFromAndReplyToHeader
    {
        NoChange = 0,
        SetToSender = 1,
        SetToValue = 2
    }

    public enum IceWarpListOriginator
    {
        Blank = 0,
        Sender = 1,
        Owner = 2
    }

    [Flags]
    public enum IceWarpListDefaultRights
    {
        Receive = 1,
        Post = 2,
        Digest = 4
    }

    public enum IceWarpListConfirmSubscription
    {
        None = 0,
        User = 1,
        Owner = 2
    }
    #endregion
}
