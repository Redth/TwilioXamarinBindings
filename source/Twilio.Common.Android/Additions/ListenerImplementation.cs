using System;

namespace Twilio.Common
{
	public class TwilioAccessManagerListener : Java.Lang.Object, global::Twilio.Common.AccessManager.IListener
	{
		public Action<global::Twilio.Common.AccessManager> TokenExpiredHandler { get; set; }
		public void OnTokenExpired(global::Twilio.Common.AccessManager accessManager)
		{
			TokenExpiredHandler?.Invoke(accessManager);
		}

		public Action<global::Twilio.Common.AccessManager, string> ErrorHandler { get; set; }
		public void OnError(global::Twilio.Common.AccessManager accessManager, string msg)
		{
			ErrorHandler?.Invoke(accessManager, msg);
		}

		public Action<global::Twilio.Common.AccessManager> TokenUpdatedHandler { get; set; }
		public void OnTokenUpdated(global::Twilio.Common.AccessManager accessManager)
		{
			TokenUpdatedHandler?.Invoke(accessManager);
		}
	}
}