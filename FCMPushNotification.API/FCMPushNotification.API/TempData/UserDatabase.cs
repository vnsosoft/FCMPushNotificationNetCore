using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCMPushNotification.API.TempData
{
    public class UserDatabase
    {
        public class User
        {
            public string Username;
            public string FullName;
            public string ConnectionId;
            public bool InCall;
            public string GroupName;
            public bool isConnect;
        }

        public List<User> ListUser()
        {
            var results = new List<User>();

            results.Add(new User
            {
                Username = "tu",
                FullName = "Tú",
                isConnect = false
            });

            results.Add(new User
            {
                Username = "tam",
                FullName = "Tâm",
                isConnect = false
            });

            results.Add(new User
            {
                Username = "guest",
                FullName = "Guest",
                isConnect = false
            });

            return results;
        }
    }
}
