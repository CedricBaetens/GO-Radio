using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLDJ_Advanced.Classes;

namespace HLDJ_Advanced
{
    public class Sound
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Category { get; set; }

        public Sound(NewSound newSound, string category, int id)
        {
            Name = newSound.Name.Replace(".wav","");

            // ID conversion
            string idString = string.Format("{0:0000}", id);
            var idCharArray = idString.ToCharArray();
            Id = idString;

            // New Path
            var newPath = string.Format("{0}\\{1}\\{2}\\{3}\\{4}", 
                Helper.HldjPath, 
                idCharArray[0], 
                idCharArray[1], 
                idCharArray[2],
                string.Format("{0}_{1}.wav",idString,Name));
            File.Copy(newSound.Path, newPath);
            File.Delete(newSound.Path);
            Path = newPath;

            // Category
            Category = category;
        }

        public Sound()
        {
            Name = "test";
        }
    }
}
