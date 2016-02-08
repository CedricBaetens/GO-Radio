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
    class Tts
    {
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
            string pathNotConv = ProgramSettings.PathSounds + "\\audio\\ttsNotConv.wav";

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


            // ReSample
            string path = string.Format("{0}\\audio\\{1}{2}", ProgramSettings.PathSounds, "tts", ".wav");

            using (var reader = new WaveFileReader(pathNotConv))
            {
                using (var resampler = new MediaFoundationResampler(reader, new WaveFormat(22050, 16, 1)))
                {
                    resampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(path, resampler);
                    resampler.Dispose();
                }
            }

            //LoadedSound = new SoundWAV()
            //{
            //    Name = "Text To Speech",
            //    Path =  path
            //};

            //SoundController.LoadSong(LoadedSound);

//

            File.Delete(pathNotConv);
        }
    }
}
