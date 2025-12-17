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

using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.OS;

namespace FuseCP.Tests
{
	[TestClass]
	public class Misc
	{

		public TestContext TestContext { get; set; }

		public void TestSerializeType<T>(T obj)
		{
			DataContractSerializer s = new DataContractSerializer(typeof(T));
			using (MemoryStream fs = new MemoryStream())
			{
				TestContext.WriteLine("Testing for type: {0}", typeof(T));
				s.WriteObject(fs, obj);
				fs.Seek(0, SeekOrigin.Begin);
				object s2 = s.ReadObject(fs);
				if (s2 == null)
				{
					TestContext.WriteLine("  Deserialized object is null");
					Assert.IsNotNull(s2);
				}
				else
				{
					TestContext.WriteLine("  Deserialized type: {0}", s2.GetType());
					Assert.AreEqual<Type>(obj.GetType(), s2.GetType());
				}
				fs.Seek(0, SeekOrigin.Begin);
				var reader = new StreamReader(fs);
				var text = reader.ReadToEnd();
				TestContext.WriteLine(text);
				Assert.IsFalse(text.Contains("settingsHash"));
			}
		}

		[TestMethod]
		public void SerializeServerSettings()
		{
			var setting = new SystemSettings();
			setting.SettingsArray = new string[][] { new string[] { "test", "a" }, new string[] { "test2", "m"} };
			TestSerializeType<SystemSettings>(new SystemSettings());
			Assert.IsTrue(true);
		}

        [TestMethod]
		public void TestWindowsOSVersion() {

			var os = OSInfo.WindowsVersion;

			Debug.WriteLine(os.ToString());
		}

    }
}
