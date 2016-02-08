using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.IO;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;
using CSGO_Radio.Views;

namespace CSGO_Radio.Classes
{
    /// <summary>
    /// Class that contains all the diffrent categories with songs.
    /// </summary>
    [ImplementPropertyChanged]
    public class SoundController
    {
        // Properties
        public string IdEntered { get; set; } = "";
        public SoundWAV LoadedSound { get; set; }

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<KeyValuePair<string, SoundWAV>> SoundsList { get; set; }

        private Tts TextToSpeech;

        // Constructor
        public SoundController()
        {
            Categories = new ObservableCollection<Category>();
            SoundsList = new ObservableCollection<KeyValuePair<string, SoundWAV>>();
            TextToSpeech = new Tts();
        }

        // Public Functions
        public void Load()
        {
            if (File.Exists(ProgramSettings.PathSounds + "\\data.json"))
            {
                string json = File.ReadAllText(ProgramSettings.PathSounds + "\\data.json");
                Categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
                Sort();
                CategoriesToSoundList();
            }
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(Categories, Formatting.Indented);

            try
            {
                File.WriteAllText(ProgramSettings.PathSounds + "\\data.json", json);
            }
            catch (Exception)
            {
                MessageBox.Show("Error writing data, please make sure the sound folder exists.");
            }
        }
        public void LoadSong()
        {
            var sound = FindSoundById(IdEntered);
            if (sound != null)
            {
                if (File.Exists(ProgramSettings.PathCsgo + "\\voice_input.wav"))
                {
                    File.Delete(ProgramSettings.PathCsgo + "\\voice_input.wav");
                }
                File.Copy(sound.Path, ProgramSettings.PathCsgo + "\\voice_input.wav");

                IdEntered = "";
                LoadedSound = sound;
                //soundPlayer.SoundLocation = sound.Path;
                //soundPlayer.Load();
            }

            //if (IdEntered.Count() >= 4)
            //    IdEntered = "";
        }

        // Private Function
        private void Sort()
        {
            Categories = new ObservableCollection<Category>(Categories.OrderBy(s => s.Name).ToList());
        }
        private void CategoriesToSoundList()
        {
            SoundsList.Clear();
            foreach (var category in Categories)
            {
                foreach (var sound in category.Sounds)
                {
                    //if (sound.Name.Contains(""))
                    //{
                    //    SoundsList.Add(new KeyValuePair<string, SoundWAV>(category.Name, sound));
                    //}
                }
            }
        }
        private SoundWAV FindSoundById(string id)
        {
            SoundWAV foundSound = null;

            // Find item
            foreach (var category in Categories)
            {
                //foundSound = category.Sounds.Where(x => x.IdFull == id).FirstOrDefault();

                //if (foundSound != null)
                //{
                //    foundSound.LoadCount++;
                //    break;
                //}
            }

            // Return
            return foundSound;
        }

        // Window Events
        private void AddCategory()
        {
            //AddCategoryWindow acw = new AddCategoryWindow()
            //{
            //    //Categories = Categories
            //};
            //acw.ShowDialog();
            ////Categories = acw.Categories;

            int a = 0;
        }
        private void AddSound()
        {
            //ImportWindow iw = new ImportWindow()
            //{
            //    Categories = Categories
            //};
            //iw.ShowDialog();
            //Categories = iw.Categories;
        }

        // Command
        public ICommand CommandAddCategory { get { return new RelayCommand(AddCategory); } }
        public ICommand CommandAddSound { get { return new RelayCommand(AddSound); } }
    }
}
