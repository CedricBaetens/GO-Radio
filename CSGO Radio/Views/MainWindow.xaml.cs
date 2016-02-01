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
using HLDJ_Advanced.Views;
using Newtonsoft.Json;
using PropertyChanged;
using HLDJ_Advanced.Classes;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Media;
using System.Reflection;
using System.Threading;
using AutoUpdate;
using NAudio.Wave;

namespace HLDJ_Advanced
{
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {
        // Public
        public Data Data { get; set; }    
        public string IdEntered { get; set; }
        public SoundWAV LoadedSound { get; set; }
        public bool ShowList { get; set; }
        public ObservableCollection<KeyValuePair<string, SoundWAV>> SoundsList { get; set; }
        public bool SoundIsPlaying { get; set; }

        // Private
        private LowLevelKeyboardListener keyboardHook;
        private SoundPlayer soundPlayer;
        System.Timers.Timer ttsTimer = new System.Timers.Timer();
        AUpdate Updater;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Instance
            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;

            Data = new Data();
            LoadedSound = new SoundWAV();
            SoundsList = new ObservableCollection<KeyValuePair<string, SoundWAV>>();

            Updater = new AUpdate(@"http://baellon.com/csgoradio", Assembly.GetEntryAssembly().GetName().Version);
            Updater.CheckVersion();

            IdEntered = "";
            ShowList = false;

            soundPlayer = new SoundPlayer();

            ttsTimer.Elapsed += TtsTimer_Elapsed;
            ttsTimer.Interval = 100;
            ttsTimer.Enabled = true;

            // Binding
            DataContext = this;

            


        }

        private void TtsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var text = Cfg.GetTtsText();
            if (!string.IsNullOrEmpty(text))
            {
                StringToTTS(text);
            }
        }

        // Hook
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

                case Key.Enter:
                    LoadedSound.PlayCount++;
                    break;

                //case Key.Add:
                //    IdEntered += "+";
                //    break;
            }
            #endregion

            LoadSound(FindSoundById(IdEntered));
        }

        // Window events
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ProgramSettings.Load();

            // Load Data
            if (File.Exists(ProgramSettings.PathSounds + "\\data.json"))
            {
                string json = File.ReadAllText(ProgramSettings.PathSounds + "\\data.json");
                Data = JsonConvert.DeserializeObject<Data>(json);
            }

            SortList();
            CategoriesToSounds();

            // Install Hook
            keyboardHook.HookKeyboard();

            // Cfg
            Cfg.CreateInit();
            Cfg.CreateSongList(SoundsList);
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Save data
            string json = JsonConvert.SerializeObject(Data, Formatting.Indented);

            try
            {
                File.WriteAllText(ProgramSettings.PathSounds + "\\data.json", json);
            }
            catch (Exception)
            {
                MessageBox.Show("Error writing data, please make sure the sound folder exists.");
            }

            ProgramSettings.Save();

            // Deinstal hook
            keyboardHook.UnHookKeyboard();

            // Cfg
            Cfg.RemoveInit();
        }

        // Menu Events
        private void AddSound()
        {
            ImportWindow iw = new ImportWindow()
            {
                Data = this.Data
            };
            iw.ShowDialog();
            int a = 0;
        }
        private void AddCategory()
        {
            AddCategoryWindow acw = new AddCategoryWindow()
            {
                Data = this.Data
            };
            acw.ShowDialog();
            Data = acw.Data;
        }
        private void ViewCategories()
        {
            ShowList = false;
        }
        private void ViewList()
        {
            ShowList = true;
            CategoriesToSounds();
        }
        private void ShowSettingsWindow()
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }
        private void TextToSpeech()
        {
            var dialog = new TextToSpeechWindow();
            dialog.ShowDialog();

            if (!dialog.Canceled)
            {
                StringToTTS(dialog.SpeechText);
            }
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

        // Custom
        private void LoadSound(SoundWAV sound)
        {        
            if (sound != null)
            {
                if (File.Exists(ProgramSettings.PathCsgo + "\\voice_input.wav"))
                {
                    File.Delete(ProgramSettings.PathCsgo + "\\voice_input.wav");
                }
                File.Copy(sound.Path, ProgramSettings.PathCsgo + "\\voice_input.wav");
                IdEntered = "";
                LoadedSound = sound;
                soundPlayer.SoundLocation = sound.Path;
                soundPlayer.Load();
            }

            if (IdEntered.Count() >= 4)
                IdEntered = "";

        }
        private SoundWAV FindSoundById(string id)
        {
            SoundWAV foundSound = null;

            // Find item
            foreach (var category in Data.Categories)
            {
                foundSound = category.Sounds.Where(x => x.IdFull == id).FirstOrDefault();

                if (foundSound != null)
                {
                    foundSound.LoadCount++;
                    break;
                }
            }

            // Return
            return foundSound;
        }
        private void SortList()
        {
            Data.Categories = new ObservableCollection<Category>(Data.Categories.OrderBy(s => s.Name).ToList());
        }
        private void CategoriesToSounds(string filter = "")
        {
            SoundsList.Clear();
            foreach (var category in Data.Categories)
            {
                foreach (var sound in category.Sounds)
                {
                    if (sound.Name.Contains(filter))
                    {
                        SoundsList.Add(new KeyValuePair<string, SoundWAV>(category.Name, sound));
                    }
                }
            }
        }

        private void StringToTTS(string input)
        {
            string pathNotConv = ProgramSettings.PathSounds + "\\audio\\ttsNotConv.wav";

            using (var synth = new SpeechSynthesizer())
            {
                // Configure the audio output. 
                synth.SetOutputToWaveFile(pathNotConv);

                // Build a prompt.
                //PromptBuilder builder = new PromptBuilder();
                //builder.AppendText(input);
                synth.Rate = -2;

                // Change voice
                //synth.SelectVoiceByHints(dialog.SelectedGender);

                // Speak the prompt.
                synth.Speak(input);
                synth.SetOutputToNull();
                synth.Dispose();
            }


            // ReSample
            string path = string.Format("{0}\\audio\\{1}{2}", ProgramSettings.PathSounds, "tts", ".wav");

            using (var reader = new WaveFileReader(pathNotConv))
            {
                using (var resampler = new MediaFoundationResampler(reader, new WaveFormat(22050, 16, 1)))
                {
                    resampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(path, resampler);
                    resampler.Dispose();
                }
            }

            LoadedSound = new SoundWAV()
            {
                Name = "Text To Speech",
                Path =  path
            };

            LoadSound(LoadedSound);

            File.Delete(pathNotConv);
        }

        // Fix scrollwheel on datagrid
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox temp = (TextBox)sender;
            CategoriesToSounds(temp.Text);
        }


        // Command Binding
        public ICommand CommandAddCategory { get { return new RelayCommand(AddCategory); } }
        public ICommand CommandAddSound{ get { return new RelayCommand(AddSound); } }
        public ICommand CommandViewCategories { get { return new RelayCommand(ViewCategories); } }
        public ICommand CommandViewList { get { return new RelayCommand(ViewList); } }
        public ICommand CommandSettings { get { return new RelayCommand(ShowSettingsWindow); } }
        public ICommand CommandTextToSpeech { get { return new RelayCommand(TextToSpeech); } }
        public ICommand CommandPlayPauzeSound { get { return new RelayCommand(PlayPauzeSound); } }



    }
}
