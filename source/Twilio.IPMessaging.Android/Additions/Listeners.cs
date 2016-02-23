using System;

namespace Twilio.IPMessaging
{
    public class InitListener : Java.Lang.Object, IConstantsInitListener
    {
        public Action<Java.Lang.Exception> ErrorHandler { get; set; }
        public Action InitializedHandler { get; set; }

        public void OnError (Java.Lang.Exception error)
        {
            ErrorHandler?.Invoke (error);
        }

        public void OnInitialized ()
        {
            InitializedHandler?.Invoke ();
        }
    }

    public class StatusListener : Java.Lang.Object, IConstantsStatusListener
    {
        public Action ErrorHandler { get; set; }
        public Action SuccessHandler { get; set; }

        public void OnError ()
        {
            ErrorHandler?.Invoke ();
        }

        public void OnSuccess ()
        {
            SuccessHandler?.Invoke ();
        }
    }
}

