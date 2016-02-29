﻿using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Add(Category cat)
        {
            cat.Parent = this;
            Categories.Add(cat);
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

    }
}
