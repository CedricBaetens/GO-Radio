using GoRadio.Logic.Services;
using NAudio.Wave;
using System.Linq;

namespace GoRadio.Logic.Managers
{
    public class SoundboardManager
    {
        private WaveOut _speakers;
        private WaveOut _virtualMicrophone;

        public SoundboardManager(AudioDeviceService audioDeviceService)
        {
            // Initiate audio devices
            _speakers = new WaveOut();
            _virtualMicrophone = new WaveOut() { DeviceNumber = audioDeviceService.GetOutputDevices().First(x => x.Item2.ProductName.Contains("VB-Audio")).Item1 };
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
