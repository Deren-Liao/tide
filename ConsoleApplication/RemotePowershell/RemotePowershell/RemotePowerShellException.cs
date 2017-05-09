using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotePowershell
{
    public class RemotePowerShellAccessDeniedException : Exception
    {
        public RemotePowerShellAccessDeniedException(Exception ex) : base("access denied", ex) { }
    }
}
