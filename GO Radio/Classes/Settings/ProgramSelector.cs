using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes.Settings
{
    class ProgramSelector
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

        public ProgramSelector()
        {
            Programs = new ObservableCollection<ProgramSelection>()
            {
                new ProgramSelection() { Name="Counter Strike: Global Offensive" },
                new ProgramSelection() { Name="Skype (Still in development" }
            };

            ActiveProgram = Programs[0];
        }

        public void Load(UserSettings settings)
        {
            Programs[0].Setting = settings.CsgoSettings;
            Programs[1].Setting = settings.SkypeSettings;
        }

        public UserSettings GetUserSettings()
        {
            return new UserSettings()
            {
                CsgoSettings = Programs[0].Setting,
                SkypeSettings = Programs[1].Setting
            };
        }

        public void Start()
        {
            State = ApplicationState.RUNNING;
            ActiveConfiguration.Start();
        }
        public void Stop()
        {
            State = ApplicationState.STANDBY;
            ActiveConfiguration.Stop();
        }
    }
}
