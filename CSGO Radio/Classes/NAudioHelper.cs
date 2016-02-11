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
            public enum States
            {
                IDLE,
                DOWNLOADING_VIDEO,
                DOWNLOAD_COMPLETE,
                CONVERTING_AUDIO,
                CONVERTION_COMPLETE
            }
            // Used for event listenere
            public delegate void StatusUpdateHandler(object sender, ProgressEventArgs e);
            public event StatusUpdateHandler OnUpdateStatus;

            // Sync Methods
            public void DownloadVideo(string url)
            {
                _downloadVideo(url);

                UpdateStatus(States.DOWNLOAD_COMPLETE);
            }
            public void DownloadAudio(string url)
            {
                // Download video
                var vid = _downloadVideo(url);

                // Extract audio
                _extractAudio(vid);
                
                // Remove video
                File.Delete(vid.Path);

                UpdateStatus(States.CONVERTION_COMPLETE);
            }

            // Async Methods
            public async void DownLoadAudioAsync(string url)
            {
                await Task.Run(() =>
                {
                    DownloadAudio(url);
                });
            }
            public async void DownloadVideoAsync(string url)
            {
                await Task.Run(() =>
                {
                    DownloadVideo(url);
                });
            }

            // Private methods
            private DownloadedVideo _downloadVideo(string url)
            {
                UpdateStatus(States.DOWNLOADING_VIDEO);

                // Download video
                var youTube = YouTube.Default;

                //var video = youTube.GetVideo(url);
                
                var list = youTube.GetAllVideos(url).ToList();
                var best = list
                    .Where(info => info.Format == VideoFormat.Mp4)
    .OrderByDescending(info => info.AudioBitrate)
    .First();
                var video = best;

                // Save video
                var path = ProgramSettings.PathTemp + video.FullName;
                File.WriteAllBytes(path, video.GetBytes());

                UpdateStatus(States.IDLE);

                return new DownloadedVideo() { Path = path, Name = video.Title };

                
            }
            private void _extractAudio(DownloadedVideo video)
            {
                UpdateStatus(States.CONVERTING_AUDIO);

                // Launch ffmpg.exe
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = "ffmpeg.exe";
                myProcess.StartInfo.Arguments = string.Format("-y -i \"{0}\" \"{1}{2}.mp3\"", video.Path, ProgramSettings.PathNew, video.Name);
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.WaitForExit();

                UpdateStatus(States.IDLE);
            }

            // Event methods          
            private void UpdateStatus(States newState)
            {
                // Make sure someone is listening to event
                if (OnUpdateStatus == null) return;

                ProgressEventArgs args = new ProgressEventArgs(newState);
                OnUpdateStatus(this, args);
            }

            // Interused classes
            private class DownloadedVideo
            {
                public string Path { get; set; }
                public string Name { get; set; }
            }
            public class ProgressEventArgs : EventArgs
            {
                public string Message { get; private set; }

                public bool Done { get; set; }

                public ProgressEventArgs(States newState)
                {
                    switch (newState)
                    {
                        case States.IDLE:
                            Message = "";
                            break;
                        case States.DOWNLOADING_VIDEO:
                            Message = "Downloading video";
                            Done = false;
                            break;
                        case States.DOWNLOAD_COMPLETE:
                            Done = true;
                            Message = "Download complete";
                            break;
                        case States.CONVERTING_AUDIO:
                            Done = false;
                            Message = "Converting audio";
                            break;
                        case States.CONVERTION_COMPLETE:
                            Done = true;
                            Message = "Convertion complete";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Created for event handler
        /// </summary>
        
    }
}
