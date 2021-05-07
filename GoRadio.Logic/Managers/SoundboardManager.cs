using GoRadio.Logic.Services;
using NAudio.Wave;

namespace GoRadio.Logic.Managers
{
    public class SoundboardManager
    {
        private WaveOutEvent _speakers;
        private WaveOutEvent _virtualMicrophone;

        public SoundboardManager(AudioDeviceService audioDeviceService, SettingsService settingsService)
        {
            // Get settings
            var settings = settingsService.Get();

            // Initiate audio devices
            _speakers = new WaveOutEvent()
            {
                DeviceNumber = audioDeviceService.GetOutputByName(settings.PlaybackDevice).DeviceNumber
            };
            _virtualMicrophone = new WaveOutEvent()
            {
                DeviceNumber = audioDeviceService.GetOutputByName(settings.VirtualCable).DeviceNumber
            };
        }

        public void Play()
        {
            var url = "http://media.ch9.ms/ch9/2876/fd36ef30-cfd2-4558-8412-3cf7a0852876/AzureWebJobs103.mp3";

            _speakers.Init(new MediaFoundationReader(url));
            _virtualMicrophone.Init(new MediaFoundationReader(url));

            _speakers.Play();
            _virtualMicrophone.Play();
        }
    }
}
