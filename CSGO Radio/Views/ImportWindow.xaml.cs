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

        private void btnYt_Click(object sender, RoutedEventArgs e)
        {
            string link = @"https://www.youtube.com/watch?v=acHKPu4oIro";

            var youTube = YouTube.Default; // starting point for YouTube actions

            var video = youTube.GetVideo(link); // gets a Video object with info about the video

            var newByte = ExtractAudio(new MemoryStream(video.GetBytes()));

            int a = 0;
            File.WriteAllBytes(@"C:\Users\Baellon\Downloads\test\" + video.FullName, video.GetBytes());
        }

        protected byte[] ExtractAudio(Stream stream)
        {
            var reader = new BinaryReader(stream);

            // Is stream a Flash Video stream
            if (reader.ReadChar() != 'F' || reader.ReadChar() != 'L' || reader.ReadChar() != 'V')
                throw new IOException("The file is not a FLV file.");

            // Is audio stream exists in the video stream
            var version = reader.ReadByte();
            var exists = reader.ReadByte();

            if ((exists != 5) && (exists != 4))
                throw new IOException("No Audio Stream");

            reader.ReadInt32(); // data offset of header. ignoring

            var output = new List<byte>();

            while (true)
            {
                try
                {
                    reader.ReadInt32(); // PreviousTagSize0 skipping

                    var tagType = reader.ReadByte();

                    while (tagType != 8)
                    {
                        var skip = ReadNext3Bytes(reader) + 11;
                        reader.BaseStream.Position += skip;

                        tagType = reader.ReadByte();
                    }

                    var DataSize = ReadNext3Bytes(reader);

                    reader.ReadInt32(); //skip timestamps 
                    ReadNext3Bytes(reader); // skip streamID
                    reader.ReadByte(); // skip audio header

                    for (int i = 0; i < DataSize - 1; i++)
                        output.Add(reader.ReadByte());
                }
                catch
                {
                    break;
                }
            }

            return output.ToArray();
        }

        private long ReadNext3Bytes(BinaryReader reader)
        {
            try
            {
                return Math.Abs((reader.ReadByte() & 0xFF) * 256 * 256 + (reader.ReadByte() & 0xFF)
                    * 256 + (reader.ReadByte() & 0xFF));
            }
            catch
            {
                return 0;
            }
        }
    }
}
