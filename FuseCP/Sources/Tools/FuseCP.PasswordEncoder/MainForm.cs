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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuseCP.PasswordEncoder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            if(CryptoKeyEntered() && ValueEntered())
                Result.Text = CryptoUtils.Encrypt(Value.Text.Trim(), CryptoKey.Text.Trim());
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            if (CryptoKeyEntered() && ValueEntered())
                Result.Text = CryptoUtils.Decrypt(Value.Text.Trim(), CryptoKey.Text.Trim());
        }

        private void Sha1Button_Click(object sender, EventArgs e)
        {
            if (ValueEntered())
                Result.Text = CryptoUtils.SHA1(Value.Text.Trim());
        }
		private void Sha256Button_Click(object sender, EventArgs e)
		{
			if (ValueEntered())
				Result.Text = CryptoUtils.SHA256(Value.Text.Trim());
		}
		private bool CryptoKeyEntered()
        {
            if (CryptoKey.Text.Trim() == "")
            {
                MessageBox.Show("Enter Crypto Key");
                CryptoKey.Focus();
                return false;
            }
            return true;
        }

        private bool ValueEntered()
        {
            if (Value.Text.Trim() == "")
            {
                MessageBox.Show("Enter Value");
                Value.Focus();
                return false;
            }
            return true;
        }
    }
}
