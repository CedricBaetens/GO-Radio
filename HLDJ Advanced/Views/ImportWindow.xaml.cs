using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using HLDJ_Advanced.Classes;
using PropertyChanged;

namespace HLDJ_Advanced.Views
{
    [ImplementPropertyChanged]
    public partial class ImportWindow : Window
    {
        public ObservableCollection<Sound> Sounds { get; set; }
        public ObservableCollection<NewSound> NewSounds { get; set; }

        public List<string> Categories { get; set; }

        public ImportWindow()
        {
            InitializeComponent();

            // Instanciate
            NewSounds = new ObservableCollection<NewSound>();
            Categories = new List<string>()
            {
                "Song (Full)",
                "Song (Part)",
                "Game",
                "Greetings",
                "Silence",
                "MLG",
                "Sound"
            };

            // Binding
            DataContext = this;
        }

        private void ImportWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            NewSounds = GetNewSounds();
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            // Copy selected Items List
            List<NewSound> selectedItems = new List<NewSound>(lvNewSongs.SelectedItems.Cast<NewSound>());

            for (int i = 0; i < selectedItems.Count; i++)
            {
                // Selected Item
                NewSound newSound = (NewSound)selectedItems[i];

                // Get Original index
                int index = NewSounds.IndexOf(newSound);

                // Count active sounds
                int count = Sounds.Count();

                // Add newsound to soundlist
                Sounds.Add(new Sound(newSound, cbCategories.SelectedValue.ToString(), count));     
               
                // Remove newsound from list
                NewSounds.RemoveAt(index);
            }

            if (NewSounds.Count == 0)
            {
                Close();
            }
        }

        private ObservableCollection<NewSound> GetNewSounds()
        {
            string[] newSoundsStrings = System.IO.Directory.GetFiles(string.Format("{0}\\{1}", Helper.HldjPath, "new"), "*.*", System.IO.SearchOption.AllDirectories);

            return
                new ObservableCollection<NewSound>(newSoundsStrings.Select(newSound => new NewSound(newSound)).ToList());
        }
    }
}
