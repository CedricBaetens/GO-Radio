using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class ApplicationType
    {
        public string Name { get; set; }
        public bool IsSelectable { get; set; } = true;

        public CategoryList Data { get; set; }      // Used to receive data

        // Features each type has
        public KeyboardController Keyboard { get; set; }
        public SoundLoader SoundLoader { get; set; }

        public ApplicationType()
        {
            Keyboard = new KeyboardController();

            // Events
            Keyboard.IdEntered += Keyboard_IdEntered;
            Keyboard.ButtonPressed += Keyboard_ButtonPressed;
        }

        // Events
        private void Keyboard_ButtonPressed(object sender, KeyboardController.ButtonEventArgs e)
        {
            if (IsActive())
            {
                switch (e.Key)
                {
                    case KeyboardController.PressedKey.PlayStop:
                        SoundLoader.PlayStop();
                        break;
                    case KeyboardController.PressedKey.PlayPauze:
                        SoundLoader.PlayPause();
                        break;
                }
            }
        }
        private void Keyboard_IdEntered(object sender, KeyboardController.IdEventArgs e)
        {
            SoundLoader.LoadSong(Data.GetSoundById(e.Input));
        }

        public virtual void Start()
        {
            Keyboard.Hook();
        }
        public void Stop()
        {
            Keyboard.UnHook();
        }

        public bool IsActive()
        {
           return ActiveProcess.IsSame(Name);
        }
    }
}
