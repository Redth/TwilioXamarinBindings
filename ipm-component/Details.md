Create chat experiences in your Xamarin.iOS and Xamarin.Android apps with [Twilio IP Messaging](http://twilio.com/ip-messaging). Twilio IP Messaging makes it easy for you to build a chat-powered app without the need to set up and scale a backend infrastructur.

To learn more about Twilio, visit [http://twilio.com].

The following code shows some basic usage of the component. For more details, have a look at the Getting Started guide or the included Samples.

## iOS

```csharp
using Twilio.Common;
using Twilio.IPMessaging;
//...
TwilioIPMessagingClient client;
string identity;
Channel generalChannel;
MessagesDataSource dataSource;

public override void ViewDidLoad ()
{
    var token = await GetToken();
    this.NavigationItem.Prompt = $"Logged in as {identity}";
    var accessManager = TwilioAccessManager.Create(token, this);
    client = TwilioIPMessagingClient.Create(accessManager, this);

    client.GetChannelsList((result, channels) =>
			{
				generalChannel = channels.GetChannelWithUniqueName("general");

				if (generalChannel != null)
				{
					generalChannel.Join(r =>
					{
						Console.WriteLine("successfully joined general channel!");
					});
				}
				else
				{
					var options = new NSDictionary("TWMChannelOptionFriendlyName", "General Chat Channel", "TWMChannelOptionType", 0);

					channels.CreateChannel(options, (creationResult, channel) =>
					{
						if (creationResult.IsSuccessful())
						{
							generalChannel = channel;
							generalChannel.Join(r =>
							{
								generalChannel.SetUniqueName("general", res => { });
							});
						}
					});
				}

			});

    sendButton.TouchUpInside += (sender, e) {
      var msg = generalChannel.Messages.CreateMessage(messageTextField.Text);
      sendButton.Enabled = false;
      generalChannel.Messages.SendMessage(msg, r =>
      {

	       BeginInvokeOnMainThread(() =>
	        {
		          messageTextField.Text = string.Empty;
		            sendButton.Enabled = true;
	        });

      });
    };
}

[Foundation.Export("ipMessagingClient:channel:messageAdded:")]
public void MessageAdded(TwilioIPMessagingClient client, Channel channel, Message message)
{
	dataSource.AddMessage(message);
	tableView.ReloadData();
	if (dataSource.Messages.Count > 0)
	{
		ScrollToBottomMessage();
	}
}


```

## Android
``` csharp
using Twilio.Common;
using Twilio.IPMessaging;

public class MainActivity : Activity, IPMessagingClientListener, IChannelListener, ITwilioAccessManagerListener
{
    ITwilioIPMessagingClient client;
    MessagesAdapter adapter;
		IChannel generalChannel;

    protected async override void OnCreate(Bundle savedInstanceState)
		{
      TwilioIPMessagingSDK.SetLogLevel((int)Android.Util.LogPriority.Debug);

			if (!TwilioIPMessagingSDK.IsInitialized)
			{
				Console.WriteLine("Initialize");

				TwilioIPMessagingSDK.InitializeSDK(this, new InitListener
				{
					InitializedHandler = async delegate
					{
						await Setup();
					},
					ErrorHandler = err =>
					{
						Console.WriteLine(err.Message);
					}
				});
			}
			else {
				await Setup();
			}

      sendButton.Click += (sender, e) => {
        if (!string.IsNullOrWhiteSpace(textMessage.Text))
        {
        	var msg = generalChannel.Messages.CreateMessage(textMessage.Text);

        	generalChannel.Messages.SendMessage(msg, new StatusListener
        	{
        		SuccessHandler = () =>
        		{
        			RunOnUiThread(() =>
        			{
        				textMessage.Text = string.Empty;
        			});
        		}
        	});
        }
      };
    }

    async Task Setup()
    {
    	var token = await GetIdentity();
    	var accessManager = TwilioAccessManagerFactory.CreateAccessManager(token, this);
    	client = TwilioIPMessagingSDK.CreateIPMessagingClientWithAccessManager(accessManager, this);

    	client.Channels.LoadChannelsWithListener(new StatusListener
    	{
    		SuccessHandler = () =>
    		{
    			generalChannel = client.Channels.GetChannelByUniqueName("general");

    			if (generalChannel != null)
    			{
    				generalChannel.Listener = this;
    				JoinGeneralChannel();
    			}
    			else
    			{
    				CreateAndJoinGeneralChannel();
    			}
    		}
    	});
    }

    void JoinGeneralChannel()
    {
    	generalChannel.Join(new StatusListener
    	{
    		SuccessHandler = () =>
    		{
    			RunOnUiThread(() =>
    			   Toast.MakeText(this, "Joined general channel!", ToastLength.Short).Show());
    		}
    	});
    }

    void CreateAndJoinGeneralChannel()
    {
    	var options = new Dictionary<string, Java.Lang.Object>();
    	options["friendlyName"] = "General Chat Channel";
    	options["ChannelType"] = ChannelChannelType.ChannelTypePublic;
    	client.Channels.CreateChannel(options, new CreateChannelListener
    	{
    		OnCreatedHandler = channel =>
    		{
    			generalChannel = channel;
    			channel.SetUniqueName("general", new StatusListener
    			{
    				SuccessHandler = () => { Console.WriteLine("set unique name successfully!"); }
    			});
    			this.JoinGeneralChannel();
    		},
    		OnErrorHandler = () => { }
    	});
    }
}
```
