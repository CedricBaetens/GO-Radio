using GoRadio.Logic.Database;
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
            services.AddSingleton<MicrophoneService>();
            services.AddSingleton<SoundService>();
            return services;
        }
    }
}
