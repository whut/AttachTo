using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Whut.AttachTo
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidAttachToPkgString)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), "Whut.AttachTo", "General", 110, 120, false)]
    public sealed class AttachToPackage : Package
    {
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            AddAttachToIISCommand(mcs);
            AddAttachToIISExpressCommand(mcs);
            AddAttachToNUnitCommand(mcs);
        }

        private void AddAttachToIISCommand(OleMenuCommandService mcs)
        {
            CommandID attachToIISMenuCommandID = new CommandID(GuidList.guidAttachToCmdSet, (int)PkgCmdIDList.cmdidWhutAttachToIIS);
            OleMenuCommand attachToIISMenuItem = new OleMenuCommand(AttachToIISMenuItemCallback, attachToIISMenuCommandID);
            attachToIISMenuItem.BeforeQueryStatus += (s, e) => attachToIISMenuItem.Visible = ((GeneralOptionsPage)this.GetDialogPage(typeof(GeneralOptionsPage))).ShowAttachToIIS;
            mcs.AddCommand(attachToIISMenuItem);
        }

        private void AddAttachToIISExpressCommand(OleMenuCommandService mcs)
        {
            CommandID attachToIISExpressMenuCommandID = new CommandID(GuidList.guidAttachToCmdSet, (int)PkgCmdIDList.cmdidWhutAttachToIISExpress);
            OleMenuCommand attachToIISExpressMenuItem = new OleMenuCommand(AttachToIISExpressMenuItemCallback, attachToIISExpressMenuCommandID);
            attachToIISExpressMenuItem.BeforeQueryStatus += (s, e) => attachToIISExpressMenuItem.Visible = ((GeneralOptionsPage)this.GetDialogPage(typeof(GeneralOptionsPage))).ShowAttachToIISExpress;
            mcs.AddCommand(attachToIISExpressMenuItem);
        }

        private void AddAttachToNUnitCommand(OleMenuCommandService mcs)
        {
            CommandID attachToNUnitMenuCommandID = new CommandID(GuidList.guidAttachToCmdSet, (int)PkgCmdIDList.cmdidWhutAttachToNUnit);
            OleMenuCommand attachToNUnitMenuItem = new OleMenuCommand(AttachToNUnitMenuItemCallback, attachToNUnitMenuCommandID);
            attachToNUnitMenuItem.BeforeQueryStatus += (s, e) => attachToNUnitMenuItem.Visible = ((GeneralOptionsPage)this.GetDialogPage(typeof(GeneralOptionsPage))).ShowAttachToNUnit;
            mcs.AddCommand(attachToNUnitMenuItem);
        }

        private void AttachToIISMenuItemCallback(object sender, EventArgs e)
        {
            DTE dte = (DTE)this.GetService(typeof(DTE));
            foreach (Process process in dte.Debugger.LocalProcesses)
            {
                if (process.Name.EndsWith("w3wp.exe"))
                {
                    process.Attach();
                }
            }
        }

        private void AttachToIISExpressMenuItemCallback(object sender, EventArgs e)
        {
            DTE dte = (DTE)this.GetService(typeof(DTE));
            foreach (Process process in dte.Debugger.LocalProcesses)
            {
                if (process.Name.EndsWith("iisexpress.exe"))
                {
                    process.Attach();
                }
            }
        }

        private void AttachToNUnitMenuItemCallback(object sender, EventArgs e)
        {
            // NUnit runs test in nunit.exe or in separate process, nunit-agent.exe - it depends on configuration, target framework version, etc.
            // Code below attaches to nunit-agent.exe processes, but when it can't find any, it attaches to nunit.exe as fallback
            DTE dte = (DTE)this.GetService(typeof(DTE));
            List<Process> nunitProcesses = new List<Process>();
            bool nunitAgentFound = false;
            foreach (Process process in dte.Debugger.LocalProcesses)
            {
                if (process.Name.EndsWith("nunit-agent.exe"))
                {
                    process.Attach();
                    nunitAgentFound = true;
                }

                if (process.Name.EndsWith("nunit.exe"))
                {
                    nunitProcesses.Add(process);
                }
            }

            if (!nunitAgentFound)
            {
                foreach (Process process in nunitProcesses)
                {
                    process.Attach();
                }
            }
        }
    }
}
