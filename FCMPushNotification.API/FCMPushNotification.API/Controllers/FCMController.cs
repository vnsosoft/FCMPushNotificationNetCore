using FCMPushNotification.Services.Interfaces;
using FCMPushNotification.Services.Models.Messaging;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FCMPushNotification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FCMController : ControllerBase
    {
        private readonly IMessagingService _messagingService;
        private readonly IAccountService _accountService;

        public FCMController(IMessagingService messagingService,
            IAccountService accountService)
        {
            _messagingService = messagingService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Send(int seconds)
        {
            try
            {
                var onlineAccounts = await _accountService.GetOnlineAccountAsync();
                var deviceTokens = onlineAccounts.Select(x => x.DeviceToken).ToList();
                var sendMulticastMessageRequest = new SendMulticastMessageRequest()
                {
                    Data = new Dictionary<string, string>
                    {
                        ["title"] = "Thông báo",
                        ["body"] = $"Send mutilple device.",
                    },
                    DeviceTokens = deviceTokens,
                };

                BackgroundJob.Schedule(() =>
                    _messagingService.SendMulticastMessageAsync(sendMulticastMessageRequest), TimeSpan.FromSeconds(seconds)
                );

                return Ok("Task scheduled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("single/send-message")]
        [HttpPost]
        public async Task<IActionResult> SendMessageRequest(string toDeviceToken)
        {
            var sampleSendMessageRequest = new SendMessageRequest()
            {
                DeviceToken = toDeviceToken,
                Data = new Dictionary<string, string>()
                {
                    ["title"] = "API notification",
                    ["body"] = "This is message from API"
                }
            };

            var response = await _messagingService.SendMessageAsync(sampleSendMessageRequest);
            return Ok(response);
        }

        [Route("send-message")]
        [HttpPost]
        public async Task<IActionResult> SendMessageRequest(SendMessageRequest request)
        {
            var response = await _messagingService.SendMessageAsync(request);
            return Ok(response);
        }

        [Route("send-multiple-message")]
        [HttpPost]
        public async Task<IActionResult> SendMulticastMessageRequest(SendMulticastMessageRequest request)
        {
            var response = await _messagingService.SendMulticastMessageAsync(request);
            return Ok(response);
        }
    }
}
