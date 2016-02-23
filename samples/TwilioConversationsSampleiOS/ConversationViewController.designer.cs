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
	[Register ("ConversationViewController")]
	partial class ConversationViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton flipCameraButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton hangupButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView localVideoContainer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint localVideoHeightConstraint { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint localVideoWidthConstraint { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton muteButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton pauseButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView remoteVideoContainer { get; set; }

		[Action ("flipButton_Clicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void flipButton_Clicked (UIButton sender);

		[Action ("hangupButton_Clicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void hangupButton_Clicked (UIButton sender);

		[Action ("muteButton_Clicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void muteButton_Clicked (UIButton sender);

		[Action ("pauseButton_Clicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void pauseButton_Clicked (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (flipCameraButton != null) {
				flipCameraButton.Dispose ();
				flipCameraButton = null;
			}
			if (hangupButton != null) {
				hangupButton.Dispose ();
				hangupButton = null;
			}
			if (localVideoContainer != null) {
				localVideoContainer.Dispose ();
				localVideoContainer = null;
			}
			if (localVideoHeightConstraint != null) {
				localVideoHeightConstraint.Dispose ();
				localVideoHeightConstraint = null;
			}
			if (localVideoWidthConstraint != null) {
				localVideoWidthConstraint.Dispose ();
				localVideoWidthConstraint = null;
			}
			if (muteButton != null) {
				muteButton.Dispose ();
				muteButton = null;
			}
			if (pauseButton != null) {
				pauseButton.Dispose ();
				pauseButton = null;
			}
			if (remoteVideoContainer != null) {
				remoteVideoContainer.Dispose ();
				remoteVideoContainer = null;
			}
		}
	}
}
