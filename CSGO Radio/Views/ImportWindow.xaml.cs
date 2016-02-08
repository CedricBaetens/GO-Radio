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
using CSGO_Radio.Classes;
using PropertyChanged;
using Microsoft.Win32;
using NAudio.Wave;
using System.IO;

namespace CSGO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class ImportWindow : Window
    {
        // Public
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<SoundUnconverted> Sounds { get; set; }

        // Private
        private int sampleRate = 22050;
        private int bits = 16;
        private int channels = 1;

        public ImportWindow(ObservableCollection<Category> categories)
        {
            InitializeComponent();

            // Instanciate
            Sounds = new ObservableCollection<SoundUnconverted>();

            this.Categories = categories;

            // Binding
            DataContext = this;
        }

        // Window events
        private void ImportWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Sounds = GetNewSounds();
        }

        // Button events
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Copy selected Items List
            List<SoundUnconverted> selectedItems = new List<SoundUnconverted>(lvNewSongs.SelectedItems.Cast<SoundUnconverted>());
            Category selectedCategory = (Category)cbCategories.SelectedItem;

            for (int i = 0; i < selectedItems.Count; i++)
            {
                SoundUnconverted newSound = (SoundUnconverted)selectedItems[i];

                var convSound = NAudioHelper.Convert(newSound);

                Sounds.RemoveAt(Sounds.IndexOf(newSound));

                selectedCategory.AddSound(convSound);
            }

            if (Sounds.Count == 0)
            {
                Close();
            }
        }

        // Custom methods
        private ObservableCollection<SoundUnconverted> GetNewSounds()
        {
            string[] newSoundsStrings = System.IO.Directory.GetFiles(ProgramSettings.PathSounds + "\\new", "*.*", System.IO.SearchOption.AllDirectories);

            return
                new ObservableCollection<SoundUnconverted>(newSoundsStrings.Select(newSound => new SoundUnconverted(newSound)).ToList());
        }       
    }
}
