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

using System;
using CryptSharp.Internal;

namespace CryptSharp.Utility
{
    partial class BaseEncoding
    {
        /// <inheritdoc />
        public override int GetMaxCharCount(int byteCount)
        {
            Check.Range("byteCount", byteCount, 0, int.MaxValue);

            return checked((byteCount * 8 + BitsPerCharacter - 1) / BitsPerCharacter);
        }

        /// <inheritdoc />
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            int charCount = GetCharCount(bytes, byteIndex, byteCount);

            return GetChars(bytes, byteIndex, byteCount, chars, charIndex, charCount);
        }

        /// <summary>
        /// Converts bytes from their binary representation to a text representation.
        /// </summary>
        /// <param name="bytes">An input array of bytes.</param>
        /// <param name="byteIndex">The index of the first byte.</param>
        /// <param name="byteCount">The number of bytes to read.</param>
        /// <param name="chars">An output array of characters.</param>
        /// <param name="charIndex">The index of the first character.</param>
        /// <param name="charCount">The number of characters to write.</param>
        /// <returns>The number of characters written.</returns>
        public int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount)
        {
            Check.Bounds("bytes", bytes, byteIndex, byteCount);
            Check.Bounds("chars", chars, charIndex, charCount);

            int byteEnd = checked(byteIndex + byteCount);

            int bitStartOffset = 0;
            for (int i = 0; i < charCount; i++)
            {
                byte value;

                byte thisByte = byteIndex + 0 < byteEnd ? bytes[byteIndex + 0] : (byte)0;
                byte nextByte = byteIndex + 1 < byteEnd ? bytes[byteIndex + 1] : (byte)0;

                int bitEndOffset = bitStartOffset + BitsPerCharacter;
                if (MsbComesFirst)
                {
                    value = BitMath.ShiftRight(thisByte, 8 - bitStartOffset - BitsPerCharacter);
                    if (bitEndOffset > 8)
                    {
                        value |= BitMath.ShiftRight(nextByte, 16 - bitStartOffset - BitsPerCharacter);
                    }
                }
                else
                {
                    value = BitMath.ShiftRight(thisByte, bitStartOffset);
                    if (bitEndOffset > 8)
                    {
                        value |= BitMath.ShiftRight(nextByte, bitStartOffset - 8);
                    }
                }

                bitStartOffset = bitEndOffset;
                if (bitStartOffset >= 8)
                {
                    bitStartOffset -= 8; byteIndex++;
                }

                chars[i] = GetChar(value);
            }

            return charCount;
        }

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            Check.Bounds("bytes", bytes, index, count);

            return GetMaxCharCount(count);
        }
    }
}
