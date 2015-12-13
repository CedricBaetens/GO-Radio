using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HLDJ_Advanced.Classes
{
    [ImplementPropertyChanged]
    public class Category
    {
        public string Name { get; set; }
        public int StartId { get; set; }

        public Visibility Show
        {
            get
            {
                if (Sounds.Count > 0)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }


        public ObservableCollection<SoundWAV> Sounds { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public Category()
        {
            Sounds = new ObservableCollection<SoundWAV>();
            Categories = new ObservableCollection<Category>();
        }

        public int GetNextId()
        {
            var nextId = Sounds.Count + StartId;
            return nextId;
        }
    }
}
