using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class KeyBinder
    {
        public enum KeyTranslation : int
        {
            PlayPauze = 0,
            PlayStop = 1,
            Random = 2,
            K0,
            K1,
            K2,
            K3,
            K4,
            K5,
            K6,
            K7,
            K8,
            K9,
            Console
        }

        public ObservableCollection<BindableKey> Keys { get; set; }

        public KeyBinder()
        {
            Keys = new ObservableCollection<BindableKey>()
            {
                new BindableKey(Key.PageDown) { Name="Play/Pauze", Description="Key that is used to play/pauze the selected sound." },
                new BindableKey(Key.PageUp) { Name="Play/Stop", Description="Key that is used to play/stop the selected sound." },
                new BindableKey(Key.Add) { Name="Random Sound", Description="Key that is used to select a random sound." },
                new BindableKey(Key.NumPad0) { Name="Number 0", Description="Key that is used to type in the number 0."},
                new BindableKey(Key.NumPad1) { Name="Number 1", Description="Key that is used to type in the number 1."},
                new BindableKey(Key.NumPad2) { Name="Number 2", Description="Key that is used to type in the number 2." },
                new BindableKey(Key.NumPad3) { Name="Number 3", Description="Key that is used to type in the number 3." },
                new BindableKey(Key.NumPad4) { Name="Number 4", Description="Key that is used to type in the number 4."  },
                new BindableKey(Key.NumPad5) { Name="Number 5", Description="Key that is used to type in the number 5."  },
                new BindableKey(Key.NumPad6) { Name="Number 6", Description="Key that is used to type in the number 6." },
                new BindableKey(Key.NumPad7) { Name="Number 7", Description="Key that is used to type in the number 7." },
                new BindableKey(Key.NumPad8) { Name="Number 8", Description="Key that is used to type in the number 8." },
                new BindableKey(Key.NumPad9) { Name="Number 9", Description="Key that is used to type in the number 9." },
                new BindableKey(Key.Delete) { Name="Open Console", Description="Key that is used to open the GO Radio Console Overlay." }
            };
        }

        public void Load()
        {
            //string loc = Path.Combine(ProgramSettings.AppFolder, "keybindings.json");
            //if (File.Exists(loc))
            //{
            //    string json = File.ReadAllText(loc);
            //    Keys = JsonConvert.DeserializeObject<ObservableCollection<BindableKey>>(json);
            //}

            //int a = 0;
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(Keys, Formatting.Indented);
            string loc = Path.Combine(ProgramSettings.AppFolder ,"keybindings.json");

            try
            {
                File.WriteAllText(loc, json);
            }
            catch (Exception e )
            {
                MessageBox.Show("Error writing to " + loc + e.Message + " TEST " );
            }
        }
    }

    [ImplementPropertyChanged]
    public class BindableKey
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Key Key { get; set; }
        public string KeyCsGo { get; set; }

        private Dictionary<Key, string> CsGoKeys = new Dictionary<Key, string>()
            {
                { Key.Insert,"ins" },
                { Key.Delete,"del" },
                { Key.Home,"Home" },
                { Key.End,"End" },
                { Key.PageUp,"pgup" },
                { Key.PageDown,"pgdn" },
            };

        private LowLevelKeyboardListener keyboardHook;

        public BindableKey(Key key)
        {
            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;

            Key = key;
            if (CsGoKeys.ContainsKey(key))
            {
                KeyCsGo = CsGoKeys[Key];
            }
        }

        public BindableKey()
        {
            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;

            if (CsGoKeys.ContainsKey(Key))
            {
                KeyCsGo = CsGoKeys[Key];
            }
        }

        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (e.KeyPressed != Key.Enter & CsGoKeys.ContainsKey(e.KeyPressed))
            {
                Key = e.KeyPressed;
                KeyCsGo = CsGoKeys[Key];
                keyboardHook.UnHookKeyboard();
            }
            else
            {
                keyboardHook.UnHookKeyboard();
                MessageBox.Show("This is not a valid key! Please choose another key.");
                keyboardHook.HookKeyboard();
            }
            
        }

        public void ChangeKey()
        {
            keyboardHook.HookKeyboard();
        }
        public ICommand CommandChangeKey => new RelayCommand(ChangeKey);
    }
}
