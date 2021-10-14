using FCMPushNotification.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FirebaseDataServices.Interfaces;
using FirebaseDataServices.Services;

namespace FCMPushNotification.API.Installers
{
    public class FirebaseDataServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connetionString = configuration.GetConnectionString("DBConnection");
            services.AddDbContext<SampleDbContext>(options =>
                options.UseSqlServer(
                    connetionString,
                    o => o.MigrationsAssembly("FCMPushNotification.Repository")));

            services.AddScoped<IStudentService, StudentService>();
        }
    }
}
