using NAudio.Wave;

namespace GoRadio.Logic.Services
{
    public class MicrophoneService
    {
        private readonly WaveInEvent _waveIn;
        private readonly WaveOut _waveOut;
        private readonly BufferedWaveProvider _waveProvider;

        public MicrophoneService()
        {
            // create wave input from mic
            _waveIn = new WaveInEvent
            {
                BufferMilliseconds = 25
            };
            _waveIn.DataAvailable += WaveIn_DataAvailable;

            // create wave provider
            _waveProvider = new BufferedWaveProvider(_waveIn.WaveFormat);

            // create wave output to speakers
            _waveOut = new WaveOut
            {
                DesiredLatency = 100,
            };
            _waveOut.Init(_waveProvider);
        }

        public void Start()
        {
            _waveIn.StartRecording();
            _waveOut.Play();
        }
        public void Stop()
        {
            _waveIn.StopRecording();
            _waveOut.Stop();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // add received data to waveProvider buffer
            if (_waveProvider != null)
                _waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
