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
using System.Text;

namespace CryptSharp
{
    /// <summary>
    /// Modified versions of the PHPass crypt algorithm.
    /// </summary>
    public enum PhpassCrypterVariant
    {
        /// <summary>
        /// Standard PHPass. WordPress uses this.
        /// </summary>
        Standard,

        /// <summary>
        /// phpBB changes the prefix but the algorithm is otherwise identical.
        /// </summary>
        Phpbb,

        /// <summary>
        /// Drupal 7+ uses SHA512 instead of MD5.
        /// </summary>
        Drupal
    }
}
