using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Security;
using System.Runtime.InteropServices;
using System.IO;
using static System.Console;

namespace RemotePowershell
{
    class Program
    {
        static void Main(string[] args)
        {
            //StartPowershell("35.185.240.69");
            //WriteLine("Input password");
            //string password = ReadLine();
            RemotePowerShellUtils rps = new RemotePowerShellUtils("35.185.240.69", "deren", s_lazySecret.Value);
            // rps.ExecuteAsync(rps.AddCopyCommands);  --- Done, good
            //  Console.ReadKey();


            //rps.EnterSessionAsync(rps.AddSetupMsvsmonCommands);
            //Console.ReadKey();

            rps.EnterSessionAsync(rps.AddStartmsvsmonCommands);
            //Console.ReadKey();
        }

        //private static void AnotherExample()
        //{
        //    string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
        //    string password;
        //    string user = "deren";
        //    SecureString secureString = new SecureString();
        //    password.ToCharArray().ToList().ForEach(p => secureString.AppendChar(p));

        //    PSCredential remoteCredential = new PSCredential(user, secureString);
        //    WSManConnectionInfo connectionInfo = new WSManConnectionInfo(true, "Ip Address of server", 5985, "/wsman", shellUri, remoteCredential, 1 * 60 * 1000);

        //    string scriptPath = $@"
        //$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri http://servername/poweshell -Credential {remoteCredential} | Out-String
        //Import-PSSession $Session";

        //    Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo);
        //    connectionInfo.AuthenticationMechanism = AuthenticationMechanism.Basic;
        //    runspace.Open();
        //    RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
        //    Pipeline pipeline = runspace.CreatePipeline();
        //    string scriptfile = scriptPath;
        //    Command myCommand = new Command(scriptfile, false);
        //    pipeline.Commands.Add(myCommand);
        //    pipeline.Invoke();
        //    runspace.Close();
        //}

        //private static void StartPowershell(string sRemote)
        //{
        //    string user = "deren";
        //    string password = @";W6?4D)r*he[g){";

        //    SecureString secureString = new SecureString();
        //    password.ToCharArray().ToList().ForEach(p => secureString.AppendChar(p));
        //    var creds = new PSCredential(user, secureString);
        //    Console.WriteLine(SecureStringToString(secureString));

        //    string shell = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
        //    var targetWsMan = new Uri(string.Format("http://{0}:5985/wsman", sRemote));
        //    var connectionInfo = new WSManConnectionInfo(targetWsMan, shell, creds);
        //    AuthenticationMechanism auth = AuthenticationMechanism.Default;
        //    connectionInfo.AuthenticationMechanism = auth;
        //    connectionInfo.SkipCACheck = true;
        //    connectionInfo.SkipCNCheck = true;
        //    connectionInfo.SkipRevocationCheck = true;
        //    using (var runSpace = RunspaceFactory.CreateRunspace(connectionInfo))
        //    {
        //        // var p = runSpace.CreatePipeline();
        //        runSpace.Open();
        //        PowerShell psh = PowerShell.Create();
        //        Console.WriteLine("Connected to {0}", sRemote);
        //        Console.WriteLine("As {0}", user);
        //        Console.Write("Command to run: ");
        //        var cmd = Console.ReadLine();
        //        PSCommand command = new PSCommand();
        //        command.AddArgument(@"echo ""aaa""  .\a.txt ");
        //        psh.Commands = command;
        //        var returnValue = psh.Invoke();
        //        foreach (var v in returnValue)
        //            Console.WriteLine(v.ToString());
        //    }
        //}



        private static readonly Lazy<string> s_lazySecret = new Lazy<string>(() => OpenFileGetLine(@".\password.txt"));

        private static string OpenFileGetLine(string relativePath)
        {
            using (var sr = new StreamReader(relativePath))
            {
                var sec = sr.ReadLine().Trim();
                if (string.IsNullOrEmpty(sec))
                {
                    throw new Exception("Failed to get secret");
                }

                return sec;
            }
        }

        static string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private static void CopyFiles(string sRemote)
        {

        }


        private static void StartPowershell(string sRemote)
        {
            int iRemotePort = 5986;
            string strShellURI = @"http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
            string strAppName = @"/wsman";
            AuthenticationMechanism auth = AuthenticationMechanism.Default;
            string user = "deren";
            string password = Console.ReadLine();

            SecureString secureString = new SecureString();
            password.ToCharArray().ToList().ForEach(p => secureString.AppendChar(p));
            var creds = new PSCredential(user, secureString);

            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(
                useSsl: true,
                computerName: sRemote,
                port: iRemotePort,
                appName: strAppName,
                shellUri: strShellURI,
                credential: creds);
            connectionInfo.AuthenticationMechanism = auth;
            connectionInfo.SkipCACheck = true;
            connectionInfo.SkipCNCheck = true;
            connectionInfo.SkipRevocationCheck = true;

            //Runspace runspace = RunspaceFactory.CreateRunspace(ci);
            //runspace.Open();

            //PowerShell psh = PowerShell.Create();
            //psh.Runspace = runspace;
            //RunCommand(psh);
            //psh.Dispose();
            //runspace.Dispose();

string script =
@"
#cd c:\users\deren\documents\remoteDebugger
#dir
#.\msvsmon.exe /prepcomputer /public
#.\msvsmon.exe /silent 
";

            using (var runSpace = RunspaceFactory.CreateRunspace(connectionInfo))
            {
                var psh = runSpace.CreatePipeline();
                runSpace.Open();
                Console.WriteLine("Connected to {0}", sRemote);
                Console.WriteLine("As {0}", user);
                //runSpace.SessionStateProxy.Path.SetLocation(@"c:\users\deren\documents\remoteDebugger");
                psh.Commands.AddScript(script);
                var returnValue = psh.Invoke();
                foreach (var v in returnValue)
                    Console.WriteLine(v.ToString());

                Console.ReadLine();
            }
        }

        //private static void RunCommand(PowerShell psh)
        //{
        //    PSCommand command = new PSCommand();
        //    command.AddArgument(@"echo ""aaa""  .\a.txt ");
        //    psh.Commands = command;
        //    var returnValue = psh.Invoke();
        //    foreach (var v in returnValue)
        //        Console.WriteLine(v.ToString());
        //}
    }
}
