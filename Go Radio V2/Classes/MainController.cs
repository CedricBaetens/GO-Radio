using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go_Radio_V2.Classes
{
    class MainController : ILoadSafe
    {
        private SettingsController settings;
        private ProgramSelector programSelector;

        public MainController()
        {
            settings = new SettingsController();
            programSelector = new ProgramSelector();
        }

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
    }
}
