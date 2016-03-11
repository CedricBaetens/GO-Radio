using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.IO;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundUnconverted
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Directory { get; set; }
        public string Extension { get; set; }


        public SoundUnconverted(string path)
        {
            FileInfo file = new FileInfo(path);

            Path = file.FullName;
            Directory = file.DirectoryName;

            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Extension = System.IO.Path.GetExtension(path);
        }
    }
}
