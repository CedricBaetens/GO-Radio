using GO_Radio.Views;
using PropertyChanged;
using System;
using System.IO;
using System.Windows.Input;

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

        public ISoundLoader SoundLoader { get; set; }
        public IKeyboarHook Keyboard { get; set; }

        // Variables
        private readonly IOverlay _overlay;

        private string _Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GORadio");

        // Constructor
        public MainViewModel(ISoundLoader soundLoader, IOverlay overlay, IKeyboarHook keyboarHook)
        {
            SoundLoader = soundLoader;
            Keyboard = keyboarHook;
            Keyboard.IdEntered += Keyboard_IdEntered;
            Keyboard.ButtonPressed += Keyboard_ButtonPressed;
            _overlay = overlay;

            State = ApplicationState.STANDBY;
            Data = new CategoryList();
        }

        // Interface Methods
        public void Load()
        {
            Data.Load(_Path);
        }
        public void Save()
        {
            Data.Save();
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
            State = MainViewModel.ApplicationState.STANDBY;
        }
        public bool IsIdle()
        {
            return State == ApplicationState.STANDBY;
        }

        // Command
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
            if (!_overlay.Showed)
                _overlay.Show();
            else
                _overlay.Hide();
        }


        // Events
        private void Keyboard_IdEntered(object sender, IdEventArgs e)
        {
            var sound = Data.GetSoundById(e.Input);
            SoundLoader.LoadSound(sound);
        }
        private void Keyboard_ButtonPressed(object sender, ButtonEventArgs e)
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
