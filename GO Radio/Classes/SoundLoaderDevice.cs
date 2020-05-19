using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GO_Radio.Classes
{
    class SoundLoaderDevice : SoundLoader
    {
        private WaveOut _WaveOut = new WaveOut();

        protected override void OnPlay()
        {
            if(State == SoundLoader.SoundState.PAUSED)
            {
                _WaveOut.Resume();
            }
            else
            {
                var waveReader = new WaveFileReader(Sound.Path);

                _WaveOut.DeviceNumber = 0;
                _WaveOut.Init(waveReader);
                _WaveOut.Play();
            }
        }


        protected override void OnPauze()
        {
            _WaveOut.Pause();
        }

        protected override void OnStop()
        {
            _WaveOut.Stop();
        }

        //private WaveOut virtualOutput;

        //public SoundLoaderDevice()
        //{
        //        virtualOutput = new WaveOut();
        //}

        //public bool Start()
        //{
        //    return VirtualDeviceFound();
        //}


        //private bool VirtualDeviceFound()
        //{
        //    // Find Output Device
        //    var found = false;
        //    for (int i = 0; i < WaveOut.DeviceCount; i++)
        //    {
        //        var output = WaveOut.GetCapabilities(i);
        //        if (output.ProductName.Contains("CABLE Input (VB-Audio Virtual C"))
        //        {
        //            found = true;
        //            virtualOutput.DeviceNumber = i;
        //        }
        //    }
        //    if (!found)
        //    {
        //        MessageBox.Show("Audio devices not found, instal VB-Audio");
        //        return false;
        //    }
        //    return true;
        //}
    }
}
