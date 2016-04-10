using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes
{
    //class SoundLoaderAdvanced : SoundLoader
    //{
    //    /*
    //    private WaveIn microphone;
    //    private WaveOut virtualOutput;
    //    private WaveOut virtualOutputForMic;
    //    private WaveOut speakers;
    //    private BufferedWaveProvider bwp;


    //    // LOADFUNC
    //    /*

        
    //                var reader = new WaveFileReader(Sound.Path);
    //                speakers.Init(reader);

    //                var reader2 = new WaveFileReader(Sound.Path);
    //                virtualOutput.Init(reader2);


    //    */


    //    //public void CorrectDevices()
    //    //{
    //    //    // Find Output Device
    //    //    for (int i = 0; i < WaveOut.DeviceCount; i++)
    //    //    {
    //    //        var output = WaveOut.GetCapabilities(i);
    //    //        if (output.ProductName.Contains("CABLE Input (VB-Audio Virtual C"))
    //    //        {
    //    //            virtualOutput = new WaveOut();
    //    //            virtualOutput.DeviceNumber = i;

    //    //            virtualOutputForMic = new WaveOut();
    //    //            virtualOutputForMic.DeviceNumber = i;
    //    //        }
    //    //    }

    //    //    // Default Audio Device
    //    //    speakers = new WaveOut();
    //    //    speakers.DeviceNumber = 0;

    //    //    // Micro
    //    //    microphone = new WaveIn();
    //    //    microphone.DeviceNumber = 0;
    //    //    microphone.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(0).Channels);

    //    //    microphone.DataAvailable += Microphone_DataAvailable;
    //    //    bwp = new BufferedWaveProvider(microphone.WaveFormat);
    //    //    bwp.DiscardOnBufferOverflow = true;

    //    //    virtualOutputForMic.Init(bwp);
    //    //    microphone.StartRecording();
    //    //    virtualOutputForMic.Play();
    //    //}



    //    //private void Microphone_DataAvailable(object sender, WaveInEventArgs e)
    //    //{
    //    //    bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
    //    //}
    //}
}
