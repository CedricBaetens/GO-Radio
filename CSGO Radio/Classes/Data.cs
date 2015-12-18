using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace HLDJ_Advanced.Classes
{
    [ImplementPropertyChanged]
    public class Data
    {
        public ObservableCollection<Category> Categories { get; set; }
        //public ProgramSettings Settings { get; set; }

        public Data()
        {
            Categories = new ObservableCollection<Category>();
            //Settings = new ProgramSettings();
        }
    }
}
