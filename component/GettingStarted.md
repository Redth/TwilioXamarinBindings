[Twilio IP Messaging](http://twilio.com/ip-messaging) is a cross-platform SDK that makes it possible for developers to build chat applications on the iOS and Android platforms without worrying about creating and scaling a backend. With the Xamarin Component for Twilio IP Messaging you'll be able to quickly add text-based chat to your existing and new Xamarin applications.

## Creating a Twilio Account

To use Twilio IP Messaging you'll need a Twilio account. Head over to [https://twilio.com/try-twilio](https://twilio.com/try-twilio) to create your free account before continuing with this guide.

## Generating access tokens and testing the chat server

To make all of this work we will need some server code to generate access tokens. An access token tells Twilio who a chat user is and what they can and can't do within the IP Messaging service. You can find out more about access tokens [here](https://www.twilio.com/docs/api/ip-messaging/guides/identity).

Head over to [this guide](https://github.com/TwilioDevEd/ipm-quickstart-csharp) and follow the instructions to get the ASP.NET version of our quickstart working on your machine. If you'd like to use a different backend language, you can find a full list of quickstart servers [here](https://www.twilio.com/docs/api/ip-messaging/guides/quickstart-js).

Once you have it set up correctly, open it in your browser and you should be looking at a chat application. You've been granted an access token by the server and assigned a random username. We'll use this same server infrastructure to request a token for our mobile application.

At this point you can use any of the included samples or continue on to build it yourself.

## Adding Twilio IP Messaging functionality on iOS

Assuming you have an application already created and the Twilio IP Messaging component is added to it, add the following `using` statements to a new View Controller in your application:

```csharp
// If you don't have these in your app
using System.Threading.Tasks;
using System.Net.Http;
using System.Json;
using System.Collections.Generic;

// For IP Messaging
using Twilio.Common;
using Twilio.IPMessaging;
```
Configure your view controller's storyboard to have a table view at the top for displaying messages and a `UITextField` and `UIButton` at the bottom for sending messages. In the view controller class, configure it so that it implements the following interfaces:

```csharp
public partial class ViewController : UIViewController, ITwilioIPMessagingClientDelegate, IUITextFieldDelegate, ITwilioAccessManagerDelegate
{
  // ...
}
```

Since this is a chat app and we'll be doing a lot of work with messages let's create a class that will help manage them for our table view. This class will be a subclass of `UITableViewSource` so that we can not only store our `Message` objects but also provide our table view with the methods it needs to render them.

Start by adding the following class to the bottom of your view controller file:

```csharp
class MessagesDataSource : UITableViewSource
{
		public List<Message> Messages { get; private set; } = new List<Message>();
}
```

We'll need the ability to add messages to the list as they come in so let's add that to `MessagesDataSource`:

```csharp
public void AddMessage (Message msg)
{
		Messages.Add (msg);
}
```

Finally, we need the `NumberOfSections`, `RowsInSection` and `GetCell` method overrides that configure our table view to display the messages. Add these to `MessagesDataSource`:

```csharp
public override nint NumberOfSections(UITableView tableView)
{
	return 1;
}

public override nint RowsInSection(UITableView tableView, nint section)
{
	return Messages.Count;
}

public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
{
    var message = Messages [indexPath.Row];

    var cell = tableView.DequeueReusableCell ("MessageCell") as MessageCell;
    cell.Message = message;
    cell.SetNeedsUpdateConstraints ();
    cell.UpdateConstraintsIfNeeded ();

    return cell;
}
```

You might have noticed that we don't have a `MessageCell` object yet. Let's create that now. Create a new class called `MessageCell` and replace its code with the following:

```csharp
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Twilio.IPMessaging;

namespace <YourNamespace>
{
    partial class MessageCell : UITableViewCell
    {
        public MessageCell (IntPtr handle) : base (handle)
        {
        }

        public Message Message { get; set; }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            authorLabel.Text = Message?.Author ?? "";
            messageLabel.Text = Message?.Body ?? "";
        }
    }
}
```

Create a custom cell in your table view inside your app's storyboard and create two labels in it. Name them `authorLabel` and `messageLabel` and set the class and identifier for the cell to `MessageCell`. The table view is ready to go. Let's populate it. Back in `ViewDidLoad` of the view controller, create a `MessagesDataSource` object and set it as the table source. While we're here we'll also provide the table view with some row dimension information:

```csharp
MessagesDataSource dataSource;

public async override void ViewDidLoad ()
{
    base.ViewDidLoad ();
    // Perform any additional setup after loading the view, typically from a nib.

    dataSource = new MsgsDataSource ();
    tableView.Source = dataSource;
    tableView.RowHeight = UITableView.AutomaticDimension;
    tableView.EstimatedRowHeight = 70;
}
```

Our table view is ready to go, now we just need to connect to Twilio IP Messaging and load it up.

## Connecting to Twilio IP Messaging

First, add some instance variables to keep track of IP Messaging related things:

```csharp
// Our chat client
TwilioIPMessagingClient twilio;
// The channel we'll chat in
Channel generalChannel;
// Our username when we connect
string identity;
```

Now let's add a method that will fetch an access token from our server:

```csharp
async Task<string> GetToken ()
{
    var deviceId = UIDevice.CurrentDevice.IdentifierForVendor.AsString ();

    var tokenEndpoint = $"https://{{your server URL and port}}/token.php?device={deviceId}";

    var http = new HttpClient ();
    var data = await http.GetStringAsync (tokenEndpoint);

    var json = JsonObject.Parse (data);
    // Set the identity for use later, this is our username
	  identity = json ["identity"]?.ToString ()?.Trim ('"');

	  return json["token"]?.ToString ()?.Trim ('"');
}
```

We pass in the device ID as a unique identifier and we're returned a token that includes our identity. Excellent, now let's go back to `ViewDidLoad` and create the IP Messaging client using the token. Add the following code to `ViewDidLoad`:

```csharp
var token = await GetToken ();
var accessManager = TwilioAccessManager.Create (token, this);
client = TwilioIPMessagingClient.Create (accessManager, this);
```

We use the returned token to create a `TwilioAccessManager` and then use that to create an IP Messaging client. We set our view controller as the delegate so we can handle the various delegate methods the `TwilioIPMessagingClient` needs to function. Let's use the client to get a list of channels and either join the `general` channel if it already exists or create it if it doesn't:

```csharp
client.GetChannelsList ((result, channels) => {
    generalChannel = channels.GetChannelWithUniqueName ("general");

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

		channels.CreateChannel(options, (creationResult, channel) => {
			if (creationResult.IsSuccessful())
			{
				generalChannel = channel;
				generalChannel.Join(r => {
					generalChannel.SetUniqueName("general", res => { });
				});
			}
		});
	}

});
```

Add some code to handle the send button press to send a new message to the general channel:

```csharp
partial void ButtonSend_TouchUpInside (UIButton sender)
{
    var msg = generalChannel.Messages.CreateMessage (messageTextField.Text);
    sendButton.Enabled = false;
    generalChannel.Messages.SendMessage(msg, r => {

        BeginInvokeOnMainThread (() => {
            messageTextField.Text = string.Empty;
            sendButton.Enabled = true;
        });

    });
}
```

When a message is sent to the channel we'll write a method that handles the `ipMessagingClient:channel:messageAdded:` delegate method:

```csharp
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

With this in place we can send and receive messages on the general channel and have a functioning chat app in iOS! Explore some of the other samples as well as the [Twiliio Docs](http://twilio.com/docs/api/ip-messaging) to find out what else you can do with your application.

## Adding Twilio IP Messaging functionality on Android

Assuming you have an application already created and the Twilio IP Messaging component is added to it, add the following `using` statements to a new Activity in your application:

```csharp
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Twilio.IPMessaging;
using Twilio.IPMessaging.Impl;
```

You'll need to set up the layout for the `Activity` to include a `ListView`, an `EditText` and a `Button`. Here's a sample you can use:

```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:orientation="vertical"
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    <ListView
        android:minWidth="25px"
        android:minHeight="40dp"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:id="@+id/listView"
        android:layout_weight="1" />
    <LinearLayout
        android:orientation="horizontal"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:id="@+id/linearLayout1">
        <EditText
            android:inputType="textMultiLine"
            android:minHeight="30dp"
            android:id="@+id/messageTextField"
            android:layout_width="wrap_content"
            android:layout_height="match_parent"
            android:layout_weight="1" />
        <Button
            android:text="SEND"
            android:layout_width="wrap_content"
            android:layout_height="match_parent"
            android:id="@+id/sendButton"
            android:gravity="center" />
    </LinearLayout>
</LinearLayout>
```

You'll also need a layout for the individual chat messages in the `ListView`. Create `MessageItemLayout.axml` in your layout resources folder and give it the following XML:

```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:orientation="vertical"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:padding="8dp">
    <TextView
        android:text="Medium Text"
        android:textAppearance="?android:attr/textAppearanceMedium"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:id="@+id/messageTextView"
        android:layout_marginLeft="8dp"
        android:layout_marginRight="8dp" />
    <TextView
        android:text="NAMEOFTHEUSER"
        android:textAppearance="?android:attr/textAppearanceSmall"
        android:layout_width="wrap_content"
        android:layout_height="match_parent"
        android:id="@+id/authorTextView"
        android:textStyle="italic"
        android:layout_marginLeft="8dp"
        android:layout_marginRight="8dp" />
</LinearLayout>
```

In your Activity's C# file add some interfaces and set up the `Activity` attribute:

```csharp
[Activity (Label = "#general", MainLauncher = true, Icon = "@mipmap/icon")]
public class MainActivity : Activity, IPMessagingClientListener, IChannelListener, ITwilioAccessManagerListener
{
  // ...
}
```
Right-click and implement the `IChannelListener` and `ITwilioAccessManagerListener` interfaces.

Next we'll add some instance variables we'll use in the application to the top of the activity:

```csharp
internal const string TAG = "TWILIO";

Button sendButton;
EditText textMessage;
ListView listView;
MessagesAdapter adapter;

ITwilioIPMessagingClient client;
IChannel generalChannel;  
```

We're going to need an `Adapter` to manage the messages we'll be displaying and creating in the chat application. This class will work with the `ListView` we created earlier to display the chat messages:

```csharp
class MessagesAdapter : BaseAdapter<IMessage>
{
    public MessagesAdapter (Activity parentActivity)
    {
        activity = parentActivity;
    }

    List<IMessage> messages = new List<IMessage> ();
    Activity activity;
}
```

We'll need a method in the `MessagesAdapter` to add messages we send from the app. Let's add that:

```csharp
public void AddMessage (IMessage msg)
{
    lock (messages) {
        messages.Add (msg);
    }

    activity.RunOnUiThread (() =>
        NotifyDataSetChanged ());
}
```

Finally, we need the adapter methods that make it possible for the `ListView` to display the messages. We'll add those now:

```csharp
public override long GetItemId (int position)
{
    return position;
}

public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
{
    var view = convertView as LinearLayout ?? activity.LayoutInflater.Inflate (Resource.Layout.MessageItemLayout, null) as LinearLayout;
    var msg = messages [position];

    // Update the text in the TextViews in the layout
    view.FindViewById<TextView> (Resource.Id.authorTextView).Text = msg.Author;
    view.FindViewById<TextView> (Resource.Id.messageTextView).Text = msg.MessageBody;

    return view;
}

public override int Count { get { return messages.Count; } }
public override IMessage this [int index] { get { return messages [index]; } }
```

Now we'll set up our user interface in `OnCreate()`:

```csharp
protected async override void OnCreate (Bundle savedInstanceState)
{
    base.OnCreate (savedInstanceState);

	  this.ActionBar.Subtitle = "logging in...";

    // Set our view from the "main" layout resource
    SetContentView (Resource.Layout.Main);

    sendButton = FindViewById<Button> (Resource.Id.sendButton);
    textMessage = FindViewById<EditText> (Resource.Id.messageTextField);
    listView = FindViewById<ListView> (Resource.Id.listView);

    adapter = new MessagesAdapter (this);
    listView.Adapter = adapter;
}
```

## Connecting to Twilio IP Messaging

Add the following code to `OnCreate` to initialize the Twilio IP Messaging SDK:

```csharp
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
```

Once initialized, the `Setup()` method will get called so let's create that method now:

```csharp
async Task Setup ()
{
  var token = await GetIdentity ();
	var accessManager = TwilioAccessManagerFactory.CreateAccessManager(token, this);
	client = TwilioIPMessagingSDK.CreateIPMessagingClientWithAccessManager(accessManager, this);

	client.Channels.LoadChannelsWithListener (new StatusListener
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
```

This method does a lot so let's break down what it does step by step.

1. A token is fetched using the `GetIdentity` (we will create this in the next step)
2. An access manager is created using the token.
3. The SDK is used to create an IP Messaging client object that we can use to interface with Twilio IP Messaging.
4. The list of channels is requested and when successful we check if there's a a channel with the unique name "general".
5. If there is, we join it. If there isn't we create it and then join it.

Let's add the methods we're missing. First we need the `GetIdentity` method:

```csharp
async Task<string> GetIdentity ()
{
    var androidId = Android.Provider.Settings.Secure.GetString (ContentResolver,
                        Android.Provider.Settings.Secure.AndroidId);

    var tokenEndpoint = $"https://{{your token server URL from above}}/token.php?device={androidId}";

    var http = new HttpClient ();
    var data = await http.GetStringAsync (tokenEndpoint);

    var json = System.Json.JsonObject.Parse (data);

    var identity = json["identity"]?.ToString ()?.Trim ('"');
	  this.ActionBar.Subtitle = $"Logged in as {identity}";
    var token = json["token"]?.ToString ()?.Trim ('"');

    return token;
}
```

Next we need the `JoinGeneralChannel` method:

```csharp
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
```

And finally the `CreateAndJoinGeneralChannel` method:

```csharp
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
			channel.SetUniqueName("general", new StatusListener {
				SuccessHandler = () => { Console.WriteLine("set unique name successfully!"); }
			});
			this.JoinGeneralChannel();
		},
		OnErrorHandler = () => { }
	});
}
```

At this point you should be able to run the app and connect to the chat instance but you won't yet be able to send any messages. Add the following code to `OnCreate` to handle clicking the send button:

```csharp
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
```

Finally, to handle incoming messages to the channel we need to add the `OnMessageAdd` function from `IChannelListener` to our `Activity`:

```csharp
public void OnMessageAdd (IMessage message)
{
    adapter.AddMessage (message);
    listView.SmoothScrollToPosition (adapter.Count - 1);
}
```

With all of this in place you've done it, you've created a chat app using Android and Twilio IP Messaging! Explore some of the other samples as well as the [Twiliio Docs](http://twilio.com/docs/api/ip-messaging) to find out what else you can do with your application.

## Other Resources

* [Twilio](http://twilio.com)
* [Twilio IP Messaging](http://twilio.com/ip-messaging)
* [Twilio IP Messaging Docs](http://twilio.com/docs/api/ip-messaging)
* Need help? Reach out to Brent Schooley on Twitter [@brentschooley](http://twitter.com/brentschooley)
