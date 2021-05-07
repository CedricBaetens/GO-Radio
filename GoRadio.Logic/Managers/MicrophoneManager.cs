using GoRadio.Logic.Services;
using NAudio.Wave;
using System.Linq;

namespace GoRadio.Logic.Managers
{
    public class MicrophoneManager
    {
        // Audio devices
        private readonly WaveInEvent _microphone;
        private readonly WaveOut _virtualMicrophone;
        private readonly BufferedWaveProvider _speakersWaveProvider;

        public MicrophoneManager(AudioDeviceService audioDeviceService, SettingsService settingsService)
        {
            // Get settings
            var settings = settingsService.Get();

            // Initiate audio devices
            _microphone = new WaveInEvent
            {
                BufferMilliseconds = 25,
                NumberOfBuffers = 5,
                DeviceNumber = audioDeviceService.GetInputByName(settings.Microphone).DeviceNumber,
            };
            _virtualMicrophone = new WaveOut()
            {
                DeviceNumber = audioDeviceService.GetOutputByName(settings.VirtualCable).DeviceNumber,
                DesiredLatency = 125,
            };

            // Get microphone data
            _microphone.DataAvailable += WaveIn_DataAvailable;

            // create wave provider
            _speakersWaveProvider = new BufferedWaveProvider(_microphone.WaveFormat);
            _virtualMicrophone.Init(_speakersWaveProvider);
        }

        public void Start()
        {
            _microphone.StartRecording();
            _virtualMicrophone.Play();
        }
        public void Stop()
        {
            _microphone.StopRecording();
            _virtualMicrophone.Stop();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // add received data to waveProvider buffer
            if (_speakersWaveProvider != null && _speakersWaveProvider.BufferedDuration.TotalMilliseconds <= 100)
                _speakersWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
