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

﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace FuseCP.Providers.Statistics
{
    public class SmarterStats4 : SmarterStats
    {

        public override bool IsInstalled()
        {
            string productName = null;
            string productVersion = null;
            String[] names = null;

            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key != null)
            {
                names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + s);

                    if (subkey != null)
                    {
                        if (!String.IsNullOrEmpty((string)subkey.GetValue("DisplayName")))
                        {
                            productName = (string)subkey.GetValue("DisplayName");
                        }
                        if (productName != null)
                            if (productName.Equals("SmarterStats") || productName.Equals("SmarterStats Service"))
                            {
                                productVersion = (string)subkey.GetValue("DisplayVersion");
                                break;
                            }
                    }
                }

                if (!String.IsNullOrEmpty(productVersion))
                {
                    string[] split = productVersion.Split(new char[] { '.' });
                    return split[0].Equals("4");
                }
            }
                
                //checking x64 platform
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

                if (key != null)
                {
                    names = key.GetSubKeyNames();

                    foreach (string s in names)
                    {
                        RegistryKey subkey =
                            HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + s);

                        if (subkey != null)
                        {
                            if (!String.IsNullOrEmpty((string)subkey.GetValue("DisplayName")))
                            {
                                productName = (string)subkey.GetValue("DisplayName");
                            }
                            if (productName != null)
                                if (productName.Equals("SmarterStats") || productName.Equals("SmarterStats Service"))
                                {
                                    productVersion = (string)subkey.GetValue("DisplayVersion");
                                    break;
                                }
                        }
                    }

                    if (!String.IsNullOrEmpty(productVersion))
                    {
                        string[] split = productVersion.Split(new char[] { '.' });
                        return split[0].Equals("4");
                    }
                }
            
            return false;
        }

    }
}
