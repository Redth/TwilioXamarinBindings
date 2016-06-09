using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace Twilio.IPMessaging
{
	// typedef void (^TWMCompletion)(TWMResult);
	delegate void Completion (Result result);

	// typedef void (^TWMChannelListCompletion)(TWMResult, TWMChannels *);
	delegate void ChannelListCompletion (Result result, Channels channels);

	// typedef void (^TWMChannelCompletion)(TWMResult, TWMChannel *);
	delegate void ChannelCompletion (Result result, Channel channel);

	[Static]
	partial interface Constants
	{
		// extern NSString *const TWMChannelOptionFriendlyName;
		[Field ("TWMChannelOptionFriendlyName", "__Internal")]
		NSString ChannelOptionFriendlyName { get; }

		// extern NSString *const TWMChannelOptionUniqueName;
		[Field ("TWMChannelOptionUniqueName", "__Internal")]
		NSString ChannelOptionUniqueName { get; }

		// extern NSString *const TWMChannelOptionType;
		[Field ("TWMChannelOptionType", "__Internal")]
		NSString ChannelOptionType { get; }

		// extern NSString *const TWMChannelOptionAttributes;
		[Field ("TWMChannelOptionAttributes", "__Internal")]
		NSString ChannelOptionAttributes { get; }

		// extern NSString *const TWMErrorDomain;
		[Field ("TWMErrorDomain", "__Internal")]
		NSString ErrorDomain { get; }

		// extern const NSInteger TWMErrorGeneric;
		[Field ("TWMErrorGeneric", "__Internal")]
		nint ErrorGeneric { get; }

		// extern NSString *const TWMErrorMsgKey;
		[Field ("TWMErrorMsgKey", "__Internal")]
		NSString ErrorMsgKey { get; }

		// extern NSString *const TWMErrorCodeKey;
		//[Field ("TWMErrorCodeKey", "__Internal")]
		//NSString ErrorCodeKey { get; }
	} 

	[BaseType (typeof (NSObject))]
	interface TwilioIPMessagingClient
	{
		// @property (nonatomic, strong) TwilioAccessManager *accessManager;
		[Export ("accessManager", ArgumentSemantic.Strong)]
		Twilio.Common.TwilioAccessManager AccessManager { get; set; }

		// @property (nonatomic, weak) id<TwilioIPMessagingClientDelegate> delegate;
		[NullAllowed]
		[Export ("delegate", ArgumentSemantic.Weak)]
		ITwilioIPMessagingClientDelegate Delegate { get; set; }

		// DEPRECATED
		// @property (nonatomic, copy, readonly) NSString *identity;
		//[Export ("identity", ArgumentSemantic.Copy)]
		//string Identity { get; }

		// @property (nonatomic, strong, readonly) TWMUserInfo *userInfo
		[Export ("userInfo", ArgumentSemantic.Strong)]
		UserInfo UserInfo { get; }

		// + (TWMLogLevel)logLevel
		// + (void)setLogLevel:(TWMLogLevel)logLevel
		[Static]
		[Export ("logLevel")]
		LogLevel LogLevel { get; set; }

		// + (TwilioIPMessagingClient *)ipMessagingClientWithToken:(NSString *)token delegate:(id<TwilioIPMessagingClientDelegate>)delegate;
		//[Static]
		//[Export ("ipMessagingClientWithToken:delegate:")]
		//TwilioIPMessagingClient Create (string token, ITwilioIPMessagingClientDelegate ipMessagingDelegate);

		// + (TwilioIPMessagingClient *)ipMessagingClientWithAccessManager:(TwilioAccessManager *)accessManager delegate:(id<TwilioIPMessagingClientDelegate>)delegate;
		[Static]
		[Export ("ipMessagingClientWithAccessManager:delegate:")]
		TwilioIPMessagingClient Create (Twilio.Common.TwilioAccessManager accessManager, ITwilioIPMessagingClientDelegate ipMessagingDelegate);

		// - (NSString *)version;
		[Export ("version")]
		string Version { get; }

		// - (void)channelsListWithCompletion:(TWMChannelListCompletion)completion;
		[Export ("channelsListWithCompletion:")]
		void GetChannelsList (ChannelListCompletion completion);

		// - (void)registerWithToken:(NSData *)token;
		[Export ("registerWithToken:")]
		void Register (NSData token);

		// - (void)deregisterWithToken:(NSData *)token;
		[Export ("deregisterWithToken:")]
		void Deregister (NSData token);

		// - (void)handleNotification:(NSDictionary *)notification;
		[Export ("handleNotification:")]
		void HandleNotification (NSDictionary notification);

		// - (void)shutdown;
		[Export ("shutdown")]
		void Shutdown ();
	}

	interface ITwilioIPMessagingClientDelegate { }

	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface TwilioIPMessagingClientDelegate
	{
		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channelAdded:(TWMChannel *)channel;
		[Export ("ipMessagingClient:channelAdded:")]
		void ChannelAdded (TwilioIPMessagingClient client, Channel channel);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channelChanged:(TWMChannel *)channel;
		[Export ("ipMessagingClient:channelChanged:")]
		void ChannelChanged (TwilioIPMessagingClient client, Channel channel);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channelDeleted:(TWMChannel *)channel;
		[Export ("ipMessagingClient:channelDeleted:")]
		void ChannelDeleted (TwilioIPMessagingClient client, Channel channel);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channelHistoryLoaded:(TWMChannel *)channel;
		[Export ("ipMessagingClient:channelHistoryLoaded:")]
		void ChannelHistoryLoaded (TwilioIPMessagingClient client, Channel channel);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel memberJoined:(TWMMember *)member;
		[Export ("ipMessagingClient:channel:memberJoined:")]
		void MemberJoined (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel memberChanged:(TWMMember *)member;
		[Export ("ipMessagingClient:channel:memberChanged:")]
		void MemberChanged (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel memberLeft:(TWMMember *)member;
		[Export ("ipMessagingClient:channel:memberLeft:")]
		void MemberLeft (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel messageAdded:(TWMMessage *)message;
		[Export ("ipMessagingClient:channel:messageAdded:")]
		void MessageAdded (TwilioIPMessagingClient client, Channel channel, Message message);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel messageChanged:(TWMMessage *)message;
		[Export ("ipMessagingClient:channel:messageChanged:")]
		void MessageChanged (TwilioIPMessagingClient client, Channel channel, Message message);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel messageDeleted:(TWMMessage *)message;
		[Export ("ipMessagingClient:channel:messageDeleted:")]
		void MessageDeleted (TwilioIPMessagingClient client, Channel channel, Message message);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client errorReceived:(TWMError *)error;
		[Export ("ipMessagingClient:errorReceived:")]
		void ErrorReceived (TwilioIPMessagingClient client, TwilioError error);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client typingStartedOnChannel:(TWMChannel *)channel member:(TWMMember *)member;
		[Export ("ipMessagingClient:typingStartedOnChannel:member:")]
		void TypingStarted (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client typingEndedOnChannel:(TWMChannel *)channel member:(TWMMember *)member;
		[Export ("ipMessagingClient:typingEndedOnChannel:member:")]
		void TypingEnded (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClientToastSubscribed:(TwilioIPMessagingClient *)client;
		[Export ("ipMessagingClientToastSubscribed:")]
		void ToastSubscribed (TwilioIPMessagingClient client);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client toastReceivedOnChannel:(TWMChannel *)channel message:(TWMMessage *)message;
		[Export ("ipMessagingClient:toastReceivedOnChannel:message:")]
		void ToastReceived (TwilioIPMessagingClient client, Channel channel, Message message);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client toastRegistrationFailedWithError:(TWMError *)error;
		[Export ("ipMessagingClient:toastRegistrationFailedWithError:")]
		void ToastRegistrationFailed (TwilioIPMessagingClient client, TwilioError error);

	}

	[BaseType (typeof (NSObject), Name="TWMChannel")]
	interface Channel
	{
		// @property (nonatomic, weak) id<TWMChannelDelegate> delegate;
		[NullAllowed]
		[Export ("delegate", ArgumentSemantic.Weak)]
		IChannelDelegate Delegate { get; }

		// @property (nonatomic, copy, readonly) NSString *sid;
		[Export ("sid", ArgumentSemantic.Copy)]
		string Sid { get; }

		// @property (nonatomic, copy, readonly) NSString *friendlyName;
		[Export ("friendlyName", ArgumentSemantic.Copy)]
		string FriendlyName { get; }

		// @property (nonatomic, copy, readonly) NSString *uniqueName;
		[Export ("uniqueName", ArgumentSemantic.Copy)]
		string UniqueName { get; }

		// @property (nonatomic, strong, readonly) TWMMessages *messages;
		[Export ("messages", ArgumentSemantic.Strong)]
		Messages Messages { get; }

		// @property (nonatomic, strong, readonly) TWMMembers *members;
		[Export ("members", ArgumentSemantic.Strong)]
		Members Members { get; }

		// @property (nonatomic, assign, readonly) TWMChannelStatus status;
		[Export ("status", ArgumentSemantic.Assign)]
		ChannelStatus Status { get; }

		// @property (nonatomic, assign, readonly) TWMChannelType type;
		[Export ("type", ArgumentSemantic.Assign)]
		ChannelType Type { get; }

		// - (NSDictionary<NSString *, id> *)attributes;
		[Export ("attributes")]
		NSDictionary Attributes { get; }

		// - (void)setAttributes:(NSDictionary<NSString *, id> *)attributes completion:(TWMCompletion)completion;
		[Export ("setAttributes:completion:")]
		void SetAttributes (NSDictionary attributes, Completion completion);

		// - (void)setFriendlyName:(NSString *)friendlyName completion:(TWMCompletion)completion;
		[Export ("setFriendlyName:completion:")]
		void SetFriendlyName (string friendlyName, Completion completion);


		// - (void)setUniqueName:(NSString *)uniqueName completion:(TWMCompletion)completion;
		[Export ("setUniqueName:completion:")]
		void SetUniqueName (string uniqueName, Completion completion);

		// - (void)joinWithCompletion:(TWMCompletion)completion;
		[Export ("joinWithCompletion:")]
		void Join (Completion completion);

		// - (void)declineInvitationWithCompletion:(TWMCompletion)completion;
		[Export ("declineInvitationWithCompletion:")]
		void DeclineInvitation (Completion completion);

		// - (void)leaveWithCompletion:(TWMCompletion)completion;
		[Export ("leaveWithCompletion:")]
		void Leave (Completion completion);

		// - (void)destroyWithCompletion:(TWMCompletion)completion;
		[Export ("destroyWithCompletion:")]
		void Destroy (Completion completion);

		// - (void)typing;
		[Export ("typing")]
		void Typing ();

		// - (TWMMember *)memberWithIdentity:(NSString *)identity
		[Export("memberWithIdentity:")]
		Member GetMember(string identity);
	}

	interface IChannelDelegate { }

	[Protocol, Model]
	[BaseType (typeof (NSObject), Name="TWMChannelDelegate")]
	interface ChannelDelegate
	{
		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channelChanged:(TWMChannel *)channel;
		[Export ("ipMessagingClient:channelChanged:")]
		void ChannelChanged (TwilioIPMessagingClient client, Channel channel);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channelDeleted:(TWMChannel *)channel;
		[Export ("ipMessagingClient:channelDeleted:")]
		void ChannelDeleted (TwilioIPMessagingClient client, Channel channel);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channelHistoryLoaded:(TWMChannel *)channel;
		[Export ("ipMessagingClient:channelHistoryLoaded:")]
		void ChannelHistoryLoaded (TwilioIPMessagingClient client, Channel channel);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel memberJoined:(TWMMember *)member;
		[Export ("ipMessagingClient:channel:memberJoined:")]
		void MemberJoined (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel memberChanged:(TWMMember *)member;
		[Export ("ipMessagingClient:channel:memberChanged:")]
		void MemberChanged (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel member:(TWMMember *)member userInfo:(TWMUserInfo *)userInfo updated:(TWMUserInfoUpdate)updated
		[Export ("ipMessagingClient:channel:member:userInfo:updated:")]
		void MemberUserInfoChanged (TwilioIPMessagingClient client, Channel channel, Member member, UserInfo userInfo, UserInfoUpdate updateType);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel memberLeft:(TWMMember *)member;
		[Export ("ipMessagingClient:channel:memberLeft:")]
		void MemberLeft (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel messageAdded:(TWMMessage *)message;
		[Export ("ipMessagingClient:channel:messageAdded:")]
		void MessageAdded (TwilioIPMessagingClient client, Channel channel, Message message);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel messageChanged:(TWMMessage *)message;
		[Export ("ipMessagingClient:channel:messageChanged:")]
		void MessageChanged (TwilioIPMessagingClient client, Channel channel, Message message);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client channel:(TWMChannel *)channel messageDeleted:(TWMMessage *)message;
		[Export ("ipMessagingClient:channel:messageDeleted:")]
		void MessageDeleted (TwilioIPMessagingClient client, Channel channel, Message message);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client typingStartedOnChannel:(TWMChannel *)channel member:(TWMMember *)member;
		[Export ("ipMessagingClient:typingStartedOnChannel:member:")]
		void TypingStarted (TwilioIPMessagingClient client, Channel channel, Member member);

		// - (void)ipMessagingClient:(TwilioIPMessagingClient *)client typingEndedOnChannel:(TWMChannel *)channel member:(TWMMember *)member;
		[Export ("ipMessagingClient:typingEndedOnChannel:member:")]
		void TypingEnded (TwilioIPMessagingClient client, Channel channel, Member member);
	}

	[Static]
	[Partial]
	interface ChannelOptionKey
	{
		[Field ("TWMChannelOptionFriendlyName", "__Internal")]
		NSString FriendlyName { get; }

		[Field ("TWMChannelOptionUniqueName", "__Internal")]
		NSString UniqueName { get; }

		[Field ("TWMChannelOptionType", "__Internal")]
		NSString ChannelType { get; }

		[Field ("TWMChannelOptionAttributes", "__Internal")]
		NSString Attributes { get; }
	}

	[StrongDictionary ("ChannelOptionKey")]
	interface ChannelOptions
	{
		[Export ("FriendlyName")]
		string FriendlyName { get; set; }

		[Export ("UniqueName")]
		string UniqueName { get; set; }

		[Export ("ChannelType")]
		int ChannelType { get; set; }

		[Export ("Attributes")]
		string Attributes { get; set; }
	}

	[BaseType (typeof (NSObject), Name="TWMChannels")]
	interface Channels
	{
		// - (void)loadChannelsWithCompletion:(TWMCompletion)completion;
		[Export ("loadChannelsWithCompletion:")]
		void LoadChannels (Completion completion);

		// - (NSArray<TWMChannel *> *)allObjects;
		[Export ("allObjects")]
		Channel[] AllChannels { get; }

		// - (void)createChannelWithFriendlyName:(NSString *)friendlyName type:(TWMChannelType)channelType completion:(TWMChannelCompletion)completion;
		//[Export ("createChannelWithFriendlyName:type:completion:")]
		//void CreateChannel (string friendlyName, ChannelType channelType, ChannelCompletion completion);

		// - (void)createChannelWithOptions:(NSDictionary *)options completion:(TWMChannelCompletion)completion;
		[Export ("createChannelWithOptions:completion:")]
		void CreateChannel ([NullAllowed] NSDictionary options, ChannelCompletion completion);

		[Wrap ("CreateChannel (options == null ? null : options.Dictionary, completion)")]
		void CreateChannel (ChannelOptions options, ChannelCompletion completion);		

		// - (TWMChannel *)channelWithId:(NSString *)channelId;
		[Export ("channelWithId:")]
		Channel GetChannelWithId (string channelId);

		// - (TWMChannel *)channelWithUniqueName:(NSString *)uniqueName;
		[Export ("channelWithUniqueName:")]
		Channel GetChannelWithUniqueName (string uniqueName);
	}

	[BaseType (typeof (NSError), Name="TWMError")]
	interface TwilioError
	{
	}

	[BaseType (typeof (NSObject), Name="TWMUserInfo")]
	interface UserInfo
	{
		// @property (nonatomic, copy, readonly) NSString *identity
		[Export ("identity", ArgumentSemantic.Copy)]
		string Identity { get; }

		// @property (nonatomic, copy, readonly) NSString *friendlyName
		[Export ("friendlyName", ArgumentSemantic.Copy)]
		string FriendlyName { get; }

		// - (NSDictionary<NSString *, id> *)attributes;
		[Export ("attributes")]
		NSDictionary Attributes { get; }

		// - (void)setAttributes:(NSDictionary<NSString *, id> *)attributes completion:(TWMCompletion)completion;
		[Export ("setAttributes:completion:")]
		void SetAttributes (NSDictionary attributes, Completion completion);

		// - (void)setFriendlyName:(NSString *)friendlyName completion:(TWMCompletion)completion;
		[Export ("setFriendlyName:completion:")]
		void SetFriendlyName (string friendlyName, Completion completion);
	}

	[BaseType (typeof (NSObject), Name="TWMMember")]
	interface Member 
	{
		[Export ("identity")]
		string Identity { get; }

		// @property (nonatomic, strong, readonly) TWMUserInfo *userInfo
		[Export ("userInfo", ArgumentSemantic.Strong)]
		UserInfo UserInfo { get; }

		// @property (nonatomic, copy, readonly) NSNumber *lastConsumedMessageIndex
		[Export ("lastConsumedMessageIndex", ArgumentSemantic.Copy)]
		nint LastConsumendMessageIndex { get; }

		// @property (nonatomic, copy, readonly) NSString *lastConsumptionTimestamp
		[Export ("lastConsumptionTimestamp", ArgumentSemantic.Copy)]
		string LastConsumptionTimestamp { get; }
	}

	[BaseType (typeof (NSObject), Name="TWMMembers")]
	interface Members
	{
		// - (NSArray<TWMMember *> *)allObjects;
		[Export ("allObjects")]
		Member[] AllMembers { get; }

		// - (void)addByIdentity:(NSString *)identity completion:(TWMCompletion)completion;
		[Export ("addByIdentity:completion:")]
		void Add (string identity, Completion completion);

		// - (void)inviteByIdentity:(NSString *)identity completion:(TWMCompletion)completion;
		[Export ("inviteByIdentity:completion:")]
		void Invite (string identity, Completion completion);

		// - (void)removeMember:(TWMMember *)member completion:(TWMCompletion)completion;
		[Export ("removeMember:completion:")]
		void Remove (Member member, Completion completion);
	}

	[BaseType (typeof (NSObject), Name="TWMMessage")]
	interface Message
	{
		// @property (nonatomic, copy, readonly) NSString *sid;
		[Export ("sid", ArgumentSemantic.Copy)]
		string Sid { get; }

		// @property (nonatomic, copy, readonly) NSNumber *index
		[Export ("index", ArgumentSemantic.Copy)]
		nint Index { get; }

		// @property (nonatomic, copy, readonly) NSString *author;
		[Export ("author", ArgumentSemantic.Copy)]
		string Author { get; }

		// @property (nonatomic, copy, readonly) NSString *body;
		[Export ("body", ArgumentSemantic.Copy)]
		string Body { get; }

		// @property (nonatomic, copy, readonly) NSString *timestamp;
		[Export ("timestamp", ArgumentSemantic.Copy)]
		string Timestamp { get; }

		// @property (nonatomic, copy, readonly) NSString *dateUpdated;
		[Export ("dateUpdated", ArgumentSemantic.Copy)]
		string DateUpdated { get; }

		// @property (nonatomic, copy, readonly) NSString *lastUpdatedBy;
		[Export ("lastUpdatedBy", ArgumentSemantic.Copy)]
		string LastUpdatedBy { get; }

		// - (void)updateBody:(NSString *)body completion:(TWMCompletion)completion;
		[Export ("updateBody:completion:")]
		void UpdateBody (string body, Completion completion);
	}

	[BaseType (typeof (NSObject), Name="TWMMessages")]
	interface Messages 
	{
		// @property (nonatomic, copy, readonly) NSNumber *lastConsumedMessageIndex
		[Export ("lastConsumedMessageIndex", ArgumentSemantic.Copy)]
		nint LastConsumedMessageIndex { get; }

		// - (TWMMessage *)createMessageWithBody:(NSString *)body;
		[Export ("createMessageWithBody:")]
		Message CreateMessage (string body);

		// - (void)sendMessage:(TWMMessage *)message completion:(TWMCompletion)completion;
		[Export ("sendMessage:completion:")]
		void SendMessage (Message message, Completion completion);

		// - (void)removeMessage:(TWMMessage *)message completion:(TWMCompletion)completion;
		[Export ("removeMessage:completion:")]
		void RemoveMessage (Message message, Completion completion);

		// - (NSArray<TWMMessage *> *)allObjects;
		[Export ("allObjects")]
		Message[] AllMessages { get; }

		// - (TWMMessage *)messageWithIndex:(NSNumber *)index
		[Export ("messageWithIndex:")]
		Message GetMessageWithIndex(nint index);

		// - (TWMMessage *)messageForConsumptionIndex:(NSNumber *)index
		[Export ("messageForConsumptionIndex:")]
		Message GetMessageForConsumptionIndex(nint index);

		// - (void)setLastConsumedMessageIndex:(NSNumber *)index
		[Export ("setLastConsumedMessageIndex:")]
		void SetLastConsumedMessageIndex(nint index);

		// - (void)advanceLastConsumedMessageIndex:(NSNumber *)index
		[Export ("advanceLastConsumedMessageIndex:")]
		void AdvanceLastConsumedMessageIndex(nint index);

		// - (void)setAllMessagesConsumed
		[Export ("setAllMessagesConsumed")]
		void SetAllMessagesConsumed();
	}

	[BaseType (typeof (NSObject), Name="TWMResult")]
	interface Result
	{
		[Export ("error", ArgumentSemantic.Strong)]
		TwilioError Error { get; }

		[Export ("isSuccessful")]
		bool IsSuccessful();
	}
}

