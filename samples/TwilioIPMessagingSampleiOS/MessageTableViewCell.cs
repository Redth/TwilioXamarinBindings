using System;

using Foundation;
using UIKit;
using Twilio.IPMessaging;

namespace TwilioIPMessagingSampleiOS
{
    public partial class MessageTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString ("MessageTableViewCell");
        public static readonly UINib Nib;

        static MessageTableViewCell ()
        {
            Nib = UINib.FromName ("MessageTableViewCell", NSBundle.MainBundle);
        }

        public MessageTableViewCell (IntPtr handle) : base (handle)
        {
        }

        public static MessageTableViewCell Create ()
        {
            return (MessageTableViewCell)Nib.Instantiate (null, null) [0];
        }

        public Message Message { get; set; }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            textMessage.Text = Message?.Body ?? "";
            labelAuthor.Text = Message?.Author ?? "";
        }
    }
}
