using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes.Settings
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
