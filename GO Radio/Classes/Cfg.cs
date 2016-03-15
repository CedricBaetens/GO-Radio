﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace GO_Radio.Classes
{
    static class Cfg
    {
        public static class Create
        {
            public static void Init(KeyBinder keybindings)
            {
                List<string> content = new List<string>();
                content.Add("alias bs_play bs_play_on");
                content.Add("alias bs_play_on \"alias bs_play bs_play_off; voice_inputfromfile 1; voice_loopback 1; +voicerecord\"");
                content.Add("alias bs_play_off \"-voicerecord; voice_inputfromfile 0; voice_loopback 0; alias bs_play bs_play_on\"");
                content.Add("alias tts \"condump;\"");
                content.Add("alias la \"exec radio_categorylist\"");
                content.Add("con_logfile radiolog.txt");
                content.Add(string.Format("bind {0} bs_play", keybindings.Keys[(int)KeyBinder.KeyTranslation.PlayPauze].KeyCsGo));
                content.Add(string.Format("bind {0} bs_play", keybindings.Keys[(int)KeyBinder.KeyTranslation.PlayStop].KeyCsGo));

                File.WriteAllLines(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio.cfg", content);
            }
            public static void CategoryList(CategoryList list)
            {
                List<string> content = new List<string>();

                int count = 0;
                foreach (var category in list.Categories)
                {                                    
                    content.Add(string.Format("echo {2}: {0}: {1}", category.Name, category.Range, count));
                    SongList(category,count);
                    content.Add(string.Format("alias {0} \"exec radio_category_soundlist{0}.cfg\"", count++));
                }

                File.WriteAllLines(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio_categorylist.cfg", content);
            }
            private static void SongList(Category category, int index)
            {
                List<string> content = new List<string>();

                int count = 0;
                foreach (var sound in category.Sounds)
                {
                    content.Add(string.Format("echo {2}: {0}: {1}", sound.Id.ToString("0000"), sound.Name, count));
                    SongLoad(sound);
                    content.Add(string.Format("alias {0} \"exec radio_category_soundload{1}.cfg\"", count++, sound.Id.ToString("0000")));
                }

                File.WriteAllLines(string.Format("{0}\\csgo\\cfg\\radio_category_soundlist{1}.cfg", ProgramSettings.PathCsgo,index), content);
            }
            private static void SongLoad(SoundNew sound)
            {
                List<string> content = new List<string>();

                content.Add(string.Format("echo Loaded {0}: {1};condump; ", sound.Id.ToString("0000"), sound.Name));

                File.WriteAllLines(string.Format("{0}\\csgo\\cfg\\radio_category_soundload{1}.cfg", ProgramSettings.PathCsgo, sound.Id.ToString("0000")), content);
            }
        }
        public static class Remove
        {
            public static void Init()
            {
                File.Delete(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio.cfg");
            }
        }       
    }
}