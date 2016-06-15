using System;
using CoreFoundation;
using CoreMedia;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Twilio.Conversations
{
    interface IVideoCapturer { }

    // @protocol TWCVideoCapturer <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject), Name="TWCVideoCapturer")]
    interface VideoCapturer
    {
        // @required @property (nonatomic, weak) TWCLocalVideoTrack * _Nullable videoTrack;
        [NullAllowed, Export ("videoTrack", ArgumentSemantic.Weak)]
        TWCLocalVideoTrack VideoTrack { get; set; }


    }

    // @interface TWCCameraCapturer : NSObject <TWCVideoCapturer>
    [BaseType (typeof(NSObject), Name="TWCCameraCapturer")]
    interface CameraCapturer : IVideoCapturer
    {
        // @property (assign, nonatomic) TWCVideoCaptureSource camera;
        [Export ("camera", ArgumentSemantic.Assign)]
        VideoCaptureSource Camera { get; set; }

        // @property (readonly, nonatomic, strong) UIView * _Nullable previewView;
        [NullAllowed, Export ("previewView", ArgumentSemantic.Strong)]
        UIView PreviewView { get; }

        // @property (nonatomic, weak) TWCLocalVideoTrack * _Nullable videoTrack;
        [NullAllowed, Export ("videoTrack", ArgumentSemantic.Weak)]
        TWCLocalVideoTrack VideoTrack { get; set; }

        // -(instancetype _Nonnull)initWithSource:(TWCVideoCaptureSource)source;
        [Export ("initWithSource:")]
        IntPtr Constructor (VideoCaptureSource source);

        // -(BOOL)startPreview;
        [Export ("startPreview")]
        bool StartPreview ();

        // -(BOOL)stopPreview;
        [Export ("stopPreview")]
        bool StopPreview ();

        // -(void)flipCamera;
        [Export ("flipCamera")]
        void FlipCamera ();
    }

    // @interface TWCMedia : NSObject
    [BaseType (typeof(NSObject), Name="TWCMedia")]
    interface Media
    {
        // @property (readonly, nonatomic, strong) NSArray<TWCAudioTrack *> * _Nonnull audioTracks;
        [Export ("audioTracks", ArgumentSemantic.Strong)]
        AudioTrack[] AudioTracks { get; }

        // @property (readonly, nonatomic, strong) NSArray<TWCVideoTrack *> * _Nonnull videoTracks;
        [Export ("videoTracks", ArgumentSemantic.Strong)]
        VideoTrack[] VideoTracks { get; }

        // -(TWCMediaTrack * _Nullable)getTrack:(NSString * _Nonnull)trackId;
        [Export ("getTrack:")]
        [return: NullAllowed]
        MediaTrack GetTrack (string trackId);
    }


    interface ILocalMediaDelegate { }

    // @protocol TWCLocalMediaDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject), Name="TWCLocalMediaDelegate")]
    interface LocalMediaDelegate
    {
        // @optional -(void)localMedia:(TWCLocalMedia * _Nonnull)media didAddVideoTrack:(TWCVideoTrack * _Nonnull)videoTrack;
        [Export ("localMedia:didAddVideoTrack:")]
        void DidAddVideoTrack (LocalMedia media, VideoTrack videoTrack);

        // @optional -(void)localMedia:(TWCLocalMedia * _Nonnull)media didRemoveVideoTrack:(TWCVideoTrack * _Nonnull)videoTrack;
        [Export ("localMedia:didRemoveVideoTrack:")]
        void DidRemoveVideoTrack (LocalMedia media, VideoTrack videoTrack);
    }

    // @interface TWCLocalMedia : TWCMedia
    [BaseType (typeof(Media), Name="TWCLocalMedia")]
    interface LocalMedia
    {
        // @property (getter = isMicrophoneMuted, assign, nonatomic) BOOL microphoneMuted;
        [Export ("microphoneMuted")]
        bool MicrophoneMuted { [Bind ("isMicrophoneMuted")] get; set; }

        // @property (readonly, getter = isMicrophoneAdded, assign, nonatomic) BOOL microphoneAdded;
        [Export ("microphoneAdded")]
        bool MicrophoneAdded { [Bind ("isMicrophoneAdded")] get; }

        // @property (nonatomic, weak) id<TWCLocalMediaDelegate> _Nullable delegate;
        [NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
        ILocalMediaDelegate Delegate { get; set; }

        // -(instancetype _Nullable)initWithDelegate:(id<TWCLocalMediaDelegate> _Nullable)delegate;
        [Export ("initWithDelegate:")]
        IntPtr Constructor ([NullAllowed] ILocalMediaDelegate @delegate);

        // -(BOOL)addTrack:(TWCVideoTrack * _Nonnull)track;
        [Export ("addTrack:")]
        bool AddTrack (VideoTrack track);

        // -(BOOL)addTrack:(TWCVideoTrack * _Nonnull)track error:(NSError * _Nullable * _Nullable)error;
        [Export ("addTrack:error:")]
        bool AddTrack (VideoTrack track, [NullAllowed] out NSError error);

        // -(BOOL)removeTrack:(TWCVideoTrack * _Nonnull)track;
        [Export ("removeTrack:")]
        bool RemoveTrack (VideoTrack track);

        // -(BOOL)removeTrack:(TWCVideoTrack * _Nonnull)track error:(NSError * _Nullable * _Nullable)error;
        [Export ("removeTrack:error:")]
        bool RemoveTrack (VideoTrack track, [NullAllowed] out NSError error);

        // -(TWCCameraCapturer * _Nullable)addCameraTrack;
        [NullAllowed, Export ("addCameraTrack")]
        CameraCapturer AddCameraTrack ();

        // -(TWCCameraCapturer * _Nullable)addCameraTrack:(NSError * _Nullable * _Nullable)error;
        [Export ("addCameraTrack:")]
        [return: NullAllowed]
        CameraCapturer AddCameraTrack ([NullAllowed] out NSError error);

        // -(BOOL)addMicrophone;
        [Export ("addMicrophone")]
        bool AddMicrophone ();

        // -(BOOL)removeMicrophone;
        [Export ("removeMicrophone")]
        bool RemoveMicrophone ();
    }

    // typedef void (^TWCInviteAcceptanceBlock)(TWCConversation * _Nullable, NSError * _Nullable);
    delegate void InviteAcceptanceBlock ([NullAllowed] Conversation conversation, [NullAllowed] NSError error);

    // @interface TWCIncomingInvite : NSObject
    [BaseType (typeof(NSObject), Name="TWCIncomingInvite")]
    interface IncomingInvite
    {
        // @property (readonly, copy, nonatomic) NSString * _Nullable conversationSid;
        [NullAllowed, Export ("conversationSid")]
        string ConversationSid { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nonnull from;
        [Export ("from")]
        string From { get; }

        // @property (readonly, copy, nonatomic) NSArray * _Nonnull participants;
        [Export ("participants", ArgumentSemantic.Copy)]
        Participant[] Participants { get; }

        // @property (readonly, assign, nonatomic) TWCInviteStatus status;
        [Export ("status", ArgumentSemantic.Assign)]
        InviteStatus Status { get; }

        // -(void)acceptWithCompletion:(TWCInviteAcceptanceBlock _Nonnull)acceptHandler;
        [Export ("acceptWithCompletion:")]
        void AcceptWithCompletion (InviteAcceptanceBlock acceptHandler);

        // -(void)acceptWithLocalMedia:(TWCLocalMedia * _Nonnull)localMedia completion:(TWCInviteAcceptanceBlock _Nonnull)acceptHandler;
        [Export ("acceptWithLocalMedia:completion:")]
        void AcceptWithLocalMedia (LocalMedia localMedia, InviteAcceptanceBlock acceptHandler);

        // -(void)reject;
        [Export ("reject")]
        void Reject ();
    }

    // @interface TWCOutgoingInvite : NSObject
    [BaseType (typeof(NSObject), Name="TWCOutgoingInvite")]
    interface OutgoingInvite
    {
        // @property (readonly, assign, nonatomic) TWCInviteStatus status;
        [Export ("status", ArgumentSemantic.Assign)]
        InviteStatus Status { get; }

        // @property (readonly, copy, nonatomic) NSArray<NSString *> * _Nonnull to;
        [Export ("to", ArgumentSemantic.Copy)]
        string[] To { get; }

        // -(void)cancel;
        [Export ("cancel")]
        void Cancel ();
    }

    interface IParticipantDelegate { }

    // @protocol TWCParticipantDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject), Name="TWCParticipantDelegate")]
    interface ParticipantDelegate
    {
        // @optional -(void)participant:(TWCParticipant * _Nonnull)participant addedVideoTrack:(TWCVideoTrack * _Nonnull)videoTrack;
        [Export ("participant:addedVideoTrack:")]
        void AddedVideoTrack (Participant participant, VideoTrack videoTrack);

        // @optional -(void)participant:(TWCParticipant * _Nonnull)participant removedVideoTrack:(TWCVideoTrack * _Nonnull)videoTrack;
        [Export ("participant:removedVideoTrack:")]
        void RemovedVideoTrack (Participant participant, VideoTrack videoTrack);

        // @optional -(void)participant:(TWCParticipant * _Nonnull)participant addedAudioTrack:(TWCAudioTrack * _Nonnull)audioTrack;
        [Export ("participant:addedAudioTrack:")]
        void AddedAudioTrack (Participant participant, AudioTrack audioTrack);

        // @optional -(void)participant:(TWCParticipant * _Nonnull)participant removedAudioTrack:(TWCAudioTrack * _Nonnull)audioTrack;
        [Export ("participant:removedAudioTrack:")]
        void RemovedAudioTrack (Participant participant, AudioTrack audioTrack);

        // @optional -(void)participant:(TWCParticipant * _Nonnull)participant disabledTrack:(TWCMediaTrack * _Nonnull)track;
        [Export ("participant:disabledTrack:")]
        void DisabledTrack (Participant participant, MediaTrack track);

        // @optional -(void)participant:(TWCParticipant * _Nonnull)participant enabledTrack:(TWCMediaTrack * _Nonnull)track;
        [Export ("participant:enabledTrack:")]
        void EnabledTrack (Participant participant, MediaTrack track);
    }

    // @interface TWCParticipant : NSObject
    [BaseType (typeof(NSObject), Name="TWCParticipant")]
    interface Participant
    {
        // @property (readonly, nonatomic, strong) NSString * _Nonnull identity;
        [Export ("identity", ArgumentSemantic.Strong)]
        string Identity { get; }

        // @property (readonly, nonatomic, weak) TWCConversation * _Null_unspecified conversation;
        [Export ("conversation", ArgumentSemantic.Weak)]
        Conversation Conversation { get; }

        // @property (nonatomic, weak) id<TWCParticipantDelegate> _Nullable delegate;
        [NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
        IParticipantDelegate Delegate { get; set; }

        // @property (readonly, nonatomic, strong) TWCMedia * _Nonnull media;
        [Export ("media", ArgumentSemantic.Strong)]
        Media Media { get; }

        // @property (readonly, nonatomic, strong) NSString * _Nullable sid;
        [NullAllowed, Export ("sid", ArgumentSemantic.Strong)]
        string Sid { get; }
    }

    // @interface TWCConversation : NSObject
    [BaseType (typeof(NSObject), Name="TWCConversation")]
    interface Conversation
    {
        // @property (readonly, nonatomic, strong) NSArray<TWCParticipant *> * _Nonnull participants;
        [Export ("participants", ArgumentSemantic.Strong)]
        Participant[] Participants { get; }

        // @property (readonly, nonatomic, strong) TWCLocalMedia * _Nullable localMedia;
        [NullAllowed, Export ("localMedia", ArgumentSemantic.Strong)]
        LocalMedia LocalMedia { get; }

        // @property (nonatomic, weak) id<TWCConversationDelegate> _Nullable delegate;
        [NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
        IConversationDelegate Delegate { get; set; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable sid;
        [NullAllowed, Export ("sid")]
        string Sid { get; }

        // -(BOOL)disconnect;
        [Export ("disconnect")]
        bool Disconnect ();

        // -(BOOL)disconnect:(NSError * _Nullable * _Nullable)error;
        [Export ("disconnect:")]
        bool Disconnect ([NullAllowed] out NSError error);

        // -(BOOL)invite:(NSString * _Nonnull)clientIdentity error:(NSError * _Nullable * _Nullable)error;
        [Export ("invite:error:")]
        bool Invite (string clientIdentity, [NullAllowed] out NSError error);

        // -(BOOL)inviteMany:(NSArray * _Nonnull)clientIdentities error:(NSError * _Nullable * _Nullable)error;
        [Export ("inviteMany:error:")]
        bool InviteMany (string[] clientIdentities, [NullAllowed] out NSError error);

        // -(TWCParticipant * _Nullable)getParticipant:(NSString * _Nonnull)participantSID;
        [Export ("getParticipant:")]
        [return: NullAllowed]
        Participant GetParticipant (string participantSID);
    }

    interface IConversationDelegate { }

    // @protocol TWCConversationDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject), Name="TWCConversationDelegate")]
    interface ConversationDelegate
    {
        // @optional -(void)conversation:(TWCConversation * _Nonnull)conversation didConnectParticipant:(TWCParticipant * _Nonnull)participant;
        [Export ("conversation:didConnectParticipant:")]
        void ConnectedToParticipant (Conversation conversation, Participant participant);

        // @optional -(void)conversation:(TWCConversation * _Nonnull)conversation didFailToConnectParticipant:(TWCParticipant * _Nonnull)participant error:(NSError * _Nonnull)error;
        [Export ("conversation:didFailToConnectParticipant:error:")]
        void FailedToConnectToParticipant (Conversation conversation, Participant participant, NSError error);

        // @optional -(void)conversation:(TWCConversation * _Nonnull)conversation didDisconnectParticipant:(TWCParticipant * _Nonnull)participant;
        [Export ("conversation:didDisconnectParticipant:")]
        void DisconnectedFromParticipant (Conversation conversation, Participant participant);

        // @optional -(void)conversationEnded:(TWCConversation * _Nonnull)conversation;
        [Export ("conversationEnded:")]
        void ConversationEnded (Conversation conversation);

        // @optional -(void)conversationEnded:(TWCConversation * _Nonnull)conversation error:(NSError * _Nonnull)error;
        [Export ("conversationEnded:error:")]
        void ConversationEnded (Conversation conversation, NSError error);

        // @optional -(void)conversation:(TWCConversation * _Nonnull)conversation didReceiveTrackStatistics:(TWCMediaTrackStatsRecord * _Nonnull)trackStatistics;
        [Export ("conversation:didReceiveTrackStatistics:")]
        void DidReceiveTrackStatistics (Conversation conversation, MediaTrackStatsRecord trackStatistics);
    }

    // @interface TWCI420Frame : NSObject
    [BaseType (typeof(NSObject), Name="TWCI420Frame")]
    interface I420Frame
    {
        // @property (readonly, nonatomic) NSUInteger width;
        [Export ("width")]
        nuint Width { get; }

        // @property (readonly, nonatomic) NSUInteger height;
        [Export ("height")]
        nuint Height { get; }

        // @property (readonly, nonatomic) NSUInteger chromaWidth;
        [Export ("chromaWidth")]
        nuint ChromaWidth { get; }

        // @property (readonly, nonatomic) NSUInteger chromaHeight;
        [Export ("chromaHeight")]
        nuint ChromaHeight { get; }

        // @property (readonly, nonatomic) NSUInteger chromaSize;
        [Export ("chromaSize")]
        nuint ChromaSize { get; }

        // @property (readonly, nonatomic) NSInteger rotation;
        [Export ("rotation")]
        nint Rotation { get; }

//        // @property (readonly, nonatomic) const uint8_t * yPlane;
//        [Export ("yPlane")]
//        unsafe byte* YPlane { get; }
//
//        // @property (readonly, nonatomic) const uint8_t * uPlane;
//        [Export ("uPlane")]
//        unsafe byte* UPlane { get; }
//
//        // @property (readonly, nonatomic) const uint8_t * vPlane;
//        [Export ("vPlane")]
//        unsafe byte* VPlane { get; }

        // @property (readonly, nonatomic) NSInteger yPitch;
        [Export ("yPitch")]
        nint YPitch { get; }

        // @property (readonly, nonatomic) NSInteger uPitch;
        [Export ("uPitch")]
        nint UPitch { get; }

        // @property (readonly, nonatomic) NSInteger vPitch;
        [Export ("vPitch")]
        nint VPitch { get; }
    }

    [BaseType (typeof (MediaTrack), Name="TWCAudioTrack")]
    interface AudioTrack { }

    // @interface TWCMediaTrack : NSObject
    [BaseType (typeof(NSObject), Name="TWCMediaTrack")]
    interface MediaTrack
    {
        // @property (readonly, getter = isEnabled, assign, nonatomic) BOOL enabled;
        [Export ("enabled")]
        bool Enabled { [Bind ("isEnabled")] get; }

        // @property (readonly, assign, nonatomic) TWCMediaTrackState state;
        [Export ("state", ArgumentSemantic.Assign)]
        MediaTrackState State { get; }

        // @property (readonly, copy, nonatomic) NSString * trackId;
        [Export ("trackId")]
        string TrackId { get; }
    }

    interface IVideoRenderer { }

    // @protocol TWCVideoRenderer <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject), Name="TWCVideoRenderer")]
    interface VideoRenderer
    {
        // @required -(void)updateVideoSize:(CMVideoDimensions)videoSize;
        [Abstract]
        [Export ("updateVideoSize:")]
        void UpdateVideoSize (CMVideoDimensions videoSize);

        // @required -(void)renderFrame:(TWCI420Frame * _Nonnull)frame;
        [Abstract]
        [Export ("renderFrame:")]
        void RenderFrame (I420Frame frame);
    }


    interface IVideoTrackDelegate { }

    // @protocol TWCVideoTrackDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject), Name="TWCVideoTrackDelegate")]
    interface VideoTrackDelegate
    {
        // @optional -(void)videoTrack:(TWCVideoTrack * _Nonnull)track dimensionsDidChange:(CMVideoDimensions)dimensions;
        [Export ("videoTrack:dimensionsDidChange:")]
        void DimensionsDidChange (VideoTrack track, CMVideoDimensions dimensions);
    }

    // @interface TWCVideoTrack : TWCMediaTrack
    [BaseType (typeof(MediaTrack), Name="TWCVideoTrack")]
    interface VideoTrack
    {
        // @property (nonatomic, weak) id<TWCVideoTrackDelegate> _Nullable delegate;
        [NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
        IVideoTrackDelegate Delegate { get; set; }

        // @property (readonly, nonatomic, strong) NSArray<UIView *> * _Nonnull attachedViews;
        [Export ("attachedViews", ArgumentSemantic.Strong)]
        UIView[] AttachedViews { get; }

        // @property (readonly, nonatomic, strong) NSArray<id<TWCVideoRenderer>> * _Nonnull renderers;
        [Export ("renderers", ArgumentSemantic.Strong)]
        VideoRenderer[] Renderers { get; }

        // @property (readonly, assign, nonatomic) CMVideoDimensions videoDimensions;
        [Export ("videoDimensions", ArgumentSemantic.Assign)]
        CMVideoDimensions VideoDimensions { get; }

        // -(void)attach:(UIView * _Nonnull)view;
        [Export ("attach:")]
        void Attach (UIView view);

        // -(void)detach:(UIView * _Nonnull)view;
        [Export ("detach:")]
        void Detach (UIView view);

        // -(void)addRenderer:(id<TWCVideoRenderer> _Nonnull)renderer;
        [Export ("addRenderer:")]
        void AddRenderer (VideoRenderer renderer);

        // -(void)removeRenderer:(id<TWCVideoRenderer> _Nonnull)renderer;
        [Export ("removeRenderer:")]
        void RemoveRenderer (VideoRenderer renderer);
    }

    // @interface TWCLocalVideoTrack : TWCVideoTrack
    [BaseType (typeof(VideoTrack), Name="TWCLocalVideoTrack")]
    interface TWCLocalVideoTrack
    {
        // -(instancetype _Nonnull)initWithCapturer:(id<TWCVideoCapturer> _Nonnull)capturer;
        [Export ("initWithCapturer:")]
        IntPtr Constructor (VideoCapturer capturer);

        // @property (getter = isEnabled, assign, nonatomic) BOOL enabled;
        [Export ("enabled")]
        bool Enabled { [Bind ("isEnabled")] get; set; }

        // @property (readonly, nonatomic, strong) id<TWCVideoCapturer> _Nonnull capturer;
        [Export ("capturer", ArgumentSemantic.Strong)]
        VideoCapturer Capturer { get; }
    }

    interface IVideoViewRendererDelegate { }

    // @protocol TWCVideoViewRendererDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject), Name="TWCVideoViewRendererDelegate")]
    interface VideoViewRendererDelegate
    {
        // @optional -(void)rendererDidReceiveVideoData:(TWCVideoViewRenderer * _Nonnull)renderer;
        [Export ("rendererDidReceiveVideoData:")]
        void RendererDidReceiveVideoData (VideoViewRenderer renderer);

        // @optional -(void)renderer:(TWCVideoViewRenderer * _Nonnull)renderer dimensionsDidChange:(CMVideoDimensions)dimensions;
        [Export ("renderer:dimensionsDidChange:")]
        void Renderer (VideoViewRenderer renderer, CMVideoDimensions dimensions);
    }

    // @interface TWCVideoViewRenderer : NSObject <TWCVideoRenderer>
    [BaseType (typeof(NSObject), Name="TWCVideoViewRenderer")]
    interface VideoViewRenderer : IVideoRenderer
    {
        // -(instancetype _Nonnull)initWithDelegate:(id<TWCVideoViewRendererDelegate> _Nullable)delegate;
        [Export ("initWithDelegate:")]
        IntPtr Constructor ([NullAllowed] IVideoViewRendererDelegate @delegate);

        // +(TWCVideoViewRenderer * _Nonnull)rendererWithDelegate:(id<TWCVideoViewRendererDelegate> _Nullable)delegate;
        [Static]
        [Export ("rendererWithDelegate:")]
        VideoViewRenderer RendererWithDelegate ([NullAllowed] IVideoViewRendererDelegate @delegate);

        // @property (readonly, nonatomic, weak) id<TWCVideoViewRendererDelegate> _Nullable delegate;
        [NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
        IVideoViewRendererDelegate Delegate { get; }

        // @property (readonly, assign, nonatomic) CMVideoDimensions videoFrameDimensions;
        [Export ("videoFrameDimensions", ArgumentSemantic.Assign)]
        CMVideoDimensions VideoFrameDimensions { get; }

        // @property (readonly, assign, atomic) BOOL hasVideoData;
        [Export ("hasVideoData")]
        bool HasVideoData { get; }

        // @property (readonly, nonatomic, strong) UIView * _Nonnull view;
        [Export ("view", ArgumentSemantic.Strong)]
        UIView View { get; }
    }

    // @interface TWCMediaTrackStatsRecord : NSObject
    [BaseType (typeof(NSObject), Name="TWCMediaTrackStatsRecord")]
    interface MediaTrackStatsRecord
    {
        // @property (readonly, nonatomic) NSString * trackId;
        [Export ("trackId")]
        string TrackId { get; }

        // @property (readonly, nonatomic) NSUInteger packetsLost;
        [Export ("packetsLost")]
        nuint PacketsLost { get; }

        // @property (readonly, nonatomic) NSString * direction;
        [Export ("direction")]
        string Direction { get; }

        // @property (readonly, nonatomic) NSString * codecName;
        [Export ("codecName")]
        string CodecName { get; }

        // @property (readonly, nonatomic) NSString * ssrc;
        [Export ("ssrc")]
        string Ssrc { get; }

        // @property (readonly, nonatomic) NSString * participantSID;
        [Export ("participantSID")]
        string ParticipantSID { get; }

        // @property (readonly, assign, nonatomic) CFTimeInterval timestamp;
        [Export ("timestamp")]
        double Timestamp { get; }
    }

    // @interface TWCLocalVideoMediaStatsRecord : TWCMediaTrackStatsRecord
    [BaseType (typeof(MediaTrackStatsRecord), Name="TWCLocalVideoMediaStatsRecord")]
    interface LocalVideoMediaStatsRecord
    {
        // @property (readonly, nonatomic) NSUInteger bytesSent;
        [Export ("bytesSent")]
        nuint BytesSent { get; }

        // @property (readonly, nonatomic) NSUInteger packetsSent;
        [Export ("packetsSent")]
        nuint PacketsSent { get; }

        // @property (readonly, assign, nonatomic) CMVideoDimensions captureDimensions;
        [Export ("captureDimensions", ArgumentSemantic.Assign)]
        CMVideoDimensions CaptureDimensions { get; }

        // @property (readonly, assign, nonatomic) CMVideoDimensions sentDimensions;
        [Export ("sentDimensions", ArgumentSemantic.Assign)]
        CMVideoDimensions SentDimensions { get; }

        // @property (readonly, nonatomic) NSUInteger frameRate;
        [Export ("frameRate")]
        nuint FrameRate { get; }

        // @property (readonly, nonatomic) NSUInteger roundTripTime;
        [Export ("roundTripTime")]
        nuint RoundTripTime { get; }
    }

    // @interface TWCLocalAudioMediaStatsRecord : TWCMediaTrackStatsRecord
    [BaseType (typeof(MediaTrackStatsRecord), Name="TWCLocalAudioMediaStatsRecord")]
    interface LocalAudioMediaStatsRecord
    {
        // @property (readonly, nonatomic) NSUInteger bytesSent;
        [Export ("bytesSent")]
        nuint BytesSent { get; }

        // @property (readonly, nonatomic) NSUInteger packetsSent;
        [Export ("packetsSent")]
        nuint PacketsSent { get; }

        // @property (readonly, nonatomic) NSUInteger audioInputLevel;
        [Export ("audioInputLevel")]
        nuint AudioInputLevel { get; }

        // @property (readonly, nonatomic) NSUInteger jitterReceived;
        [Export ("jitterReceived")]
        nuint JitterReceived { get; }

        // @property (readonly, nonatomic) NSUInteger jitter;
        [Export ("jitter")]
        nuint Jitter { get; }

        // @property (readonly, nonatomic) NSUInteger roundTripTime;
        [Export ("roundTripTime")]
        nuint RoundTripTime { get; }
    }

    // @interface TWCRemoteVideoMediaStatsRecord : TWCMediaTrackStatsRecord
    [BaseType (typeof(MediaTrackStatsRecord), Name="TWCRemoteVideoMediaStatsRecord")]
    interface RemoteVideoMediaStatsRecord
    {
        // @property (readonly, nonatomic) NSUInteger bytesReceived;
        [Export ("bytesReceived")]
        nuint BytesReceived { get; }

        // @property (readonly, nonatomic) NSUInteger packetsReceived;
        [Export ("packetsReceived")]
        nuint PacketsReceived { get; }

        // @property (readonly, assign, nonatomic) CMVideoDimensions dimensions;
        [Export ("dimensions", ArgumentSemantic.Assign)]
        CMVideoDimensions Dimensions { get; }

        // @property (readonly, nonatomic) NSUInteger frameRate;
        [Export ("frameRate")]
        nuint FrameRate { get; }

        // @property (readonly, nonatomic) NSUInteger jitterBuffer;
        [Export ("jitterBuffer")]
        nuint JitterBuffer { get; }
    }

    // @interface TWCRemoteAudioMediaStatsRecord : TWCMediaTrackStatsRecord
    [BaseType (typeof(MediaTrackStatsRecord), Name="TWCRemoteAudioMediaStatsRecord")]
    interface RemoteAudioMediaStatsRecord
    {
        // @property (readonly, nonatomic) NSUInteger bytesReceived;
        [Export ("bytesReceived")]
        nuint BytesReceived { get; }

        // @property (readonly, nonatomic) NSUInteger packetsReceived;
        [Export ("packetsReceived")]
        nuint PacketsReceived { get; }

        // @property (readonly, nonatomic) NSUInteger audioOutputLevel;
        [Export ("audioOutputLevel")]
        nuint AudioOutputLevel { get; }

        // @property (readonly, nonatomic) NSUInteger jitterBuffer;
        [Export ("jitterBuffer")]
        nuint JitterBuffer { get; }

        // @property (readonly, nonatomic) NSUInteger jitterReceived;
        [Export ("jitterReceived")]
        nuint JitterReceived { get; }
    }

    // @interface TwilioConversationsClient : NSObject
    [BaseType (typeof(NSObject))]
    interface TwilioConversationsClient
    {
        // extern NSString *const _Nonnull TwilioConversationsClientOptionUserNameKey;
        [Field ("TwilioConversationsClientOptionUserNameKey", "__Internal")]
        NSString OptionUserNameKey { get; }

        // extern NSString *const _Nonnull TwilioConversationsClientOptionPasswordKey;
        [Field ("TwilioConversationsClientOptionPasswordKey", "__Internal")]
        NSString OptionPasswordKey { get; }

        // extern NSString *const _Nonnull TwilioConversationsClientOptionStunURLKey;
        [Field ("TwilioConversationsClientOptionStunURLKey", "__Internal")]
        NSString OptionStunURLKey { get; }

        // extern NSString *const _Nonnull TwilioConversationsClientOptionTurnURLKey;
        [Field ("TwilioConversationsClientOptionTurnURLKey", "__Internal")]
        NSString OptionTurnURLKey { get; }

        // extern NSString *const _Nonnull TwilioConversationsClientOptionEnableH264Key;
        [Field ("TwilioConversationsClientOptionEnableH264Key", "__Internal")]
        NSString OptionEnableH264Key { get; }

        // @property (nonatomic, weak) id<TwilioConversationsClientDelegate> _Nullable delegate;
        [NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
        ITwilioConversationsClientDelegate Delegate { get; set; }

        // @property (readonly, nonatomic, strong) TwilioAccessManager * _Nonnull accessManager;
        [Export ("accessManager", ArgumentSemantic.Strong)]
        Twilio.Common.TwilioAccessManager AccessManager { get; }

        // @property (readonly, nonatomic, strong) NSString * _Nullable identity;
        [NullAllowed, Export ("identity", ArgumentSemantic.Strong)]
        string Identity { get; }

        // @property (readonly, assign, nonatomic) BOOL listening;
        [Export ("listening")]
        bool Listening { get; }

        // +(TwilioConversationsClient * _Nullable)conversationsClientWithToken:(NSString * _Nonnull)token delegate:(id<TwilioConversationsClientDelegate> _Nullable)delegate;
        [Static]
        [Export ("conversationsClientWithToken:delegate:")]
        [return: NullAllowed]
        TwilioConversationsClient From (string token, [NullAllowed] ITwilioConversationsClientDelegate @delegate);

        // +(TwilioConversationsClient * _Nullable)conversationsClientWithAccessManager:(TwilioAccessManager * _Nonnull)accessManager delegate:(id<TwilioConversationsClientDelegate> _Nullable)delegate;
        [Static]
        [Export ("conversationsClientWithAccessManager:delegate:")]
        [return: NullAllowed]
        TwilioConversationsClient From (Twilio.Common.TwilioAccessManager accessManager, [NullAllowed] ITwilioConversationsClientDelegate @delegate);

        // +(TwilioConversationsClient * _Nullable)conversationsClientWithAccessManager:(TwilioAccessManager * _Nonnull)accessManager options:(NSDictionary * _Nullable)options delegateQueue:(dispatch_queue_t _Nullable)queue delegate:(id<TwilioConversationsClientDelegate> _Nullable)delegate;
        [Static]
        [Export ("conversationsClientWithAccessManager:options:delegateQueue:delegate:")]
        [return: NullAllowed]
        TwilioConversationsClient From (Twilio.Common.TwilioAccessManager accessManager, [NullAllowed] NSDictionary options, [NullAllowed] DispatchQueue queue, [NullAllowed] ITwilioConversationsClientDelegate @delegate);

        // -(void)listen;
        [Export ("listen")]
        void Listen ();

        // -(void)unlisten;
        [Export ("unlisten")]
        void Unlisten ();

        // -(TWCOutgoingInvite * _Nullable)inviteToConversation:(NSString * _Nonnull)client handler:(TWCInviteAcceptanceBlock _Nonnull)handler;
        [Export ("inviteToConversation:handler:")]
        [return: NullAllowed]
        OutgoingInvite InviteToConversation (string client, InviteAcceptanceBlock handler);

        // -(TWCOutgoingInvite * _Nullable)inviteToConversation:(NSString * _Nonnull)client localMedia:(TWCLocalMedia * _Nonnull)localMedia handler:(TWCInviteAcceptanceBlock _Nonnull)handler;
        [Export ("inviteToConversation:localMedia:handler:")]
        [return: NullAllowed]
        OutgoingInvite InviteToConversation (string client, LocalMedia localMedia, InviteAcceptanceBlock handler);

        // -(TWCOutgoingInvite * _Nullable)inviteManyToConversation:(NSArray<NSString *> * _Nonnull)clients localMedia:(TWCLocalMedia * _Nonnull)localMedia handler:(TWCInviteAcceptanceBlock _Nonnull)handler;
        [Export ("inviteManyToConversation:localMedia:handler:")]
        [return: NullAllowed]
        OutgoingInvite InviteManyToConversation (string[] clients, LocalMedia localMedia, InviteAcceptanceBlock handler);

        // +(NSString * _Nonnull)version;
        [Static]
        [Export ("version")]
        string Version { get; }

        // +(TWCLogLevel)logLevel;
        // +(void)setLogLevel:(TWCLogLevel)logLevel;
        [Static]
        [Export ("logLevel")]
        LogLevel LogLevel { get; set; }

        // +(void)setAudioOutput:(TWCAudioOutput)audioOutput;
        [Static]
        [Export ("setAudioOutput:")]
        void SetAudioOutput (AudioOutput audioOutput);

        // +(TWCAudioOutput)audioOutput;
        [Static]
        [Export ("audioOutput")]
        AudioOutput AudioOutput ();
    }

    interface ITwilioConversationsClientDelegate { }

    // @protocol TwilioConversationsClientDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject))]
    interface TwilioConversationsClientDelegate
    {
        // @optional -(void)conversationsClientDidStartListeningForInvites:(TwilioConversationsClient * _Nonnull)conversationsClient;
        [Export ("conversationsClientDidStartListeningForInvites:")]
        void DidStartListeningForInvites (TwilioConversationsClient conversationsClient);

        // @optional -(void)conversationsClient:(TwilioConversationsClient * _Nonnull)conversationsClient didFailToStartListeningWithError:(NSError * _Nonnull)error;
        [Export ("conversationsClient:didFailToStartListeningWithError:")]
        void DidFailToStartListening (TwilioConversationsClient conversationsClient, NSError error);

        // @optional -(void)conversationsClientDidStopListeningForInvites:(TwilioConversationsClient * _Nonnull)conversationsClient error:(NSError * _Nullable)error;
        [Export ("conversationsClientDidStopListeningForInvites:error:")]
        void DidStopListeningForInvites (TwilioConversationsClient conversationsClient, [NullAllowed] NSError error);

        // @optional -(void)conversationsClient:(TwilioConversationsClient * _Nonnull)conversationsClient didReceiveInvite:(TWCIncomingInvite * _Nonnull)invite;
        [Export ("conversationsClient:didReceiveInvite:")]
        void DidReceiveInvite (TwilioConversationsClient conversationsClient, IncomingInvite invite);

        // @optional -(void)conversationsClient:(TwilioConversationsClient * _Nonnull)conversationsClient inviteDidCancel:(TWCIncomingInvite * _Nonnull)invite;
        [Export ("conversationsClient:inviteDidCancel:")]
        void InviteDidCancel (TwilioConversationsClient conversationsClient, IncomingInvite invite);
    }
}