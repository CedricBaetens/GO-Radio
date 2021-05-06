using GoRadio.Logic.Services;
using NAudio.Wave;
using System.Linq;

namespace GoRadio.Logic.Managers
{
    public class MicrophoneManager
    {
        public bool Listen { get; set; } = true;

        // Audio devices
        private WaveInEvent _microphone;
        private WaveOut _virtualMicrophone;


        private readonly BufferedWaveProvider _speakersWaveProvider;
        //private readonly BufferedWaveProvider _virtualMicrophoneWaveProvider;

        public MicrophoneManager(AudioDeviceService audioDeviceService)
        {
            // Initiate audio devices
            _microphone = new WaveInEvent { BufferMilliseconds = 25, NumberOfBuffers = 5 };
            _virtualMicrophone = new WaveOut() 
            { 
                DeviceNumber = audioDeviceService.GetOutputDevices().First(x => x.Item2.ProductName.Contains("VB-Audio")).Item1,
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
