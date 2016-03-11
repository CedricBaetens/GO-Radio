using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundLoader
    {
        public SoundNew Sound { get; set; }
        public SoundNew SoundPauzed { get; set; }

        public TimeSpan TimePlaying { get; set; }

        private Stopwatch stopwatch;
        private Timer timer;

        public SoundState State { get; set; }

        public enum SoundState
        {
            LOADED,
            STOPPED,
            PLAYING,
            PAUSED,
            LOADEDSTILLPLAYING
        }

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
            switch (State)
            {
                case SoundState.LOADED:
                    Play();
                    break;
                case SoundState.STOPPED:
                    Play();
                    break;
                case SoundState.PLAYING:
                    Pauze();
                    break;
                case SoundState.PAUSED:
                    Play();
                    break;
                case SoundState.LOADEDSTILLPLAYING:
                    Stop();
                    break;
                default:
                    break;
            }
        }
        public void PlayStop()
        {
            switch (State)
            {
                case SoundState.LOADED:
                    Play();
                    break;
                case SoundState.STOPPED:
                    Play();
                    break;
                case SoundState.PLAYING:
                    Stop();
                    break;
                case SoundState.PAUSED:
                    Play();
                    break;
                case SoundState.LOADEDSTILLPLAYING:
                    Stop();
                    break;
                default:
                    break;
            }      
        }

        public void Reset()
        {
            if (MessageBox.Show("Make sure you are not playing any sound in game!","Reset Sound Monitor",MessageBoxButton.OKCancel)==MessageBoxResult.OK)
            {
                LoadSong(Sound);
                State = SoundState.LOADED;
                stopwatch.Reset();
            }
        }

        private void Play()
        {
            State = SoundState.PLAYING;
            stopwatch.Start();
        }

        private void Stop()
        {
            State = SoundState.STOPPED;
            stopwatch.Reset();
        }

        private void Pauze()
        {
            State = SoundState.PAUSED;
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

        public void LoadSong(SoundNew sound)
        {
            try
            {
                if (sound != null)
                {

                    if (State == SoundState.PLAYING)
                    {
                        State = SoundState.LOADEDSTILLPLAYING;
                    }
                    else
                    {
                        State = SoundState.LOADED;
                    }

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
