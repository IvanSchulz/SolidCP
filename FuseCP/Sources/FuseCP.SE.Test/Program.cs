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
using System.Threading.Tasks;
using FuseCP.SE;

namespace FuseCP.SE.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AddDomain test01.expertservices.co");
            SE.AddDomain("test01.expertservices.co", "fjiwoejfow", "test@test01.expertservices.co", null);
            Console.ReadLine();

            Console.WriteLine("AddDomain test01.expertservices.co");
            SE.AddDomain("test01.expertservices.co", "fjiwoejfow", "test@test01.expertservices.co", null);
            Console.ReadLine();


            Console.WriteLine("AddEmail test@test01.expertservices.co Aa123123~");
            SE.AddEmail("test", "test01.expertservices.co", "Aa123123~");
            Console.ReadLine();

            Console.WriteLine("AddEmail test@test01.expertservices.co Aa123123~");
            SE.AddEmail("test", "test01.expertservices.co", "Aa123123~");
            Console.ReadLine();

            Console.WriteLine("SetEmailPassword test@test01.expertservices.co Bb123123~");
            SE.SetEmailPassword("test@test01.expertservices.co", "Bb123123~");

            Console.WriteLine("DeleteEmail test@test01.expertservices.co");
            SE.DeleteEmail("test@test01.expertservices.co");
            Console.ReadLine();

            Console.WriteLine("DeleteEmail test@test01.expertservices.co");
            SE.DeleteEmail("test@test01.expertservices.co");
            Console.ReadLine();

            Console.WriteLine("DeleteDomain test01.expertservices.co");
            SE.DeleteDomain("test01.expertservices.co");
            Console.ReadLine();

            Console.WriteLine("DeleteDomain test01.expertservices.co");
            SE.DeleteDomain("test01.expertservices.co");
            Console.ReadLine();
        }
    }
}
