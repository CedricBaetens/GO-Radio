﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go_Radio_V2.Classes
{
    class SettingsController
    {
        public string AppFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GO Radio"); } }

        public UserSettings LoadJSON()
        {
            string loc = Path.Combine(AppFolder, "settings.json");
            if (File.Exists(loc))
            {
                string json = File.ReadAllText(loc);
                return JsonConvert.DeserializeObject<UserSettings>(json);
            }
            return new UserSettings();
        }
        public void SaveJSON(UserSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            try
            {
                File.WriteAllText(AppFolder + "\\settings.json", json);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Error writing settings.");
            }
        }
    }
}