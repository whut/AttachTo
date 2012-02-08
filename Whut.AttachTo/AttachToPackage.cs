using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace Whut.AttachTo
{
    //// This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    //// This attribute is used to register the informations needed to show the this package in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    //// This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidAttachToPkgString)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), "Whut.AttachTo", "General", 110, 120, false)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    public sealed class AttachToPackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            this.AddAttachToIISCommand(mcs);
            this.AddAttachToIISExpressCommand(mcs);
            this.AddAttachToNUnitCommand(mcs);
        }

        private void AddAttachToIISCommand(OleMenuCommandService mcs)
        {
            CommandID attachToIISMenuCommandID = new CommandID(GuidList.guidAttachToCmdSet, (int)PkgCmdIDList.cmdidWhutAttachToIIS);
            OleMenuCommand attachToIISMenuItem = new OleMenuCommand(
                delegate(object sender, EventArgs e)
                {
                    DTE dte = (DTE)this.GetService(typeof(DTE));
                    foreach (Process process in dte.Debugger.LocalProcesses)
                    {
                        if (process.Name.EndsWith("w3wp.exe"))
                        {
                            process.Attach();
                        }
                    }
                },
                attachToIISMenuCommandID);
            attachToIISMenuItem.BeforeQueryStatus += (s, e) => attachToIISMenuItem.Visible = ((GeneralOptionsPage)this.GetDialogPage(typeof(GeneralOptionsPage))).ShowAttachToIIS;
            mcs.AddCommand(attachToIISMenuItem);
        }

        private void AddAttachToIISExpressCommand(OleMenuCommandService mcs)
        {
            CommandID attachToIISExpressMenuCommandID = new CommandID(GuidList.guidAttachToCmdSet, (int)PkgCmdIDList.cmdidWhutAttachToIISExpress);
            OleMenuCommand attachToIISExpressMenuItem = new OleMenuCommand(
                delegate(object sender, EventArgs e)
                {
                    DTE dte = (DTE)this.GetService(typeof(DTE));
                    foreach (Process process in dte.Debugger.LocalProcesses)
                    {
                        if (process.Name.EndsWith("iisexpress.exe"))
                        {
                            process.Attach();
                        }
                    }
                },
                attachToIISExpressMenuCommandID);
            attachToIISExpressMenuItem.BeforeQueryStatus += (s, e) => attachToIISExpressMenuItem.Visible = ((GeneralOptionsPage)this.GetDialogPage(typeof(GeneralOptionsPage))).ShowAttachToIISExpress;
            mcs.AddCommand(attachToIISExpressMenuItem);
        }

        private void AddAttachToNUnitCommand(OleMenuCommandService mcs)
        {
            CommandID attachToNUnitMenuCommandID = new CommandID(GuidList.guidAttachToCmdSet, (int)PkgCmdIDList.cmdidWhutAttachToNUnit);
            OleMenuCommand attachToNUnitMenuItem = new OleMenuCommand(
                delegate(object sender, EventArgs e)
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
                        nunitProcesses.ForEach(p => p.Attach());
                    }
                },
                attachToNUnitMenuCommandID);
            attachToNUnitMenuItem.BeforeQueryStatus += (s, e) => attachToNUnitMenuItem.Visible = ((GeneralOptionsPage)this.GetDialogPage(typeof(GeneralOptionsPage))).ShowAttachToNUnit;
            mcs.AddCommand(attachToNUnitMenuItem);
        }
    }
}
