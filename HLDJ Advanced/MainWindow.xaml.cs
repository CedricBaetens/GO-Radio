using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        private const string hldjPath = @"C:\Users\Baellon\Desktop\HLDJ\audio";

        public ObservableCollection<Category> Categories { get; set; }


        public ObservableCollection<Sound> AllSounds { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void KeyboardHook_KeyboardEvent(KeyboardEvents kEvent, System.Windows.Forms.Keys key)
        {
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Instanciate
            Categories = new ObservableCollection<Category>();

            // Keyboard hool
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyboardEvent += KeyboardHook_KeyboardEvent;
            keyboardHook.InstallHook();

            // Read Json
            var jsonRead = File.ReadAllText("data_format.json");
            var jsonArray = JsonConvert.DeserializeObject<List<Sound>>(jsonRead);

            foreach (var jsonItem in jsonArray)
            {
                // Place item in correct category
                bool found = false;
                foreach (var category in Categories)
                {
                    if (category.Name == jsonItem.Category)
                    {
                        category.Sounds.Add(jsonItem);
                        found = true;
                    }
                }

                // Create category if its new
                if (!found)
                {
                    Categories.Add(new Category()
                    {
                        Name = jsonItem.Category
                    });
                    Categories[Categories.Count - 1].Sounds.Add(jsonItem);
                }
            }


            // Tree structure
            //AllSounds = new ObservableCollection<Sound>(Helper.GetAllSounds(hldjPath));
            //var json = JsonConvert.SerializeObject(AllSounds, Formatting.Indented);
            //File.WriteAllText("test.txt", json);



            //AllSounds = JsonConvert.DeserializeObject<List<Sound>>(jsonRead);



            int a = 0;
        }


    }
}
