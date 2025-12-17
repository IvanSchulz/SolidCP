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
using System.IO;
using System.Collections.Generic;

namespace FuseCP.LinuxVmConfig
{
    public class TxtHelper
    {
        public static void ReplaceStr(string filePath, string findStr, string replaceStr)
        {
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                List<string> list = new List<string>();
                foreach (string str in listIE)
                {
                    string res = str.Replace(findStr, replaceStr, StringComparison.Ordinal);
                    list.Add(res);
                }
                File.WriteAllLines(filePath, list);
            }
            catch (Exception ex) {
                Log.WriteError("ReplaceStr error: " + ex.ToString());
            }
        }

        public static void ReplaceStr(string filePath, string newStr, int pos)
        {
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                List<string> list = new List<string>();
                int count = -1;
                foreach (string str in listIE)
                {
                    count++;
                    if (count == pos)
                    {
                        list.Add(newStr);
                    }
                    else
                    {
                        list.Add(str);
                    }
                }
                if (pos == -1) list.Add(newStr);
                File.WriteAllLines(filePath, list);
            }
            catch (Exception ex) {
                Log.WriteError("ReplaceStr error: " + ex.ToString());
            }
        }

        public static void DelStr(string filePath, int pos)
        {
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                List<string> list = new List<string>();
                int count = -1;
                foreach (string str in listIE)
                {
                    count++;
                    if (count != pos) list.Add(str);
                }
                File.WriteAllLines(filePath, list);
            }
            catch (Exception ex) {
                Log.WriteError("DelStr error: " + ex.ToString());
            }
        }

        public static int GetStrPos(string filePath, string findStr, int startPos, int endPos)
        {
            int count = -1;
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                foreach (string str in listIE)
                {
                    count++;
                    if (count < startPos) continue;
                    if (endPos != -1 && count > endPos) break;
                    if (str.Contains(findStr))
                    {
                        return count;
                    }
                }
            }
            catch (Exception ex) {
                Log.WriteError("GetStrPos error: " + ex.ToString());
            }
            return -1;
        }

        public static string GetStr(string filePath, string findStr, int startPos, int endPos)
        {
            int count = -1;
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                foreach (string str in listIE)
                {
                    count++;
                    if (count < startPos) continue;
                    if (endPos != -1 && count > endPos) break;
                    if (str.Contains(findStr))
                    {
                        return str;
                    }
                }
            }
            catch (Exception ex) {
                Log.WriteError("GetStr error: " + ex.ToString());
            }
            return null;
        }

        public static void ReplaceAllStr(string filePath, List<string> strList, int startPos, int endPos)
        {
            int count = -1;
            bool added = false;
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                List<string> newList = new List<string>();
                foreach (string str in listIE)
                {
                    count++;
                    if (count < startPos || (count > endPos && endPos != -1))
                    {
                        newList.Add(str);
                    }
                    else
                    {
                        if (!added)
                        {
                            added = true;
                            foreach (string newStr in strList)
                            {
                                newList.Add(newStr);
                            }
                        }
                    }
                }
                File.WriteAllLines(filePath, newList);
            }
            catch (Exception ex) {
                Log.WriteError("ReplaceAllStr error: " + ex.ToString());
            }
        }
    }
}
