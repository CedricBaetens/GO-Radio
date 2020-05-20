using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class KeyboardHook
    {
        public string Input { get; set; } = "";
        public KeyBinder KeyBindings { get; set; }

        private LowLevelKeyboardListener keyboardHook;
        private System.Timers.Timer timerClearInput;

        public KeyboardHook()
        {
            // Instanciate
            keyboardHook = new LowLevelKeyboardListener();
            timerClearInput = new System.Timers.Timer(5000);
            KeyBindings = new KeyBinder();

            // Events
            timerClearInput.Elapsed += TimerClearInput_Elapsed;
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
        }

        public void Hook()
        {
            keyboardHook.HookKeyboard();
        }
        public void UnHook()
        {
            keyboardHook.UnHookKeyboard();
            Input = "";
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

    public class LowLevelKeyboardListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public LowLevelKeyboardListener()
        {
            _proc = HookCallback;
        }

        public void HookKeyboard()
        {
            _hookID = SetHook(_proc);
        }

        public void UnHookKeyboard()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (OnKeyPressed != null) { OnKeyPressed(this, new KeyPressedArgs(KeyInterop.KeyFromVirtualKey(vkCode))); }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }

    public class KeyPressedArgs : EventArgs
    {
        public Key KeyPressed { get; private set; }

        public KeyPressedArgs(Key key)
        {
            KeyPressed = key;
        }
    }
}
