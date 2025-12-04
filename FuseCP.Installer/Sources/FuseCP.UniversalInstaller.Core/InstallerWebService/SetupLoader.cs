// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace FuseCP.UniversalInstaller.Core;

public class LoaderEventArgs<T> : EventArgs
{
	public string StatusMessage { get; set; }
	public T EventData { get; set; }
	public bool Cancellable { get; set; }
}

/// <summary>
/// Loader form.
/// </summary>
public class SetupLoader
{
	public const string ConnectingRemotServiceMessage = "Connecting...";
	public const string DownloadingSetupFilesMessage = "Downloading setup files...";
	public const string CopyingSetupFilesMessage = "Copying setup files...";
	public const string PreparingSetupFilesMessage = "Please wait while Setup prepares the necessary files...";
	public const string DownloadProgressMessage = "{0} KB of {1} KB";
	public const string PrepareSetupProgressMessage = "{0}%";

	public const int ChunkSize = 262144;
	private RemoteFile remoteFile;
	private CancellationTokenSource cts;

	public event EventHandler<LoaderEventArgs<String>> StatusChanged;
	public event EventHandler<LoaderEventArgs<Exception>> OperationFailed;
	public event EventHandler<LoaderEventArgs<Int32>> ProgressChanged;
	public event EventHandler<EventArgs> DownloadComplete;
	public event EventHandler<EventArgs> OperationCompleted;
	public event EventHandler<EventArgs> NoUnzipStatus;
	public event EventHandler<LoaderEventArgs<int>> SetMaximumProgress;
	public SetupLoader(RemoteFile remoteFile)
	{
		this.remoteFile = remoteFile;
	}

	public bool SetupOnly { get; set; } = false;
	public void LoadAppDistributive()
	{
		ThreadPool.QueueUserWorkItem(q => LoadAppDistributiveInternal());
	}

	protected void RaiseOnStatusChangedEvent(string statusMessage)
	{
		RaiseOnStatusChangedEvent(statusMessage, String.Empty);
	}

	protected void RaiseOnStatusChangedEvent(string statusMessage, string eventData)
	{
		RaiseOnStatusChangedEvent(statusMessage, eventData, true);
	}

	protected void RaiseDownloadCompleteEvent() => DownloadComplete?.Invoke(this, EventArgs.Empty);
	protected void RaiseOnStatusChangedEvent(string statusMessage, string eventData, bool cancellable)
	{
		if (StatusChanged == null)
		{
			return;
		}
		// No event data for status updates
		StatusChanged(this, new LoaderEventArgs<String>
		{
			StatusMessage = statusMessage,
			EventData = eventData,
			Cancellable = cancellable
		});
	}

	protected void RaiseOnProgressChangedEvent(int eventData)
	{
		RaiseOnProgressChangedEvent(eventData, true);
	}

	int maxProgress = 0;
	protected void RaiseSetMaximumProgressEvent(int eventData)
	{
		if (maxProgress != eventData)
		{
			SetMaximumProgress?.Invoke(eventData, new LoaderEventArgs<int>() { EventData = eventData });
		}
	}

	protected void RaiseOnProgressChangedEvent(int eventData, bool cancellable)
	{
		if (ProgressChanged == null)
		{
			return;
		}
		//
		ProgressChanged(this, new LoaderEventArgs<int>
		{
			EventData = eventData,
			Cancellable = cancellable
		});
	}

	protected void RaiseOnOperationFailedEvent(Exception ex)
	{
		if (OperationFailed == null)
		{
			return;
		}
		//
		OperationFailed(this, new LoaderEventArgs<Exception> { EventData = ex });
	}

	protected void RaiseOnOperationCompletedEvent()
	{
		if (OperationCompleted == null)
		{
			return;
		}
		//
		OperationCompleted(this, EventArgs.Empty);
	}

	protected void RaiseNoUzipStatusEvent() => NoUnzipStatus?.Invoke(this, EventArgs.Empty);

	/// <summary>
	/// Executes a file download request asynchronously
	/// </summary>
	private void LoadAppDistributiveInternal()
	{
		try
		{
			string dataFolder;
			string tmpFolder;
			// Retrieve local storage configuration
			GetLocalStorageInfo(out dataFolder, out tmpFolder);
			// Initialize storage
			InitializeLocalStorage(dataFolder, tmpFolder);

			cts = new CancellationTokenSource();
			CancellationToken token = cts.Token;

			var fileToDownload = Path.GetFileName(remoteFile.File);
			var exeFile = Installer.Current.GetEntryAssembly().Location;
			var exeFolder = Path.GetDirectoryName(exeFile);
			var installerPath = remoteFile.Release.InstallerPath.Replace('/', Path.DirectorySeparatorChar);
			var setupFileName = Path.Combine(exeFolder,
				Path.GetFileName(installerPath));
			var destinationFile = Path.Combine(dataFolder, fileToDownload);
			var tmpFile = Path.Combine(tmpFolder, fileToDownload);

			var progressFile = Path.Combine(Path.GetDirectoryName(tmpFolder), DownloadProgressFile);

			if (File.Exists(setupFileName) && File.GetLastWriteTimeUtc(exeFile) <= File.GetLastWriteTimeUtc(setupFileName))
			{
				var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(setupFileName);
				var version = assembly.Name.Version;
				if (version.Major == remoteFile.Release.Version.Major &&
					version.Minor == remoteFile.Release.Version.Minor &&
					version.Build == remoteFile.Release.Version.Build)
				{
					RaiseNoUzipStatusEvent();
					RaiseSetMaximumProgressEvent(100);
					RaiseOnProgressChangedEvent(100);
					RaiseDownloadCompleteEvent();
					RaiseOnOperationCompletedEvent();

					if (!SetupOnly) StartDownloadAsyncRelease(remoteFile, tmpFile, destinationFile, tmpFolder, installerPath, token);
					else
					{
						File.Delete(progressFile);
					}

					return;
				}
			}

			// Check for setup file
			var info = Installer.Current.Releases.GetReleaseFileInfo(Path.GetFileNameWithoutExtension(setupFileName).ToLower(), remoteFile.Release.VersionName);
			if (info != null)
			{
				try
				{
					Task downloadSetupTask;

					if (info.FullFilePath.EndsWith(".7z", StringComparison.OrdinalIgnoreCase) ||
						info.FullFilePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
					{
						setupFileName = Path.ChangeExtension(setupFileName, Path.GetExtension(info.FullFilePath));
					}
					else RaiseNoUzipStatusEvent();

					downloadSetupTask = GetDownloadFileTask(new RemoteFile(info, true), setupFileName, exeFolder, null, token);

					var downloadComponentTask = downloadSetupTask.ContinueWith(task =>
					{
						RaiseDownloadCompleteEvent();
						RaiseOnOperationCompletedEvent();

						if (!SetupOnly) StartDownloadAsyncRelease(remoteFile, tmpFile, destinationFile, tmpFolder, installerPath, token);
						else
						{
							var progressFile = Path.Combine(Path.GetDirectoryName(tmpFolder), DownloadProgressFile);
							File.Delete(progressFile);
						}
					}, token);
					downloadSetupTask.Start();
				}
				catch (Exception ex) { }

				return;
			}

			try
			{
				var progressStream = new FileStream(progressFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete);

				// report progress in progress file so it can be read by setup
				EventHandler<LoaderEventArgs<int>> handler = (sender, args) =>
				{
					try
					{
						while (args.EventData > progressStream.Length)
						{
							progressStream.WriteByte(0);
							progressStream.Flush();
						}
					}
					catch { }
				};
				ProgressChanged += handler;

				// Download the file requested
				Task downloadFileTask = GetDownloadFileTask(remoteFile, tmpFile, tmpFolder, null, token);

				var notifyCompletionTask = downloadFileTask.ContinueWith(t =>
				{
					RaiseDownloadCompleteEvent();
					RaiseOnOperationCompletedEvent();

					var nofFilesFile = Path.Combine(Path.GetDirectoryName(tmpFolder), NofFilesFile);
					File.WriteAllText(nofFilesFile, Installer.Current.Files.ToString());
					progressStream.Close();
					File.Delete(progressFile);
					ProgressChanged -= handler;
				}, CancellationToken.None, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted,
				TaskScheduler.Current);

				var failedTask = downloadFileTask.ContinueWith(t =>
				{
					RaiseOnOperationFailedEvent(t.Exception);
					var nofFilesFile = Path.Combine(Path.GetDirectoryName(tmpFolder), NofFilesFile);
					File.WriteAllText(nofFilesFile, Installer.Current.Files.ToString());
					progressStream.Close();
					File.Delete(progressFile);
					ProgressChanged -= handler;
				}, CancellationToken.None, TaskContinuationOptions.NotOnRanToCompletion, TaskScheduler.Current);

				downloadFileTask.Start();
				downloadFileTask.Wait();
			}
			catch (AggregateException ae)
			{
				ae.Handle((e) =>
				{
					// We handle cancellation requests
					if (e is OperationCanceledException)
					{
						CancelDownload(tmpFile);
						Log.WriteInfo("Download has been cancelled by the user");
						return true;
					}
					// But other issues just being logged
					Log.WriteError("Could not download the file", e);
					return false;
				});
			}
		}
		catch (Exception ex)
		{
			if (ex is ThreadAbortException)
				return;

			Log.WriteError("Loader module error", ex);
			//
			RaiseOnOperationFailedEvent(ex);
		}
	}


	public const string DownloadProgressFile = "_downloadProgress";
	public const string NofFilesFile = "_nofFiles";
	protected void StartDownloadAsyncRelease(RemoteFile remoteFile, string tmpFile, string destinationFile,
		string tmpFolder, string installerPath, CancellationToken token)
	{
		var progressFile = Path.Combine(Path.GetDirectoryName(tmpFolder), DownloadProgressFile);
		var nofFilesFile = Path.Combine(Path.GetDirectoryName(tmpFolder), NofFilesFile);

		try
		{
			var installerPathDir = Path.GetDirectoryName(installerPath).Replace(Path.DirectorySeparatorChar, '/');
			if (!string.IsNullOrEmpty(installerPathDir) && installerPathDir != "/") installerPath = installerPathDir;
			var loader = new SetupLoader(remoteFile);
			var progressStream = new FileStream(progressFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete);

			// report progress in progress file so it can be read by setup
			EventHandler<LoaderEventArgs<int>> handler = (sender, args) =>
			{
				try
				{
					while (args.EventData > progressStream.Length)
					{
						progressStream.WriteByte(0);
						progressStream.Flush();
					}
				}
				catch { }
			};
			loader.ProgressChanged += handler;

			// Download the file requested
			Task downloadFileTask = loader.GetDownloadFileTask(remoteFile, tmpFile, tmpFolder, null, token);
			var faultDownloadFileTask = downloadFileTask.ContinueWith(t =>
			{
				progressStream.Close();
				File.Delete(progressFile);
				loader.ProgressChanged -= handler;
				RaiseOnOperationFailedEvent(t.Exception);
			}, TaskContinuationOptions.NotOnRanToCompletion);
			var successDownloadFileTask = downloadFileTask.ContinueWith(t =>
			{
				progressStream.Close();
				File.WriteAllText(nofFilesFile, Installer.Current.Files.ToString());
				File.Delete(progressFile);
				loader.ProgressChanged -= handler;
				RaiseDownloadCompleteEvent();
				RaiseOnOperationCompletedEvent();
			}, CancellationToken.None, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted,
			TaskScheduler.Current);
			//

			try
			{
				downloadFileTask.Start();
			}
			catch { }
		}
		catch (AggregateException ae)
		{
			ae.Handle((e) =>
			{
				// We handle cancellation requests
				if (e is OperationCanceledException)
				{
					CancelDownload(tmpFile);
					Log.WriteInfo("Download has been cancelled by the user");
					return true;
				}
				// But other issues just being logged
				Log.WriteError("Could not download the file", e);
				return false;
			});
		}
		catch (Exception ex)
		{
			if (ex is ThreadAbortException)
				return;

			Log.WriteError("Loader module error", ex);
			//
			RaiseOnOperationFailedEvent(ex);
		}
	}

	protected virtual Task GetDownloadFileTask(RemoteFile sourceFile, string tmpFile, string destPath, Func<string, bool> filter, CancellationToken ct)
	{
		var downloadFileTask = new Task(async () =>
		{
			if (!File.Exists(tmpFile))
			{
				RaiseSetMaximumProgressEvent(100);
				RaiseOnProgressChangedEvent(0);
				RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage);

				Log.WriteStart("Downloading file");
				Log.WriteInfo(string.Format("Downloading file \"{0}\" to \"{1}\"", sourceFile.File, tmpFile));

				long downloadedSize = 0;
				if (sourceFile.File.EndsWith(".7z", StringComparison.OrdinalIgnoreCase) ||
					sourceFile.File.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
				{
					await Installer.Current.Releases.GetFileAndUnzipAsync(sourceFile, tmpFile, destPath, filter,
						(downloaded, fileSize) =>
						{
							downloadedSize = downloaded;
							// Update download progress
							RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage,
								string.Format(DownloadProgressMessage, downloaded / 1024, fileSize / 1024));

							RaiseOnProgressChangedEvent((int)(downloaded * 100 / fileSize));
						});
				}
				else
				{
					await Installer.Current.Releases.GetFileAsync(sourceFile, tmpFile,
						(downloaded, fileSize) =>
						{
							downloadedSize = downloaded;
							// Update download progress
							RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage,
								string.Format(DownloadProgressMessage, downloaded / 1024, fileSize / 1024));

							RaiseOnProgressChangedEvent((int)(downloaded * 100 / fileSize));
						});
				}
				RaiseOnProgressChangedEvent(100);
				RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage, "100%");
				Log.WriteEnd(string.Format("Downloaded {0} bytes", downloadedSize));
				RaiseDownloadCompleteEvent();
				RaiseOnOperationCompletedEvent();
			}
		}, ct);

		return downloadFileTask;
	}

	private static void InitializeLocalStorage(string dataFolder, string tmpFolder)
	{
		if (!Directory.Exists(dataFolder))
		{
			Directory.CreateDirectory(dataFolder);
			Log.WriteInfo("Data directory created");
		}

		if (Directory.Exists(tmpFolder))
		{
			try
			{
				Directory.Delete(tmpFolder, true);
			}
			catch { }
		}

		if (!Directory.Exists(tmpFolder))
		{
			Directory.CreateDirectory(tmpFolder);
			Log.WriteInfo("Tmp directory created");
		}
	}

	private static void GetLocalStorageInfo(out string dataFolder, out string tmpFolder)
	{
		dataFolder = FileUtils.GetDataDirectory();
		tmpFolder = Installer.Current.ComponentTempPath; //FileUtils.GetTempDirectory(); + "\Component"
	}

	/// <summary>
	/// Cleans up temporary file if the download process has been cancelled.
	/// </summary>
	/// <param name="tmpFile">Path to the temporary file to cleanup</param>
	protected virtual void CancelDownload(string tmpFile)
	{
		if (File.Exists(tmpFile))
		{
			File.Delete(tmpFile);
		}
	}

	public void AbortOperation()
	{
		// Make sure we are in business
		if (cts != null)
		{
			cts.Cancel();
		}
	}
}
