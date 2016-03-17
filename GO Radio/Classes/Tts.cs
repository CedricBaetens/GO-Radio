using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes
{
    public class Tts
    {
        int count = 0;
        public SoundNew GetSound(string text)
        {
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
            sound.Name = "Text To Speech: " + text;
            return sound;
        }
    }
}
