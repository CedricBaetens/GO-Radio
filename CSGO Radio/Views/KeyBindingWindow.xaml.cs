using CSGO_Radio.Classes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CSGO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class KeyBindingWindow : Window
    {
        public Classes.KeyBinding KeyBinding { get; set; }
        public KeyValuePair<Classes.KeyBinding.KeyTranslation, Key> SelectedItem { get; set; }

        private LowLevelKeyboardListener keyboardHook;
        private Classes.KeyBinding.KeyTranslation toChangeKey;

        public KeyBindingWindow(Classes.KeyBinding keybinding)
        {
            InitializeComponent();

            KeyBinding = keybinding;

            keyboardHook = new LowLevelKeyboardListener();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
            keyboardHook.HookKeyboard();

            DataContext = this;
        }

        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            KeyBinding.Keys[(int)toChangeKey] = new KeyValuePair<Classes.KeyBinding.KeyTranslation, Key>(toChangeKey, e.KeyPressed);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            toChangeKey = SelectedItem.Key;
        }
    }
}
