using FCMPushNotification.Services.Interfaces;
using FCMPushNotification.Services.Services;
using FCMPushNotification.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FCMPushNotification.Repository.Interfaces;
using FCMPushNotification.Repository.Repository;

namespace FCMPushNotification.API.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connetionString = configuration.GetConnectionString("DBConnection");
            services.AddDbContext<SampleDbContext>(options =>
                options.UseSqlServer(
                    connetionString,
                    o => o.MigrationsAssembly("FCMPushNotification.Repository")));

            services.AddScoped<IMessagingService, MessagingService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
