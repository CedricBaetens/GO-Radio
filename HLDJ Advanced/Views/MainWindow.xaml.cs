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
using HLDJ_Advanced.Classes;

namespace HLDJ_Advanced
{
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {
        private KeyboardHook keyboardHook;
        public ObservableCollection<Category> Categories { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Instance
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyboardEvent += KeyboardHook_KeyboardEvent;
            keyboardHook.InstallHook();

            Categories = new ObservableCollection<Category>();

            // Binding
            DataContext = this;
        }

        private void KeyboardHook_KeyboardEvent(KeyboardEvents kEvent, System.Windows.Forms.Keys key)
        {
            int a = 0;
        }



        // Window events
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("data.json"))
            {
                string json = File.ReadAllText("data.json");
                Categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
            }
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Save data
            string json = JsonConvert.SerializeObject(Categories, Formatting.Indented);
            File.WriteAllText("data.json", json);

            // Deinstal hook
            keyboardHook.UninstallHook();
        }

        // Menu Events
        private void miImport_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow iw = new ImportWindow()
            {
                Categories = this.Categories
            };
            iw.ShowDialog();

            Categories = iw.Categories;
        }

        private void miCreateFolders_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow acw = new AddCategoryWindow();
            acw.ShowDialog();

            Categories.Add(acw.Category);
        }
    }
}
