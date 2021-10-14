using System;
using System.ComponentModel.DataAnnotations;

namespace FCMPushNotification.Entities
{
    public class Account
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        public string DeviceToken { get; set; }

        public bool IsOnline { get; set; }
    }
}
