using Android.App;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Twilio.IPMessaging;
using System.Collections.Generic;
using Android.Content;
using Twilio.IPMessaging.Impl;
using System.Linq;

namespace TwilioIPMessagingSample
{
    [Activity (Label = "Twilio IP Msging", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, IPMessagingClientListener, IChannelListener
    {
        internal const string TAG = "TWILIO";

        Button buttonSend;
        EditText textMessage;
        ListView listView;
        MessagesAdapter adapter;

        ITwilioIPMessagingClient twilio;
        IChannel generalChannel;

        protected async override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            buttonSend = FindViewById<Button> (Resource.Id.buttonSend);
            textMessage = FindViewById<EditText> (Resource.Id.textMessage);
            listView = FindViewById<ListView> (Resource.Id.listView);

            adapter = new MessagesAdapter (this);
            listView.Adapter = adapter;

            buttonSend.Click += ButtonSend_Click;

            TwilioIPMessagingSDK.SetLogLevel ((int)Android.Util.LogPriority.Debug);

            if (!TwilioIPMessagingSDK.IsInitialized) {
                Console.WriteLine ("Initialize");

                TwilioIPMessagingSDK.InitializeSDK (this, new InitListener {
                    InitializedHandler = async delegate {
                        await Setup ();
                    },
                    ErrorHandler = err => {
                        Console.WriteLine (err.Message);
                    }
                });
            } else {
                await Setup ();
            }
        }

        async Task Setup ()
        {
            var token = await GetIdentity ();

            twilio = TwilioIPMessagingSDK.CreateIPMessagingClientWithToken (token, this);

            twilio.Channels.LoadChannelsWithListener (new StatusListener {
                SuccessHandler = () => {
                    generalChannel = twilio.Channels.GetChannelByUniqueName ("general");
                    generalChannel.Listener = this;
                    generalChannel.Join (new StatusListener {
                        SuccessHandler = () => {
                            RunOnUiThread (() => 
                                Toast.MakeText (this, "Joined general channel!", ToastLength.Short).Show ());
                        }
                    });
                }
            });
        }

        void ButtonSend_Click (object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace (textMessage.Text)) {
                var msg = generalChannel.Messages.CreateMessage (textMessage.Text);

                generalChannel.Messages.SendMessage (msg, new StatusListener {
                    SuccessHandler = () => {
                        RunOnUiThread (() => {
                            textMessage.Text = string.Empty;
                        });
                    }
                });
            }
        }

        async Task<string> GetIdentity ()
        {
            var androidId = Android.Provider.Settings.Secure.GetString (ContentResolver,
                                Android.Provider.Settings.Secure.AndroidId);
            
            var tokenEndpoint = $"http://twilio.redth.info/token.php?device={androidId}";

            var http = new HttpClient ();
            var data = await http.GetStringAsync (tokenEndpoint);

            var json = System.Json.JsonObject.Parse (data);

            var identity = json["identity"]?.ToString ()?.Trim ('"');
            var token = json["token"]?.ToString ()?.Trim ('"');

            return token;
        }


        public void OnAttributesChange (string attr)
        {
            
        }
        public void OnChannelAdd (IChannel channel)
        {
            
        }
        public void OnChannelChange (IChannel channel)
        {
            Android.Util.Log.Debug (TAG, "Channel Changed");
            adapter.UpdateMessages (channel.Messages.GetMessages ());
        }
        public void OnChannelDelete (IChannel channel)
        {
        }
        public void OnChannelHistoryLoaded (IChannel channel)
        {
            Android.Util.Log.Debug (TAG, "Channel History Loaded");
            adapter.UpdateMessages (channel.Messages.GetMessages ());
            listView.SmoothScrollToPosition (adapter.Count - 1);
        }
        public void OnError (int code, string msg)
        {
            Console.WriteLine ($"Error: {code} -> {msg}");
        }

        public void OnAttributesChange (IDictionary<string, string> attrs)
        {
        }

        public void OnMemberChange (IMember member)
        {
            Android.Util.Log.Debug (TAG, $"Member Changed: {member.Sid}");
        }

        public void OnMemberDelete (IMember member)
        {
        }

        public void OnMemberJoin (IMember member)
        {
            Android.Util.Log.Debug (TAG, $"Member Joined: {member.Sid}");
        }

        public void OnMessageAdd (IMessage message)
        {
            adapter.AddMessage (message);
            listView.SmoothScrollToPosition (adapter.Count - 1);
        }

        public void OnMessageChange (IMessage message)
        {
            Android.Util.Log.Debug (TAG, "Message Changed");
        }

        public void OnMessageDelete (IMessage message)
        {
            Android.Util.Log.Debug (TAG, "Message Deleted");
        }

        public void OnTypingEnded (IMember member)
        {
            Android.Util.Log.Debug (TAG, $"Typing Ended for {member.Sid}");
        }

        public void OnTypingStarted (IMember member)
        {
            Android.Util.Log.Debug (TAG, $"Typing Started for {member.Sid}");
        }
    }

    class MessagesAdapter : BaseAdapter<IMessage>
    {
        public MessagesAdapter (Activity parentActivity)
        {
            activity = parentActivity;
        }

        List<IMessage> messages = new List<IMessage> ();
        Activity activity;

        public void UpdateMessages (IEnumerable<IMessage> msgs)
        {
            lock (messages) {
                messages.Clear ();
                messages.AddRange (msgs.OrderBy (m => m.TimeStamp));
            }

            activity.RunOnUiThread (() =>
                NotifyDataSetChanged ());
        }

        public void AddMessage (IMessage msg)
        {
            lock (messages) {
                messages.Add (msg);
            }

            activity.RunOnUiThread (() =>
                NotifyDataSetChanged ());
        }

        public override long GetItemId (int position)
        {
            return position;
        }

        public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = convertView as LinearLayout ?? activity.LayoutInflater.Inflate (Resource.Layout.MessageItemLayout, null) as LinearLayout;
            var msg = messages [position];

            view.FindViewById<TextView> (Resource.Id.textAuthor).Text = msg.Author;
            view.FindViewById<TextView> (Resource.Id.textTimestamp).Text = msg.TimeStamp;
            view.FindViewById<TextView> (Resource.Id.textMessage).Text = msg.MessageBody;

            return view;
        }

        public override int Count { get { return messages.Count; } }
        public override IMessage this [int index] { get { return messages [index]; } }
    }
}
