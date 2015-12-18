using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
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

namespace HLDJ_Advanced.Views
{
    [ImplementPropertyChanged]
    public partial class TextToSpeechWindow : Window
    {
        public string SpeechText { get; set; }
        public IEnumerable<VoiceGender> Genders { get; set; }
        public VoiceGender SelectedGender { get; set; }

        public TextToSpeechWindow()
        {
            InitializeComponent();

            Genders = Enum.GetValues(typeof(VoiceGender)).Cast<VoiceGender>()
                .Where(e => e != VoiceGender.NotSet);

            DataContext = this; 
        }

        public bool Canceled { get; set; }

        private void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Canceled = true;
            Close();
        }

        private void BtnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SpeechText))
            {
                Canceled = false;
                Close();
            }
            else
            {
                MessageBox.Show("Input is empty.");
            }
        }
    }
}
