using CSGO_Radio.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSGO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundControllerNew
    {
        public ObservableDictionary<string,SoundNew> Sounds { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public SoundControllerNew()
        {
            Sounds = new ObservableDictionary<string, SoundNew>();

            Categories = new ObservableCollection<Category>();
        }

        private ObservableDictionary<string, SoundNew> GetDirectory()
        {
            foreach (var cat in Categories)
            {
                var d3 = d1.Concat(d2).ToDictionary(x => x.Key, x => x.Value);
                Sounds = new ObservableDictionary<string, SoundNew>(cat.Sounds + );
            }
        }

        // Command
        public ICommand CommandAddCategory { get { return new RelayCommand(ShowCategoryWindow); } }
        public ICommand CommandAddSound { get { return new RelayCommand(ShowSoundWindow); } }
        private void ShowCategoryWindow()
        {
            AddCategoryWindow acw = new AddCategoryWindow(Categories);
            acw.ShowDialog();
        }
        private void ShowSoundWindow()
        {
            ImportWindow iw = new ImportWindow(Categories);
            iw.ShowDialog();
        }
    }
}
