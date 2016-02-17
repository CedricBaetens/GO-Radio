using CSGO_Radio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

        //public TimeSpan StartTime { get; set; }
        //public TimeSpan EndTime { get; set; }


        private SoundPlayer soundPlayer;
        private bool isPlaying = false;

        public TrimWindow(SoundNew sound)
        {
            InitializeComponent();
            Sound = sound;

            soundPlayer = new SoundPlayer();

            DataContext = this;
            tsDown.DataContext = Sound;
            tsUp.DataContext = Sound;
        }

        public ICommand CommandTrimSound => new RelayCommand(TrimSound);
        public ICommand CommandTestSound => new RelayCommand(TestSound);

        private void TrimSound()
        {
            Sound.Trim();
            Close();      
        }

        private void TestSound()
        {
            if (!isPlaying)
            {
                Sound.Trim();
                soundPlayer.SoundLocation = Sound.PathTrim;
                soundPlayer.Load();
                soundPlayer.Play();
            }
            else
            {
                soundPlayer.Stop();
            }
            isPlaying = !isPlaying;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isPlaying)
            {
                soundPlayer.Stop();
            }
        }
    }
}
