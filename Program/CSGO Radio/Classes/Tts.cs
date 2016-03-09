using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_Radio.Classes
{
    public class Tts
    {
        // Used for event
        public delegate void StatusUpdateHandler(object sender, ProgressEventArgs e);
        public event StatusUpdateHandler OnTtsDetected;

        private int count = 0;

        System.Timers.Timer ttsTimer = new System.Timers.Timer();

        public Tts()
        {
            ttsTimer.Elapsed += TtsTimer_Elapsed;
            ttsTimer.Interval = 100;
        }

        public void Start()
        {
            ttsTimer.Start();
        }

        private void TtsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var text = GetTtsText();
            if (!string.IsNullOrEmpty(text))
            {
                ttsTimer.Stop(); 
                string pathNotConv = string.Format("{0}\\audio\\tmp\\Text To Speech {1}.wav", ProgramSettings.PathSounds, count++);                

                using (var synth = new SpeechSynthesizer())
                {
                    // Configure the audio output. 
                    synth.SetOutputToWaveFile(pathNotConv);

                    // Build a prompt.
                    //PromptBuilder builder = new PromptBuilder();
                    //builder.AppendText(input);
                    synth.Rate = -2;

                    // Change voice
                    //synth.SelectVoiceByHints(dialog.SelectedGender);

                    // Speak the prompt.
                    synth.Speak(text);
                    synth.SetOutputToNull();
                    synth.Dispose();
                }

                var sound = AudioHelper.Convert(new SoundUnconverted(pathNotConv));
                sound.Name = "TTS: " + text;
                TtsDetected(sound);
                ttsTimer.Start();
            }
        }
        private static string GetTtsText()
        {
            if (File.Exists(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt"))
            {
                var lastLine = File.ReadLines(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt").Last().Remove(0, 6);
                File.Delete(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt");
                return lastLine;
            }
            return "";
        }   

        // Event methods          
        private void TtsDetected(SoundNew sound)
        {
            // Make sure someone is listening to event
            if (OnTtsDetected == null) return;

            ProgressEventArgs args = new ProgressEventArgs(sound);
            OnTtsDetected(this, args);
        }
        public class ProgressEventArgs : EventArgs
        {
            public SoundNew Sound { get; set; }
            public ProgressEventArgs(SoundNew sound)
            {
                Sound = sound;
            }
        }
    }
}
