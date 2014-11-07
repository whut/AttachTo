namespace Whut.AttachTo
{
    using System.ComponentModel;

    using Microsoft.VisualStudio.Shell;

    /// <summary>
    ///     Class GeneralOptionsPage.
    /// </summary>
    public class GeneralOptionsPage : DialogPage
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GeneralOptionsPage" /> class.
        /// </summary>
        public GeneralOptionsPage()
        {
            this.ShowAttachToIIS = true;
            this.ShowAttachToIISExpress = true;
            this.ShowAttachToNUnit = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether [show attach automatic IIS].
        /// </summary>
        /// <value><c>true</c> if [show attach automatic IIS]; otherwise, <c>false</c>.</value>
        [Category("General")]
        [DisplayName("Show 'Attach to IIS' command")]
        [Description("Show 'Attach to IIS' command in Tools menu.")]
        [DefaultValue(true)]
        public bool ShowAttachToIIS { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [show attach automatic IIS express].
        /// </summary>
        /// <value><c>true</c> if [show attach automatic IIS express]; otherwise, <c>false</c>.</value>
        [Category("General")]
        [DisplayName("Show 'Attach to IIS Express command")]
        [Description("Show 'Attach to IIS Express command in Tools menu.")]
        [DefaultValue(true)]
        public bool ShowAttachToIISExpress { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [show attach automatic asynchronous unit].
        /// </summary>
        /// <value><c>true</c> if [show attach automatic asynchronous unit]; otherwise, <c>false</c>.</value>
        [Category("General")]
        [DisplayName("Show 'Attach to NUnit' command")]
        [Description("Show 'Attach to NUnit' command in Tools menu.")]
        [DefaultValue(true)]
        public bool ShowAttachToNUnit { get; set; }

        #endregion
    }
}