using GoRadio.Logic.Model;
using Newtonsoft.Json;
using System.IO;

namespace GoRadio.Logic.Services
{
    public class SettingsService
    {
        private UserSettings _settings;

        public UserSettings Get()
        {
            if (_settings == null)
                Load();
            return _settings;
        }
        public void Load()
        {
            if (!File.Exists("settings.json"))
            {
                _settings = new UserSettings();
                return;
            }

            var json = File.ReadAllText("settings.json");
            _settings = JsonConvert.DeserializeObject<UserSettings>(json);
        }
        public void Save(UserSettings userSettings)
        {
            var json = JsonConvert.SerializeObject(userSettings);
            File.WriteAllText("settings.json", json);
        }
    }
}
