using System;

namespace Twilio.Common
{
    public class TwilioAccessManagerListener : Java.Lang.Object, ITwilioAccessManagerListener
    {
        public Action<ITwilioAccessManager> TokenExpiryManager { get; set; }
        public void OnAccessManagerTokenExpire (ITwilioAccessManager accessManager)
        {
            TokenExpiryManager?.Invoke (accessManager);
        }

        public Action<ITwilioAccessManager, string> ErrorHandler { get; set; }
        public void OnError (ITwilioAccessManager accessManager, string msg)
        {
            ErrorHandler?.Invoke (accessManager, msg);
        }

        public Action<ITwilioAccessManager> TokenUpdatedHandler { get;set; }
        public void OnTokenUpdated (ITwilioAccessManager accessManager)
        {
            TokenUpdatedHandler?.Invoke (accessManager);
        }
    }
}

