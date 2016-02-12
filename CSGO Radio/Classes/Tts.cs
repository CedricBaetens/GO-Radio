using NAudio.Wave;
using System;
using System.Collections.Generic;
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


        System.Timers.Timer ttsTimer = new System.Timers.Timer();

        public Tts()
        {
            ttsTimer.Elapsed += TtsTimer_Elapsed;
            ttsTimer.Interval = 100;
            ttsTimer.Enabled = true;
        }

        private void TtsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var text = GetTtsText();
            if (!string.IsNullOrEmpty(text))
            {
                StringToTTS(text);
            }
        }

        public static string GetTtsText()
        {
            if (File.Exists(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt"))
            {
                var lastLine = File.ReadLines(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt").Last().Remove(0, 6);
                File.Delete(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt");
                return lastLine;
            }
            return "";
        }

        private void StringToTTS(string input)
        {
            string pathNotConv = ProgramSettings.PathSounds + "\\audio\\tts.wav";

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
                synth.Speak(input);
                synth.SetOutputToNull();
                synth.Dispose();
            }

            var sound = AudioHelper.Convert(new SoundUnconverted(pathNotConv));

            TtsDetected(sound);

            //File.Delete(pathNotConv);
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
