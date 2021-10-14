using FCMPushNotification.Repository.Interfaces;
using FCMPushNotification.Services.Constants;
using FCMPushNotification.Services.Interfaces;
using FCMPushNotification.Services.Models.Messaging;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCMPushNotification.Services.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<MessagingService> _logger;

        public MessagingService(
            IAccountRepository accountRepository,
            ILogger<MessagingService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        // Send message to single device
        public async Task<string> SendMessageAsync(SendMessageRequest request)
        {
            try
            {
                var message = new Message()
                {
                    Token = request.DeviceToken,
                    Data = request.Data,
                };
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendAsync(message);

                _logger.LogTrace(FCMConstants.FCM_SENDED, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, request);
                return ex.Message;
            }
        }

        // Send a message to multiple device
        public async Task<string> SendMulticastMessageAsync(SendMulticastMessageRequest request)
        {
            try
            {
                var message = new MulticastMessage()
                {
                    Data = request.Data,
                    Tokens = request.DeviceTokens,
                };
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendMulticastAsync(message);

                _logger.LogTrace(FCMConstants.FCM_SENDED, result);
                return result.SuccessCount.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, request);
                return ex.Message;
            }
        }

        public async Task SendAllMessageSample()
        {
            var onlineAccounts = await _accountRepository.GetOnlineAccountsAsync();
            var deviceTokens = onlineAccounts.Select(x => x.DeviceToken).ToList();
            var sendMulticastMessageRequest = new SendMulticastMessageRequest()
            {
                Data = new Dictionary<string, string>
                {
                    ["title"] = "Thông báo",
                    ["body"] = $"Đây là tin nhắn tự động.",
                },
                DeviceTokens = deviceTokens,
            };

            await SendMulticastMessageAsync(sendMulticastMessageRequest);
        }
    }
}
