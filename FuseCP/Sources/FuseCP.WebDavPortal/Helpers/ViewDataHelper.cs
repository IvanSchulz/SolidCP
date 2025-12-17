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

namespace FuseCP.WebDavPortal.Helpers
{
    public class ViewDataHelper
    {
        public static string BytesToSize(long bytes)
        {
            if (bytes == 0)
            {
                return string.Format("0 {0}", Resources.UI.Byte);
            }

            var k = 1024;

            var sizes = new[]
            {
                Resources.UI.Bytes,
                Resources.UI.KilobyteShort,
                Resources.UI.MegabyteShort,
                Resources.UI.GigabyteShort,
                Resources.UI.TerabyteShort,
                Resources.UI.PetabyteShort,
                Resources.UI.ExabyteShort
            };

            var i = (int) Math.Floor(Math.Log(bytes)/Math.Log(k));
            return string.Format("{0} {1}", Math.Round(bytes/Math.Pow(k, i), 3), sizes[i]);
        }
    }
}
