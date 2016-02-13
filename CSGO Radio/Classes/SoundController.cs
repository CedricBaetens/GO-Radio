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
    public class SoundController
    {
        // Properties
        public ObservableCollection<Category> Categories { get; set; }
        public Dictionary<int, SoundNew> Sounds { get; set; }   // Used for easy finding of songs
        public string IdEntered { get; set; } = "";
        public SoundNew SelectedSound { get; set; }
        public Tts TextToSpeech { get; set; }
        public bool SoundIsPlaying { get; set; } = false;

        // Varaibles
        private LowLevelKeyboardListener keyboardHook;
        private SoundPlayer soundPlayer;

        // Constructor
        public SoundController()
        {
            Sounds = new Dictionary<int, SoundNew>();
            Categories = new ObservableCollection<Category>();

            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
            soundPlayer = new SoundPlayer();
            TextToSpeech = new Tts();
            TextToSpeech.OnTtsDetected += TextToSpeech_OnTtsDetected;
        }

        // Events
        private void TextToSpeech_OnTtsDetected(object sender, Tts.ProgressEventArgs e)
        {
            LoadSong(e.Sound);
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

                case Key.Add:
                    IdEntered += "+";
                    break;
            }
            #endregion

            if (IdEntered.Contains("+"))
            {
                var IdEnteredCharArr = IdEntered.Replace("+", "").ToCharArray();

                List<int> temp = new List<int>();

                foreach (var sound in Sounds)
                {
                    var soundCharArray = sound.Key.ToString("0000").ToCharArray();

                    bool keep = true;
                    for (int i = 0; i < IdEnteredCharArr.Length; i++)
                    {
                        if (soundCharArray[i] != IdEnteredCharArr[i])
                        {
                            keep = false;
                        }
                    }

                    if (keep)
                    {
                        temp.Add(sound.Key);
                    }
                }

                if (temp.Count > 0)
                {
                    Random rnd = new Random();
                    var random = rnd.Next(temp.Count);

                    IdEntered = temp[random].ToString("0000");
                }
                else
                {
                    IdEntered = "";
                }

            }
            if (IdEntered.Length >= 4)
            {
                // Check if id is valid
                if (!string.IsNullOrEmpty(IdEntered))
                {
                    int id = Convert.ToInt32(IdEntered);

                    // Get sound
                    LoadSong(GetSoundById(id));
                }

                IdEntered = "";
            }
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
        private void LoadSong(SoundNew sound)
        {
            try
            {
                if (sound != null)
                {
                    if (File.Exists(ProgramSettings.PathCsgo + "\\voice_input.wav"))
                    {
                        File.Delete(ProgramSettings.PathCsgo + "\\voice_input.wav");
                    }

                    if (!string.IsNullOrEmpty(sound.PathTrim))
                    {
                        File.Copy(sound.PathTrim, ProgramSettings.PathCsgo + "\\voice_input.wav");
                    }
                    else
                    {
                        File.Copy(sound.Path, ProgramSettings.PathCsgo + "\\voice_input.wav");
                    }
                    

                    IdEntered = "";
                    SelectedSound = sound;

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
                foreach (var sound in cat.Sounds)
                {
                    dic.Add(sound.Id, sound);
                }               
            }
            Sounds = dic;
        }     
        private SoundNew GetSoundById(int id)
        {
            try
            {
                var sound = Sounds[id];
                return sound;
            }
            catch (Exception)
            {
                return null;
            }          
        }

        // Command
        public ICommand CommandAddCategory => new RelayCommand(ShowCategoryWindow);
        public ICommand CommandAddSound => new RelayCommand(ShowSoundWindow);
        public ICommand CommandPlayPauzeSound => new RelayCommand(PlayPauzeSound);


        private void ShowTrimSound()
        {
            
        }
        private void ShowCategoryWindow()
        {
            AddCategoryWindow acw = new AddCategoryWindow(Categories);
            acw.ShowDialog();
        }
        private void ShowSoundWindow()
        {
            ImportWindow iw = new ImportWindow(Categories);
            iw.ShowDialog();
            UpdateDictionary();
        }

        private void PlayPauzeSound()
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
    }
}
