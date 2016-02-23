using ObjCRuntime;

[assembly: LinkWith ("libTwilioIPMessagingClient.a",
    LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.Arm64,
    Frameworks = "",
    LinkerFlags = "",
    IsCxx = true,
    SmartLink = true, 
    ForceLoad = true)]
