using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDJ_Advanced.Classes
{
    [ImplementPropertyChanged]
    public class Category
    {
        public string Name { get; set; }
        public int StartId { get; set; }
        public ObservableCollection<SoundWAV> Sounds { get; set; }

        public Category()
        {
            Sounds = new ObservableCollection<SoundWAV>();
        }
    }
}
