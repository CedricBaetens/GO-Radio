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

        #region Variables
        private LowLevelKeyboardListener keyboardHook;
        //private SoundPlayer soundPlayer;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Hook
            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;

            // Instanciate
            SoundController = new SoundController();
            //soundPlayer = new SoundPlayer();

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

            // Install Hook
            keyboardHook.HookKeyboard();

            // Cfg
            Cfg.Create.Init();
            Cfg.CreateSongList(SoundController.SoundsList);

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

            // Deinstal hook
            keyboardHook.UnHookKeyboard();

            // Cfg
            Cfg.Remove.Init();
        }

        #endregion

        #region Keyboard Hook
        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            #region keys
            switch (e.KeyPressed)
            {
                case Key.NumPad0:
                    SoundController.IdEntered += "0";
                    break;

                case Key.NumPad1:
                    SoundController.IdEntered += "1";
                    break;

                case Key.NumPad2:
                    SoundController.IdEntered += "2";
                    break;

                case Key.NumPad3:
                    SoundController.IdEntered += "3";
                    break;

                case Key.NumPad4:
                    SoundController.IdEntered += "4";
                    break;

                case Key.NumPad5:
                    SoundController.IdEntered += "5";
                    break;

                case Key.NumPad6:
                    SoundController.IdEntered += "6";
                    break;

                case Key.NumPad7:
                    SoundController.IdEntered += "7";
                    break;

                case Key.NumPad8:
                    SoundController.IdEntered += "8";
                    break;

                case Key.NumPad9:
                    SoundController.IdEntered += "9";
                    break;

                case Key.Delete:
                    SoundController.IdEntered = "";
                    break;

                case Key.Enter:
                    SoundController.LoadedSound.PlayCount++;
                    break;

                    //case Key.Add:
                    //    SoundController.IdEntered += "+";
                    //    break;
            }
            #endregion

            SoundController.LoadSong();
        }
        #endregion

        private void ShowSettingsWindow()
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }
        private void TextToSpeech()
        {
            //var dialog = new TextToSpeechWindow();
            //dialog.ShowDialog();

            //if (!dialog.Canceled)
            //{
            //    StringToTTS(dialog.SpeechText);
            //}
        }
        //private void PlayPauzeSound()
        //{
        //    if (soundPlayer.IsLoadCompleted)
        //    {
        //        if (SoundIsPlaying)
        //        {
        //            soundPlayer.Stop();
        //            SoundIsPlaying = false;
        //        }
        //        else
        //        {
        //            soundPlayer.Play();
        //            SoundIsPlaying = true;
        //        }
        //    }
        //}

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
