using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCMPushNotification.API.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
