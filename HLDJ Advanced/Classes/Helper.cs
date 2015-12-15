﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLDJ_Advanced.Classes;
using System.Windows.Forms;

namespace HLDJ_Advanced
{
    static class Helper
    {
        /*
        public static string PathCsgo = @"C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive\";
        public static string PathSounds = @"C:\Users\Baellon\Desktop\Songs\";*/

        public static string PathCsgo;
        public static string PathSounds;

        public static void Load()
        {
            var set = Properties.Settings.Default;

            // Load settings
            PathCsgo = set.PathCsgo;
            PathSounds = set.PathSounds;

            // Check values
            if (String.IsNullOrEmpty(PathCsgo))
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

            if (String.IsNullOrEmpty(PathSounds))
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
        }   
        public static void Save()
        {
            // Load settings
            Properties.Settings.Default.PathCsgo = PathCsgo;
            Properties.Settings.Default.PathSounds = PathSounds;
            Properties.Settings.Default.Save();
        }
        
        private static void CreateFolders()
        {
            Directory.CreateDirectory(PathSounds + "\\audio");
            Directory.CreateDirectory(PathSounds + "\\new");
        }                
    }
}
