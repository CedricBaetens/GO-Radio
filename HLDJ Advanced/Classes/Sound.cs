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

        private int sampleRate = 16000;
        private int bits = 16;
        private int channels = 1;

        public Sound(NewSound newSound, string category, int id)
        {
            //// Resample
            //if (String.IsNullOrEmpty(newSound.Path))
            //{
            //    return;
            //}

            //var saveFile = SelectSaveFile("resampled");
            //if (saveFile == null)
            //{
            //    return;
            //}

            //// do the resample
            //using (var reader = new MediaFoundationReader(inputFile))
            //using (var resampler = new MediaFoundationResampler(reader, CreateOutputFormat(reader.WaveFormat)))
            //{
            //    WaveFileWriter.CreateWaveFile(saveFile, resampler);
            //}
            //MessageBox.Show("Resample complete");



            // Name
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
