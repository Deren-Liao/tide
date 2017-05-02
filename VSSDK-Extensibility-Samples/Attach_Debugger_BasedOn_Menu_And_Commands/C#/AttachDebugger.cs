using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System;
using System.Runtime.InteropServices;
//using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

using System.Windows.Automation;

using dag = System.Diagnostics;

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ComponentModel;

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Samples.VisualStudio.MenuCommands
{
    #region commentout
    ///// <summary>
    ///// Class to impersonate another user. Requires user, pass and domain/computername
    ///// All code run after impersonationuser has been run will run as this user.
    ///// Remember to Dispose() afterwards.
    ///// </summary>
    //public class ImpersonateUser:IDisposable {

    //    private WindowsImpersonationContext LastContext = null;
    //    private IntPtr LastUserHandle = IntPtr.Zero;

    //    #region User Impersonation api
    //    [DllImport("advapi32.dll", SetLastError = true)]
    //    public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);

    //    [DllImport("advapi32.dll", SetLastError = true)]
    //    public static extern bool ImpersonateLoggedOnUser(int Token);

    //    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //    public static extern bool DuplicateToken(IntPtr token, int impersonationLevel, ref IntPtr duplication);

    //    [DllImport("kernel32.dll")]
    //    public static extern Boolean CloseHandle(IntPtr hObject);

    //    public const int LOGON32_PROVIDER_DEFAULT = 0;
    //    public const int LOGON32_PROVIDER_WINNT35 = 1;
    //    public const int LOGON32_LOGON_INTERACTIVE = 2;
    //    public const int LOGON32_LOGON_NETWORK = 3;
    //    public const int LOGON32_LOGON_BATCH = 4;
    //    public const int LOGON32_LOGON_SERVICE = 5;
    //    public const int LOGON32_LOGON_UNLOCK = 7;
    //    public const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;// Win2K or higher
    //    public const int LOGON32_LOGON_NEW_CREDENTIALS = 9;// Win2K or higher
    //    #endregion

    //    public ImpersonateUser(string username, string domainOrComputerName, string password, int nm = LOGON32_LOGON_NETWORK) {

    //        IntPtr userToken = IntPtr.Zero;
    //        IntPtr userTokenDuplication = IntPtr.Zero;

    //        bool loggedOn = false;

    //        if (domainOrComputerName == null) domainOrComputerName = Environment.UserDomainName;

    //        if (domainOrComputerName.ToLower() == "nt authority") {
    //            loggedOn = LogonUser(username, domainOrComputerName, password, LOGON32_LOGON_SERVICE, LOGON32_PROVIDER_DEFAULT, out userToken);
    //        } else {
    //            loggedOn = LogonUser(username, domainOrComputerName, password, nm, LOGON32_PROVIDER_DEFAULT, out userToken);
    //        }

    //        WindowsImpersonationContext _impersonationContext = null;
    //        if (loggedOn) {
    //            try {
    //                // Create a duplication of the usertoken, this is a solution
    //                // for the known bug that is published under KB article Q319615.
    //                if (DuplicateToken(userToken, 2, ref userTokenDuplication)) {
    //                    // Create windows identity from the token and impersonate the user.
    //                    WindowsIdentity identity = new WindowsIdentity(userTokenDuplication);
    //                    _impersonationContext = identity.Impersonate();
    //                } else {
    //                    // Token duplication failed!
    //                    // Use the default ctor overload
    //                    // that will use Mashal.GetLastWin32Error();
    //                    // to create the exceptions details.
    //                    throw new Win32Exception();
    //                }
    //            } finally {
    //                // Close usertoken handle duplication when created.
    //                if (!userTokenDuplication.Equals(IntPtr.Zero)) {
    //                    // Closes the handle of the user.
    //                    CloseHandle(userTokenDuplication);
    //                    userTokenDuplication = IntPtr.Zero;
    //                }

    //                // Close usertoken handle when created.
    //                if (!userToken.Equals(IntPtr.Zero)) {
    //                    // Closes the handle of the user.
    //                    CloseHandle(userToken);
    //                    userToken = IntPtr.Zero;
    //                }
    //            }
    //        } else {
    //            // Logon failed!
    //            // Use the default ctor overload that 
    //            // will use Mashal.GetLastWin32Error();
    //            // to create the exceptions details.
    //            throw new Win32Exception();
    //        }

    //        if (LastContext == null) LastContext = _impersonationContext;
    //    }

    //    public void Dispose() {
    //        LastContext.Undo();
    //        LastContext.Dispose();
    //    }
    //    }


    //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    //public class Impersonation : IDisposable
    //{
    //    private readonly SafeTokenHandle _handle;
    //    private readonly WindowsImpersonationContext _context;

    //    public const int LOGON32_PROVIDER_DEFAULT = 0;
    //    public const int LOGON32_PROVIDER_WINNT35 = 1;
    //    public const int LOGON32_LOGON_INTERACTIVE = 2;
    //    public const int LOGON32_LOGON_NETWORK = 3;
    //    public const int LOGON32_LOGON_BATCH = 4;
    //    public const int LOGON32_LOGON_SERVICE = 5;
    //    public const int LOGON32_LOGON_UNLOCK = 7;
    //    public const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;// Win2K or higher
    //    public const int LOGON32_LOGON_NEW_CREDENTIALS = 9;// Win2K or higher

    //    public Impersonation(string domain, string username, string password)
    //    {
    //        var ok = LogonUser(username, domain, password,
    //                       LOGON32_LOGON_NEW_CREDENTIALS, 0, out this._handle);
    //        if (!ok)
    //        {
    //            var errorCode = Marshal.GetLastWin32Error();
    //            throw new ApplicationException(
    //                string.Format("Could not impersonate the elevated user.  LogonUser returned error code {0}.",
    //                errorCode));
    //        }

    //        this._context = WindowsIdentity.Impersonate(this._handle.DangerousGetHandle());
    //    }

    //    public void Dispose()
    //    {
    //        this._context.Dispose();
    //        this._handle.Dispose();
    //    }

    //    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    //    private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

    //    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    //    {
    //        private SafeTokenHandle()
    //            : base(true) { }

    //        [DllImport("kernel32.dll")]
    //        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    //        [SuppressUnmanagedCodeSecurity]
    //        [return: MarshalAs(UnmanagedType.Bool)]
    //        private static extern bool CloseHandle(IntPtr handle);

    //        protected override bool ReleaseHandle()
    //        {
    //            return CloseHandle(handle);
    //        }
    //    }
    //}
    #endregion

    public class VSDebugger
    {
        private static Lazy<VSDebugger> s_instance = new Lazy<VSDebugger>();

        private const string RemoteName = "35.185.240.69"; 
        private const string ProcessName = "w3wp.exe";
        private string password = s_lazySecret.Value;

        public static void AttachTest()
        {
            s_instance.Value.AttachToRemoteProcess(RemoteName, ProcessName);
            //s_instance.Value.AttachToRemoteProcessV4(RemoteName, ProcessName);
            //using (var impersonation =
            //    new ImpersonateUser("derenz",
            //            RemoteName /*OtherMachineName*/,
            //            password /*Password*/,
            //            ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
            //{
            //    s_instance.Value.LaunchDebugger(RemoteName, ProcessName);
            //}

            //using (new Impersonation("35.185.229.199", "deren", "Fm6X%R9[(|K|6iV"))
            //{
            //    s_instance.Value.LaunchDebugger(RemoteName, ProcessName);
            //}

            //s_instance.Value.LaunchDebugger(RemoteName, ProcessName);
            //s_instance.Value.LaunchDebuggerCommand(RemoteName, ProcessName);
        }

        private void AttachToRemoteProcess(string machineName, string processName)
        {
            IVsDebugger3 vsDebugger = Package.GetGlobalService(typeof(IVsDebugger)) as IVsDebugger3;
            VsDebugTargetInfo3[] arrDebugTargetInfo = new VsDebugTargetInfo3[1];
            VsDebugTargetProcessInfo[] arrTargetProcessInfo = new VsDebugTargetProcessInfo[1];

            arrDebugTargetInfo[0].bstrExe = processName;
            arrDebugTargetInfo[0].bstrRemoteMachine = machineName;
            arrDebugTargetInfo[0].dlo = (uint)DEBUG_LAUNCH_OPERATION.DLO_AlreadyRunning;
            arrDebugTargetInfo[0].guidLaunchDebugEngine = Guid.Empty;
            arrDebugTargetInfo[0].dwDebugEngineCount = 1;

            Guid guidDbgEngine = VSConstants.DebugEnginesGuids.ManagedAndNative_guid;
            IntPtr pGuids = Marshal.AllocCoTaskMem(Marshal.SizeOf(guidDbgEngine));
            Marshal.StructureToPtr(guidDbgEngine, pGuids, false);
            arrDebugTargetInfo[0].pDebugEngines = pGuids;
            int hr = vsDebugger.LaunchDebugTargets3(1, arrDebugTargetInfo, arrTargetProcessInfo);

            // cleanup
            if (pGuids != IntPtr.Zero)
                Marshal.FreeCoTaskMem(pGuids);
        }

        private void AttachToRemoteProcessV4(string machineName, string processName)
        {
            IVsDebugger4 vsDebugger = Package.GetGlobalService(typeof(IVsDebugger)) as IVsDebugger4;
            VsDebugTargetInfo4[] arrDebugTargetInfo = new VsDebugTargetInfo4[1];
            VsDebugTargetProcessInfo[] arrTargetProcessInfo = new VsDebugTargetProcessInfo[1];

            arrDebugTargetInfo[0].bstrExe = processName;
            arrDebugTargetInfo[0].bstrRemoteMachine = machineName;
            arrDebugTargetInfo[0].dlo = (uint)DEBUG_LAUNCH_OPERATION.DLO_AlreadyRunning;
            arrDebugTargetInfo[0].guidLaunchDebugEngine = Guid.Empty;
            arrDebugTargetInfo[0].dwDebugEngineCount = 1;
            arrDebugTargetInfo[0].LaunchFlags = (uint)__VSDBGLAUNCHFLAGS.DBGLAUNCH_DetachOnStop;

            Guid guidDbgEngine = VSConstants.DebugEnginesGuids.ManagedAndNative_guid;
            IntPtr pGuids = Marshal.AllocCoTaskMem(Marshal.SizeOf(guidDbgEngine));
            Marshal.StructureToPtr(guidDbgEngine, pGuids, false);
            arrDebugTargetInfo[0].pDebugEngines = pGuids;
            var t = new System.Threading.Thread(() =>
            {
                vsDebugger.LaunchDebugTargets4(1, arrDebugTargetInfo, arrTargetProcessInfo);
            });

            t.Start();

            InputUserPassword();

            t.Join();

            // cleanup
            if (pGuids != IntPtr.Zero)
                Marshal.FreeCoTaskMem(pGuids);
        }



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

        // List process and attach.
        private void LaunchDebugger(string machineName, string processName)
        {
            Processes processes;
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            var debugger = dte.Debugger as Debugger2;
            var transport = debugger.Transports.Item("Default");
            processes = debugger.GetProcesses(transport, machineName);

            foreach (var process in processes)
            {
                var pro = process as Process;
                var pro2 = process as Process2;
                dag.Debug.WriteLine($"name {pro2.Name}");
                var name = Path.GetFileName(pro2.Name);
                if (name.ToLower() == processName.ToLower())
                {
                    pro2.Attach();
                    break;
                }
            }
        }

        private void LaunchDebuggerCommand(string machineName, string processName)
        {
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            dte.ExecuteCommand("Debug.AttachToProcess");    // The command does not accept parameters
        }


        // UI automation to fill password
        private void LaunchDebuggerAutoFillPassword(string machineName, string processName)
        {
            Processes processes = null;
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            var debugger = dte.Debugger as Debugger2;
            var transport = debugger.Transports.Item("Default");
            var t = new System.Threading.Thread(() =>
            {
                try
                {
                    processes = debugger.GetProcesses(transport, machineName);
                }
                catch (Exception ex)
                {}
            });

            t.Start();

            InputUserPassword();

            t.Join();


            foreach(var process in processes)
            {
                var pro = process as Process;
                var pro2 = process as Process2;
                dag.Debug.WriteLine($"name {pro2.Name}");
                var name = Path.GetFileName(pro2.Name);
                if (name.ToLower() == processName.ToLower())
                {
                    pro2.Attach();
                    break;
                }
            }
        }


        private void InputUserPassword()
        {
            AutomationElement desktop = AutomationElement.RootElement;
            //get all windows on the desktop
            AutomationElementCollection windows = 
                desktop.FindAll(TreeScope.Descendants, 
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            foreach (AutomationElement window in windows)
            {
                if (window.Current.ClassName.Equals("#32770"))   //security dialog
                {

                    // access the appropriate AutomationElements to enter credentials here

                }
            }
        }

    }
}
