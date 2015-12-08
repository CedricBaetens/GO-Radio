using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace HLDJ_Advanced.Classes
{
    [ImplementPropertyChanged]
    public class NewSound
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public NewSound(string path)
        {
            Path = path;
            Name = path.Split('\\').Last();
        }
    }
}
