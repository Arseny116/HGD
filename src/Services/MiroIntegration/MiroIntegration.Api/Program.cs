using MiroIntegration.Api;
using MiroIntegration.API.WebSock;
using MiroIntegration.Application;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Application.Features.ProjectService;
using MiroIntegration.Infrastructure;

namespace MiroIntegration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddApiServices();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddSecurity(builder.Configuration);

            var app = builder.Build();

            app.Services.MigrateDatabase();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<NotificationHub>("/hubs/notifications");

            app.Run();
        }
    }
}
