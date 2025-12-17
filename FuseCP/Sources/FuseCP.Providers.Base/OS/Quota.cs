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

namespace FuseCP.Providers.OS
{
    public class Quota
    {
        #region Fields

        private int _Size;
        private QuotaType _QuotaType;
        private int _Usage;

        #endregion

        #region Properties

        public int Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        public QuotaType QuotaType
        {
            get { return _QuotaType; }
            set { _QuotaType = value; }
        }

        public int Usage
        {
            get { return _Usage; }
            set { _Usage = value; }
        }

        public long DiskFreeSpaceInBytes { get; set; }

        #endregion

        #region Constructors

        public Quota()
        {
            _Size = -1;
            _QuotaType = QuotaType.Soft;
            _Usage = -1;
        }

        #endregion

        // 05.09.2015 roland.breitschaft@x-company.de
        // Add an Empty-Quota Creator
        public static Quota Empty()
        {
            return new Quota();
        }
    }
}
