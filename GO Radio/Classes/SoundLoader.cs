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
        //public SoundNew SoundPauzed { get; set; }
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
        protected virtual void StatePlay()
        {
            State = SoundState.PLAYING;
            Stopwatch.Start();
        }
        protected virtual void StateStop()
        {
            State = SoundState.STOPPED;
            Stopwatch.Reset();
        }
        protected virtual void StatePauze()
        {
            State = SoundState.PAUSED;
            Stopwatch.Stop();
        }

        // Button Commands
        public virtual void PlayPause()
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
        public virtual void PlayStop()
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
    }
}
