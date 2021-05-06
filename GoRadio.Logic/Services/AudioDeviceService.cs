using NAudio.Wave;
using System.Collections.Generic;

namespace GoRadio.Logic.Services
{
    public class AudioDeviceService
    {
        public IEnumerable<(int, WaveOutCapabilities)> GetOutputDevices()
        {
            var result = new List<(int, WaveOutCapabilities)>();
            int waveInDevices = WaveOut.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                var deviceInfo = WaveOut.GetCapabilities(waveInDevice);
                result.Add((waveInDevice, deviceInfo));
            }
            return result;
        }
        public IEnumerable<(int, WaveInCapabilities)> GetInputDevices()
        {
            var result = new List<(int, WaveInCapabilities)>();
            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                var deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                result.Add((waveInDevice, deviceInfo));
            }
            return result;
        }
    }
}
