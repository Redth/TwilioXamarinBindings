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

namespace TwilioConversationsSampleiOS
{
	[Register ("CreateConversationViewController")]
	partial class CreateConversationViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton createConversationButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField inviteeIdentityField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel inviteeLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel listeningStatusLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel selfIdentityLabel { get; set; }

		[Action ("createConversationButton_Clicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void createConversationButton_Clicked (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (createConversationButton != null) {
				createConversationButton.Dispose ();
				createConversationButton = null;
			}
			if (inviteeIdentityField != null) {
				inviteeIdentityField.Dispose ();
				inviteeIdentityField = null;
			}
			if (inviteeLabel != null) {
				inviteeLabel.Dispose ();
				inviteeLabel = null;
			}
			if (listeningStatusLabel != null) {
				listeningStatusLabel.Dispose ();
				listeningStatusLabel = null;
			}
			if (selfIdentityLabel != null) {
				selfIdentityLabel.Dispose ();
				selfIdentityLabel = null;
			}
		}
	}
}
