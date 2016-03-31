using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GO_Radio.Classes;
using System.Windows.Forms;
using PropertyChanged;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace GO_Radio
{
    /// <summary>
    /// Class that contains where the CS folder and sound folder is.
    /// </summary>
    [ImplementPropertyChanged]
    [DataContract]
    public class ProgramSettings : ILoadSave
    {
        // Singleton Patern
        private static ProgramSettings instance;
        private ProgramSettings()
        {
        }
        public static ProgramSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProgramSettings();
                }
                return instance;
            }
        }

        // Properties
        [DataMember]
        public string PathCsgo { get; set; }
        [DataMember]
        public string PathSounds { get; set; }
        public string PathTemp { get { return PathSounds + "\\audio\\tmp\\"; } }
        public string PathVideo { get { return PathSounds + "\\audio\\tmp\\vid\\"; } }
        public string PathNew { get { return PathSounds + "\\new\\"; } }
        public string AppFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GO Radio"); } }

        // Public Methods
        public void Save()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            try
            {
                File.WriteAllText(AppFolder + "\\settings.json", json);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Error writing settings.");
            }
        }
        public void Load()
        {
            string loc = Path.Combine(AppFolder, "settings.json");
            if (File.Exists(loc))
            {
                string json = File.ReadAllText(loc);
                instance = JsonConvert.DeserializeObject<ProgramSettings>(json);
            }

            Check();
        }

        // Private Methods
        private void Check()
        {
            // CSGO Path
            if (string.IsNullOrEmpty(Instance.PathCsgo))
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


                    if (!string.IsNullOrEmpty(fbd.SelectedPath))
                    {
                        Instance.PathCsgo = fbd.SelectedPath;
                    }
                    else
                    {
                        Environment.Exit(1);
                    }
                }

            }

            // Sound Path
            if (string.IsNullOrEmpty(Instance.PathSounds))
            {
                    FolderBrowserDialog fbd = new FolderBrowserDialog()
                    {
                        Description = "Please select a location where you want your sounds to be stored."
                    };
                    fbd.ShowDialog();

                    if (!string.IsNullOrEmpty(fbd.SelectedPath))
                    {
                        string path = fbd.SelectedPath + "\\Sounds";
                        Instance.PathSounds = path;
                        CreateFolders();
                    }
                    else
                    {
                        Environment.Exit(1);
                    }         
            }


            CheckFolder(Instance.PathTemp);
            CheckFolder(Instance.PathVideo);
            CheckFolder(Instance.PathNew);
            CheckFolder(Instance.AppFolder);
        }  
        private void CreateFolders()
        {
            Directory.CreateDirectory(PathSounds + "\\audio");
            Directory.CreateDirectory(PathSounds + "\\new");
        }        
        private void CheckFolder(string url)
        {
            if (!Directory.Exists(url))
            {
                Directory.CreateDirectory(url);
            }
        }              
    }
}
