using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLDJ_Advanced.Classes;
using PropertyChanged;

namespace HLDJ_Advanced
{
    [ImplementPropertyChanged]
    public class SoundWAV
    {
        public int Id { get; set; }
        public string IdFull { get { return Id.ToString("0000"); } }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Directory { get; set; }
        public string Extension { get; set; }
        public int LoadCount { get; set; }
        public DateTime Date { get; set; }

        public SoundWAV(string path, int id)
        {
            FileInfo file = new FileInfo(path);

            Path = file.FullName;
            Directory = file.DirectoryName;

            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Extension = System.IO.Path.GetExtension(path);

            Id = id;

            Date = DateTime.Now;
        }

        public SoundWAV()
        {

        }
    }
}
