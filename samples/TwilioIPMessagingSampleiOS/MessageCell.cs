using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Twilio.IPMessaging;

namespace TwilioIPMessagingSampleiOS
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

            textAuthor.Text = Message?.Author ?? "";
            textMessage.Text = Message?.Body ?? "";
        }
    }
}
