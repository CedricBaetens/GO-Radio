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

namespace HLDJ_Advanced.Views
{
    [ImplementPropertyChanged]
    public partial class ImportWindow : Window
    {
        public ObservableCollection<Sound> Sounds { get; set; }
        public ObservableCollection<NewSound> NewSounds { get; set; }

        public List<string> Categories { get; set; }

        // NAudio
        private string inputFile;
        private string inputFileFormat;

        private int sampleRate = 16000;
        private int bits = 16;
        private int channels = 1;

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

        // Window events
        private void ImportWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            NewSounds = GetNewSounds();
        }

        // Button evens
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
        private void BtnConvertAll_Click(object sender, RoutedEventArgs e)
        {
            //foreach (var newSound in NewSounds)
            //{

            //}
            ////SelectInputFile();
            //TryOpenInputFile(inputFile);
            //Resample();
        }

        // Custom methods
        private ObservableCollection<NewSound> GetNewSounds()
        {
            string[] newSoundsStrings = System.IO.Directory.GetFiles(string.Format("{0}\\mp3", Helper.HldjPath), "*.*", System.IO.SearchOption.AllDirectories);

            return
                new ObservableCollection<NewSound>(newSoundsStrings.Select(newSound => new NewSound(newSound)).ToList());
        }

        private bool TryOpenInputFile(string file)
        {
            bool isValid = false;
            try
            {
                using (var reader = new MediaFoundationReader(file))
                {
                    inputFileFormat = reader.WaveFormat.ToString();
                    isValid = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("Not a supported input file ({0})", e.Message));
            }
            return isValid;
        }

        //private void SelectInputFile()
        //{
        //    var ofd = new OpenFileDialog();
        //    ofd.Filter = "Audio files|*.mp3;*.wav;*.wma;*.aiff;*.aac";
        //    if (ofd.ShowDialog() == true)
        //    {
        //        if (TryOpenInputFile(ofd.FileName))
        //        {
        //            inputFile = ofd.FileName;
        //        }
        //    }

        //    inputFile = @"C:\Users\Cedric Baetens\Desktop\temp\mp3\new";
        //}

        private string SelectSaveFile(string desc)
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = @"C:\Users\Cedric Baetens\Desktop\temp\mp3\new";
            sfd.Filter = "WAV File|*.wav";
            //return (sfd.ShowDialog() == true) ? new Uri(sfd.FileName).AbsoluteUri : null;
            return (sfd.ShowDialog() == true) ? sfd.FileName : null;
        }

        private void Resample()
        {
            if (String.IsNullOrEmpty(inputFile))
            {
                MessageBox.Show("Select a file first");
                return;
            }
            var saveFile = SelectSaveFile("resampled");
            if (saveFile == null)
            {
                return;
            }

            // do the resample
            using (var reader = new MediaFoundationReader(inputFile))
            using (var resampler = new MediaFoundationResampler(reader, CreateOutputFormat(reader.WaveFormat)))
            {
                WaveFileWriter.CreateWaveFile(saveFile, resampler);
            }
            MessageBox.Show("Resample complete");
        }

        private WaveFormat CreateOutputFormat(WaveFormat inputFormat)
        {
            WaveFormat waveFormat;
            waveFormat = new WaveFormat(sampleRate, bits, channels);
            return waveFormat;
        }

    }
}
