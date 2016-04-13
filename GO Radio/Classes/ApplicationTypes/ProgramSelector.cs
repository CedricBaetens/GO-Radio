using GO_Radio.Classes.Settings;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.IO;
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
        private UserSettings _userSettings;

        // Constructor
        public ProgramSelector()
        {
            Programs = new ObservableCollection<ProgramSelection>()
            {
                new SourceGame() { Name="Counter Strike: Global Offensive" },
                new GenericApplication() { Name="Generic Application", IsSelectable = false }
            };
            Data = new CategoryList();

            ActiveProgram = Programs[0];
            State = ApplicationState.STANDBY;
        }

        public void Load(UserSettings settings)
        {
            _userSettings = settings;

            Programs[0].Load(_userSettings.CsgoSettings);
            Programs[1].Load(_userSettings.SkypeSettings);

            // Load sound data
            if (!Directory.Exists(_userSettings.SoundPath))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog()
                {
                    Description = @"Please select a location where you want your sounds to be stored."
                };
                fbd.ShowDialog();

                if (!string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    _userSettings.SoundPath = fbd.SelectedPath + "\\Sounds";
                    Directory.CreateDirectory(_userSettings.SoundPath + "\\audio");
                    Directory.CreateDirectory(_userSettings.SoundPath + "\\new");
                }
            }

            Data.Load(_userSettings.SoundPath);
            AudioHelper.Load(_userSettings.SoundPath);
        }
        public UserSettings Save()
        {
            // Save sounddata
            Data.Save();

            // Return usersettings
            _userSettings.CsgoSettings = Programs[0].Setting;
            _userSettings.SkypeSettings = Programs[1].Setting;
            _userSettings.SoundPath = Data.Path;

            return _userSettings;
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
            return State == ApplicationState.STANDBY;
        }
    }
}
