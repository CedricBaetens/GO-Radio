﻿using GO_Radio.Classes.Settings;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GO_Radio.Classes.ApplicationTypes
{
    [ImplementPropertyChanged]
    public class ProgramSelector
    {
        public enum ApplicationState
        {
            STANDBY,
            RUNNING
        };

        // Properties
        public ObservableCollection<ProgramSelection> Programs { get; set; }
        public ProgramSelection ActiveProgram { get; set; }
        public ApplicationState State { get; set; }
        public CategoryList Data { get; set; }

        // Variables
        private UserSettings userSettings;

        // Constructor
        public ProgramSelector()
        {
            Programs = new ObservableCollection<ProgramSelection>()
            {
                new SourceGame() { Name="Counter Strike: Global Offensive" },
                new GenericApplication() { Name="Generic Application", IsSelectable = false }
            };
            Data = new CategoryList();

            ActiveProgram = Programs[1];
            State = ApplicationState.STANDBY;
        }

        public void Load(UserSettings settings)
        {
            userSettings = settings;

            Programs[0].Load(userSettings.CsgoSettings);
            Programs[1].Load(userSettings.SkypeSettings);

            // Load sound data
            if (!Directory.Exists(userSettings.SoundPath))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog()
                {
                    Description = @"Please select a location where you want your sounds to be stored."
                };
                fbd.ShowDialog();

                if (!string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    userSettings.SoundPath = fbd.SelectedPath + "\\Sounds";
                    Directory.CreateDirectory(userSettings.SoundPath + "\\audio");
                    Directory.CreateDirectory(userSettings.SoundPath + "\\new");
                }
            }

            Data.Load(userSettings.SoundPath);
            AudioHelper.Load(userSettings.SoundPath);
        }
        public UserSettings Save()
        {
            // Save sounddata
            Data.Save();

            // Return usersettings
            userSettings.CsgoSettings = Programs[0].Setting;
            userSettings.SkypeSettings = Programs[1].Setting;
            userSettings.SoundPath = Data.Path;

            return userSettings;
        }
        public void Start()
        {
            ActiveProgram.Start(Data);
            State = ActiveProgram.State;
        }
        public void Stop()
        {
            ActiveProgram.Stop();
            State = ActiveProgram.State;
        }
        public bool IsIdle()
        {
            return State == ApplicationState.STANDBY ? true : false;
        }
    }
}
