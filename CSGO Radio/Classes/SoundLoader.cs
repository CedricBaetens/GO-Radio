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

        public TimeSpan TimePlaying { get; set; }

        private Stopwatch stopwatch;
        private Timer timer;

        public SoundLoader()
        {
            stopwatch = new Stopwatch();
            timer = new Timer();
            timer.Interval = 1;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimePlaying = stopwatch.Elapsed;
        }

        public void PlayPause()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();

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
                stopwatch.Start();
            }    
        }
        public void PlayStop()
        {
            if (Sound != null)
            {
                if (stopwatch.IsRunning)
                {
                    stopwatch.Stop();
                    stopwatch.Reset();
                    LoadSong(Sound);
                }
                else
                {
                    stopwatch.Start();
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
