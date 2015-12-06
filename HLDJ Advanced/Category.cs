using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDJ_Advanced
{
    public class Category
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public List<Sound> Sounds { get; set; }

        public Category()
        {
             Sounds = new List<Sound>();
        }
    }
}
