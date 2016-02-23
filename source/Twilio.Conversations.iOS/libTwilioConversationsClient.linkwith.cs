using ObjCRuntime;

[assembly: LinkWith ("libTwilioConversationsClient.a", 
    LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.Arm64,
    Frameworks = "CoreTelephony VideoToolbox CoreMedia CoreVideo QuartzCore ImageIO CoreText AssetsLibrary MobileCoreServices CoreImage GLKit OpenGLES QuickLook UIKit Foundation CoreGraphics AudioToolbox CFNetwork SystemConfiguration AVFoundation",
    LinkerFlags = "",
    IsCxx = true,
    SmartLink = true, 
    ForceLoad = true)]
