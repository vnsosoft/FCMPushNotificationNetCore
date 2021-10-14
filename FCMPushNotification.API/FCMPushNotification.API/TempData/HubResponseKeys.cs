using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCMPushNotification.API.TempData
{
    public class HubResponseKeys
    {
        //Responses
        public const string ERROR = "error";
        public const string SUCCESS = "success";
        public const string MESSAGE = "message";
        public const string NOTIFY = "notify";
        public const string DISCONNECT = "disconnect";
        public const string PRIVATE_MESSAGE = "private_message";

        //ROOM
        public const string MESSAGE_ROOM = "message_room";
        public const string READY = "ready";
        public const string LEAVE_ROOM = "leave_room";
        public const string JOIN_ROOM = "join_room";
        public const string CREATE_ROOM = "create_room";
        public const string UPDATE_ROOM = "update_room";
        public const string UPDATE_USERS = "update_users";
    }
}
