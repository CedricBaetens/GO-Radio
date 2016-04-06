using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go_Radio_V2.Classes
{
    class ProgramSelector
    {
        public List<ProgramSelection> Programs { get; set; }

        public ProgramSelector()
        {
            Programs = new List<ProgramSelection>()
            {
                new ProgramSelection() { Name="CSGO" },
                new ProgramSelection() { Name="Skype" }
            };
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
    }
}
