using CSGO_Radio.Classes;
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
    /// <summary>
    /// Interaction logic for TrimWindow.xaml
    /// </summary>
    public partial class TrimWindow : Window
    {
        public SoundNew Sound { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public TrimWindow(SoundNew sound)
        {
            InitializeComponent();
            Sound = sound;

            DataContext = this;
        }

        public ICommand CommandTrimSound => new RelayCommand(TrimSound);

        private void TrimSound()
        {
            Sound.Trim(StartTime, EndTime);
            Close();      
        }
    }
}
