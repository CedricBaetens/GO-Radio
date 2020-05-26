using GO_Radio.Classes;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace GO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class ImportWindow : Window
    {
        // Public
        public CategoryList Data { get; set; }
        public ObservableCollection<Sound> NewSounds { get; set; }
        public string YoutubeUrl { get; set; }
        public bool HasSounds { get { return NewSounds.Count > 0 ? true : false; } }

        // Constructor
        public ImportWindow(CategoryList data)
        {
            InitializeComponent();

            // Properites
            this.Data = data;

            // Instanciate
            NewSounds = new ObservableCollection<Sound>();

            // Binding
            DataContext = this;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (!YtDownloader.IsDone())
            //{
            //    if (MessageBox.Show("Download still in progress. Do you want to cancel all remaining downloads?", "Abort", MessageBoxButton.YesNo) == MessageBoxResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //}
            //YtDownloader.End();
            Data.UpdateDictionary();
        }

        // Buttons
        public ICommand CommandSelectAll => new RelayCommand(SelectAll);
        public ICommand CommandDeselectAll => new RelayCommand(DeselectAll);
        public ICommand CommandDelete => new RelayCommand(Delete);
        public ICommand CommandAdd => new RelayCommand(Add);
        //public ICommand CommandYoutube => new RelayCommand(Youtube);
        public ICommand CommandImport => new RelayCommand(Import);

        private void SelectAll()
        {
            lvNewSongs.SelectAll();
        }
        private void DeselectAll()
        {
            lvNewSongs.UnselectAll();
        }
        private void Add()
        {
            //Copy selected Items List
            List<Sound> selectedItems = new List<Sound>(lvNewSongs.SelectedItems.Cast<Sound>());
            Category selectedCategory = (Category)cbCategories.SelectedItem;

            for (int i = 0; i < selectedItems.Count; i++)
            {

                if (!selectedCategory.IsFull)
                {
                    Sound newSound = (Sound)selectedItems[i];
                    selectedCategory.AddSound(newSound);
                    NewSounds.RemoveAt(NewSounds.IndexOf(newSound));

                }

                else
                {
                    System.Windows.MessageBox.Show("Category is full!");
                }
            }
            if (NewSounds.Count == 0)
            {
                Close();
            }
        }
        private void Delete()
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete all the selected songs?", "Delete!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //Copy selected Items List
                List<Sound> selectedItems = new List<Sound>(lvNewSongs.SelectedItems.Cast<Sound>());

                for (int i = 0; i < selectedItems.Count; i++)
                {
                    Sound newSound = (Sound)selectedItems[i];
                    File.Delete(newSound.Path);
                    NewSounds.RemoveAt(NewSounds.IndexOf(newSound));
                }
            }
            //NewSounds = GetNewSounds();
        }
        //private void Youtube()
        //{
        //    if (!string.IsNullOrEmpty(YoutubeUrl))
        //    {
        //        YtDownloader.DownloadAudioList(YoutubeUrl);
        //    }
        //}
        private void Import()
        {
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                //Filter = "Database files (*.mdb, *.accdb)|*.mdb;*.accdb",
                FilterIndex = 0,
                RestoreDirectory = true,
                Multiselect = true
            };

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var fileName in openFileDialog1.FileNames)
                {
                    NewSounds.Add(new Sound(fileName));
                }
            }
        }

        // Custom methods
        //private ObservableCollection<SoundUnconverted> GetNewSounds()
        //{
        //    string[] newSoundsStrings = System.IO.Directory.GetFiles(Data.Path + "\\new", "*.*", System.IO.SearchOption.AllDirectories);

        //    return
        //        new ObservableCollection<SoundUnconverted>(newSoundsStrings.Select(newSound => new SoundUnconverted(newSound)).ToList());
        //}  
    }
}
