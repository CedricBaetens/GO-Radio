using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HLDJ_Advanced.Views;
using Kennedy.ManagedHooks;
using Newtonsoft.Json;
using PropertyChanged;

namespace HLDJ_Advanced
{
    /// 
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {
        private KeyboardHook keyboardHook;

        public ObservableCollection<Sound> Sounds { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Instance
            Sounds = new ObservableCollection<Sound>();

            // Binding
            DataContext = this;
        }

        private void KeyboardHook_KeyboardEvent(KeyboardEvents kEvent, System.Windows.Forms.Keys key)
        {
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("data.json"))
            {
                string json = File.ReadAllText("data.json");
                Sounds = JsonConvert.DeserializeObject<ObservableCollection<Sound>>(json);
            }

            //Sounds.Add(new Sound());
            //// Keyboard hool
            //keyboardHook = new KeyboardHook();
            //keyboardHook.KeyboardEvent += KeyboardHook_KeyboardEvent;
            //keyboardHook.InstallHook();

            //// Read Json
            //var jsonRead = File.ReadAllText("data_format.json");
            //var jsonArray = JsonConvert.DeserializeObject<List<Sound>>(jsonRead);

            //foreach (var jsonItem in jsonArray)
            //{
            //    // Place item in correct category
            //    bool found = false;
            //    foreach (var category in Categories)
            //    {
            //        if (category.Name == jsonItem.Category)
            //        {
            //            category.Sounds.Add(jsonItem);
            //            found = true;
            //        }
            //    }

            //    // Create category if its new
            //    if (!found)
            //    {
            //        Categories.Add(new Category()
            //        {
            //            Name = jsonItem.Category
            //        });
            //        Categories[Categories.Count - 1].Sounds.Add(jsonItem);
            //    }
            //}


            //// Tree structure
            ////Sounds = new ObservableCollection<Sound>(Helper.GetNewSounds(hldjPath));
            ////var json = JsonConvert.SerializeObject(Sounds, Formatting.Indented);
            ////File.WriteAllText("test.txt", json);



            ////Sounds = JsonConvert.DeserializeObject<List<Sound>>(jsonRead);



            //int a = 0;

            //CreateFolderStructure();
        }

        private void CreateFolderStructure()
        {
            string pathnewsong = string.Format(@"{0}\{1}", Helper.HldjPath, "new");
            Directory.CreateDirectory(pathnewsong);
            for (int i = 0; i < 10; i++)
            {
                string pathl0 = string.Format(@"{0}\{1}", Helper.HldjPath, i);
                Directory.CreateDirectory(pathl0);

                for (int j = 0; j < 10; j++)
                {
                    string pathl1 = string.Format(@"{0}\{1}", pathl0, j);
                    Directory.CreateDirectory(pathl1);

                    for (int k = 0; k < 10; k++)
                    {
                        string pathl2 = string.Format(@"{0}\{1}", pathl1, k);
                        Directory.CreateDirectory(pathl2);
                    }
                }
            }
        }

        private void BtnImport_OnClick(object sender, RoutedEventArgs e)
        {
            ImportWindow iw = new ImportWindow()
            {
                Sounds = Sounds
            };           
            iw.ShowDialog();

            Sounds = iw.Sounds;

            int a = 0;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            string json = JsonConvert.SerializeObject(Sounds);
            File.WriteAllText("data.json",json);
        }
    }
}
