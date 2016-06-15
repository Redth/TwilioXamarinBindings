using System;
using Twilio.Common;

namespace Twilio.Conversations
{
	public class CapturerErrorListener : Java.Lang.Object, ICapturerErrorListener
	{
		public Action<CapturerException> ErrorHandler { get; set; }
		public void OnError(CapturerException error)
		{
			ErrorHandler?.Invoke(error);
		}
	}

	public class ConversationCallback : Java.Lang.Object, IConversationCallback
	{
		public Action<Conversation, TwilioConversationsException> ConversationHandler { get; set; }
		public void OnConversation(Conversation conversation, TwilioConversationsException ex)
		{
			ConversationHandler?.Invoke(conversation, ex);
		}
	}

	public class LocalMediaListener : Java.Lang.Object, global::Twilio.Conversations.LocalMedia.IListener
	{
		public Action<LocalMedia, LocalVideoTrack> LocalVideoTrackAddedHandler { get; set; }
		public void OnLocalVideoTrackAdded(LocalMedia media, LocalVideoTrack videoTrack)
		{
			LocalVideoTrackAddedHandler?.Invoke(media, videoTrack);
		}

		public Action<LocalMedia, LocalVideoTrack, TwilioConversationsException> LocalVideoTrackErrorHandler { get; set; }
		public void OnLocalVideoTrackError(LocalMedia media, LocalVideoTrack videoTrack, TwilioConversationsException error)
		{
			LocalVideoTrackErrorHandler?.Invoke(media, videoTrack, error);
		}

		public Action<LocalMedia, LocalVideoTrack> LocalVideoTrackRemovedHandler { get; set; }
		public void OnLocalVideoTrackRemoved(LocalMedia media, LocalVideoTrack videoTrack)
		{
			LocalVideoTrackRemovedHandler?.Invoke(media, videoTrack);
		}
	}

	public class ParticipantListener : Java.Lang.Object, global::Twilio.Conversations.Participant.IListener
	{
		public Action<Conversation, Participant, AudioTrack> AudioTrackAddedHandler { get; set; }
		public void OnAudioTrackAdded(Conversation conversation, Participant participant, AudioTrack audioTrack)
		{
			AudioTrackAddedHandler?.Invoke(conversation, participant, audioTrack);
		}

		public Action<Conversation, Participant, AudioTrack> AudioTrackRemovedHandler { get; set; }
		public void OnAudioTrackRemoved(Conversation conversation, Participant participant, AudioTrack audioTrack)
		{
			AudioTrackRemovedHandler?.Invoke(conversation, participant, audioTrack);
		}

		public Action<Conversation, Participant, IMediaTrack> TrackDisabledHandler { get; set; }
		public void OnTrackDisabled(Conversation conversation, Participant participant, IMediaTrack mediaTrack)
		{
			TrackDisabledHandler?.Invoke(conversation, participant, mediaTrack);
		}

		public Action<Conversation, Participant, IMediaTrack> TrackEnabledHandler { get; set; }
		public void OnTrackEnabled(Conversation conversation, Participant participant, IMediaTrack mediaTrack)
		{
			TrackEnabledHandler?.Invoke(conversation, participant, mediaTrack);
		}

		public Action<Conversation, Participant, VideoTrack> VideoTrackAddedHandler { get; set; }
		public void OnVideoTrackAdded(Conversation conversation, Participant participant, VideoTrack videoTrack)
		{
			VideoTrackAddedHandler?.Invoke(conversation, participant, videoTrack);
		}

		public Action<Conversation, Participant, VideoTrack> VideoTrackRemovedHandler { get; set; }
		public void OnVideoTrackRemoved(Conversation conversation, Participant participant, VideoTrack videoTrack)
		{
			VideoTrackRemovedHandler?.Invoke(conversation, participant, videoTrack);
		}
	}

	public class VideoRendererObserver : Java.Lang.Object, IVideoRendererObserver
	{
		public Action FirstFrameHandler { get; set; }
		public void OnFirstFrame()
		{
			FirstFrameHandler?.Invoke();
		}

		public Action<int, int, int> FrameDimensionsChangedHandler { get; set; }
		public void OnFrameDimensionsChanged(int width, int height, int i)
		{
			FrameDimensionsChangedHandler?.Invoke(width, height, i);
		}
	}

	public class ConversationListener : Java.Lang.Object, global::Twilio.Conversations.Conversation.IListener
	{
		public Action<Conversation, TwilioConversationsException> ConversationEndedHandler { get; set; }
		public void OnConversationEnded(Conversation conversation, TwilioConversationsException conversationException)
		{
			ConversationEndedHandler?.Invoke(conversation, conversationException);
		}

		public Action<Conversation, Participant, TwilioConversationsException> FailedToConnectToParticipantHandler { get; set; }
		public void OnFailedToConnectParticipant(Conversation conversation, Participant participant, TwilioConversationsException conversationException)
		{
			FailedToConnectToParticipantHandler?.Invoke(conversation, participant, conversationException);
		}

		public Action<Conversation, Participant> ParticipantConnectedHandler { get; set; }
		public void OnParticipantConnected(Conversation conversation, Participant participant)
		{
			ParticipantConnectedHandler?.Invoke(conversation, participant);
		}

		public Action<Conversation, Participant> ParticipantDisconnectedHandler { get; set; }
		public void OnParticipantDisconnected(Conversation conversation, Participant participant)
		{
			ParticipantDisconnectedHandler?.Invoke(conversation, participant);
		}
	}

	    public class ConversationsClientListener : Java.Lang.Object, global::Twilio.Conversations.TwilioConversationsClient.IListener
	    {
	        public Action<TwilioConversationsClient, TwilioConversationsException> FailedToStartHandler { get; set; }
	        public void OnFailedToStartListening (TwilioConversationsClient conversationsClient, TwilioConversationsException conversationException)
	        {
	            FailedToStartHandler?.Invoke (conversationsClient, conversationException);
	        }
	
	        public Action<TwilioConversationsClient, IncomingInvite> IncomingInviteHandler { get;set; }
	        public void OnIncomingInvite (TwilioConversationsClient conversationsClient, IncomingInvite incomingInvite)
	        {
	            IncomingInviteHandler?.Invoke (conversationsClient, incomingInvite);
	        }
	
	        public Action<TwilioConversationsClient, IncomingInvite> InviteCancelledHandler { get;set; }
	        public void OnIncomingInviteCancelled (TwilioConversationsClient conversationsClient, IncomingInvite incomingInvite)
	        {
	            InviteCancelledHandler?.Invoke (conversationsClient, incomingInvite);
	        }
	
	        public Action<TwilioConversationsClient> StartListeningForInvitesHandler { get;set; }
	        public void OnStartListeningForInvites (TwilioConversationsClient conversationsClient)
	        {
	            StartListeningForInvitesHandler?.Invoke (conversationsClient);
	        }
	
	        public Action<TwilioConversationsClient> StopListeningForInvitesHandler { get;set; }
	        public void OnStopListeningForInvites (TwilioConversationsClient conversationsClient)
	        {
	            StopListeningForInvitesHandler?.Invoke (conversationsClient);
	        }
	    }
}