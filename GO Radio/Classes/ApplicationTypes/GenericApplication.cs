using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace GO_Radio.Classes.ApplicationTypes
{
    [ImplementPropertyChanged]
    class GenericApplication : ProgramSelection
    {
        public SoundLoader SoundLoader { get; set; }

        public GenericApplication()
        {
            SoundLoader = new SoundLoaderDevice();
            Keyboard.IdEntered += Keyboard_IdEntered;
            Keyboard.ButtonPressed += Keyboard_ButtonPressed;
        }

        private void Keyboard_IdEntered(object sender, KeyboardController.IdEventArgs e)
        {
            SoundLoader.LoadSound(Data.GetSoundById(e.Input));
        }

        private void Keyboard_ButtonPressed(object sender, KeyboardController.ButtonEventArgs e)
        {
            throw new NotImplementedException();
        }


        public override void Start(CategoryList data)
        {
            State = 
                SoundLoader.Start() 
                ? ProgramSelector.ApplicationState.RUNNING : ProgramSelector.ApplicationState.STANDBY;

            // Sta
            if (State == ProgramSelector.ApplicationState.RUNNING)
                base.Start(data);
        }
    }
}
