using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;

namespace API.Test
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = MauiApp.CreateBuilder();
            
            // Configure app settings
            builder.Configuration
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // Configure services
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            builder.Services.AddTransient<MyService>();

            builder
                .UseMauiApp<App>();

            var app = builder.Build();
            app.Run();
        }
    }
}
