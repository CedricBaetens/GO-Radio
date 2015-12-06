using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDJ_Advanced
{
    public class Sound
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string FullPath { get; set; }
        public string Path { get; set; }

        public string Category { get; set; }

        public Sound(string fullpath, string rootPath)
        {
            FullPath = fullpath;
            Path = fullpath.Replace(rootPath + "\\", "");

            var split = fullpath.Replace(rootPath + "\\", "").Split('\\').Last().Replace(".wav", "").Split('_');

            Id = split[0];
            Name = string.Join(" ", split.Skip(1));

        }
    }
}
