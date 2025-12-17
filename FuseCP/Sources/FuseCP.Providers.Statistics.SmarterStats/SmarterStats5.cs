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
    class SmarterStats5 : SmarterStats
    {
        public override bool IsInstalled()
        {
            string productName = null, productVersion = null;
            
            // Check x86 platform
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key != null)
            {
                var names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = key.OpenSubKey(s);
                    //
                    if (subkey == null)
                        continue;
                    //
                    productName = subkey.GetValue("DisplayName") as String;
                    //
                    if (String.IsNullOrEmpty(productName))
                        continue;

                    if (productName.Equals("SmarterStats")
                        || productName.Equals("SmarterStats Service"))
                    {
                        productVersion = subkey.GetValue("DisplayVersion") as String;
                        goto Version_Match;
                    }
                }
            }

            // Check x64 platform
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key != null)
            {
                var names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = key.OpenSubKey(s);
                    //
                    if (subkey == null)
                        continue;
                    //
                    productName = subkey.GetValue("DisplayName") as String;
                    //
                    if (String.IsNullOrEmpty(productName))
                        continue;

                    if (productName.Equals("SmarterStats") 
                        || productName.Equals("SmarterStats Service"))
                    {
                        productVersion = subkey.GetValue("DisplayVersion") as String;
                        goto Version_Match;
                    }
                }
            }
    
    Version_Match:
            //
            if (String.IsNullOrEmpty(productVersion))
                return false;
				
			// Match SmarterStats 5.x or newer versions
			int version = 0;
			string[] split = productVersion.Split(new[] { '.' });

			if (int.TryParse(split[0], out version))
			{
				if(version >= 5)
					return true;
			}
			//
			
            return false;
        }
    }
}

