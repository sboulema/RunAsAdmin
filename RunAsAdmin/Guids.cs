// Guids.cs
// MUST match guids.h
using System;

namespace FundaRealEstateBV.RunAsAdmin
{
    static class GuidList
    {
        public const string guidRunAsAdminPkgString = "f6211522-a083-49ba-aba4-9c082ab8a570";
        public const string guidRunAsAdminCmdSetString = "69afcb2a-c6c8-4934-bdab-816ed79bd026";

        public static readonly Guid guidRunAsAdminCmdSet = new Guid(guidRunAsAdminCmdSetString);
    };
}