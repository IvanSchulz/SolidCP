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
using System.Xml;

namespace FuseCP.Setup
{
	public sealed class XmlUtils
	{
		private XmlUtils()
		{

		}

		/// <summary>
		/// Retrieves the specified attribute from the XML node.
		/// </summary>
		/// <param name="node">XML node.</param>
		/// <param name="attribute">Attribute to retreive.</param>
		/// <returns>Attribute value.</returns>
		internal static string GetXmlAttribute(XmlNode node, string attribute)
		{
			string ret = null;
			if (node != null)
			{
				XmlAttribute xmlAttribute = node.Attributes[attribute];
				if (xmlAttribute != null)
				{
					ret = xmlAttribute.Value;
				}
			}
			return ret;
		}

		internal static void SetXmlAttribute(XmlNode node, string attribute, string value)
		{
			if (node != null)
			{
				XmlAttribute xmlAttribute = node.Attributes[attribute];
				if (xmlAttribute == null)
				{
					xmlAttribute = node.OwnerDocument.CreateAttribute(attribute);
					node.Attributes.Append(xmlAttribute);
				}
				xmlAttribute.Value = value;
			}
		}


		internal static void RemoveXmlNode(XmlNode node)
		{
			if (node != null && node.ParentNode != null)
			{
				node.ParentNode.RemoveChild(node);
			}
		}
	}
}
