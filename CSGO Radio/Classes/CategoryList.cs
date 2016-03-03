using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSGO_Radio.Classes
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

        public void Import(ObservableCollection<Category> Categories)
        {
            foreach (var item in Categories)
            {
                Add(item);
            }
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
    }
}
