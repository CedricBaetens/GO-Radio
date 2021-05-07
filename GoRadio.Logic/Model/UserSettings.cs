using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRadio.Logic.Model
{
    public class UserSettings
    {
        // Audio devices
        public string VirtualCable { get; set; }
        public string Microphone { get; set; }
        public string PlaybackDevice { get; set; }
    }
}
