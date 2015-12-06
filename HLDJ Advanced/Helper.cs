 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDJ_Advanced
{
    static class Helper
    {
        public static List<Sound> GetAllSounds(string path)
        {
            string[] rawSounds = System.IO.Directory.GetFiles(path, "*.*", System.IO.SearchOption.AllDirectories);

            List<Sound> sounds = rawSounds.Select(rawSound => new Sound(rawSound, path)).ToList();

            return sounds;
        }
    }
}
