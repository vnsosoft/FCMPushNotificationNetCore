using System;

namespace FCMPushNotification.Services.Models.Account
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string DeviceToken { get; set; }
    }
}
