using System;
using ObjCRuntime;

namespace Twilio.Conversations 
{
    [Native]
    public enum VideoCaptureSource : ulong
    {
        FrontCamera = 0,
        BackCamera
    }

    [Native]
    public enum InviteStatus : ulong
    {
        Pending = 0,
        Accepting,
        Accepted,
        Rejected,
        Cancelled,
        Failed
    }

    [Native]
    public enum MediaTrackState : ulong
    {
        Idle = 0,
        Starting,
        Started,
        Ending,
        Ended
    }

    [Native]
    public enum LogLevel : ulong
    {
        Disabled = 0,
        Error = 3,
        Warning = 4,
        Info = 6,
        Debug = 7,
        Verbose = 8
    }

    [Native]
    public enum AudioOutput : ulong
    {
        Default = 0,
        Speaker = 1,
        Receiver = 2
    }
}