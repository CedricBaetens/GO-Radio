using GoRadio.DesktopApp.Core;

namespace GoRadio.DesktopApp.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand SoundboardViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        private object _currentView;

        public MainViewModel(SoundboardViewModel soundboardViewModel, SettingsViewModel settingsViewModel)
        {
            CurrentView = soundboardViewModel;
            SoundboardViewCommand = new RelayCommand(o => { CurrentView = soundboardViewModel; });
            SettingsViewCommand = new RelayCommand(o => { CurrentView = settingsViewModel; });
        }
    }
}
