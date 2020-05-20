using PropertyChanged;
using System.Windows.Input;
using GO_Radio.Views;
using GO_Radio.Classes.Settings;
using GO_Radio.Classes.ApplicationTypes;
using GameOverlay.Windows;
using GameOverlay.Drawing;
using System.Text;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
        private SettingsController settings;
        private Overlay _Overlay;
        public ProgramSelector ProgramSelector { get; set; }

        // Constructor
        public MainViewModel()
        {
            settings = new SettingsController();
            ProgramSelector = new ProgramSelector();
            _Overlay = new Overlay();
            _Overlay.Show();
        }

       

        // Interface Methods
        public void Load()
        {
            // Load the usersettings and pass them to the program
            ProgramSelector.Load(settings.LoadUserSettingsFromJSON());
        }
        public void Save()
        {
            // Get the latest usersettings from the program and save them as JSON.
            settings.SaveUserSettingsToJSON(ProgramSelector.Save());
        }

        // Command
        public ICommand CommandResetPlayingMonitor => new RelayCommand(ProgramSelector.ActiveProgram.SoundLoader.Reset);      // NEEDS TO BE CLEANED UP
        public ICommand CommandKeyBinding => new RelayCommand(ShowKeyBinding);
        public ICommand CommandAddCategory => new RelayCommand(ShowCategoryWindow);
        public ICommand CommandAddSound => new RelayCommand(ShowSoundWindow);
        public ICommand CommandPlayPauzeSound => new RelayCommand(SoundplayerPlayPauzeSound);
        public ICommand CommandStart => new RelayCommand(ProgramSelector.Start);
        public ICommand CommandStop => new RelayCommand(ProgramSelector.Stop);

        private void ShowCategoryWindow()
        {
            if (ProgramSelector.IsIdle())
            {
                AddCategoryWindow acw = new AddCategoryWindow(ProgramSelector.Data);
                acw.ShowDialog();
            }
        }
        private void ShowSoundWindow()
        {
            if (ProgramSelector.IsIdle())
            {
                ImportWindow iw = new ImportWindow(ProgramSelector.Data);
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
    }
}
