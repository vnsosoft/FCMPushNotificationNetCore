using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMPushNotification.Services.Models.Account
{
    public class AccountResponse
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string DeviceToken { get; set; }

        public bool IsOnline { get; set; }
    }
}
