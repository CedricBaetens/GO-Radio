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
using System.Timers;

namespace CSGO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundController
    {
        // Properties
        public CategoryList CategoriesList { get; set; }
        public string IdEntered { get; set; } = "";
        public SoundLoader SoundLoader { get; set; }
        public Tts TextToSpeech { get; set; }

        public bool SoundIsPlaying { get; set; } = false;

        // Varaibles
        private LowLevelKeyboardListener keyboardHook;
        private SoundPlayer soundPlayer;
        private Timer timerClearInput;
        
        // Constructor
        public SoundController()
        {
            //Sounds = new Dictionary<int, SoundNew>();
            //Categories = new ObservableCollection<Category>();
            CategoriesList = new CategoryList();

            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
            soundPlayer = new SoundPlayer();
            
            TextToSpeech = new Tts();
            TextToSpeech.OnTtsDetected += TextToSpeech_OnTtsDetected;

            timerClearInput = new Timer();
            timerClearInput.Elapsed += TimerClearInput_Elapsed;
            timerClearInput.Interval = 5000;

            SoundLoader = new SoundLoader();
        }

        private void TimerClearInput_Elapsed(object sender, ElapsedEventArgs e)
        {
            IdEntered = "";
            timerClearInput.Stop();
        }

        // Events
        private void TextToSpeech_OnTtsDetected(object sender, Tts.ProgressEventArgs e)
        {
            SoundLoader.LoadSong(e.Sound);
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

                case Key.Subtract:
                    SoundLoader.PlayPause();
                    break;

                case Key.Enter:
                    SoundLoader.PlayStop();
                    break;
            }
            #endregion

            if (IdEntered.Contains("+"))
            {
                var IdEnteredCharArr = IdEntered.Replace("+", "").ToCharArray();

                List<int> temp = new List<int>();

                foreach (var sound in CategoriesList.Sounds)
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
                    var sound = GetSoundById(id);

                    // Load Sound
                    if (sound != null)
                    {
                        SoundLoader.LoadSong(sound);
                        soundPlayer.SoundLocation = sound.IsTrimmed ? SoundLoader.Sound.PathTrim : SoundLoader.Sound.Path;                        
                        soundPlayer.Load();
                    }                   
                }

                IdEntered = "";
            }


            if (IdEntered != "")
            {
                timerClearInput.Start();
            }
        }
        
        // Public methods
        public void Load()
        {
            if (File.Exists(ProgramSettings.PathSounds + "\\data.json"))
            {
                string json = File.ReadAllText(ProgramSettings.PathSounds + "\\data.json");
                CategoriesList.Import(JsonConvert.DeserializeObject<ObservableCollection<Category>>(json));
                CategoriesList.UpdateDictionary();
            }

            TextToSpeech.Start();
            keyboardHook.HookKeyboard();
            
        }
        public void Exit()
        {
            Save();
            keyboardHook.UnHookKeyboard();
        }

        // Private methods
        private void Save()
        {
            string json = JsonConvert.SerializeObject(CategoriesList.Categories, Formatting.Indented);

            try
            {
                File.WriteAllText(ProgramSettings.PathSounds + "\\data.json", json);
            }
            catch (Exception)
            {
                MessageBox.Show("Error writing data, please make sure the sound folder exists.");
            }
        }
        private SoundNew GetSoundById(int id)
        {
            try
            {
                var sound = CategoriesList.Sounds[id];
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
        public ICommand CommandPlayPauzeSound => new RelayCommand(SoundplayerPlayPauzeSound);

        private void ShowCategoryWindow()
        {
            keyboardHook.UnHookKeyboard();

            AddCategoryWindow acw = new AddCategoryWindow(CategoriesList);
            acw.ShowDialog();
            Save();

            keyboardHook.HookKeyboard();
        }
        private void ShowSoundWindow()
        {
            keyboardHook.UnHookKeyboard();

            ImportWindow iw = new ImportWindow(CategoriesList);
            iw.ShowDialog();

            keyboardHook.HookKeyboard();
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
    }
}
