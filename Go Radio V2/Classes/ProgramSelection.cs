using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go_Radio_V2.Classes
{
    class ProgramSelection
    {
        public string Name { get; set; }

        public ProgramSelectionSetting Setting { get; set; }

        public ProgramSelection()
        {
            Setting = new ProgramSelectionSetting();
        }
    }
}
