using GO_Radio.Classes.Settings;
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
            RUNNING,
            UNDEFINED
        };

        // Diffrent Application Selections
        public ObservableCollection<ProgramSelection> Programs { get; set; }
        public ProgramSelection ActiveProgram { get; set; }
        public ApplicationState State { get; set; }
        public CategoryList Data { get; set; }

        private UserSettings userSettings;

        public ProgramSelector()
        {
            Programs = new ObservableCollection<ProgramSelection>()
            {
                new SourceGame() { Name="Counter Strike: Global Offensive" },
                new ProgramSelection() { Name="Skype (Still in development", IsSelectable = false }
            };
            Data = new CategoryList();

            ActiveProgram = Programs[0];
            State = ApplicationState.STANDBY;
        }

        public void Load(UserSettings settings)
        {
            userSettings = settings;

            Programs[0].Load(userSettings.CsgoSettings);

            // Load sound data
            if (!Directory.Exists(userSettings.SoundPath))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog()
                {
                    Description = "Please select a location where you want your sounds to be stored."
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
        public UserSettings GetUserSettings()
        {
            userSettings.CsgoSettings = Programs[0].Setting;
            userSettings.SkypeSettings = Programs[1].Setting;

            return userSettings;
        }

        public void Start()
        {
            State = ApplicationState.RUNNING;
            ActiveProgram.Start(Data);
        }
        public void Stop()
        {
            State = ApplicationState.STANDBY;
            ActiveProgram.Stop();
        }
    }
}
