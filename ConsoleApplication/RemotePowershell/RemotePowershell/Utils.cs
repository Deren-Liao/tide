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
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace GoogleCloudExtension.RemotePowerShell
{
    /// <summary>
    /// Utilities for remote powershell operations.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Gets the embedded resource text content.
        /// </summary>
        /// <param name="resourceName">
        /// i.e GoogleCloudExtension.RemotePowershell.Resources.EmbededScript.ps1
        /// </param>
        /// <returns>The text content of the embeded resource file</returns>
        /// <exception cref="FileNotFoundException">The file of <paramref name="resourceName"/> is not found.</exception>
        public static string GetScript(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine(String.Join(";", assembly.GetManifestResourceNames()));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException(resourceName);
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Create <seealso cref="PSCredential"/> object from username and password.
        /// </summary>
        public static PSCredential CreatePSCredential(string user, string password)
        {
            return new PSCredential(user, ConvertToSecureString(password));
        }

        /// <summary>
        /// Create <seealso cref="PSCredential"/> object from username and secured password.
        /// </summary>
        public static PSCredential CreatePSCredential(string user, SecureString securePassword)
        {
            return new PSCredential(user, securePassword);
        }

        /// <summary>
        /// Convert string to secure string.
        /// </summary>
        public static SecureString ConvertToSecureString(string input)
        {
            // TODO: validate input not null empty.
            SecureString output = new SecureString();
            input?.ToCharArray().ToList().ForEach(p => output.AppendChar(p));
            return output;
        }
    }
}
