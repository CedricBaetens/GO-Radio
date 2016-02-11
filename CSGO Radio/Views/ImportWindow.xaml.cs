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
using VideoLibrary;
using YoutubeExtractor;

namespace CSGO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class ImportWindow : Window
    {
        // Public
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<SoundUnconverted> Sounds { get; set; }
        public AudioHelper.YoutubeDownloader YtDownloader { get; set; }
        public string Message { get; set; }

        // Constructor
        public ImportWindow(ObservableCollection<Category> categories)
        {
            InitializeComponent();

            // Instanciate
            Sounds = new ObservableCollection<SoundUnconverted>();
            YtDownloader = new AudioHelper.YoutubeDownloader();
            YtDownloader.OnUpdateStatus += YtDownloader_OnUpdateStatus;

            this.Categories = categories;

            // Binding
            DataContext = this;
        }

        private void YtDownloader_OnUpdateStatus(object sender, AudioHelper.ProgressEventArgs e)
        {
            Message = e.Status;
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

                var convSound = AudioHelper.Convert(newSound);

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

        private void btnYt_Click(object sender, RoutedEventArgs e)
        {
            string link = @"https://www.youtube.com/watch?v=acHKPu4oIro";

            YtDownloader.DownLoadAudioAsync(link);

            Sounds = GetNewSounds();

            int a = 0;
        }

    }
}
