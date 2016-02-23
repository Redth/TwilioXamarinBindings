using ObjCRuntime;

[assembly: LinkWith ("libTwilioCommon.a", 
    LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.Arm64,
    Frameworks = "",
    LinkerFlags = "",
    SmartLink = true, 
    ForceLoad = true)]
