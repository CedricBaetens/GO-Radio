using NAudio.Wave;
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
        // Properties
        public SoundNew Sound { get; set; }
        public SoundNew SoundPauzed { get; set; }
        public TimeSpan TimePlaying { get; set; }
        public SoundState State { get; set; }

        // Variables
        private Stopwatch stopwatch;
        private Timer timer;
        private string copyPath = "";

        // Enum
        public enum SoundState
        {
            LOADED,
            STOPPED,
            PLAYING,
            PAUSED,
            LOADEDSTILLPLAYING,
            NOTLOADED
        }

        // Constructor
        public SoundLoader()
        {
            stopwatch = new Stopwatch();
            timer = new Timer(1);
            timer.Elapsed += Timer_Elapsed;
            //timer.Start();

            //State = SoundState.NOTLOADED;
        }

        public void Start(string path)
        {
            copyPath = path;
            timer.Start();
            State = SoundState.NOTLOADED;
        }
        public void Stop()
        {
            timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimePlaying = stopwatch.Elapsed;
        }

        // Button Commands
        public void PlayPause()
        {
            switch (State)
            {
                case SoundState.NOTLOADED:
                case SoundState.LOADED:
                case SoundState.STOPPED:
                case SoundState.PAUSED:
                    StatePlay();
                    break;
                case SoundState.PLAYING:
                    StatePauze();
                    break;              
                case SoundState.LOADEDSTILLPLAYING:
                    StateStop();
                    break;
            }
        }
        public void PlayStop()
        {
            switch (State)
            {
                case SoundState.NOTLOADED:
                case SoundState.LOADED:
                case SoundState.STOPPED:
                case SoundState.PAUSED:
                    StatePlay();
                    break;
                case SoundState.PLAYING:
                case SoundState.LOADEDSTILLPLAYING:
                    StateStop();
                    break;
            }
        }

        // State Functions
        public void Reset()
        {
            if (MessageBox.Show("Make sure you are not playing any sound in game!","Reset Sound Monitor",MessageBoxButton.OKCancel)==MessageBoxResult.OK)
            {
                LoadSong(Sound);
                State = SoundState.LOADED;
                stopwatch.Reset();
            }
        }
        private void StatePlay()
        {
            State = SoundState.PLAYING;
            stopwatch.Start();
        }
        private void StateStop()
        {
            State = SoundState.STOPPED;
            stopwatch.Reset();
        }
        private void StatePauze()
        {
            State = SoundState.PAUSED;
            stopwatch.Stop();

            // Copy sound and trim it
            SoundPauzed = new SoundNew(Sound.GetPath());
            SoundPauzed.Pauze(TimePlaying);

            CopyToGameDirectory(SoundPauzed);
        }

        // Load Functions
        public void LoadSong(SoundNew sound)
        {
            try
            {
                if (sound != null)
                {
                    if (State == SoundState.PLAYING)
                        State = SoundState.LOADEDSTILLPLAYING;
                    else
                        stopwatch.Reset();

                    CopyToGameDirectory(sound);
                    Sound = sound;
                }
            }
            catch (Exception)
            {
                //
            }
        }

        private void CopyToGameDirectory(SoundNew sound)
        {
            if (File.Exists(copyPath + "\\voice_input.wav"))
                File.Delete(copyPath + "\\voice_input.wav");

            AudioHelper.Create(sound, copyPath + "\\voice_input.wav");
        }
    }
}
