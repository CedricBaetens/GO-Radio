using GO_Radio.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
using PropertyChanged;

namespace GO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class TrimWindow : Window
    {
        public Sound Sound { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        private SoundPlayer soundPlayer;
        private bool isPlaying = false;
        private string tempPath;

        public TrimWindow(Sound sound)
        {
            InitializeComponent();
            Sound = sound;

            soundPlayer = new SoundPlayer();

            DataContext = this;

            StartTime = new TimeSpan(sound.TrimStart.Ticks);
            EndTime = new TimeSpan(sound.TrimEnd.Ticks);
        }

        public ICommand CommandTrimSound => new RelayCommand(TrimSound);
        public ICommand CommandTestSound => new RelayCommand(TestSound);

        private void TrimSound()
        {
            Sound.TrimStart = StartTime;
            Sound.TrimEnd = EndTime;
        }

        private void TestSound()
        {
            if (!isPlaying)
            {
                tempPath = Sound.Directory + "\\test.wav";
                AudioHelper.TrimWavFile(Sound.Path, tempPath, StartTime, EndTime);
                soundPlayer.SoundLocation = tempPath;
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

            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

        }
    }
}
