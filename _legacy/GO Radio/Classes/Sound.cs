using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class Sound
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Directory { get; set; }
        public string Extension { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TrimStart { get; set; }
        public TimeSpan TrimEnd { get; set; }
        public float Volume { get; set; } = 100;

        public Sound(string path, int id = -1)
        {
            FileInfo file = new FileInfo(path);

            Path = file.FullName;
            Directory = file.DirectoryName;

            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Extension = System.IO.Path.GetExtension(path);

            Date = DateTime.Now;
        }

        public Sound()
        {
                
        }

        public void Remove()
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }

        public void RemoveTrim()
        {
            TrimStart = new TimeSpan();
            TrimEnd = new TimeSpan();
        }

        public void Pauze(TimeSpan time)
        {
            var outPath = string.Format("{0}\\tmp\\{1} pauzed.wav", Directory, Name);
            AudioHelper.TrimWavFile(Path, outPath, time, new TimeSpan(0,0,0,0));
            Path = outPath;
        }
    }
}
