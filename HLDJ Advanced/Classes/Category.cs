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

        public bool HasData
        {
            get
            {
                if (Sounds.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }


        public ObservableCollection<SoundWAV> Sounds { get; set; }

        public Category()
        {
            Sounds = new ObservableCollection<SoundWAV>();
        }

        public int GetNextId()
        {
            var nextId = Sounds.Count + StartId;
            return nextId;
        }
    }
}
