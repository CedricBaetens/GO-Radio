using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace TestConvertion
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string file = @"C:\Users\Baellon\Desktop\Songs\backup\original.mp3";

            // WORKS BUT PLAY TO SLOW + LAGGY
            int outRate = 22050;
            var mp3In = @"C:\Users\Baellon\Desktop\Songs\backup\original.mp3";
            var wavOut = @"C:\Users\Baellon\Desktop\Songs\backup\wavout.wav";
            var wavOutConv = @"C:\Users\Baellon\Desktop\Songs\backup\voice_input.wav";
            var testFile = @"C:\Users\Baellon\Desktop\Songs\backup\test.wav";

            //using (Mp3FileReader reader = new Mp3FileReader(mp3In))
            //{
            //    using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
            //    {
            //        WaveFileWriter.CreateWaveFile(wavOut, pcmStream);
            //    }
            //}

            var reader = new Mp3FileReader(mp3In);
            var output = new WaveFormat(22050, 16, 1);
            var resampler = new MediaFoundationResampler(reader, output);
            resampler.ResamplerQuality = 10;
            WaveFileWriter.CreateWaveFile(wavOutConv, resampler);
            resampler.Dispose();



            //using (var reader = new WaveFileReader(testFile))
            //{
            //    var newFormat = new WaveFormat(reader.WaveFormat.SampleRate, reader.WaveFormat.BitsPerSample, reader.WaveFormat.Channels);
            //    using (WaveStream conversionStream = new WaveFormatConversionStream(reader.WaveFormat, reader))
            //    {
            //        WaveFileWriter.CreateWaveFile(wavOutConv, conversionStream);
            //    }
            //}






            Console.ReadLine();
        }
    }
}
