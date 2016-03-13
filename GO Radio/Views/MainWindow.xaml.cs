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
        public SoundController SoundController { get; set; }
        public bool ShowList { get; set; } = false;
        public bool SoundIsPlaying { get; set; }
        #endregion

        AutoUpdater au;

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Updates
            au = new AutoUpdater("http://www.baellon.com/goradioapp/version.txt", Assembly.GetExecutingAssembly().GetName().Version);
            au.CheckForUpdate();

            // Instanciate
            SoundController = new SoundController();

            // Binding
            DataContext = SoundController;
        }
        #endregion

        #region Window Events

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Settings
            ProgramSettings.Init();

            // SoundController
            SoundController.Load();
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Settings
            ProgramSettings.Save();

            // SoundController
            SoundController.Exit();

            // Cfg
            Cfg.Remove.Init();
        }

        #endregion

        private void ShowSettingsWindow()
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }

        // Fix scrollwheel on datagrid
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)e.Key;
            int a = 0;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int a = 0;
        }

        // Command Binding
        //public ICommand CommandViewCategories { get { return new RelayCommand(ViewCategories); } }
        //public ICommand CommandViewList { get { return new RelayCommand(ViewList); } }
        //public ICommand CommandSettings { get { return new RelayCommand(ShowSettingsWindow); } }
        //public ICommand CommandTextToSpeech { get { return new RelayCommand(TextToSpeech); } }
        //public ICommand CommandPlayPauzeSound { get { return new RelayCommand(PlayPauzeSound); } }
    }
}
