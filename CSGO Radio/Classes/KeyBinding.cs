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
    public class KeyBinding
    {
        public enum KeyTranslation : int
        {
           K0 = 0,
           K1,
           K2,
           K3,
           K4,
           K5,
           K6,
           K7,
           K8,
           K9,
           PlayStop = 10,
           PlayPauze = 11,
           Random = 12
        }

        public ObservableCollection<KeyValuePair<KeyTranslation,Key>> Keys{ get; set; }

        public KeyBinding()
        {
            Keys = new ObservableCollection<KeyValuePair<KeyTranslation, Key>>()
            {
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K0, Key.NumPad0),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K1, Key.NumPad1),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K2, Key.NumPad2),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K3, Key.NumPad3),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K4, Key.NumPad4),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K5, Key.NumPad5),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K6, Key.NumPad6),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K7, Key.NumPad7),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K8, Key.NumPad8),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.K9, Key.NumPad9),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.PlayPauze, Key.Subtract),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.PlayStop, Key.Enter),
                new KeyValuePair<KeyTranslation, Key>(KeyTranslation.Random, Key.Add),

            };
        }
    }
}
