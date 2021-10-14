using FCMPushNotification.API.Hubs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCMPushNotification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class HomeController : ControllerBase
    {
        private IHubContext<ChatHub> _hub;

        public HomeController(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("{message}")]
        public void Post(string message)
        {
            _hub.Clients.All.SendAsync("publicMessageMethodName", message);
        }

        [HttpPost("{connectionId}/{message}")]
        public void Post(string connectionId, string message)
        {
            _hub.Clients.Client(connectionId).SendAsync("privateMessageMethodName", message);
        }
    }
}
