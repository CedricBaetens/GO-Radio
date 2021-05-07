using GoRadio.Logic.Model;
using NAudio.Wave;
using System.Collections.Generic;
using System.Linq;

namespace GoRadio.Logic.Services
{
    public class AudioDeviceService
    {
        public IEnumerable<AudioDevice> GetOutputDevices()
        {
            var result = new List<AudioDevice>();
            int waveInDevices = WaveOut.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                var deviceInfo = WaveOut.GetCapabilities(waveInDevice);
                result.Add(new AudioDevice() { DeviceNumber = waveInDevice, Name = deviceInfo.ProductName });
            }
            return result;
        }
        public IEnumerable<AudioDevice> GetInputDevices()
        {
            var result = new List<AudioDevice>();
            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                var deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                result.Add(new AudioDevice() { DeviceNumber = waveInDevice, Name = deviceInfo.ProductName });
            }
            return result;
        }

        public AudioDevice GetInputByName(string name)
        {
            return GetInputDevices().FirstOrDefault(x => x.Name == name);
        }
        public AudioDevice GetOutputByName(string name)
        {
            return GetOutputDevices().FirstOrDefault(x => x.Name == name);
        }
    }
}
