using GO_Radio.Classes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class ImportWindow : Window
    {
        // Public
        public CategoryList Data { get; set; }
        public ObservableCollection<SoundUnconverted> NewSounds { get; set; }
        public YoutubeDownloader YtDownloader { get; set; }
        public string YoutubeUrl { get; set; }     
        public bool HasSounds { get { return NewSounds.Count > 0 ? true : false; }}
        
        // Constructor
        public ImportWindow(CategoryList data)
        {
            InitializeComponent();

            // Properites
            this.Data = data;

            // Instanciate
            NewSounds = new ObservableCollection<SoundUnconverted>();
            YtDownloader = new YoutubeDownloader(Data.Path);
            YtDownloader.ConvertionComplete += YtDownloader_ConvertionComplete;
                
            // Binding
            DataContext = this;
            lbQueue.DataContext = YtDownloader;
        }

        // Youtube
        private void YtDownloader_ConvertionComplete(object sender, YoutubeDownloader.ProgressEventArgs e)
        {
            NewSounds = GetNewSounds();
        }

        // Window events
        private void ImportWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            NewSounds = GetNewSounds();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!YtDownloader.IsDone())
            {
                if (MessageBox.Show("Download still in progress. Do you want to cancel all remaining downloads?", "Abort", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            YtDownloader.End();
            Data.UpdateDictionary();
        }

        // Buttons
        public ICommand CommandSelectAll => new RelayCommand(SelectAll);
        public ICommand CommandDeselectAll => new RelayCommand(DeselectAll);
        public ICommand CommandDelete => new RelayCommand(Delete);
        public ICommand CommandAdd => new RelayCommand(Add);
        public ICommand CommandYoutube => new RelayCommand(Youtube);

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
            List<SoundUnconverted> selectedItems = new List<SoundUnconverted>(lvNewSongs.SelectedItems.Cast<SoundUnconverted>());
            Category selectedCategory = (Category)cbCategories.SelectedItem;

            for (int i = 0; i < selectedItems.Count; i++)
            {
                
                if (!selectedCategory.IsFull)
                {
                    SoundUnconverted newSound = (SoundUnconverted)selectedItems[i];

                    var convSound = AudioHelper.Convert(newSound);

                    selectedCategory.AddSound(convSound);
                    NewSounds.RemoveAt(NewSounds.IndexOf(newSound));

                }

                else
                {
                    MessageBox.Show("Category is full!");
                }
            }

            if (NewSounds.Count == 0)
            {
                Close();
            }
        }
        private void Delete()
        {
            if (MessageBox.Show("Are you sure you want to delete all the selected songs?", "Delete!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //Copy selected Items List
                List<SoundUnconverted> selectedItems = new List<SoundUnconverted>(lvNewSongs.SelectedItems.Cast<SoundUnconverted>());

                for (int i = 0; i < selectedItems.Count; i++)
                {
                    SoundUnconverted newSound = (SoundUnconverted)selectedItems[i];
                    File.Delete(newSound.Path);
                    NewSounds.RemoveAt(NewSounds.IndexOf(newSound));
                }
            }
            NewSounds = GetNewSounds();
        }
        private void Youtube()
        {
            if (!string.IsNullOrEmpty(YoutubeUrl))
            {
                YtDownloader.DownloadAudioList(YoutubeUrl);
            }
        }

        // Custom methods
        private ObservableCollection<SoundUnconverted> GetNewSounds()
        {
            string[] newSoundsStrings = System.IO.Directory.GetFiles(Data.Path + "\\new", "*.*", System.IO.SearchOption.AllDirectories);

            return
                new ObservableCollection<SoundUnconverted>(newSoundsStrings.Select(newSound => new SoundUnconverted(newSound)).ToList());
        }  
    }
}
