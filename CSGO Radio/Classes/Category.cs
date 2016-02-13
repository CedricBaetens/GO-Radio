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
        public ObservableCollection<SoundNew> Sounds { get; set; }

        public SoundNew SelectedSound { get; set; }

        // Constructor
        public Category()
        {
            Sounds = new ObservableCollection<SoundNew>();
        }

        public void AddSound(SoundNew sound)
        {
            var nextId = Sounds.Count + StartId;
            sound.Id = nextId;
            Sounds.Add(sound);
        }

        //// Command Binding
        public ICommand CommandRemoveSound => new RelayCommand(RemoveSound);
        public ICommand CommandTrimSound => new RelayCommand(TrimSound);
        public ICommand CommandRemoveTrim => new RelayCommand(RemoveTrim);

        private void RemoveSound()
        {
            Sounds.Remove(SelectedSound);
        }

        private void TrimSound()
        {
            TrimWindow tw = new TrimWindow(SelectedSound);
            tw.ShowDialog();
        }
        private void RemoveTrim()
        {
            SelectedSound.RemoveTrim();
        }

        //public ICommand CommandRecalculateIds { get { return new RelayCommand(RecalculateIds); } }
        //public ICommand CommandEditCategory { get { return new RelayCommand(EditCategory); } }
        //public ICommand CommandTrimSound { get { return new RelayCommand(TrimSound); } }


    }
}
