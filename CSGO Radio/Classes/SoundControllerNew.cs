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
using CSGO_Radio.Views;

namespace CSGO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundControllerNew
    {
        // Properties
        public ObservableCollection<Category> Categories { get; set; }
        public Dictionary<int, SoundNew> Sounds { get; set; }   // Used for easy finding of songs
        public string IdEntered { get; set; } = "";
        public KeyValuePair<int, SoundNew> SelectedSound { get; set; }

        // Varaibles
        private LowLevelKeyboardListener keyboardHook;
        private SoundPlayer soundPlayer;
        private bool soundIsPlaying = false;

        // Constructor
        public SoundControllerNew()
        {
            Sounds = new Dictionary<int, SoundNew>();
            Categories = new ObservableCollection<Category>();

            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
            soundPlayer = new SoundPlayer();
        }

        // Public methods
        public void Load()
        {
            if (File.Exists(ProgramSettings.PathSounds + "\\data.json"))
            {
                string json = File.ReadAllText(ProgramSettings.PathSounds + "\\data.json");
                Categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
                UpdateDictionary();
            }

            keyboardHook.HookKeyboard();
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(Categories, Formatting.Indented);

            try
            {
                File.WriteAllText(ProgramSettings.PathSounds + "\\data.json", json);
            }
            catch (Exception)
            {
                MessageBox.Show("Error writing data, please make sure the sound folder exists.");
            }
            keyboardHook.UnHookKeyboard();
        }

        // Private methods
        private void LoadSong(int id)
        {
            try
            {
                var sound = Sounds[id];
                if (sound != null)
                {
                    if (File.Exists(ProgramSettings.PathCsgo + "\\voice_input.wav"))
                    {
                        File.Delete(ProgramSettings.PathCsgo + "\\voice_input.wav");
                    }
                    File.Copy(sound.Path, ProgramSettings.PathCsgo + "\\voice_input.wav");

                    IdEntered = "";
                    SelectedSound = new KeyValuePair<int, SoundNew>(id,sound);

                    // Load for player
                    soundPlayer.SoundLocation = sound.Path;
                    soundPlayer.Load();
                }

                //if (IdEntered.Count() >= 4)
                //    IdEntered = "";
            }
            catch
            {
                // ignored
            }
        }
        private void UpdateDictionary()
        {
            Dictionary<int, SoundNew> dic = new Dictionary<int, SoundNew>();
            foreach (var cat in Categories)
            {
                dic = Sounds.Concat(cat.Sounds).ToDictionary(x => x.Key, x => x.Value);
            }
            Sounds = dic;
        }
        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            #region keys
            switch (e.KeyPressed)
            {
                case Key.NumPad0:
                    IdEntered += "0";
                    break;

                case Key.NumPad1:
                    IdEntered += "1";
                    break;

                case Key.NumPad2:
                    IdEntered += "2";
                    break;

                case Key.NumPad3:
                    IdEntered += "3";
                    break;

                case Key.NumPad4:
                    IdEntered += "4";
                    break;

                case Key.NumPad5:
                    IdEntered += "5";
                    break;

                case Key.NumPad6:
                    IdEntered += "6";
                    break;

                case Key.NumPad7:
                    IdEntered += "7";
                    break;

                case Key.NumPad8:
                    IdEntered += "8";
                    break;

                case Key.NumPad9:
                    IdEntered += "9";
                    break;

                case Key.Delete:
                    IdEntered = "";
                    break;
                    //case Key.Add:
                    //    SoundController.IdEntered += "+";
                    //    break;
            }
            #endregion

            if (IdEntered.Length >= 4)
            {
                // Check if id is valid
                if (!string.IsNullOrEmpty(IdEntered))
                {
                    int id = Convert.ToInt32(IdEntered);
                    LoadSong(id);
                }
            }
        }

        // Command
        public ICommand CommandAddCategory => new RelayCommand(ShowCategoryWindow);
        public ICommand CommandAddSound => new RelayCommand(ShowSoundWindow);
        public ICommand CommandPlayPauzeSound => new RelayCommand(PlayPauzeSound);

        private void ShowCategoryWindow()
        {
            AddCategoryWindow acw = new AddCategoryWindow(Categories);
            acw.ShowDialog();
        }
        private void ShowSoundWindow()
        {
            ImportWindow iw = new ImportWindow(Categories);
            iw.ShowDialog();
        }
        private void PlayPauzeSound()
        {
            if (soundPlayer.IsLoadCompleted)
            {
                if (soundIsPlaying)
                {
                    soundPlayer.Stop();
                    soundIsPlaying = false;
                }
                else
                {
                    soundPlayer.Play();
                    soundIsPlaying = true;
                }
            }
        }
    }
}
