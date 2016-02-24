using System;
using Android.Content;
using Android.Support.V7.App;
using Android.Widget;

namespace TwilioConversationsSampleAndroid
{
    public class Dialog
    {
        public static AlertDialog CreateInviteDialog (string caller, EventHandler<DialogClickEventArgs> accept, EventHandler<DialogClickEventArgs> reject, Context context)
        {
            AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder (context);

            alertDialogBuilder.SetIcon (Resource.Drawable.ic_call_black_24dp);
            alertDialogBuilder.SetTitle ("Incoming Call");
            alertDialogBuilder.SetMessage (caller + " is calling");
            alertDialogBuilder.SetPositiveButton ("Accept", accept);
            alertDialogBuilder.SetNegativeButton ("Reject", reject);
            alertDialogBuilder.SetCancelable (false);

            return alertDialogBuilder.Create ();
        }

        public static AlertDialog CreateCallParticipantsDialog (EditText participantEditText, EventHandler<DialogClickEventArgs> callParticipantsClickListener, EventHandler<DialogClickEventArgs> cancelClickListener, Context context)
        {
            AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder (context);

            alertDialogBuilder.SetIcon (Resource.Drawable.ic_call_black_24dp);
            alertDialogBuilder.SetTitle ("Call Participant");
            alertDialogBuilder.SetPositiveButton ("Call", callParticipantsClickListener);
            alertDialogBuilder.SetNegativeButton ("Cancel", cancelClickListener);
            alertDialogBuilder.SetCancelable (false);

            setParticipantFieldInDialog (participantEditText, alertDialogBuilder, context);

            return alertDialogBuilder.Create ();
        }

        static void setParticipantFieldInDialog (EditText participantEditText, AlertDialog.Builder alertDialogBuilder, Context context)
        {
            // Add a participant field to the dialog
            participantEditText.Hint =  "participant name";
            int horizontalPadding = context.Resources.GetDimensionPixelOffset (Resource.Dimension.activity_horizontal_margin);
            int verticalPadding = context.Resources.GetDimensionPixelOffset (Resource.Dimension.activity_vertical_margin);
            //alertDialogBuilder.SetView (participantEditText, horizontalPadding, verticalPadding, horizontalPadding, 0);
            alertDialogBuilder.SetView (participantEditText);
        }
    }
}

