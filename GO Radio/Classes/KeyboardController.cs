using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class KeyboardController
    {
        public string Input { get; set; } = "";
        public KeyBinder KeyBindings { get; set; }

        private LowLevelKeyboardListener keyboardHook;
        private System.Timers.Timer timerClearInput;

        public KeyboardController()
        {
            // Instanciate
            keyboardHook = new LowLevelKeyboardListener();
            timerClearInput = new System.Timers.Timer(5000);
            KeyBindings = new KeyBinder();

            // Events
            timerClearInput.Elapsed += TimerClearInput_Elapsed;
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
            keyboardHook.HookKeyboard();
        }

        public void Hook()
        {
            keyboardHook.HookKeyboard();
        }
        public void UnHook()
        {
            keyboardHook.UnHookKeyboard();
        }


        private void TimerClearInput_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Input = "";
            timerClearInput.Stop();
        }
        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.PlayStop].Key)
                FireButtonEvent(PressedKey.PlayStop);

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.PlayPauze].Key)
               FireButtonEvent(PressedKey.PlayPauze);

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.Random].Key)
                Input += "+";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K0].Key)
                Input += "0";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K1].Key)
                Input += "1";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K2].Key)
                Input += "2";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K3].Key)
                Input += "3";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K4].Key)
                Input += "4";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K5].Key)
                Input += "5";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K6].Key)
                Input += "6";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K7].Key)
                Input += "7";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K8].Key)
                Input += "8";

            if (e.KeyPressed == KeyBindings.Keys[(int)KeyBinder.KeyTranslation.K9].Key)
                Input += "9";



            if (Input.Contains("+"))
            {
                FireEventIdEntered(Input);
                Input = "";
            }
            


            if (Input.Length >= 4)
            {
                // Check if id is valid
                if (!string.IsNullOrEmpty(Input))
                {
                    FireEventIdEntered(Input);
                }

                Input = "";
            }

            // Start Clear timer when id is Entered
            if (Input != "")
                timerClearInput.Start();
        }


        // Used for id event 
        public delegate void IdUpdateEventHandler(object sender, IdEventArgs e);
        public event IdUpdateEventHandler IdEntered;
        private void FireEventIdEntered(string id)
        {
            // Make sure someone is listening to event
            if (IdEntered == null) return;

            IdEventArgs args = new IdEventArgs(id);
            IdEntered(this, args);
        }
        public class IdEventArgs : EventArgs
        {
            public string Input { get; set; }
            public IdEventArgs(string id)
            {
                Input = id;
            }
        }

        // Used for button event
        public delegate void ButtonUpdateEventHandler(object sender, ButtonEventArgs e);
        public event ButtonUpdateEventHandler ButtonPressed;
        private void FireButtonEvent(PressedKey key)
        {
            // Make sure someone is listening to event
            if (ButtonPressed == null) return;

            ButtonEventArgs args = new ButtonEventArgs(key);
            ButtonPressed(this, args);
        }
        public class ButtonEventArgs : EventArgs
        {
            public PressedKey Key { get; set; }
            public ButtonEventArgs(PressedKey key)
            {
                Key = key;
            }
        }
        public enum PressedKey
        {
            PlayPauze,
            PlayStop
        }
    }
}
