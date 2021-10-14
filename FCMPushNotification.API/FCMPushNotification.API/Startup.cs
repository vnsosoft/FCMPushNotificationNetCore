using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FCMPushNotification.API.Installers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using AutoMapper;
using FCMPushNotification.Services.AutoMapper;
using Hangfire;
using FCMPushNotification.API.Hubs;

namespace FCMPushNotification.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Allow any origin to access a resource
            services.AddCors();

            // Mapper service
            services.AddScoped<IMapper>(sp =>
            {
                return new Mapper(AutoMapperConfig.RegisterMappings());
            });
            services.AddSingleton(AutoMapperConfig.RegisterMappings());

            // Services installer
            services.InstallServicesInAssembly(Configuration);
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FCMPushNotification.API", Version = "v1" });
            });

            // GoogleCredential
            var googleCredentialJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "google-credential.json");
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(googleCredentialJson),
            });

            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"));
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FCMPushNotification.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
