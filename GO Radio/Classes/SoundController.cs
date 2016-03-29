using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.IO;
using System.Media;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;
using GO_Radio.Views;
using System.Timers;
using System.Windows.Forms;
using System.Drawing;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundController
    {
        // Properties
        public CategoryList CategoriesList { get; set; }
        public SoundLoader SoundLoader { get; set; }
        public Tts TextToSpeech { get; set; }
        public ConfigSelection Config { get; set; }
        public KeyboardController Keyboard { get; set; }

        public bool SoundIsPlaying { get; set; } = false;

        // Varaibles
        private SoundPlayer soundPlayer;
        private ConsoleChecker consoleChecker;
        private ConsoleWindow consoleWindow;

        // Constructor
        public SoundController()
        {
            // Instanciate
            CategoriesList = new CategoryList();            
            TextToSpeech = new Tts();
            SoundLoader = new SoundLoader();
            Config = new ConfigSelection();
            Keyboard = new KeyboardController();

            soundPlayer = new SoundPlayer();
            consoleChecker = new ConsoleChecker();
            consoleWindow = new ConsoleWindow();


            // Events        
            Keyboard.IdEntered += Keyboard_IdEntered;
            Keyboard.ButtonPressed += Keyboard_ButtonPressed;
            consoleChecker.OnCommandDetected += ConsoleChecker_OnCommandDetected;
        }

        private void Keyboard_ButtonPressed(object sender, KeyboardController.ButtonEventArgs e)
        {
            switch(e.Key)
            {
                case KeyboardController.PressedKey.PlayStop:
                    SoundLoader.PlayStop();
                    break;
                case KeyboardController.PressedKey.PlayPauze:
                    SoundLoader.PlayPause();
                    break;
            }
        }
        private void Keyboard_IdEntered(object sender, KeyboardController.IdEventArgs e)
        {
            SoundLoader.LoadSong(CategoriesList.GetSoundById(e.Input));
        }

        
        private void ConsoleChecker_OnCommandDetected(object sender, ConsoleChecker.ProgressEventArgs e)
        {
            switch (e.Detected.Command)
            {
                case Commandos.LOAD:
                    SoundLoader.LoadSong(CategoriesList.GetSoundById(Convert.ToInt32(e.Detected.Response)));
                    break;
                case Commandos.TTS:
                    SoundLoader.LoadSong(TextToSpeech.GetSound(e.Detected.Response));
                    break;
                default:
                    break;
            }
        }

        // Public methods
        public void Load()
        {
            if (File.Exists(ProgramSettings.Instance.PathSounds + "\\data.json"))
            {
                string json = File.ReadAllText(ProgramSettings.Instance.PathSounds + "\\data.json");
                CategoriesList.Import(JsonConvert.DeserializeObject<ObservableCollection<Category>>(json));
                CategoriesList.UpdateDictionary();
            }

            //TextToSpeech.Start();
            SoundLoader.LoadSong(CategoriesList.GetSoundById(0));

            Cfg.Create.Init(Keyboard.KeyBindings);
            Cfg.Create.CategoryList(CategoriesList);

        }
        public void Exit()
        {
            Save();
            Keyboard.UnHook();
        }

        // Private methods
        private void Save()
        {
            string json = JsonConvert.SerializeObject(CategoriesList.Categories, Formatting.Indented);

            try
            {
                File.WriteAllText(ProgramSettings.Instance.PathSounds + "\\data.json", json);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Error writing data, please make sure the sound folder exists.");
            }
        }
        private void Start()
        {
            //Config.ActiveConfiguration.
        }

        // Command
        public ICommand CommandResetPlayingMonitor => new RelayCommand(SoundLoader.Reset);
        public ICommand CommandKeyBinding => new RelayCommand(ShowKeyBinding);
        public ICommand CommandAddCategory => new RelayCommand(ShowCategoryWindow);
        public ICommand CommandAddSound => new RelayCommand(ShowSoundWindow);
        public ICommand CommandPlayPauzeSound => new RelayCommand(SoundplayerPlayPauzeSound);
        public ICommand CommandStart => new RelayCommand(Start);

        private void ShowCategoryWindow()
        {
            Keyboard.UnHook();

            AddCategoryWindow acw = new AddCategoryWindow(CategoriesList);
            acw.ShowDialog();
            Save();

            Keyboard.Hook();
        }
        private void ShowSoundWindow()
        {
            Keyboard.UnHook();

            ImportWindow iw = new ImportWindow(CategoriesList);
            iw.ShowDialog();

            Keyboard.Hook();
            CategoriesList.UpdateDictionary();
            Save();
        }
        private void SoundplayerPlayPauzeSound()
        {
            if (soundPlayer.IsLoadCompleted)
            {
                if (SoundIsPlaying)
                {
                    soundPlayer.Stop();
                    SoundIsPlaying = false;
                }
                else
                {
                    soundPlayer.Play();
                    SoundIsPlaying = true;
                }
            }
        }
        private void ShowKeyBinding()
        {
            Keyboard.UnHook();

            KeyBindingWindow kbw = new KeyBindingWindow(Keyboard.KeyBindings);
            kbw.ShowDialog();

            Cfg.Create.Init(Keyboard.KeyBindings);

            Keyboard.Hook();
        }
    }
}
