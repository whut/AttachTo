namespace Whut.AttachTo
{
    // PkgCmdID.cs
    // MUST match PkgCmdID.h

    /// <summary>
    /// Class PkgCmdIDList.
    /// </summary>
    public static class PkgCmdIDList
    {
        /// <summary>
        /// The cmdid whut attach automatic IIS
        /// </summary>
        public const uint cmdidWhutAttachToIIS = 0x100;

        /// <summary>
        /// The cmdid whut attach automatic IIS express
        /// </summary>
        public const uint cmdidWhutAttachToIISExpress = 0x101;

        /// <summary>
        /// The cmdid whut attach automatic asynchronous unit
        /// </summary>
        public const uint cmdidWhutAttachToNUnit = 0x102;
    }
}