using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace Twilio.Conversations
{
	static class CFunctions
	{
		// CGAffineTransform TWCVideoOrientationMakeTransform (TWCVideoOrientation orientation);
		[DllImport("__Internal")]
		static extern CoreGraphics.CGAffineTransform VideoOrientationMakeTransform(VideoOrientation orientation);

		// BOOL TWCVideoOrientationIsRotated (TWCVideoOrientation orientation);
		[DllImport("__Internal")]
		static extern bool TWCVideoOrientationIsRotated(VideoOrientation orientation);
	}

	[Native]
	public enum VideoCaptureSource : ulong
	{
		FrontCamera = 0,
		BackCamera
	}

	[Native]
	public enum VideoOrientation : ulong
	{
		Up = 0,
		Left,
		Down,
		Right
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
		Off = 0,
		Fatal,
		Error,
		Warning,
		Info,
		Debug,
		Trace,
		All
	}

	[Native]
	public enum LogModule : ulong
	{
		Core = 0,
		Platform,
		Signaling,
		WebRTC
	}

	public enum ErrorCode : long
	{
		Unknown = -1,
		InvalidAuthData = 100,
		InvalidSIPAccount = 102,
		ClientRegistration = 103,
		InvalidConversation = 105,
		ConversationParticipantNotAvailable = 106,
		ConversationRejected = 107,
		ConversationIgnored = 108,
		ConversationFailed = 109,
		ConversationTerminated = 110,
		PeerConnectFailed = 111,
		InvalidParticipantAddresses = 112,
		ClientDisconnected = 200,
		ClientFailedToReconnect = 201,
		TooManyActiveConversations = 202,
		TooManyTracks = 300,
		InvalidVideoCapturer = 301,
		InvalidVideoTrack = 302,
		VideoFailed = 303,
		TrackOperationInProgress = 304,
		TrackAddFailed = 305
	}

	[Native]
	public enum AudioOutput : ulong
	{
		Default = 0,
		Speaker = 1,
		Receiver = 2,
		Application = 3
	}

	[Native]
	public enum IceTransportPolicy : ulong
	{
		All = 0,
		Relay = 1
	}
}