using PropertyChanged;
using System.Windows.Input;
using GO_Radio.Views;
using GO_Radio.Classes.Settings;
using GameOverlay.Windows;
using GameOverlay.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using NAudio.Wave;
using System.Linq;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
        public enum ApplicationState
        {
            STANDBY,
            RUNNING
        };

        // Properties
        public ApplicationState State { get; set; }
        public CategoryList Data { get; set; }

        public List<string> OutputDevices { get; set; }
        public string SelectedOutputDevice { get; set; }

        public SoundLoader SoundLoader { get; set; } = new SoundLoader();
        public KeyboardHook Keyboard { get; set; } = new KeyboardHook();

        // Variables
        private UserSettings _userSettings;
        private Overlay _Overlay;

        // Constructor
        public MainViewModel()
        {
            Keyboard = new KeyboardHook();
            Keyboard.IdEntered += Keyboard_IdEntered;
            Keyboard.ButtonPressed += Keyboard_ButtonPressed;

            _Overlay = new Overlay();

            State = ApplicationState.STANDBY;
            Data = new CategoryList();

            OutputDevices = new List<string>();
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var output = WaveOut.GetCapabilities(i);
                OutputDevices.Add(output.ProductName);
            }
            SelectedOutputDevice = OutputDevices.FirstOrDefault();
        }

        // Interface Methods
        public void Load()
        {
            _userSettings = SettingsController.LoadUserSettingsFromJSON();


            // Load sound data
            if (!Directory.Exists(_userSettings.SoundPath))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog()
                {
                    Description = @"Please select a location where you want your sounds to be stored."
                };
                fbd.ShowDialog();

                if (!string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    _userSettings.SoundPath = fbd.SelectedPath + "\\Sounds";
                    Directory.CreateDirectory(_userSettings.SoundPath + "\\audio");
                    Directory.CreateDirectory(_userSettings.SoundPath + "\\new");
                }
            }

            Data.Load(_userSettings.SoundPath);
            AudioHelper.Load(_userSettings.SoundPath);
        }
        public void Save()
        {
            // Get the latest usersettings from the program and save them as JSON.
            // Save sounddata
            Data.Save();

            // Return usersettings
            //_userSettings.SkypeSettings = Programs[1].Setting;
            _userSettings.SoundPath = Data.Path;
            SettingsController.SaveUserSettingsToJSON(_userSettings);
        }

        // Public Methods
        public void Start()
        {
            Keyboard.Hook();
            State = SoundLoader.Start() ? ApplicationState.RUNNING : ApplicationState.STANDBY;
        }
        public void Stop()
        {
            SoundLoader.Stop();
            Keyboard.UnHook();

        }
        public bool IsIdle()
        {
            return State == ApplicationState.STANDBY;
        }

        // Command
        public ICommand CommandResetPlayingMonitor => new RelayCommand(SoundLoader.Reset);     
        public ICommand CommandKeyBinding => new RelayCommand(ShowKeyBinding);
        public ICommand CommandAddCategory => new RelayCommand(ShowCategoryWindow);
        public ICommand CommandAddSound => new RelayCommand(ShowSoundWindow);
        public ICommand CommandPlayPauzeSound => new RelayCommand(SoundplayerPlayPauzeSound);
        public ICommand CommandStart => new RelayCommand(Start);
        public ICommand CommandStop => new RelayCommand(Stop);
        public ICommand CommandShowOverlay => new RelayCommand(ShowOverlay);


        private void ShowCategoryWindow()
        {
            if (IsIdle())
            {
                AddCategoryWindow acw = new AddCategoryWindow(Data);
                acw.ShowDialog();
            }
        }
        private void ShowSoundWindow()
        {
            if (IsIdle())
            {
                ImportWindow iw = new ImportWindow(Data);
                iw.ShowDialog();
            }
        }
        private void SoundplayerPlayPauzeSound()
        {
            //if (soundPlayer.IsLoadCompleted)
            //{
            //    if (SoundIsPlaying)
            //    {
            //        soundPlayer.Stop();
            //        SoundIsPlaying = false;
            //    }
            //    else
            //    {
            //        soundPlayer.Play();
            //        SoundIsPlaying = true;
            //    }
            //}
        }
        private void ShowKeyBinding()
        {
            //if (ApplicationSelection.IsIdle())
            //{
            //    KeyBindingWindow kbw = new KeyBindingWindow(Keyboard.KeyBindings);
            //    kbw.ShowDialog();

            //    Cfg.Create.Init(Keyboard.KeyBindings);

            //}
        }
        private void ShowOverlay()
        {
            if (!_Overlay.Showed)
                _Overlay.Show();
            else
                _Overlay.Hide();
        }


        // Events
        private void Keyboard_IdEntered(object sender, KeyboardHook.IdEventArgs e)
        {
            var sound = Data.GetSoundById(e.Input);

            SoundLoader.LoadSound(sound);
            _Overlay.DisplaySound(sound);
        }
        private void Keyboard_ButtonPressed(object sender, KeyboardHook.ButtonEventArgs e)
        {
            switch (e.Key)
            {
                case KeyboardHook.PressedKey.PlayPauze:
                    SoundLoader.PlayPause();
                    break;
                case KeyboardHook.PressedKey.PlayStop:
                    SoundLoader.PlayStop();
                    break;
            }
        }
    }
}
