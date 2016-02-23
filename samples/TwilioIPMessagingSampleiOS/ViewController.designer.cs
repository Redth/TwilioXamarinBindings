// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TwilioIPMessagingSampleiOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton buttonSend { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView tableView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textMessage { get; set; }

		[Action ("ButtonSend_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonSend_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (buttonSend != null) {
				buttonSend.Dispose ();
				buttonSend = null;
			}
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
			if (textMessage != null) {
				textMessage.Dispose ();
				textMessage = null;
			}
		}
	}
}
