using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace Twilio.Common
{
    [BaseType (typeof (NSObject))]
    interface TwilioAccessManager
    {
        // @property (nonatomic, weak) id<TwilioAccessManagerDelegate> delegate;
        [NullAllowed]
        [Export ("delegate", ArgumentSemantic.Weak)]
        ITwilioAccessManagerDelegate Delegate { get; set; }

        // + (instancetype)accessManagerWithToken:(NSString *)token delegate:(id<TwilioAccessManagerDelegate>)delegate;
        [Static]
        [Export ("accessManagerWithToken:delegate:")]
        TwilioAccessManager Create (string token, ITwilioAccessManagerDelegate accessManagerDelegate);

        // - (void)updateToken:(NSString *)token;
        [Export ("updateToken:")]
        void UpdateToken (string token);

        // - (NSString *)token;
        [Export ("token")]
        string Token { get; }

        // - (NSString *)identity;
        [Export ("identity")]
        string Identity { get; }

        // - (BOOL)isExpired;
        [Export ("isExpired")]
        bool IsExpired { get; }

        // - (NSDate *)expirationDate;
        [Export ("expirationDate")]
        NSDate ExpirationDate { get; }
    }

    interface ITwilioAccessManagerDelegate { }

    [Protocol, Model]
    [BaseType (typeof (NSObject))]
    interface TwilioAccessManagerDelegate 
    {
        // - (void)accessManagerTokenExpired:(TwilioAccessManager *)accessManager;
        [Export ("accessManagerTokenExpired:")]
        void TokenExpired (TwilioAccessManager accessManager);

        // - (void)accessManager:(TwilioAccessManager *)accessManager error:(NSError *)error;
        [Export ("accessManager:error:")]
        void Error (TwilioAccessManager accessManager, NSError error);
    }
}

