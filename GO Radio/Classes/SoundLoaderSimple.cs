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

        public override void PlayStop()
        {
            base.PlayStop();
            CopyToGameDirectory(Sound);
        }

        protected override void StatePauze()
        {
            base.StatePauze();
            CopyToGameDirectory(Sound);           
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
            if (State == SoundState.LOADEDSTILLPLAYING)
            {
                AudioHelper.Create(sound, CopyPath, new TimeSpan(0));
            }
            else
            {
                AudioHelper.Create(sound, CopyPath, Stopwatch.Elapsed);
            }
        }
    }
}
