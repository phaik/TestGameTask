using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TestGame.Common.Interfaces;
using TestGame.HostedServices;
using TestGame.Hubs;
using TestGame.Mapper;
using TestGame.Repository;
using TestGame.Repository.Repositories;
using TestGame.Services;
using TestGame.UseCases.CreateLobby;

namespace TestGame
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestGame", Version = "v1" });
            });

            services.AddMediatR(typeof(CreateLobbyCommandHandler).Assembly);
            services.AddAutoMapper(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddDbContext<ITestGameContext, TestGameContext >(d => d.UseSqlServer(Configuration.GetConnectionString("TestGameDatabase")));
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ILobbyRepository, LobbyRepository>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddHostedService<MakeMovesHostedService>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITestGameContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestGame v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("notification");
            });

            dataContext.Migrate();
        }
    }
}
