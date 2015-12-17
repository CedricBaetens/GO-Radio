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

        // Private
        private LowLevelKeyboardListener keyboardHook;

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

            IdEntered = "";
            ShowList = false;

            // Binding
            DataContext = this;
            

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
            }
            #endregion

            if (IdEntered.Count() >= 4)
                LoadSound(IdEntered);
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
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Save data
            string json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(ProgramSettings.PathSounds + "\\data.json", json);
            ProgramSettings.Save();

            // Deinstal hook
            keyboardHook.UnHookKeyboard();
        }

        // Menu Events
        private void miImport_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow iw = new ImportWindow()
            {
                Data = this.Data
            };
            iw.ShowDialog();
            int a = 0;
        }
        private void miCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow acw = new AddCategoryWindow()
            {
                Data = this.Data
            };
            acw.ShowDialog();
            Data = acw.Data;
        }
        private void miViewCategories_Click(object sender, RoutedEventArgs e)
        {
            ShowList = false;
        }
        private void miViewList_Click(object sender, RoutedEventArgs e)
        {
            ShowList = true;
            CategoriesToSounds();
        }



        // Custom
        private void LoadSound(string id)
        {
            SoundWAV foundSound = null;

            // Find item
            foreach (var category in Data.Categories)
            {
                foreach (SoundWAV sound in category.Sounds)
                {
                    if (sound.IdFull == id)
                    {
                        foundSound = sound;
                        sound.LoadCount++;
                        break;
                    }
                }

                if (foundSound != null)
                {
                    break;
                }
            }

            if (foundSound != null)
            {
                if (File.Exists(ProgramSettings.PathCsgo + "\\voice_input.wav"))
                {
                    File.Delete(ProgramSettings.PathCsgo + "\\voice_input.wav");
                }
                File.Copy(foundSound.Path, ProgramSettings.PathCsgo + "\\voice_input.wav");
                IdEntered = "";
                LoadedSound = foundSound;
            }

            if (IdEntered.Count() >= 4)
                IdEntered = "";

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
    }
}
