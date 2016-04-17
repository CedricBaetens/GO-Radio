using NAudio.Wave;
using System;
using System.IO;
using PropertyChanged;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public static class AudioHelper
    {
        static string soundPath;
        
        public static void Load(string path)
        {
            soundPath = path;
        }
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
            string path = string.Format("{0}\\audio\\{1}{2}", soundPath, unconvertedSound.Name, ".wav");// NEEDS TO BE DELETED
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
        public static void Create(SoundNew sound, string path, TimeSpan extraTrim = new TimeSpan())
        {
            int sampleRate = 22050;
            int bits = 16;
            int channels = 1;

            // ReSample to temp file
            using (var reader = new MediaFoundationReader(sound.Path))
            {
                WaveChannel32 wav = new WaveChannel32(reader);
                wav.Volume = sound.Volume / 100;
                wav.PadWithZeroes = false;

                using (var resampler = new MediaFoundationResampler(wav, new WaveFormat(sampleRate, bits, channels)))
                {
                    resampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(path + "\\temp.wav", resampler);
                }
            }

            // Remove old file
            if (File.Exists(path + "\\voice_input.wav"))
                File.Delete(path + "\\voice_input.wav");

            // Trim file
            TrimWavFile(path + "\\temp.wav", path + "\\voice_input.wav", sound.TrimStart + extraTrim, sound.TrimEnd);
            File.Delete(path + "\\temp.wav");
        }
    }
}
