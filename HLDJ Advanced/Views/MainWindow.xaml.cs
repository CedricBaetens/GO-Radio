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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HLDJ_Advanced.Views;
using Kennedy.ManagedHooks;
using Newtonsoft.Json;
using PropertyChanged;
using HLDJ_Advanced.Classes;

namespace HLDJ_Advanced
{
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {
        private KeyboardHook keyboardHook;
        public ObservableCollection<Category> Categories { get; set; }

        private Dictionary<string, SoundWAV> AllSounds;


        public string SelectedSound { get; set; }

        private bool keyDown = false;

        public MainWindow()
        {
            InitializeComponent();

            // Instance
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyboardEvent += KeyboardHook_KeyboardEvent;
            keyboardHook.InstallHook();

            SelectedSound = "";

            Categories = new ObservableCollection<Category>();
            AllSounds = new Dictionary<string, SoundWAV>();

            // Binding
            DataContext = this;
        }

        // Hook
        private void KeyboardHook_KeyboardEvent(KeyboardEvents kEvent, System.Windows.Forms.Keys key)
        {
            if (kEvent == KeyboardEvents.KeyDown)
            {
                keyDown = true;
            }

            if (keyDown == true && kEvent == KeyboardEvents.KeyUp)
            {
                keyDown = false;

                #region keys
                switch (key)
                {
                    case Keys.NumPad0:
                        SelectedSound += "0";
                        break;

                    case Keys.NumPad1:
                        SelectedSound += "1";
                        break;

                    case Keys.NumPad2:
                        SelectedSound += "2";
                        break;

                    case Keys.NumPad3:
                        SelectedSound += "3";
                        break;

                    case Keys.NumPad4:
                        SelectedSound += "4";
                        break;

                    case Keys.NumPad5:
                        SelectedSound += "5";
                        break;

                    case Keys.NumPad6:
                        SelectedSound += "6";
                        break;

                    case Keys.NumPad7:
                        SelectedSound += "7";
                        break;

                    case Keys.NumPad8:
                        SelectedSound += "8";
                        break;

                    case Keys.NumPad9:
                        SelectedSound += "9";
                        break;
                }
                #endregion

                if (SelectedSound.Count() == 4)
                {
                    LoadSound(SelectedSound);
                }

            }
            
        }



        // Window events
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Helper.SoundPath + "data.json"))
            {
                string json = File.ReadAllText(Helper.SoundPath + "data.json");
                Categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
            }

            foreach (var category in Categories)
            {
                foreach (var sound in category.Sounds)
                {
                    AllSounds.Add(sound.IdFull, sound);
                }
            }

            // Install Hook
            keyboardHook.InstallHook();
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Save data
            string json = JsonConvert.SerializeObject(Categories, Formatting.Indented);
            File.WriteAllText(Helper.SoundPath + "data.json", json);

            // Deinstal hook
            keyboardHook.UninstallHook();
        }

        // Menu Events
        private void miImport_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow iw = new ImportWindow()
            {
                Categories = this.Categories
            };
            iw.ShowDialog();

            Categories = iw.Categories;

            UpdateAllSoundList();
        }
        private void miCreateFolders_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow acw = new AddCategoryWindow()
            {
                Categories = this.Categories
            };
            acw.ShowDialog();

            this.Categories = acw.Categories;
        }

        // Button
        private void BtnLoadSong_OnClick(object sender, RoutedEventArgs e)
        {
            if (AllSounds.ContainsKey(SelectedSound))
            {
                LoadSound(SelectedSound);
            }
        }

        // Custom
        private void LoadSound(string id)
        {
            if (AllSounds.ContainsKey(SelectedSound))
            {
                var song = AllSounds[id];

                if (File.Exists(Helper.CsgoPath + "voice_input.wav"))
                {
                    File.Delete(Helper.CsgoPath + "voice_input.wav");
                }
                File.Copy(song.Path, Helper.CsgoPath + "voice_input.wav");
            }

            SelectedSound = "";


        }
        private void UpdateAllSoundList()
        {
            AllSounds.Clear();
            foreach (var category in Categories)
            {
                foreach (var sound in category.Sounds)
                {
                    AllSounds.Add(sound.IdFull, sound);
                }
            }
        }
    }
}
