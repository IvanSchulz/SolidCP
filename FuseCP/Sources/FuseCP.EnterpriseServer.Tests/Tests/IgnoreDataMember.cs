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
using System.Runtime.Serialization;
using FuseCP.EnterpriseServer;
using System.Xml;

namespace FuseCP.Tests
{
	[TestClass]
	public class IgnoreDataMember
	{

		public TestContext TestContext { get; set; }

		public T SerializeType<T>(T obj)
		{
			DataContractSerializer s = new DataContractSerializer(typeof(T));

			using (MemoryStream fs = new MemoryStream())
			{
				TestContext!.WriteLine("Testing for type: {0}", typeof(T));
				var writer = XmlDictionaryWriter.CreateBinaryWriter(fs);
				s.WriteObject(writer, obj);
				writer.Flush();

				fs.Seek(0, SeekOrigin.Begin);

				var reader = XmlDictionaryReader.CreateBinaryReader(fs, new XmlDictionaryReaderQuotas());
				object s2 = s.ReadObject(reader);
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
				/* fs.Seek(0, SeekOrigin.Begin);
				var textReader = new StreamReader(fs);
				var text = textReader.ReadToEnd();
				TestContext.WriteLine(text);
				*/

				return (T)s2;
			}
		}

		[TestMethod]

		public void TestIgnoreDataMember()
		{
			var cntx = new PackageContext();
			var quota = new QuotaValueInfo() { QuotaName = "Test" };
			cntx.Quotas.Add("Test", quota);
			cntx.QuotasArray = new QuotaValueInfo[] { quota };
			Assert.IsTrue(cntx.Quotas.Any());
			Assert.IsTrue(cntx.QuotasArray.Any());
			var copy = SerializeType<PackageContext>(cntx);
			Assert.IsTrue(copy.QuotasArray.Any());
			Assert.IsFalse(copy.Quotas.Any());
		}
	}
}
