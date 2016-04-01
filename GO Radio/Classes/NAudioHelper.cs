using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VideoLibrary;
using System.Diagnostics;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public static class AudioHelper
    {
        public static void TrimWavFile(string inPath,string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {

            using (WaveFileReader reader = new WaveFileReader(inPath))
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
                {
                    int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000;

                    int startPos = (int)cutFromStart.TotalMilliseconds * bytesPerMillisecond;
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)cutFromEnd.TotalMilliseconds * bytesPerMillisecond;
                    endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                    int endPos = (int)reader.Length - endBytes;

                    TrimWavFile(reader, writer, startPos, endPos);
                }
            }
        }
        private static void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public static SoundNew Convert(SoundUnconverted unconvertedSound)
        {
             int sampleRate = 22050;
             int bits = 16;
             int channels = 1;

            // ReSample
            string path = string.Format("{0}\\audio\\{1}{2}", ProgramSettings.Instance.PathSounds, unconvertedSound.Name, ".wav");
            using (var reader = new MediaFoundationReader(unconvertedSound.Path))
            {
                WaveChannel32 wav = new WaveChannel32(reader);
                //wav.Volume = (150 / 100) ^ 6;
                using (var resampler = new MediaFoundationResampler(reader, new WaveFormat(sampleRate, bits, channels)))
                {
                    resampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(path, resampler);
                }
            }
            

            File.Delete(unconvertedSound.Path);

            return new SoundNew(path);
        }

        public static void Create(SoundNew sound, string output)
        {
            int sampleRate = 22050;
            int bits = 16;
            int channels = 1;

            // ReSample
            //string path = string.Format("{0}\\audio\\{1}{2}", ProgramSettings.Instance.PathSounds, unconvertedSound.Name, ".wav");
            using (var reader = new MediaFoundationReader(sound.Path))
            {
                WaveChannel32 wav = new WaveChannel32(reader);
                wav.Volume = sound.Volume / 100;
                wav.PadWithZeroes = false;

                using (var resampler = new MediaFoundationResampler(wav, new WaveFormat(sampleRate, bits, channels)))
                {
                    resampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(output, resampler);
                }
            }
        }
    }
}
