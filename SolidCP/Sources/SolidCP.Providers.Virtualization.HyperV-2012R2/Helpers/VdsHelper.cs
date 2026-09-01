using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;
using Microsoft.Storage.Vds;
using Microsoft.Storage.Vds.Advanced;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Virtualization.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Path = System.IO.Path;


namespace SolidCP.Providers.Virtualization
{
    public class VdsHelper
    {
        private const string GPT_BASIC_DATA_PARTITION = "{ebd0a0a2-b9e5-4433-87c0-68b6b72699c7}";

        private MiManager _miCim;
        private MiManager _miStorage;
        private FileSystemHelper _fileSystemHelper;
        private string _serverName;

        public VdsHelper(MiManager mi, FileSystemHelper fileSystemHelper)
        {
            _serverName = mi.TargetComputer;
            _miCim = new MiManager(mi, Constants.WMI_CIMV2_NAMESPACE);
            _miStorage = new MiManager(mi, Constants.WMI_STORAGE_NAMESPACE);
            _fileSystemHelper = fileSystemHelper;
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            // find mounted disk using VDS
            AdvancedDisk advancedDisk = null;
            Pack diskPack = null;

            FindVdsDisk(diskAddress, out advancedDisk, out diskPack);

            if (advancedDisk == null)
                throw new Exception("Could not find mounted disk");

            // find volume
            Volume diskVolume = null;
            foreach (Volume volume in diskPack.Volumes)
            {
                if (volume.DriveLetter.ToString() == volumeName)
                {
                    diskVolume = volume;
                    break;
                }
                volume?.Dispose();
            }

            if (diskVolume == null)
                throw new Exception("Could not find disk volume: " + volumeName);

            // determine maximum available space
            ulong oneMegabyte = 1048576;
            ulong freeSpace = 0;
            foreach (DiskExtent extent in advancedDisk.Extents)
            {
                if (extent.Type != Microsoft.Storage.Vds.DiskExtentType.Free)
                    continue;

                if (extent.Size > oneMegabyte)
                    freeSpace += extent.Size;
            }

            if (freeSpace == 0)
                return;

            // input disk
            InputDisk inputDisk = new InputDisk();
            foreach (VolumePlex plex in diskVolume.Plexes)
            {
                inputDisk.DiskId = advancedDisk.Id;
                inputDisk.Size = freeSpace;
                inputDisk.PlexId = plex.Id;

                foreach (DiskExtent extent in plex.Extents)
                    inputDisk.MemberIndex = extent.MemberIndex;

                break;
            }

            // extend volume
            Async extendEvent = diskVolume.BeginExtend(new InputDisk[] { inputDisk }, null, null);
            while (!extendEvent.IsCompleted)
                System.Threading.Thread.Sleep(100);
            diskVolume.EndExtend(extendEvent);

            advancedDisk?.Dispose();
            diskPack?.Dispose();
        }

        public MountedDiskInfo GetMountedDiskInfo(int driveNumber)
        {
            MountedDiskInfo diskInfo = new MountedDiskInfo { DiskNumber = driveNumber };

            // find mounted disk using VDS
            AdvancedDisk advancedDisk = null;
            Pack diskPack = null;

            // first attempt
            Thread.Sleep(3000);
            HostedSolutionLog.LogInfo("Trying to find mounted disk - first attempt");
            FindVdsDisk(diskInfo.DiskNumber, out advancedDisk, out diskPack);

            // second attempt
            if (advancedDisk == null)
            {
                Thread.Sleep(20000);
                HostedSolutionLog.LogInfo("Trying to find mounted disk - second attempt");
                advancedDisk?.Dispose();
                diskPack?.Dispose();
                FindVdsDisk(diskInfo.DiskNumber, out advancedDisk, out diskPack);
            }

            if (advancedDisk == null)
                throw new Exception("Could not find mounted disk");

            // Set disk address
            diskInfo.DiskAddress = advancedDisk.DiskAddress;
            var addressParts = diskInfo.DiskAddress.ParseExact("Port{0}Path{1}Target{2}Lun{3}");
            var portNumber = addressParts[0];
            var targetId = addressParts[2];
            var lun = addressParts[3];

            // check if DiskPart must be used to bring disk online and clear read-only flag
            bool useDiskPartToClearReadOnly = false;
            if (ConfigurationManager.AppSettings[Constants.CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG] != null)
                useDiskPartToClearReadOnly = Boolean.Parse(ConfigurationManager.AppSettings[Constants.CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG]);

            // determine disk index for DiskPart
            CimInstance objDisk = _miCim.GetCimInstanceWithSelect(
                    "win32_diskdrive",
                    "Index", // Select only "Index"
                    "Model='Msft Virtual Disk SCSI Disk Device' and ScsiTargetID={0} and ScsiLogicalUnit={1} and scsiPort={2}",
                    targetId, lun, portNumber
                );
            using (objDisk)
            {
                if (useDiskPartToClearReadOnly)
                {
                    // *** Clear Read-Only and bring disk online with DiskPart ***
                    HostedSolutionLog.LogInfo("Clearing disk Read-only flag and bringing disk online");

                    if (objDisk != null && objDisk.CimInstanceProperties["Index"]?.Value != null)
                    {
                        // disk found
                        // run DiskPart
                        string diskPartResult = RunDiskPart(
                            String.Format(@"select disk {0}
                                    attributes disk clear readonly
                                    online disk
                                    exit", Convert.ToInt32(objDisk.CimInstanceProperties["Index"].Value)));

                        HostedSolutionLog.LogInfo("DiskPart Result: " + diskPartResult);
                    }
                }
                else
                {
                    // *** Clear Read-Only and bring disk online with VDS ***
                    // clear Read-Only
                    if ((advancedDisk.Flags & DiskFlags.ReadOnly) == DiskFlags.ReadOnly)
                    {
                        HostedSolutionLog.LogInfo("Clearing disk Read-only flag");
                        advancedDisk.ClearFlags(DiskFlags.ReadOnly);
                        while ((advancedDisk.Flags & DiskFlags.ReadOnly) == DiskFlags.ReadOnly)
                        {
                            Thread.Sleep(100);
                            advancedDisk.Refresh();
                        }
                    }

                    // bring disk ONLINE
                    if (advancedDisk.Status == DiskStatus.Offline)
                    {
                        HostedSolutionLog.LogInfo("Bringing disk online");
                        advancedDisk.Online();
                        while (advancedDisk.Status == DiskStatus.Offline)
                        {
                            Thread.Sleep(100);
                            advancedDisk.Refresh();
                        }
                    }
                }

                // small pause after getting disk online
                Thread.Sleep(3000);

                //dispose objects get disk again
                advancedDisk?.Dispose();
                diskPack?.Dispose();
                FindVdsDisk(diskInfo.DiskNumber, out advancedDisk, out diskPack);

                // find volumes using VDS
                List<string> volumes = new List<string>();
                HostedSolutionLog.LogInfo("Querying disk volumes with VDS");
                foreach (Volume volume in diskPack.Volumes)
                {
                    // VDS returns '\0' or ' ' when the volume has no drive letter, so never compare the string against ""
                    if (Char.IsLetter(volume.DriveLetter))
                        volumes.Add(volume.DriveLetter.ToString());
                }

                // assign drive letters if the volumes have none
                if (volumes.Count == 0)
                    volumes.AddRange(AssignMissingDriveLetters(diskInfo.DiskNumber));

                // find volumes using WMI
                if (volumes.Count == 0 && objDisk != null)
                {
                    HostedSolutionLog.LogInfo("Querying disk volumes with WMI");
                    var partitions = _miCim.EnumerateAssociatedInstances(objDisk, "Win32_DiskDriveToDiskPartition", "Win32_DiskPartition");
                    foreach (var objPartition in partitions)
                    {
                        var logicalDisks = _miCim.EnumerateAssociatedInstances(objPartition, "Win32_LogicalDiskToPartition", "Win32_LogicalDisk");
                        foreach (var objVolume in logicalDisks)
                        {
                            if (objVolume.CimInstanceProperties["Name"]?.Value != null)
                            {
                                volumes.Add(objVolume.CimInstanceProperties["Name"].Value.ToString().TrimEnd(':'));
                            }
                        }
                    }
                }

                HostedSolutionLog.LogInfo(String.Format("Volumes found: {0} ({1})", volumes.Count, String.Join(", ", volumes)));

                // Set volumes
                diskInfo.DiskVolumes = volumes.ToArray();
            }

            // dispose objects
            advancedDisk?.Dispose();
            diskPack?.Dispose();

            return diskInfo;
        }

        /// <summary>
        /// Assigns drive letters to the data partitions of a mounted disk that have none.
        /// Template VHDs frequently carry the GPT attribute "NoDefaultDriveLetter", which suppresses automount.
        /// </summary>
        private List<string> AssignMissingDriveLetters(int diskNumber)
        {
            List<string> assigned = new List<string>();

            HostedSolutionLog.LogInfo("No drive letters assigned - assigning them for disk " + diskNumber);

            foreach (CimInstance partition in _miStorage.QueryCimInstances(
                String.Format("SELECT * FROM MSFT_Partition WHERE DiskNumber = {0}", diskNumber)))
            {
                using (partition)
                {
                    if (!IsDataPartition(partition))
                        continue;

                    int partitionNumber = Convert.ToInt32(partition.CimInstanceProperties["PartitionNumber"].Value);

                    using (var inParams = new CimMethodParametersCollection
                        {
                            CimMethodParameter.Create("AssignDriveLetter", true, CimType.Boolean, CimFlags.In)
                        })
                    using (CimMethodResult result = _miStorage.InvokeMethod(partition, "AddAccessPath", inParams))
                    {
                        UInt32 returnValue = Convert.ToUInt32(result.ReturnValue.Value);
                        if (returnValue != 0)
                        {
                            HostedSolutionLog.LogWarning(String.Format(
                                "Could not assign a drive letter to partition {0}: error {1}", partitionNumber, returnValue));
                            continue;
                        }
                    }

                    Thread.Sleep(3000);

                    using (CimInstance assignedPartition = _miStorage.GetCimInstanceWithSelect(
                        "MSFT_Partition", "DriveLetter", "DiskNumber = {0} AND PartitionNumber = {1}", diskNumber, partitionNumber))
                    {
                        char letter = GetDriveLetter(assignedPartition);

                        if (Char.IsLetter(letter))
                        {
                            HostedSolutionLog.LogInfo(String.Format("Assigned drive letter {0} to partition {1}", letter, partitionNumber));
                            assigned.Add(letter.ToString());
                        }
                        else
                        {
                            HostedSolutionLog.LogWarning("No drive letter reported for partition " + partitionNumber);
                        }
                    }
                }
            }

            return assigned;
        }

        private static bool IsDataPartition(CimInstance partition)
        {
            object isHidden = partition.CimInstanceProperties["IsHidden"]?.Value;
            if (isHidden != null && Convert.ToBoolean(isHidden))
                return false;

            // GptType is empty on MBR disks
            string gptType = partition.CimInstanceProperties["GptType"]?.Value as string;
            if (!String.IsNullOrEmpty(gptType) && !GPT_BASIC_DATA_PARTITION.Equals(gptType, StringComparison.OrdinalIgnoreCase))
                return false;

            // filter out system/recovery partitions
            object size = partition.CimInstanceProperties["Size"]?.Value;
            if (size != null && Convert.ToUInt64(size) < (ulong)Constants.Size1G)
                return false;

            return true;
        }

        private static char GetDriveLetter(CimInstance partition)
        {
            object value = partition?.CimInstanceProperties["DriveLetter"]?.Value;

            if (value is char)
                return (char)value;

            string letter = value as string;
            return String.IsNullOrEmpty(letter) ? '\0' : letter[0];
        }

        public void FindVdsDisk(int driveNumber, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            Func<Disk, bool> compareFunc = disk => disk.Name.EndsWith("PhysicalDrive" + driveNumber);
            FindVdsDisk(compareFunc, out advancedDisk, out diskPack);
        }

        public void FindVdsDisk(string diskAddress, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            Func<Disk, bool> compareFunc = disk => disk.DiskAddress == diskAddress;
            FindVdsDisk(compareFunc, out advancedDisk, out diskPack);
        }

        private void FindVdsDisk(Func<Disk, bool> compareFunc, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            advancedDisk = null;
            diskPack = null;

            using (ServiceLoader serviceLoader = new ServiceLoader())
            using (Service vds = serviceLoader.LoadService(_serverName))
            {
                vds.WaitForServiceReady();

                foreach (Disk disk in vds.UnallocatedDisks)
                {
                    if (compareFunc(disk))
                    {
                        advancedDisk = (AdvancedDisk)disk;
                        break;
                    }
                    disk?.Dispose(); // disose disk if it is not an AdvancedDisk
                }

                if (advancedDisk == null)
                {
                    vds.HardwareProvider = false;
                    vds.SoftwareProvider = true;

                    foreach (SoftwareProvider provider in vds.Providers)
                    {
                        using (provider)
                        {
                            foreach (Pack pack in provider.Packs)
                            {
                                foreach (Disk disk in pack.Disks)
                                {
                                    if (compareFunc(disk))
                                    {
                                        diskPack = pack;
                                        advancedDisk = (AdvancedDisk)disk;
                                        //break; //this looks like a bug, why do we break here and then continue the loop?
                                        return;
                                    }
                                    disk?.Dispose();
                                }
                                pack?.Dispose(); // manually dispose, without using block (cause out parameter)
                            }
                        }
                    }
                }
            }
        }

        // obsolete and currently is not used
        private string RunDiskPart(string script)
        {
            // create temp script file name
            string localPath = Path.Combine(_fileSystemHelper.GetTempRemoteFolder(), Guid.NewGuid().ToString("N"));

            // save script to remote temp file
            string remotePath = _fileSystemHelper.ConvertToUNC(localPath);
            File.AppendAllText(remotePath, script);

            // run diskpart
            _fileSystemHelper.ExecuteRemoteProcess("DiskPart /s " + localPath);

            // delete temp script
            try
            {
                File.Delete(remotePath);
            }
            catch
            {
                // TODO
            }

            return "";
        }

        //public static void ExecuteRemoteProcess(string serverName, string command)
        //{
        //    Wmi cimv2 = new Wmi(serverName, "root\\cimv2");
        //    ManagementClass objProcess = cimv2.GetWmiClass("Win32_Process");

        //    // run process
        //    object[] methodArgs = { command, null, null, 0 };
        //    objProcess.InvokeMethod("Create", methodArgs);

        //    // process ID
        //    int processId = Convert.ToInt32(methodArgs[3]);

        //    // wait until finished
        //    // Create event query to be notified within 1 second of 
        //    // a change in a service
        //    WqlEventQuery query =
        //        new WqlEventQuery("__InstanceDeletionEvent",
        //        new TimeSpan(0, 0, 1),
        //        "TargetInstance isa \"Win32_Process\"");

        //    // Initialize an event watcher and subscribe to events 
        //    // that match this query
        //    ManagementEventWatcher watcher = new ManagementEventWatcher(cimv2.GetScope(), query);
        //    // times out watcher.WaitForNextEvent in 20 seconds
        //    watcher.Options.Timeout = new TimeSpan(0, 0, 20);

        //    // Block until the next event occurs 
        //    // Note: this can be done in a loop if waiting for 
        //    //        more than one occurrence
        //    while (true)
        //    {
        //        ManagementBaseObject e = null;

        //        try
        //        {
        //            // wait untill next process finish
        //            e = watcher.WaitForNextEvent();
        //        }
        //        catch
        //        {
        //            // nothing has been finished in timeout period
        //            return; // exit
        //        }

        //        // check process id
        //        int pid = Convert.ToInt32(((ManagementBaseObject)e["TargetInstance"])["ProcessID"]);
        //        if (pid == processId)
        //        {
        //            //Cancel the subscription
        //            watcher.Stop();

        //            // exit
        //            return;
        //        }
        //    }
        //}
    }
}
