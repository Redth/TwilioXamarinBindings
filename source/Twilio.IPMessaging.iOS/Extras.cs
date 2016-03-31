using System;
using Foundation;
using ObjCRuntime;

namespace Twilio.IPMessaging
{
	public class ChannelOptions : DictionaryContainer
	{
		static string FriendlyNameKey = "TWMChannelOptionFriendlyName";
		static string UniqueNameKey = "TWMChannelOptionUniqueName";
		static string ChannelTypeKey = "TWMChannelOptionType";
		static string AttributesKey = "TWMChannelOptionAttributes";

		public ChannelOptions () : base (new NSMutableDictionary ()) {}
		public ChannelOptions (NSDictionary dictionary) : base (dictionary){}

		public string? FriendlyName {
			get { return GetStringValue (FriendlyNameKey); }
			set { SetStringValue (FriendlyNameKey, value); }
		}

		public string? UniqueName {
			get { return GetStringValue (UniqueNameKey); }
			set { SetStringValue (UniqueNameKey, value); }
		}

		public string? ChannelType {
			get { return GetNIntValue (ChannelTypeKey); }
			set { SetNumberValue (ChannelTypeKey, value); }
		}
	}
}

