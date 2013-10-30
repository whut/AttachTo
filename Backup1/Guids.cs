// Guids.cs
// MUST match guids.h
using System;

namespace Whut.AttachTo
{
    public static class GuidList
    {
        public const string guidAttachToPkgString = "8d6080f0-7276-44d7-8dc4-6378fb6ce225";

        public const string guidAttachToCmdSetString = "16e2ac5c-ec3d-4ff1-a237-11ccef54fe0f";

        public static readonly Guid guidAttachToCmdSet = new Guid(guidAttachToCmdSetString);
    }
}