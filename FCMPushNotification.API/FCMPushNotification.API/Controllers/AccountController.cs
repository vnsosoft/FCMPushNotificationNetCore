using FCMPushNotification.Services.Interfaces;
using FCMPushNotification.Services.Models.Account;
using FCMPushNotification.Services.Models.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCMPushNotification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IMessagingService _messagingService;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, IMessagingService messagingService)
        {
            _accountService = accountService;
            _messagingService = messagingService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accounts = await _accountService.GetOnlineAccountAsync();
            return Ok(accounts);
        }

        [Route("auth")]
        [HttpPost]
        public async Task<IActionResult> Authenticate(LoginRequest request)
        {
            var success = await _accountService.VerifyAccountAsync(request);
            if (success)
            {
                var onlineAccounts = await _accountService.GetOnlineAccountAsync();
                var deviceTokens = onlineAccounts.Select(x => x.DeviceToken).ToList();
                if (deviceTokens.Count < 1)
                {
                    return BadRequest("No one online");
                }
                var sendMulticastMessageRequest = new SendMulticastMessageRequest()
                {
                    Data = new Dictionary<string, string>
                    {
                        ["title"] = "Thông báo",
                        ["body"] = $"Tài khoản {request.UserName} đăng nhập.",
                    },
                    DeviceTokens = deviceTokens
                };

                await _messagingService.SendMulticastMessageAsync(sendMulticastMessageRequest);
            }
            return Ok(success);
        }

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout(string username)
        {
            await _accountService.LogoutAccountAsync(username);
            return Ok();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(AddAccountRequest request)
        {
            try
            {
                var response = await _accountService.AddAccountAsync(request);
                return Ok(response);
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
