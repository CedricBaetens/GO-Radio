using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes.Settings
{
    static class SettingsController
    {
        public static string AppFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GO Radio"); } }

        public static UserSettings LoadUserSettingsFromJSON()
        {
            string loc = Path.Combine(AppFolder, "settings.json");
            if (File.Exists(loc))
            {
                string json = File.ReadAllText(loc);
                return JsonConvert.DeserializeObject<UserSettings>(json);
            }
            return new UserSettings();
        }
        public static void SaveUserSettingsToJSON(UserSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            try
            {
                var dir = Directory.CreateDirectory(AppFolder);
                File.WriteAllText(AppFolder + "\\settings.json", json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error writing settings.");
            }
        }
    }
}
