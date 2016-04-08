using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class CategoryList
    {
        public Dictionary<int, SoundNew> Sounds { get; set; }   // Used for easy finding of songs
        public ObservableCollection<Category> Categories { get; set; }

        public CategoryList()
        {
            Categories = new ObservableCollection<Category>();
            Sounds = new Dictionary<int, SoundNew>();
        }

        public bool Add(Category cat)
        {
            if (string.IsNullOrEmpty(cat.Name))
            {
                MessageBox.Show("Category name is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (IsValidRange(cat))
            {
                cat.Parent = this;
                Categories.Add(cat);
                return true;
            }
            else
            {

                MessageBox.Show("Invallid category range!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }        
        }
        public void Remove(Category cat)
        {
            Categories.Remove(cat);
        }
        public void UpdateDictionary()
        {
            Dictionary<int, SoundNew> dic = new Dictionary<int, SoundNew>();
            foreach (var cat in Categories)
            {
                foreach (var sound in cat.Sounds)
                {
                    dic.Add(sound.Id, sound);
                }
            }
            Sounds = dic;
        }

        public bool IsValidRange(Category newCat)
        {
            if (newCat.Sounds.Count > newCat.Size)
            {
                return false;
            }
            foreach (var cat in Categories)
            {
                if (newCat.Name != cat.Name)
                {
                    if (Math.Max(newCat.EndId, cat.EndId) - Math.Min(newCat.StartId, cat.StartId) < (newCat.EndId - newCat.StartId) + (cat.EndId - cat.StartId))
                    {
                        return false;
                    }
                }            
            }         
            return true;
        }

        public SoundNew GetSoundById(int id)
        {
            if (Sounds.ContainsKey(id))
            {
                return Sounds[id];
            }
            else
            {
                return null;
            }
        }
        public SoundNew GetSoundById(string id)
        {
            // Get Random Song
            if (id.Contains("+"))
            {
                var IdEnteredCharArr = id.Replace("+", "").ToCharArray();
                List<int> temp = new List<int>();
                foreach (var sound in Sounds)
                {
                    var soundCharArray = sound.Key.ToString("0000").ToCharArray();

                    bool keep = true;
                    for (int i = 0; i < IdEnteredCharArr.Length; i++)
                    {
                        if (soundCharArray[i] != IdEnteredCharArr[i])
                        {
                            keep = false;
                        }
                    }

                    if (keep)
                    {
                        temp.Add(sound.Key);
                    }
                }

                if (temp.Count > 0)
                {
                    Random rnd = new Random();
                    var random = rnd.Next(temp.Count);

                    return GetSoundById(temp[random]);
                }
                return null;
            }

            // Return sound
            return GetSoundById(Convert.ToInt32(id));           
        }

        // Interface Methods
        public void Load(string path)
        {
            path = path + "\\data.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
                UpdateDictionary();
            }
        }
        public void Save()
        {
            //string json = JsonConvert.SerializeObject(Categories, Formatting.Indented);

            //try
            //{
            //    File.WriteAllText(ProgramSettings.Instance.PathSounds + "\\data.json", json);
            //}
            //catch (Exception)
            //{
            //    System.Windows.MessageBox.Show("Error writing data, please make sure the sound folder exists.");
            //}
        }
    }
}
