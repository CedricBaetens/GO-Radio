﻿using System;
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
        // Public
        public Data Data { get; set; }
        public Dictionary<string, SoundWAV> AllSounds;
        public string IdEntered { get; set; }
        public SoundWAV LoadedSound { get; set; }

        // Private
        private KeyboardHook keyboardHook;
        private bool keyDown = false;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Instance
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyboardEvent += KeyboardHook_KeyboardEvent;
            keyboardHook.InstallHook();

            Data = new Data();
            AllSounds = new Dictionary<string, SoundWAV>();
            LoadedSound = new SoundWAV();
            
            IdEntered = "";
            
            // Binding
            DataContext = this;
        }


        // Hook
        private void KeyboardHook_KeyboardEvent(KeyboardEvents kEvent, System.Windows.Forms.Keys key)
        {
            if (kEvent == KeyboardEvents.KeyDown)
                keyDown = true;

            if (keyDown == true && kEvent == KeyboardEvents.KeyUp)
            {
                keyDown = false;

                #region keys
                switch (key)
                {
                    case Keys.NumPad0:
                        IdEntered += "0";
                        break;

                    case Keys.NumPad1:
                        IdEntered += "1";
                        break;

                    case Keys.NumPad2:
                        IdEntered += "2";
                        break;

                    case Keys.NumPad3:
                        IdEntered += "3";
                        break;

                    case Keys.NumPad4:
                        IdEntered += "4";
                        break;

                    case Keys.NumPad5:
                        IdEntered += "5";
                        break;

                    case Keys.NumPad6:
                        IdEntered += "6";
                        break;

                    case Keys.NumPad7:
                        IdEntered += "7";
                        break;

                    case Keys.NumPad8:
                        IdEntered += "8";
                        break;

                    case Keys.NumPad9:
                        IdEntered += "9";
                        break;
                }
                #endregion

                if (IdEntered.Count() == 4)
                    LoadSound(IdEntered);
            }            
        }



        // Window events
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Helper.Load();

            // Load Data
            if (File.Exists(Helper.PathSounds + "\\data.json"))
            {
                string json = File.ReadAllText(Helper.PathSounds + "\\data.json");
                Data = JsonConvert.DeserializeObject<Data>(json);

                foreach (var category in Data.Categories)
                {
                    foreach (var sound in category.Sounds)
                    {
                        AllSounds.Add(sound.IdFull, sound);
                    }
                }
            }

            SortList();

            // Install Hook
            keyboardHook.InstallHook();
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Save data
            string json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(Helper.PathSounds + "\\data.json", json);
            Helper.Save();

            // Deinstal hook
            keyboardHook.UninstallHook();
        }

        // Menu Events
        private void miImport_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow iw = new ImportWindow()
            {
                Data = this.Data
            };
            iw.ShowDialog();
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



        // Custom
        private void LoadSound(string id)
        {
            if (AllSounds.ContainsKey(IdEntered))
            {
                var song = AllSounds[id];

                if (File.Exists(Helper.PathCsgo + "voice_input.wav"))
                {
                    File.Delete(Helper.PathCsgo + "voice_input.wav");
                }
                File.Copy(song.Path, Helper.PathCsgo + "voice_input.wav");
            }
            IdEntered = "";
        }
        private void SortList()
        {
            Data.Categories = new ObservableCollection<Category>(Data.Categories.OrderBy(s => s.Name).ToList());
        }



        // Fix scrollwheel on datagrid
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
        }
    }
}
