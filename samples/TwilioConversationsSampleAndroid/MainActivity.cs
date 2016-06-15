using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Twilio.Conversations;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Media;
using System;
using Twilio.Common;
using Android.Content;
using System.Collections.Generic;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using System.Threading.Tasks;
using System.Net.Http;
using System.Json;

[assembly: UsesFeature("android.hardware.camera")]
[assembly: UsesFeature(GLESVersion = 0x00020000, Required = true)]

[assembly: UsesPermission(Android.Manifest.Permission.Camera)]
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.RecordAudio)]
[assembly: UsesPermission(Android.Manifest.Permission.ModifyAudioSettings)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessWifiState)]

namespace TwilioConversationsSampleAndroid
{
	[Activity(Label = "Twilio Conversations",
		MainLauncher = true,
		Theme = "@style/AppTheme",
		Icon = "@mipmap/ic_launcher")]
	public class MainActivity : AppCompatActivity
	{
		const string TAG = "TWILIO";
		const int CAMERA_MIC_PERMISSION_REQUEST_CODE = 1;
		const string urlBase = "http://twilio-video.redth.info/";

		/*
		 * Twilio AccessToken authorizes you to connect to the Conversations service
		 */
		string accessToken;

		/*
		 * Twilio Conversations Client allows a client to create or participate in a conversation.
		 */
		TwilioConversationsClient conversationsClient;

		/*
		 * A Conversation represents communication between the client and one or more participants.
		 */
		Conversation conversation;

		/*
		 * An OutgoingInvite represents an invitation to start or join a conversation with one or more participants
		 */
		OutgoingInvite outgoingInvite;

		/*
		 * A VideoViewRenderer receives frames from a local or remote video track and renders the frames to a provided view
		 */
		VideoViewRenderer participantVideoRenderer;
		VideoViewRenderer localVideoRenderer;

		/*
		 * Android application UI elements
		 */
		FrameLayout previewFrameLayout;
		ViewGroup localContainer;
		ViewGroup participantContainer;
		TextView conversationStatusTextView;
		AccessManager accessManager;
		CameraCapturer cameraCapturer;
		FloatingActionButton callActionFab;
		FloatingActionButton switchCameraActionFab;
		FloatingActionButton localVideoActionFab;
		FloatingActionButton muteActionFab;
		FloatingActionButton speakerActionFab;
		Android.Support.V7.App.AlertDialog alertDialog;

		bool muteMicrophone;
		bool pauseVideo;

		protected async override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_conversation);

			/*
			 * Check camera and microphone permissions. Needed in Android M.
			 */
			if (!checkPermissionForCameraAndMicrophone())
			{
				requestPermissionForCameraAndMicrophone();
			}

			/*
			 * Load views from resources
			 */
			previewFrameLayout = FindViewById<FrameLayout>(Resource.Id.previewFrameLayout);
			localContainer = FindViewById<ViewGroup>(Resource.Id.localContainer);
			participantContainer = FindViewById<ViewGroup>(Resource.Id.participantContainer);
			conversationStatusTextView = FindViewById<TextView>(Resource.Id.conversation_status_textview);

			callActionFab = FindViewById<FloatingActionButton>(Resource.Id.call_action_fab);
			switchCameraActionFab = FindViewById<FloatingActionButton>(Resource.Id.switch_camera_action_fab);
			localVideoActionFab = FindViewById<FloatingActionButton>(Resource.Id.local_video_action_fab);
			muteActionFab = FindViewById<FloatingActionButton>(Resource.Id.mute_action_fab);
			speakerActionFab = FindViewById<FloatingActionButton>(Resource.Id.speaker_action_fab);

			/*
			 * Enable changing the volume using the up/down keys during a conversation
			 */
			VolumeControlStream = Stream.VoiceCall;

			/*
			 * Get the capability token
			 */
			accessToken = await GetIdentity();

			/*
            * Initialize the Twilio Conversations SDK
            */
			initializeTwilioSdk();

			/*
            * Set the initial state of the UI
            */
			setCallAction();
		}

		protected override void OnResume()
		{
			base.OnResume();

			//if (participantVideoRenderer != null)
			//    participantVideoRenderer.OnResume ();

			//if (localVideoRenderer != null)
			//    localVideoRenderer.OnResume ();

			if (TwilioConversationsClient.IsInitialized && conversationsClient != null && !conversationsClient.IsListening)
				conversationsClient.Listen();
		}

		protected override void OnPause()
		{
			base.OnPause();

			//if(participantVideoRenderer != null)
			//    participantVideoRenderer.OnPause ();

			//if (localVideoRenderer != null)
			//    localVideoRenderer.OnPause ();

			if (TwilioConversationsClient.IsInitialized && conversationsClient != null && conversationsClient.IsListening)
				conversationsClient.Unlisten();
		}

		/*
     * The initial state when there is no active conversation.
     */
		void setCallAction()
		{
			callActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_call_white_24px));
			callActionFab.Show();
			callActionFab.Click += (sender, e) =>
			{
				showCallDialog();
			};
			switchCameraActionFab.Show();
			switchCameraActionFab.Click += (sender, e) =>
			{
				if (cameraCapturer != null)
					cameraCapturer.SwitchCamera();
			};

			localVideoActionFab.Show();
			localVideoActionFab.Click += (sender, e) =>
			{
				/*
                 * Enable/disable local video track
                 */
				pauseVideo = !pauseVideo;
				if (conversation != null)
				{
					var videoTrackList = conversation.LocalMedia.LocalVideoTracks;
					if (videoTrackList.Count > 0)
					{
						var videoTrack = videoTrackList[0];
						videoTrack.Enable(!pauseVideo);
					}
					else {
						Android.Util.Log.Warn(TAG, "LocalVideoTrack is not present, unable to pause");
					}
				}
				if (pauseVideo)
				{
					switchCameraActionFab.Hide();
					localVideoActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_videocam_off_red_24px));
				}
				else {
					switchCameraActionFab.Show();
					localVideoActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_videocam_green_24px));
				}
			};

			muteActionFab.Show();
			muteActionFab.Click += (sender, e) =>
			{
				/*
                 * Mute/unmute microphone
                 */
				muteMicrophone = !muteMicrophone;
				if (conversation != null)
					conversation.LocalMedia.Mute(muteMicrophone);

				if (muteMicrophone)
					muteActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_mic_off_red_24px));
				else
					muteActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_mic_green_24px));
			};
			speakerActionFab.Hide();
		}

		/*
		 * The actions performed during hangup.
		 */
		void setHangupAction()
		{
			callActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_call_end_white_24px));
			callActionFab.Show();
			callActionFab.Click += (sender, e) =>
			{
				hangup();
				setCallAction();
			};

			speakerActionFab.Show();
			speakerActionFab.Click += (sender, e) =>
			{
				/*
                 * Audio routing to speakerphone or headset
                 */
				if (conversationsClient == null)
				{
					Android.Util.Log.Error(TAG, "Unable to set audio output, conversation client is null");
					return;
				}
				var speakerOn = !(conversationsClient.AudioOutput == AudioOutput.Speakerphone) ? true : false;
				conversationsClient.AudioOutput = speakerOn ? AudioOutput.Speakerphone : AudioOutput.Headset;
				if (speakerOn)
					speakerActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_volume_down_green_24px));
				else
					speakerActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_volume_down_white_24px));
			};
		}

		/*
     * Creates an outgoing conversation UI dialog
     */
		private void showCallDialog()
		{
			var participantEditText = new EditText(this);
			alertDialog = Dialog.CreateCallParticipantsDialog(participantEditText, delegate
			{
				/*
                 * Make outgoing invite
                 */
				var participant = participantEditText.Text;
				if (!string.IsNullOrEmpty(participant) && conversationsClient != null)
				{
					stopPreview();
					// Create participants set (we support only one in this example)
					var participants = new List<string>();
					participants.Add(participant);

					// Create local media
					var localMedia = setupLocalMedia();

					// Create outgoing invite

					outgoingInvite = conversationsClient.InviteToConversation(participants, localMedia, new ConversationCallback
					{
						ConversationHandler = (c, err) =>
						{
							if (err == null)
							{
								// Participant has accepted invite, we are in active conversation
								this.conversation = c;
								conversation.ConversationListener = conversationListener();
							}
							else {
								Android.Util.Log.Error(TAG, err.Message);
								hangup();
								reset();
							}
						}
					});

					setHangupAction();
				}
				else {
					Android.Util.Log.Error(TAG, "invalid participant call");
					conversationStatusTextView.Text = "call participant failed";
				}
			}, delegate
			{
				setCallAction();
				alertDialog.Dismiss();
			}, this);

			alertDialog.Show();
		}

		/*
		 * Creates an incoming conversation UI dialog
		 */
		void showInviteDialog(IncomingInvite incomingInvite)
		{

			alertDialog = Dialog.CreateInviteDialog(incomingInvite.Inviter,
				new EventHandler<DialogClickEventArgs>((s, e) =>
				{
					/*
                 * Accept incoming invite
                 */
					var localMedia = setupLocalMedia();
					incomingInvite.Accept(localMedia, new ConversationCallback
					{
						ConversationHandler = (c, ex) =>
						{
							Android.Util.Log.Error(TAG, "sendConversationInvite onConversation");
							if (ex == null)
							{
								this.conversation = c;
								c.ConversationListener = conversationListener();
							}
							else {
								Android.Util.Log.Error(TAG, ex.Message);
								hangup();
								reset();
							}
						}
					});
					setHangupAction();
				}),
				new EventHandler<DialogClickEventArgs>((s, e) =>
				{
					incomingInvite.Reject();
					setCallAction();
				}), this);
			alertDialog.Show();
		}


		/*
		 * Initialize the Twilio Conversations SDK
		 */
		void initializeTwilioSdk()
		{
			//TwilioConversationsClient.SetLogLevel (TwilioConversationsClient.LogLevel.Debug);

			if (!TwilioConversationsClient.IsInitialized)
			{

				TwilioConversationsClient.Initialize(ApplicationContext);
				accessManager = AccessManager.Create(ApplicationContext, accessToken, accessManagerListener());
				conversationsClient = TwilioConversationsClient.Create(accessManager, conversationsClientListener());
				conversationsClient.AudioOutput = AudioOutput.Speakerphone;
				cameraCapturer = CameraCapturer.Create(this, CameraCapturer.CameraSource.CameraSourceFrontCamera, capturerErrorListener());

				conversationsClient.Listen();
				startPreview();

			}
		}

		void startPreview()
		{
			RunOnUiThread(() =>
						  cameraCapturer.StartPreview(localContainer));
		}

		void stopPreview()
		{
			if (cameraCapturer != null && cameraCapturer.IsPreviewing)
				cameraCapturer.StopPreview();
		}

		void hangup()
		{
			if (conversation != null)
			{
				conversation.Disconnect();
			}
			else if (outgoingInvite != null)
			{
				outgoingInvite.Cancel();
			}
		}

		/*
     * Resets UI elements. Used after conversation has ended.
     */
		void reset()
		{
			if (participantVideoRenderer != null)
			{
				//participantVideoRenderer.OnPause ();
				participantVideoRenderer = null;
			}
			localContainer.RemoveAllViews();
			localContainer = FindViewById<ViewGroup>(Resource.Id.localContainer);
			participantContainer.RemoveAllViews();

			if (conversation != null)
			{
				conversation.Dispose();
				conversation = null;
			}
			outgoingInvite = null;

			muteMicrophone = false;
			muteActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_mic_green_24px));

			pauseVideo = false;
			localVideoActionFab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_videocam_green_24px));
			if (conversationsClient != null)
				conversationsClient.AudioOutput = AudioOutput.Headset;

			setCallAction();
			startPreview();
		}




		/*
     * Conversation Listener
     */
		ConversationListener conversationListener()
		{
			return new ConversationListener
			{
				ParticipantConnectedHandler = (conversation, participant) =>
				{
					conversationStatusTextView.Text = "onParticipantConnected " + participant.Identity;
					participant.ParticipantListener = participantListener();
				},
				FailedToConnectToParticipantHandler = (conversation, participant, conversationException) =>
				{
					Android.Util.Log.Error(TAG, conversationException.Message);
					conversationStatusTextView.Text = "onFailedToConnectParticipant " + participant.Identity;
				},
				ParticipantDisconnectedHandler = (conversation, participant) =>
				{
					conversationStatusTextView.Text = "onParticipantDisconnected " + participant.Identity;
				},
				ConversationEndedHandler = (conversation, e) =>
				{
					conversationStatusTextView.Text = "onConversationEnded";
					reset();
				}
			};
		}

		/*
     * LocalMedia listener
     */
		LocalMediaListener localMediaListener()
		{
			return new LocalMediaListener
			{
				LocalVideoTrackAddedHandler = (conversation, localVideoTrack) =>
				{
					conversationStatusTextView.Text = "onLocalVideoTrackAdded";
					localVideoRenderer = new VideoViewRenderer(this, localContainer);
					localVideoTrack.AddRenderer(localVideoRenderer);
				},
				LocalVideoTrackRemovedHandler = (conversation, localVideoTrack) =>
				{
					conversationStatusTextView.Text = "onLocalVideoTrackRemoved";
					localContainer.RemoveAllViews();
				}
			};
		}


		ParticipantListener participantListener()
		{
			return new ParticipantListener
			{
				VideoTrackAddedHandler = (conversation, participant, videoTrack) =>
				{
					Android.Util.Log.Info(TAG, "onVideoTrackAdded " + participant.Identity);
					conversationStatusTextView.Text = "onVideoTrackAdded " + participant.Identity;

					// Remote participant
					participantVideoRenderer = new VideoViewRenderer(this, participantContainer);
					participantVideoRenderer.SetObserver(new VideoRendererObserver
					{
						FirstFrameHandler = () =>
						{
							Android.Util.Log.Info(TAG, "Participant onFirstFrame");
						},
						FrameDimensionsChangedHandler = (width, height, i) =>
						{
							Android.Util.Log.Info(TAG, "Participant onFrameDimensionsChanged " + width + " " + height);
						}
					});
					videoTrack.AddRenderer(participantVideoRenderer);
				}
			};
		}

		/*
		 * ConversationsClient listener
		 */
		ConversationsClientListener conversationsClientListener()
		{
			return new ConversationsClientListener
			{
				StartListeningForInvitesHandler = (c) =>
				{
					conversationStatusTextView.Text = "onStartListeningForInvites";
				},
				StopListeningForInvitesHandler = (c) =>
				{
					conversationStatusTextView.Text = "onStopListeningForInvites";
				},
				FailedToStartHandler = (c, ex) =>
				{
					conversationStatusTextView.Text = "onFailedToStartListening";
				},
				IncomingInviteHandler = (c, invite) =>
				{
					conversationStatusTextView.Text = "onIncomingInvite";
					if (conversation == null)
					{
						showInviteDialog(invite);
					}
					else {
						Android.Util.Log.Warn(TAG, string.Format("Conversation in progress. Invite from {0} ignored", invite.Inviter));
					}
				},
				InviteCancelledHandler = (c, invite) =>
				{
					conversationStatusTextView.Text = "onIncomingInviteCancelled";
				}
			};
		}


		/*
		 * CameraCapture error listener
		 */
		ICapturerErrorListener capturerErrorListener()
		{
			return new CapturerErrorListener
			{
				ErrorHandler = (e) =>
				{
					Android.Util.Log.Error(TAG, "Camera capturer error:" + e.Message);
				}
			};
		}

		TwilioAccessManagerListener accessManagerListener()
		{
			return new TwilioAccessManagerListener()
			{
				TokenExpiredHandler = (am) =>
				{
					conversationStatusTextView.Text = "onTokenExpired";
				},
				TokenUpdatedHandler = (am) =>
				{
					conversationStatusTextView.Text = "onTokenUpdated";
				},
				ErrorHandler = (am, msg) =>
				{
					conversationStatusTextView.Text = "onError";
				}
			};
		}



		LocalMedia setupLocalMedia()
		{

			var localMedia = new LocalMedia(localMediaListener());
			var localVideoTrack = new LocalVideoTrack(cameraCapturer);

			if (pauseVideo)
			{
				localVideoTrack.Enable(false);
			}
			localMedia.AddLocalVideoTrack(localVideoTrack);
			if (muteMicrophone)
				localMedia.Mute(true);
			return localMedia;
		}

		bool checkPermissionForCameraAndMicrophone()
		{
			var resultCamera = ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.Camera);
			var resultMic = ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.RecordAudio);
			if ((resultCamera == Android.Content.PM.Permission.Granted) && (resultMic == Android.Content.PM.Permission.Granted))
				return true;
			else
				return false;
		}

		void requestPermissionForCameraAndMicrophone()
		{
			if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Android.Manifest.Permission.Camera) || ActivityCompat.ShouldShowRequestPermissionRationale(this, Android.Manifest.Permission.RecordAudio))
			{
				Toast.MakeText(this, "Camera and Microphone permissions needed. Please allow in App Settings for additional functionality.", ToastLength.Long).Show();
			}
			else {
				ActivityCompat.RequestPermissions(this, new[] { Android.Manifest.Permission.Camera, Android.Manifest.Permission.RecordAudio }, CAMERA_MIC_PERMISSION_REQUEST_CODE);
			}
		}

		async Task<string> GetIdentity()
		{
			var androidId = Android.Provider.Settings.Secure.GetString(ContentResolver,
				Android.Provider.Settings.Secure.AndroidId);

			var tokenEndpoint = urlBase + $"token.php?device={androidId}";

			var http = new HttpClient();
			// If you're here with a NameResolutionFailure try using a non Nexus5 emulator / device
			// or check internet connection
			var data = await http.GetStringAsync(tokenEndpoint);

			var json = System.Json.JsonObject.Parse(data);

			var token = json["token"]?.ToString()?.Trim('"');

			return token;
		}
	}
}
