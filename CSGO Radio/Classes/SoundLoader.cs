using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CSGO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundLoader
    {
        public SoundNew Sound { get; set; }
        public SoundNew SoundPauzed { get; set; }

        public TimeSpan TimePlaying { get { return timer.Elapsed; } }

        private Stopwatch timer;


        public SoundLoader()
        {
            timer = new Stopwatch();
        }

        public void PlayPause()
        {
            if (timer.IsRunning)
            {
               timer.Stop();

                if (Sound.IsTrimmed)
                {
                    SoundPauzed = new SoundNew(Sound.PathTrim);
                }
                else
                {
                    SoundPauzed = new SoundNew(Sound.Path);
                }
                
                SoundPauzed.Pauze(TimePlaying);

                LoadSongPauzed(SoundPauzed);
            }
            else
            {
                timer.Start();
            }    
        }
        public void PlayStop()
        {
            if (Sound != null)
            {
                if (timer.IsRunning)
                {
                    timer.Stop();
                    timer.Reset();
                    LoadSong(Sound);
                }
                else
                {
                    timer.Start();
                }
            }         
        }

        public void LoadSong(SoundNew sound)
        {
            try
            {
                if (sound != null)
                {
                    SoundToCsDir(sound);
                    Sound = sound;
                }
            }
            catch (Exception e)
            {
                int a = 0;
            }
        }
        private void LoadSongPauzed(SoundNew sound)
        {
            try
            {
                if (sound != null)
                {
                    SoundToCsDir(sound);
                }
            }
            catch
            {
                // ignored
            }
        }

        private void SoundToCsDir(SoundNew sound)
        {
            if (File.Exists(ProgramSettings.PathCsgo + "\\voice_input.wav"))
            {
                File.Delete(ProgramSettings.PathCsgo + "\\voice_input.wav");
            }

            if (!string.IsNullOrEmpty(sound.PathTrim))
            {
                File.Copy(sound.PathTrim, ProgramSettings.PathCsgo + "\\voice_input.wav");
            }
            else
            {
                File.Copy(sound.Path, ProgramSettings.PathCsgo + "\\voice_input.wav");
            }
        }
    }
}
