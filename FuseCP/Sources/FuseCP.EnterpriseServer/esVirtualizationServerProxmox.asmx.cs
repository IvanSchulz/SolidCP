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
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using FuseCP.Web.Services;
using System.ComponentModel;
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers;
using FuseCP.Providers.Virtualization;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esVirtualizationServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esVirtualizationServerProxmox: WebService
    {
        #region Virtual Machines
        [WebMethod]
        public VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachines(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        [WebMethod]
        public VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachinesByServiceId(serviceId);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineItem(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachineByItemId(itemId);
        }

        [WebMethod]
        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            if (SecurityContext.CheckAccount(DemandAccount.IsActive | DemandAccount.IsAdmin | DemandAccount.NotDemo) != 0)
                throw new Exception("This method could be called by serveradmin only.");

            return VirtualizationServerControllerProxmox.EvaluateVirtualMachineTemplate(itemId, false, false, template);
        }
        #endregion

        #region External Network
        [WebMethod]
        public int GetExternalNetworkVLAN(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetExternalNetworkVLAN(itemId);
        }

        [WebMethod]
        public NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return VirtualizationServerControllerProxmox.GetExternalNetworkDetails(packageId);
        }
        #endregion

        #region Private Network
        [WebMethod]
        public PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerControllerProxmox.GetPackagePrivateIPAddressesPaged(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<PrivateIPAddress> GetPackagePrivateIPAddresses(int packageId)
        {
            return VirtualizationServerControllerProxmox.GetPackagePrivateIPAddresses(packageId);
        }

        [WebMethod]
        public NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return VirtualizationServerControllerProxmox.GetPrivateNetworkDetails(packageId);
        }
        #endregion

        #region User Permissions
        [WebMethod]
        public List<VirtualMachinePermission> GetSpaceUserPermissions(int packageId)
        {
            return VirtualizationServerControllerProxmox.GetSpaceUserPermissions(packageId);
        }

        [WebMethod]
        public int UpdateSpaceUserPermissions(int packageId, VirtualMachinePermission[] permissions)
        {
            return VirtualizationServerControllerProxmox.UpdateSpaceUserPermissions(packageId, permissions);
        }
        #endregion

        #region Audit Log
        [WebMethod]
        public List<LogRecord> GetSpaceAuditLog(int packageId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerControllerProxmox.GetSpaceAuditLog(packageId, startPeriod, endPeriod,
                severity, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<LogRecord> GetVirtualMachineAuditLog(int itemId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachineAuditLog(itemId, startPeriod, endPeriod,
                severity, sortColumn, startRow, maximumRows);
        }
        #endregion

        #region VPS Create – Name & OS
        [WebMethod]
        public LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            return VirtualizationServerControllerProxmox.GetOperatingSystemTemplates(packageId);
        }

        [WebMethod]
        public LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return VirtualizationServerControllerProxmox.GetOperatingSystemTemplatesByServiceId(serviceId);
        }
        #endregion

        #region VPS Create - Configuration
        [WebMethod]
        public int GetMaximumCpuCoresNumber(int packageId)
        {
            return VirtualizationServerControllerProxmox.GetMaximumCpuCoresNumber(packageId);
        }

        [WebMethod]
        public string GetDefaultExportPath(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetDefaultExportPath(itemId);
        }
        #endregion

        #region VPS Create
        [WebMethod]
        public IntResult CreateDefaultVirtualMachine(int packageId,
            string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return VirtualizationServerControllerProxmox.CreateDefaultVirtualMachine(packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        [WebMethod]
        public IntResult CreateVirtualMachine(int packageId,
                string hostname, string osTemplateId, string password, string summaryLetterEmail,
                int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock,
                bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
                bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, VirtualMachine otherSettings)
        {
            return VirtualizationServerControllerProxmox.CreateVirtualMachine(packageId,
                hostname, osTemplateId, password, summaryLetterEmail,
                cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock,
                startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses,
                privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
        }
        #endregion

        #region VPS - Import
        [WebMethod]
        public IntResult ImportVirtualMachine(int packageId,
            int serviceId, string vmId,
            string osTemplateFile, string adminPassword,
            bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
            string externalNicMacAddress, int[] externalAddresses,
            string managementNicMacAddress, int managementAddress)
        {
            return VirtualizationServerControllerProxmox.ImportVirtualMachine(packageId,
                serviceId, vmId,
                osTemplateFile, adminPassword,
                startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                externalNicMacAddress, externalAddresses,
                managementNicMacAddress, managementAddress);
        }
        #endregion

        #region VPS – General
        [WebMethod]
        public ImageFile GetVirtualMachineThumbnail(int itemId, ThumbnailSize size)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachineThumbnail(itemId, size);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachineGeneralDetails(itemId);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachineExtendedInfo(serviceId, vmId);
        }

        [WebMethod]
        public int CancelVirtualMachineJob(string jobId)
        {
            return VirtualizationServerControllerProxmox.CancelVirtualMachineJob(jobId);
        }

        [WebMethod]
        public ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return VirtualizationServerControllerProxmox.UpdateVirtualMachineHostName(itemId, hostname, updateNetBIOS);
        }

        [WebMethod]
        public ResultObject ChangeVirtualMachineState(int itemId, VirtualMachineRequestedState state)
        {
            return VirtualizationServerControllerProxmox.ChangeVirtualMachineStateExternal(itemId, state);
        }


        [WebMethod]
        public List<ConcreteJob> GetVirtualMachineJobs(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachineJobs(itemId);

        }

        [WebMethod]
        public VncCredentials GetPveVncCredentials(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetPveVncCredentials(itemId);
        }
        #endregion

        #region VPS - Configuration
        [WebMethod]
        public ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return VirtualizationServerControllerProxmox.ChangeAdministratorPassword(itemId, password);
        }
        #endregion

        #region VPS – Edit Configuration
        [WebMethod]
        public ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots,
                    bool dvdInstalled, bool bootFromCD, bool numLock,
                    bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                    bool externalNetworkEnabled,
                    bool privateNetworkEnabled, VirtualMachine otherSettings)
        {
            return VirtualizationServerControllerProxmox.UpdateVirtualMachineConfiguration(
                    itemId, cpuCores, ramMB, hddGB, snapshots,
                    dvdInstalled, bootFromCD, numLock,
                    startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                    externalNetworkEnabled, privateNetworkEnabled,
                    otherSettings);
        }
        #endregion

        #region DVD
        [WebMethod]
        public LibraryItem GetInsertedDvdDisk(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetInsertedDvdDisk(itemId);
        }

        [WebMethod]
        public LibraryItem[] GetLibraryDisks(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetLibraryDisks(itemId);
        }

        [WebMethod]
        public ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return VirtualizationServerControllerProxmox.InsertDvdDisk(itemId, isoPath);
        }

        [WebMethod]
        public ResultObject EjectDvdDisk(int itemId)
        {
            return VirtualizationServerControllerProxmox.EjectDvdDisk(itemId);
        }
        #endregion

        #region Snaphosts
        [WebMethod]
        public VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachineSnapshots(itemId);
        }

        [WebMethod]
        public VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return VirtualizationServerControllerProxmox.GetSnapshot(itemId, snaphostId);
        }

        [WebMethod]
        public ResultObject CreateSnapshot(int itemId)
        {
            return VirtualizationServerControllerProxmox.CreateSnapshot(itemId);
        }

        [WebMethod]
        public ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return VirtualizationServerControllerProxmox.ApplySnapshot(itemId, snapshotId);
        }

        [WebMethod]
        public ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return VirtualizationServerControllerProxmox.RenameSnapshot(itemId, snapshotId, newName);
        }

        [WebMethod]
        public ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return VirtualizationServerControllerProxmox.DeleteSnapshot(itemId, snapshotId);
        }

        [WebMethod]
        public ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return VirtualizationServerControllerProxmox.DeleteSnapshotSubtree(itemId, snapshotId);
        }

        [WebMethod]
        public ImageFile GetSnapshotThumbnail(int itemId, string snapshotId, ThumbnailSize size)
        {
            return VirtualizationServerControllerProxmox.GetSnapshotThumbnail(itemId, snapshotId, size);
        }
        #endregion

        #region VPS - External Network
        [WebMethod]
        public NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetExternalNetworkAdapterDetails(itemId);
        }

        [WebMethod]
        public ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom,
            int addressesNumber, int[] addressId)
        {
            return VirtualizationServerControllerProxmox.AddVirtualMachineExternalIPAddresses(itemId, selectRandom,
                addressesNumber, addressId, true, -1);
        }

        [WebMethod]
        public ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return VirtualizationServerControllerProxmox.SetVirtualMachinePrimaryExternalIPAddress(itemId, addressId, true);
        }

        [WebMethod]
        public ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return VirtualizationServerControllerProxmox.DeleteVirtualMachineExternalIPAddresses(itemId, addressId, true);
        }
        #endregion

        #region VPS – Private Network
        [WebMethod]
        public NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetPrivateNetworkAdapterDetails(itemId);
        }

        [WebMethod]
        public ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom,
            int addressesNumber, string[] addresses)
        {
            return VirtualizationServerControllerProxmox.AddVirtualMachinePrivateIPAddresses(itemId, selectRandom,
                addressesNumber, addresses, true);
        }

        [WebMethod]
        public ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return VirtualizationServerControllerProxmox.SetVirtualMachinePrimaryPrivateIPAddress(itemId, addressId, true);
        }

        [WebMethod]
        public ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return VirtualizationServerControllerProxmox.DeleteVirtualMachinePrivateIPAddresses(itemId, addressId, true);
        }
        #endregion

        #region Virtual Machine Permissions
        [WebMethod]
        public List<VirtualMachinePermission> GetVirtualMachinePermissions(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachinePermissions(itemId);
        }

        [WebMethod]
        public int UpdateVirtualMachineUserPermissions(int itemId, VirtualMachinePermission[] permissions)
        {
            return VirtualizationServerControllerProxmox.UpdateVirtualMachineUserPermissions(itemId, permissions);
        }
        #endregion

        #region Virtual Switches
        [WebMethod]
        public VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return VirtualizationServerControllerProxmox.GetExternalSwitches(serviceId, computerName);
        }
        #endregion

        #region Tools
        [WebMethod]
        public ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return VirtualizationServerControllerProxmox.DeleteVirtualMachine(itemId, saveFiles, exportVps, exportPath);
        }

        [WebMethod]
        public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return VirtualizationServerControllerProxmox.ReinstallVirtualMachine(itemId, adminPassword, preserveVirtualDiskFiles,
                saveVirtualDisk, exportVps, exportPath);
        }
        #endregion

        #region Help
        [WebMethod]
        public string GetVirtualMachineSummaryText(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetVirtualMachineSummaryText(itemId, false, false);
        }

        [WebMethod]
        public ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return VirtualizationServerControllerProxmox.SendVirtualMachineSummaryLetter(itemId, to, bcc, false);
        }
        #endregion

        #region Replication

        [WebMethod]
        public CertificateInfo[] GetCertificates(int serviceId, string remoteServer)
        {
            return VirtualizationServerControllerProxmox.GetCertificates(serviceId, remoteServer);
        }

        [WebMethod]
        public ResultObject SetReplicaServer(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            return VirtualizationServerControllerProxmox.SetReplicaServer(serviceId, remoteServer, thumbprint, storagePath);
        }

        [WebMethod]
        public ResultObject UnsetReplicaServer(int serviceId, string remoteServer)
        {
            return VirtualizationServerControllerProxmox.UnsetReplicaServer(serviceId, remoteServer);
        }

        [WebMethod]
        public ReplicationServerInfo GetReplicaServer(int serviceId, string remoteServer)
        {
            return VirtualizationServerControllerProxmox.GetReplicaServer(serviceId, remoteServer);
        }

        [WebMethod]
        public VmReplication GetReplication(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetReplication(itemId);
        }

        [WebMethod]
        public ReplicationDetailInfo GetReplicationInfo(int itemId)
        {
            return VirtualizationServerControllerProxmox.GetReplicationInfo(itemId);
        }

        [WebMethod]
        public ResultObject SetVmReplication(int itemId, VmReplication replication)
        {
            return VirtualizationServerControllerProxmox.SetVmReplication(itemId, replication);
        }

        [WebMethod]
        public ResultObject DisableVmReplication(int itemId)
        {
            return VirtualizationServerControllerProxmox.DisableVmReplication(itemId);
        }

        [WebMethod]
        public ResultObject PauseReplication(int itemId)
        {
            return VirtualizationServerControllerProxmox.PauseReplication(itemId);
        }

        [WebMethod]
        public ResultObject ResumeReplication(int itemId)
        {
            return VirtualizationServerControllerProxmox.ResumeReplication(itemId);
        }


        #endregion
    }
}
