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
        public SoundNew Sound { get; set; }
        public SoundNew SoundPauzed { get; set; }

        public TimeSpan TimePlaying { get; set; }

        private Stopwatch stopwatch;
        private Timer timer;

        public SoundState State { get; set; }

        private WaveIn microphone;
        private WaveOut virtualOutput;
        private WaveOut virtualOutputForMic;
        private WaveOut speakers;
        private BufferedWaveProvider bwp;

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

            CorrectDevices();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimePlaying = stopwatch.Elapsed;
        }

        // Butoon Commands
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
        private void Play()
        {
            State = SoundState.PLAYING;
            stopwatch.Start();

            speakers.Play();
            virtualOutput.Play();

        }
        private void Stop()
        {
            State = SoundState.STOPPED;
            stopwatch.Reset();
            LoadSong(Sound);

            speakers.Stop();
            virtualOutput.Stop();
        }
        private void Pauze()
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

                    var reader = new WaveFileReader(Sound.Path);
                    speakers.Init(reader);

                    var reader2 = new WaveFileReader(Sound.Path);
                    virtualOutput.Init(reader2);
                }
            }
            catch (Exception e){
                int a = 0;
            }
        }
        private void CopyToGameDirectory(SoundNew sound)
        {
            if (File.Exists(ProgramSettings.Instance.PathCsgo + "\\voice_input.wav"))
                File.Delete(ProgramSettings.Instance.PathCsgo + "\\voice_input.wav");

            AudioHelper.Create(sound, ProgramSettings.Instance.PathCsgo + "\\voice_input.wav");

            int a = 0;
            //if (!string.IsNullOrEmpty(sound.GetPath()))
            //    File.Copy(sound.GetPath(), ProgramSettings.Instance.PathCsgo + "\\voice_input.wav");
        }


        // Audio device
        public void CorrectDevices()
        {
            // Find Output Device
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var output = WaveOut.GetCapabilities(i);
                if (output.ProductName.Contains("CABLE Input (VB-Audio Virtual C"))
                {
                    virtualOutput = new WaveOut();
                    virtualOutput.DeviceNumber = i;

                    virtualOutputForMic = new WaveOut();
                    virtualOutputForMic.DeviceNumber = i;
                }
            }

            // Default Audio Device
            speakers = new WaveOut();
            speakers.DeviceNumber = 0;

            // Micro
            microphone = new WaveIn();
            microphone.DeviceNumber = 0;
            microphone.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(0).Channels);

            microphone.DataAvailable += Microphone_DataAvailable;
            bwp = new BufferedWaveProvider(microphone.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;

            virtualOutputForMic.Init(bwp);
            microphone.StartRecording();
            virtualOutputForMic.Play();
        }

        private void Microphone_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
