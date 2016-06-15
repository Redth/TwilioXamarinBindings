using System;
using ObjCRuntime;

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

[Native]
public enum LogLevel: ulong
{
	Fatal = 0,
	Critical,
	Warning,
	Info,
	Debug
}

[Native]
public enum UserInfoUpdate: ulong
{
	FriendlyName,
	Attributes
}