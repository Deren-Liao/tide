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
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCloudExtension.RemotePowerShell
{
    /// <summary>
    /// Installs Visual Studio Remote Debugger Tool.
    /// </summary>
    public class RemoteToolInstaller : RemoteTarget
    {
        private static string LocalDebuggerPath => @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Remote Debugger\x64\";
        private string DestinationPath => $@"c:\users\{_userName}\documents\RemoteDebugger";

        public RemoteToolInstaller(string computerName, string username, SecureString password)
            : base (computerName, username, password)
        { }

        public async Task<bool> Install()
        {
            return await ExecuteAsync(AddInstallCommands);
        }

        private void AddInstallCommands(PowerShell powerShell)
        {
            var setupScript = Utils.GetScript("RemotePowershell.Resources.InstallRemoteTool.ps1");

            powerShell.AddCommand("Set-Variable");
            powerShell.AddParameter("Name", "cred");
            powerShell.AddParameter("Value", _credential);

            powerShell.AddScript(@"$sessionOptions = New-PSSessionOption –SkipCACheck –SkipCNCheck –SkipRevocationCheck");
            powerShell.AddScript($@"$session = New-PSSession {_computerName} -UseSSL -Credential $cred -SessionOption $sessionOptions");

            powerShell.AddScript(setupScript);
            powerShell.AddScript(@"Remove-PSSession -Session $session");
        }
    }
}
