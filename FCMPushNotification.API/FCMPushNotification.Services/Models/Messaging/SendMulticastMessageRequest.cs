using System.Collections.Generic;

namespace FCMPushNotification.Services.Models.Messaging
{
    public class SendMulticastMessageRequest
    {
        public Dictionary<string, string> Data { get; set; }

        public List<string> DeviceTokens { get; set; }
    }
}
