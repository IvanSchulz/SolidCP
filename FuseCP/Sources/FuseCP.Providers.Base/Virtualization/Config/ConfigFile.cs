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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace FuseCP.Providers.Virtualization
{
    public class ConfigFile
    {
        const string resultTemplate = @"
<?xml version=""1.0""?>
<items>
    {0}
</items>";

        const string itemTemplate = @"
    <item path=""{0}"" legacyNetworkAdapter=""{1}"" remoteDesktop=""{2}"" processVolume=""{3}"" 
                        generation=""{11}"" enableSecureBoot=""{12}"" secureBootTemplate=""{16}"" vhdBlockSizeBytes=""{13}"" diskSize=""{17}"">
        <name>{4}</name>
        <description>{5}</description>
        <DeployScriptParams>{10}</DeployScriptParams>
        <provisioning>
            {6}
            <vmconfig computerName=""{7}"" administratorPassword=""{8}"" networkAdapters=""{9}"" 
                cdKey=""{14}"" timeZoneId=""{15}"" />
        </provisioning>
    </item>";

        const string sysprepTemplate = @"<sysprep file=""{0}""/>";

        public ConfigFile(string xml)
        {
            xml = xml?.Trim();
            Xml = xml;
            Load(xml);
        }

        public ConfigFile(LibraryItem[] libraryItems)
        {
            LibraryItems = libraryItems;
            Load(libraryItems);
        }


        public LibraryItem[] LibraryItems { get; private set; }

        public string Xml { get; private set; }


        private void Load(string xmlText)
        {
            // create list
            List<LibraryItem> items = new List<LibraryItem>();

            if (string.IsNullOrEmpty(xmlText))
            {
                LibraryItems = items.ToArray();
                return;
            }

            // load xml
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlText);

            XmlNodeList nodeItems = xml.SelectNodes("/items/item");

            foreach (XmlNode nodeItem in nodeItems)
            {
                LibraryItem item = new LibraryItem();
                if (nodeItem.Attributes["path"] != null)
                    item.Path = nodeItem.Attributes["path"].Value;

                if (nodeItem.Attributes["generation"] != null)
                    item.Generation = Int32.Parse(nodeItem.Attributes["generation"].Value);

                // optional attributes
                item.EnableSecureBoot = true;
                if (nodeItem.Attributes["enableSecureBoot"] != null)
                    item.EnableSecureBoot = Boolean.Parse(nodeItem.Attributes["enableSecureBoot"].Value);

                item.SecureBootTemplate = "";
                if (nodeItem.Attributes["secureBootTemplate"] != null)
                    item.SecureBootTemplate = nodeItem.Attributes["secureBootTemplate"].Value;

                item.VhdBlockSizeBytes = 0;
                if (nodeItem.Attributes["vhdBlockSizeBytes"] != null)
                    item.VhdBlockSizeBytes = UInt32.Parse(nodeItem.Attributes["vhdBlockSizeBytes"].Value);

                if (nodeItem.Attributes["diskSize"] != null)
                    item.DiskSize = Int64.Parse(nodeItem.Attributes["diskSize"].Value);

                if (nodeItem.Attributes["DeployScriptParams"] != null)
                    item.DeployScriptParams = nodeItem.Attributes["DeployScriptParams"].Value;

                if (nodeItem.Attributes["legacyNetworkAdapter"] != null)
                    item.LegacyNetworkAdapter = Boolean.Parse(nodeItem.Attributes["legacyNetworkAdapter"].Value);

                item.ProcessVolume = 0; // process (extend and sysprep) 1st volume by default
                if (nodeItem.Attributes["processVolume"] != null)
                    item.ProcessVolume = Int32.Parse(nodeItem.Attributes["processVolume"].Value);

                if (nodeItem.Attributes["remoteDesktop"] != null)
                    item.RemoteDesktop = Boolean.Parse(nodeItem.Attributes["remoteDesktop"].Value);

                // inner nodes
                item.Name = nodeItem.SelectSingleNode("name").InnerText;
                var descriptionNode = nodeItem.SelectSingleNode("description");
                if (descriptionNode != null)
                    item.Description = descriptionNode.InnerText;

                var DeployScriptParamsNode = nodeItem.SelectSingleNode("DeployScriptParams");
                if (DeployScriptParamsNode != null)
                    item.DeployScriptParams = DeployScriptParamsNode.InnerText;


                // sysprep files
                XmlNodeList nodesSyspep = nodeItem.SelectNodes("provisioning/sysprep");
                List<string> sysprepFiles = new List<string>();
                foreach (XmlNode nodeSyspep in nodesSyspep)
                {
                    if (nodeSyspep.Attributes["file"] != null)
                        sysprepFiles.Add(nodeSyspep.Attributes["file"].Value);
                }
                item.SysprepFiles = sysprepFiles.ToArray();

                // vmconfig
                XmlNode nodeVmConfig = nodeItem.SelectSingleNode("provisioning/vmconfig");
                if (nodeVmConfig != null)
                {
                    if (nodeVmConfig.Attributes["computerName"] != null)
                        item.ProvisionComputerName = Boolean.Parse(nodeVmConfig.Attributes["computerName"].Value);

                    if (nodeVmConfig.Attributes["administratorPassword"] != null)
                        item.ProvisionAdministratorPassword =
                            Boolean.Parse(nodeVmConfig.Attributes["administratorPassword"].Value);

                    if (nodeVmConfig.Attributes["networkAdapters"] != null)
                        item.ProvisionNetworkAdapters = Boolean.Parse(nodeVmConfig.Attributes["networkAdapters"].Value);

                    item.CDKey = "";
                    if (nodeVmConfig.Attributes["cdKey"] != null)
                        item.CDKey = nodeVmConfig.Attributes["cdKey"].Value;

                    item.TimeZoneId = ""; //default value
                    if (nodeVmConfig.Attributes["timeZoneId"] != null)
                        item.TimeZoneId = nodeVmConfig.Attributes["timeZoneId"].Value;
                }

                items.Add(item);
            }

            LibraryItems = items.ToArray();
        }

        private void Load(LibraryItem[] libraryItems)
        {
            List<string> items = new List<string>();

            foreach (var libraryItem in libraryItems)
            {
                var sysprep = "";

                if (libraryItem.SysprepFiles != null && libraryItem.SysprepFiles.Any())
                    sysprep = string.Join("", libraryItem.SysprepFiles.Select(s => string.Format(sysprepTemplate, s)).ToArray());

                items.Add(string.Format(itemTemplate, libraryItem.Path, libraryItem.LegacyNetworkAdapter,
                    libraryItem.RemoteDesktop, libraryItem.ProcessVolume, libraryItem.Name, libraryItem.Description,
                    sysprep, libraryItem.ProvisionComputerName, libraryItem.ProvisionAdministratorPassword,
                    libraryItem.ProvisionNetworkAdapters, libraryItem.DeployScriptParams, libraryItem.Generation,
                    libraryItem.EnableSecureBoot, libraryItem.VhdBlockSizeBytes, libraryItem.CDKey, libraryItem.TimeZoneId, 
                    libraryItem.SecureBootTemplate, libraryItem.DiskSize));
            }

            Xml = string.Format(resultTemplate, string.Join("", items.ToArray()));
        }
    }
}
