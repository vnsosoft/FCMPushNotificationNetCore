using FCMPushNotification.Services.Models.Messaging;
using FirebaseAdmin.Messaging;
using System.Threading.Tasks;

namespace FCMPushNotification.Services.Interfaces
{
    public interface IMessagingService
    {
        Task<string> SendMessageAsync(SendMessageRequest token);

        Task<string> SendMulticastMessageAsync(SendMulticastMessageRequest request);

        Task SendAllMessageSample();
    }
}
