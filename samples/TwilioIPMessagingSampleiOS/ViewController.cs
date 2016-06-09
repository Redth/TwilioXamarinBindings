using System;
using System.Linq;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Json;

using Twilio.Common;
using Twilio.IPMessaging;
using System.Collections.Generic;
using CoreGraphics;

namespace TwilioIPMessagingSampleiOS
{
	public partial class ViewController : UIViewController, ITwilioIPMessagingClientDelegate, IUITextFieldDelegate, ITwilioAccessManagerDelegate
	{
		public ViewController(IntPtr handle) : base(handle)
		{
		}

		MessagesDataSource dataSource;
		TwilioIPMessagingClient client;
		Channel generalChannel;
		string identity;

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			dataSource = new MessagesDataSource();
			tableView.Source = dataSource;
			tableView.RowHeight = UITableView.AutomaticDimension;
			tableView.EstimatedRowHeight = 70;

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

			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShow);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyboardDidShow);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillHide);

			this.View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
			{
				this.messageTextField.ResignFirstResponder();
			}));

			this.messageTextField.Delegate = this;
		}

		async Task<string> GetToken()
		{
			var deviceId = UIDevice.CurrentDevice.IdentifierForVendor.AsString();

			var tokenEndpoint = $"https://{{your token URL}}/token.php?device={deviceId}";

			var http = new HttpClient();
			var data = await http.GetStringAsync(tokenEndpoint);

			var json = JsonObject.Parse(data);
			identity = json["identity"]?.ToString()?.Trim('"');

			return json["token"]?.ToString()?.Trim('"');
		}

		[Foundation.Export("ipMessagingClient:channelHistoryLoaded:")]
		public void ChannelHistoryLoaded(TwilioIPMessagingClient client, Channel channel)
		{
			var msgs = channel.Messages.AllMessages.OrderBy(m => m.Timestamp);

			dataSource.UpdateMessages(msgs);

			tableView.ReloadData();
			this.ScrollToBottomMessage();
		}

		[Foundation.Export("ipMessagingClient:errorReceived:")]
		public void ErrorReceived(TwilioIPMessagingClient client, TwilioError error)
		{
			Console.WriteLine("Error: " + error.Description);
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

		partial void ButtonSend_TouchUpInside(UIButton sender)
		{
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
		}

		public void ScrollToBottomMessage()
		{
			if (dataSource.Messages.Count == 0)
			{
				return;
			}

			var bottomIndexPath = NSIndexPath.FromRowSection(this.tableView.NumberOfRowsInSection(0) - 1, 0);
			this.tableView.ScrollToRow(bottomIndexPath, UITableViewScrollPosition.Bottom, true);
		}

		[Foundation.Export("textFieldShouldReturn:")]
		public bool ShouldReturn(UIKit.UITextField textField)
		{
			var message = generalChannel.Messages.CreateMessage(textField.Text);
			generalChannel.Messages.SendMessage(message, (r) =>
			{
				textField.Text = "";
			});

			return true;
		}

		[Foundation.Export("accessManagerTokenExpired:")]
		public void TokenExpired(Twilio.Common.TwilioAccessManager accessManager)
		{
			Console.WriteLine("token expired");
		}

		[Foundation.Export("accessManager:error:")]
		public void Error(Twilio.Common.TwilioAccessManager accessManager, Foundation.NSError error)
		{
			Console.WriteLine("access manager error");
		}

		#region Keyboard Management
		private void KeyboardWillShow(NSNotification notification)
		{
			var keyboardHeight = ((NSValue)notification.UserInfo.ValueForKey(UIKeyboard.FrameBeginUserInfoKey)).RectangleFValue.Height;
			UIView.Animate(0.1, () =>
			{
				this.messageTextFieldBottomConstraint.Constant = keyboardHeight + 8;
				this.sendButtonBottomConstraint.Constant = keyboardHeight + 8;
				this.View.LayoutIfNeeded();
			});
		}

		private void KeyboardDidShow(NSNotification notification)
		{
			this.ScrollToBottomMessage();
		}

		private void KeyboardWillHide(NSNotification notification)
		{
			UIView.Animate(0.1, () =>
			{
				this.messageTextFieldBottomConstraint.Constant = 8;
				this.sendButtonBottomConstraint.Constant = 8;
			});
		}
		#endregion
	}

	class MessagesDataSource : UITableViewSource
	{
		public void UpdateMessages(IEnumerable<Message> messages)
		{
			Messages.Clear();
			Messages.AddRange(messages);
		}

		public void AddMessage(Message msg)
		{
			Messages.Add(msg);
		}

		public List<Message> Messages { get; private set; } = new List<Message>();

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return Messages.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var message = Messages[indexPath.Row];

			var cell = tableView.DequeueReusableCell("MessageCell") as MessageCell;
			cell.Message = message;
			cell.SetNeedsUpdateConstraints();
			cell.UpdateConstraintsIfNeeded();

			return cell;
		}
	}
}
