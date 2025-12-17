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
using System.Web;

namespace FuseCP.Portal
{
    /// <summary>
    /// Summary description for PanelRequest.
    /// </summary>
    public class PanelRequest
    {
        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        public static int GetInt(string key, int defaultValue)
        {
            int result = defaultValue;
            try { result = Int32.Parse(HttpContext.Current.Request[key]); }
            catch { /* do nothing */ }
            return result;
        }

        public static bool GetBool(string key)
        {
            return GetBool(key, false);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            bool result = defaultValue;
            try { result = bool.Parse(HttpContext.Current.Request[key]); }
            catch { /* do nothing */ }
            return result;
        }

        public static int UserID
        {
            get { return GetInt("UserID"); }
        }

        public static int AccountID
        {
            get { return GetInt("AccountID"); }
        }

        public static int PeerID
        {
            get { return GetInt("PeerID"); }
        }

        public static int ServerId
        {
            get { return GetInt("ServerID"); }
        }

        public static string PoolId
        {
            get { return HttpContext.Current.Request["PoolID"]; }
        }

        public static string DeviceId
        {
            get { return HttpContext.Current.Request["DeviceID"]; }
        }

        public static int ServiceId
        {
            get { return GetInt("ServiceID"); }
        }

        public static string TaskID
        {
            get { return HttpContext.Current.Request["TaskID"]; }
        }

        public static int GroupID
        {
            get { return GetInt("GroupID"); }
        }

        public static int AddressID
        {
            get { return GetInt("AddressID"); }
        }

        public static int VlanID
        {
            get { return GetInt("VlanID"); }
        }

        public static string Addresses
        {
            get { return HttpContext.Current.Request["Addresses"]; }
        }

        public static int ResourceID
        {
            get { return GetInt("ResourceID"); }
        }

        public static int PlanID
        {
            get { return GetInt("PlanID"); }
        }

        public static int AddonID
        {
            get { return GetInt("AddonID"); }
        }

        public static int PackageID
        {
            get { return GetInt("PackageID", -1); }
        }

        public static int PackageAddonID
        {
            get { return GetInt("PackageAddonID"); }
        }

        public static int ItemID
        {
            get { return GetInt("ItemID"); }
        }

        public static int RegistrationID
        {
            get { return GetInt("RegistrationID"); }
        }

        public static int DomainID
        {
            get { return GetInt("DomainID"); }
        }

        public static int InstallationID
        {
            get { return GetInt("InstallationID"); }
        }

        public static int ScheduleID
        {
            get { return GetInt("ScheduleID"); }
        }

        public static string RecordID
        {
            get { return HttpContext.Current.Request["RecordID"]; }
        }

        public static string InstanceID
        {
            get { return HttpContext.Current.Request["InstanceID"]; }
        }


        public static string VirtDir
        {
            get { return HttpContext.Current.Request["VirtDir"] != null ? HttpContext.Current.Request["VirtDir"].Trim().Replace("__DOT__", ".") : ""; }
        }

        public static string Path
        {
            get { return HttpContext.Current.Request["Path"] != null ? HttpContext.Current.Request["Path"] : ""; }
        }

        public static string ApplicationID
        {
            get { return HttpContext.Current.Request["ApplicationID"] != null ? HttpContext.Current.Request["ApplicationID"].Trim() : ""; }
        }

        public static string Name
        {
            get { return HttpContext.Current.Request["Name"] != null
                ? HttpContext.Current.Request["Name"].Trim() : ""; }
        }

        public static string Context
        {
            get { return HttpContext.Current.Request["Context"]; }
        }

        public static string FolderID
        {
            get { return HttpContext.Current.Request["FolderID"] ?? ""; }
        }

        public static string Ctl
        {
            get { return HttpContext.Current.Request["ctl"] ?? ""; }
        }

        public static int CollectionID
        {
            get { return GetInt("CollectionId"); }
        }

        public static int SsLevelId
        {
            get { return GetInt("SsLevelId"); }
        }

        public static int StorageSpaceId
        {
            get { return GetInt("StorageSpaceId"); }
        }
    }
}
