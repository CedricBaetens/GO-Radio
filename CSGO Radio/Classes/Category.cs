using CSGO_Radio.Views;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CSGO_Radio.Classes
{
    [ImplementPropertyChanged]
    [DataContract]
    public class Category
    {
        // Properties
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int StartId { get; set; }

        [DataMember]
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
        private void EditCategory()
        {
            CategoryWindow cw = new CategoryWindow();
            cw.SelectedCategory = this;
            cw.ShowDialog();
        }


        private void TrimSound()
        {
            NAudioHelper.TrimWavFile(SelectedSound.Path, string.Format("{0}/{1}(trimmed).wav", SelectedSound.Directory, SelectedSound.Name), new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 5));
        }


        // Command Binding
        public ICommand CommandRemoveSound { get { return new RelayCommand(RemoveSound); } }
        public ICommand CommandRecalculateIds { get { return new RelayCommand(RecalculateIds); } }
        public ICommand CommandEditCategory { get { return new RelayCommand(EditCategory); } }
        public ICommand CommandTrimSound { get { return new RelayCommand(TrimSound); } }


    }
}
