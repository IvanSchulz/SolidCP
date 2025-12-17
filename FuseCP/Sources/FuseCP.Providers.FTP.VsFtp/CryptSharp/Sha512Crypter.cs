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

#region License
/*
CryptSharp
Copyright (c) 2013 James F. Bellinger <http://www.zer7.com/software/cryptsharp>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/
#endregion

using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CryptSharp
{
    /// <summary>
    /// SHA512 crypt. A reasonable choice if you cannot use Blowfish crypt for policy reasons.
    /// </summary>
    public class Sha512Crypter : ShaCrypter
    {
        static readonly Regex _regex = CreateDefaultRegex("$6$", 86);

        protected override HashAlgorithm CreateHashAlgorithm()
        {
            return System.Security.Cryptography.SHA512.Create();
        }

        protected override int[] GetCryptPermutation()
        {
            return new[]
            {
                42, 21, 0,
                1, 43, 22,
                23, 2, 44,
                45, 24, 3,
                4, 46, 25,
                26, 5, 47,
                48, 27, 6,
                7, 49, 28,
                29, 8, 50,
                51, 30, 9,
                10, 52, 31,
                32, 11, 53,
                54, 33, 12,
                13, 55, 34,
                35, 14, 56,
                57, 36, 15,
                16, 58, 37,
                38, 17, 59,
                60, 39, 18,
                19, 61, 40,
                41, 20, 62,
                63
            };
        }

        protected override Regex GetRegex()
        {
            return _regex;
        }

        protected override string CryptPrefix
        {
            get { return "$6$"; }
        }
    }
}
