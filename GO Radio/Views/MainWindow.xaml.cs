using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GO_Radio.Views;
using Newtonsoft.Json;
using PropertyChanged;
using GO_Radio.Classes;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Media;
using System.Reflection;
using System.Threading;
using NAudio.Wave;
using System.Deployment.Application;

namespace GO_Radio
{
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {
        #region Properties
        public MainController MainController { get; set; }
        #endregion

        AutoUpdater au;

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Updates
            au = new AutoUpdater("http://www.baellon.com/goradioapp/version.txt", Assembly.GetExecutingAssembly().GetName().Version);
            au.CheckForUpdate();
        }
        #endregion

        #region Window Events

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Settings
            ProgramSettings.Instance.Load();

            // Instanciate
            MainController = new MainController();
            MainController.Load();

            // Binding
            DataContext = MainController;
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Settings
            ProgramSettings.Instance.Save();

            // SoundController
            MainController.Exit();

            // Cfg
            Cfg.Remove.Init();
        }

        #endregion

        // Fix scrollwheel on datagrid
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
        }
    }
}
