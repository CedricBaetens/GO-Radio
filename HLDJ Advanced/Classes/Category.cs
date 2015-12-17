using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HLDJ_Advanced.Classes
{
    [ImplementPropertyChanged]
    public class Category
    {
        // Properties
        public string Name { get; set; }
        public int StartId { get; set; }
        public ObservableCollection<SoundWAV> Sounds { get; set; }
        public SoundWAV SelectedSound { get; set; }

        // Constructor
        public Category()
        {
            Sounds = new ObservableCollection<SoundWAV>();
        }

        // Public Methods
        public int GetNextId()
        {
            var nextId = Sounds.Count + StartId;
            return nextId;
        }

        // Private Methods
        private void RemoveSound()
        {
            Sounds.Remove(SelectedSound);
        }
        private void RecalculateIds()
        {
            for (int i = 0; i < Sounds.Count; i++)
            {
                Sounds[i].Id = StartId + i;
            }
        }

        // Command Binding
        public ICommand CommandRemoveSound { get { return new RelayCommand(RemoveSound); } }
        public ICommand CommandRecalculateIds { get { return new RelayCommand(RecalculateIds); } }
 
    }
}
