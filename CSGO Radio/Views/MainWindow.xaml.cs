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
using CSGO_Radio.Views;
using Newtonsoft.Json;
using PropertyChanged;
using CSGO_Radio.Classes;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Media;
using System.Reflection;
using System.Threading;
using AutoUpdate;
using NAudio.Wave;
using System.Deployment.Application;

namespace CSGO_Radio
{
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {
        #region Properties
        public string AppName { get; set; }
        public SoundController SoundController { get; set; }

        public bool ShowList { get; set; } = false;
        public bool SoundIsPlaying { get; set; }
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

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

            // Cfg
            Cfg.Create.Init();
            //Cfg.CreateSongList(SoundController.SoundsList);

            // Add application name and version
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                AppName = string.Format("CS: GO Radio - v{0}", ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
            }
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Settings
            ProgramSettings.Save();

            // SoundController
            SoundController.Save();

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

        // Command Binding
        //public ICommand CommandViewCategories { get { return new RelayCommand(ViewCategories); } }
        //public ICommand CommandViewList { get { return new RelayCommand(ViewList); } }
        //public ICommand CommandSettings { get { return new RelayCommand(ShowSettingsWindow); } }
        //public ICommand CommandTextToSpeech { get { return new RelayCommand(TextToSpeech); } }
        //public ICommand CommandPlayPauzeSound { get { return new RelayCommand(PlayPauzeSound); } }
    }
}
