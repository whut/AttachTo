using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachToPackage.cs" company="">
//   
// </copyright>
// <summary>
//   This is the class that implements the package exposed by this assembly.
//   The minimum requirement for a class to be considered a valid package for Visual Studio
//   is to implement the IVsPackage interface and register itself with the shell.
//   This package uses the helper classes defined inside the Managed Package Framework (MPF)
//   to do it: it derives from the Package class that provides the implementation of the
//   IVsPackage interface and uses the registration attributes defined in the framework to
//   register itself and its components with the shell.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Whut.AttachTo
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;

    using EnvDTE;

    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;

    using Process = EnvDTE.Process;

    /// <summary>
    ///     This is the class that implements the package exposed by this assembly.
    ///     The minimum requirement for a class to be considered a valid package for Visual Studio
    ///     is to implement the IVsPackage interface and register itself with the shell.
    ///     This package uses the helper classes defined inside the Managed Package Framework (MPF)
    ///     to do it: it derives from the Package class that provides the implementation of the
    ///     IVsPackage interface and uses the registration attributes defined in the framework to
    ///     register itself and its components with the shell.
    /// </summary>
    //// This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    //// a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    //// This attribute is used to register the information needed to show this package
    //// in the Help/About dialog of Visual Studio.
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
        ///     Initialization of the package; this method is called right after the package is sited, so this is the place
        ///     where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var oleMenuCommandService = this.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (oleMenuCommandService == null)
            {
                return;
            }
            
            // Create the command for the menu item.
            this.AddAttachToCommand(
                oleMenuCommandService, 
                PkgCmdIDList.cmdidWhutAttachToIIS, 
                gop => gop.ShowAttachToIIS, 
                "w3wp.exe");

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
            IMenuCommandService oleMenuCommandService, 
            uint commandId, 
            Func<GeneralOptionsPage, bool> isVisible, 
            params string[] programsToAttach)
        {
            Contract.Requires<ArgumentNullException>(oleMenuCommandService != null);

            var menuItemCommand = new OleMenuCommand(
                delegate
                    {
                        // ReSharper disable once IdentifierTypo
                        var dte = (DTE)this.GetService(typeof(DTE));
                        foreach (var process in
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