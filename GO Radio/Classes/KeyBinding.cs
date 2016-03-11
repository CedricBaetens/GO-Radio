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
            
        }

        public ObservableCollection<BindableKey> Keys { get; set; }

        public KeyBinder()
        {
            Keys = new ObservableCollection<BindableKey>()
            {
                new BindableKey() { Name="Play/Pauze", Description="Key that is used to play/pauze the selected sound.", Key = Key.Enter },
                new BindableKey() { Name="Play/Stop", Description="Key that is used to play/stop the selected sound.", Key = Key.Subtract },
                new BindableKey() { Name="Random Sound", Description="Key that is used to select a random sound.", Key = Key.Add },
                new BindableKey() { Name="Number 0", Description="Key that is used to type in the number 0.", Key = Key.NumPad0 },
                new BindableKey() { Name="Number 1", Description="Key that is used to type in the number 1.", Key = Key.NumPad1 },
                new BindableKey() { Name="Number 2", Description="Key that is used to type in the number 2.", Key = Key.NumPad2 },
                new BindableKey() { Name="Number 3", Description="Key that is used to type in the number 3.", Key = Key.NumPad3 },
                new BindableKey() { Name="Number 4", Description="Key that is used to type in the number 4.", Key = Key.NumPad4 },
                new BindableKey() { Name="Number 5", Description="Key that is used to type in the number 5.", Key = Key.NumPad5 },
                new BindableKey() { Name="Number 6", Description="Key that is used to type in the number 6.", Key = Key.NumPad6 },
                new BindableKey() { Name="Number 7", Description="Key that is used to type in the number 7.", Key = Key.NumPad7 },
                new BindableKey() { Name="Number 8", Description="Key that is used to type in the number 8.", Key = Key.NumPad8 },
                new BindableKey() { Name="Number 9", Description="Key that is used to type in the number 9.", Key = Key.NumPad9 }
            };
        }

        public void Load()
        {
            string loc = Path.Combine(ProgramSettings.AppFolder, "keybindings.json");
            if (File.Exists(loc))
            {
                string json = File.ReadAllText(loc);
                Keys = JsonConvert.DeserializeObject<ObservableCollection<BindableKey>>(json);
            }
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

        private LowLevelKeyboardListener keyboardHook;

        public BindableKey()
        {
            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;           
        }

        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            Key = e.KeyPressed;
            keyboardHook.UnHookKeyboard();
        }

        public void ChangeKey()
        {
            keyboardHook.HookKeyboard();
        }
        public ICommand CommandChangeKey => new RelayCommand(ChangeKey);

    }
}
