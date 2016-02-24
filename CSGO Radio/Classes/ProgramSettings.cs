using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGO_Radio.Classes;
using System.Windows.Forms;
using PropertyChanged;

namespace CSGO_Radio
{
    /// <summary>
    /// Class that contains where the CS folder and sound folder is.
    /// </summary>
    [ImplementPropertyChanged]
    public static class ProgramSettings
    {
        // Properties
        public static string PathCsgo { get; set; }
        public static string PathSounds { get; set; }
        public static string PathTemp { get { return PathSounds + "\\audio\\tmp\\"; } }
        public static string PathVideo { get { return PathSounds + "\\audio\\tmp\\vid\\"; } }
        public static string PathNew { get { return PathSounds + "\\new\\"; } }


        // Public Methods
        public static void Init()
        {
            Load();
            Check();
        }
        public static void Save()
        {
            Properties.Settings.Default.PathCsgo = PathCsgo;
            Properties.Settings.Default.PathSounds = PathSounds;
            Properties.Settings.Default.Save();
        }

        // Private Methods
        private static void Load()
        {
            var set = Properties.Settings.Default;

            PathCsgo = set.PathCsgo;
            PathSounds = set.PathSounds;
        }
        private static void Check()
        {
            // CSGO Path
            if (string.IsNullOrEmpty(PathCsgo))
            {
                if (Directory.Exists(@"C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive"))
                {
                    PathCsgo = @"C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive";
                }
                else
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog()
                    {
                        Description = "Please select the csgo folder."
                    };
                    fbd.ShowDialog();
                    PathCsgo = fbd.SelectedPath;
                }

            }

            // Sound Path
            if (string.IsNullOrEmpty(PathSounds))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog()
                {
                    Description = "Please select a location where you want your sounds to be stored."
                };
                fbd.ShowDialog();
                string path = fbd.SelectedPath + "\\Sounds";
                PathSounds = path;
                CreateFolders();
            }


            CheckFolder(PathTemp);
            CheckFolder(PathVideo);
            CheckFolder(PathNew);         
        }  
        private static void CreateFolders()
        {
            Directory.CreateDirectory(PathSounds + "\\audio");
            Directory.CreateDirectory(PathSounds + "\\new");
        }  
        
        private static void CheckFolder(string url)
        {
            if (!Directory.Exists(url))
            {
                Directory.CreateDirectory(url);
            }
        }              
    }
}
