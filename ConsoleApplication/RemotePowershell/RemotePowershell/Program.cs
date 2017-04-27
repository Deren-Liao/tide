using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Security;
using System.Runtime.InteropServices;

namespace RemotePowershell
{
    class Program
    {
        static void Main(string[] args)
        {
            StartPowershell("35.184.53.125");
        }

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

        private static void StartPowershell(string sRemote)
        {
            int iRemotePort = 5985;
            string strShellURI = @"http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
            string strAppName = @"/wsman";
            AuthenticationMechanism auth = AuthenticationMechanism.Default;
            string user = "deren";
            string password = 

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

            using (var runSpace = RunspaceFactory.CreateRunspace(connectionInfo))
            {
                // var p = runSpace.CreatePipeline();
                runSpace.Open();
                PowerShell psh = PowerShell.Create();
                Console.WriteLine("Connected to {0}", sRemote);
                Console.WriteLine("As {0}", user);
                Console.Write("Command to run: ");
                var cmd = Console.ReadLine();
                PSCommand command = new PSCommand();
                command.AddArgument(@"echo ""aaa""  .\a.txt ");
                psh.Commands = command;
                var returnValue = psh.Invoke();
                foreach (var v in returnValue)
                    Console.WriteLine(v.ToString());
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
