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
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;


namespace FuseCP.Portal.VPS2012.guacamole
{
    public class Encryption
    {

        public static string GenerateEncryptionKey()
        {
            var rj = new RijndaelManaged()
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256,
            };
            rj.GenerateKey();
            rj.GenerateIV();

            var key = Convert.ToBase64String(rj.Key);
            var IV = Convert.ToBase64String(rj.IV);
            string strkey = "";
            foreach (var value in key)
            {
                decimal decValue = value;
                strkey = String.Format("{0} {1}", strkey, decValue.ToString());
            }
            return String.Format("{0}:{1}", key, IV);
        }
    }
}
