using GoRadio.Logic.Database.Entities;
using GoRadio.Logic.Services;
using System.Collections.ObjectModel;

namespace GoRadio.DesktopApp.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<Command<Sound>> Sounds { get; } = new ObservableCollection<Command<Sound>>();

        private readonly SoundService _soundService;

        public MainViewModel(SoundService soundService)
        {
            _soundService = soundService;
        }

        public void Load()
        {
            var sounds = _soundService.GetAll();
            foreach (var item in sounds)
            {
                Sounds.Add(new Command<Sound>(item, PlaySound));
            }
        }

        private void PlaySound(Sound sound)
        {
            _soundService.Play(1);
        }
    }
}
