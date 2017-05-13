// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Runtime.InteropServices;


namespace GoogleCloudExtension.RemotePowerShell
{
    /// <summary>
    /// Reomote powershell target machine.
    /// </summary>
    public class RemoteTarget
    {
        private const int RemotePort = 5986;
        private const string ShellURI = @"http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
        private const string AppName = @"/wsman";
        private const ulong ErrorCodeAccessDenied = 5L;

        protected readonly string _computerName;
        protected readonly string _userName;
        protected readonly SecureString _password;
        protected readonly PSCredential _credential;

        public RemoteTarget(string computerName, string username, SecureString password)
        {
            _computerName = computerName;
            _userName = username;
            _password = password;
            _credential = Utils.CreatePSCredential(username, password);
        }

        /// <summary>
        /// Executes a PowerShell script asynchronously.
        /// </summary>
        public async Task<bool> ExecuteAsync(
            Action<PowerShell> addCommndsCallback)
        {
            using (PowerShell powerShell = PowerShell.Create())
            {
                addCommndsCallback(powerShell);
                return await WaitComplete(powerShell);
            }
        }

        public void EnterSessionExecute(
            string script,
            CancellationToken cancelToken)
        {
            // TODO: verify script is not null
            AuthenticationMechanism auth = AuthenticationMechanism.Default;

            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(
                useSsl: true,
                computerName: _computerName,
                port: RemotePort,
                appName: AppName,
                shellUri: ShellURI,
                credential: _credential);
            connectionInfo.AuthenticationMechanism = auth;
            connectionInfo.SkipCACheck = true;
            connectionInfo.SkipCNCheck = true;
            connectionInfo.SkipRevocationCheck = true;

            using (var runSpace = RunspaceFactory.CreateRunspace(connectionInfo))
            {
                var psh = runSpace.CreatePipeline();

                try
                {
                    runSpace.Open();
                }
                catch (PSRemotingTransportException ex)
                {
                    var remotingEx = ex as PSRemotingTransportException;
                    if (remotingEx.ErrorCode == (int)ErrorCodeAccessDenied)
                    {
                        throw new RemotePowerShellAccessDeniedException(ex);
                    }

                    throw;
                }

                Debug.WriteLine($"Connected to {_computerName}");
                Debug.WriteLine($"As {_userName}");
                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.Runspace = runSpace;
                    powerShell.AddScript(script);
                    var task = WaitComplete(powerShell);
                    try
                    {
                        Debug.WriteLine($"EnterSessionExecute Task.WaitAll");
                        Task.WaitAll(new Task[] { task }, cancelToken);
                        Debug.WriteLine($"EnterSessionExecute Task.WaitAll completes, result code {task.Result}");
                    }
                    catch (Exception ex) when (
                        ex is OperationCanceledException
                        || ex is ObjectDisposedException
                        || ex is PSRemotingDataStructureException
                        || ex is PSRemotingTransportException
                        || ex is PSRemotingTransportRedirectException
                        || ex is AggregateException)
                    {
                        Debug.WriteLine($"EnterSessionExecute caught exception. {ex}");
                    }
                }
            }
        }

        private static async Task<bool> WaitComplete(PowerShell powerShell)
        {
            // prepare a new collection to store output stream objects
            PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
            outputCollection.DataAdded += outputCollection_DataAdded;

            // the streams (Error, Debug, Progress, etc) are available on the PowerShell instance.
            // we can review them during or after execution.
            // we can also be notified when a new item is written to the stream (like this):
            powerShell.Streams.Error.DataAdded += Error_DataAdded;

            #region comment out
            // begin invoke execution on the pipeline
            // use this overload to specify an output stream buffer
            //IAsyncResult result = powerShell.BeginInvoke<PSObject, PSObject>(null, outputCollection);

            //// do something else until execution has completed.
            //// this could be sleep/wait, or perhaps some other work
            //while (!result.IsCompleted && !cancelToken.IsCancellationRequested)
            //{
            //    // Debug.WriteLine("Waiting for pipeline to finish...");
            //    await Task.Delay(100, cancelToken);
            //}

            //if (cancelToken.IsCancellationRequested)
            //{
            //    return false;
            //}

            //if (!result.IsCompleted)
            //{
            //    return false;
            //}


            //foreach (PSObject outputItem in outputCollection)
            //{
            //    //TODO: handle/process the output items if required
            //    Debug.WriteLine(outputItem?.ToString());
            //}

            //foreach (ErrorRecord error in powerShell.Streams.Error)
            //{
            //    //TODO: handle/process the output items if required
            //    Debug.WriteLine(error.ToString());

            //    // With all exception, throw the first one.
            //    if (error.Exception != null)
            //    {
            //        TranslateException(error.Exception);
            //    }
            //}

            #endregion

            var results = await Task.Factory.FromAsync(
                powerShell.BeginInvoke(),
                pResult =>
                {
                    try
                    {
                        return powerShell.EndInvoke(pResult);
                    }
                    catch (Exception ex) when (
                        ex is ObjectDisposedException ||
                        ex is PSRemotingDataStructureException ||
                        ex is PSRemotingTransportRedirectException ||
                        ex is PSRemotingTransportException)
                    {
                        Debug.WriteLine($"powerShell.EndInvoke exception. {ex}");
                        return false;
                    }
                });
            // Using the PowerShell.EndInvoke method, get the
            // results from the IAsyncResult object.
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject psObject in results)
            {
                stringBuilder.AppendLine(psObject.ToString());
            } // End foreach.

            Debug.WriteLine(stringBuilder.ToString());

            Debug.WriteLine("Execution has stopped. The pipeline state: " + powerShell.InvocationStateInfo.State);
            return (powerShell.Streams.Error?.Count).GetValueOrDefault() == 0;
        }

        /// <summary>
        /// Event handler for when data is added to the output stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private static void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            // do something when an object is written to the output stream
            Debug.WriteLine("Object added to output.");

            //foreach (PSObject outputItem in (sender as PSDataCollection<PSObject>))
            //{
            //    Debug.WriteLine(outputItem?.ToString());
            //}
        }

        /// <summary>
        /// Event handler for when Data is added to the Error stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all error output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private static void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            // do something when an error is written to the error stream
            Debug.WriteLine("An error was written to the Error stream!");
        }
    }
}
