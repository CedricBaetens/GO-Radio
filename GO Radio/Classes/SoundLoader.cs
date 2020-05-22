using NAudio.Wave;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SoundLoader
    {
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

        // Properties
        public SoundNew Sound { get; set; }
        public SoundState State { get; set; }
        public TimeSpan TimePlaying { get; set; }

        public List<WaveOutCapabilities> OutputDevices { get; set; } = new List<WaveOutCapabilities>();
        public WaveOutCapabilities SelectedOutputDevice { get; set; }

        // Variablbe
        protected Stopwatch Stopwatch;
        protected string CopyPath = "";
        private readonly Timer _timer;
        private WaveOut _WaveOut = new WaveOut();

        // Constructor
        public SoundLoader()
        {
            Stopwatch = new Stopwatch();
            _timer = new Timer(1);
            _timer.Elapsed += Timer_Elapsed;

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var output = WaveOut.GetCapabilities(i);
                OutputDevices.Add(output);
            }
            SelectedOutputDevice = OutputDevices.FirstOrDefault();
        }

        // Timer
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimePlaying = Stopwatch.Elapsed;
        }

        public virtual bool Start(string path = "")
        {
            _WaveOut.DeviceNumber = OutputDevices.FindIndex(x => x.ProductName == SelectedOutputDevice.ProductName);
            CopyPath = path;
            _timer.Start();
            State = SoundState.NOTLOADED;
            return true;
        }
        public virtual void Stop()
        {
            _timer.Stop();
        }

        public virtual void LoadSound(SoundNew sound)
        {
            Sound = sound;
        }

        // State Functions
        public virtual void Reset()
        {
            LoadSound(Sound);
            State = SoundState.LOADED;
            Stopwatch.Reset();
        }
        protected virtual void OnPlay()
        {
            if (State == SoundLoader.SoundState.PAUSED)
            {
                _WaveOut.Resume();
            }
            else
            {
                var waveReader = new WaveFileReader(Sound.Path);
                _WaveOut.Init(waveReader);
                _WaveOut.Play();
            }
        }
        protected virtual void OnStop()
        {
            _WaveOut.Stop();
        }
        protected virtual void OnPauze()
        {
            _WaveOut.Pause();
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
                    {
                        OnPlay();
                        State = SoundState.PLAYING;
                        Stopwatch.Start();
                    }
                    break;
                case SoundState.PLAYING:
                    {
                        OnPauze();
                        State = SoundState.PAUSED;
                        Stopwatch.Stop();
                    }
                    break;
                case SoundState.LOADEDSTILLPLAYING:
                    {
                        OnStop();
                        State = SoundState.STOPPED;
                        Stopwatch.Reset();
                    }
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
                    {
                        OnPlay();
                        State = SoundState.PLAYING;
                        Stopwatch.Start();
                    }
                    break;
                case SoundState.PLAYING:
                case SoundState.LOADEDSTILLPLAYING:
                    {
                        OnStop();
                        State = SoundState.STOPPED;
                        Stopwatch.Reset();
                    }
                    break;
            }
        }
    }
}
