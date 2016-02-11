﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VideoLibrary;
using System.Diagnostics;
using PropertyChanged;

namespace CSGO_Radio.Classes
{
    [ImplementPropertyChanged]
    public static class AudioHelper
    {
        public static void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
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
                        writer.WriteData(buffer, 0, bytesRead);
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
            string path = string.Format("{0}\\audio\\{1}{2}", ProgramSettings.PathSounds, unconvertedSound.Name, ".wav");
            using (var reader = new MediaFoundationReader(unconvertedSound.Path))
            using (var resampler = new MediaFoundationResampler(reader, new WaveFormat(sampleRate, bits, channels)))
            {
                resampler.ResamplerQuality = 60;
                WaveFileWriter.CreateWaveFile(path, resampler);
            }

            File.Delete(unconvertedSound.Path);
            return new SoundNew(path);
        }

        [ImplementPropertyChanged]
        public class YoutubeDownloader
        {
            public delegate void StatusUpdateHandler(object sender, ProgressEventArgs e);
            public event StatusUpdateHandler OnUpdateStatus;


            public void DownloadVideo(string url)
            {
                // Download video
                var youTube = YouTube.Default;
                var video = youTube.GetVideo(url);

                // Save video
                var path = ProgramSettings.PathTemp + video.FullName;
                File.WriteAllBytes(path, video.GetBytes());
            }
            public void DownloadAudio(string url)
            {
                UpdateStatus("Downloading video...");

                // Download video
                var youTube = YouTube.Default;
                var video = youTube.GetVideo(url);

                // Save video
                var videoPath = ProgramSettings.PathTemp + video.FullName;
                File.WriteAllBytes(videoPath, video.GetBytes());


                Process myProcess = new Process();

                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = "ffmpeg.exe";
                myProcess.StartInfo.Arguments = string.Format("-y -i \"{0}\" \"{1}{2}.mp3\"", videoPath, ProgramSettings.PathNew, video.Title);
                myProcess.StartInfo.CreateNoWindow = false;
                myProcess.Start();

                UpdateStatus("Converting audio...");

                myProcess.WaitForExit();


                // Delete mp4
                File.Delete(videoPath);

                UpdateStatus("Download complete!");
            }

            public async void DownLoadAudioAsync(string url)
            {
                await Task.Run(() =>
                {
                    DownloadAudio(url);
                });
            }

            private void UpdateStatus(string status)
            {
                // Make sure someone is listening to event
                if (OnUpdateStatus == null) return;

                ProgressEventArgs args = new ProgressEventArgs(status);
                OnUpdateStatus(this, args);
            }
        }

        public class ProgressEventArgs : EventArgs
        {
            public string Status { get; private set; }
            public ProgressEventArgs(string status)
            {
                Status = status;
            }
        }
    }
}
