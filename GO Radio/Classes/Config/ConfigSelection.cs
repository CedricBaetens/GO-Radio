using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class ConfigSelection
    {
        public ObservableCollection<ConfigType> Configurations { get; set; }
        public ConfigType ActiveConfiguration { get; set; }

        public ConfigSelection()
        {
            Configurations = new ObservableCollection<ConfigType>
            {
                new ConfigType() { Name="Counter Strike: Global Offensive" },
                new ConfigType() { Name="Other (Skype, ...)" }
            };

            ActiveConfiguration = Configurations[0];
        }
    }

    [ImplementPropertyChanged]
    public class ConfigType
    {
        public string Name { get; set; }
    }
}
