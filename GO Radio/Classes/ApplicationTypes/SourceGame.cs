using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes.ApplicationTypes
{
    [ImplementPropertyChanged]
    class SourceGame : ProgramSelection
    {
        //private KeyboardController keyboard;
        public KeyboardController Keyboard { get; set; }
        //public string input { get { return keyboard.Input; } }

        public SourceGame()
        {
            Keyboard = new KeyboardController();
            Keyboard.ButtonPressed += Keyboard_ButtonPressed;
            Keyboard.IdEntered += Keyboard_IdEntered;
        }

        public override void Start()
        {
            Keyboard.Hook();
        }

        public override void Stop()
        {
            Keyboard.UnHook();
        }

        private void Keyboard_IdEntered(object sender, KeyboardController.IdEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Keyboard_ButtonPressed(object sender, KeyboardController.ButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
