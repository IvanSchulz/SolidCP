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
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Setup.WIXBootstrapper
{
    static class Program
    {
        const string MsiFile = "FuseCP.msi";

        private static string ExtractFromAssembly(string path, string file)
        {
            string strPath = path + file;
            if (File.Exists(strPath)) File.Delete(strPath);

            Assembly assembly = Assembly.GetExecutingAssembly();

            var input = assembly.GetManifestResourceStream("Setup.WIXBootstrapper."+file);
            var output = File.Open(strPath, FileMode.CreateNew);

            CopyStream(input, output);
            input.Dispose();
            output.Dispose();

            return strPath;
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
            }
        }

        [STAThread]
        static void Main()
        {
            string tempPath = Path.GetTempPath();

            string msiFileName = ExtractFromAssembly(tempPath, MsiFile);

            if (!string.IsNullOrEmpty(msiFileName))
                System.Diagnostics.Process.Start(msiFileName);
        }
    }
}
