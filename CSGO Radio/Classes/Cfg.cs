﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace HLDJ_Advanced.Classes
{
    static class Cfg
    {
        public static void CreateInit()
        {
            List<string> content = new List<string>();
            content.Add("alias bs_play bs_play_on");
            content.Add("alias bs_play_on \"alias bs_play bs_play_off; voice_inputfromfile 1; voice_loopback 1; +voicerecord\"");
            content.Add("alias bs_play_off \"-voicerecord; voice_inputfromfile 0; voice_loopback 0; alias bs_play bs_play_on\"");
            content.Add("alias tts \"condump;\"");
            content.Add("alias la \"exec radio_songs\"");
            content.Add("bind KP_ENTER bs_play");

            File.WriteAllLines(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio.cfg", content);
        }

        public static void RemoveInit()
        {
            File.Delete(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio.cfg");
        }


        public static void CreateSongList(ObservableCollection<KeyValuePair<string, SoundWAV>> list)
        {
            List<string> content = new List<string>();

            foreach (var sound in list)
            {
                 content.Add(string.Format("echo {0}: {1}", sound.Value.IdFull, sound.Value.Name));
            }

            File.WriteAllLines(ProgramSettings.PathCsgo + "\\csgo\\cfg\\radio_songs.cfg", content);
        }

        public static string GetTtsText()
        {
            if (File.Exists(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt"))
            {
                var lastLine = File.ReadLines(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt").Last().Remove(0,6);
                File.Delete(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt");
                return lastLine;
            }
            return "";
        }
    }
}