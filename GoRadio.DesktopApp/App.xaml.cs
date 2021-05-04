using GoRadio.DesktopApp.ViewModel;
using GoRadio.Logic;
using GoRadio.Logic.Database;
using GoRadio.Logic.Database.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GoRadio.DesktopApp
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            _serviceProvider = new ServiceCollection()
                .RegisterGoRadio()
                .AddSingleton<MainViewModel>()
                .AddSingleton<MainWindow>()
                .BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            // Create database
            var context = _serviceProvider.GetRequiredService<DatabaseContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed database
            context.Sounds.Add(new Sound() { Name = "Sound1" });
            context.Sounds.Add(new Sound() { Name = "Sound2" });
            context.SaveChanges();

            // Start window
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
