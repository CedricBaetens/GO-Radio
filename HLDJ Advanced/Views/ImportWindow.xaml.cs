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
using Microsoft.Win32;
using NAudio.Wave;
using System.IO;

namespace HLDJ_Advanced.Views
{
    [ImplementPropertyChanged]
    public partial class ImportWindow : Window
    {
        // Public
        public Data Data { get; set; }
        public ObservableCollection<SoundMP3> Sounds { get; set; }

        // Private
        private int sampleRate = 22050;
        private int bits = 16;
        private int channels = 1;

        public ImportWindow()
        {
            InitializeComponent();

            // Instanciate
            Sounds = new ObservableCollection<SoundMP3>();

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
            List<SoundMP3> selectedItems = new List<SoundMP3>(lvNewSongs.SelectedItems.Cast<SoundMP3>());
            Category selectedCategory = (Category)cbCategories.SelectedItem;

            for (int i = 0; i < selectedItems.Count; i++)
            {
                SoundMP3 newSound = (SoundMP3)selectedItems[i];

                // ReSample
                string path = string.Format("{0}\\audio\\{1}{2}", ProgramSettings.PathSounds, newSound.Name, ".wav");
                using (var reader = new MediaFoundationReader(newSound.Path))
                using (var resampler = new MediaFoundationResampler(reader, new WaveFormat(sampleRate, bits, channels)))
                {
                    resampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(path, resampler);
                }

                Sounds.RemoveAt(Sounds.IndexOf(newSound));
                File.Delete(newSound.Path);

                selectedCategory.Sounds.Add(new SoundWAV(path, selectedCategory.GetNextId()));

            }

            if (Sounds.Count == 0)
            {
                Close();
            }
        }

        // Custom methods
        private ObservableCollection<SoundMP3> GetNewSounds()
        {
            string[] newSoundsStrings = System.IO.Directory.GetFiles(ProgramSettings.PathSounds + "\\new", "*.*", System.IO.SearchOption.AllDirectories);

            return
                new ObservableCollection<SoundMP3>(newSoundsStrings.Select(newSound => new SoundMP3(newSound)).ToList());
        }       
    }
}
