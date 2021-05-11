using GoRadio.Logic.Services;
using NAudio.Wave;
using System.IO;

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

        public void Load(byte[] data)
        {
            if (_speakers.PlaybackState == PlaybackState.Playing)
                _speakers.Stop();

            if (_virtualMicrophone.PlaybackState == PlaybackState.Playing)
                _virtualMicrophone.Stop();

            _speakers.Init(new Mp3FileReader(new MemoryStream(data)));
            _virtualMicrophone.Init(new Mp3FileReader(new MemoryStream(data)));
        }
        public void Play()
        {
            _speakers.Play();
            _virtualMicrophone.Play();
        }
        public void Stop()
        {
            _speakers.Stop();
            _virtualMicrophone.Stop();
        }
    }
}
