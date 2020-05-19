using PropertyChanged;
using System;
using System.Diagnostics;
using System.Timers;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public abstract class SoundLoader
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

        // Variablbe
        protected Stopwatch Stopwatch;
        protected string CopyPath = "";
        private readonly Timer _timer;

        // Constructor
        protected SoundLoader()
        {
            Stopwatch = new Stopwatch();
            _timer = new Timer(1);
            _timer.Elapsed += Timer_Elapsed;
        }

        // Timer
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimePlaying = Stopwatch.Elapsed;
        }

        public virtual bool Start(string path = "")
        {
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
        protected virtual void OnPlay() { }
        protected virtual void OnStop() { }
        protected virtual void OnPauze() { }

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
