using System;
using ObjCRuntime;

[Native]
public enum Result : ulong
{
    Success = 0,
    Failure
}

[Native]
public enum ChannelStatus : ulong
{
    Invited = 0,
    Joined,
    NotParticipating
}

[Native]
public enum ChannelType : ulong
{
    Public = 0,
    Private
}
