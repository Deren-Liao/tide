using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnvDTE;
using EnvDTE80;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSOLEInterop = Microsoft.VisualStudio.OLE.Interop;

namespace Microsoft.Samples.VisualStudio.MenuCommands
{
    public static class Class1
    {
        private const string ProjectPath = @"c:\tmp";


        public static ServiceProvider GetGloblalServiceProvider()
        {
            var dte2 = (DTE2)Package.GetGlobalService(typeof(SDTE));
            VSOLEInterop.IServiceProvider sp = (VSOLEInterop.IServiceProvider)dte2;
            return new ServiceProvider(sp);
        }

        public static void CreateNewProject()
        {
            var dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE2;
            var locationItem = dte.Properties["Environment", "ProjectsAndSolution"].Item("ProjectsLocation");
            locationItem.Value = ProjectPath;

            var serviceProvider = GetGloblalServiceProvider();
            var solution = serviceProvider?.GetService(typeof(SVsSolution)) as IVsSolution;
            solution?.CreateNewProjectViaDlg(null, null, 0);

            //dte.ExecuteCommand("File.AddNewProject");
        }
    }
}
