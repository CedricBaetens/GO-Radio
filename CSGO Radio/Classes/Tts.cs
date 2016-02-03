using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_Radio.Classes
{
    class Tts
    {
        System.Timers.Timer ttsTimer = new System.Timers.Timer();

        public Tts()
        {
            ttsTimer.Elapsed += TtsTimer_Elapsed;
            ttsTimer.Interval = 100;
            ttsTimer.Enabled = true;
        }

        private void TtsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var text = Cfg.GetTtsText();
            if (!string.IsNullOrEmpty(text))
            {
                //StringToTTS(text);
            }
        }
    }
}
