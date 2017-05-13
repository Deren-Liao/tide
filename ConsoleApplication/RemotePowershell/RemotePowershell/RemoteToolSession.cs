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
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCloudExtension.RemotePowerShell
{
    /// <summary>
    /// An open session to a remote target that starts msvsmon.exe
    /// </summary>
    public class RemoteToolSession
    {
        private readonly RemoteTarget _target;
        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private readonly Task _powerShellTask;

        public bool IsEnded => _powerShellTask.IsCompleted || _powerShellTask.IsCanceled || _powerShellTask.IsFaulted;

        public RemoteToolSession(string computerName, string username, SecureString password)
        {
            _target = new RemoteTarget(computerName, username, password);
            string script = Utils.GetScript("RemotePowershell.Resources.StartRemoteTool.ps1");
            _powerShellTask = Task.Run(() =>_target.EnterSessionExecute(script, _cancelTokenSource.Token));
        }

        public void Stop()
        {
            if (!IsEnded)
            {
                Debug.WriteLine($"_cancelTokenSource.Cancel() ");
                _cancelTokenSource.Cancel();
            }
            Debug.WriteLine($"_powerShellTask.Wait() ");
            _powerShellTask.Wait();
            Debug.WriteLine($"_powerShellTask.Wait() complete.");
        }
    }
}
