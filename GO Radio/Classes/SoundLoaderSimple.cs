using System;
using System.IO;
using System.Windows;

namespace GO_Radio.Classes
{
    class SoundLoaderSimple : SoundLoader
    {
        public override void LoadSound(SoundNew sound)
        {
            try
            {
                if (sound != null)
                {
                    if (State == SoundState.PLAYING)
                        State = SoundState.LOADEDSTILLPLAYING;
                    else
                        Stopwatch.Reset();

                    CopyToGameDirectory(sound);
                    base.LoadSound(sound);
                }
            }
            catch (Exception)
            {
                //
            }
        }

        protected override void StatePauze()
        {
            base.StatePauze();
            CopyToGameDirectory(SoundPauzed);           
        }

        protected override void Reset()
        {
            if (MessageBox.Show("Make sure you are not playing any sound in game!", "Reset Sound Monitor", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                base.Reset();
            }
        }

        private void CopyToGameDirectory(SoundNew sound)
        {
            if (File.Exists(CopyPath + "\\voice_input.wav"))
                File.Delete(CopyPath + "\\voice_input.wav");

            AudioHelper.Create(sound, CopyPath + "\\voice_input.wav");
        }
    }
}
