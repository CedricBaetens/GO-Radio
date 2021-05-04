using GO_Radio.Classes;
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

namespace GO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class KeyBindingWindow : Window
    {
        public KeyBinder KeyBinding { get; set; }
        public BindableKey SelectedItem { get; set; }

        

        public KeyBindingWindow(Classes.KeyBinder keybinding)
        {
            InitializeComponent();

            KeyBinding = keybinding;

            DataContext = this;
        }

        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            //SelectedItem.ChangeKey(e.KeyPressed);
        }
    }
}
