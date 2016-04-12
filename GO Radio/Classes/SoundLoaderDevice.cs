using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GO_Radio.Classes
{
    //https://bitbucket.org/Baellon/csgo-radio/src/ad7bda283d580d583fb12ff424fba480a456bd81/GO%20Radio/Classes/SoundLoader.cs?at=universal&fileviewer=file-view-default
    class SoundLoaderDevice
    {
        private WaveOut virtualOutput;

        public SoundLoaderDevice()
        {
                virtualOutput = new WaveOut();
        }

        public void Start()
        {
            // Find Output Device
            var found = false;
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var output = WaveOut.GetCapabilities(i);
                if (output.ProductName.Contains("CABLE Input (VB-Audio Virtual C"))
                {
                    found = true;
                    virtualOutput.DeviceNumber = i;
                }
            }

            if (!found)
            {
                MessageBox.Show("Audio devices not found, instal VB-Audio");
            }
        }
    }
}
