using System;
using System.Linq;
using MonoTouch.Dialog;
using UIKit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Json;

using Twilio.IPMessaging;
using System.Collections.Generic;
using CoreGraphics;

namespace TwilioIPMessagingSampleiOS
{
    public partial class ViewController : UIViewController, ITwilioIPMessagingClientDelegate
    {
        public ViewController (IntPtr handle) : base (handle)
        {
        }

        MsgsDataSource dataSource;
        TwilioIPMessagingClient twilio;
        Channel generalChannel;

        public async override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            // Perform any additional setup after loading the view, typically from a nib.

            dataSource = new MsgsDataSource ();
            tableView.Source = dataSource;
            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = 70;

            var token = await GetIdentity ();

            twilio = TwilioIPMessagingClient.Create (token, this);

            twilio.GetChannelsList ((result, channels) => {
                generalChannel = channels.GetChannelWithUniqueName ("general");
                generalChannel.Join (r => {
                    
                });
            });
        }

        async Task<string> GetIdentity ()
        {
            var deviceId = UIDevice.CurrentDevice.IdentifierForVendor.AsString ();

            var tokenEndpoint = $"http://twilio.redth.info/token.php?device={deviceId}";

            var http = new HttpClient ();
            var data = await http.GetStringAsync (tokenEndpoint);

            var json = JsonObject.Parse (data);

            var token = json["token"]?.ToString ()?.Trim ('"');

            return token;
        }

        [Foundation.Export ("ipMessagingClient:channelHistoryLoaded:")]
        public void ChannelHistoryLoaded (TwilioIPMessagingClient client, Channel channel)
        {
            var msgs = channel.Messages.AllMessages.OrderBy (m => m.Timestamp);

            dataSource.UpdateMessages (msgs);

            tableView.ReloadData ();
        }

        [Foundation.Export ("ipMessagingClient:errorReceived:")]
        public void ErrorReceived (TwilioIPMessagingClient client, TwilioError error)
        {
            Console.WriteLine ("Error: " + error.Description);
        }

        [Foundation.Export ("ipMessagingClient:channel:messageAdded:")]
        public void MessageAdded (TwilioIPMessagingClient client, Channel channel, Message message)
        {
            dataSource.AddMessage (message);
            tableView.ReloadData ();
        }

        partial void ButtonSend_TouchUpInside (UIButton sender)
        {
            var msg = generalChannel.Messages.CreateMessage (textMessage.Text);
            buttonSend.Enabled = false;
            generalChannel.Messages.SendMessage(msg, r => {

                BeginInvokeOnMainThread (() => {
                    textMessage.Text = string.Empty;
                    buttonSend.Enabled = true;
                });

            });
        }
    }

    class MsgsDataSource : UITableViewSource
    {
        public void UpdateMessages (IEnumerable<Message> messages)
        {
            Messages.Clear ();
            Messages.AddRange (messages);
        }

        public void AddMessage (Message msg)
        {
            Messages.Add (msg);
        }

        public List<Message> Messages { get; private set; } = new List<Message> ();

        public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var msg = Messages [indexPath.Row];

            var cell = tableView.DequeueReusableCell ("MessageCell") as MessageCell;
            cell.Message = msg;
            cell.SetNeedsUpdateConstraints ();
            cell.UpdateConstraintsIfNeeded ();

            return cell;
        }

        public override nint NumberOfSections (UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection (UITableView tableView, nint section)
        {
            return Messages.Count;
        }
    }
}
