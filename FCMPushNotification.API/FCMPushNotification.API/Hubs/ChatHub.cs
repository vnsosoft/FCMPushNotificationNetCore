using FCMPushNotification.API.TempData;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FCMPushNotification.API.TempData.UserDatabase;

namespace FCMPushNotification.API.Hubs
{
    public class ChatHub: Hub
    {
        private static readonly UserDatabase Data = new UserDatabase();
        private static readonly List<User> Users = Data.ListUser();
        private static RoomManager RoomManager = new RoomManager();

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            RoomManager.DeleteRoom(Context.ConnectionId);
            var exist = Users.SingleOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (exist != null)
            {
                exist.isConnect = false;
                exist.ConnectionId = null;

                _ = Clients.Group(exist.GroupName).SendAsync(HubResponseKeys.NOTIFY, $"{exist.FullName} has offline");
            }

            _ = GetUsers();
            return base.OnDisconnectedAsync(exception);
        }

        private async Task GetUsers()
        {
            var list = from user in Users
                       select new
                       {
                           UID = user.ConnectionId,
                           UserName = user.Username,
                           FullName = user.FullName,
                           GroupName = user.GroupName,
                           IsConnect = user.isConnect,
                       };

            var data = JsonConvert.SerializeObject(list);
            await Clients.All.SendAsync(HubResponseKeys.UPDATE_USERS, data);
        }

        public async Task SendMessageToUser(string fromUser, string toUser, string message)
        {
           await Clients.User(toUser).SendAsync(HubResponseKeys.MESSAGE, $"{fromUser}: {message}");
        }

        public async Task SendMessageToAllConnection(string user, string message)
        {
            await Clients.All.SendAsync(HubResponseKeys.MESSAGE, user, message);
        }

        public async Task JoinToGroup(string groupName, string user)
        {
            var exist = Users.SingleOrDefault(x => x.Username == user);
            if (exist != null)
            {
                exist.isConnect = true;
                exist.ConnectionId = Context.ConnectionId;
                exist.GroupName = groupName;

                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync(HubResponseKeys.NOTIFY, $"{user} online");

                await GetUsers();
            }
            else
            {
                await Clients.Caller.SendAsync(HubResponseKeys.DISCONNECT, "User name is incorrect!");
            }
        }

        public async Task SendMessageToGroup(string groupName, string user, string message)
        {
            await Clients.Group(groupName).SendAsync(HubResponseKeys.MESSAGE, user, message);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            var exist = Users.SingleOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (exist != null)
            {
                exist.isConnect = false;
                exist.ConnectionId = null;
                exist.GroupName = null;
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync(HubResponseKeys.NOTIFY, $"{Context.ConnectionId} has left the group {groupName}.");
            await Clients.Client(Context.ConnectionId).SendAsync(HubResponseKeys.DISCONNECT);
        }
    }
}
