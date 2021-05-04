using GO_Radio.Classes;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GO_Radio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            _Container = new StandardKernel();

            _Container.Bind<MainViewModel>().To<MainViewModel>();

            _Container.Bind<IOverlay>().To<Overlay>().InSingletonScope();
            _Container.Bind<ISoundLoader>().To<SoundLoader>().InSingletonScope();
            _Container.Bind<IKeyboarHook>().To<KeyboardHook>().InSingletonScope();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = _Container.Get<MainWindow>();
        }
    }
}
