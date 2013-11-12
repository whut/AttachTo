namespace Whut.AttachTo
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;

    using EnvDTE;

    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    ///     Class AttachToPackage. This class cannot be inherited.
    /// </summary>
    //// This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    //// This attribute is used to register the information needed to show the this package in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    //// This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidAttachToPkgString)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), "Whut.AttachTo", "General", 110, 120, false)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    public sealed class AttachToPackage : Package
    {
        #region Methods

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            var oleMenuCommandService = this.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            this.AddAttachToCommand(oleMenuCommandService, PkgCmdIDList.cmdidWhutAttachToIIS, gop => gop.ShowAttachToIIS, "w3wp.exe");
            this.AddAttachToCommand(
                oleMenuCommandService, 
                PkgCmdIDList.cmdidWhutAttachToIISExpress, 
                gop => gop.ShowAttachToIISExpress, 
                "iisexpress.exe");
            this.AddAttachToCommand(
                oleMenuCommandService, 
                PkgCmdIDList.cmdidWhutAttachToNUnit, 
                gop => gop.ShowAttachToNUnit, 
                "nunit-agent.exe", 
                "nunit.exe", 
                "nunit-console.exe", 
                "nunit-agent-x86.exe", 
                "nunit-x86.exe", 
                "nunit-console-x86.exe");
        }

        /// <summary>
        /// Adds the attach automatic command.
        /// </summary>
        /// <param name="oleMenuCommandService">
        /// The MCS.
        /// </param>
        /// <param name="commandId">
        /// The command unique identifier.
        /// </param>
        /// <param name="isVisible">
        /// The is visible.
        /// </param>
        /// <param name="programsToAttach">
        /// The programs automatic attach.
        /// </param>
        private void AddAttachToCommand(
            OleMenuCommandService oleMenuCommandService, 
            uint commandId, 
            Func<GeneralOptionsPage, bool> isVisible, 
            params string[] programsToAttach)
        {
            Contract.Requires<ArgumentNullException>(oleMenuCommandService != null);

            var menuItemCommand = new OleMenuCommand(
                delegate
                    {
                        var dte = (DTE)this.GetService(typeof(DTE));
                        foreach (
                            var process in
                                dte.Debugger.LocalProcesses.Cast<Process>()
                                    .Where(process => programsToAttach.Any(p => process.Name.EndsWith(p))))
                        {
                            process.Attach();
                        }
                    }, 
                new CommandID(GuidList.guidAttachToCmdSet, (int)commandId));
            menuItemCommand.BeforeQueryStatus +=
                (s, e) =>
                menuItemCommand.Visible = isVisible((GeneralOptionsPage)this.GetDialogPage(typeof(GeneralOptionsPage)));
            oleMenuCommandService.AddCommand(menuItemCommand);
        }

        #endregion
    }
}