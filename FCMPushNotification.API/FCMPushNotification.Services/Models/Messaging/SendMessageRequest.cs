using System;
using System.Collections.Generic;

namespace FCMPushNotification.Services.Models.Messaging
{
    public class SendMessageRequest
    {
        public string DeviceToken { get; set; }

        public Dictionary<string, string> Data { get; set; }
    }
}
