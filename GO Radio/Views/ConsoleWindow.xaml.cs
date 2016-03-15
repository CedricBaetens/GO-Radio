using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GO_Radio.Views
{
    /// <summary>
    /// Interaction logic for ConsoleWindow.xaml
    /// </summary>
    public partial class ConsoleWindow : Window
    {
        public ConsoleWindow()
        {
            InitializeComponent();

            // CONSOLE TEMP HAS TO GO
            Screen screen2 = Screen.AllScreens[0];
            var rectangle = screen2.WorkingArea;

            this.Top = rectangle.Top;
            this.Left = rectangle.Left;
            this.Topmost = true;
        }
    }
}
