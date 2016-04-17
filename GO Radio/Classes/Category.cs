using GO_Radio.Views;
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

namespace GO_Radio.Classes
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
        public int EndId { get { return (StartId + Size)-1; } }
        public string Range { get { return string.Format("({0} - {1})", StartId.ToString("0000"), EndId.ToString("0000")); } }

        [DataMember]
        public int Size { get; set; } = 100;

        [DataMember]
        public ObservableCollection<SoundNew> Sounds { get; set; }

        public SoundNew SelectedSound { get; set; }

        public CategoryList Parent { get; set; }

        public bool IsFull { get { return Sounds.Count >= Size ? true : false; } }

        // Constructor
        public Category()
        {
            Sounds = new ObservableCollection<SoundNew>();
        }

        // Public methods
        public void AddSound(SoundNew sound)
        {
            /* CHECK IF NUMBERS FOLLOW IN CATEGORY, VOODOO DO NOT TOUCH. IT MIGHT BITE  */

            // First part is to determend if category is empty or not.
            var lastId = StartId-1;
            if (Sounds.Count > 0)
            {
                lastId = Sounds.Last().Id;
            }

            // Used to calculate next id when items folow
            if ((lastId - StartId + 1) == Sounds.Count)
            {
                var nextId = Sounds.Count + StartId;
                sound.Id = nextId;
            }

            // Items do not folow id's
            else
            {
                if (MessageBox.Show("Category has a missing sound. Do you want to fill this?", "Missing sound", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    sound.Id = FindMissingId();
                }
                else
                {
                    sound.Id = Sounds.Last().Id++;
                }
            }

            // Add sound to list and sort it.
            Sounds.Add(sound);
            Sounds = new ObservableCollection<SoundNew>(Sounds.OrderBy(o => o.Id).ToList());
        }
        public void MoveSound(Category category)
        {
            var sound = SelectedSound;
            Sounds.Remove(sound);

            category.AddSound(sound);
            Parent.UpdateDictionary();
        }
        public void Edit(Category cat)
        {
            Name = cat.Name;
            StartId = cat.StartId;
            Size = cat.StartId;
        }
        public void Remove()
        {
            if(MessageBox.Show(string.Format("Are you sure you want to delete this categoy including {0} sounds?",Sounds.Count), "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var sound in Sounds)
                {
                    sound.Remove();
                }
                Parent.Remove(this);
            }
        }

        public Category Clone()
        {
            return new Category() { Name = this.Name, Size = this.Size, StartId = this.StartId, Sounds = this.Sounds };
        }

        public void RecalculateIds()
        {
            var id = StartId;
            foreach (var sound in Sounds)
            {
                sound.Id = id++;
            }
            Parent.UpdateDictionary();
        }


        // Private methods
        private int FindMissingId()
        {
            int prevId = StartId -1;
            foreach (var sound in Sounds)
            {
                if (sound.Id - 1 == prevId)
                {
                    prevId = sound.Id;
                }
                else
                {
                    return sound.Id - 1;
                }
            }

            return -1;
        }

        // Command Binding
        public ICommand CommandRemoveSound => new RelayCommand(RemoveSound);
        public ICommand CommandTrimSound => new RelayCommand(TrimSound);
        public ICommand CommandRemoveTrim => new RelayCommand(RemoveTrim);
        public ICommand CommandMove => new RelayCommand(MoveSound);
        public ICommand CommandEdit => new RelayCommand(Edit);
        public ICommand CommandRemove => new RelayCommand(Remove);


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
            //SelectedSound.RemoveTrim();
        }
        private void MoveSound()
        {
            MoveSoundWindow ecw = new MoveSoundWindow(this);
            ecw.ShowDialog();
        }

        private void Edit()
        {
            EditCategoryWindow ecw = new EditCategoryWindow(this);
            ecw.ShowDialog();

        }
    }
}
