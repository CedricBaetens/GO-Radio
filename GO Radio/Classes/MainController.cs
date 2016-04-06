﻿using System;
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
using GO_Radio.Classes.Settings;
using GO_Radio.Classes.ApplicationTypes;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class MainController : ILoadSave
    {
        private SettingsController settings;
        private ProgramSelector programSelector;

        // Constructor
        public MainController()
        {
            settings = new SettingsController();
            programSelector = new ProgramSelector();
        }
    
       // Interface Methods
        public void Load()
        {
            // Load the usersettings and pass them to the program
            programSelector.Load(settings.LoadJSON());
        }
        public void Save()
        {
            // Get the latest usersettings from the program and save them as JSON.
            settings.SaveJSON(programSelector.GetUserSettings());
        }

        // Command
        //public ICommand CommandResetPlayingMonitor => new RelayCommand(SoundLoader.Reset);
        public ICommand CommandKeyBinding => new RelayCommand(ShowKeyBinding);
        public ICommand CommandAddCategory => new RelayCommand(ShowCategoryWindow);
        public ICommand CommandAddSound => new RelayCommand(ShowSoundWindow);
        public ICommand CommandPlayPauzeSound => new RelayCommand(SoundplayerPlayPauzeSound);
        //public ICommand CommandStart => new RelayCommand(ApplicationSelection.Start);
        //public ICommand CommandStop => new RelayCommand(ApplicationSelection.Stop);

        private void ShowCategoryWindow()
        {
            //if (ApplicationSelection.IsIdle())
            //{
            //    AddCategoryWindow acw = new AddCategoryWindow(ApplicationSelection.Data);
            //    acw.ShowDialog();
            //    Save();
            //}

        }
        private void ShowSoundWindow()
        {
            //if (ApplicationSelection.IsIdle())
            //{

            //    ImportWindow iw = new ImportWindow(ApplicationSelection.Data);
            //    iw.ShowDialog();

            //    ApplicationSelection.Data.UpdateDictionary();
            //    Save();

            //}
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
