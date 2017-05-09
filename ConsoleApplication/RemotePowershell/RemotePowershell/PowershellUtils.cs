using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Runtime.InteropServices;


namespace RemotePowershell
{
    public class RemotePowerShellUtils
    {

        private readonly string _computerName;
        private readonly SecureString _password;
        private readonly string _user;
        private readonly PSCredential _credential;

        public RemotePowerShellUtils(string computerName, string user, string password) : 
            this(computerName, user, ConvertToSecureString(password))
        { }

        public RemotePowerShellUtils(string computerName, string user, SecureString securedPassword)
        {
            _computerName = computerName;
            _user = user;
            _password = securedPassword;
            _credential = new PSCredential(user, securedPassword);
        }

        private static SecureString ConvertToSecureString(string input)
        {
            // TODO: validate input not null empty.
            SecureString output = new SecureString();
            input.ToCharArray().ToList().ForEach(p => output.AppendChar(p));
            return output;
        }

        private static string LocalDebuggerPath => @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Remote Debugger\x64\";
        private string DestinationPath => $@"c:\users\{_user}\documents\RemoteDebugger";

        /// <summary>
        /// Sample execution scenario 2: Asynchronous
        /// </summary>
        /// <remarks>
        /// Executes a PowerShell script asynchronously with script output and event handling.
        /// </remarks>
        public void ExecuteAsync(Action<PowerShell> addCommndsCallback)
        {
            using (PowerShell powerShell = PowerShell.Create())
            {
                addCommndsCallback(powerShell);

                // this script has a sleep in it to simulate a long running script
                //powerShell.AddScript("$s1 = 'test1'; $s2 = 'test2'; $s1; write-error 'some error';start-sleep -s 7; $s2");

                // prepare a new collection to store output stream objects
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += outputCollection_DataAdded;

                // the streams (Error, Debug, Progress, etc) are available on the PowerShell instance.
                // we can review them during or after execution.
                // we can also be notified when a new item is written to the stream (like this):
                powerShell.Streams.Error.DataAdded += Error_DataAdded;

                // begin invoke execution on the pipeline
                // use this overload to specify an output stream buffer
                IAsyncResult result = powerShell.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                // do something else until execution has completed.
                // this could be sleep/wait, or perhaps some other work
                while (result.IsCompleted == false)
                {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    Thread.Sleep(1000);

                    // might want to place a timeout here...
                }

                Console.WriteLine("Execution has stopped. The pipeline state: " + powerShell.InvocationStateInfo.State);

                foreach (PSObject outputItem in outputCollection)
                {
                    //TODO: handle/process the output items if required
                    Console.WriteLine(outputItem?.ToString());
                }

                foreach (ErrorRecord error in powerShell.Streams.Error)
                {
                    //TODO: handle/process the output items if required
                    Console.WriteLine(error.ToString());

                    if (error.Exception != null)
                    {
                        TranslateException(error.Exception);
                    }
                }
            }
        }

        public void AddCopyCommands(PowerShell powerShell)
        {
            var copyScript = $@"Copy-Item -Path ""{LocalDebuggerPath}""  -Destination ""{DestinationPath}""  -ToSession $session -Recurse";

            powerShell.AddCommand("Set-Variable");
            powerShell.AddParameter("Name", "cred");
            powerShell.AddParameter("Value", _credential);

            powerShell.AddScript(@"$sessionOptions = New-PSSessionOption –SkipCACheck –SkipCNCheck –SkipRevocationCheck");
            powerShell.AddScript($@"$session = New-PSSession {_computerName} -UseSSL -Credential $cred -SessionOption $sessionOptions");

            //powerShell.AddScript($@".\dir.ps1 ");

            powerShell.AddScript($@" .\InstallStartMsvsmon.ps1 ");
            powerShell.AddScript(@"Remove-PSSession -Session $session");
        }

        public void AddSetupMsvsmonCommands(PowerShell powerShell)
        {
            string script =
            @"
$ErrorActionPreference = ""Stop""
cd remoteDebugger
dir .\msvsmon.exe
.\msvsmon.exe /prepcomputer /public
";
            powerShell.AddScript(script);
        }

        public void AddStartmsvsmonCommands(PowerShell powerShell)
        {
            string script =
            @"$ErrorActionPreference = ""Stop""
$env:computername
Get-Location
cd remoteDebugger
dir .\msvsmon.exe
.\msvsmon.exe /silent
";
            powerShell.AddScript(script);
        }

        public void AddDir(PowerShell powerShell)
        {
            string script =
            @"
$ErrorActionPreference = ""Stop""
echo test > a.txt 
dir;";
            powerShell.AddScript(script);
        }

        /// <summary>
        /// Event handler for when data is added to the output stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            // do something when an object is written to the output stream
            Console.WriteLine("Object added to output.");

            //foreach (PSObject outputItem in (sender as PSDataCollection<PSObject>))
            //{
            //    Console.WriteLine(outputItem?.ToString());
            //}
        }

        /// <summary>
        /// Event handler for when Data is added to the Error stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all error output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            // do something when an error is written to the error stream
            Console.WriteLine("An error was written to the Error stream!");
        }

        public void EnterSessionAsync(Action<PowerShell> addScriptCallback)
        {
            int iRemotePort = 5986;
            string strShellURI = @"http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
            string strAppName = @"/wsman";
            AuthenticationMechanism auth = AuthenticationMechanism.Default;

            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(
                useSsl: true,
                computerName: _computerName,
                port: iRemotePort,
                appName: strAppName,
                shellUri: strShellURI,
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
                catch (Exception ex) when (ex is PSRemotingTransportException)
                {
                    TranslateException(ex);
                }


                Console.WriteLine("Connected to {0}", _computerName);
                Console.WriteLine("As {0}", _user);
                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.Runspace = runSpace;
                    //powerShell.AddScript("$env:computername");
                    //powerShell.AddScript("Get-Location");

                    addScriptCallback(powerShell);

                    // prepare a new collection to store output stream objects
                    PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                    outputCollection.DataAdded += outputCollection_DataAdded;

                    // the streams (Error, Debug, Progress, etc) are available on the PowerShell instance.
                    // we can review them during or after execution.
                    // we can also be notified when a new item is written to the stream (like this):
                    powerShell.Streams.Error.DataAdded += Error_DataAdded;

                    // begin invoke execution on the pipeline
                    // use this overload to specify an output stream buffer
                    IAsyncResult result = powerShell.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                    // do something else until execution has completed.
                    // this could be sleep/wait, or perhaps some other work
                    while (result.IsCompleted == false)
                    {
                        Console.WriteLine("Waiting for pipeline to finish...");
                        Thread.Sleep(1000);

                        // might want to place a timeout here...
                    }

                    Console.WriteLine("Execution has stopped. The pipeline state: " + powerShell.InvocationStateInfo.State);

                    // powerShell.EndInvoke(result);
                    //foreach (PSObject outputItem in psObjects)
                    //{
                    //    //TODO: handle/process the output items if required
                    //    Console.WriteLine(outputItem?.ToString());
                    //}

                    foreach (PSObject outputItem in outputCollection)
                    {
                        //TODO: handle/process the output items if required
                        Console.WriteLine(outputItem?.ToString());
                    }

                    foreach (ErrorRecord error in powerShell.Streams.Error)
                    {
                        //TODO: handle/process the output items if required
                        Console.WriteLine(error.ToString());

                        // With all exception, throw the first one.
                        if (error.Exception != null)
                        {
                            TranslateException(error.Exception);
                        }
                    }

                    while (true)
                    {
                        string s = Console.ReadLine();
                        if (String.IsNullOrWhiteSpace(s))
                        {
                            return;
                        }
                    }
                }
            }
        }

        public enum Win32ErrorCode : long
        {
            ERROR_SUCCESS = 0L,
            NO_ERROR = 0L,
            ERROR_INVALID_FUNCTION = 1L,
            ERROR_FILE_NOT_FOUND = 2L,
            ERROR_PATH_NOT_FOUND = 3L,
            ERROR_TOO_MANY_OPEN_FILES = 4L,
            ERROR_ACCESS_DENIED = 5L,
        }   

        private void TranslateException(Exception ex)
        {
            if (ex is PSRemotingTransportException)
            {
                var remotingEx = ex as PSRemotingTransportException;
                if (remotingEx.ErrorCode == (int)Win32ErrorCode.ERROR_ACCESS_DENIED)
                {
                    throw new RemotePowerShellAccessDeniedException(ex);
                }
            }

            throw ex;
        }
    }
}
