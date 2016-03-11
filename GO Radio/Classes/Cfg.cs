using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace GO_Radio.Classes
{
    static class Cfg
    {
        public static class Create
        {
            public static void Init()
            {
                List<string> content = new List<string>();
                content.Add("alias bs_play bs_play_on");
                content.Add("alias bs_play_on \"alias bs_play bs_play_off; voice_inputfromfile 1; voice_loopback 1; +voicerecord\"");
                content.Add("alias bs_play_off \"-voicerecord; voice_inputfromfile 0; voice_loopback 0; alias bs_play bs_play_on\"");
                content.Add("alias tts \"condump;\"");
                content.Add("alias la \"exec radio_songs\"");
                content.Add("bind KP_ENTER bs_play");
                content.Add("bind KP_MINUS bs_play");

                File.WriteAllLines(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio.cfg", content);
            }
        }
        public static class Remove
        {
            public static void Init()
            {
                File.Delete(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio.cfg");
            }
        }

        public static void CreateSongList(ObservableCollection<KeyValuePair<int, SoundNew>> list)
        {
            List<string> content = new List<string>();

            foreach (var sound in list)
            {
                 content.Add(string.Format("echo {0}: {1}", sound.Key.ToString("0000"), sound.Value.Name));
            }

            File.WriteAllLines(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio_songs.cfg", content);
        }

        
    }
}
