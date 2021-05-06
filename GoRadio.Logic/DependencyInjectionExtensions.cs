using GoRadio.Logic.Database;
using GoRadio.Logic.Managers;
using GoRadio.Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoRadio.Logic
{
    public static class DependencyInjectionExtensions
    {
        public static ServiceCollection RegisterGoRadio(this ServiceCollection services)
        {
            // Datbase
            services.AddDbContext<DatabaseContext>();

            // Services
            services.AddTransient<SoundService>();
            services.AddTransient<AudioDeviceService>();

            // Managers
            services.AddSingleton<MicrophoneManager>();
            services.AddSingleton<SoundboardManager>();
            return services;
        }
    }
}
