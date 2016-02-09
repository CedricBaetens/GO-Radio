using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundNew
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Directory { get; set; }
        public string Extension { get; set; }
        public DateTime Date { get; set; }

        public SoundNew(string path, int id = -1)
        {
            FileInfo file = new FileInfo(path);

            Path = file.FullName;
            Directory = file.DirectoryName;

            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Extension = System.IO.Path.GetExtension(path);

            Date = DateTime.Now;
        }

        public SoundNew()
        {
                
        }
    }
}
