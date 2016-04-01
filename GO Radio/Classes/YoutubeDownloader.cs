using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VideoLibrary;

namespace GO_Radio
{
    [ImplementPropertyChanged]
    public class YoutubeDownloader
    {
        // Properties
        public ObservableCollection<ItemToDownload> Queue { get; set; }

        // Variables
        bool isDownloading = false;
        int count = 0;
        Process ffmpegProces = new Process();

        // Constructor
        public YoutubeDownloader()
        {
            Queue = new ObservableCollection<ItemToDownload>();
        }

        // Public Methods
        public void DownloadAudioList(string url)
        {
            var vid = GetVideoInfo(url);
            if (vid != null)
            {
                var item = new ItemToDownload(vid, url);
                Queue.Add(item);

                if (!isDownloading)
                {
                    DownLoadAudioAsync(Queue[count]);
                }
            }
            else
            {
                MessageBox.Show("Invallid URL.");
            }
        }
        public bool IsDone()
        {
            return !isDownloading;
        }
        public void End()
        {
            try
            {
                Queue.Clear();

                EmptyVideoFolder();

                ffmpegProces.Kill();
            }
            catch (Exception)
            {
                // Catch
            }
            
        }

        // Private methods
        private void DownloadVideo(string url)
        {
            _downloadVideo(url);
        }
        private void DownloadAudio(ItemToDownload item)
        {
            try
            {
                // Download video
                item.CurrentState = ItemToDownload.Status.DOWNLOADING;
                var vid = _downloadVideo(item.Url);

                // Extract audio
                item.CurrentState = ItemToDownload.Status.CONVERTING;
                _extractAudio(vid);

                // Remove video
                File.Delete(vid.Path);

                item.CurrentState = ItemToDownload.Status.DONE;
                FireConvertionComplete();
            }
            catch (Exception)
            {
                item.CurrentState = ItemToDownload.Status.FAILED;
            }
        }
        private DownloadedVideo _downloadVideo(string url)
        {
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
            var path = ProgramSettings.Instance.PathVideo + video.FullName;
            File.WriteAllBytes(path, video.GetBytes());

            return new DownloadedVideo() { Path = path, Name = video.Title };


        }
        private void _extractAudio(DownloadedVideo video)
        {
            // Launch ffmpg.exe
            
            ffmpegProces.StartInfo.UseShellExecute = false;
            ffmpegProces.StartInfo.FileName = "ffmpeg.exe";
            ffmpegProces.StartInfo.Arguments = string.Format("-y -i \"{0}\" \"{1}{2}.mp3\"", video.Path, ProgramSettings.Instance.PathNew, ReplaceInvalidChar(video.Name));
            ffmpegProces.StartInfo.CreateNoWindow = true;
            ffmpegProces.Start();
            ffmpegProces.WaitForExit();
        }
        private async void DownLoadAudioAsync(ItemToDownload item)
        {
            isDownloading = true;
            await Task.Run(() =>
            {
                DownloadAudio(item);
            });
            isDownloading = false;

            // Complete
            count++;
            if (count < Queue.Count)
            {
                DownLoadAudioAsync(Queue[count]);
            }
        }
        private YouTubeVideo GetVideoInfo(string url)
        {
            try
            {
                // Download video
                var youTube = YouTube.Default;

                //var video = youTube.GetVideo(url);

                var list = youTube.GetAllVideos(url).ToList();
                var best = list
                    .Where(info => info.Format == VideoFormat.Mp4)
    .OrderByDescending(info => info.AudioBitrate)
    .First();
                var video = best;

                return video;
            }
            catch (Exception)
            {
                return null;
            }

        }
        private string ReplaceInvalidChar(string input)
        {
            var invalidChars = Path.GetInvalidFileNameChars();

            string invalidCharsRemoved = new string(input
            .Where(x => !invalidChars.Contains(x))
            .ToArray());

            return invalidCharsRemoved;
        }
        private void EmptyVideoFolder()
        {
            // Clear vid folder
            DirectoryInfo dir = new DirectoryInfo(ProgramSettings.Instance.PathVideo);
            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }
        }

        // Classes
        [ImplementPropertyChanged]
        public class ItemToDownload
        {
            public enum Status
            {
                [Description("Standby")]
                STANDBY,

                [Description("Downloading")]
                DOWNLOADING,

                [Description("Converting")]
                CONVERTING,

                [Description("Done")]
                DONE,

                [Description("Failed")]
                FAILED
            }

            public string Name { get; set; }
            public string Url { get; set; }
            public Status CurrentState { get; set; }
            public string StateString { get { return GetEnumDescription((Status)CurrentState); } }

            public static string GetEnumDescription(Enum value)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }

            public ItemToDownload(YouTubeVideo video, string url)
            {
                Url = url;
                Name = video.Title;
                CurrentState = Status.STANDBY;
            }
        }
        private class DownloadedVideo
        {
            public string Path { get; set; }
            public string Name { get; set; }
        }

        // Eventhandler    
        public delegate void StatusUpdateHandler(object sender, ProgressEventArgs e);
        public event StatusUpdateHandler ConvertionComplete;
        private void FireConvertionComplete()
        {
            // Make sure someone is listening to event
            if (ConvertionComplete == null) return;

            ProgressEventArgs args = new ProgressEventArgs();
            ConvertionComplete(this, args);
        }
        public class ProgressEventArgs : EventArgs
        {
        }
    }
}
