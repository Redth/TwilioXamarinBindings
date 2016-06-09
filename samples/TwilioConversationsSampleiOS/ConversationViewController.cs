using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Twilio.Conversations;
using CoreGraphics;

namespace TwilioConversationsSampleiOS
{
    partial class ConversationViewController : UIViewController,
        IConversationDelegate, IParticipantDelegate, ILocalMediaDelegate, IVideoTrackDelegate
    {
        public ConversationViewController (IntPtr handle) : base (handle)
        {
        }

        public string InviteeIdentity { get; set; }

        public TwilioConversationsClient TwilioClient { get;set; }

        public IncomingInvite IncomingInvite { get;set; }

        LocalMedia localMedia;
        CameraCapturer camera;
        Conversation conversation;
        OutgoingInvite outgoingInvite;
        VideoTrack remoteVideoTrack;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            /* LocalMedia represents our local camera and microphone (media) configuration */
            localMedia = new LocalMedia (this);

            if (ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.DEVICE) {
                /* Microphone is enabled by default, to enable Camera, we first create a Camera capturer */
                camera = localMedia.AddCameraTrack ();
            } else {
                localVideoContainer.Hidden = true;
                pauseButton.Enabled = false;
                flipCameraButton.Enabled = false;
            }

            /* 
             We attach a view to display our local camera track immediately.
             You could also wait for localMedia:addedVideoTrack to attach a view or add a renderer. 
             */
            if (camera != null) {
                camera.VideoTrack.Attach (localVideoContainer);
                camera.VideoTrack.Delegate = this;
            }

            /* For this demonstration, we always use Speaker audio output (vs. TWCAudioOutputReceiver) */
            TwilioConversationsClient.SetAudioOutput (AudioOutput.Speaker);

        }

        public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);

            if (IncomingInvite != null) {
                /* This ViewController is being loaded to present an incoming Conversation request */
                IncomingInvite.AcceptWithLocalMedia (localMedia, acceptHandler);
            }
            else if (!string.IsNullOrEmpty (InviteeIdentity)) {
                /* This ViewController is being loaded to present an outgoing Conversation request */
                SendConversationInvite ();
            }
        }

        public override void UpdateViewConstraints ()
        {
            base.UpdateViewConstraints ();

            if (camera != null) {
                var cameraTrack = camera.VideoTrack;
				if (cameraTrack != null && cameraTrack.VideoDimensions().Width > 0 && cameraTrack.VideoDimensions().Height > 0) {
					var dimensions = camera.VideoTrack.VideoDimensions();

                    if (dimensions.Width > 0 && dimensions.Height > 0) {
                        var boundingRect = new CGRect (0, 0, 160, 160);
                        var fitRect = AVFoundation.AVUtilities.WithAspectRatio (boundingRect, new CGSize (dimensions.Width, dimensions.Height));
                        var fitSize = fitRect.Size;
                        localVideoWidthConstraint.Constant = fitSize.Width;
                        localVideoHeightConstraint.Constant = fitSize.Height;
                    }
                }
            }
        }

        void acceptHandler (Conversation c, NSError error) 
        {
            if (c != null) {
                c.Delegate = this;
                conversation = c;
            }
            else {
                Console.WriteLine (@"Invite failed with error: {0}", error);
                DismissConversation ();
            }
        }


        void SendConversationInvite ()
        {
            if (TwilioClient != null) {
                /* 
                 * The createConversation method attempts to create and connect to a Conversation. The 'localStatusChanged' delegate method can be used to track the success or failure of connecting to the newly created Conversation.
                */
                outgoingInvite = TwilioClient.InviteToConversation (InviteeIdentity, localMedia, acceptHandler);
            }
        }

        void DismissConversation ()
        {
            localMedia = null;
            conversation = null;
            DismissViewControllerAsync (true);
        }

        void UpdatePauseButton ()
        {
            var title = camera.VideoTrack.Enabled ? "Pause" : "Unpause";
            pauseButton.SetTitle (title, UIControlState.Normal);
        }
            
        partial void flipButton_Clicked (UIButton sender)
        {
            if (conversation != null)
                camera.FlipCamera ();
        }

        partial void muteButton_Clicked (UIButton sender)
        {
            if (conversation != null) {
                conversation.LocalMedia.MicrophoneMuted = !conversation.LocalMedia.MicrophoneMuted;
                muteButton.SetTitle (conversation.LocalMedia.MicrophoneMuted ? "Unmute" : "Mute", UIControlState.Normal);
            }
        }

        partial void pauseButton_Clicked (UIButton sender)
        {
            if (conversation != null) {
                camera.VideoTrack.Enabled = !camera.VideoTrack.Enabled;
                UpdatePauseButton ();
            }
        }

        partial void hangupButton_Clicked (UIButton sender)
        {
            if (conversation != null)
                conversation.Disconnect ();
            if (IncomingInvite != null)
                IncomingInvite.Reject ();
            if (outgoingInvite != null)
                outgoingInvite.Cancel ();
            remoteVideoTrack = null;
        }

        [Export ("conversation:didConnectParticipant:")]
        public void ConnectedToParticipant (Conversation conversation, Participant participant)
        {
            Console.WriteLine ($"Participant connected: {participant.Identity}");
            participant.Delegate = this;
        }

        [Export ("conversationEnded:")]
        public void ConversationEnded (Conversation conversation)
        {
            DismissConversation ();
        }

        [Export ("conversationEnded:error:")]
        public void ConversationEnded (Conversation conversation, NSError error)
        {
            DismissConversation ();
        }

        [Export ("conversation:didDisconnectParticipant:")]
        public void DisconnectedFromParticipant (Conversation conversation, Participant participant)
        {
            Console.WriteLine ($"Participant disconnected: {participant.Identity}");

            if (conversation.Participants.Length <= 1)
                conversation.Disconnect ();
        }

        [Export ("conversation:didFailToConnectParticipant:error:")]
        public void FailedToConnectToParticipant (Conversation conversation, Participant participant, NSError error)
        {
            Console.WriteLine ("Participant failed to connect: {0} with error: {1}", participant.Identity, error.Description);

            conversation.Disconnect ();
        }


        [Export ("localMedia:didAddVideoTrack:")]
        public void DidAddVideoTrack (LocalMedia media, VideoTrack videoTrack)
        {
            Console.WriteLine ("Local video track added: {0}", videoTrack);
        }

        [Export ("localMedia:didRemoveVideoTrack:")]
        public void DidRemoveVideoTrack (LocalMedia media, VideoTrack videoTrack)
        {
            Console.WriteLine ("Local video track removed: {0}", videoTrack);

            /* You do not need to call [videoTrack detach:] here, your view will be detached once this call returns. */
            camera = null;
        }

        [Export ("participant:addedAudioTrack:")]
        public void AddedAudioTrack (Participant participant, AudioTrack audioTrack)
        {
            Console.WriteLine ("Audio added for participant: {0}", participant.Identity);
        }

        [Export ("participant:addedVideoTrack:")]
        public void AddedVideoTrack (Participant participant, VideoTrack videoTrack)
        {
            Console.WriteLine ("Video added for participant: {0}", participant.Identity);

            remoteVideoTrack = videoTrack;

            BeginInvokeOnMainThread (() => {
                remoteVideoTrack.Attach (this.remoteVideoContainer);
                remoteVideoTrack.Delegate = this;
            });
        }

        [Export ("participant:disabledTrack:")]
        public void DisabledTrack (Participant participant, MediaTrack track)
        {
            Console.WriteLine ("Disabled track: {0}", track);
        }

        [Export ("participant:enabledTrack:")]
        public void EnabledTrack (Participant participant, MediaTrack track)
        {
            Console.WriteLine ("Enabled track: {0}", track);
        }

        [Export ("participant:removedAudioTrack:")]
        public void RemovedAudioTrack (Participant participant, AudioTrack audioTrack)
        {
            Console.WriteLine ("Audio removed for participant: {0}", participant.Identity);
        }

        [Export ("participant:removedVideoTrack:")]
        public void RemovedVideoTrack (Participant participant, VideoTrack videoTrack)
        {
            Console.WriteLine ("Video removed for participant: {0}", participant.Identity);

            /* You do not need to call [videoTrack detach:] here, your view will be detached once this call returns. */
        }

        [Export ("videoTrack:dimensionsDidChange:")]
        public void DimensionsDidChange (VideoTrack track, CoreMedia.CMVideoDimensions dimensions)
        {
            Console.WriteLine ("Dimensions changed to: {0} x {1}", dimensions.Width, dimensions.Height);

            View.SetNeedsUpdateConstraints ();
        }
    }

}
