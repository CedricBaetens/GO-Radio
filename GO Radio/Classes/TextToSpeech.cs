using System.Speech.Synthesis;

namespace GO_Radio.Classes
{
    public class TextToSpeech
    {
        string _path;
        int _count;

        public void Start(string inpath)
        {
            _path = inpath;
        }

        public SoundNew GetSound(string text)
        {
            string pathNotConv = string.Format("{0}\\audio\\tmp\\Text To Speech {1}.wav", _path, _count++);

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
