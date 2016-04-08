using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes.Settings
{
    public class UserSettings
    {
        public string SoundPath { get; set; }
        public ProgramSelectionSetting CsgoSettings { get; set; }
        public ProgramSelectionSetting SkypeSettings { get; set; }
    }
}
