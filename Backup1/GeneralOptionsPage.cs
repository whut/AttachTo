using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Whut.AttachTo
{
    public class GeneralOptionsPage : DialogPage
    {
        public GeneralOptionsPage()
        {
            this.ShowAttachToIIS = true;
            this.ShowAttachToIISExpress = true;
            this.ShowAttachToNUnit = true;
        }

        [Category("General")]
        [DisplayName("Show 'Attach to IIS' command")]
        [Description("Show 'Attach to IIS' command in Tools menu.")]
        [DefaultValue(true)]
        public bool ShowAttachToIIS { get; set; }

        [Category("General")]
        [DisplayName("Show 'Attach to IIS Express command")]
        [Description("Show 'Attach to IIS Express command in Tools menu.")]
        [DefaultValue(true)]
        public bool ShowAttachToIISExpress { get; set; }

        [Category("General")]
        [DisplayName("Show 'Attach to NUnit' command")]
        [Description("Show 'Attach to NUnit' command in Tools menu.")]
        [DefaultValue(true)]
        public bool ShowAttachToNUnit { get; set; }
    }
}
