namespace Whut.AttachTo
{
    // Guids.cs
    // MUST match guids.h
    using System;

    /// <summary>
    /// Class GuidList.
    /// </summary>
    public static class GuidList
    {
        /// <summary>
        /// The unique identifier attach automatic PKG string
        /// </summary>
        public const string guidAttachToPkgString = "8d6080f0-7276-44d7-8dc4-6378fb6ce225";

        /// <summary>
        /// The unique identifier attach automatic command set string
        /// </summary>
        public const string guidAttachToCmdSetString = "16e2ac5c-ec3d-4ff1-a237-11ccef54fe0f";

        /// <summary>
        /// The unique identifier attach automatic command set
        /// </summary>
        public static readonly Guid guidAttachToCmdSet = new Guid(guidAttachToCmdSetString);
    }
}