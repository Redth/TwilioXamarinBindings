using System;
using Twilio.Common;

namespace Twilio.Conversations
{
    public class CapturerErrorListener : Java.Lang.Object, ICapturerErrorListener
    {
        public Action<CapturerException> ErrorHandler { get;set; }
        public void OnError (CapturerException error)
        {
            ErrorHandler?.Invoke (error);
        }
    }

    public class ConversationCallback : Java.Lang.Object, IConversationCallback
    {
        public Action<IConversation, ConversationException> ConversationHandler { get; set; }
        public void OnConversation (IConversation conversation, ConversationException ex)
        {
            ConversationHandler?.Invoke (conversation, ex);
        }
    }

    public partial class TwilioConversations
    {

        public class InitListener : Java.Lang.Object, IInitListener
        {
            public Action<Java.Lang.Exception> ErrorHandler { get;set; }
            public Action InitHandler { get; set; }

            public void OnError (Java.Lang.Exception err)
            {
                ErrorHandler?.Invoke (err);
            }

            public void OnInitialized ()
            {
                InitHandler?.Invoke ();
            }
        }
    }

    public class LocalMediaListener : Java.Lang.Object, ILocalMediaListener
    {
        public Action<IConversation, ILocalVideoTrack> LocalVideoTrackAddedHandler { get;set; }
        public void OnLocalVideoTrackAdded (IConversation conversation, ILocalVideoTrack videoTrack)
        {
            LocalVideoTrackAddedHandler?.Invoke (conversation, videoTrack);
        }

        public Action<IConversation, ILocalVideoTrack> LocalVideoTrackRemovedHandler { get;set; }
        public void OnLocalVideoTrackRemoved (IConversation conversation, ILocalVideoTrack videoTrack)
        {
            LocalVideoTrackRemovedHandler?.Invoke (conversation, videoTrack);
        }
    }

    public class ParticipantListener : Java.Lang.Object, IParticipantListener
    {
        public Action<IConversation, IParticipant, IAudioTrack> AudioTrackAddedHandler { get; set; }
        public void OnAudioTrackAdded (IConversation conversation, IParticipant participant, IAudioTrack audioTrack)
        {
            AudioTrackAddedHandler?.Invoke (conversation, participant, audioTrack);
        }

        public Action<IConversation, IParticipant, IAudioTrack> AudioTrackRemovedHandler { get; set; }
        public void OnAudioTrackRemoved (IConversation conversation, IParticipant participant, IAudioTrack audioTrack)
        {
            AudioTrackRemovedHandler?.Invoke (conversation, participant, audioTrack);
        }

        public Action<IConversation, IParticipant, IMediaTrack> TrackDisabledHandler { get; set; }
        public void OnTrackDisabled (IConversation conversation, IParticipant participant, IMediaTrack mediaTrack)
        {
            TrackDisabledHandler?.Invoke (conversation, participant, mediaTrack);
        }

        public Action<IConversation, IParticipant, IMediaTrack> TrackEnabledHandler { get; set; }
        public void OnTrackEnabled (IConversation conversation, IParticipant participant, IMediaTrack mediaTrack)
        {
            TrackEnabledHandler?.Invoke (conversation, participant, mediaTrack);
        }

        public Action<IConversation, IParticipant, IVideoTrack> VideoTrackAddedHandler { get; set; }
        public void OnVideoTrackAdded (IConversation conversation, IParticipant participant, IVideoTrack videoTrack)
        {
            VideoTrackAddedHandler?.Invoke (conversation, participant, videoTrack);
        }

        public Action<IConversation, IParticipant, IVideoTrack> VideoTrackRemovedHandler { get; set; }
        public void OnVideoTrackRemoved (IConversation conversation, IParticipant participant, IVideoTrack videoTrack)
        {
            VideoTrackRemovedHandler?.Invoke (conversation, participant, videoTrack);
        }
    }

    public class VideoRendererObserver : Java.Lang.Object, IVideoRendererObserver
    {
        public Action FirstFrameHandler { get; set; }
        public void OnFirstFrame ()
        {
            FirstFrameHandler?.Invoke ();
        }

        public Action<int, int> FrameDimensionsChangedHandler { get; set; }
        public void OnFrameDimensionsChanged (int width, int height)
        {
            FrameDimensionsChangedHandler?.Invoke (width, height);
        }
    }

    public class ConversationListener : Java.Lang.Object, IConversationListener
    {
        public Action<IConversation, ConversationException> ConversationEndedHandler { get;set; }
        public void OnConversationEnded (IConversation conversation, ConversationException conversationException)
        {
            ConversationEndedHandler?.Invoke (conversation, conversationException);
        }

        public Action<IConversation, IParticipant, ConversationException> FailedToConnectToParticipantHandler { get;set; }
        public void OnFailedToConnectParticipant (IConversation conversation, IParticipant participant, ConversationException conversationException)
        {
            FailedToConnectToParticipantHandler?.Invoke (conversation, participant, conversationException);
        }

        public Action<IConversation, IParticipant> ParticipantConnectedHandler { get;set; }
        public void OnParticipantConnected (IConversation conversation, IParticipant participant)
        {
            ParticipantConnectedHandler?.Invoke (conversation, participant);
        }

        public Action<IConversation, IParticipant> ParticipantDisconnectedHandler { get;set; }
        public void OnParticipantDisconnected (IConversation conversation, IParticipant participant)
        {
            ParticipantDisconnectedHandler?.Invoke (conversation, participant);
        }
    }

    public class ConversationsClientListener : Java.Lang.Object, IConversationsClientListener
    {
        public Action<IConversationsClient, ConversationException> FailedToStartHandler { get; set; }
        public void OnFailedToStartListening (IConversationsClient conversationsClient, ConversationException conversationException)
        {
            FailedToStartHandler?.Invoke (conversationsClient, conversationException);
        }

        public Action<IConversationsClient, IIncomingInvite> IncomingInviteHandler { get;set; }
        public void OnIncomingInvite (IConversationsClient conversationsClient, IIncomingInvite incomingInvite)
        {
            IncomingInviteHandler?.Invoke (conversationsClient, incomingInvite);
        }

        public Action<IConversationsClient, IIncomingInvite> InviteCancelledHandler { get;set; }
        public void OnIncomingInviteCancelled (IConversationsClient conversationsClient, IIncomingInvite incomingInvite)
        {
            InviteCancelledHandler?.Invoke (conversationsClient, incomingInvite);
        }

        public Action<IConversationsClient> StartListeningForInvitesHandler { get;set; }
        public void OnStartListeningForInvites (IConversationsClient conversationsClient)
        {
            StartListeningForInvitesHandler?.Invoke (conversationsClient);
        }

        public Action<IConversationsClient> StopListeningForInvitesHandler { get;set; }
        public void OnStopListeningForInvites (IConversationsClient conversationsClient)
        {
            StopListeningForInvitesHandler?.Invoke (conversationsClient);
        }
    }
}

