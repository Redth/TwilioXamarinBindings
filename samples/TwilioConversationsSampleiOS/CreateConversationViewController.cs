using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Twilio.Conversations;
using System.Net.Http;
using System.Json;
using System.Threading.Tasks;

namespace TwilioConversationsSampleiOS
{
    partial class CreateConversationViewController : UIViewController, ITwilioConversationsClientDelegate, ITwilioAccessManagerDelegate
    {
        public CreateConversationViewController (IntPtr handle) : base (handle)
        {
        }

        UIAlertView alertView;
        IncomingInvite incomingInvite;
        TwilioConversationsClient twilio;

        public async override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            listeningStatusLabel.Text = "Attempting to listen for Invites...";

            var token = await GetIdentity ();

            TwilioConversationsClient.LogLevel = LogLevel.Info;

			var accessManager = TwilioAccessManager.Create(token, this);

			twilio = TwilioConversationsClient.From(accessManager, this);
            
			twilio.Listen ();
        }

        async Task<string> GetIdentity ()
        {
            var deviceId = UIDevice.CurrentDevice.IdentifierForVendor.AsString ();

            var tokenEndpoint = $"http://twilio-video.redth.info/token.php?device={deviceId}";

            var http = new HttpClient ();
            var data = await http.GetStringAsync (tokenEndpoint);

            var json = JsonObject.Parse (data);

            var token = json ["token"]?.ToString ()?.Trim ('"');

            return token;
        }

        [Foundation.Export ("conversationsClientDidStartListeningForInvites:")]
        public void DidStartListeningForInvites (Twilio.Conversations.TwilioConversationsClient conversationsClient)
        {
            BeginInvokeOnMainThread (() => {                
                listeningStatusLabel.Text = "Now listening for Conversation invites...";
                selfIdentityLabel.Text = "Your Identity: " + conversationsClient.Identity;
            });

            Task.Delay (1000).ContinueWith (t => {
                BeginInvokeOnMainThread (() => {
                    listeningStatusLabel.Hidden = true;
                    inviteeLabel.Hidden = false;
                    inviteeIdentityField.Hidden = false;
                    createConversationButton.Hidden = false;
                    selfIdentityLabel.Hidden = false;
                });
            });
        }

        [Foundation.Export ("conversationsClient:didReceiveInvite:")]
        public void DidReceiveInvite (Twilio.Conversations.TwilioConversationsClient conversationsClient, Twilio.Conversations.IncomingInvite invite)
        {
            // Reject if we already have an incoming invite, or are in a conversation
            if (this.incomingInvite != null || NavigationController.VisibleViewController != this) {
                invite.Reject ();
                return;
            }

            incomingInvite = invite;

            alertView = new UIAlertView ("", $"Incoming invite from {invite.From}", null, "Reject", "Acept");
            alertView.Clicked += (sender, e) => {
                if (alertView.CancelButtonIndex != e.ButtonIndex) {
                    PerformSegue ("TSQSegueAcceptInvite", this);
                }
            };
            alertView.Show ();

        }

        public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue (segue, sender);

            if (segue.Identifier == "TSQSegueStartConversation") {
                var conversationVC = (ConversationViewController)segue.DestinationViewController;
                conversationVC.InviteeIdentity = inviteeIdentityField.Text;
                conversationVC.TwilioClient = twilio;
            }
            else if (segue.Identifier == "TSQSegueAcceptInvite") {
                var conversationVC = (ConversationViewController)segue.DestinationViewController;
                conversationVC.IncomingInvite = incomingInvite;
                conversationVC.TwilioClient = twilio;
            }

        }

        [Foundation.Export ("conversationsClient:inviteDidCancel:")]
        public void InviteDidCancel (Twilio.Conversations.TwilioConversationsClient conversationsClient, Twilio.Conversations.IncomingInvite invite)
        {
            if (alertView != null) {
                alertView.DismissWithClickedButtonIndex (alertView.CancelButtonIndex, true);
                incomingInvite = null;
            }
        }

        [Foundation.Export ("conversationsClient:didFailToStartListeningWithError:")]
        public void DidFailToStartListening (Twilio.Conversations.TwilioConversationsClient conversationsClient, Foundation.NSError error)
        {
            listeningStatusLabel.Text = "Failed to start listening for Invites";
        }

        [Foundation.Export ("conversationsClientDidStopListeningForInvites:error:")]
        public void DidStopListeningForInvites (Twilio.Conversations.TwilioConversationsClient conversationsClient, Foundation.NSError error)
        {
            if (error != null) {
                Console.WriteLine ("Successfully stopped listening for Conversation invites");
                twilio = null;
            } else {
                Console.WriteLine ($"Stopped listening for Conversation invites (error): {error.Code}");
            }
        }

        partial void createConversationButton_Clicked (UIButton sender)
        {
            if (!string.IsNullOrEmpty (inviteeIdentityField.Text)) {
                inviteeIdentityField.ResignFirstResponder ();

                /* Present the conversation ViewController and initiate the Conversation once the view appears */
                PerformSegue ("TSQSegueStartConversation", this);
            }
        }
    }
}
